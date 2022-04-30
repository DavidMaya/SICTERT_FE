using FacturaElectronica.Documento;
using FacturaElectronica.SRI;
using FacturaElectronica.Tools;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FacturaElectronica.Procesos
{
    public static class GenerarPDF
    {
        public static Resultado Factura(DocumentoElectronico documento, Directorio directorio, string table)
        {
            Resultado resultado = new Resultado();
            Resultado pathFirmado = directorio.Path(EstadoDocumento.Firmado);
            Resultado pathAutorizado = directorio.Path(EstadoDocumento.Autorizado);
            Resultado pathPDF = directorio.Path(EstadoDocumento.Pdf);
            Resultado pathLogo = directorio.Path(System.Configuration.ConfigurationManager.AppSettings["pathP12"]);

            if (pathFirmado.Estado && pathPDF.Estado && pathAutorizado.Estado)
            {
                XDocument FacturaXML = XDocument.Load(pathFirmado.Mensaje + documento.Nombre + ".xml");
                XDocument FacturaAuth = XDocument.Load(pathAutorizado.Mensaje + documento.Nombre + ".xml");

                string fechaEmision = (from item in FacturaAuth.Descendants("fechaAutorizacion")
                                       select item.Value).FirstOrDefault();
                fechaEmision = DateTime.Parse(fechaEmision).ToString("dd/MM/yyyy hh:mm:ss");
                FacturaAuth = null;

                if (string.IsNullOrEmpty(documento.LogoEmpresa.Trim()))
                    documento.LogoEmpresa = "SIN_LOGO.png";

                byte[] imagenStream = File.ReadAllBytes(pathLogo.Mensaje + documento.LogoEmpresa);
                Stream rutaImagen = new MemoryStream(imagenStream);

                string ambiente, tipoEmision, razonSocial, nombreComercial, ruc, claveAccesoXML, codDoc, estab, ptoEmi, secuencial, dirMatriz;
                ambiente = (from item in FacturaXML.Descendants("ambiente")
                            select item.Value).FirstOrDefault();
                tipoEmision = (from item in FacturaXML.Descendants("tipoEmision")
                               select item.Value).FirstOrDefault();
                razonSocial = (from item in FacturaXML.Descendants("razonSocial")
                               select item.Value).FirstOrDefault();
                nombreComercial = (from item in FacturaXML.Descendants("nombreComercial")
                                   select item.Value).FirstOrDefault();
                ruc = (from item in FacturaXML.Descendants("ruc")
                       select item.Value).FirstOrDefault();
                claveAccesoXML = (from item in FacturaXML.Descendants("claveAcceso")
                                  select item.Value).FirstOrDefault();
                codDoc = (from item in FacturaXML.Descendants("codDoc")
                          select item.Value).FirstOrDefault();
                estab = (from item in FacturaXML.Descendants("estab")
                         select item.Value).FirstOrDefault();
                ptoEmi = (from item in FacturaXML.Descendants("ptoEmi")
                          select item.Value).FirstOrDefault();
                secuencial = (from item in FacturaXML.Descendants("secuencial")
                              select item.Value).FirstOrDefault();
                dirMatriz = (from item in FacturaXML.Descendants("dirMatriz")
                             select item.Value).FirstOrDefault();
                //infoFactura
                string dirEstablecimiento, contribuyenteEspecial, regimenMicroempresa, contribuyenteRimpe,
                    obligadoContabilidad, tipoIdentificacionComprador, razonSocialComprador,
                    identificacionComprador, direccionComprador, totalSinImpuestos, totalDescuento, codigo,
                    codigoPorcentaje, baseImponible, valor, propina, importeTotal, moneda, formaPago, total;

                string _baseImponible = "0.00";
                string _baseImponibleIva = "0.00";
                string _codigoPorcentaje = "0";
                string _codigoPorcentajeIva = "0.00";
                string _iva = "0.00";

                foreach (XElement xmlTotalImpuesto in FacturaXML.Descendants("totalImpuesto"))
                {
                    if (xmlTotalImpuesto.ToString().Contains("codigoPorcentaje"))
                    {
                        if (xmlTotalImpuesto.Element("codigoPorcentaje").Value == "0")
                        {
                            //Valores sin IVA
                            _codigoPorcentaje = "0";
                            _baseImponible = xmlTotalImpuesto.Element("baseImponible").Value;
                        } 
                        else if(xmlTotalImpuesto.Element("codigoPorcentaje").Value == "2")
                        {
                            _codigoPorcentajeIva = "2";
                            _baseImponibleIva = xmlTotalImpuesto.Element("baseImponible").Value;
                            _iva = xmlTotalImpuesto.Element("valor").Value;
                        }
                    }
                }

                dirEstablecimiento = (from item in FacturaXML.Descendants("dirEstablecimiento")
                                      select item.Value).FirstOrDefault();
                obligadoContabilidad = (from item in FacturaXML.Descendants("obligadoContabilidad")
                                        select item.Value).FirstOrDefault();
                contribuyenteEspecial = (from item in FacturaXML.Descendants("contribuyenteEspecial")
                                         select item.Value).FirstOrDefault();
                regimenMicroempresa = (from item in FacturaXML.Descendants("regimenMicroempresas")
                                         select item.Value).FirstOrDefault();
                contribuyenteRimpe = (from item in FacturaXML.Descendants("contribuyenteRimpe")
                                       select item.Value).FirstOrDefault();
                tipoIdentificacionComprador = (from item in FacturaXML.Descendants("tipoIdentificacionComprador")
                                               select item.Value).FirstOrDefault();
                razonSocialComprador = (from item in FacturaXML.Descendants("razonSocialComprador")
                                        select item.Value).FirstOrDefault();
                identificacionComprador = (from item in FacturaXML.Descendants("identificacionComprador")
                                           select item.Value).FirstOrDefault();
                direccionComprador = (from item in FacturaXML.Descendants("direccionComprador")
                                      select item.Value).FirstOrDefault();
                totalSinImpuestos = (from item in FacturaXML.Descendants("totalSinImpuestos")
                                     select item.Value).FirstOrDefault();
                totalDescuento = (from item in FacturaXML.Descendants("totalDescuento")
                                  select item.Value).FirstOrDefault();
                codigo = (from item in FacturaXML.Descendants("codigo")
                          select item.Value).FirstOrDefault();
                codigoPorcentaje = (from item in FacturaXML.Descendants("codigoPorcentaje")
                                    select item.Value).FirstOrDefault();
                baseImponible = (from item in FacturaXML.Descendants("baseImponible")
                                 select item.Value).FirstOrDefault();
                valor = (from item in FacturaXML.Descendants("valor")
                         select item.Value).FirstOrDefault();
                propina = (from item in FacturaXML.Descendants("propina")
                           select item.Value).FirstOrDefault();
                importeTotal = (from item in FacturaXML.Descendants("importeTotal")
                                select item.Value).FirstOrDefault();
                moneda = (from item in FacturaXML.Descendants("moneda")
                          select item.Value).FirstOrDefault();
                formaPago = (from item in FacturaXML.Descendants("formaPago")
                             select item.Value).FirstOrDefault();
                total = (from item in FacturaXML.Descendants("total")
                         select item.Value).FirstOrDefault();


                string codigoPrincipal, descripcion, cantidad, precioUnitario, descuento, precioTotalSinImpuesto;
                DataTable dtDetalle = new DataTable("Detalles");
                dtDetalle.Columns.Add("codigoPrincipal");

                dtDetalle.Columns.Add("cantidad");
                dtDetalle.Columns.Add("descripcion");
                dtDetalle.Columns.Add("precioUnitario");
                dtDetalle.Columns.Add("descuento");
                dtDetalle.Columns.Add("precioTotalSinImpuestos");


                foreach (XElement xmlDetalles in FacturaXML.Descendants("detalle"))
                {
                    DataRow dr = dtDetalle.NewRow();
                    if (xmlDetalles.ToString().Contains("codigoPrincipal"))
                    {
                        codigoPrincipal = xmlDetalles.Element("codigoPrincipal").Value;
                    }
                    else
                    {
                        codigoPrincipal = xmlDetalles.Element("codigoInterno").Value;
                    }


                    descripcion = xmlDetalles.Element("descripcion").Value;
                    cantidad = xmlDetalles.Element("cantidad").Value;
                    precioUnitario = xmlDetalles.Element("precioUnitario").Value;
                    descuento = xmlDetalles.Element("descuento").Value;
                    precioTotalSinImpuesto = xmlDetalles.Element("precioTotalSinImpuesto").Value;

                    dr["codigoPrincipal"] = codigoPrincipal;
                    dr["cantidad"] = cantidad;
                    dr["descripcion"] = descripcion;
                    dr["precioUnitario"] = precioUnitario;
                    dr["descuento"] = descuento;
                    dr["precioTotalSinImpuestos"] = precioTotalSinImpuesto;
                    dtDetalle.Rows.Add(dr);
                }


                string codigoI, codigoPorcentajeI, tarifa, baseImponibleI, valorI;
                DataTable dtImpuesto = new DataTable("Impuesto");
                dtImpuesto.Columns.Add("codigo");
                dtImpuesto.Columns.Add("codigoPorcentaje");
                dtImpuesto.Columns.Add("tarifa");
                dtImpuesto.Columns.Add("baseImponible");
                dtImpuesto.Columns.Add("valor");

                foreach (XElement xmlDetalles in FacturaXML.Descendants("impuesto"))
                {
                    DataRow dr = dtImpuesto.NewRow();
                    codigoI = xmlDetalles.Element("codigo").Value;
                    codigoPorcentajeI = xmlDetalles.Element("codigoPorcentaje").Value;
                    tarifa = xmlDetalles.Element("tarifa").Value;
                    baseImponibleI = xmlDetalles.Element("baseImponible").Value;
                    valorI = xmlDetalles.Element("valor").Value;

                    dr["codigo"] = codigoI;
                    dr["codigoPorcentaje"] = codigoPorcentajeI;
                    dr["tarifa"] = tarifa;
                    dr["baseImponible"] = baseImponibleI;
                    dr["valor"] = valorI;
                    dtImpuesto.Rows.Add(dr);
                }
                string campoAdicional, campoAdicionalNombre;

                DataTable dtCampoAdicional = new DataTable("Campo Adicional");
                dtCampoAdicional.Columns.Add("campoAdicionalNombre");
                dtCampoAdicional.Columns.Add("campoAdicional");

                foreach (XElement xmlDetalles in FacturaXML.Descendants("campoAdicional"))
                {
                    DataRow dr = dtCampoAdicional.NewRow();
                    campoAdicionalNombre = (string)xmlDetalles.Attribute("nombre");
                    campoAdicional = xmlDetalles.Value;
                    dr["campoAdicionalNombre"] = campoAdicionalNombre;
                    dr["campoAdicional"] = campoAdicional;
                    dtCampoAdicional.Rows.Add(dr);
                }


                string telefonoPDF = "", webPDF = "";

                try
                {
                    using (FileStream fs = new FileStream(pathPDF.Mensaje + documento.Nombre + ".pdf", FileMode.Create))
                    {

                        //FacturaXML.Save(patch + claveAccesoXML + ".xml");

                        Document doc = new Document(PageSize.A4);
                        doc.AddAuthor(" ");
                        PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                        doc.Open();
                        PdfContentByte pcb = writer.DirectContent;


                        //tabla Razon Social

                        PdfPTable tableR = new PdfPTable(2);
                        tableR.HorizontalAlignment = Element.ALIGN_LEFT;
                        tableR.TotalWidth = 400f;
                        tableR.SetWidthPercentage(new float[] { 100, 200 }, PageSize.A4);
                        tableR.SpacingBefore = 30f;

                        var FontColour = new BaseColor(54, 81, 167);

                        PdfPCell FacN = new PdfPCell(new Paragraph("Factura N°:" + estab + "-" + ptoEmi + "-" + secuencial, FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.WHITE)));
                        FacN.FixedHeight = 25f;
                        FacN.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        FacN.HorizontalAlignment = Element.ALIGN_CENTER;
                        FacN.BackgroundColor = new BaseColor(54, 81, 167);
                        FacN.Colspan = 2;
                        FacN.BorderWidth = 0f;
                        tableR.AddCell(FacN);

                        PdfPCell RazonNombre = new PdfPCell(new Paragraph(razonSocialComprador, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        RazonNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                        RazonNombre.Colspan = 2;
                        RazonNombre.BorderWidth = 0f;
                        tableR.AddCell(RazonNombre);

                        PdfPCell identificacion = new PdfPCell(new Paragraph("Identificación", FontFactory.GetFont("Arial", 8, Font.BOLD)));
                        identificacion.BorderWidth = 0f;
                        tableR.AddCell(identificacion);

                        PdfPCell identificaionNU = new PdfPCell(new Paragraph(identificacionComprador, FontFactory.GetFont("Arial", 8)));
                        identificaionNU.Border = 0;
                        tableR.AddCell(identificaionNU);

                        PdfPCell fechaE = new PdfPCell(new Paragraph("Fecha Emisión", FontFactory.GetFont("Arial", 8, Font.BOLD)));
                        fechaE.BorderWidth = 0f;
                        tableR.AddCell(fechaE);

                        PdfPCell fechaEm = new PdfPCell(new Paragraph(fechaEmision, FontFactory.GetFont("Arial", 8)));
                        fechaEm.BorderWidth = 0f;
                        tableR.AddCell(fechaEm);

                        PdfPCell Direccion = new PdfPCell(new Paragraph("Dirección :", FontFactory.GetFont("Arial", 8, Font.BOLD)));
                        Direccion.BorderWidth = 0f;
                        tableR.AddCell(Direccion);

                        PdfPCell DirecionO = new PdfPCell(new Paragraph(direccionComprador, FontFactory.GetFont("Arial", 8)));
                        DirecionO.BorderWidth = 0f;
                        DirecionO.Colspan = 2;
                        tableR.AddCell(DirecionO);

                        //tableR.WriteSelectedRows(0, 10, 35, 820, pcb);
                        doc.Add(tableR);


                        //Tabla de empresa de PDF

                        Image jpg = Image.GetInstance(rutaImagen);
                        //Resize image depend upon your need
                        jpg.ScaleToFit(150, 180);
                        //Give space before image
                        jpg.SpacingBefore = 10f;
                        //Give some space after the image
                        jpg.SpacingAfter = 50f;
                        jpg.Alignment = Element.ALIGN_RIGHT;
                        //  jpg.Alignment = Element.ALIGN_TOP;
                        jpg.SetAbsolutePosition(350, 700);
                        doc.Add(jpg);

                        PdfPTable tableE = new PdfPTable(2);
                        tableE.HorizontalAlignment = Element.ALIGN_LEFT;
                        tableE.TotalWidth = 400f;
                        tableE.SetWidthPercentage(new float[] { 140, 180 }, PageSize.A4);
                        tableE.LockedWidth = true;


                        PdfPCell NOmbreEmpresa = new PdfPCell(new Paragraph(razonSocial, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        NOmbreEmpresa.Colspan = 2;

                        NOmbreEmpresa.BorderWidth = 0f;
                        tableE.AddCell(NOmbreEmpresa);

                        PdfPCell NombreCiuada = new PdfPCell(new Paragraph("Ruc: " + ruc, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        NombreCiuada.Colspan = 2;
                        NombreCiuada.BorderWidth = 0f;
                        tableE.AddCell(NombreCiuada);


                        PdfPCell DirecionMatriz1 = (new PdfPCell(new Paragraph(dirMatriz, FontFactory.GetFont("Arial", 8))));
                        DirecionMatriz1.Colspan = 2;
                        DirecionMatriz1.BorderWidth = 0f;
                        tableE.AddCell(DirecionMatriz1);

                        PdfPCell telefonoC = (new PdfPCell(new Paragraph("Telefono", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        telefonoC.BorderWidth = 0f;
                        tableE.AddCell(telefonoC);

                        if (telefonoPDF != "")
                        {
                            PdfPCell telefonoEmp = (new PdfPCell(new Paragraph("", FontFactory.GetFont("Arial", 8))));
                            telefonoEmp.BorderWidth = 0f;
                            tableE.AddCell(telefonoEmp);


                        }
                        else
                        {
                            PdfPCell telefonoEmp = (new PdfPCell(new Paragraph(" ", FontFactory.GetFont("Arial", 8))));
                            telefonoEmp.BorderWidth = 0f;
                            tableE.AddCell(telefonoEmp);


                        }
                        PdfPCell web = (new PdfPCell(new Paragraph("Direcion Web", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        web.BorderWidth = 0f;
                        tableE.AddCell(web);

                        if (webPDF != "")
                        {
                            PdfPCell webEmpresa = new PdfPCell(new Paragraph(" ", FontFactory.GetFont("Arial", 8)));
                            webEmpresa.BorderWidth = 0f;
                            tableE.AddCell(webEmpresa);
                        }
                        else
                        {
                            PdfPCell webEmpresa = (new PdfPCell(new Paragraph("", FontFactory.GetFont("Arial", 8))));
                            webEmpresa.BorderWidth = 0f;
                            tableE.AddCell(webEmpresa);

                        }



                        PdfPCell DirecionSucursal = (new PdfPCell(new Paragraph("Dirección Sucursal:", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        DirecionSucursal.BorderWidth = 0f;
                        tableE.AddCell(DirecionSucursal);


                        PdfPCell DirecionSucursal1 = (new PdfPCell(new Paragraph(dirEstablecimiento, FontFactory.GetFont("Arial", 8))));
                        DirecionSucursal1.BorderWidth = 0f;
                        tableE.AddCell(DirecionSucursal1);

                        PdfPCell ContriEsp = (new PdfPCell(new Paragraph("Contribuyente Especial Nro", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        ContriEsp.BorderWidth = 0f;
                        ContriEsp.HorizontalAlignment = Element.ALIGN_LEFT;
                        tableE.AddCell(ContriEsp);


                        PdfPCell contriespNUm = (new PdfPCell(new Paragraph(contribuyenteEspecial, FontFactory.GetFont("Arial", 8))));
                        contriespNUm.BorderWidth = 0f;
                        tableE.AddCell(contriespNUm);


                        PdfPCell ObliCont = (new PdfPCell(new Paragraph("Obligado a llevar contabilidad", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        ObliCont.BorderWidth = 0f;
                        ObliCont.HorizontalAlignment = Element.ALIGN_LEFT;
                        tableE.AddCell(ObliCont);


                        PdfPCell oblicontyn = (new PdfPCell(new Paragraph(obligadoContabilidad, FontFactory.GetFont("Arial", 8))));
                        oblicontyn.BorderWidth = 0f;
                        tableE.AddCell(oblicontyn);

                        if (!string.IsNullOrEmpty(regimenMicroempresa))
                        {
                            PdfPCell regMicroempresas = (new PdfPCell(new Paragraph(regimenMicroempresa, FontFactory.GetFont("Arial", 8))));
                            regMicroempresas.Colspan = 2;
                            regMicroempresas.BorderWidth = 0f;
                            tableE.AddCell(regMicroempresas);
                        } 
                        else if (!string.IsNullOrEmpty(contribuyenteRimpe))
                        {
                            PdfPCell contribRipe = (new PdfPCell(new Paragraph(contribuyenteRimpe, FontFactory.GetFont("Arial", 8))));
                            contribRipe.Colspan = 2;
                            contribRipe.BorderWidth = 0f;
                            tableE.AddCell(contribRipe);
                        }

                        

                        tableE.WriteSelectedRows(0, 10, 310, 680, pcb);






                        PdfPTable tablef = new PdfPTable(2);
                        tablef.HorizontalAlignment = Element.ALIGN_LEFT;
                        tablef.TotalWidth = 400f;
                        tablef.SetWidthPercentage(new float[] { 88, 120 }, PageSize.A4);
                        tablef.LockedWidth = true;
                        tablef.SpacingBefore = 20f;



                        PdfPCell NuAutorizacion = new PdfPCell(new Paragraph("Autorización", FontFactory.GetFont("Arial", 12, Font.BOLD)));

                        NuAutorizacion.Colspan = 2;
                        NuAutorizacion.BorderWidth = 0;
                        NuAutorizacion.HorizontalAlignment = Element.ALIGN_CENTER;
                        tablef.AddCell(NuAutorizacion);


                        PdfPCell fechayhora = new PdfPCell(new Paragraph("Fecha y hora", FontFactory.GetFont("Arial", 8, Font.BOLD)));
                        fechayhora.BorderWidth = 0;
                        tablef.AddCell(fechayhora);

                        PdfPCell fechayhoraf = new PdfPCell(new Paragraph(fechaEmision, FontFactory.GetFont("Arial", 8)));
                        fechayhoraf.BorderWidth = 0f;
                        tablef.AddCell(fechayhoraf);


                        PdfPCell AMBIENTE = new PdfPCell(new Paragraph("Ambiente", FontFactory.GetFont("Arial", 8, Font.BOLD)));

                        AMBIENTE.BorderWidth = 0f;
                        tablef.AddCell(AMBIENTE);

                        PdfPCell AMBIENTE1 = new PdfPCell(new Paragraph(ambiente == "1" ? "Pruebas" : "Producción", FontFactory.GetFont("Arial", 8)));
                        AMBIENTE1.BorderWidth = 0f;
                        tablef.AddCell(AMBIENTE1);

                        PdfPCell EMISIoN = new PdfPCell(new Paragraph("Emisión", FontFactory.GetFont("Arial", 8, Font.BOLD)));
                        EMISIoN.BorderWidth = 0f;
                        tablef.AddCell(EMISIoN);
                        PdfPCell EMISIoN1 = new PdfPCell(new Paragraph("Normal", FontFactory.GetFont("Arial", 8)));
                        EMISIoN1.BorderWidth = 0f;
                        tablef.AddCell(EMISIoN1);

                        //CLAVE ACCESO
                        PdfPCell claveAccesoPDF = new PdfPCell(new Paragraph("CLAVE DE ACCESO - AUTORIZACIÓN", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                        claveAccesoPDF.Colspan = 2;
                        claveAccesoPDF.BorderWidth = 0f;
                        claveAccesoPDF.HorizontalAlignment = Element.ALIGN_CENTER;
                        tablef.AddCell(claveAccesoPDF);


                        ///cODIGO DE BARRAS
                        PdfContentByte content = writer.DirectContent;
                        //BarcodeQRCode qr = new BarcodeQRCode("", 150, 150, null);
                        Barcode128 bar128 = new Barcode128();
                        bar128.Code = claveAccesoXML;


                        Image img39 = bar128.CreateImageWithBarcode(content, null, null);
                        tablef.SpacingAfter = 3f;
                        PdfPCell cell = new PdfPCell(img39);
                        cell.Colspan = 2;
                        cell.BorderWidthTop = 0f;
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidth = 0f;
                        tablef.AddCell(cell);
                        doc.Add(tablef);




                        PdfPTable tblEncabezadosItem = new PdfPTable(5);
                        tblEncabezadosItem.SetWidthPercentage(new float[] { 30, 20, 80, 30, 30 }, PageSize.A4);
                        tblEncabezadosItem.HorizontalAlignment = Element.ALIGN_CENTER;
                        tblEncabezadosItem.TotalWidth = 540f;
                        tblEncabezadosItem.LockedWidth = true;
                        tblEncabezadosItem.SpacingBefore = 40f;


                        //Headers

                        tblEncabezadosItem.AddCell(new PdfPCell(new Paragraph("Cod.Principal: ", FontFactory.GetFont("Arial", 8, BaseColor.WHITE))));
                        tblEncabezadosItem.AddCell(new PdfPCell(new Paragraph("Cant:", FontFactory.GetFont("Arial", 8, BaseColor.WHITE))));
                        tblEncabezadosItem.AddCell(new PdfPCell(new Paragraph("Descripcion:", FontFactory.GetFont("Arial", 8, BaseColor.WHITE))));
                        tblEncabezadosItem.AddCell(new PdfPCell(new Paragraph("P.Unitario:", FontFactory.GetFont("Arial", 8, BaseColor.WHITE))));
                        tblEncabezadosItem.AddCell(new PdfPCell(new Paragraph("P.T.Sin Impuestos:", FontFactory.GetFont("Arial", 8, BaseColor.WHITE))));





                        //¿Le damos un poco de formato?
                        foreach (PdfPCell celda in tblEncabezadosItem.Rows[0].GetCells())
                        {

                            celda.BackgroundColor = new BaseColor(54, 81, 167);
                            celda.HorizontalAlignment = Element.ALIGN_CENTER;
                            celda.Colspan = 1;

                        }
                        if (dtDetalle.Rows.Count <= 5)
                        {
                            tblEncabezadosItem.WriteSelectedRows(0, 10, 35, 560, pcb);
                        }
                        else
                        {
                            doc.Add(tblEncabezadosItem);
                        }

                        //   


                        PdfPTable tblItem = new PdfPTable(5);
                        tblItem.SetWidthPercentage(new float[] { 30, 20, 80, 30, 30 }, PageSize.A4);
                        tblItem.HorizontalAlignment = Element.ALIGN_CENTER;
                        tblItem.TotalWidth = 540f;
                        tblItem.LockedWidth = true;



                        for (int i = 0; i < dtDetalle.Rows.Count; i++)
                        {
                            tblItem.AddCell(new PdfPCell(new Paragraph(dtDetalle.Rows[i]["codigoPrincipal"].ToString(), FontFactory.GetFont("Arial", 8))));
                            tblItem.AddCell(new PdfPCell(new Paragraph(dtDetalle.Rows[i]["cantidad"].ToString(), FontFactory.GetFont("Arial", 8))));
                            tblItem.AddCell(new PdfPCell(new Paragraph(dtDetalle.Rows[i]["descripcion"].ToString(), FontFactory.GetFont("Arial", 8))));
                            tblItem.AddCell(new PdfPCell(new Paragraph(dtDetalle.Rows[i]["precioUnitario"].ToString().Replace(",", "."), FontFactory.GetFont("Arial", 8))));
                            tblItem.AddCell(new PdfPCell(new Paragraph(dtDetalle.Rows[i]["precioTotalSinImpuestos"].ToString().Replace(",", "."), FontFactory.GetFont("Arial", 8))));


                            foreach (PdfPCell celda in tblItem.Rows[i].GetCells())
                            {


                                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                                celda.Colspan = 1;

                            }

                        }
                        if (dtDetalle.Rows.Count <= 5)
                        {
                            tblItem.WriteSelectedRows(0, 10, 35, 540, pcb);
                        }
                        else
                        {
                            doc.Add(tblItem);
                            // 
                        }



                        // tabal de observaciones

                        PdfPTable tableO = new PdfPTable(2);
                        tableO.HorizontalAlignment = Element.ALIGN_LEFT;
                        tableO.TotalWidth = 350f;
                        tableO.SpacingBefore = 20f;
                        tableO.SetWidthPercentage(new float[] { 90, 250 }, PageSize.A4);
                        //  tableO.LockedWidth = true;

                        PdfPCell titlosOb = (new PdfPCell(new Paragraph("Información Adicional ", FontFactory.GetFont("Arial", 10, Font.BOLD))));
                        titlosOb.Colspan = 2;
                        titlosOb.BorderWidthBottom = 1f;
                        tableO.AddCell(titlosOb);


                        for (int i = 0; i < dtCampoAdicional.Rows.Count; i++)
                        {
                            tableO.AddCell(new PdfPCell(new Paragraph(dtCampoAdicional.Rows[i]["campoAdicionalNombre"].ToString(), FontFactory.GetFont("Arial", 8))));
                            tableO.AddCell(new PdfPCell(new Paragraph(dtCampoAdicional.Rows[i]["campoAdicional"].ToString(), FontFactory.GetFont("Arial", 8))));


                            //foreach (PdfPCell celda in tableO.Rows[i].GetCells())
                            //{


                            //    celda.HorizontalAlignment = Element.ALIGN_CENTER;
                            //    celda.Colspan = 1;

                            //}

                        }


                        if (dtDetalle.Rows.Count <= 5)
                        {
                            tableO.WriteSelectedRows(0, 10, 35, 400, pcb);
                        }
                        else
                        {
                            doc.Add(tableO);

                        }






                        //tabla de valores 

                        PdfPTable tableV = new PdfPTable(2);
                        tableV.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableV.TotalWidth = 350f;
                        tableV.SpacingBefore = 20f;
                        tableV.SetWidthPercentage(new float[] { 120, 55 }, PageSize.A4);
                        DateTime fechaInicio = Convert.ToDateTime(fechaEmision).Date;
                        DateTime fechaIVA12 = Convert.ToDateTime("01/06/2017").Date;
                        if (fechaInicio <= fechaIVA12)
                        {
                            PdfPCell subtotal = (new PdfPCell(new Paragraph("SUBTOTAL 14%", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                            tableV.AddCell(subtotal);

                            if (_codigoPorcentajeIva != "2")
                            {
                                PdfPCell valorsub = (new PdfPCell(new Paragraph(_baseImponibleIva, FontFactory.GetFont("Arial", 8))));
                                valorsub.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tableV.AddCell(valorsub);

                            }
                            else
                            {

                                PdfPCell valorsub = (new PdfPCell(new Paragraph("0.00", FontFactory.GetFont("Arial", 8))));
                                valorsub.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tableV.AddCell(valorsub);
                            }
                        }
                        else if (fechaInicio >= fechaIVA12)
                        {
                            PdfPCell subtotal = (new PdfPCell(new Paragraph("SUBTOTAL 12%", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                            tableV.AddCell(subtotal);
                            if (_codigoPorcentajeIva == "2")
                            {
                                PdfPCell valorsub = (new PdfPCell(new Paragraph(_baseImponibleIva, FontFactory.GetFont("Arial", 8))));
                                valorsub.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tableV.AddCell(valorsub);
                            }
                            else
                            {
                                PdfPCell valorsub = (new PdfPCell(new Paragraph("0.00", FontFactory.GetFont("Arial", 8))));
                                valorsub.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tableV.AddCell(valorsub);
                            }
                        }
                        else
                        {

                            PdfPCell valorsub = (new PdfPCell(new Paragraph("0.00", FontFactory.GetFont("Arial", 8))));
                            valorsub.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableV.AddCell(valorsub);
                        }

                        PdfPCell subtotalIva = (new PdfPCell(new Paragraph("SUBTOTAL 0%", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        tableV.AddCell(subtotalIva);



                        if (_codigoPorcentaje == "0")
                        {
                            PdfPCell valoriva0 = (new PdfPCell(new Paragraph(_baseImponible, FontFactory.GetFont("Arial", 8))));
                            valoriva0.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableV.AddCell(valoriva0);
                        }
                        else
                        {
                            PdfPCell valoriva0 = (new PdfPCell(new Paragraph("0.00", FontFactory.GetFont("Arial", 8))));
                            valoriva0.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableV.AddCell(valoriva0);
                        }

                        PdfPCell subtotalNOI = (new PdfPCell(new Paragraph("SUBTOTAL NO OBJETO IVA ", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        tableV.AddCell(subtotalNOI);
                        PdfPCell subtotalNOI1 = (new PdfPCell(new Paragraph("0.00 ", FontFactory.GetFont("Arial", 8))));
                        subtotalNOI1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableV.AddCell(subtotalNOI1);


                        PdfPCell subtotalEI = (new PdfPCell(new Paragraph("SUBTOTAL EXENTO IVA", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        tableV.AddCell(subtotalEI);
                        PdfPCell subtotalEI1 = (new PdfPCell(new Paragraph("0.00", FontFactory.GetFont("Arial", 8))));
                        subtotalEI1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableV.AddCell(subtotalEI1);


                        PdfPCell subtotalSI = (new PdfPCell(new Paragraph("SUBTOTAL SIN IMPUESTOS", FontFactory.GetFont("Arial", 8))));
                        tableV.AddCell(subtotalSI);
                        PdfPCell subtotalSI1 = (new PdfPCell(new Paragraph(totalSinImpuestos, FontFactory.GetFont("Arial", 8))));
                        subtotalSI1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableV.AddCell(subtotalSI1);


                        PdfPCell descuentoPDF = (new PdfPCell(new Paragraph("DESCUENTO", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        tableV.AddCell(descuentoPDF);

                        PdfPCell descuento1 = (new PdfPCell(new Paragraph("0.00 ", FontFactory.GetFont("Arial", 8))));
                        descuento1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableV.AddCell(descuento1);


                        PdfPCell ice = (new PdfPCell(new Paragraph("ICE", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        tableV.AddCell(ice);
                        PdfPCell ice1 = (new PdfPCell(new Paragraph("0.00 ", FontFactory.GetFont("Arial", 8))));
                        ice1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableV.AddCell(ice1);
                        if (fechaInicio <= fechaIVA12)
                        {
                            PdfPCell iva14 = (new PdfPCell(new Paragraph("IVA 14%", FontFactory.GetFont("Arial", 8))));
                            tableV.AddCell(iva14);
                            if (_codigoPorcentajeIva != "2")
                            {

                                PdfPCell iva141 = (new PdfPCell(new Paragraph(_iva, FontFactory.GetFont("Arial", 8))));
                                iva141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tableV.AddCell(iva141);

                            }
                            else
                            {

                                PdfPCell iva141 = (new PdfPCell(new Paragraph(_iva, FontFactory.GetFont("Arial", 8))));
                                iva141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tableV.AddCell(iva141);

                            }
                        }
                        else if (fechaInicio >= fechaIVA12)
                        {

                            PdfPCell iva14 = (new PdfPCell(new Paragraph("IVA 12%", FontFactory.GetFont("Arial", 8))));
                            tableV.AddCell(iva14);
                            if (_codigoPorcentajeIva != "2")
                            {

                                PdfPCell iva141 = (new PdfPCell(new Paragraph(_iva, FontFactory.GetFont("Arial", 8))));
                                iva141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tableV.AddCell(iva141);

                            }
                            else if (_codigoPorcentajeIva == "2")
                            {
                                if (_codigoPorcentajeIva == "2")
                                {
                                    PdfPCell iva141 = (new PdfPCell(new Paragraph(_iva, FontFactory.GetFont("Arial", 8))));
                                    iva141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    tableV.AddCell(iva141);
                                }
                                else
                                {

                                    PdfPCell iva141 = (new PdfPCell(new Paragraph(_iva, FontFactory.GetFont("Arial", 8))));
                                    iva141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    tableV.AddCell(iva141);

                                }
                            }
                            else
                            {

                                PdfPCell iva141 = (new PdfPCell(new Paragraph(_iva, FontFactory.GetFont("Arial", 8))));
                                iva141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tableV.AddCell(iva141);
                            }
                        }
                        PdfPCell valorTotal = (new PdfPCell(new Paragraph("VALOR TOTAL", FontFactory.GetFont("Arial", 8, BaseColor.WHITE))));
                        valorTotal.BackgroundColor = new BaseColor(54, 81, 167);
                        tableV.AddCell(valorTotal);
                        PdfPCell valorTotal1 = (new PdfPCell(new Paragraph(importeTotal, FontFactory.GetFont("Arial", 8, BaseColor.WHITE))));
                        valorTotal1.BackgroundColor = new BaseColor(54, 81, 167);
                        valorTotal1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tableV.AddCell(valorTotal1);
                        //¿Le damos un poco de formato?
                        if (dtDetalle.Rows.Count <= 5)
                        {
                            tableV.WriteSelectedRows(0, 11, 400, 400, pcb);
                        }
                        else
                        {
                            doc.Add(tableV);
                        }

                        // 

                        //Tabla Forma de Pago

                        PdfPTable tablaFP = new PdfPTable(2);
                        tablaFP.HorizontalAlignment = Element.ALIGN_LEFT;
                        tablaFP.TotalWidth = 350f;
                        tablaFP.SetWidthPercentage(new float[] { 160, 50 }, PageSize.A4);


                        PdfPCell formaP = (new PdfPCell(new Paragraph("Forma de Pago ", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        tablaFP.AddCell(formaP);
                        PdfPCell valorFP = (new PdfPCell(new Paragraph(" Valor  ", FontFactory.GetFont("Arial", 8, Font.BOLD))));
                        tablaFP.AddCell(valorFP);

                        if (dtDetalle.Rows.Count <= 5)
                        {
                            tablaFP.WriteSelectedRows(0, 2, 35, 250, pcb);
                        }
                        else
                        {
                            doc.Add(tablaFP);
                        }


                        PdfPTable tablaFP1 = new PdfPTable(2);
                        tablaFP1.HorizontalAlignment = Element.ALIGN_LEFT;
                        tablaFP1.TotalWidth = 350f;
                        tablaFP1.SetWidthPercentage(new float[] { 160, 50 }, PageSize.A4);


                        if (formaPago == "01")
                        {
                            tablaFP1.AddCell(new PdfPCell(new Paragraph("SIN UTILIZACION DEL SISTEMA FINANCIERO", FontFactory.GetFont("Arial", 8))));

                            tablaFP1.AddCell(new PdfPCell(new Paragraph(total, FontFactory.GetFont("Arial", 8))));


                        }
                        else if (formaPago == "15")
                        {
                            tablaFP1.AddCell(new PdfPCell(new Paragraph("COMPENSACION DE DEUDAS", FontFactory.GetFont("Arial", 8))));

                            tablaFP1.AddCell(new PdfPCell(new Paragraph(total, FontFactory.GetFont("Arial", 8))));

                        }
                        else if (formaPago == "16")
                        {
                            tablaFP1.AddCell(new PdfPCell(new Paragraph("TARJETA DE DEBITO", FontFactory.GetFont("Arial", 8))));

                            tablaFP1.AddCell(new PdfPCell(new Paragraph(total, FontFactory.GetFont("Arial", 8))));
                        }
                        else if (formaPago == "17")
                        {
                            tablaFP1.AddCell(new PdfPCell(new Paragraph("DINERO ELECTRONICO", FontFactory.GetFont("Arial", 8))));

                            tablaFP1.AddCell(new PdfPCell(new Paragraph(total, FontFactory.GetFont("Arial", 8))));

                        }

                        else if (formaPago == "18")
                        {
                            tablaFP1.AddCell(new PdfPCell(new Paragraph("TARJETA PREPAGO", FontFactory.GetFont("Arial", 8))));

                            tablaFP1.AddCell(new PdfPCell(new Paragraph(total, FontFactory.GetFont("Arial", 8))));


                        }
                        if (formaPago == "19")
                        {
                            tablaFP1.AddCell(new PdfPCell(new Paragraph("TARJETA DE CRÉDITO", FontFactory.GetFont("Arial", 8))));

                            tablaFP1.AddCell(new PdfPCell(new Paragraph(total, FontFactory.GetFont("Arial", 8))));


                        }
                        else if (formaPago == "20")
                        {

                            tablaFP1.AddCell(new PdfPCell(new Paragraph("OTROS CON UTILIZACION DEL SISTEMA FINANCIERO", FontFactory.GetFont("Arial", 8))));

                            tablaFP1.AddCell(new PdfPCell(new Paragraph(total, FontFactory.GetFont("Arial", 8))));


                        }
                        else if (formaPago == "21")
                        {

                            tablaFP1.AddCell(new PdfPCell(new Paragraph("ENDOSO DE TITULOS", FontFactory.GetFont("Arial", 8))));

                            tablaFP1.AddCell(new PdfPCell(new Paragraph(total, FontFactory.GetFont("Arial", 8))));
                        }

                        int cont = 0;

                        for (int i = -1; i < tablaFP.Rows.Count; i++)
                        {

                            if (cont == 0)
                            {
                                cont++;
                                if (dtDetalle.Rows.Count <= 5)
                                {
                                    tablaFP1.WriteSelectedRows(0, i, 35, 235, pcb);
                                }
                                else
                                {
                                    doc.Add(tablaFP1);
                                }
                            }


                        }

                        //
                        PdfPTable tableHoly = new PdfPTable(1);
                        tableHoly.HorizontalAlignment = Element.ALIGN_CENTER;
                        tableHoly.TotalWidth = 350f;
                        tableHoly.SetWidthPercentage(new float[] { 130 }, PageSize.A4);

                        resultado.Estado = true;
                        //System.IO.File.Delete(pathFirmado.Mensaje + documento.Nombre + ".xml");

                        doc.Close();
                        fs.Close();

                        return resultado;
                    }
                }
                catch (Exception ex)
                {
                    resultado.Estado = false;
                    resultado.Mensaje = ex.Message;
                    return resultado;
                }

            } 
            else
            {
                resultado.Estado = false;
                if (!pathFirmado.Estado)
                    resultado.Mensaje = "No se ha conseguido cargar el directorio Inicial:\n" + pathFirmado.Mensaje;
                if (!pathPDF.Estado)
                    resultado.Mensaje = "No se ha conseguido cargar el directorio Inicial:\n" + pathPDF.Mensaje;
                return resultado;
            }
        }

        public static Resultado Factura2(DocumentoElectronico documento, Directorio directorio, string table)
        {
            Resultado resultado = new Resultado();
            Resultado pathFirmado = directorio.Path(EstadoDocumento.Firmado);
            Resultado pathAutorizado = directorio.Path(EstadoDocumento.Autorizado);
            Resultado pathPDF = directorio.Path(EstadoDocumento.Pdf);
            Resultado pathLogo = directorio.Path(System.Configuration.ConfigurationManager.AppSettings["pathP12"]);

            if (pathFirmado.Estado && pathPDF.Estado && pathAutorizado.Estado)
            {
                XDocument FacturaAuth = XDocument.Load(pathAutorizado.Mensaje + documento.Nombre + ".xml");
                string fechaEmision = (from item in FacturaAuth.Descendants("fechaAutorizacion")
                                       select item.Value).FirstOrDefault();
                fechaEmision = DateTime.Parse(fechaEmision).ToString("dd/MM/yyyy hh:mm:ss");
                var xml  = (from item in FacturaAuth.Descendants("comprobante")
                          select item.Value).FirstOrDefault();

                factura fact = Tools.XmlTools.Deserialize<factura>(xml);
                FacturaAuth = null;

                if (string.IsNullOrEmpty(documento.LogoEmpresa.Trim()))
                    documento.LogoEmpresa = "SIN_LOGO.png";
                byte[] imagenStream = File.ReadAllBytes(pathLogo.Mensaje + documento.LogoEmpresa);
                Stream rutaImagen = new MemoryStream(imagenStream);

                try
                {
                    using (FileStream fileStream = new FileStream(pathPDF.Mensaje + documento.Nombre + "_test.pdf", FileMode.Create))
                    {
                        Document document = new Document(PageSize.A4);
                        document.AddAuthor(" ");
                        PdfWriter writer = PdfWriter.GetInstance(document, fileStream);

                        document.Open();
                        PdfContentByte content = writer.DirectContent;

                        //Rectangle rectAll = new Rectangle(document.PageSize);

                        //rectAll.Left += document.LeftMargin;
                        //rectAll.Right -= document.RightMargin;
                        //rectAll.Top -= document.TopMargin;
                        //rectAll.Bottom += document.BottomMargin;
                        //content.SetColorStroke(BaseColor.RED);
                        //content.Rectangle(rectAll.Left, rectAll.Bottom, rectAll.Width, rectAll.Height);
                        //content.Stroke();

                        // Imagen del documento.
                        Image imageLogo = Image.GetInstance(rutaImagen);
                        //Resize image depend upon your need
                        imageLogo.ScaleToFit(150, 180);
                        //Give space before image
                        //imageLogo.SpacingBefore = 0f;
                        ////Give some space after the image
                        //imageLogo.SpacingAfter = 0f;
                        imageLogo.Alignment = Element.ALIGN_LEFT;
                        //  jpg.Alignment = Element.ALIGN_TOP;
                        //imageLogo.SetAbsolutePosition(350, 700);
                        document.Add(imageLogo);


                        //PdfPTable tableInfoFactura = new PdfPTable(2);
                        //tableInfoFactura.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //tableInfoFactura.TotalWidth = 400f;
                        //tableInfoFactura.SetWidthPercentage(new float[] { 100, 200 }, PageSize.A4);
                        ////tableInfoFactura.SpacingBefore = 30f;
                        //tableInfoFactura.DefaultCell.BackgroundColor = new BaseColor(54, 81, 167);
                        //tableInfoFactura.DefaultCell.BorderWidth = 1;


                        //PdfPCell FacN = new PdfPCell(new Paragraph("Factura N°:", FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK)));
                        ////FacN.FixedHeight = 25f;
                        //FacN.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        //FacN.HorizontalAlignment = Element.ALIGN_CENTER;
                        ////FacN.BackgroundColor = new BaseColor(54, 81, 167);
                        //FacN.Colspan = 2;
                        //FacN.BorderWidth = 0f;
                        //tableInfoFactura.AddCell(FacN);
                        //document.Add(tableInfoFactura);
                        //PdfPTable pdfTable = new PdfPTable(3);
                        //pdfTable.DefaultCell.Padding = 3;
                        //pdfTable.WidthPercentage = 100;
                        //pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        //pdfTable.DefaultCell.BorderWidth = 1;

                        PdfPTable table1 = new PdfPTable(3);

                        table1.TotalWidth = 144f;
                        table1.LockedWidth = true;
                        table1.HorizontalAlignment = 0;
                        table1.HorizontalAlignment = Element.ALIGN_RIGHT;

                        PdfPCell left = new PdfPCell(new Paragraph("Rotated"));
                        table1.AddCell(left);
                        PdfPCell middle = new PdfPCell(new Paragraph("Rotated"));
                        table1.AddCell(middle);
                        table1.AddCell("Not Rotated");
                        document.Add(table1);


                        resultado.Estado = true;
                        //System.IO.File.Delete(pathFirmado.Mensaje + documento.Nombre + ".xml");

                        document.Close();
                        fileStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    resultado.Estado = false;
                    resultado.Mensaje = ex.Message;
                    return resultado;
                }

                return resultado;
            }
            else
            {
                resultado.Estado = false;
                if (!pathFirmado.Estado)
                    resultado.Mensaje = "No se ha conseguido cargar el directorio Inicial:\n" + pathFirmado.Mensaje;
                if (!pathPDF.Estado)
                    resultado.Mensaje = "No se ha conseguido cargar el directorio Inicial:\n" + pathPDF.Mensaje;
                return resultado;
            }
        }
    }
}
