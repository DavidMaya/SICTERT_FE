using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.AreasEquipos
{
    public partial class EnlaceExterno : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long Id_Enlace { get; set; }
        public string Nombre { get; set; }
        public string Servidor { get; set; }
        public string Recurso { get; set; }
        public string Protocolo { get; set; }
        public bool Activo { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public int Timeout { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public EnlaceExterno()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public EnlaceExterno(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Enlace = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Enlace = Convert.ToInt64(dt.Rows[0]["Id_Enlace"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Servidor = Convert.ToString(dt.Rows[0]["Servidor"]);
                    Recurso = dt.Rows[0]["Recurso"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Recurso"]) : null;
                    Protocolo = Convert.ToString(dt.Rows[0]["Protocolo"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    Timeout = Convert.ToInt32(dt.Rows[0]["Timeout"]);
                    Usuario = dt.Rows[0]["Usuario"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Usuario"]) : null;
                    Contrasena = dt.Rows[0]["Contrasena"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Contrasena"]) : null;
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
                }
            }
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="nombre"> nombre de la fila </param>
        public EnlaceExterno(string nombre)
        {
            using (DataTable dt = GetAllData(String.Format("Nombre = '{0}'", nombre)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Enlace = Convert.ToInt64(dt.Rows[0]["Id_Enlace"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Servidor = Convert.ToString(dt.Rows[0]["Servidor"]);
                    Recurso = dt.Rows[0]["Recurso"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Recurso"]) : null;
                    Protocolo = Convert.ToString(dt.Rows[0]["Protocolo"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    Timeout = Convert.ToInt32(dt.Rows[0]["Timeout"]);
                    Usuario = dt.Rows[0]["Usuario"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Usuario"]) : null;
                    Contrasena = dt.Rows[0]["Contrasena"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Contrasena"]) : null;
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
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
            return String.Format("SELECT Id_Enlace, Nombre, Servidor, Recurso, Protocolo, Activo, Timeout, Usuario, Contrasena, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion FROM ENLACE_EXTERNO");
        }

        public static DataTable GetAllData(string Where)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + String.Format((Where.Length > 0) ? (" WHERE " + Where) : ""));
        }

        public static DataTable GetAllData(string Where, string Order)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + String.Format((Where.Length > 0) ? (" WHERE " + Where) : "")
                + String.Format((Order.Length > 0) ? (" ORDER BY " + Order) : ""));
        }

        public static DataTable GetAllData(string Where, string Join, string Order)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + " " + Join + " " + String.Format((Where.Length > 0) ? (" WHERE " + Where) : "")
                + String.Format((Order.Length > 0) ? (" ORDER BY " + Order) : ""));
        }

        public static DataTable GetAllData()
        {
            return GetAllData("");
        }

        public static EnlaceExterno GetEnlace(long id)
        {
            return new EnlaceExterno(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Enlace", "ENLACE_EXTERNO", "");
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
            return String.Format("INSERT INTO ENLACE_EXTERNO (Nombre, Servidor, Recurso, Protocolo, Activo, Timeout, Usuario, Contrasena, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion) VALUES('{0}', '{1}', {2}, '{3}', {4}, {5}, {6}, {7}, GETDATE(), {8}, {9}, {10}); ", 
                SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Servidor), 
                Recurso == null ? "NULL" : "'" + SqlServer.ValidarTexto(Recurso) + "'", Protocolo, Activo ? 1 : 0, Timeout, 
                Usuario == null ? "NULL" : "'" + SqlServer.ValidarTexto(Usuario) + "'", Contrasena == null ? "NULL" : "'" + SqlServer.ValidarTexto(Contrasena) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
                (Fecha_Modificacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Modificacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public string Delete()
        {
            return Delete(Id_Enlace);
        }

        static public string Delete(long ID)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(ID));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete()
        {
            return GetSQLDelete(Id_Enlace);
        }

        static public string GetSQLDelete(long ID)
        {
            return String.Format("DELETE FROM ENLACE_EXTERNO WHERE Id_Enlace = {0};", ID);
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
            return String.Format("UPDATE ENLACE_EXTERNO SET Nombre = '{1}', Servidor = '{2}', Recurso = {3}, Protocolo = '{4}', Activo = {5}, Timeout = {6}, Usuario = {7}, " +
                "Contrasena = {8}, Fecha_Creacion = {9}, Usuario_Creacion = {10}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {11} WHERE Id_Enlace = {0};", 
                Id_Enlace, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Servidor), Recurso == null ? "NULL" : "'" + SqlServer.ValidarTexto(Recurso) + "'", 
                Protocolo, Activo ? 1 : 0, Timeout, Usuario == null ? "NULL" : "'" + SqlServer.ValidarTexto(Usuario) + "'", 
                Contrasena == null ? "NULL" : "'" + SqlServer.ValidarTexto(Contrasena) + "'",
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
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
        ~EnlaceExterno()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
