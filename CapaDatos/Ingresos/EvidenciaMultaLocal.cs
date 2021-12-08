using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class EvidenciaMultaLocal : SqlConnecion, IDisposable
    {
        private long _Id_Multa;
        private MultaLocal _MultaLocal;

        #region Public Properties
        public long Id_Evidencia { get; set; }
        public long Id_Multa
        {
            get { return _Id_Multa; }
            set
            {
                _Id_Multa = value;
                _MultaLocal = null;
            }
        }
        public string Descripcion { get; set; }
        public string Url { get; set; }
        public int Tipo { get; set; }
        public DateTime FechaHora_Reg { get; set; }
        public MultaLocal Multa
        {
            get
            {
                if (_MultaLocal != null && _MultaLocal.Id_Multa != 0)
                    return _MultaLocal;
                else if (_Id_Multa != 0)
                    return _MultaLocal = new MultaLocal(_Id_Multa);
                else
                    return null;
            }
        }
        #endregion

        public EvidenciaMultaLocal()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public EvidenciaMultaLocal(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Evidencia = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Evidencia = Convert.ToInt64(dt.Rows[0]["Id_Evidencia"]);
                    Id_Multa = Convert.ToInt64(dt.Rows[0]["Id_Multa"]);
                    Descripcion = dt.Rows[0]["Descripcion"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Descripcion"]) : "";
                    Url = dt.Rows[0]["Url"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Url"]) : "";
                    Tipo = dt.Rows[0]["Tipo"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Tipo"]) : -1;
                    FechaHora_Reg = Convert.ToDateTime(dt.Rows[0]["FechaHora_Reg"]);
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
            return String.Format("SELECT Id_Evidencia, Id_Multa, Descripcion, Url, Tipo, FechaHora_Reg FROM EVIDENCIA_MULTA_LOCAL");
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

        public static MultaLocal GetMulta(long id)
        {
            return new MultaLocal(id);
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
            return String.Format("INSERT INTO EVIDENCIA_MULTA_LOCAL (Id_Multa, Descripcion, Url, Tipo, FechaHora_Reg) " +
                " VALUES({0}, {1}, {2}, {3}, '{4}'); ", Id_Multa, string.IsNullOrEmpty(Descripcion) ? "NULL" : "'" + SqlServer.ValidarTexto(Descripcion) + "'",
                string.IsNullOrEmpty(Url) ? "NULL" : "'" + SqlServer.ValidarTexto(Url) + "'", Tipo, Convert.ToDateTime(FechaHora_Reg.ToString()).ToString(SqlServer.FormatofechaHora));
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
            return String.Format("DELETE FROM EVIDENCIA_MULTA_LOCAL WHERE Id_Evidencia = {0};", ID);
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
            return String.Format("UPDATE EVIDENCIA_MULTA_LOCAL SET Id_Multa = {1}, Descripcion = {2}, Url = {3}, Tipo = {4}, FechaHora_Reg = '{5}' " +
                "WHERE Id_Evidencia = {0}; ", Id_Evidencia, Id_Multa, string.IsNullOrEmpty(Descripcion) ? "NULL" : "'" + SqlServer.ValidarTexto(Descripcion) + "'",
                string.IsNullOrEmpty(Url) ? "NULL" : "'" + SqlServer.ValidarTexto(Url) + "'", Tipo, 
                Convert.ToDateTime(FechaHora_Reg.ToString()).ToString(SqlServer.FormatofechaHora));
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
        ~EvidenciaMultaLocal()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
