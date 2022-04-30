namespace FacturaElectronica.Tools
{
    public static class Queries
    {
        public static string SelectFacturaGeneric(string id, string table, string table_pago) => "SELECT " +
                // infoTributaria
                $"f.{id} AS idFactura, " +
                "f.Id_TipoAmbiente AS ambiente, " +
                "UPPER(f.RazonSocial_Emisor) AS razonSocial, " +
                "f.NombreComercial_Emisor AS nombreComercial, " +
                "f.RUC_Emisor AS ruc, " +
                "f.ClaveAccesoFactElectronica AS claveAcceso, " +
                "(SELECT Codigo FROM TIPO_COMPROBANTE tc WHERE tc.Nombre = 'FACTURA') AS codDoc, " +
                "f.Serie AS estab, " +
                "f.CodEstablecimiento_Emisor AS ptoEmi, " +
                "f.Numero secuencial, " +
                "UPPER(f.DirMatriz_Emisor) AS dirMatriz," +
                "(SELECT trt.Codigo FROM TIPO_REGIMEN_TRIBUTARIO trt WHERE trt.Id_RegimenTributario = f.Id_RegimenTributario_Emisor) AS regimenCodigo, " +
                "(SELECT trt.Leyenda FROM TIPO_REGIMEN_TRIBUTARIO trt WHERE trt.Id_RegimenTributario = f.Id_RegimenTributario_Emisor) AS regimenMicroempresas, " +
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
                "CONVERT(DECIMAL(19, 2), (f.Valor + f.ValorBaseIVA)) AS totalSinImpuestos, " +
                "CONVERT(DECIMAL(19, 2), f.Valor_Total) AS importeTotal, " +
                // Certificado y datos adicionales
                "c.CertificadoP12 as certificado, c.ClaveCertificadoP12 as clave, " +
                "c.LogoRIDE AS LogoEmpresa, " +
                "(SELECT tp.Codigo_FormaPago_FE FROM TIPO_PAGO tp WHERE tp.Id_Tipo_Pago = p.Id_Tipo_Pago) AS formaPago " +
                // table
                $"FROM {table} f " +
                $"INNER JOIN {table_pago} p ON f.{id} = p.{id} " +
                "INNER JOIN CAJA c ON f.Id_Caja = c.Id_Caja " +
                // Filtro de las facturas
                "WHERE f.Id_EstadoFE IS NULL AND f.Id_TipoFactura = (SELECT tf.Id_TipoFactura FROM TIPO_FACTURA tf WHERE tf.Codigo = 'FE') " +
                "ORDER BY f.Fecha_Hora";

        public static string SelectImpuestosGeneric(string id, string table, long idFactura)
        {
            // totalConImpuestos
            string field = table == "DETALLE_FACT_RECAUDA" ? "Iva_Valor" : "Iva";
            return "SELECT " +
                $"det.CodigoTarifa_IVA AS codigoPorcentaje, " +
                "CONVERT(DECIMAL(19, 2), SUM(det.Valor * det.Cantidad)) AS baseImponible, " +
                "tti.PorcIVA AS tarifa, " +
                $"CONVERT(DECIMAL(19, 2), SUM(det.{field} * det.Cantidad)) AS valor " +
                $"FROM {table} det " +
                "INNER JOIN TIPO_TARIFA_IVA tti ON det.CodigoTarifa_IVA = tti.CodigoTarifa_IVA " +
                $"WHERE det.{id} = {idFactura} " +
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
                "CONVERT(DECIMAL(19, 2), det.Valor * det.Cantidad) AS precioTotalSinImpuesto, " +
                $"CONVERT(DECIMAL(19, 2), det.{field} * det.Cantidad) AS valor " + 
                $"FROM {table} det " +
                "INNER JOIN TIPO_TARIFA_IVA tti ON det.CodigoTarifa_IVA = tti.CodigoTarifa_IVA " +
                $"WHERE det.{id} = {idFactura}";
        }

        public static string SelectCodigoIva() =>
            "SELECT Codigo_ImpuestoIVA FROM CODIGO_IMPUESTO WHERE Impuesto = 'IVA'";

        public static string UpdateEstadoFactura() => 
            "UPDATE {0} SET Id_EstadoFE = (SELECT Id_EstadoFE FROM ESTADO_FACTELECTRONICA WHERE Codigo = '{2}') WHERE {1} = {3}";

        public static string UpdateClaveAcceso() => 
            "UPDATE {0} SET ClaveAccesoFactElectronica = '{1}' WHERE {2} = {3}";

        public static string UpdateLogFactura() =>
            "UPDATE {0} SET Log_Fe = {1} WHERE {2} = {3}";

        public static string SelectNotaCredito(string idNotaCredito, string idFactura, string table, string tableFactura, string table_pago) =>
            $"SELECT nc.{idNotaCredito} AS idNotaCredito, " +
            $"nc.{idFactura} AS idFactura, " +
            "nc.Id_TipoAmbiente AS ambiente, " +
            "UPPER(nc.RazonSocial_Emisor) AS razonSocial, " +
            "nc.NombreComercial_Emisor AS nombreComercial, " +
            "nc.RUC_Emisor AS ruc, " +
            "nc.ClaveAccesoFactElectronica AS claveAcceso, " +
            "(SELECT Codigo FROM TIPO_COMPROBANTE tc WHERE tc.Nombre = 'NOTA DE CRÉDITO') AS codDoc, " +
            "nc.Serie AS estab, " +
            "nc.CodEstablecimiento_Emisor AS ptoEmi, " +
            "nc.Numero secuencial, " +
            "UPPER(nc.DirMatriz_Emisor) AS dirMatriz, " +
            "(SELECT trt.Codigo " +
            "FROM TIPO_REGIMEN_TRIBUTARIO trt " +
            "WHERE trt.Id_RegimenTributario = nc.Id_RegimenTributario_Emisor) AS regimenCodigo, " +
            "(SELECT trt.Leyenda " +
            "FROM TIPO_REGIMEN_TRIBUTARIO trt " +
            "WHERE trt.Id_RegimenTributario = nc.Id_RegimenTributario_Emisor) AS regimenMicroempresas, " +
            "c.ResolAgenteRet AS agenteRetencion, " +
            "CONVERT(VARCHAR, nc.Fecha_Hora, 103) AS fechaEmision, " +
            "UPPER(nc.DirEstablecimiento_Emisor) AS dirEstablecimiento, " +
            "nc.ResolContribEsp_Emisor AS contribuyenteEspecial, " +
            "IIF(nc.ObligadoContab_Emisor = 1, 'SI', 'NO') AS obligadoContabilidad, " +
            "nc.Codigo_TipoIdCliente AS tipoIdentificacionComprador, " +
            "nc.Razon_Social AS razonSocialComprador, " +
            "nc.CI_Ruc AS identificacionComprador, " +
            "nc.Direccion AS direccionComprador, " +
            "nc.Telefono, " +
            "nc.Email, " +
            "f.Serie AS estabFac, " + 
            "f.CodEstablecimiento_Emisor AS ptoEmiFac, " +
            "f.Numero AS secuencialFac, " +
            "CONVERT(VARCHAR, f.Fecha_Hora, 103) AS fechaEmisionDocSustento, " +
            "CONVERT(DECIMAL(19, 2), (nc.Valor + nc.ValorBaseIVA)) AS totalSinImpuestos, " +
            "CONVERT(DECIMAL(19, 2), nc.Valor_Total) AS importeTotal, " +
            "c.CertificadoP12 AS certificado, " +
            "c.ClaveCertificadoP12 AS clave, " +
            "c.LogoRIDE AS LogoEmpresa, " +
            "(SELECT tp.Codigo_FormaPago_FE FROM TIPO_PAGO tp WHERE tp.Id_Tipo_Pago = p.Id_Tipo_Pago) AS formaPago " +
            $"FROM {table} nc " +
            $"INNER JOIN {table_pago} p ON nc.{idFactura} = p.{idFactura} " +
            "INNER JOIN CAJA c ON nc.Id_Caja = c.Id_Caja " +
            $"INNER JOIN {tableFactura} f ON f.Id_Factura = nc.Id_Factura " +
            "ORDER BY nc.Fecha_Hora ";

        public static string SelectNotaCreditoCompensacion(string id, long idNotaCredito, string table) => 
            "SELECT ncc.CodigoTarifa_IVA AS codigo, " +
            "tti.PorcIVA AS tarifa, " +
            "CONVERT(DECIMAL(19, 2), ncc.Iva* ncc.Cantidad) AS valor " +
            $"FROM {table} ncc " +
            "INNER JOIN TIPO_TARIFA_IVA tti ON ncc.CodigoTarifa_IVA = tti.CodigoTarifa_IVA " +
            $"WHERE ncc.{id} = {idNotaCredito}";

        public static string SelectNotaCreditoImpuesto(string id, string table, long idNotaCredito)
        {
            string field = table == "DETALLE_NC_RECAUDA" ? "Iva_Valor" : "Iva";
            return "SELECT " +
                "ncc.CodigoTarifa_IVA AS codigo, " +
                "tti.PorcIVA AS codigoPorcentaje, " +
                "CONVERT(DECIMAL(19, 2), SUM(ncc.Valor * ncc.Cantidad)) AS baseImponible, " +
                $"CONVERT(DECIMAL(19, 2), SUM(ncc.{field} * ncc.Cantidad)) AS valorDevolucionIva, " +
                $"CONVERT(DECIMAL(19, 2), SUM(ncc.{field} * ncc.Cantidad) + SUM(ncc.Valor * ncc.Cantidad)) AS valor " +
                $"FROM {table} ncc " +
                "INNER JOIN TIPO_TARIFA_IVA tti ON ncc.CodigoTarifa_IVA = tti.CodigoTarifa_IVA " +
                $"WHERE ncc.{id} = {idNotaCredito}" + 
                $"GROUP BY ncc.CodigoTarifa_IVA, tti.PorcIVA";
        }

        public static string SelectNotaCreditoDetalle(string id, string table, long idNotaCredito)
        {
            string field = table == "DETALLE_NC_RECAUDA" ? "Iva_Valor" : "Iva";

            return "SELECT ncc.Id_Concepto_Cuenta AS codigoInterno, " +
                "ncc.Id_Concepto_Cuenta AS codigoAdicional, " +
                "ncc.Nombre AS descripcion, " +
                "ncc.Cantidad AS cantidad, " +
                "CONVERT(DECIMAL(19, 4), ncc.Valor) AS precioUnitario, " +
                "ncc.CodigoTarifa_IVA AS codigoPorcentaje, " +
                "tti.PorcIVA AS tarifa, " +
                "CONVERT(DECIMAL(19, 2), ncc.Valor * ncc.Cantidad) AS precioTotalSinImpuesto, " +
                $"CONVERT(DECIMAL(19, 2), ncc.{field} * ncc.Cantidad) AS valor " +
                $"FROM {table} ncc " +
                "INNER JOIN TIPO_TARIFA_IVA tti ON ncc.CodigoTarifa_IVA = tti.CodigoTarifa_IVA " +
                $"WHERE ncc.{id} = {idNotaCredito}";
        }
    }
}
