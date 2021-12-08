using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Service
{
    public class SaldoFactPrepago : SqlConnecion, IDisposable
    {
        public long IdDetalleTicket { get; set; }
        public long IdCaja { get; set; }
        public long IdFacturaTicket { get; set; }
        public long IdTarifa { get; set; }
        public decimal Saldo { get; set; }

        public static List<SaldoFactPrepago> GetData(long IdCuentaPrepago)
        {
            List<SaldoFactPrepago> saldosFact = new List<SaldoFactPrepago>();
            string sql = String.Format(@"SELECT sf.Id_Factura_Ticket, f.Id_Caja, 
                Id_Detalle_Fact_Ticket = (SELECT TOP 1 Id_Detalle_Fact_Ticket FROM DETALLE_FACT_TICKET WHERE Id_Factura_Ticket = f.Id_Factura_Ticket),
                Id_Tarifa = (SELECT TOP 1 Id_tipo_tarifa FROM DETALLE_FACT_TICKET WHERE Id_Factura_Ticket = f.Id_Factura_Ticket), sf.Saldo
                FROM SALDO_FACT_PREPAGO sf INNER JOIN FACTURA_TICKET f ON sf.Id_Factura_Ticket = f.id_factura_ticket
                WHERE Id_CuentaPrepago = {0} AND sf.Saldo > 0 ORDER BY F.fecha_hora ASC", IdCuentaPrepago);

            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        saldosFact.Add(new SaldoFactPrepago
                        {
                            IdFacturaTicket = Convert.ToInt64(row["Id_Factura_Ticket"]),
                            IdCaja = Convert.ToInt64(row["Id_Caja"]),
                            IdDetalleTicket = Convert.ToInt64(row["Id_Detalle_Fact_Ticket"]),
                            IdTarifa = Convert.ToInt64(row["Id_Tarifa"]),
                            Saldo = Convert.ToDecimal(row["Saldo"])
                        });
                    }
                }

                return saldosFact;
            }
        }

        public static string InsertProceso(string operadora, string jsonPasajeros, string jsonTransacciones, string jsonSaldosFactura)
        {
            using (SqlConnection connection = new SqlConnection(SqlServer.CadenaConexion))
            {
                using (SqlCommand command = new SqlCommand("GuardarWebService", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameter = null;

                    parameter = command.Parameters.Add("@operadora", SqlDbType.VarChar);
                    parameter.Value = operadora;
                    parameter = command.Parameters.Add("@pasajeros", SqlDbType.NVarChar);
                    parameter.Value = jsonPasajeros;
                    parameter = command.Parameters.Add("@transacciones", SqlDbType.VarChar);
                    parameter.Value = jsonTransacciones;
                    parameter = command.Parameters.Add("@saldosFactura", SqlDbType.VarChar);
                    parameter.Value = jsonSaldosFactura;

                    parameter = command.Parameters.Add("@IdsTransacciones", SqlDbType.VarChar, 1000);
                    parameter.Direction = ParameterDirection.Output;

                    parameter = command.Parameters.Add("return_value", SqlDbType.VarChar);
                    parameter.Direction = ParameterDirection.ReturnValue;

                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    command.ExecuteReader();

                    string result = command.Parameters["@IdsTransacciones"].Value.ToString();

                    return result;
                }
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
        ~SaldoFactPrepago()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
