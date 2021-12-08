using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CapaDatos.Boleteria
{
    public class TarifaViaje : SqlConnecion, IDisposable
    {
        public long IdConceptoCuenta { get; set; }
        public long IdTarifaViaje { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public bool Iva { get; set; }
        public string NombreCiudad { get; set; }
        public decimal ImpIva { get; set; }
        public bool Diferenciada { get; set; }

        public static List<TarifaViaje> GetAll(long IdCiudadOrigen, long IdCiudadDestino)
        {
            List<TarifaViaje> result = new List<TarifaViaje>();

            string sql = String.Format(@"SELECT cc.Id_Concepto_Cuenta, tv.Id_TarifaViaje, cc.Nombre, cc.Valor, cc.Iva, c.Nombre AS NombreCiudad, 
			ImpIva = (SELECT Valor FROM CONFIGURACION_GLOBAL WHERE Configuracion = 'IVA'), ttv.Diferenciada
            FROM TARIFA_VIAJE tv
            INNER JOIN CONCEPTO_CUENTA cc
            ON cc.Id_Concepto_Cuenta = tv.Id_Concepto_Cuenta
            INNER JOIN CIUDAD c
            ON tv.Id_CiudadDestino = c.Id_Ciudad
            INNER JOIN TIPO_TARIFA_VIAJE ttv
            ON ttv.Id_TipoTarifaViaje = tv.Id_TipoTarifaViaje
            WHERE Id_CiudadOrigen = {0} AND Id_CiudadDestino = {1} AND cc.Estado = 1", 
            IdCiudadOrigen, IdCiudadDestino);

            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                if (table.Rows.Count > 0 && table.Rows.Count == 2)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        result.Add(new TarifaViaje()
                        {
                            IdConceptoCuenta = Convert.ToInt64(row["Id_Concepto_Cuenta"]),
                            IdTarifaViaje = Convert.ToInt64(row["Id_TarifaViaje"]),
                            Nombre = Convert.ToString(row["Nombre"]),
                            Valor = Convert.ToDecimal(row["Valor"]),
                            Iva = Convert.ToBoolean(row["Iva"]),
                            NombreCiudad = Convert.ToString(row["NombreCiudad"]),
                            ImpIva = Convert.ToDecimal(row["ImpIva"]),
                            Diferenciada = Convert.ToBoolean(row["Diferenciada"])
                        });
                    }

                    return result;
                }
                else
                    return null;
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
        ~TarifaViaje()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
