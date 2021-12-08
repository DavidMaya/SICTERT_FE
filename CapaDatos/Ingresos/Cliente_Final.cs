using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class Cliente_Final : SqlConnecion, IDisposable
    {
        private string _Nombre;
        private string _Codigo_TipoIdCliente;
        private Tipo_IdCliente _Tipo_IdCliente;

        #region Public Properties
        public long Id_Cliente_Final { get; set; }
        public string Cedula { get; set; }
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value.ToUpper(); }
        }
        public string Ciu { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
        public string Codigo_TipoIdCliente
        {
            get { return _Codigo_TipoIdCliente; }
            set
            {
                _Codigo_TipoIdCliente = value;
                _Tipo_IdCliente = null;
            }
        }
        public Tipo_IdCliente tipo_idcliente
        {
            get
            {
                if (_Tipo_IdCliente != null && _Tipo_IdCliente.Codigo_TipoIdCliente != "")
                    return _Tipo_IdCliente;
                else if (_Codigo_TipoIdCliente != "")
                    return _Tipo_IdCliente = new Tipo_IdCliente(_Codigo_TipoIdCliente);
                else
                    return null;
            }
        }
        public DateTime? FechaComprobacionDinardap { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Cliente_Final()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Cliente_Final(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Cliente_Final = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Cliente_Final = Convert.ToInt64(dt.Rows[0]["Id_Cliente_Final"]);
                    _Codigo_TipoIdCliente = Convert.ToString(dt.Rows[0]["Codigo_TipoIdCliente"]);
                    Cedula = (dt.Rows[0]["Cedula"]) == null ? "": Convert.ToString(dt.Rows[0]["Cedula"]);
                    _Nombre = (dt.Rows[0]["Nombre"]) == null ? "" : Convert.ToString(dt.Rows[0]["Nombre"]);
                    Telefono = (dt.Rows[0]["Telefono"]) == null ? "" : Convert.ToString(dt.Rows[0]["Telefono"]);
                    Direccion = (dt.Rows[0]["Direccion"]) == null ? "" : Convert.ToString(dt.Rows[0]["Direccion"]);
                    Email = (dt.Rows[0]["Email"]) == null ? "" : Convert.ToString(dt.Rows[0]["Email"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    Ciu = (dt.Rows[0]["Ciu"]) == null ? "" : Convert.ToString(dt.Rows[0]["Ciu"]);
                    FechaComprobacionDinardap = valorDateTime(dt.Rows[0]["FechaComprobacionDinardap"]);
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
                }
            }
        }

        public Cliente_Final(string ci_ruc)
        {
            using (DataTable dt = GetAllData(String.Format("Cedula = '{0}'", ci_ruc)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Cliente_Final = Convert.ToInt64(dt.Rows[0]["Id_Cliente_Final"]);
                    _Codigo_TipoIdCliente = Convert.ToString(dt.Rows[0]["Codigo_TipoIdCliente"]);
                    Cedula = Convert.ToString(dt.Rows[0]["Cedula"]);
                    _Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Telefono = (dt.Rows[0]["Telefono"]) == null ? "" : Convert.ToString(dt.Rows[0]["Telefono"]);
                    Direccion = (dt.Rows[0]["Direccion"]) == null ? "" : Convert.ToString(dt.Rows[0]["Direccion"]);
                    Email = (dt.Rows[0]["Email"]) == null ? "" : Convert.ToString(dt.Rows[0]["Email"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    Ciu = (dt.Rows[0]["Ciu"]) == null ? "" : Convert.ToString(dt.Rows[0]["Ciu"]);
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
            return String.Format("SELECT Id_Cliente_Final, Codigo_TipoIdCliente, Cedula, Nombre, Telefono, Direccion, Email, Activo, Ciu, " +
                "FechaComprobacionDinardap, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM CLIENTE_FINAL");
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

        public static Cliente_Final GetCliente_Final(long id)
        {
            return new Cliente_Final(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Cliente_Final", "Cliente_Final", "");
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
            return String.Format("INSERT INTO CLIENTE_FINAL (Cedula, Nombre, Telefono, Direccion, Email, Activo, Ciu, Codigo_TipoIdCliente, " +
                "FechaComprobacionDinardap, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES( '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', {8}, GETDATE(), {9}, {10}, {11}); ", 
                SqlServer.ValidarTexto(Cedula), SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Telefono), SqlServer.ValidarTexto(Direccion), 
                SqlServer.ValidarTexto(Email), Activo, SqlServer.ValidarTexto(Ciu), Codigo_TipoIdCliente, 
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
            return String.Format("DELETE FROM CLIENTE_FINAL WHERE Id_Cliente_Final = {0};", ID);
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
            return String.Format("UPDATE CLIENTE_FINAL SET Cedula = '{1}', Nombre = '{2}', Telefono = '{3}', Direccion = '{4}', Email = '{5}', " +
                "Activo = '{6}', Ciu = '{7}', Codigo_TipoIdCliente = '{8}', FechaComprobacionDinardap = {9}, Fecha_Creacion = {10}, Usuario_Creacion = {11}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {12} WHERE Id_Cliente_Final = {0};", 
                Id_Cliente_Final, SqlServer.ValidarTexto(Cedula), SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Telefono), 
                SqlServer.ValidarTexto(Direccion), SqlServer.ValidarTexto(Email), Activo, SqlServer.ValidarTexto(Ciu), Codigo_TipoIdCliente,
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
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Cliente_Final) FROM CLIENTE_FINAL " +
                "WHERE Cedula = '{0}' {1}", sCed, id != 0 ? "AND Id_Cliente_Final <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Ciu(string sCiu)
        {
            return IsDuplicate_Ciu(0, sCiu);
        }

        public bool IsDuplicate_Ciu(long id, string sCiu)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Cliente_Final) FROM CLIENTE_FINAL " +
                "WHERE Ciu = '{0}' {1}", sCiu, id != 0 ? "AND Id_Cliente_Final <> " + id.ToString() : ""))) > 0;
        }

        public long ObtenerId(string ced)
        {
            try
            {
                return Convert.ToInt64(SqlServer.EXEC_SCALAR(string.Format("SELECT Id_Cliente_Final FROM CLIENTE_FINAL WHERE Cedula = '{0}'", ced)));
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
        ~Cliente_Final()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
