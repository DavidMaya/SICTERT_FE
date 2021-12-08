using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.UsuariosPerfiles
{
    public partial class  Usuarios : SqlConnecion, IDisposable
    {
        private long _Id_Perfil;
        private Perfilusuario _Perfil;

        #region Public Properties
        public long Id_Usuario { get; set; }
        public string CI_Ruc { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public bool Estado { get; set; }
        public long Id_Perfil
        {
            get { return _Id_Perfil; }
            set
            {
                _Id_Perfil = value;
                _Perfil = null;
            }
        }
        public Nullable<DateTime> Fecha_Activo { get; set; }
        public bool Activo { get; set; }
        public Perfilusuario Perfil
        {
            get
            {
                if (_Perfil != null && _Perfil.Id_Perfil != 0)
                    return _Perfil;
                else if (_Id_Perfil != 0)
                    return _Perfil = new Perfilusuario(_Id_Perfil);
                else
                    return null;
            }
        }
        public string Modulo_Defecto { get; set; }
        public DateTime? FechaComprobacionDinardap { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Usuarios()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Usuarios(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Usuario = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Usuario = Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
                    CI_Ruc = Convert.ToString(dt.Rows[0]["CI_Ruc"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Usuario = Convert.ToString(dt.Rows[0]["Usuario"]);
                    Clave = Convert.ToString(dt.Rows[0]["Clave"]);
                    Estado = Convert.ToBoolean(dt.Rows[0]["Estado"]);
                    _Id_Perfil = Convert.ToInt64(dt.Rows[0]["Id_Perfil"]);
                    Fecha_Activo = valorDateTime(dt.Rows[0]["Fecha_Activo"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    Modulo_Defecto = dt.Rows[0]["Modulo_Defecto"] != DBNull.Value ? dt.Rows[0]["Modulo_Defecto"].ToString() : "";
                    FechaComprobacionDinardap = valorDateTime(dt.Rows[0]["FechaComprobacionDinardap"]);
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
                }
            }
        }

        public Usuarios(string ced)
        {
            using (DataTable dt = GetAllData(String.Format("CI_Ruc = '{0}'", ced)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Usuario = Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
                    CI_Ruc = Convert.ToString(dt.Rows[0]["CI_Ruc"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Usuario = Convert.ToString(dt.Rows[0]["Usuario"]);
                    Clave = Convert.ToString(dt.Rows[0]["Clave"]);
                    Estado = Convert.ToBoolean(dt.Rows[0]["Estado"]);
                    _Id_Perfil = Convert.ToInt64(dt.Rows[0]["Id_Perfil"]);
                    Fecha_Activo = valorDateTime(dt.Rows[0]["Fecha_Activo"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    Modulo_Defecto = dt.Rows[0]["Modulo_Defecto"] != DBNull.Value ? dt.Rows[0]["Modulo_Defecto"].ToString() : "";
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
            return String.Format("SELECT Id_Usuario, CI_Ruc, Nombre, Usuario, Clave, Estado, Id_Perfil, Fecha_Activo, ISNULL(Activo, 0) Activo, " +
                "Modulo_Defecto, FechaComprobacionDinardap, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM USUARIO");
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

        public static Usuarios GetUsuario(long id)
        {
            return new Usuarios(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Usuario", "USUARIO", "");
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
            return String.Format("INSERT INTO USUARIO (CI_Ruc, Nombre, Usuario, Clave, Estado, Id_Perfil, Fecha_Activo, Activo, Modulo_Defecto, " +
                "FechaComprobacionDinardap, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, '{7}', {8}, {9}, GETDATE(), {10}, {11}, {12}); ", 
                SqlServer.ValidarTexto(CI_Ruc), SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Usuario), 
                SqlServer.ValidarTexto(Clave), Estado, Id_Perfil, 
                (Fecha_Activo == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Activo.ToString()).ToString(SqlServer.Formatofecha) + "'", 
                Activo, string.IsNullOrEmpty(Modulo_Defecto) ? "NULL" : Modulo_Defecto,
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
            return String.Format("DELETE FROM USUARIO WHERE Id_Usuario = {0};", ID);
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
            return String.Format("UPDATE USUARIO SET CI_Ruc= '{1}', Nombre= '{2}', Usuario= '{3}', Clave= '{4}', Estado= '{5}', Id_Perfil= '{6}', " +
                "Fecha_Activo= {7}, Activo= '{8}', Modulo_Defecto= {9}, FechaComprobacionDinardap = {10}, Fecha_Creacion = {11}, Usuario_Creacion = {12}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {13} WHERE Id_Usuario = {0};", 
                Id_Usuario, SqlServer.ValidarTexto(CI_Ruc), SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Usuario), 
                SqlServer.ValidarTexto(Clave), Estado, Id_Perfil, 
                (Fecha_Activo == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Activo.ToString()).ToString(SqlServer.Formatofecha) + "'", Activo, 
                string.IsNullOrEmpty(Modulo_Defecto) ? "NULL" : "'" + Modulo_Defecto + "'",
                (FechaComprobacionDinardap == null) ? "NULL" : "'" + Convert.ToDateTime(FechaComprobacionDinardap.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool verificarUsuarioFrecExtra(string usuario, string contrasenia)
        {
            string sql = string.Format("SELECT COUNT(Id_Usuario) FROM USUARIO u INNER JOIN OPCIONES_PERFIL o ON o.Id_Perfil = u.Id_Perfil INNER JOIN " +
                "MODULO m ON m.Id_Modulo = o.Id_Modulo WHERE USUARIO = '{0}' AND ESTADO = 1 AND CLAVE = '{1}' AND m.Codigo = 'HAB-FREC-EXTRA'", 
                usuario, contrasenia);
            return (Convert.ToInt16(SqlServer.EXEC_SCALAR(sql)) > 0) ? true : false;
        }

        public bool IsDuplicate_Cedula(string sCed)
        {
            return IsDuplicate_Cedula(0, sCed);
        }

        public bool IsDuplicate_Cedula(long id, string sCed)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Usuario) FROM USUARIO WHERE CI_Ruc = '{0}' {1}", 
                sCed, id != 0 ? "AND Id_Usuario <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Usuario(string Usr)
        {
            return IsDuplicate_Usuario(0, Usr);
        }

        public bool IsDuplicate_Usuario(long id, string Usr)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Usuario) FROM USUARIO WHERE Usuario = '{0}' {1}", 
                Usr, id != 0 ? "AND Id_Usuario <> " + id.ToString() : ""))) > 0;
        }

        public long ObtenerId(string ced)
        {
            try
            {
                return Convert.ToInt64(SqlServer.EXEC_SCALAR(string.Format("SELECT Id_Usuario FROM USUARIO WHERE CI_Ruc = '{0}'", ced)));
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
        ~Usuarios()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
