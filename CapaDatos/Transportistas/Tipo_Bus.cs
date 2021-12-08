using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Transportistas
{
    public partial class Tipo_Bus : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long Id_Tipo_Bus { get; set; }
        public int CantAsientos { get; set; }
        public String Nombre { get; set; }
        public bool Activo { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Tipo_Bus()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Tipo_Bus(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Tipo_Bus = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Tipo_Bus = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Bus"]);
                    this.Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    CantAsientos = Convert.ToInt32(dt.Rows[0]["CantAsientos"]);
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
            return String.Format("SELECT Id_Tipo_Bus, Nombre, CantAsientos, Activo, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, " +
                "Usuario_Modificacion FROM TIPO_BUS");
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

        public static Tipo_Bus GetTipo_Bus(long id)
        {
            return new Tipo_Bus(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Tipo_Bus", "TIPO_BUS", "");
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
            return String.Format("INSERT INTO TIPO_BUS (Nombre, CantAsientos, Activo, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, " +
                "Usuario_Modificacion) VALUES ('{0}', {1}, {2}, GETDATE(), {3}, {4}, {5}); ", 
                SqlServer.ValidarTexto(Nombre), CantAsientos, Activo ? 1 : 0, Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM TIPO_BUS WHERE Id_Tipo_Bus = {0};", ID);
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
            return String.Format("UPDATE TIPO_BUS SET Nombre = '{1}', CantAsientos = {2}, Activo = {3}, Fecha_Creacion = {4}, Usuario_Creacion = {5}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {6} WHERE Id_Tipo_Bus = {0};", 
                Id_Tipo_Bus, SqlServer.ValidarTexto(Nombre), CantAsientos, Activo ? 1 : 0,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Nombre(string Cod)
        {
            return IsDuplicate_Nombre(0, Cod);
        }

        public bool IsDuplicate_Nombre(long id, string Cod)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Bus) FROM TIPO_BUS WHERE Activo = 1 AND Nombre = '{0}' {1}", 
                Cod, id != 0 ? "AND Id_Tipo_Bus <> " + id.ToString() : ""))) > 0;
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
        ~Tipo_Bus()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
