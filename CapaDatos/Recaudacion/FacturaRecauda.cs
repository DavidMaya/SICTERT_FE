using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.Recaudacion
{
    public class FacturaRecauda : SqlConnecion, IDisposable
    {
        private long _IdCaja;
        private Caja _Caja;
        private string _Codigo_TipoIdCliente;
        private Tipo_IdCliente _Tipo_IdCliente;

        #region Public Properties
        public long IdFacturaRecauda { get; set; }
        public DateTime? FechaHora { get; set; }
        public bool Estado { get; set; }
        public int Cantidad { get; set; }
        public decimal IvaPorcentaje { get; set; }
        public decimal IvaValor { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorBaseIva { get; set; }
        public decimal ValorTotal { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string CIRuc { get; set; }
        public long IdCaja
        {
            get { return _IdCaja; }
            set
            {
                _IdCaja = value;
                _Caja = null;
            }
        }
        public long IdCierre { get; set; }
        public long IdTurno { get; set; }
        public long Numero { get; set; }
        public string Serie { get; set; }
        public string ClaveAccesoFactElectronica { get; set; }
        public int IdEstadoFE { get; set; }
        public int IdTipoFactura { get; set; }
        public int IdTipoAmbiente { get; set; }
        public string RUCEmisor { get; set; }
        public string RazonSocialEmisor { get; set; }
        public string NombreComercialEmisor { get; set; }
        public string DirMatrizEmisor { get; set; }
        public string DirEstablecimientoEmisor { get; set; }
        public string CodEstablecimientoEmisor { get; set; }
        public string ResolContribEspEmisor { get; set; }
        public bool ObligadoContabEmisor { get; set; }
        public string Recaudador { get; set; }
        public string NombreCaja { get; set; }
        public Caja caja
        {
            get
            {
                if (_Caja != null && _Caja.Id_Caja != 0)
                    return _Caja;
                else if (IdCaja != 0)
                    return _Caja = new Caja(IdCaja);
                else
                    return null;
            }
        }
        public string NumeroSerie
        {
            get
            {
                return CodEstablecimientoEmisor.ToString().PadLeft(3, '0') + "-" + Serie.ToString().PadLeft(3, '0');
            }
        }
        public string NumeroCompleto
        {
            get
            {
                return CodEstablecimientoEmisor.ToString().PadLeft(3, '0') + "-" + Serie.ToString().PadLeft(3, '0') + "-" +
                   Numero.ToString().PadLeft(9, '0');
            }
        }
        public string Codigo_TipoIdCliente
        {
            get { return _Codigo_TipoIdCliente; }
            set
            {
                _Codigo_TipoIdCliente = value;
                _Tipo_IdCliente = null;
            }
        }
        public Tipo_IdCliente tipo_idcliente
        {
            get
            {
                if (_Tipo_IdCliente != null && _Tipo_IdCliente.Codigo_TipoIdCliente != "")
                    return _Tipo_IdCliente;
                else if (_Codigo_TipoIdCliente != "")
                    return _Tipo_IdCliente = new Tipo_IdCliente(_Codigo_TipoIdCliente);
                else
                    return null;
            }
        }
        #endregion

        public FacturaRecauda()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public FacturaRecauda(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Factura_Recauda = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    IdFacturaRecauda = Convert.ToInt64(dt.Rows[0]["Id_Factura_Recauda"]);
                    FechaHora = valorDateTime(dt.Rows[0]["Fecha_Hora"]);
                    Estado = Convert.ToBoolean(dt.Rows[0]["Estado"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    Cantidad = Convert.ToInt32(dt.Rows[0]["Cantidad"]);
                    IvaPorcentaje = Convert.ToDecimal(dt.Rows[0]["Iva_Porcentaje"]);
                    IvaValor = Convert.ToDecimal(dt.Rows[0]["Iva_Valor"]);
                    ValorBaseIva = Convert.ToDecimal(dt.Rows[0]["ValorBaseIva"]);
                    ValorTotal = Convert.ToDecimal(dt.Rows[0]["Valor_Total"]);
                    RazonSocial = Convert.ToString(dt.Rows[0]["Razon_Social"]);
                    Direccion = dt.Rows[0]["Direccion"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Direccion"]) : "";
                    Telefono = dt.Rows[0]["Telefono"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Telefono"]) : "";
                    CIRuc = Convert.ToString(dt.Rows[0]["CI_Ruc"]);
                    _IdCaja = Convert.ToInt64(dt.Rows[0]["Id_Caja"]);
                    IdCierre = Convert.ToInt64(dt.Rows[0]["Id_Cierre"]);
                    Numero = Convert.ToInt64(dt.Rows[0]["Numero"]);
                    Serie = Convert.ToString(dt.Rows[0]["Serie"]);
                    ClaveAccesoFactElectronica = Convert.ToString(dt.Rows[0]["ClaveAccesoFactElectronica"]);
                    IdEstadoFE = dt.Rows[0]["Id_EstadoFE"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Id_EstadoFE"]) : 0;
                    IdTipoFactura = Convert.ToInt32(dt.Rows[0]["Id_TipoFactura"]);
                    IdTipoAmbiente = Convert.ToInt32(dt.Rows[0]["Id_TipoAmbiente"]);
                    RUCEmisor = Convert.ToString(dt.Rows[0]["RUC_Emisor"]);
                    RazonSocialEmisor = Convert.ToString(dt.Rows[0]["RazonSocial_Emisor"]);
                    NombreComercialEmisor = Convert.ToString(dt.Rows[0]["NombreComercial_Emisor"]);
                    DirMatrizEmisor = Convert.ToString(dt.Rows[0]["DirMatriz_Emisor"]);
                    DirEstablecimientoEmisor = Convert.ToString(dt.Rows[0]["DirEstablecimiento_Emisor"]);
                    CodEstablecimientoEmisor = Convert.ToString(dt.Rows[0]["CodEstablecimiento_Emisor"]);
                    ResolContribEspEmisor = Convert.ToString(dt.Rows[0]["ResolContribEsp_Emisor"]);
                    ObligadoContabEmisor = Convert.ToBoolean(dt.Rows[0]["ObligadoContab_Emisor"]);
                    Recaudador = Convert.ToString(dt.Rows[0]["Recaudador"]);
                    NombreCaja = Convert.ToString(dt.Rows[0]["NombreCaja"]);
                    _Codigo_TipoIdCliente = Convert.ToString(dt.Rows[0]["Codigo_TipoIdCliente"]);
                }
            }
        }

        private Nullable<DateTime> valorDateTime(object ValorFecha)
        {
            if (string.IsNullOrEmpty(ValorFecha.ToString()))
                return null;
            else
                return Convert.ToDateTime(ValorFecha);
        }

        private static string GetSqlSelect()
        {
            return String.Format("SELECT Id_Factura_Recauda, Fecha_Hora, Estado, Valor, Cantidad, Iva_Porcentaje, Iva_Valor, Razon_Social, Direccion, Telefono, CI_Ruc, Id_Caja, Id_Cierre, Numero, Serie, " +
                "ClaveAccesoFactElectronica, Id_EstadoFE, Id_TipoFactura, Id_TipoAmbiente, RUC_Emisor, RazonSocial_Emisor, NombreComercial_Emisor, DirMatriz_Emisor, Codigo_TipoIdCliente, " +
                "DirEstablecimiento_Emisor, CodEstablecimiento_Emisor, ResolContribEsp_Emisor, ObligadoContab_Emisor, Recaudador, NombreCaja, ValorBaseIva, Valor_Total" +
                "FROM FACTURA_RECAUDA");
        }

        public static DataTable GetAllData(string Where)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + String.Format((Where.Length > 0) ? (" WHERE " + Where) : ""));
        }

        public static DataTable GetAllData(string Where, string Order)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + String.Format((Where.Length > 0) ? (" WHERE " + Where) : "")
                + String.Format((Order.Length > 0) ? (" ORDER BY " + Order) : ""));
        }

        public static DataTable GetAllData(string Where, string Join, string Order)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + " " + Join + " " + String.Format((Where.Length > 0) ? (" WHERE " + Where) : "")
                + String.Format((Order.Length > 0) ? (" ORDER BY " + Order) : ""));
        }

        public static DataTable GetAllData()
        {
            return GetAllData("");
        }

        public static FacturaRecauda GetFactura(long id)
        {
            return new FacturaRecauda(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Factura_Recauda", "FACTURA_RECAUDA", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        public string Insert()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }

        public string GetSQLInsert()
        {
            return String.Format("INSERT INTO FACTURA_RECAUDA (Serie, Numero, Fecha_Hora, Estado, Valor, Iva_Porcentaje, Iva_Valor, Razon_Social, CI_Ruc, Direccion, Telefono, Id_Caja, Id_Cierre, " +
                "Cantidad, ClaveAccesoFactElectronica, Id_EstadoFE, Id_TipoFactura, Id_TipoAmbiente, RUC_Emisor, RazonSocial_Emisor, NombreComercial_Emisor, DirMatriz_Emisor, " +
                "DirEstablecimiento_Emisor, CodEstablecimiento_Emisor, ResolContribEsp_Emisor, ObligadoContab_Emisor, Recaudador, NombreCaja, ValorBaseIva, Codigo_TipoIdCliente) " +
                "VALUES ('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', {14}, {15}, {16}, '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', {27}, '{28}'); ",
                Serie, Numero, (FechaHora == null) ? "NULL" : "'" + Convert.ToDateTime(FechaHora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Estado ? 1 : 0, Valor.ToString().Replace(",", SqlServer.SigFloatSql), 
                IvaPorcentaje, IvaValor.ToString().Replace(",", SqlServer.SigFloatSql), RazonSocial, CIRuc, Direccion, Telefono, IdCaja, IdCierre, Cantidad, ClaveAccesoFactElectronica,
                IdEstadoFE == 0 ? "NULL" : IdEstadoFE.ToString(), IdTipoFactura == 0 ? "NULL" : IdTipoFactura.ToString(), IdTipoAmbiente == 0 ? "NULL" : IdTipoAmbiente.ToString(), RUCEmisor, RazonSocialEmisor, NombreComercialEmisor,
                DirMatrizEmisor, DirEstablecimientoEmisor, CodEstablecimientoEmisor, ResolContribEspEmisor, ObligadoContabEmisor, Recaudador, NombreCaja, ValorBaseIva, Codigo_TipoIdCliente);
        }

        public string Delete(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete(long ID)
        {
            return String.Format("DELETE FROM Factura WHERE Id_Factura = {0};", ID);
        }

        public string Update()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate());
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;
        }

        public string GetSQLUpdate()
        {
            return String.Format("UPDATE FACTURA_RECAUDA SET Serie= '{1}', Numero= '{2}', Fecha_Hora= {3}, Estado= '{4}', Valor= '{5}', Iva_Porcentaje= '{6}', " +
                "Iva_Valor= '{7}', Razon_Social= '{8}', CI_Ruc= '{9}', Direccion= '{10}', Telefono= '{11}', Id_Caja= '{12}', Id_Cierre= '{13}', " +
                "Cantidad= '{14}', ClaveAccesoFactElectronica= '{15}', Id_EstadoFE= {16}, Id_TipoFactura= {17}, Id_TipoAmbiente= {18}, RUC_Emisor= '{19}', " +
                "RazonSocial_Emisor= '{20}', NombreComercial_Emisor= '{21}', DirMatriz_Emisor= '{22}', DirEstablecimiento_Emisor= '{23}', " +
                "CodEstablecimiento_Emisor= '{24}', ResolContribEsp_Emisor= '{25}', ObligadoContab_Emisor= '{26}', Recaudador= '{27}' NombreCaja= '{28}' " +
                "ValorBaseIva = {29}, Codigo_TipoIdCliente= '{30}' WHERE Id_Factura_Recauda = {0};", IdFacturaRecauda, Serie, Numero, 
                (FechaHora == null) ? "NULL" : "'" + Convert.ToDateTime(FechaHora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Estado ? 1 : 0, 
                Valor.ToString().Replace(",", SqlServer.SigFloatSql), IvaPorcentaje, IvaValor.ToString().Replace(",", SqlServer.SigFloatSql), 
                RazonSocial, CIRuc, Direccion, Telefono, IdCaja, IdCierre, Cantidad, ClaveAccesoFactElectronica,
                IdEstadoFE == 0 ? "NULL" : IdEstadoFE.ToString(), IdTipoFactura == 0 ? "NULL" : IdTipoFactura.ToString(), 
                IdTipoAmbiente == 0 ? "NULL" : IdTipoAmbiente.ToString(), RUCEmisor, RazonSocialEmisor, NombreComercialEmisor,
                DirMatrizEmisor, DirEstablecimientoEmisor, CodEstablecimientoEmisor, ResolContribEspEmisor, ObligadoContabEmisor, Recaudador, 
                NombreCaja, ValorBaseIva, Codigo_TipoIdCliente);
        }

        #region Codigo nuevo

        static public long Next_NumeroFactura(string ruc, string establecimiento, string numerodeserie)
        {
            //string sql = SqlServer.GetFormatoSQLNEXT("Numero", "Factura", string.Format("where  serie ='{0}'", numerodeserie));
            string sql = string.Format("EXEC ObtenerProximaFactura '{0}', '{1}', '{2}'", ruc, establecimiento, numerodeserie);
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        #endregion

        #region Metodo Dispose

        /// <summary>
        /// Implementaci??n de IDisposable. No se sobreescribe.
        /// </summary>
        /// 
        private Boolean disposed;
        public void Dispose()
        {
            this.Dispose(true);
            // GC.SupressFinalize quita de la cola de finalizaci??n al objeto.
            GC.SuppressFinalize(this);

        }
        /// <summary>
        /// Limpia los recursos manejados y no manejados.
        /// </summary>
        /// <param name="disposing">
        /// Si es true, el m??todo es llamado directamente o indirectamente
        /// desde el c??digo del usuario.
        /// Si es false, el m??todo es llamado por el finalizador
        /// y s??lo los recursos no manejados son finalizados.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // Preguntamos si Dispose ya fue llamado.
            if (!this.disposed)
            {
                if (disposing)
                {
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Destructor de la instancia
        ///  </summary>
        ~FacturaRecauda()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
