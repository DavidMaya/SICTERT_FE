using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fechaEmision = "14/12/2021";
            string tipoComprobante = "01";
            string ruc = "1891760171001";
            string tipoAmbiente = "2"; //Producción
            string serie = "001001";
            string numeroComprobante = "000000101";
            string tipoEmision = "1";

            string res = GetClaveAcceso(fechaEmision, tipoComprobante, ruc, tipoAmbiente, serie, numeroComprobante, tipoEmision);

            Console.ReadKey();
        }

        private static string GetClaveAcceso(
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

            int mod = total % 11;
            if (mod == 0) 
                return clave + mod.ToString();
            else
            {
                int verificacion = 11 - mod;
                return clave + verificacion.ToString();
            }
        }

        private void TestXML()
        {
            string path = "C:\\Users\\david\\OneDrive\\Desktop\\XMLBase\\Mega.xml";
            string xmlString = System.IO.File.ReadAllText(path);


            var factura = utils.XmlTools.Deserialize<factura>(xmlString);

            factura factura1 = new factura();
            factura1.version = "1.1.0";
            factura1.id = "comprobante";
            factura1.infoTributaria = new facturaInfoTributaria()
            {
                ambiente = 2,
                tipoEmision = 1,
                razonSocial = "CORPORACION FAVORITA C.A.",
                ruc = "1790016919001",
                codDoc = "01",
                estab = "085",
                ptoEmi = "107",
                secuencial = "000489209",
                dirMatriz = "AV. GENERAL ENRIQUEZ VIA COTOGCHOA"
            };

            factura1.infoFactura = new facturaInfoFactura()
            {
                fechaEmision = "04/12/2021",
                dirEstablecimiento = "AV. VICTOR HUGO S/N Y ATAHUALPA",
                contribuyenteEspecial = 5368,
                obligadoContabilidad = "SI",
                tipoIdentificacionComprador = "05",
                razonSocialComprador = "MAYANCELA YANEZ DAVID JESUS",
                identificacionComprador = "1804279345",
                totalSinImpuestos = 37.65f,
                totalDescuento = 6.30f,
                totalConImpuestos = new List<facturaInfoFacturaTotalImpuesto>(),
                propina = 0,
                importeTotal = 39.73f,
                moneda = "DOLAR"
            };

            factura1.infoFactura.totalConImpuestos.Add(new facturaInfoFacturaTotalImpuesto()
            {
                codigo = "2",
                codigoPorcentaje = "0",
                descuentoAdicional = 0,
                baseImponible = 20.30f,
                valor = 0,
            });

            factura1.detalles.Add(new facturaDetalle()
            {
                codigoPrincipal = "786100491193",
                codigoAuxiliar = "786100491193",
                descripcion = "SALTICAS GALLETAS INTEGRALES",
                cantidad = 5.0000f,
                precioUnitario = "0.2768",
                descuento = 0.14f,
                precioTotalSinImpuesto = 1.24f,
                impuestos = new facturaDetalleImpuestos()
                {
                    impuesto = new facturaDetalleImpuestosImpuesto()
                    {
                        codigo = "2",
                        codigoPorcentaje = "2",
                        tarifa = 12f,
                        baseImponible = 1.24f,
                        valor = 0.15f
                    }
                }
            });

            factura1.infoAdicional.Add(new facturaCampoAdicional()
            {
                nombre = "DEDUCIBLE ALIMENTACION",
                Value = "24.76"
            });



            string xml = utils.XmlTools.Serialize(factura1);

        }
    }
}
