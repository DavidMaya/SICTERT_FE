using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class Tipo_IdCliente : SqlConnecion, IDisposable
    {
        #region Public Properties
        // Códigos:
        // 4 - RUC, 5 - CEDULA, 6 - PASAPORTE, 7 - CONSUMIDOR FINAL
        public string Codigo_TipoIdCliente { get; set; }
        public string Nombre { get; set; }
        public bool Visible { get; set; }
        #endregion

        public Tipo_IdCliente()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Tipo_IdCliente(string id)
        {
            using (DataTable dt = GetAllData(String.Format("Codigo_TipoIdCliente = '{0}'", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Codigo_TipoIdCliente = Convert.ToString(dt.Rows[0]["Codigo_TipoIdCliente"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Visible = Convert.ToBoolean(dt.Rows[0]["Visible"]);
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
            return String.Format("SELECT Codigo_TipoIdCliente, Nombre, Visible FROM TIPO_IDCLIENTE");
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

        public static Tipo_IdCliente GetTipoId_Cliente(string id)
        {
            return new Tipo_IdCliente(id);
        }

        static public string Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Codigo_TipoIdCliente", "TIPO_IDCLIENTE", "");
            return Convert.ToString(SqlServer.EXEC_SCALAR(sql));
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
            return String.Format("INSERT INTO TIPO_IDCLIENTE (Codigo_TipoIdCliente, Nombre, Visible) VALUES ('{0}', '{1}', {2}); ",
                Codigo_TipoIdCliente, SqlServer.ValidarTexto(Nombre), Visible);
        }

        public string Delete(string id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete(string ID)
        {
            return String.Format("DELETE FROM TIPO_IDCLIENTE WHERE Codigo_TipoIdCliente = '{0}';", ID);
        }

        public string Update(string id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;
        }

        public string GetSQLUpdate(string ID)
        {
            return String.Format("UPDATE TIPO_IDCLIENTE SET Codigo_TipoIdCliente = '{1}', Nombre = '{2}', Visible = {3} WHERE Codigo_TipoIdCliente = '{0}';", 
                ID, Codigo_TipoIdCliente, SqlServer.ValidarTexto(Nombre), Visible);
        }

        public bool IsDuplicate_Codigo(string sCod)
        {
            return IsDuplicate_Codigo("", sCod);
        }

        public bool IsDuplicate_Codigo(string id, string sCod)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Codigo_TipoIdCliente) FROM TIPO_IDCLIENTE WHERE Codigo_TipoIdCliente = '{0}' {1}", sCod, id != "" ? "AND Codigo_TipoIdCliente <> " + id.ToString() : ""))) > 0;
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
        ~Tipo_IdCliente()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
