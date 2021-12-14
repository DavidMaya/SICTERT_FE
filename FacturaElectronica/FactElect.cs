using AccesoDatos;
using HRDA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Timers;
using FacturaElectronica.Clases;
using FacturaElectronica.Documento;

namespace FacturaElectronica
{
    public partial class FactElect : ServiceBase
    {
		Timer runTim;
		public static string SignoDecimal;
		public static string SignoDecimalSQLServer;
		private static readonly byte[] baClavePredet = ASCIIEncoding.ASCII.GetBytes("sicTERT+");
		public static string CadenaSQLServer;
		public static bool ArchivoLog = true;
		public static string DirectorioTrabajo = AppDomain.CurrentDomain.BaseDirectory;
		public static string path = AppDomain.CurrentDomain.BaseDirectory + "\\log-factelect.txt";
		public static funciones fun = new funciones(DirectorioTrabajo, path, ArchivoLog);

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
				// buscar si hay facturas electrónicas por procesar

				// facturas del módulo Frecuencias
				//string sql = "SELECT * FROM FACTURA f INNER JOIN TIPO_FACTURA tf ON tf.Id WHERE ";




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
			List<DocumentoElectronico> documentos = new List<DocumentoElectronico>();
			documentos = Consultas.GetListFacturas();

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

			runTim = new Timer();
			runTim.Interval = RefrescarCada * 1000;
			runTim.Elapsed += new ElapsedEventHandler(run_Tick);
			runTim.Start();
		}

		protected override void OnStop()
        {
			fun.Log("Deteniendo servicio de respaldo y mantenimiento...");
		}
	}
}
