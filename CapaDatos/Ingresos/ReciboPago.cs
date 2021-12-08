using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using CapaDatos.Transportistas;
using CapaDatos.Ingresos;

namespace AccesoDatos
{
    public partial class Recibo_pago : SqlConnecion, IDisposable
    {
        private long _Id_Cierre_Caja;
        private long _Id_Caja;
        private long _Id_Unidad_Transporte;
        private Cierre_caja _Cierre_caja;
        private Caja _Caja;
        private Unidad_transporte _Unidad_transporte;

        #region Public Properties
        public long Id_Recibo_Pago { get; set; }
        public long Numero { get; set; }
        public Nullable<DateTime> Fecha_Hora { get; set; }
        public decimal Valor { get; set; }
        public long Id_Cierre_Caja
        {
            get { return _Id_Cierre_Caja; }
            set { 
                _Id_Cierre_Caja = value;
                _Cierre_caja = null;
            }
        }
        public long Id_Caja
        {
            get { return _Id_Caja; }
            set { 
                _Id_Caja = value;
                _Caja = null;
            }
        }
        public long Id_Unidad_Transporte
        {
            get { return _Id_Unidad_Transporte; }
            set { 
                _Id_Unidad_Transporte = value;
                _Unidad_transporte = null;
            }
        }
        public string Descripcion { get; set; }
        public Cierre_caja cierre_caja
        {
            get
            {
                if (_Cierre_caja != null && _Cierre_caja.Id_Cierre_Caja != 0)
                    return _Cierre_caja;
                else if (_Id_Cierre_Caja != 0)
                    return _Cierre_caja = new Cierre_caja(_Id_Cierre_Caja);
                else
                    return null;
            }
        }
        public Caja caja
        {
            get
            {
                if (_Caja != null && _Caja.Id_Caja != 0)
                    return _Caja;
                else if (_Id_Caja != 0)
                    return _Caja = new Caja(_Id_Caja);
                else
                    return null;
            }
        }
        public Unidad_transporte unidad_transporte
        {
            get
            {
                if (_Unidad_transporte != null && _Unidad_transporte.Id_Unidad_Transporte != 0)
                    return _Unidad_transporte;
                else if (_Id_Unidad_Transporte != 0)
                    return _Unidad_transporte = new Unidad_transporte(_Id_Unidad_Transporte);
                else
                    return null;
            }
        }
        #endregion

        public Recibo_pago()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Recibo_pago(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Recibo_Pago = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Recibo_Pago = Convert.ToInt64(dt.Rows[0]["Id_Recibo_Pago"]);
                    Numero = Convert.ToInt64(dt.Rows[0]["Numero"]);
                    Fecha_Hora = valorDateTime(dt.Rows[0]["Fecha_Hora"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["valor"]);
                    _Id_Cierre_Caja = Convert.ToInt64(dt.Rows[0]["Id_Cierre_Caja"]);
                    _Id_Caja = Convert.ToInt64(dt.Rows[0]["Id_Caja"]);
                    _Id_Unidad_Transporte = Convert.ToInt64(dt.Rows[0]["Id_Unidad_Transporte"]);
                    Descripcion = Convert.ToString(dt.Rows[0]["Descripcion"]);
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
            return String.Format("SELECT Id_Recibo_Pago, Numero, Fecha_Hora, valor, Id_Cierre_Caja, Id_Caja, Id_Unidad_Transporte, Descripcion FROM RECIBO_PAGO");
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

        public static Recibo_pago GetRecibo_pago(long id)
        {
            return new Recibo_pago(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Recibo_Pago", "Recibo_pago", "");
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
            return String.Format("INSERT INTO RECIBO_PAGO (Id_Recibo_Pago, Numero, Fecha_Hora, valor, Id_Cierre_Caja, Id_Caja, Id_Unidad_Transporte, Descripcion) " +
                "VALUES( '{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}'); ", Id_Recibo_Pago, Numero, 
                (Fecha_Hora == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                Valor.ToString().Replace(",", SqlServer.SigFloatSql), Id_Cierre_Caja, Id_Caja, Id_Unidad_Transporte, SqlServer.ValidarTexto(Descripcion));
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
            return String.Format("DELETE FROM RECIBO_PAGO WHERE Id_Recibo_Pago = {0};", ID);
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
            return String.Format("UPDATE  RECIBO_PAGO   SET Numero= '{1}', Fecha_Hora= {2}, valor= '{3}', Id_Cierre_Caja= '{4}', Id_Caja= '{5}', " +
                "Id_Unidad_Transporte= '{6}', Descripcion= '{7}'  WHERE Id_Recibo_Pago = {0};", Id_Recibo_Pago, Numero, 
                (Fecha_Hora == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                Valor.ToString().Replace(",", SqlServer.SigFloatSql), Id_Cierre_Caja, Id_Caja, Id_Unidad_Transporte, SqlServer.ValidarTexto(Descripcion));
        }

        #region Codigo nuevo

        static public long Next_Numero()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Numero", "Recibo_pago", "");
            return Convert.ToInt32(SqlServer.EXEC_SCALAR(sql));
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
        ~Recibo_pago()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
