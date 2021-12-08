using AccesoDatos;
using System;
using System.Data;
using System.Linq;

namespace CapaDatos.Boleteria
{
    public class EstadoAsiento : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long IdEstadoAsiento { get; set; }
        public string NombreEstado { get; set; }
        public string CodigoEstado { get; set; }
        #endregion

        #region Constructor
        public EstadoAsiento() { }

        public EstadoAsiento(long id)
        {
            string sql = String.Format(@"SELECT Id_Estado_Asiento, Nombre_Estado, Codigo_Estado
                FROM ESTADO_ASIENTO WHERE Id_Estado_Asiento = {0}", id);
            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                if (table.Rows.Count > 0)
                {
                    IdEstadoAsiento = Convert.ToInt64(table.Rows[0]["Id_Estado_Asiento"]);
                    NombreEstado = Convert.ToString(table.Rows[0]["Nombre_Estado"]);
                    CodigoEstado = Convert.ToString(table.Rows[0]["Codigo_Estado"]);
                }
            }
        }
        #endregion

        #region Select Data Methods
        private static string GetSqlSelect()
        {
            return String.Format("SELECT Id_Estado_Asiento, Nombre_Estado, Codigo_Estado FROM ESTADO_ASIENTO");
        }

        public static DataTable GetAllData()
        {
            return GetAllData("");
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

        public static EstadoAsiento getEstadoAsiento(long id)
        {
            return new EstadoAsiento(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Estado_Asiento", "ESTADO_ASIENTO", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        } 
        #endregion

        #region Insert Data Method
        public string Insert()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }

        private string GetSQLInsert()
        {
            return String.Format(@"INSERT INTO ESTADO_ASIENTO (Nombre_Estado, Codigo_Estado)
                VALUES('{0}','{1}')",
                SqlServer.ValidarTexto(NombreEstado),
                SqlServer.ValidarTexto(CodigoEstado));
        }
        #endregion

        #region Update Data Method
        public string Update()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate());
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;

        }

        private string GetSQLUpdate()
        {
            return String.Format(@"UPDATE ESTADO_ASIENTO SET Nombre_Estado = '{0}', Codigo_Estado = '{1}', WHERE Id_Estado_Asiento = {2}",
                SqlServer.ValidarTexto(NombreEstado), SqlServer.ValidarTexto(CodigoEstado), IdEstadoAsiento);
        }
        #endregion

        #region Delete Data Method
        public string Delete(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        private string GetSQLDelete(long id)
        {
            return String.Format("DELETE FROM ESTADO_ASIENTO WHERE Id_Estado_Asiento = {0}", id);
        }
        #endregion

        #region Control Datos Duplicados
        public bool IsDuplicate_Codigo_Estado(string Nom)
        {
            return IsDuplicate_Codigo_Estado(0, Nom);
        }

        private bool IsDuplicate_Codigo_Estado(long id, string nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(
                String.Format(@"SELECT Id_Estado_Asiento, Nombre_Estado, Codigo_Estado, Activo
                FROM ESTADO_ASIENTO WHERE Activo = 1 AND Codigo_Estado = '{0}' {1}",
                nom,
                id != 0 ? string.Format("AND Id_Estado_Asiento <> {0}", id) : ""))) > 0;
        }

        public bool IsDuplicate_Nombre_Estado(string Nom)
        {
            return IsDuplicate_Nombre_Estado(0, Nom);
        }

        private bool IsDuplicate_Nombre_Estado(long id, string nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(
                String.Format(@"SELECT Id_Estado_Asiento, Nombre_Estado, Codigo_Estado, Activo
                FROM ESTADO_ASIENTO WHERE Activo = 1 AND Nombre_Estado = '{0}' {1}",
                nom,
                id != 0 ? string.Format("AND Id_Estado_Asiento <> {0}", id) : ""))) > 0;
        }
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
        ~EstadoAsiento()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
