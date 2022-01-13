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
                "f.ClaveAccesoFactElectronica AS claveAcceso, " +
                "(SELECT Codigo FROM TIPO_COMPROBANTE tc WHERE tc.Nombre = 'FACTURA') AS codDoc, " +
                "f.Serie AS estab, " +
                "f.CodEstablecimiento_Emisor AS ptoEmi, " +
                "f.Numero secuencial, " +
                "UPPER(f.DirMatriz_Emisor) AS dirMatriz," +
                "(SELECT trt.Leyenda FROM TIPO_REGIMEN_TRIBUTARIO trt WHERE trt.Id_RegimenTributario = c.Id_RegimenTributario) AS regimenMicroempresas, " +
                "c.ResolAgenteRet AS agenteRetencion, " +
                // infoFactura
                "CONVERT(VARCHAR, f.Fecha_Hora, 103) AS fechaEmision, " +
                "UPPER(f.DirEstablecimiento_Emisor) AS dirEstablecimiento, " +
                "f.ResolContribEsp_Emisor AS contribuyenteEspecial, " +
                "IIF(f.ObligadoContab_Emisor = 1, 'SI', 'NO') AS obligadoContabilidad, " +
                "f.Codigo_TipoIdCliente AS tipoIdentificacionComprador, " +
                "f.Razon_Social AS razonSocialComprador, " +
                "f.CI_Ruc AS identificacionComprador, " +
                "f.Direccion AS direccionComprador, " +
                "f.Telefono, " +
                "f.Email, " +
                "CONVERT(DECIMAL(19, 4), (f.Valor + f.ValorBaseIVA)) AS totalSinImpuestos, " +
                "CONVERT(DECIMAL(19, 4), f.Valor_Total) AS importeTotal, " +
                // Certificado y datos adicionales
                "c.CertificadoP12 as certificado, c.ClaveCertificadoP12 as clave, " +
                "(SELECT tp.Codigo_FormaPago_FE FROM TIPO_PAGO tp WHERE tp.Id_Tipo_Pago = p.Id_Tipo_Pago) AS formaPago " +
                // table
                $"FROM {table} f " +
                "INNER JOIN PAGO p ON f.Id_Factura = p.Id_Factura " +
                "INNER JOIN CAJA C ON f.Id_Caja = C.Id_Caja " +
                // Filtro de las facturas
                "WHERE f.Id_EstadoFE IS NULL AND f.Id_TipoFactura = (SELECT tf.Id_TipoFactura FROM TIPO_FACTURA tf WHERE tf.Codigo = 'FE')";
        }

        public static string SelectImpuestosGeneric(string id, string table, long idFactura)
        {
            // totalConImpuestos
            string field = table == "DETALLE_FACT_RECAUDA" ? "Iva_Valor" : "Iva";
            return "SELECT " +
                $"det.CodigoTarifa_IVA AS codigoPorcentaje, " +
                "CONVERT(DECIMAL(19, 4), SUM(det.Valor * det.Cantidad)) AS baseImponible, " +
                "tti.PorcIVA AS tarifa, " +
                $"CONVERT(DECIMAL(19, 4), SUM(det.{field})) AS valor " +
                $"FROM {table} det " +
                "INNER JOIN TIPO_TARIFA_IVA tti ON det.CodigoTarifa_IVA = tti.CodigoTarifa_IVA " +
                $"WHERE CONVERT(DECIMAL(19, 4), det.{id}) = {idFactura} " +
                $"GROUP BY det.CodigoTarifa_IVA, tti.PorcIVA";
        }

        public static string SelectDetalleGeneric(string id, string table, long idFactura)
        {
            // detalles
            string field = table == "DETALLE_FACT_RECAUDA" ? "Iva_Valor" : "Iva";
            return "SELECT " +
                "det.Id_Concepto_Cuenta AS codigoPrincipal, " +
                "det.Nombre AS descripcion, " +
                "det.Cantidad AS cantidad, " +
                "CONVERT(DECIMAL(19, 4), det.Valor) AS precioUnitario, " +
                $"det.CodigoTarifa_IVA AS codigoPorcentaje, " +
                $"tti.PorcIVA AS tarifa, " +
                "CONVERT(DECIMAL(19, 4), det.Valor * det.Cantidad) AS precioTotalSinImpuesto, " +
                $"det.{field} AS valor " + 
                $"FROM {table} det " +
                "INNER JOIN TIPO_TARIFA_IVA tti ON det.CodigoTarifa_IVA = tti.CodigoTarifa_IVA " +
                $"WHERE det.{id} = {idFactura}";
        }

        public static string SelectCodigoIva()
        {
            // Código IVA que va en todos los detalles: Tabla 16
            return "SELECT Codigo_ImpuestoIVA FROM CODIGO_IMPUESTO WHERE Impuesto = 'IVA'";
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
