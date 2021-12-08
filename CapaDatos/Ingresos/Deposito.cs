using AccesoDatos;
using CapaDatos.UsuariosPerfiles;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class Deposito : SqlConnecion, IDisposable
    {
        private long _Id_Usuario;
        private Usuarios _Usuario;
        private long _Id_CierreCaja;
        private Cierre_caja _CierreCaja;

        #region Public Properties
        public long Id_Deposito { get; set; }
        public DateTime Fecha { get; set; }
        public long Id_Usuario
        {
            get { return _Id_Usuario; }
            set
            {
                _Id_Usuario = value;
                _Usuario = null;
            }
        }
        public string NombreUsuario { get; set; }
        public Usuarios usuario
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
        public long Id_CierreCaja
        {
            get { return _Id_CierreCaja; }
            set
            {
                _Id_CierreCaja = value;
                _CierreCaja = null;
            }
        }
        public Cierre_caja CierreCaja
        {
            get
            {
                if (_CierreCaja != null && _CierreCaja.Id_Cierre_Caja != 0)
                    return _CierreCaja;
                else if (_Id_CierreCaja != 0)
                    return _CierreCaja = new Cierre_caja(_Id_CierreCaja);
                else
                    return null;
            }
        }
        #endregion

        public Deposito()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Deposito(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Deposito = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Deposito = Convert.ToInt64(dt.Rows[0]["Id_Deposito"]);
                    Fecha = Convert.ToDateTime(dt.Rows[0]["Fecha"]);
                    _Id_CierreCaja = Convert.ToInt64(dt.Rows[0]["Id_CierreCaja"]);
                    _Id_Usuario = Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
                    NombreUsuario = Convert.ToString(dt.Rows[0]["NombreUsuario"]);
                }
            }
        }

        public Deposito(long id, bool cierre)
        {
            if (cierre)
            {
                // el id es de un cierre
                using (DataTable dt = GetAllData(String.Format("Id_CierreCaja = {0}", id)))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Id_Deposito = Convert.ToInt64(dt.Rows[0]["Id_Deposito"]);
                        Fecha = Convert.ToDateTime(dt.Rows[0]["Fecha"]);
                        _Id_CierreCaja = Convert.ToInt64(dt.Rows[0]["Id_CierreCaja"]);
                        _Id_Usuario = Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
                        NombreUsuario = Convert.ToString(dt.Rows[0]["NombreUsuario"]);
                    }
                }
            }
            else
            {
                // el id es de un deposito
                using (DataTable dt = GetAllData(String.Format("Id_Deposito = {0}", id)))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Id_Deposito = Convert.ToInt64(dt.Rows[0]["Id_Deposito"]);
                        Fecha = Convert.ToDateTime(dt.Rows[0]["Fecha"]);
                        _Id_CierreCaja = Convert.ToInt64(dt.Rows[0]["Id_CierreCaja"]);
                        _Id_Usuario = Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
                        NombreUsuario = Convert.ToString(dt.Rows[0]["NombreUsuario"]);
                    }
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
            return String.Format("SELECT Id_Deposito, Id_CierreCaja, Fecha, Id_Usuario, NombreUsuario FROM DEPOSITO");
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

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Deposito", "DEPOSITO", "");
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
            return String.Format("INSERT INTO DEPOSITO (Fecha, Id_CierreCaja, Id_Usuario, NombreUsuario) VALUES ('{0}', {1}, {2}, '{3}'); ",
                Convert.ToDateTime(Fecha.ToString()).ToString(SqlServer.FormatofechaHora), _Id_CierreCaja, _Id_Usuario, SqlServer.ValidarTexto(NombreUsuario));
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
            return String.Format("DELETE FROM DEPOSITO WHERE Id_Deposito = {0};", ID);
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
            return String.Format("UPDATE DEPOSITO SET Fecha = '{1}', Id_CierreCaja = {2}, Id_Usuario = {3}, NombreUsuario = '{4}' WHERE Id_Deposito = {0};", 
                Id_Deposito, Convert.ToDateTime(Fecha.ToString()).ToString(SqlServer.FormatofechaHora), _Id_CierreCaja, _Id_Usuario, 
                SqlServer.ValidarTexto(NombreUsuario));
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
        ~Deposito()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
