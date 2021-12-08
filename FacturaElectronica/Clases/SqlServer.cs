using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace AccesoDatos
{
    public abstract class SqlConnecion
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
        private static string _cadena_coneccion;
        public static string CadenaConexion
        {
            get
            {
                return _cadena_coneccion;
            }
            set { _cadena_coneccion = value; }
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

        private static SqlConnection GetNewConexion()
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
        public static DataTable EXEC_SELECT(string sql)
        {
           using( System.Data.DataSet dt = new DataSet())
           {
               SqlConnection current_conexion = GetNewConexion();
               try
               {                  
                   SqlDataAdapter da = new SqlDataAdapter(sql, current_conexion);
                   da.Fill(dt);                
               }
               catch(SqlException er)
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

        private static string sigFloatSql;
        
        public static string SigFloatSql
        {
        	get { return sigFloatSql; }
        	set { sigFloatSql = value; }
        }

        public static int EXEC_PROCEDURE(string nombreSProcedure, int valor)
        {
            int var_Retorno = 0;
            using (SqlConnection connection = GetNewConexion())
            {
                using (SqlCommand sqlCMD = new SqlCommand(nombreSProcedure, connection))
                {
                    SqlParameter sqlPAR = null;
                    sqlCMD.CommandType = CommandType.StoredProcedure;

                    sqlPAR = sqlCMD.Parameters.Add("@id_caja", SqlDbType.Int);
                    sqlPAR.Value = valor;
                    sqlPAR = sqlCMD.Parameters.Add("return_value", SqlDbType.Int);
                    sqlPAR.Direction = ParameterDirection.ReturnValue;

                    if (sqlCMD.Connection.State == ConnectionState.Closed)
                        sqlCMD.Connection.Open();
                    sqlCMD.ExecuteReader();
                    var_Retorno = (int)sqlCMD.Parameters["return_value"].Value;                    
                }
            }   
            return var_Retorno;
        }

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

        static public SqlDataReader SEXEC_COMMAND_READER(string SQL)
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
            catch (SqlException er)
            {
               // throw new ArgumentException(er.Message);
                resultado = er.Message;
            }
            catch (Exception er_)
            {
               // throw new ArgumentException(er_.Message);
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
        public static string EXEC_TRANSACTION( string sql )
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
                 getError = "\n\nOcurrio un problema, no se realizó la operación solicitada:\n" + er.Message + " " + er.ErrorCode;
                
                 try
                 {
                     myTrans.Rollback();
                 }
                 catch (SqlException)
                 {
                     if (myTrans.Connection != null)
                     {
                         getError = "\n\nOcurrio un problema, los datos se guradron con errores:\n" + er.Message;
                     }
                 }
             }
             finally
             {
                 current_conexion_trans.Close();
             }
             return getError;
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
            return string.Format(" select isnull( MAX({0}) ,0)  + 1 from {1}   {2} ;", campo, tabla, sqlwhere);        
        }

        public static string  MensajeDeGuardar
        {
            get{return "La información se guardó correctamente"; }
        }

        public static string MensajeDeEliminar
        {
            get { return "La información se eliminó correctamente"; }
        }
        
        public static string MensajeDeActualizar
        {
            get { return "La información se actualizó correctamente"; }
        }
        
        public static string MensajeDeTransaccion
        {
            get { return "La operación se realizó correctamente"; }
        }

        public static string ValidarTexto(string texto)
        {
            string TextoValidado = "";
            if (texto != null)
            {
                for (int i = 0; i < texto.Length; i++)
                {
                    if (texto[i].ToString() == "'")
                        TextoValidado += "\\\'";
                    else
                    {
                        if (Char.ToUpper(texto[i]) == 92)
                            TextoValidado += "\\\\";
                        else
                            TextoValidado += texto[i].ToString();
                    }
                }
            }
            return (TextoValidado);
        }
        
        static public string Formatofecha
        {
            get { return "MM-dd-yyyy"; }
        }

        static public string FormatofechaHora
        {
            get { return "MM-dd-yyyy HH:mm:ss"; }
        }

        static public string FormatoHora
        {
            get { return "hh:mm:ss"; }
        }

        static public string FormatofechaInicio
        {
            get { return "MM-dd-yyyy 00:00:00"; }
        }

        static public string FormatofechaFin
        {
            get { return "MM-dd-yyyy 23:59:59"; }
        }
    }
}
