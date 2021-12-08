using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CapaDatos.Boleteria
{
    public class PagoBoleto : SqlConnecion, IDisposable
    {
        public long IdPagoBoleto { get; set; }
        public long IdTipoPago { get; set; }
        public long IdFacturaBoleto { get; set; }
        public decimal Valor { get; set; }
        public string Detalle { get; set; }
        public DateTime FechaHora { get; set; }
        public int Estado { get; set; }

        public PagoBoleto(){ }

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
        ~PagoBoleto()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
