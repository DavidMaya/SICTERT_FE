using AccesoDatos;
using CapaDatos.Frecuencias;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.Parqueo
{
    public partial class Pago_Parqueo : SqlConnecion, IDisposable
    {
        private long _Id_Tipo_Pago;
        private long _Id_Factura_Parqueo;
        private Tipo_pago _Tipo_pago;
        private Factura_Parqueo _Factura_parqueo;

        #region Public Properties
        public long Id_Pago_Parqueo { get; set; }
        public long Id_Tipo_Pago
        {
            get { return _Id_Tipo_Pago; }
            set
            {
                _Id_Tipo_Pago = value;
                _Tipo_pago = null;
            }
        }
        public decimal Valor { get; set; }
        public string Detalle { get; set; }
        public Nullable<DateTime> Fecha_Hora { get; set; }
        public int Estado { get; set; }
        public long Id_Factura_Parqueo
        {
            get { return _Id_Factura_Parqueo; }
            set
            {
                _Id_Factura_Parqueo = value;
                _Factura_parqueo = null;
            }
        }
        public Tipo_pago TipoPago
        {
            get
            {
                if (_Tipo_pago != null && _Tipo_pago.Id_Tipo_Pago != 0)
                    return _Tipo_pago;
                else if (_Id_Tipo_Pago != 0)
                    return _Tipo_pago = new Tipo_pago(_Id_Tipo_Pago);
                else
                    return null;
            }
        }
        public Factura_Parqueo FacturaTicket
        {
            get
            {
                if (_Factura_parqueo != null && _Factura_parqueo.Id_Factura_Parqueo != 0)
                    return _Factura_parqueo;
                else if (_Id_Factura_Parqueo != 0)
                    return _Factura_parqueo = new Factura_Parqueo(_Id_Factura_Parqueo);
                else
                    return null;
            }
        }
        #endregion

        public Pago_Parqueo()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Pago_Parqueo(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Pago_Parqueo = {0}", id)))
            {
                if (dt.Rows.Count > 0)
                {
                    Id_Pago_Parqueo = Convert.ToInt64(dt.Rows[0]["Id_Pago_Parqueo"]);
                    _Id_Tipo_Pago = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Pago"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    Detalle = Convert.ToString(dt.Rows[0]["Detalle"]);
                    Fecha_Hora = valorDateTime(dt.Rows[0]["Fecha_Hora"]);
                    Estado = Convert.ToInt32(dt.Rows[0]["Estado"]);
                    _Id_Factura_Parqueo = Convert.ToInt64(dt.Rows[0]["Id_Factura_Parqueo"]);
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
            return String.Format("SELECT Id_Pago_Parqueo, Id_Tipo_Pago, Valor, Detalle, Fecha_Hora, Estado, Id_Factura_Parqueo FROM PAGO_PARQUEO");
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

        public static Pago GetPago(long id)
        {
            return new Pago(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Pago_Parqueo", "PAGO_PARQUEO", "");
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
            return String.Format("INSERT INTO PAGO_PARQUEO (Id_Tipo_Pago, Valor, Detalle, Fecha_Hora, Estado, Id_Factura_Parqueo) VALUES ('{0}', '{1}', '{2}', {3}, '{4}', '{5}'); ", 
                Id_Tipo_Pago, Valor.ToString().Replace(",",SqlServer.SigFloatSql), SqlServer.ValidarTexto(Detalle), 
                (Fecha_Hora == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Estado, Id_Factura_Parqueo);
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
            return String.Format("DELETE FROM PAGO_PARQUEO WHERE Id_Pago_Parqueo = {0};", ID);
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
            return String.Format("UPDATE PAGO_PARQUEO SET Id_Tipo_Pago= '{1}', Valor= '{2}', Detalle= '{3}', Fecha_Hora= {4}, Estado= '{5}', " +
                "Id_Factura_Parqueo= '{6}' WHERE Id_Pago_Parqueo = {0};", Id_Pago_Parqueo, Id_Tipo_Pago, 
                Valor.ToString().Replace(",",SqlServer.SigFloatSql), SqlServer.ValidarTexto(Detalle), 
                (Fecha_Hora == null) ? "null" : "'" + Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Estado, 
                Id_Factura_Parqueo);
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
        ~Pago_Parqueo()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
