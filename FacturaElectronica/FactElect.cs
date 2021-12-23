using AccesoDatos;
using HRDA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Runtime.InteropServices;
using System.Timers;
using FacturaElectronica.Clases;
using FacturaElectronica.Documento;
using FacturaElectronica.Tools;
using FacturaElectronica.Procesos;

namespace FacturaElectronica
{
    public partial class FactElect : ServiceBase
    {
        System.Timers.Timer runTim;
		public static string SignoDecimal;
		public static string SignoDecimalSQLServer;
		private static readonly byte[] baClavePredet = ASCIIEncoding.ASCII.GetBytes("sicTERT+");
		public static string CadenaSQLServer;
		public static bool ArchivoLog = true;
		public static string DirectorioTrabajo = AppDomain.CurrentDomain.BaseDirectory;
		public static string path = AppDomain.CurrentDomain.BaseDirectory + "\\log-factelect.txt";
		public static funciones fun = new funciones(DirectorioTrabajo, path, ArchivoLog);
		private static Directorio directorio = new Directorio(DirectorioTrabajo, ConfigurationManager.AppSettings["pathDocs"]);

		List<DocumentoElectronico> documentos = null;

		public void OnDebug()
        {
            OnStart(null);
        }

        public static string CrearLlaveMD5(string stValor)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] bValor = md5.ComputeHash(Encoding.ASCII.GetBytes(stValor));

			StringBuilder sbValor = new StringBuilder();
			foreach (byte b in bValor)
				sbValor.Append(b.ToString("x2"));

			return sbValor.ToString();
		}

		public static bool VerificarMD5(string stValor, string stCodigo)
		{
			string sValor = CrearLlaveMD5(stValor);
			StringComparer scComp = StringComparer.OrdinalIgnoreCase;

			return scComp.Compare(sValor, stCodigo) == 0;
		}

		public static string EncriptarCadena(string stOriginal)
		{
			try
			{
				DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(baClavePredet, baClavePredet),
					CryptoStreamMode.Write);
				StreamWriter writer = new StreamWriter(cryptoStream);
				writer.Write(stOriginal);
				writer.Flush();
				cryptoStream.FlushFinalBlock();
				writer.Flush();

				return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			}
			catch
			{
				return stOriginal;
			}
		}

		public static string DesencriptarCadena(string cryptedString)
		{
			try
			{
				DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
				MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
				CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(baClavePredet, baClavePredet), CryptoStreamMode.Read);
				StreamReader reader = new StreamReader(cryptoStream);

				return reader.ReadToEnd();
			}
			catch
			{
				return cryptedString;
			}
		}

		void run_Tick(object sender, ElapsedEventArgs e)
		{
			runTim.Stop();
			try
			{
				// Tabla FACTURA
				ProcesarDocumentos("Id_Factura", "FACTURA", "FACTURA_CONCEPTO");
				
				//Tabla FACTURA_BOLETOS
				ProcesarDocumentos("Id_factura_boleto", "FACTURA_BOLETO", "DETALLE_FACT_BOLETO");

				//Tabla FACTURA_PARQUEO
				ProcesarDocumentos("Id_factura_parqueo", "FACTURA_PARQUEO", "DETALLE_FACT_PARQUEO");

				//Tabla FACTURA_PARQUEO
				ProcesarDocumentos("Id_factura_parqueo", "FACTURA_PARQUEO", "DETALLE_FACT_PARQUEO");


			}
			catch (Exception ex)
			{
				fun.Log("Error: " + ex.Message);
				runTim.Stop();
			}
			finally
			{
				//fun.Dispose();
				liberarMemoria();
				runTim.Start();
			}
		}

        private void ProcesarDocumentos(string id, string table, string tableDetalles)
        {
			// Cargar documentos
			documentos = new List<DocumentoElectronico>();
            documentos = Consultas.GetListFacturas(id, table, tableDetalles, directorio);
			fun.Log($"Se han creado {documentos.Count()} documentos de la tabla {table} para ser firmados");

			//Actions.ConsultarSRI(documentos.FirstOrDefault(), directorio, table);

            // Proceso de firmar
            Actividad(EstadoDocumento.SinFirma, table);

            // Proceso de enviar al SRI
            Actividad(EstadoDocumento.Firmado, table);

            // Proceso de consultar al SRI
            Actividad(EstadoDocumento.Recibido, table);

            // Creación de pdf
            Actividad(EstadoDocumento.Autorizado, table);
        }

		private void Actividad(EstadoDocumento Estado, string table)
        {
			Resultado resultado = new Resultado();

			foreach (DocumentoElectronico documento in documentos.Where(xx => xx.Estado == Estado))
            {
                string mensaje = "";
                if (documento.Estado == EstadoDocumento.SinFirma)
                {
                    // Traer los datos para firmar y la ubicación
                    resultado = directorio.Path(ConfigurationManager.AppSettings["pathP12"]);
                    string certificado = resultado.Mensaje + documento.certificado;
                    string clave = DesencriptarCadena(documento.clave);

                    resultado = Actions.Firmar(documento, certificado, clave, directorio, table);
                    mensaje = "firma";
                }
                else if (documento.Estado == EstadoDocumento.Firmado)
                {
                    resultado = Actions.ValidarSRI(documento, directorio, table);
                    mensaje = "envío";
                }
                else if (documento.Estado == EstadoDocumento.Recibido)
                {
                    resultado = Actions.ConsultarSRI(documento, directorio, table);
                    mensaje = "recepción";
                }
                else if (documento.Estado == EstadoDocumento.Autorizado)
                {
                    resultado = GenerarPDF.Factura(documento, directorio, table);
                    mensaje = "generar pdf";
                }
                else
                {
                    resultado.Estado = false;
                    resultado.Mensaje = "Ha ocurrido un error desconocido...";
                }

                if (!resultado.Estado)
                    fun.Log($"Error en {mensaje} de documento: {documento.Nombre} => {resultado.Mensaje}");
                else
                    fun.Log($"Proceso {mensaje} correcta de documento: {documento.Nombre}");
            }
		}

		private void CrearDirectorios()
        {
			directorio.Path(EstadoDocumento.SinFirma);
			directorio.Path(EstadoDocumento.Firmado);
			directorio.Path(EstadoDocumento.Recibido);
			directorio.Path(EstadoDocumento.Rechazado);
			directorio.Path(EstadoDocumento.Devuelto);
			directorio.Path(EstadoDocumento.Autorizado);
			directorio.Path(EstadoDocumento.Pdf);
			directorio.Path(ConfigurationManager.AppSettings["pathP12"]);
		}

        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

		public static void liberarMemoria()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
		}

		public FactElect()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
			ArchivoLog = ConfigurationManager.AppSettings["CrearLog"].CompareTo("1") == 0;

			CadenaSQLServer = DesencriptarCadena(ConfigurationManager.AppSettings["cnSictert"]);
			SqlServer.CadenaConexion = CadenaSQLServer;

			fun.Log("Inicializando servicio de facturación electrónica...");
			//fun.Log("Conectado a " + SqlServer.EXEC_SCALAR("SELECT Valor FROM CONFIGURACION_GLOBAL WHERE Configuracion = 'ubicacion'").ToString());
			CrearDirectorios();

			int RefrescarCada = 2000;
            try
            {
                RefrescarCada = int.Parse(ConfigurationManager.AppSettings["RefrescarCada"]);
            }
            catch (Exception)
            {
                RefrescarCada = 2000;
            }
            SignoDecimal = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString().Trim();
            SignoDecimalSQLServer = ConfigurationManager.AppSettings["SignoDecimalSQLServer"];

            runTim = new System.Timers.Timer();
            runTim.Interval = RefrescarCada * 1000;
            runTim.Elapsed += new ElapsedEventHandler(run_Tick);
            runTim.Start();
        }

		protected override void OnStop()
        {
			fun.Log("Deteniendo servicio de respaldo y mantenimiento...");
		}

        protected override void OnPause()
        {
            base.OnPause();
        }
    }
}
