using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class EnvioCierre_D : SqlConnecion, IDisposable
    {
        private long _Id_EnvioCierre;
        private Envio_Cierre _EnvioCierre;
        
        #region Public Properties
        public long Id_DetalleEnvio { get; set; }
        public int CodigoRubro { get; set; }
        public long NumDesde { get; set; }
        public long NumHasta { get; set; }
        public string TipoPago { get; set; }
        public decimal ValorUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal ValorConIva { get; set; }
        public decimal ValorSinIva { get; set; }
        public decimal ValorTotal { get; set; }

        public long Id_EnvioCierre
        {
            get { return _Id_EnvioCierre; }
            set
            {
                _Id_EnvioCierre = value;
                _EnvioCierre = null;
            }
        }
        public Envio_Cierre EnvioCierre
        {
            get
            {
                if (_EnvioCierre != null && _EnvioCierre.Id_EnvioCierre != 0)
                    return _EnvioCierre;
                else if (_Id_EnvioCierre != 0)
                    return _EnvioCierre = new Envio_Cierre(_Id_EnvioCierre);
                else
                    return null;
            }
        }
        #endregion

        public EnvioCierre_D()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public EnvioCierre_D(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_DetalleEnvio = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_DetalleEnvio = Convert.ToInt64(dt.Rows[0]["Id_DetalleEnvio"]);
                    _Id_EnvioCierre = Convert.ToInt64(dt.Rows[0]["Id_EnvioCierre"]);
                    CodigoRubro = Convert.ToInt32(dt.Rows[0]["CodigoRubro"]);
                    NumDesde = Convert.ToInt64(dt.Rows[0]["NumDesde"]);
                    NumHasta = Convert.ToInt64(dt.Rows[0]["NumHasta"]);
                    TipoPago = Convert.ToString(dt.Rows[0]["TipoPago"]);
                    ValorUnitario = Convert.ToDecimal(dt.Rows[0]["ValorUnitario"]);
                    Cantidad = Convert.ToInt32(dt.Rows[0]["Cantidad"]);
                    ValorConIva = Convert.ToDecimal(dt.Rows[0]["ValorConIva"]);
                    ValorSinIva = Convert.ToDecimal(dt.Rows[0]["ValorSinIva"]);
                    ValorTotal = Convert.ToDecimal(dt.Rows[0]["ValorTotal"]);
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
            return String.Format("SELECT Id_DetalleEnvio, Id_EnvíoCierre, CodigoRubro, NumDesde, NumHasta, TipoPago, ValorUnitario, Cantidad, " +
                "ValorConIva, ValorSinIva, ValorTotal FROM DETALLE_ENVIO_CIERRE");
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

        public static EnvioCierre_D GetEnvioCierre_D(long id)
        {
            return new EnvioCierre_D(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_DetalleEnvio", "DETALLE_ENVIO_CIERRE", "");
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
            return String.Format("INSERT INTO DETALLE_ENVIO_CIERRE (Id_EnvíoCierre, CodigoRubro, NumDesde, NumHasta, TipoPago, ValorUnitario, Cantidad, " +
                "ValorConIva, ValorSinIva, ValorTotal) VALUES ({0}, {1}, {2}, {3}, '{4}', {5}, {6}, {7}, {8}, {9}); ", 
                _Id_EnvioCierre, CodigoRubro, NumDesde, NumHasta, TipoPago, ValorUnitario.ToString().Replace(",", SqlServer.SigFloatSql), 
                Cantidad, ValorConIva.ToString().Replace(",", SqlServer.SigFloatSql), ValorSinIva.ToString().Replace(",", SqlServer.SigFloatSql),
                ValorTotal.ToString().Replace(",", SqlServer.SigFloatSql));
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
            return String.Format("DELETE FROM DETALLE_ENVIO_CIERRE WHERE Id_DetalleEnvio = {0};", ID);
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
            return String.Format("UPDATE DETALLE_ENVIO_CIERRE SET Id_EnvíoCierre = {1}, CodigoRubro = {2}, NumDesde = {3}, NumHasta = {4}, TipoPago = '{5}', " +
                "ValorUnitario = {6}, Cantidad = {7}, ValorConIva = {8}, ValorSinIva = {9}, ValorTotal = {10} WHERE Id_DetalleEnvio = {0};", 
                Id_DetalleEnvio, _Id_EnvioCierre, CodigoRubro, NumDesde, NumHasta, TipoPago, ValorUnitario.ToString().Replace(",", SqlServer.SigFloatSql),
                Cantidad, ValorConIva.ToString().Replace(",", SqlServer.SigFloatSql), ValorSinIva.ToString().Replace(",", SqlServer.SigFloatSql),
                ValorTotal.ToString().Replace(",", SqlServer.SigFloatSql));
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
        ~EnvioCierre_D()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
