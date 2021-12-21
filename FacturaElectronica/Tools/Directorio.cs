using FacturaElectronica.Documento;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronica.Tools
{
    public class Directorio : IDisposable
    {
        private string path;
        private string folder;

        public Directorio(string path, string folder)
        {
            this.path = path;
            this.folder = folder;
        }

        public Resultado Path(EstadoDocumento estado)
        {
            Resultado resultado = new Resultado();
            try
            {
                if (!Directory.Exists(path + "\\" + folder + "\\" + estado.ToString() + "\\"))
                    Directory.CreateDirectory(path + "\\" + folder + "\\" + estado.ToString()+ "\\");
                resultado.Estado = true;
                resultado.Mensaje = path + "\\" + folder + "\\" + estado.ToString() + "\\";
                return resultado;
            }
            catch (Exception ex)
            {
                resultado.Estado = false;
                resultado.Mensaje = ex.Message;
                return resultado;
            }
        }

        public Resultado Path(string estado)
        {
            Resultado resultado = new Resultado();
            try
            {
                if (!Directory.Exists(path + estado.ToString() + "\\"))
                    Directory.CreateDirectory(path + estado.ToString() + "\\");
                resultado.Estado = true;
                resultado.Mensaje = path + estado.ToString() + "\\";
                return resultado;
            }
            catch (Exception ex)
            {
                resultado.Estado = false;
                resultado.Mensaje = ex.Message;
                return resultado;
            }
        }

        public Resultado Documento(TipoDocumento doc, EstadoDocumento estado)
        {
            Resultado resultado = new Resultado();
            try
            {
                if (!Directory.Exists(path + "\\" + folder + "\\" + estado.ToString() + "\\" + doc.ToString()))
                {
                    Directory.CreateDirectory(path + "\\" + folder + "\\" + estado.ToString() + "\\" + doc.ToString());
                }
                resultado.Estado = true;
                resultado.Mensaje = path + "\\" + folder + "\\" + estado.ToString() + "\\" + doc.ToString();
            }
            catch (Exception ex)
            {
                resultado.Estado = false;
                resultado.Mensaje = ex.Message;
            }
            return resultado;

        }

        #region Método Disposed
        private Boolean disposed;
        public void Dispose()
        {
            this.Dispose(true);
            // GC.SupressFinalize quita de la cola de finalizaci??n al objeto.
            GC.SuppressFinalize(this);
        }

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

        ~Directorio()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
