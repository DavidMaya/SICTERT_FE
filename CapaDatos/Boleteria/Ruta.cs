using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos.Boleteria
{
    public class Ruta : SqlConnecion, IDisposable
    {
        public long IdFrecuencia { get; set; }
        public string CiudadOrigen { get; set; }
        public string CiudadDestino { get; set; }
        public string NombreFrecuencia 
        {
            get
            {
                return String.Format("{0} - {1}",
                    CiudadOrigen,
                    CiudadDestino);
            }
        }

        public string FrecuenciaDetalle
        {
            get
            {
                return String.Format("{0} - {1} - {2}",
                    CiudadOrigen,
                    CiudadDestino,
                    Frecuencia);
            }
        }
        public string Frecuencia { get; set; }
        public List<string> Dias { get; set; }

        public Ruta() { }

        public static List<Ruta> List (long idCooperativa, DateTime fechaSeleccionada)
        {
            List<Ruta> rutas = new List<Ruta>();
            string sql = String.Format(@"SELECT f.Id_Frecuencia, f.Dias, LEFT(CONVERT(VARCHAR, f.Hora_Salida, 108), 5) AS Frecuencia, cd.Nombre AS Ciudad_Origen, 
                ci.Nombre AS Ciudad_Destino
                FROM FRECUENCIA f 
                INNER JOIN COOPERATIVA co ON co.Id_Cooperativa = f.Id_Cooperativa 
                INNER JOIN CIUDAD cd ON cd.Id_Ciudad = f.Id_Ciudad_Origen
                INNER JOIN CIUDAD ci ON ci.Id_Ciudad = f.Id_Ciudad_Destino
                WHERE f.Activo = 1 AND f.Id_Tipo_Frecuencia = 1 AND co.Id_Cooperativa = {0}
                AND CONVERT(DATETIME, '{1} ' + CONVERT(VARCHAR, f.Hora_Salida, 108)) >= GETDATE() AND f.Activo = 1
                ORDER BY f.Hora_Salida, co.Nombre", idCooperativa, fechaSeleccionada.ToString("dd-MM-yyyy"));

            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        rutas.Add(new Ruta
                        {
                            IdFrecuencia = Convert.ToInt64(row["Id_Frecuencia"]),
                            Frecuencia = Convert.ToString(row["Frecuencia"]),
                            CiudadOrigen = Convert.ToString(row["Ciudad_Origen"]),
                            CiudadDestino = Convert.ToString(row["Ciudad_Destino"]),
                            Dias = GetArrayString(Convert.ToString(row["Dias"]))
                        });
                    }
                }
            }

            return rutas;
        }

        public static List<Ruta> GetDataObject(DateTime fecha, long idCooperativa)
        {
            //Filtrar por día de la semana que corresponda}
            int dia = (int)Convert.ToDateTime(fecha).DayOfWeek;
            return List(idCooperativa, fecha).Where(r => r.Dias[dia] == "1").ToList();
        }
        private static List<string> GetArrayString(string texto)
        {
            return texto.Select(c => c.ToString()).ToList();
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
        ~Ruta()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
