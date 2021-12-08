using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class Tipo_Ambiente : SqlConnecion, IDisposable
    {
        #region Public Properties
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        #endregion

        public Tipo_Ambiente()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Tipo_Ambiente(string id)
        {
            using (DataTable dt = GetAllData(String.Format("Codigo = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Codigo = Convert.ToString(dt.Rows[0]["Codigo"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
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
            return String.Format("SELECT Codigo, Nombre FROM TIPO_AMBIENTE");
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

        public static Tipo_Ambiente GetTipo_Ambiente(string id)
        {
            return new Tipo_Ambiente(id);
        }

        static public string Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Codigo", "TIPO_AMBIENTE", "");
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
            return String.Format("INSERT INTO TIPO_AMBIENTE (Codigo, Nombre) VALUES ('{0}', '{1}'); ",
                Codigo, SqlServer.ValidarTexto(Nombre));
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
            return String.Format("DELETE FROM TIPO_AMBIENTE WHERE Codigo = '{0}';", ID);
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
            return String.Format("UPDATE TIPO_AMBIENTE SET Codigo = '{1}', Nombre = '{2}' WHERE Codigo = '{0}';", 
                ID, Codigo, SqlServer.ValidarTexto(Nombre));
        }

        public bool IsDuplicate_Codigo(string sCod)
        {
            return IsDuplicate_Codigo("", sCod);
        }

        public bool IsDuplicate_Codigo(string id, string sCod)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Codigo) FROM TIPO_AMBIENTE WHERE Codigo = '{0}' {1}", sCod, id != "" ? "AND Codigo <> " + id.ToString() : ""))) > 0;
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
        ~Tipo_Ambiente()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
