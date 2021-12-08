using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Transportistas
{
    public partial class Cuenta_Prepago_Coop : SqlConnecion, IDisposable
    {
        private long _Id_Cooperativa;
        private Cooperativa _Cooperativa;

        #region Public Properties
        public long Id_CuentaPrepago { get; set; }
        public string Clave { get; set; }
        public long Id_Cooperativa
        {
            get { return _Id_Cooperativa; }
            set
            {
                _Id_Cooperativa = value;
                _Cooperativa = null;
            }
        }
        public Cooperativa cooperativa
        {
            get
            {
                if (_Cooperativa != null && _Cooperativa.Id_Cooperativa != 0)
                    return _Cooperativa;
                else if (_Id_Cooperativa != 0)
                    return _Cooperativa = new Cooperativa(_Id_Cooperativa);
                else
                    return null;
            }
        }
        public bool Activo { get; set; }
        public string CuentaContable { get; set; }
        #endregion

        public Cuenta_Prepago_Coop()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Cuenta_Prepago_Coop(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_CuentaPrepago = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_CuentaPrepago = Convert.ToInt64(dt.Rows[0]["Id_CuentaPrepago"]);
                    Clave = Convert.ToString(dt.Rows[0]["Clave"]);
                    _Id_Cooperativa = Convert.ToInt64(dt.Rows[0]["Id_Cooperativa"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                }
            }
        }

        public Cuenta_Prepago_Coop(long id_coop, bool coop)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Cooperativa = {0}", id_coop)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_CuentaPrepago = Convert.ToInt64(dt.Rows[0]["Id_CuentaPrepago"]);
                    Clave = Convert.ToString(dt.Rows[0]["Clave"]);
                    _Id_Cooperativa = Convert.ToInt64(dt.Rows[0]["Id_Cooperativa"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
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
            return String.Format("SELECT Id_CuentaPrepago, Clave, Id_Cooperativa, Activo FROM CUENTA_PREPAGO_COOP");
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

        public static Cuenta_Prepago_Coop GetCuenta_prepago(long id)
        {
            return new Cuenta_Prepago_Coop(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_CuentaPrepago", "CUENTA_PREPAGO_COOP", "");
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
            return String.Format("INSERT INTO CUENTA_PREPAGO_COOP (Clave, Id_Cooperativa, Activo) VALUES ('{0}', {1}, {2}); ",
                SqlServer.ValidarTexto(Clave), _Id_Cooperativa, Activo ? "1" : "0");
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
            return String.Format("DELETE FROM CUENTA_PREAPAGO_COOP WHERE Id_CuentaPrepago = {0};", ID);
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
            return String.Format("UPDATE CUENTA_PREPAGO_COOP SET Clave = '{1}', Id_Cooperativa = {2}, Activo = {3} WHERE Id_CuentaPrepago = {0};",
                Id_CuentaPrepago, SqlServer.ValidarTexto(Clave), _Id_Cooperativa, Activo ? "1" : "0");
        }

        public decimal ObtenerSaldo(long ID)
        {
            try
            {
                return Convert.ToDecimal(SqlServer.EXEC_SCALAR("SELECT CAST(ISNULL(SUM(Saldo), 0) AS DECIMAL(19,2)) FROM SALDO_PREPAGO_COOP " +
                    "WHERE Id_CuentaPrepago = " + ID.ToString()));
            }
            catch
            {
                return 0;
            }
        }

        public DataTable ObtenerSaldos(long ID)
        {
            try
            {
                return SqlServer.EXEC_SELECT("EXEC ObtenerSaldoPrepagoCoop " + ID.ToString());
            }
            catch
            {
                return null;
            }
        }

        #region Codigo nuevo

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
        ~Cuenta_Prepago_Coop()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
