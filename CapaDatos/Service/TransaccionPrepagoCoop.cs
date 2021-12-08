using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CapaDatos.Service
{
    public class TransaccionPrepagoCoop : SqlConnecion, IDisposable
    {
        public long IdTransaccionPrepago { get; set; }
        public long IdCuentaPrepago { get; set; }
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; }
        public long IdTipoTransaccion { get; set; }
        public long IdTipoTarifa { get; set; }
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }
        public int ConsecutivoTasa { get; set; }

        public TransaccionPrepagoCoop() { }

        public static int GuardarDatos(
            Saldo saldo, long IdTipoTarifa, int Cantidad, decimal Valor, SaldoFactPrepago saldoFactura)
        {
            using (SqlConnection connection = new SqlConnection(SqlServer.CadenaConexion))
            {
                using (SqlCommand command = new SqlCommand("GuardarTransaccionPrepagoCoop", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameter = null;

                    parameter = command.Parameters.Add("@IdCuentaPrepago", SqlDbType.BigInt);
                    parameter.Value = saldo.IdCuentaPrepago;
                    parameter = command.Parameters.Add("@Descripcion", SqlDbType.NVarChar);
                    parameter.Value = "";
                    parameter = command.Parameters.Add("@IdTipoTransaccion", SqlDbType.BigInt);
                    parameter.Value = 2;
                    parameter = command.Parameters.Add("@IdTipoTarifa", SqlDbType.BigInt);
                    parameter.Value = IdTipoTarifa;
                    parameter = command.Parameters.Add("@Cantidad", SqlDbType.Int);
                    parameter.Value = Cantidad;
                    parameter = command.Parameters.Add("@Valor", SqlDbType.Decimal);
                    parameter.Value = Valor;
                    parameter = command.Parameters.Add("@Saldo", SqlDbType.Decimal);
                    parameter.Value = saldo.Valor - (Cantidad * Valor);
                    parameter = command.Parameters.Add("@IdFacturaTicket", SqlDbType.BigInt);
                    parameter.Value = saldoFactura.IdDetalleTicket;

                    parameter = command.Parameters.Add("return_value", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;

                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    command.ExecuteReader();

                    return Int32.Parse(command.Parameters["return_value"].Value.ToString());

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
        ~TransaccionPrepagoCoop()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
