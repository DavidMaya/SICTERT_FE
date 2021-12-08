using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Runtime.CompilerServices;

namespace AccesoDatos
{
    public class Generico :  IDisposable
    {
        static public Random rnd = new Random();

        public Generico()
        { 
        
        }

        /// <summary>
        /// Obtine el listado de la tabla parqueo
        /// 
        /// </summary>
        /// <param name="IdUnidad"> El id de la unidad de transporte</param>
        /// <returns></returns>
        public static string ObtenerValorConfiguracion(string config, bool entero = false)
        {
            try
            {
                return SqlServer.EXEC_SCALAR(string.Format("SELECT Valor FROM CONFIGURACION_GLOBAL WHERE Configuracion = '{0}'", config)).ToString();
            }
            catch
            {
                return entero ? "0" : "";
            }
        }

        public DataTable Get_Detalle_Parqueo(long IdUnidad  )
        {
            //string sql = String.Format("SELECT  Id_parqueo, Tag,Fecha_ingreso, Fecha_salida ,Valor,valor_tmp FROM parqueo WHERE ESTADO=1 and Tag ={0}", IdUnidad);
            // saco lista de parqueos para cobrar, exceptuando el ultimo
            string sql = String.Format("SELECT Id_parqueo, Tag, dbo.fn_formatearFecha(Fecha_ingreso) AS Fecha_ingreso, dbo.fn_formatearFecha(Fecha_salida) AS Fecha_salida, Valor, Valor_IVA, Tiempo_Gratis, valor_tmp, Valor + Valor_IVA AS Total, Id_Chofer FROM PARQUEO WHERE ESTADO = 1 AND Tag = {0} AND Fecha_salida IS NOT NULL", IdUnidad);
            return SqlServer.EXEC_SELECT(sql);
        }

        public string InsertChofer(string cedula, string nombre, string direccion)
        {
            string sql = String.Format("EXEC InsertarConductores '{0}','{1}','','','{2}'", cedula, nombre, direccion);
            return SqlServer.EXEC_SCALAR(sql).ToString();
        }

        public DataTable BuscarChofer(string texto)
        {         
            string sql = String.Format("SELECT id_chofer, Nombre, Cedula, Direccion  FROM CONDUCTOR WHERE Activo = 1 AND (Nombre + ' ' + Cedula + ' ' + Id_tar ) LIKE '%{0}%'", texto);
            return SqlServer.EXEC_SELECT(sql);
        }

        public DataTable ListaChat(long id_usuario)
        {
            string sql = String.Format("EXEC ListarUsuariosChat {0}",id_usuario);
            return SqlServer.EXEC_SELECT(sql);  
        }

        public DataTable Get_sp_lista_Conductores(bool soloActivos)
        {
            string sql = String.Format(string.Format("EXEC ListarConductores '{0}'", soloActivos));
            return SqlServer.EXEC_SELECT(sql); 
        }

        public DataTable Get_sp_lista_TiposPagos(long id_caja)
        {
            string sql = String.Format("EXEC ListaTiposPago {0}", id_caja);
            return SqlServer.EXEC_SELECT(sql);
        }

        public string Get_sql_update_parqueo(long id_factura, long idUnidad)
        {
            // return string.Format("UPDATE PARQUEO SET factura = {0}, estado = 2, valor = valor_tmp, valor_tmp = 0 WHERE factura IS NULL AND Tag = {1};", id_factura, idUnidad);
            // el valor ya se calculó cuando pasó por barrera, ahora estoy cobrando todos los pendientes menos el último pues aún no sale la UTT
            return string.Format("UPDATE PARQUEO SET factura = {0}, estado = 2 WHERE factura IS NULL AND Tag = {1} AND Fecha_salida IS NOT NULL; ", id_factura, idUnidad);
        }

        public string Get_sql_update_parqueo_publico(long id_factura, string sTiquete, decimal valor, decimal valor_iva, int tiempo_gracia, long id_tipo_tarifa)
        {
            return string.Format("UPDATE PARQUEO_PUBLICO SET factura = {0}, estado = 2, valor = {2}, valor_iva = {3}, fecha_max_salida = DATEADD(MINUTE, {4}, GETDATE()), Id_tipo_tarifa = {5} WHERE factura IS NULL AND Codigo_barra = '{1}' AND Fecha_salida IS NULL; ", 
                id_factura, sTiquete, valor.ToString().Replace(",", SqlServer.SigFloatSql), valor_iva.ToString().Replace(",", SqlServer.SigFloatSql), tiempo_gracia, id_tipo_tarifa);
        }

        public string Get_sql_update_SaldoPrepago (long id_unidad_tranporte, decimal valor)
        {            
            return string.Format("UPDATE CUENTA_PREPAGO_UTT SET saldo = saldo - '{0}' WHERE id_unidad_transporte = {1}; ", valor.ToString().Replace(",", SqlServer.SigFloatSql), id_unidad_tranporte);
        }

        //public string Get_Titulo1()
        //{
        //    var  titulo = SqlServer.EXEC_SCALAR("SELECT  valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION ='TITULO 1'");
        //    return titulo==null?"":titulo.ToString() ;
        //}

        //public string Get_Titulo2()
        //{
        //    var titulo = SqlServer.EXEC_SCALAR("SELECT  valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION ='TITULO 2'");
        //    return titulo==null?"":titulo.ToString() ;
        //}

        //public string Get_Titulo3()
        //{
        //    var titulo = SqlServer.EXEC_SCALAR("SELECT  valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION ='TITULO 3'");
        //    return titulo == null ? "" : titulo.ToString();
        //}

        //public string Get_Titulo4()
        //{
        //    var titulo = SqlServer.EXEC_SCALAR("SELECT  valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION ='TITULO 4'");
        //    return titulo == null ? "" : titulo.ToString();
        //}

        //public string Get_Titulo5()
        //{
        //    var titulo = SqlServer.EXEC_SCALAR("SELECT  valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION ='TITULO 5'");
        //    return titulo == null ? "" : titulo.ToString();
        //}

        public int Get_CaducidadTasaUsuario()
        {
            var titulo = SqlServer.EXEC_SCALAR("SELECT  valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION ='caducidad_tasa_usuario'");
            return titulo == null ? 0 : Convert.ToInt32(titulo);
        }

        public bool EsExtra(long id_frecuencia)
        {
            var titulo = Convert.ToInt16(SqlServer.EXEC_SCALAR(String.Format("SELECT COUNT(*) FROM FRECUENCIA_VENDIDA WHERE DATEDIFF(mi, fecha, GETDATE()) < (SELECT CAST(Valor AS INT) FROM CONFIGURACION_GLOBAL WHERE Configuracion = 'MINUTOS') " +
                "AND Anulado = 0 AND Id_Frecuencia = {0}", id_frecuencia)));
            return titulo >= 1;
        }

        public Decimal Get_MIN_PREPAGO()
        {
            var cant = SqlServer.EXEC_SCALAR("SELECT  valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION ='MIN_PREPAGO'");
            return cant == null ? 0 : Convert.ToDecimal(cant);
        }

        /// <summary>
        /// Retorna falso para Poder guaradar   la factura
        /// </summary>
        /// <param name="id_transporte"></param>
        /// <returns></returns>
        public bool UnidadTransporteHaComprado(long id_transporte)
        {
            var titulo = Convert.ToInt16(SqlServer.EXEC_SCALAR(String.Format("SELECT CAST(ISNULL(COMPROFRE, 0) AS SMALLINT) FROM UNIDAD_TRANSPORTE WHERE Id_Unidad_Transporte = {0}", id_transporte)));
            return titulo == 1;
        }

        public bool UnidadTransporteHaComprado(long id_transporte, long id_frecuencia)
        {
            var titulo = Convert.ToInt16(SqlServer.EXEC_SCALAR(String.Format("SELECT COUNT(*) FROM FRECUENCIA_VENDIDA WHERE DATEDIFF(mi, fecha, GETDATE()) < (SELECT CAST(Valor AS INT) FROM CONFIGURACION_GLOBAL WHERE Configuracion = 'MINUTOS') " +
                "AND Anulado = 0 AND Id_Frecuencia = {0} AND Id_Unidad_Transporte = {1}", id_frecuencia, id_transporte)));
            return titulo >= 1;
        }

        public bool CajaFormaPago(long id_caja)
        {
            var cantFormas = Convert.ToInt16(SqlServer.EXEC_SCALAR(String.Format("SELECT  COUNT(*) FROM CAJA_TIPOPAGO WHERE Id_Caja = {0}", id_caja)));
            return cantFormas > 0 ? true : false;
        }

        public DataTable Get_ListadoFrecuenciasVendidas(bool sin_salir)
        {
            string sql = String.Format("EXEC ObtenerListaFrecuenciasVendidas " + (sin_salir ? "1" : "0"));
            return SqlServer.EXEC_SELECT(sql);
        }

        public bool ActivarConductor()
        {
            var valor = SqlServer.EXEC_SCALAR(String.Format(" SELECT valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION = 'activachofer'"));
            return valor == null ? false : (valor.ToString().ToUpper() == "TRUE");
        }

        public bool ActivarReimpresion()
        {
            var valor = SqlServer.EXEC_SCALAR(String.Format(" SELECT valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION = 'reimprimir'"));
            return valor == null ? false : (valor.ToString().ToUpper() == "TRUE");
        }

        public bool TecladoVirtualActivo()
        {
            var valor = SqlServer.EXEC_SCALAR(String.Format(" SELECT valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION = 'tecladovirtual'"));
            return valor == null ? false : (valor.ToString().ToUpper() == "TRUE");
        }

        public bool VentaTasaUsuarioDestino()
        {
            var valor = SqlServer.EXEC_SCALAR(String.Format(" SELECT valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION = 'venta_tasa_usuario'"));
            return valor == null ? false : (valor.ToString() == "1");
        }

        public bool ChatActivo()
        {
            var valor = SqlServer.EXEC_SCALAR(String.Format(" SELECT valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION = 'activachat'"));
            return valor == null ? false : (valor.ToString().ToUpper() == "TRUE");
        }

        public bool UnidadTransporteenPreoperacional( long id_unidad)
        {
            string sql = String.Format("SELECT  COUNT(u.Id_Unidad_Transporte)  FROM UNIDAD_TRANSPORTE u INNER JOIN area a " +
                " on a.id_area = u.id_area WHERE a.Numero = 0 and  u.Id_Unidad_Transporte={0} ", id_unidad);
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(sql)) > 0;
        }

        public string UpdateTotalCaja(long id_cierre_caja, long id_tipo_pago, decimal valor)
        {
            return String.Format("UPDATE CIERRE_CAJA SET Total_Sistema = Total_Sistema + '{2}', " +
                "Total_Contado = CASE WHEN (SELECT Deposito FROM TIPO_PAGO WHERE Id_Tipo_Pago = {1}) = 1 THEN Total+Contado + {2} ELSE Total_Contado END" +
                "WHERE Id_Cierre_Caja = {0};", id_cierre_caja, id_tipo_pago, valor.ToString().Replace(",",SqlServer.SigFloatSql));
        }

        public string GetNumeroSerieCajaActual(long id_caja)
        {
            var serie = SqlServer.EXEC_SCALAR(String.Format("SELECT Serie FROM CAJA WHERE Id_Caja = {0}", id_caja));
            return serie == null ? "000" : serie.ToString();
        }

        public string GetValorFrecuenciaExtra()
        {
            return GetConfiguracion("frecuenciaextra");
        }

        private string GetConfiguracion(string nombre)
        {
            string sql = string.Format("SELECT valor FROM CONFIGURACION_GLOBAL WHERE CONFIGURACION = '{0}'", nombre);
            object tmpv = SqlServer.EXEC_SCALAR(sql);
            if (tmpv == null)
                return "";
            else
            return    tmpv.ToString();
        }

        #region Metodo Dispose

        /// <summary>
        /// Implementaci??n de IDisposable. No se sobreescribe.
        /// </summary>
        /// 
        private Boolean disposed;
        public void Dispose()
        {
            this.Dispose(true);
            // GC.SupressFinalize quita de la cola de finalizaci??n al objeto.
            GC.SuppressFinalize(this);

        }
        /// <summary>
        /// Limpia los recursos manejados y no manejados.
        /// </summary>
        /// <param name="disposing">
        /// Si es true, el m??todo es llamado directamente o indirectamente
        /// desde el c??digo del usuario.
        /// Si es false, el m??todo es llamado por el finalizador
        /// y s??lo los recursos no manejados son finalizados.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // Preguntamos si Dispose ya fue llamado.
            if (!this.disposed)
            {
                if (disposing)
                {
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Destructor de la instancia
        ///  </summary>
        ~Generico()
        {
            this.Dispose(false);
        }

        public long BuscarNumNotificacionMulta()
        {
            try
            {
                return Convert.ToInt64(SqlServer.EXEC_SCALAR("SELECT MAX(ISNULL(A.N, 0)) +1 FROM(SELECT DISTINCT Num_Notif AS N FROM MULTA UNION SELECT DISTINCT Num_Notif AS N FROM MULTA_LOCAL) AS A"));
            }
            catch
            {
                return 1;
            }
        }
        #endregion

        public string GenerarCodigo(int cantidad, int tablatasacaja, bool normal, ref int consecutivocaja)
        {
            string config = GetConfiguracion("algoritmo-tu");

            if (config == "C13")
            {
                // ### caja/tabla
                // #### consecutivo
                // #### hora de emisión desde el inicio de año
                // # verificador
                // # checksum

                string codigos = string.Empty;
                if (tablatasacaja < 900)
                    tablatasacaja += 100;
                var CCC = tablatasacaja.ToString();
                DateTime fechaIncio = new DateTime(DateTime.Now.Year, 1, 1);
                TimeSpan ts = DateTime.Now.Subtract(fechaIncio);
                string MMMM = ((int)ts.TotalHours).ToString().PadLeft(4, '0');

                for (int i = 0; i < cantidad; i++)
                {
                    string IIII = consecutivocaja.ToString().PadLeft(4, '0');
                    if (consecutivocaja > 9999)
                        consecutivocaja = 0;
                    consecutivocaja++;

                    string CodigoBarra = CCC + IIII + MMMM;
                    int suma = 0;

                    for (int x = 1; x <= 9; x += 2)
                    {
                        int dig = Convert.ToInt32(CodigoBarra.Substring(x, 1));
                        suma += dig * 3;
                        if (suma > 9)
                            suma = Convert.ToInt32(suma.ToString().Substring(0, 1)) + Convert.ToInt32(suma.ToString().Substring(1, 1));
                        if (suma > 9)
                            suma -= 9;
                    }
                    //string codigo = CalculateChecksumDigit(suma.ToString());

                    CodigoBarra += suma.ToString();
                    codigos += CodigoBarra + CalculateTUChecksumDigit(CodigoBarra) + ",";
                }

                return codigos;
            }
            else if (config == "C15")
            {
                // ### caja/tabla
                // #### consecutivo
                // #### hora de emisión desde el inicio de año
                // ## año emisión
                // # 1 normal 2 discapacitado
                // # checksum

                string codigos = string.Empty;
                if (tablatasacaja < 900)
                    tablatasacaja += 100;
                var CCC = tablatasacaja.ToString();
                DateTime fechaIncio = new DateTime(DateTime.Now.Year, 1, 1);
                TimeSpan ts = DateTime.Now.Subtract(fechaIncio);
                string MMMM = ((int)ts.TotalHours).ToString().PadLeft(4, '0');
                string YY = fechaIncio.ToString("yy");
                string ND = normal ? "1" : "2";

                for (int i = 0; i < cantidad; i++)
                {
                    string IIII = consecutivocaja.ToString().PadLeft(4, '0');
                    if (consecutivocaja > 9999)
                        consecutivocaja = 0;
                    consecutivocaja++;

                    string CodigoBarra = CCC + IIII + MMMM + YY + ND;
                    int suma = 0;

                    for (int x = 1; x <= 13; x += 2)
                    {
                        int dig = Convert.ToInt32(CodigoBarra.Substring(x, 1));
                        suma += dig * 3;
                        if (suma > 9)
                            suma = Convert.ToInt32(suma.ToString().Substring(0, 1)) + Convert.ToInt32(suma.ToString().Substring(1, 1));
                        if (suma > 9)
                            suma -= 9;
                    }

                    codigos += CodigoBarra + suma.ToString() + ",";
                }
                return codigos;
            }
            else
                return "";
        }

        public static string CalculateTUChecksumDigit(string codigo)
        {
            string sTemp = codigo;
            int iSum = 0;
            int iDigit = 0;

            // Calculate the checksum digit here.
            for (int i = sTemp.Length; i >= 1; i--)
            {
                iDigit = Convert.ToInt32(sTemp.Substring(i - 1, 1));
                // This appears to be backwards but the 
                // EAN-13 checksum must be calculated
                // this way to be compatible with UPC-A.
                if (i % 2 == 0)
                { // odd  
                    iSum += iDigit * 3;
                }
                else
                { // even
                    iSum += iDigit * 1;
                }
            }
            int iCheckSum = (10 - (iSum % 10)) % 10;
            return iCheckSum.ToString();
        }

        public static int DigitoVerificadorModulo11(string cadena)
        {
            int[] m = { 2, 3, 4, 5, 6, 7 };
            int s = 0, p = 0;
            for (int i = cadena.Length - 1; i >= 0; i--)
            {
                int n = Convert.ToInt16(cadena[i].ToString());
                n *= m[p];
                p++;
                if (p == 6)
                    p = 0;
                s += n;
            }
            int r = 11 - s % 11;
            if (r == 11)
                r = 0;
            else if (r == 10)
                r = 1;
            return r;
        }

        public static string GenerarClaveAccesoFactElectronica(string tipocomp, string ruc, string tipoamb, string codestab, string seriecaja, string numcomp, string tipoemi)
        {
            string clave = string.Format("{0:ddMMyyyy}{1}{2}{3}{4}{5}{6}12345678{7}", DateTime.Now.ToString("ddMMyyyy"), tipocomp, ruc, tipoamb,
                codestab, seriecaja, numcomp.PadLeft(9, '0'), tipoemi);
            clave += DigitoVerificadorModulo11(clave).ToString();
            return clave;
        }

        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static int CompareIntKey(KeyValuePair<int, int> a, KeyValuePair<int, int> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        public static int CompareIntVal(KeyValuePair<int, int> a, KeyValuePair<int, int> b)
        {
            return a.Value.CompareTo(b.Value);
        }
    }
}
