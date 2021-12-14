using FacturaElectronica.Documento;
using System;

namespace FacturaElectronica.SRI
{
    public class ServicioSRI
    {
        public AmbienteActivo Ambiente { get; set; }
        public String Recepción { get; set; }
        public String Autorización { get; set; }
    }
}
