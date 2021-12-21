using System;
using System.Configuration;

namespace FacturaElectronica.Tools
{
    public static class Calcs
    {
        public static string GetClaveAcceso(
            string fechaEmision,
            string tipoComprobante,
            string ruc,
            string tipoAmbiente,
            string serie,
            string numeroComprobante,
            string tipoEmision)
        {
            string codigoNumerico = ConfigurationManager.AppSettings["codigoNumerico"];
            fechaEmision = fechaEmision.Replace("/", "");
            string clave = fechaEmision + tipoComprobante +
                ruc + tipoAmbiente + serie + numeroComprobante +
                codigoNumerico + tipoEmision;
            int verificador;

            int count = 2, suma = 0;
            for (int i = clave.Length; i > 0; i--)
            {
                count = count > 7 ? 2 : count;
                suma += (int)Char.GetNumericValue(clave[i - 1]) * count;
                count++;
            }

            if (suma == 0 || suma == 1)
                verificador = 0;
            else
                verificador = (11 - suma % 11 == 11) ? 0 : (11 - suma % 11);

            if (verificador == 10)
                verificador = 1;

            return clave + verificador.ToString();
        }
    }
}
