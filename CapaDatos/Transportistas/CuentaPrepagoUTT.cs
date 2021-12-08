using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Transportistas
{
    public partial class Cuenta_Prepago_UTT : SqlConnecion, IDisposable
    {
        private long _Id_Unidad_Transporte;
        private Unidad_transporte _Unidad_Transporte;

        #region Public Properties
        public long Id_CuentaPrepago { get; set; }
        public long Id_Unidad_Transporte
        {
            get { return _Id_Unidad_Transporte; }
            set
            {
                _Id_Unidad_Transporte = value;
                _Unidad_Transporte = null;
            }
        }
        public decimal Saldo { get; set; }
        public Unidad_transporte Unidad_Transporte
        {
            get
            {
                if (_Unidad_Transporte != null && _Unidad_Transporte.Id_Unidad_Transporte != 0)
                    return _Unidad_Transporte;
                else if (_Id_Unidad_Transporte != 0)
                    return _Unidad_Transporte = new Unidad_transporte(_Id_Unidad_Transporte);
                else
                    return null;
            }
        }
        #endregion

        public Cuenta_Prepago_UTT()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Cuenta_Prepago_UTT(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_CuentaPrepago = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_CuentaPrepago = Convert.ToInt64(dt.Rows[0]["Id_CuentaPrepago"]);
                    _Id_Unidad_Transporte = Convert.ToInt64(dt.Rows[0]["Id_Unidad_Transporte"]);
                    Saldo = Convert.ToDecimal(dt.Rows[0]["Saldo"]);
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
            return String.Format("SELECT Id_CuentaPrepago, Id_Unidad_Transporte, Saldo FROM CUENTA_PREPAGO_UTT");
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

        public static Cuenta_Prepago_UTT GetCuenta_prepago(long id)
        {
            return new Cuenta_Prepago_UTT(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_CuentaPrepago", "CUENTA_PREPAGO_UTT", "");
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
            return String.Format("INSERT INTO CUENTA_PREPAGO_UTT (Id_Unidad_Transporte, Saldo) VALUES ({0}, '{1}'); ", 
                Id_Unidad_Transporte, Saldo.ToString().Replace(",", SqlServer.SigFloatSql));
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
            return String.Format("DELETE FROM CUENTA_PREPAGO_UTT WHERE Id_CuentaPrepago = {0};", ID);
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
            return String.Format("UPDATE CUENTA_PREPAGO_UTT SET Id_Unidad_Transporte= '{1}', Saldo= '{2}' WHERE Id_CuentaPrepago = {0};", 
                Id_CuentaPrepago, Id_Unidad_Transporte, Saldo.ToString().Replace(",", SqlServer.SigFloatSql));
        }

        public static decimal GetSaldo(long ID)
        {
            string sql = string.Format("EXEC SaldoPrepago {0} ", ID);
            return Convert.ToDecimal(SqlServer.EXEC_SCALAR(sql));
        }

        public string Get_SQL_UPDATE_SALDO(long id, decimal valor)
        {
            return string.Format("UPDATE CUENTA_PREPAGO_UTT SET Valor = Valor + {0} WHERE Id_CuentaPrepago = {1} ; ", valor, id.ToString().Replace(",", SqlServer.SigFloatSql));
        }

        public Cuenta_Prepago_UTT ObtenerCuentaPorIdUnidad(long id_unidad)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Unidad_Transporte = {0}", id_unidad)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_CuentaPrepago = Convert.ToInt64(dt.Rows[0]["Id_CuentaPrepago"]);
                    _Id_Unidad_Transporte = Convert.ToInt64(dt.Rows[0]["Id_Unidad_Transporte"]);
                    Saldo = Convert.ToDecimal(dt.Rows[0]["Saldo"]);
                }
            }
            return this;
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
        ~Cuenta_Prepago_UTT()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
