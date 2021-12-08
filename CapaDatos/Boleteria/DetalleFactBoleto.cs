using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CapaDatos.Boleteria
{
    public class DetalleFactBoleto : SqlConnecion, IDisposable
    {

        #region Public Properties
        public long IdDetalleFactBoleto { get; set; }
        public long IdFacturaBoleto { get; set; }
        public long IdConceptoCuenta { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }
        public decimal Iva { get; set; }
        public decimal IvaValor { get; set; }
        public bool Estado { get; set; } 
        #endregion

        #region Constructor
        public DetalleFactBoleto() { }

        public DetalleFactBoleto(long id)
        {
            string sql = String.Format(@"SELECT Id_detalle_fact_boleto ,Id_factura_boleto, Id_Concepto_Cuenta,
                Nombre, Valor, Estado, Cantidad, Iva
                FROM dbo.DETALLE_FACT_BOLETO
                WHERE Id_detalle_fact_boleto = {0}", id);

            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                IdDetalleFactBoleto = Convert.ToInt64(table.Rows[0]["Id_Viaje"]);
                IdFacturaBoleto = Convert.ToInt64(table.Rows[0]["Id_factura_boleto"]);
                IdConceptoCuenta = Convert.ToInt64(table.Rows[0]["Id_Concepto_Cuenta"]);
                Nombre = table.Rows[0]["Nombre"].ToString();
                Cantidad = Convert.ToInt32(table.Rows[0]["Cantidad"].ToString());
                Valor = Convert.ToDecimal(table.Rows[0]["Valor"].ToString());
                Iva = Convert.ToDecimal(table.Rows[0]["Valor"].ToString());
                Estado = Convert.ToBoolean(table.Rows[0]["Estado"].ToString());
            }
        }
        #endregion

        #region Select Data Method
        private static string GetSqlSelect()
        {
            return String.Format(@"SELECT Id_detalle_fact_boleto ,Id_factura_boleto, Id_Concepto_Cuenta,
                Nombre, Valor, Estado, Cantidad, Iva
                FROM DETALLE_FACT_BOLETO");
        }

        public static DataTable GetAllData()
        {
            return GetAllData("");
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

        public static EstadoAsiento getEstadoAsiento(long id)
        {
            return new EstadoAsiento(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_detalle_fact_boleto", "DETALLE_FACT_BOLETO", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }
        #endregion

        #region Insert Data Method
        public string Insert()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }
        public string GetSQLInsert()
        {
            return String.Format(@"INSERT INTO DETALLE_FACT_BOLETO (Id_factura_boleto, Id_Concepto_Cuenta, 
                Nombre, Valor, Estado, Cantidad, Iva)
                VALUES ({0},{1},'{2}',{3},{4},{5},{6});",
                IdFacturaBoleto,
                IdConceptoCuenta,
                Nombre,
                Valor.ToString().Replace(",", SqlServer.SigFloatSql),
                Estado ? 1 : 0,
                Cantidad.ToString().Replace(",", SqlServer.SigFloatSql),
                Iva.ToString().Replace(",", SqlServer.SigFloatSql));
        }
        #endregion

        #region Update Data Method
        public string Update()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate());
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;

        }
        private string GetSQLUpdate()
        {
            return String.Format(@"UPDATE DETALLE_FACT_BOLETO SET Id_factura_boleto = {0}, Id_Concepto_Cuenta = {1}, Nombre = '{2}',
                Valor = {3}, Estado = {4}, Cantidad = {5}, Iva = {6} WHERE Id_detalle_fact_boleto = {7}", IdFacturaBoleto, IdConceptoCuenta,
                Nombre, Valor, Estado ? 1 : 0, Cantidad, Iva, IdDetalleFactBoleto);
        }
        #endregion

        #region Delete Data Method
        public string Delete(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        private string GetSQLDelete(long id)
        {
            return String.Format("DELETE FROM DETALLE_FACT_BOLETO WHERE Id_factura_boleto = {0}", id);
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
        ~DetalleFactBoleto()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
