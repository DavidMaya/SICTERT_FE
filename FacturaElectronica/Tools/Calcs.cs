using System;

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
            string codigoNumerico = "12345678";
            fechaEmision = fechaEmision.Replace("/", "");

            string clave = fechaEmision + tipoComprobante +
                ruc + tipoAmbiente + serie + numeroComprobante +
                codigoNumerico + tipoEmision;

            int count = 2, total = 0;
            for (int i = clave.Length; i > 0; i--)
            {
                count = count > 7 ? 2 : count;
                total += (int)Char.GetNumericValue(clave[i - 1]) * count;
                count++;
            }

            int verificacion = 11 - (total % 11);
            return clave + verificacion.ToString();
        }
    }
}
