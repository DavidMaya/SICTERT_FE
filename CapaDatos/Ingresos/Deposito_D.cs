using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class Deposito_D : SqlConnecion, IDisposable
    {
        private long _Id_Deposito;
        private Deposito _Deposito;
        private long _Id_CuentaBancaria;
        private CuentaBancaria _CuentaBancaria;
        private long _Id_Tipo_Pago;
        private Tipo_pago _Tipo_Pago;

        #region Public Properties
        public long Id_DetalleDeposito { get; set; }
        public decimal Valor { get; set; }
        public string Banco { get; set; }
        public string NumeroCuenta { get; set; }
        public string CuentaContable { get; set; }
        public long Id_Deposito
        {
            get { return _Id_Deposito; }
            set
            {
                _Id_Deposito = value;
                _Deposito = null;
            }
        }
        public Deposito deposito
        {
            get
            {
                if (_Deposito != null && _Deposito.Id_Deposito != 0)
                    return _Deposito;
                else if (_Id_Deposito != 0)
                    return _Deposito = new Deposito(_Id_Deposito);
                else
                    return null;
            }
        }
        public long Id_CuentaBancaria
        {
            get { return _Id_CuentaBancaria; }
            set
            {
                _Id_CuentaBancaria = value;
                _CuentaBancaria = null;
            }
        }
        public CuentaBancaria cuentabancaria
        {
            get
            {
                if (_CuentaBancaria != null && _CuentaBancaria.Id_CuentaBancaria != 0)
                    return _CuentaBancaria;
                else if (_Id_CuentaBancaria != 0)
                    return _CuentaBancaria = new CuentaBancaria(_Id_CuentaBancaria);
                else
                    return null;
            }
        }
        public long Id_TipoPago
        {
            get { return _Id_Tipo_Pago; }
            set
            {
                _Id_Tipo_Pago = value;
                _Tipo_Pago = null;
            }
        }
        public Tipo_pago tipopago
        {
            get
            {
                if (_Tipo_Pago != null && _Tipo_Pago.Id_Tipo_Pago != 0)
                    return _Tipo_Pago;
                else if (_Id_Tipo_Pago != 0)
                    return _Tipo_Pago = new Tipo_pago(_Id_Tipo_Pago);
                else
                    return null;
            }
        }
        public string TipoPago { get; set; }
        public string NumPapeleta { get; set; }
        #endregion

        public Deposito_D()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Deposito_D(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_DetalleDeposito = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_DetalleDeposito = Convert.ToInt64(dt.Rows[0]["Id_DetalleDeposito"]);
                    _Id_Deposito = Convert.ToInt64(dt.Rows[0]["Id_Deposito"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    _Id_CuentaBancaria = Convert.ToInt64(dt.Rows[0]["Id_CuentaBancaria"]);
                    Banco = Convert.ToString(dt.Rows[0]["Banco"]);
                    NumeroCuenta = Convert.ToString(dt.Rows[0]["NumeroCuenta"]);
                    CuentaContable = Convert.ToString(dt.Rows[0]["CuentaContable"]);
                    _Id_Tipo_Pago = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Pago"]);
                    TipoPago = Convert.ToString(dt.Rows[0]["TipoPago"]);
                    NumPapeleta = Convert.ToString(dt.Rows[0]["NumPapeleta"]);
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
            return String.Format("SELECT Id_DetalleDeposito, Id_Deposito, Valor, Id_CuentaBancaria, Banco, NumeroCuenta, CuentaContable, Id_Tipo_Pago, " +
                "TipoPago, NumPapeleta FROM DETALLE_DEPOSITO");
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

        public static Deposito_D GetDeposito_D(long id)
        {
            return new Deposito_D(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_DetalleDeposito", "DETALLE_DEPOSITO", "");
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
            return String.Format("INSERT INTO DETALLE_DEPOSITO (Id_Deposito, Valor, Id_CuentaBancaria, Banco, NumeroCuenta, CuentaContable, " +
                "Id_Tipo_Pago, TipoPago, NumPapeleta) VALUES ({0}, {1}, {2}, '{3}', '{4}', '{5}', {6}, '{7}', {8}); ", 
                _Id_Deposito, Valor.ToString().Replace(",", SqlServer.SigFloatSql), _Id_CuentaBancaria, SqlServer.ValidarTexto(Banco), 
                SqlServer.ValidarTexto(NumeroCuenta), SqlServer.ValidarTexto(CuentaContable), _Id_Tipo_Pago, SqlServer.ValidarTexto(TipoPago),
                NumPapeleta != null ? "'" + SqlServer.ValidarTexto(NumPapeleta) + "'" : "NULL");
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
            return String.Format("DELETE FROM DETALLE_DEPOSITO WHERE Id_DetalleDeposito = {0};", ID);
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
            return String.Format("UPDATE DETALLE_DEPOSITO SET Id_Deposito = {1}, Valor = {2}, Id_CuentaBancaria = {3}, Banco = '{4}', NumeroCuenta = '{5}', " +
                "CuentaContable = '{6}', Id_TipoPago = {7}, TipoPago = '{8}', NumPapeleta = '{9}' WHERE Id_DetalleDeposito = {0};", Id_DetalleDeposito, 
                _Id_Deposito,  Valor.ToString().Replace(",", SqlServer.SigFloatSql), _Id_CuentaBancaria, SqlServer.ValidarTexto(Banco), 
                SqlServer.ValidarTexto(NumeroCuenta), SqlServer.ValidarTexto(CuentaContable), _Id_Tipo_Pago, SqlServer.ValidarTexto(TipoPago), 
                NumPapeleta != null ? "'" + SqlServer.ValidarTexto(NumPapeleta) + "'" : "NULL");
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
        ~Deposito_D()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
