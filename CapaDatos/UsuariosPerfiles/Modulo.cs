using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.UsuariosPerfiles
{
    public partial class Modulo : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long Id_Modulo { get; set; }
        public string Nombre { get; set; }
        public string Direccion_Acceso { get; set; }
        public bool Visible { get; set; }
        public bool Principal { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public long Id_Principal { get; set; }
        #endregion

        public Modulo()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Modulo(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Modulo = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Modulo = Convert.ToInt64(dt.Rows[0]["Id_Modulo"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Direccion_Acceso = dt.Rows[0]["Direccion_Acceso"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Direccion_Acceso"]) : "";
                    Visible = Convert.ToBoolean(dt.Rows[0]["Visible"]);
                    Principal = Convert.ToBoolean(dt.Rows[0]["Principal"]);
                    Codigo = Convert.ToString(dt.Rows[0]["Codigo"]);
                    Descripcion = dt.Rows[0]["Descripcion"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Descripcion"]) : "";
                    Id_Principal = dt.Rows[0]["Id_Principal"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Principal"]) : 0;
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
            return String.Format("SELECT Id_Modulo, Nombre, Direccion_Acceso, Visible, Principal, Codigo, Descripcion, Id_Principal FROM MODULO");
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

        public static Modulo GetModulo(long id)
        {
            return new Modulo(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Modulo", "Modulo", "");
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
            return String.Format("INSERT INTO MODULO (Nombre, Direccion_Acceso, Visible, Principal, Codigo, Descripcion, Id_Principal) " +
                "VALUES('{0}', '{1}', {2}, {3}, '{4}', {5}, {6}); ", SqlServer.ValidarTexto(Nombre), 
                Direccion_Acceso.Length > 0 ? "'" + SqlServer.ValidarTexto(Direccion_Acceso) + "'" : "NULL", Visible ? "1" : "0",
                Principal ? "1" : "0", SqlServer.ValidarTexto(Codigo), Descripcion.Length > 0 ? "'" + SqlServer.ValidarTexto(Descripcion) + "'" : "NULL", 
                Id_Principal != 0 ? Id_Principal.ToString() : "NULL");
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
            return String.Format("DELETE FROM MODULO WHERE Id_Modulo = {0};", ID);
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
            return String.Format("UPDATE MODULO SET Nombre = '{0}', Direccion_Acceso = '{1}', Visible = {2}, Principal = {3}, Codigo = '{4}', " +
                "Descripcion = {5}, Id_Principal = {6} WHERE Id_Modulo = {0}", Id_Modulo, SqlServer.ValidarTexto(Nombre), 
                Direccion_Acceso.Length > 0 ? "'" + SqlServer.ValidarTexto(Direccion_Acceso) + "'" : "NULL", Visible ? "1" : "0",
                Principal ? "1" : "0", SqlServer.ValidarTexto(Codigo), Descripcion.Length > 0 ? "'" + SqlServer.ValidarTexto(Descripcion) + "'" : "NULL",
                Id_Principal != 0 ? Id_Principal.ToString() : "NULL");
        }

        public static long GetId_Modulo_Codigo(string sCod)
        {
            return Convert.ToInt64(SqlServer.EXEC_SCALAR("SELECT Id_Modulo FROM MODULO WHERE Codigo = '" + SqlServer.ValidarTexto(sCod) + "'"));
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
        ~Modulo()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
