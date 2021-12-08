using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace AccesoDatos
{
    public class Configuracion_global : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long Id_Configuracion_Global { get; set; }

        public long Id_Modulo { get; set; }

        public string Configuracion { get; set; }

        public string Valor { get; set; }

        #endregion
        public Configuracion_global()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Configuracion_global(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Configuracion_Global = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Configuracion_Global = Convert.ToInt64(dt.Rows[0]["Id_Configuracion_Global"]);
                    Id_Modulo = Convert.ToInt64(dt.Rows[0]["Id_Modulo"]);
                    Configuracion = Convert.ToString(dt.Rows[0]["Configuracion"]);
                    Valor = Convert.ToString(dt.Rows[0]["Valor"]);
                 
                }
            }
        }

        private Nullable<DateTime> valorDateTime(object ValorFecha)
        {
            if (string.IsNullOrEmpty(ValorFecha.ToString()))
                return null;
            else
                return Convert.ToDateTime(ValorFecha);
        }

        private static string GetSqlSelect()
        {
            return String.Format("SELECT Id_Configuracion_Global, Id_Modulo, Configuracion, Valor FROM CONFIGURACION_GLOBAL");
        }

        public static DataTable GetAllData(string Where)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + String.Format((Where.Length > 0) ? (" WHERE " + Where) : ""));
        }

        public static DataTable GetAllData(string Where, string Order)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + String.Format((Where.Length > 0) ? (" WHERE " + Where) : "") +
                String.Format((Order.Length > 0) ? (" ORDER BY " + Order) : ""));
        }

        public static DataTable GetAllData(string Where, string Join, string Order)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + " " + Join + " " + String.Format((Where.Length > 0) ? (" WHERE " + Where) : "") +
                String.Format((Order.Length > 0) ? (" ORDER BY " + Order) : ""));
        }

        public static DataTable GetAllData()
        {
            return GetAllData("");
        }

        public static Configuracion_global GetConfiguracion_global(long id)
        {
            return new Configuracion_global(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Configuracion_Global", "CONFIGURACION_GLOBAL", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        public string Insert()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }

        public string GetSQLInsert()
        {
            return String.Format("INSERT INTO CONFIGURACION_GLOBAL (Id_Configuracion_Global, Id_Modulo, Configuracion, Valor) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}'); ", Id_Configuracion_Global, Id_Modulo, SqlServer.ValidarTexto(Configuracion), 
                SqlServer.ValidarTexto(Valor));
        }

        public string Delete(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete(long ID)
        {
            return String.Format("DELETE FROM CONFIGURACION_GLOBAL WHERE Id_Configuracion_Global = {0};", ID);
        }

        public string Update()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate());
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;
        }

        public string GetSQLUpdate()
        {
            return String.Format("UPDATE CONFIGURACION_GLOBAL SET Id_Modulo= '{1}', Configuracion= '{2}', Valor= '{3}' " +
                "WHERE Id_Configuracion_Global = {0};", Id_Configuracion_Global, Id_Modulo, SqlServer.ValidarTexto(Configuracion), 
                SqlServer.ValidarTexto(Valor));
        }

        #region Codigo nuevo

        #endregion

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
        ~Configuracion_global()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
