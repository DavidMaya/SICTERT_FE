using AccesoDatos;
using System;
using System.Data;
using System.Security.Permissions;

namespace CapaDatos.Service
{
    public class SaldoPrepagoCoop : SqlConnecion, IDisposable
    {
        public long IdCuentaPrepago { get; set; }
        public long IdTipoTarifa { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaHora { get; set; }

        public SaldoPrepagoCoop()
        {
        }

        public SaldoPrepagoCoop(long id_cuenta, long id_tipo)
        {
            using (DataTable dt = GetAllData(String.Format("Id_CuentaPrepago = {0} AND Id_Tipo_Tarifa = {1}", id_cuenta, id_tipo)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    IdCuentaPrepago = Convert.ToInt64(dt.Rows[0]["Id_CuentaPrepago"]);
                    IdTipoTarifa = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Tarifa"]);
                    Saldo = Convert.ToDecimal(dt.Rows[0]["Saldo"]);
                    FechaHora = Convert.ToDateTime(dt.Rows[0]["FechaHora"]);
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
            return String.Format("SELECT Id_CuentaPrepago, Id_Tipo_Tarifa, Saldo, FechaHora FROM SALDO_PREPAGO_COOP");
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

        public static SaldoPrepagoCoop GetSaldoPrepagoCoop(long id_cuenta, long id_tipo)
        {
            return new SaldoPrepagoCoop(id_cuenta, id_tipo);
        }

        static public long Next_Codigo()
        {
            return 0;
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
            return String.Format("INSERT INTO SALDO_PREPAGO_COOP (Id_CuentaPrepago, Id_Tipo_Tarifa, Saldo, FechaHora) VALUES ({0}, {1}, {2}, '{3}'); ",
                IdCuentaPrepago, IdTipoTarifa, Saldo.ToString().Replace(",", SqlServer.SigFloatSql), Convert.ToDateTime(FechaHora.ToString()).ToString(SqlServer.FormatofechaHora));
        }

        public string Delete(long id_Cuenta, long id_Tipo)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id_Cuenta, id_Tipo));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete(long id_Cuenta, long id_Tipo)
        {
            return String.Format("DELETE FROM SALDO_PREPAGO_COOP WHERE Id_CuentaPrepago = {0} AND Id_Tipo_Tarifa = {1};", id_Cuenta, id_Tipo);
        }

        public string Update(long id_Cuenta, long id_Tipo)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate(id_Cuenta, id_Tipo));
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;
        }

        public string GetSQLUpdate(long id_Cuenta, long id_Tipo)
        {
            return String.Format("UPDATE SALDO_PREPAGO_COOP SET Saldo = {2}, FechaHora = '{3}' WHERE Id_CuentaPrepago = {0} AND Id_Tipo_Tarifa = {1};",
                id_Cuenta, id_Tipo, Saldo.ToString().Replace(",", SqlServer.SigFloatSql), Convert.ToDateTime(FechaHora.ToString()).ToString(SqlServer.FormatofechaHora));
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
        ~SaldoPrepagoCoop()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
