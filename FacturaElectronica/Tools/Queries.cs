using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronica.Clases
{
    public static class Queries
    {
        public static string SelectFacturaGeneric(string id, string table)
        {
            return "SELECT " +
                // infoTributaria
                $"f.{id} AS idFactura, " +
                "f.Id_TipoAmbiente AS ambiente, " +
                "UPPER(f.RazonSocial_Emisor) razonSocial, " +
                "f.RUC_Emisor ruc, " +
                "f.Serie AS estab, " +
                "f.CodEstablecimiento_Emisor ptoEmi, " +
                "f.Numero secuencial, " +
                "UPPER(f.DirMatriz_Emisor) dirMatriz," +
                // infoFactura
                "FORMAT(f.Fecha_Hora, 'dd/mm/yyyy') fechaEmision, " +
                "UPPER(f.DirEstablecimiento_Emisor) dirEstablecimiento, " +
                "f.ResolContribEsp_Emisor AS contribuyenteEspecial, " +
                "IIF(f.ObligadoContab_Emisor = 1, 'SI', 'NO') AS obligadoContabilidad, " +
                "f.Razon_Social AS razonSocialComprador, " +
                "f.CI_Ruc AS identificacionComprador, " +
                "f.Direccion AS direccionComprador, " +
                "CONVERT(DECIMAL(10, 2), (f.Valor + f.ValorBaseIVA)) AS totalSinImpuestos, " +
                "CONVERT(DECIMAL(10,2), f.Valor_Total) AS importeTotal, " +
                // infoAdicional
                "cf.Direccion, cf.Telefono, cf.Email, " +
                "c.CertificadoP12 as certificado, c.ClaveCertificadoP12 as clave " +
                // table
                $"FROM {table} f " +
                "INNER JOIN CLIENTE_FINAL cf ON cf.Id_Cliente_Final = f.Codigo_TipoIdCliente " +
                "INNER JOIN CAJA C ON f.Id_Caja = C.Id_Caja " +
                // where
                $"WHERE f.{id} = 1214303";
        }

        public static string SelectImpuestosGeneric(string id, string table, long idFactura)
        {
            return "SELECT " +
                "CASE WHEN dfr.Iva = 0 THEN 0 WHEN dfr.Iva = 12 THEN 2 " +
                "WHEN dfr.Iva = 14 THEN 3 END AS codigoPorcentaje, " +
                "CONVERT(DECIMAL(10, 2), SUM(dfr.Valor * dfr.Cantidad)) AS baseImponible, " +
                "CONVERT(DECIMAL(10, 2), SUM(dfr.Iva)) AS valor " +
                $"FROM {table} dfr " +
                $"WHERE dfr.{id} = {idFactura} " +
                "GROUP BY dfr.Iva";
        }

        public static string SelectDetalleGeneric(string id, string table, long idFactura)
        {
            string field = table != "FACTURA_CONCEPTO" ? 
                ", dfr.Iva_Valor AS valor " : ", CONVERT(DECIMAL(10, 2), dfr.Valor * dfr.Cantidad) AS valor "; //Aquí puede haber un cambio en el Iva
            return "SELECT " +
                "dfr.Id_Concepto_Cuenta AS codigoPrincipal, " +
                "dfr.Nombre AS descripcion, " +
                "dfr.Cantidad AS cantidad, " +
                "CONVERT(DECIMAL(10, 2), dfr.Valor) AS precioUnitario, " +
                "CONVERT(DECIMAL(10, 2), dfr.Valor * dfr.Cantidad) AS precioTotalSinImpuesto, " +
                "CASE WHEN dfr.Iva = 0 THEN 0 WHEN dfr.Iva = 12 THEN 2 " +
                "WHEN dfr.Iva = 14 THEN 3 END AS codigoPorcentaje, " +
                "CONVERT(DECIMAL(10, 2), dfr.Iva) AS tarifa " +
                field + 
                $"FROM {table} dfr " +
                $"WHERE dfr.{id} = {idFactura}";
        }

        internal static string UpdateEstadoFactura(long id, string table, string state)
        {
            //return $"UPDATE {table} SET Id_EstadoFE = '{state}' WHERE Id_Factura = {id}";
            return $"UPDATE {table} SET Id_EstadoFE = (SELECT Id_EstadoFE FROM ESTADO_FACTELECTRONICA WHERE Codigo = '{state}') WHERE Id_Factura = {id}";
        }
    }
}
