using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Service
{
    public class TipoTransaccionCuentas : SqlConnecion, IDisposable
    {
        public long IdTipoTransaccion { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }

        public TipoTransaccionCuentas() { }

        public TipoTransaccionCuentas(long id)
        {
            string sql = String.Format(@"SELECT Id_TipoTransaccion, Nombre, Codigo 
                FROM TIPO_TRANSACCION_CUENTAS
                WHERE Id_TipoTransaccion = {0}", id);

            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                if (table.Rows.Count > 0)
                {
                    IdTipoTransaccion = Convert.ToInt64(table.Rows[0]["Id_TipoTransaccion"]);
                    Nombre = Convert.ToString(table.Rows[0]["Nombre"]);
                    Codigo = Convert.ToString(table.Rows[0]["Codigo"]);
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
        ~TipoTransaccionCuentas()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
