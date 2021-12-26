using System;

namespace FacturaElectronica.Tools
{
    public class Resultado
    {
        public Boolean Estado { get; set; }
        public String Mensaje { get; set; }
        public T Datos<T>()
        {
            Object r = null;
            if ((Estado) && (Mensaje.Length > 0))
            {
                try
                {
                    r = XmlTools.Deserialize<T>(this.Mensaje);
                }
                catch (Exception ex)
                {
                    this.Estado = false;
                    this.Mensaje = this.Mensaje + "\n" + ex.Message;
                }
            }
            return (T)r;

        }
    }
}
