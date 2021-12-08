using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CapaDatos.Recaudacion
{
    public class DetalleFactRecauda : SqlConnecion, IDisposable
    {
        public long IdDetalleFactRecauda { get; set; }
        public long IdFacturaRecauda { get; set; }
        public long IdConceptoCuenta { get; set; }
        public long IdCooperativa { get; set; }
        public long IdUnidadTransporte { get; set; }
        public string CodigoConceptoCuenta { get; set; }
        public string IdMulta { get; set; }
        public long IdLocal { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }
        public bool Estado { get; set; }
        public decimal Iva { get; set; }
        public decimal IvaValor { get; set; }
        //Campos no incluídos en las tablas
        public decimal ValorTotal { get; set; }
        public int TipoFacturaVenta { get; set; }
        public long IdHorarioPautaje { get; set; }
        public long IdClienteFinal { get; set; }
        public DateTime? FechaDesde { get; set; }
        public string FechaDesdeSQL
        {
            get
            {
                if (FechaDesde != null)
                    return FechaDesde == null ? "" : Convert.ToDateTime(FechaDesde.ToString()).ToString(SqlServer.FormatofechaHora);
                return null;
            }
        }
        public DateTime? FechaHasta { get; set; }
        public string FechaHastaSQL
        {
            get
            {
                if (FechaHasta != null)
                    return FechaHasta == null ? "" : Convert.ToDateTime(FechaHasta.ToString()).ToString(SqlServer.FormatofechaHora);
                return null;
            }
        }
        public string Observaciones { get; set; }
        public int idHorarioPautaje { get; set; }

        // Venta de Tarjetas de Acceso
        public long IdTarjeta { get; set; }
        public string TarjetaNueva { get; set; }
        public int EstadoTarjeta { get; set; }
        public long IdTag { get; set; }
        public string Tag { get; set; }
        public int EstadoTag { get; set; }
        public string TagNuevo { get; set; }
        public int PasoAnden { get; set; }
        public string IDsTipoPago { get; set; }

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
        ~DetalleFactRecauda()
        {
            this.Dispose(false);
        }
        #endregion
    }

    public enum TipoFacturaVenta
    {
        Ninguna = 0,
        Arriendo = 1,
        PrepagoUTT = 2,
        Publicidad = 3,
        Otros = 4,
        Multas = 5,
        Tarjetas = 6,
        Tags = 7,
        RutaEnlace = 8
    }
}
