using AccesoDatos;
using CapaDatos.UsuariosPerfiles;
using System;
using System.Data;

namespace CapaDatos.Transportistas
{
    public partial class AgenteTransito : SqlConnecion, IDisposable
    {
        private long _Id_Usuario;
        private Usuarios _Usuario;
        private string _Nombre;

        #region Public Properties
        public long Id_Agente { get; set; }
        public string Cedula { get; set; }
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value.ToUpper(); }
        }
        public long Id_Usuario 
        {
            get { return _Id_Usuario; }
            set
            {
                _Id_Usuario = value;
                _Usuario = null;
            }
        }
        public Usuarios Usuario
        {
            get
            {
                if (_Usuario != null && _Usuario.Id_Usuario != 0)
                    return _Usuario;
                else if (_Id_Usuario != 0)
                    return _Usuario = new Usuarios(_Id_Usuario);
                else
                    return null;
            }
        }
        public bool Activo { get; set; }
        public DateTime? FechaComprobacionDinardap { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public AgenteTransito()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public AgenteTransito(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Agente = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Agente = Convert.ToInt64(dt.Rows[0]["Id_Agente"]);
                    Cedula = (dt.Rows[0]["Cedula"]) == null ? "": Convert.ToString(dt.Rows[0]["Cedula"]);
                    _Nombre = (dt.Rows[0]["Nombre"]) == null ? "" : Convert.ToString(dt.Rows[0]["Nombre"]);
                    _Id_Usuario = DBNull.Value.Equals(dt.Rows[0]["Id_Usuario"]) ? 0 : Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    FechaComprobacionDinardap = valorDateTime(dt.Rows[0]["FechaComprobacionDinardap"]);
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
                }
            }
        }

        public AgenteTransito(string ci_ruc)
        {
            using (DataTable dt = GetAllData(String.Format("Cedula = '{0}'", ci_ruc)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Agente = Convert.ToInt64(dt.Rows[0]["Id_Agente"]);
                    Cedula = (dt.Rows[0]["Cedula"]) == null ? "" : Convert.ToString(dt.Rows[0]["Cedula"]);
                    _Nombre = (dt.Rows[0]["Nombre"]) == null ? "" : Convert.ToString(dt.Rows[0]["Nombre"]);
                    _Id_Usuario = DBNull.Value.Equals(dt.Rows[0]["Id_Usuario"]) ? 0 : Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    FechaComprobacionDinardap = valorDateTime(dt.Rows[0]["FechaComprobacionDinardap"]);
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
            return String.Format("SELECT Id_Agente, Cedula, Nombre, Id_Usuario, Activo, FechaComprobacionDinardap, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion FROM AGENTE_TRANSITO");
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

        public static AgenteTransito GetAgente(long id)
        {
            return new AgenteTransito(id);
        }

        public static long GetIdAgenteIdUsuario(long id)
        {
            try 
            {
                return Convert.ToInt64(SqlServer.EXEC_SCALAR(string.Format("SELECT Id_Agente FROM AGENTE_TRANSITO WHERE Id_Usuario =  {0}", id)));
            }
            catch
            {
                return 0;
            }
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Agente", "AGENTE_TRANSITO", "");
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
            return String.Format("INSERT INTO AGENTE_TRANSITO (Cedula, Nombre, Id_Usuario, Activo, FechaComprobacionDinardap, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion) VALUES( '{0}', '{1}', {2}, '{3}', {4}, GETDATE(), {5}, {6}, {7}); ", 
                SqlServer.ValidarTexto(Cedula), SqlServer.ValidarTexto(Nombre), Id_Usuario == 0 ? "NULL": Id_Usuario.ToString(), Activo,
                (FechaComprobacionDinardap == null) ? "NULL" : "'" + Convert.ToDateTime(FechaComprobacionDinardap.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
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
            return String.Format("DELETE FROM AGENTE_TRANSITO WHERE Id_Agente = {0};", ID);
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
            return String.Format("UPDATE AGENTE_TRANSITO SET Cedula = '{1}', Nombre = '{2}', Id_Usuario = {3}, Activo = '{4}', FechaComprobacionDinardap = {5}, " +
                "Fecha_Creacion = {6}, Usuario_Creacion = {7}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {8} WHERE Id_Agente = {0};", 
                Id_Agente, SqlServer.ValidarTexto(Cedula), SqlServer.ValidarTexto(Nombre), Id_Usuario == 0 ? "NULL" : Id_Usuario.ToString(), Activo,
                (FechaComprobacionDinardap == null) ? "NULL" : "'" + Convert.ToDateTime(FechaComprobacionDinardap.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Cedula(string sCed)
        {
            return IsDuplicate_Cedula(0, sCed);
        }

        public bool IsDuplicate_Cedula(long id, string sCed)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Agente) FROM AGENTE_TRANSITO WHERE Cedula = '{0}' {1}", 
                sCed, id != 0 ? "AND Id_Agente <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Usuario(long idUsr)
        {
            return IsDuplicate_Usuario(0, idUsr);
        }

        public bool IsDuplicate_Usuario(long id, long idUsr)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Agente) FROM AGENTE_TRANSITO WHERE Id_Usuario = {0} {1}", 
                idUsr, id != 0 ? "AND Id_Agente <> " + id.ToString() : ""))) > 0;
        }

        public long ObtenerId(string ced)
        {
            try
            {
                return Convert.ToInt64(SqlServer.EXEC_SCALAR(string.Format("SELECT Id_Agente FROM AGENTE_TRANSITO WHERE Cedula = '{0}'", ced)));
            }
            catch
            {
                return 0;
            }
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
        ~AgenteTransito()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
