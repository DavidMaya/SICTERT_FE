using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Frecuencias
{
    public partial class Tipo_Ruta : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long Id_Tipo_Ruta { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
        public int Tiempo_Parqueo { get; set; }
        public bool Activo { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Tipo_Ruta()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Tipo_Ruta(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Tipo_Ruta = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Tipo_Ruta = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Ruta"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Abreviatura = Convert.ToString(dt.Rows[0]["Abreviatura"]);
                    Tiempo_Parqueo = Convert.ToInt32(dt.Rows[0]["Tiempo_Parqueo"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
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
            return String.Format("SELECT Id_Tipo_Ruta, Nombre, Abreviatura, Tiempo_Parqueo, Activo, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion FROM TIPO_RUTA");
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

        public static Tipo_Ruta GetTipo_ruta(long id)
        {
            return new Tipo_Ruta(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Tipo_Ruta", "Tipo_Ruta", "");
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
            return String.Format("INSERT INTO TIPO_RUTA (Nombre, Abreviatura, Tiempo_Parqueo, Activo, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion) VALUES ('{0}', '{1}', '{2}', '{3}', GETDATE(), {4}, {5}, {6}); ", 
                SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Abreviatura), Tiempo_Parqueo, Activo,
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
                (Fecha_Modificacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Modificacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
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
            return String.Format("DELETE FROM TIPO_RUTA WHERE Id_Tipo_Ruta = {0};", ID);
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
            return String.Format("UPDATE TIPO_RUTA SET Nombre = '{1}', Abreviatura = '{2}', Tiempo_Parqueo = '{3}', Activo = '{4}', Fecha_Creacion = {5}, " +
                "Usuario_Creacion = {6}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {7} WHERE Id_Tipo_Ruta = {0};", 
                Id_Tipo_Ruta, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Abreviatura), Tiempo_Parqueo, Activo,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Ruta) FROM TIPO_RUTA " +
                "WHERE Activo = 1 AND Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Tipo_Ruta <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Abreviatura(string Abrev)
        {
            return IsDuplicate_Abreviatura(0, Abrev);
        }

        public bool IsDuplicate_Abreviatura(long id, string Abrev)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Ruta) FROM TIPO_RUTA " +
                "WHERE Activo = 1 AND Abreviatura = '{0}' {1}", Abrev, id != 0 ? "AND Id_Tipo_Ruta <> " + id.ToString() : ""))) > 0;
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
        ~Tipo_Ruta()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
