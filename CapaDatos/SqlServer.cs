using System;
using System.Data;
using System.Data.SqlClient;

namespace AccesoDatos
{
    public  abstract class SqlConnecion
    {
        public SqlConnecion()
        {
            if (_Connection == null) _Connection = new SqlConnection(SqlServer.CadenaConexion);
        }
        private static SqlConnection _Connection = null;
        public static SqlConnection Connection 
        {
            get
            {
                if (_Connection == null) _Connection = new SqlConnection(SqlServer.CadenaConexion);
                return _Connection;
            }
        }
    }

    public class SqlServer
    {
        public static string SignoDecimal = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString().Trim();

        public static Decimal Decimal_validado(string valor)
        {
            return Convert.ToDecimal(valor.Replace(".", SignoDecimal));
        }

        private static string _cadena_coneccion;
        public static string CadenaConexion
        {
            get
            {
                return _cadena_coneccion;
            }
            set 
            { 
                _cadena_coneccion = value; 
            }
        }

        private static int _sql_timeout_reportes;
        public static int SQLTimeoutReportes
        {
            get
            {
                return _sql_timeout_reportes;
            }
            set { _sql_timeout_reportes = value; }
        }

        public static SqlConnection GetNewConexion()
        {
            SqlConnection conexion = new SqlConnection(CadenaConexion);
            try
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
                conexion.Open();
            }
            catch (SqlException er)
            {
                throw new ArgumentException(er.Message); 
            }
            return conexion;
        }

        static public string SigFloatSql { get; set; }

        //public void EXEC_PROCEDURE(string nombreSProcedure, params object[] parametros)
        //{
        //    int var_Retorno = 0;
        //    using (SqlConnection connection = GetNewConexion())
        //    {
        //        using (SqlCommand sqlCMD = new SqlCommand(nombreSProcedure, connection))
        //        {
        //            SqlParameter sqlPAR = null;
        //            sqlCMD.CommandType = CommandType.StoredProcedure;

        //            foreach (object constParam in parametros)
        //            {
        //                constParamTypes.Add(constParam.GetType());
        //            }

        //            sqlPAR = sqlCMD.Parameters.Add("@id_caja", SqlDbType.Int);
        //            sqlPAR.Value = valor;


        //            if (sqlCMD.Connection.State == ConnectionState.Closed)
        //                sqlCMD.Connection.Open();
        //            sqlCMD.ExecuteReader();


        //        }
        //    }

        //}

        public static DataTable EXEC_SELECT(string sql)
        {
            return EXEC_SELECT(sql, 0);
        }

        public static DataTable EXEC_SELECT(string sql, int timeout)
        {
            using (System.Data.DataSet dt = new DataSet())
            {
                SqlConnection current_conexion = GetNewConexion();
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(sql, current_conexion);
                    if (timeout > 0)
                        da.SelectCommand.CommandTimeout = timeout;
                    da.Fill(dt);
                }
                catch (SqlException er)
                {
                    throw new ArgumentException(er.Message);
                }
                finally
                {
                    current_conexion.Dispose();
                    current_conexion.Close();
                }
                return dt.Tables[0];
            }
        }

        public static DataTable EXEC_SELECT(string sql, SqlConnection current_conexion)
        {
            using (System.Data.DataSet dt = new DataSet())
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(sql, current_conexion);
                    da.Fill(dt);
                }
                catch (SqlException er)
                {
                    throw new ArgumentException(er.Message);
                }
               
                return dt.Tables[0];
            }
        }

        static public SqlDataReader EXEC_COMMAND_READER(string SQL)
        {
            SqlConnection current_conexion = GetNewConexion();
            SqlDataReader dr;
            try
            {
                using (SqlCommand comando = new SqlCommand(SQL, current_conexion))
                {
                    dr = comando.ExecuteReader();
                }
            }
            catch (Exception er_)
            {
                throw new ArgumentException(er_.Message);
            }
            finally
            {
                current_conexion.Dispose();
                current_conexion.Close();
            }
            return dr;
        }

        static public string EXEC_COMMAND(string SQL)
        {
             string resultado = "OK";
             SqlConnection current_conexion = GetNewConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(SQL, current_conexion))
                {
                    if (Convert.ToInt16(comando.ExecuteNonQuery()) <= 0)
                        resultado = "No se efectuaron cambios ";
                }
            }
            catch (Exception er_)
            {
                resultado = er_.Message;
            }
            finally
            {
                current_conexion.Dispose();
                current_conexion.Close();
            }
            return resultado;
        }

        static public object EXEC_SCALAR(string sql)
        {
            object resultado = new object();
            SqlConnection current_conexion = GetNewConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(sql, current_conexion))
                {
                    resultado = comando.ExecuteScalar();
                }
            }
            catch (SqlException er)
            {
                throw new ArgumentException(er.Message);
            }
            catch (Exception er_)
            {
                throw new ArgumentException(er_.Message);
            }
            finally
            {
                current_conexion.Dispose();
                current_conexion.Close();
            }
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>La Operación se realizo correctamente.</returns>
        public static string EXEC_TRANSACTION(string sql)
        {
            string getError;
            SqlConnection current_conexion_trans = GetNewConexion();
            SqlCommand myCommand = current_conexion_trans.CreateCommand();
            SqlTransaction myTrans;
            myTrans = current_conexion_trans.BeginTransaction();

            myCommand.Connection = current_conexion_trans;
            myCommand.Transaction = myTrans;
            try
            {
                myCommand.CommandText = sql;
                myCommand.ExecuteNonQuery();
                myTrans.Commit();
                getError = "La operación se realizó correctamente.";
            }
            catch (SqlException er)
            {
                getError = "\n\nOcurrió un problema, no se realizó la operación solicitada:\n" + er.Message + " " + er.ErrorCode;
                try
                {
                    myTrans.Rollback();
                }
                catch (SqlException)
                {
                    if (myTrans.Connection != null)
                    {
                        getError = "\n\nOcurrió un problema, los datos se guardaron con errores:\n" + er.Message;
                    }
                }
            }
            finally
            {
                current_conexion_trans.Close();
            }
            return getError;
        }

        public static string verificarDependencias(string[] nom_Tablas, string[] comparacion)
        { 
            SqlConnection current_conexion = GetNewConexion();
            int i = 0;
            long n_registros = 0;
            string txt_registros = "";
            if (nom_Tablas.Length != comparacion.Length)
                txt_registros = "El número de parámetros enviado para la verificación de dependencias no es correcto.";
            else
            { 
                foreach(string tabla_d in nom_Tablas)
                {
                    string campo = comparacion[i].Substring(0, comparacion[i].IndexOf("="));
                    n_registros = Convert.ToInt64(EXEC_SCALAR(string.Format("SELECT ISNULL(COUNT({2}), 0) FROM {0} " + (comparacion[i].Trim().Length <= 0 ? "" : "WHERE") + " {1}", tabla_d, comparacion[i], campo)));
                    if (n_registros > 0)
                    {
                        txt_registros += string.Format("{0} [{1} registro(s)] ", tabla_d, n_registros);
                        break;
                    }
                    i++;
                }
            }
            return txt_registros;
        }

        public static void InsertarBitacora(string usuario, string modulo, string prioridad, string descripcion, string equipo)
        {
            SqlConnection current_conexion = GetNewConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand("InsertBitacora", current_conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("Id_Usuario", usuario);
                    comando.Parameters.AddWithValue("Id_Modulo", modulo);
                    comando.Parameters.AddWithValue("Id_Prioridad", prioridad);
                    comando.Parameters.AddWithValue("Descripcion", descripcion);
                    comando.Parameters.AddWithValue("Id_Equipo", equipo);
                    comando.ExecuteNonQuery();
                }
            }
            catch {}
            finally
            {
                current_conexion.Dispose();
                current_conexion.Close();
            }
            
        }

        /// <summary>
        /// formato " where [campo]=[1]
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="tabla"></param>
        /// <param name="sqlwhere"></param>
        /// <returns></returns>
        static public string GetFormatoSQLNEXT(string campo, string tabla, string sqlwhere)
        {
            return string.Format("SELECT ISNULL(MAX({0}), 0) + 1 FROM {1} {2};", campo, tabla, sqlwhere);        
        }

        public static string  MensajeDeGuardar
        {
            get { return "La información se guardó correctamente."; }
        }

        public static string MensajeDeEliminar
        {
            get { return "La información se eliminó correctamente."; }
        }

        public static string MensajeDeActualizar
        {
            get { return "La información se actualizó correctamente."; }
        }
        
        public static string MensajeDeTransaccion
        {
            get { return "La operación se realizó correctamente."; }
        }

        public static string ValidarTexto(string texto)
        {
            string TextoValidado = "";
            if (texto != null)
            {
                for (int i = 0; i < texto.Length; i++)
                {
                    if (texto[i].ToString() == "'")
                        TextoValidado += "''";
                    else
                    {
                        if (Char.ToUpper(texto[i]) == 92)
                            TextoValidado += "\\\\";
                        else if (Char.ToUpper(texto[i]) == 10)
                            TextoValidado += " ";
                        else
                            TextoValidado += texto[i].ToString();
                    }
                }
            }
            return (TextoValidado);
        }

        static public string Formatofecha
        {          
            get {
                if (FormatoFechaSQL == null)
                {
                    string value = GET_DBCC_USEROPTION("dateformat");
                    if (value != "")
                        FormatoFechaSQL = ConvertirFormatoFechaSQL(value);
                    else 
                        FormatoFechaSQL = "MM-dd-yyyy";
                }
                return FormatoFechaSQL;
            }
        }

        static public string FormatofechaHora
        {
            get
            {
                if (FormatoFechaSQL == null)
                {
                    string value = GET_DBCC_USEROPTION("dateformat");
                    if (value != "")
                        FormatoFechaSQL = ConvertirFormatoFechaSQL(value);
                    else
                        FormatoFechaSQL = "MM-dd-yyyy";
                }
                return FormatoFechaSQL + " HH:mm:ss";
            }
        }

        static public string FormatofechaHoraMinuto
        {
            get
            {
                if (FormatoFechaSQL == null)
                {
                    string value = GET_DBCC_USEROPTION("dateformat");
                    if (value != "")
                        FormatoFechaSQL = ConvertirFormatoFechaSQL(value);
                    else
                        FormatoFechaSQL = "MM-dd-yyyy";
                }
                return FormatoFechaSQL + " HH:mm";
            }
        }

        static public string FormatoHora
        {
            get { return "HH:mm:ss"; }
        }

        static public string FormatofechaInicio
        {
            get
            {
                if (FormatoFechaSQL == null)
                {
                    string value = GET_DBCC_USEROPTION("dateformat");
                    if (value != "")
                        FormatoFechaSQL = ConvertirFormatoFechaSQL(value);
                    else
                        FormatoFechaSQL = "MM-dd-yyyy";
                }
                return FormatoFechaSQL + " 00:00:00.000";
            }
        }
        
        static public string FormatofechaFin
        {
            get {
                if (FormatoFechaSQL == null)
                {
                    string value = GET_DBCC_USEROPTION("dateformat");
                    if (value != "")
                        FormatoFechaSQL = ConvertirFormatoFechaSQL(value);
                    else 
                        FormatoFechaSQL = "MM-dd-yyyy";
                }
                return FormatoFechaSQL + " 23:59:59.998";
            }
        }

        static private string FormatoFechaSQL = "yyyyMMdd";

        static private string ConvertirFormatoFechaSQL(string formato)
        {
            if (formato == "mdy")
                return "MM-dd-yyyy";
            else if (formato == "dmy")
                return "dd-MM-yyyy";
            else if (formato == "ymd")
                return "yyyy-MM-dd";
            else if (formato == "ydm")
                return "yyyy-dd-MM";
            else if (formato == "myd")
                return "MM-yyyy-dd";
            else if (formato == "dym")
                return "dd-yyyy-MM";
            else
                return formato;
        }

        static private string GET_DBCC_USEROPTION(string option)
        {
            DataTable uo = null;
            string value = "";
            try
            {
                uo = EXEC_SELECT("DBCC USEROPTIONS");
                if (uo != null)
                    foreach (DataRow dr in uo.Rows)
                    {
                        if (dr[0].ToString() == option)
                        {
                            value = dr[1].ToString();
                            break;
                        }
                    }
            }
            catch
            {
                value = "";
            }
            finally
            {
                uo = null;
            }
            return value;
        }

    }
}