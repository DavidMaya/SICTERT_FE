namespace FacturaElectronica.Tools
{
    public static class Queries
    {
        public static string SelectFacturaGeneric(string id, string table)
        {
            return "SELECT " +
                // infoTributaria
                $"f.{id} AS idFactura, " +
                "f.Id_TipoAmbiente AS ambiente, " +
                "UPPER(f.RazonSocial_Emisor) AS razonSocial, " +
                "f.RUC_Emisor AS ruc, " +
                "f.Serie AS estab, " +
                "f.CodEstablecimiento_Emisor AS ptoEmi, " +
                "f.Numero secuencial, " +
                "UPPER(f.DirMatriz_Emisor) AS dirMatriz," +
                // infoFactura
                "FORMAT(f.Fecha_Hora, 'dd/mm/yyyy') AS fechaEmision, " +
                "UPPER(f.DirEstablecimiento_Emisor) AS dirEstablecimiento, " +
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
                //$"WHERE f.{id} = 1214772"; // Test
                "WHERE f.Id_EstadoFE IS NULL AND f.Id_TipoFactura = (SELECT tf.Id_TipoFactura FROM TIPO_FACTURA tf WHERE tf.Codigo = 'FE')";
        }

        public static string SelectImpuestosGeneric(string id, string table, long idFactura)
        {
            string field = table == "DETALLE_FACT_RECAUDA" ? "Iva_Valor" : "Iva";
            
            return "SELECT " +
                $"IIF(det.{field} >= 0, 0, 2) AS codigoPorcentaje, " +
                "CONVERT(DECIMAL(10, 2), SUM(det.Valor * det.Cantidad)) AS baseImponible, " +
                $"CONVERT(DECIMAL(10, 2), SUM(det.{field})) AS valor " +
                $"FROM {table} det " +
                $"WHERE det.{id} = {idFactura} " +
                $"GROUP BY IIF(det.{field} >= 0, 0, 2)";
        }

        public static string SelectDetalleGeneric(string id, string table, long idFactura)
        {
            string field = table == "DETALLE_FACT_RECAUDA" ? "Iva_Valor" : "Iva";

            return "SELECT " +
                "det.Id_Concepto_Cuenta AS codigoPrincipal, " +
                "det.Nombre AS descripcion, " +
                "det.Cantidad AS cantidad, " +
                "CONVERT(DECIMAL(10, 2), det.Valor) AS precioUnitario, " +
                $"IIF(det.{field} >= 0, 0, 2) AS codigoPorcentaje, " +
                $"IIF(det.{field} >= 0, 12, 2) AS tarifa, " +
                "CONVERT(DECIMAL(10, 2), det.Valor * det.Cantidad) AS precioTotalSinImpuesto, " +
                $"det.{field} AS valor " + 
                $"FROM {table} det " +
                $"WHERE det.{id} = {idFactura}";
        }

        public static string UpdateEstadoFactura()
        {
            return "UPDATE {0} SET Id_EstadoFE = (SELECT Id_EstadoFE FROM ESTADO_FACTELECTRONICA WHERE Codigo = '{1}') WHERE Id_Factura = {2}";
        }

        public static string UpdateClaveAcceso()
        {
            return "UPDATE {0} SET ClaveAccesoFactElectronica = '{1}' WHERE Id_Factura = {2}";
        }
    }
}
