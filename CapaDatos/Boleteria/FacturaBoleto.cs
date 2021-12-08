using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.Boleteria
{
    public class FacturaBoleto : SqlConnecion, IDisposable
    {
        private string _Codigo_TipoIdCliente;
        private Tipo_IdCliente _Tipo_IdCliente;

        #region Public properties
        public long IdFacturaBoleto { get; set; }
        public DateTime? FechaHora { get; set; }
        public string FechaHoraSQL 
        { 
            get 
            {
                return FechaHora == null ? "" : Convert.ToDateTime(FechaHora.ToString()).ToString(SqlServer.FormatofechaHora);
            } 
        }
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
        public long IdCaja { get; set; }
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

        public FacturaBoleto(){ }

        public FacturaBoleto(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_factura_boleto = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    IdFacturaBoleto = Convert.ToInt64(dt.Rows[0]["Id_factura_boleto"]);
                    FechaHora = valorDateTime(dt.Rows[0]["Fecha_hora"]);
                    Estado = Convert.ToBoolean(dt.Rows[0]["Estado"]);
                    Cantidad = Convert.ToInt32(dt.Rows[0]["Cantidad"]);
                    IvaPorcentaje = Convert.ToDecimal(dt.Rows[0]["Iva_porcentaje"]);
                    IvaValor = Convert.ToDecimal(dt.Rows[0]["Iva_valor"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    ValorBaseIva = Convert.ToDecimal(dt.Rows[0]["ValorBaseIva"]);
                    ValorTotal = Convert.ToDecimal(dt.Rows[0]["Valor_Total"]);
                    RazonSocial = Convert.ToString(dt.Rows[0]["Razon_social"]);
                    Direccion = dt.Rows[0]["Direccion"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Direccion"]) : "";
                    Telefono = dt.Rows[0]["Telefono"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Telefono"]) : "";
                    CIRuc = Convert.ToString(dt.Rows[0]["Ci_ruc"]);
                    IdCaja = Convert.ToInt64(dt.Rows[0]["Id_Caja"]);
                    IdCierre = Convert.ToInt64(dt.Rows[0]["Id_Cierre"]);
                    IdTurno = Convert.ToInt64(dt.Rows[0]["Id_Turno"]);
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

        #region Consultas
        private static string GetSqlSelect()
        {
            return String.Format(@"SELECT Id_factura_boleto, Fecha_hora, Estado, Valor, Cantidad, Iva_porcentaje, Iva_valor, Razon_social, Direccion, Telefono, 
                Ci_ruc, Id_Caja, Id_Cierre, Id_Turno, Numero, Serie, ClaveAccesoFactElectronica, Id_EstadoFE, Id_TipoFactura, Id_TipoAmbiente, RUC_Emisor, RazonSocial_Emisor, 
                NombreComercial_Emisor, DirMatriz_Emisor, DirEstablecimiento_Emisor, CodEstablecimiento_Emisor, ResolContribEsp_Emisor, ObligadoContab_Emisor, Recaudador, NombreCaja,
                ValorBaseIva, Valor_Total, Codigo_TipoIdCliente
                FROM FACTURA_BOLETO");
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

        public static FacturaBoleto GetFacturaBoleto(long id)
        {
            return new FacturaBoleto(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_factura_boleto", "FACTURA_BOLETO", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }
        #endregion

        #region Insert Data Method
        public string Insert()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }
        public string GetSQLInsert()
        {
            return String.Format(@"INSERT INTO FACTURA_BOLETO (Fecha_hora, Estado, Valor, Cantidad, Iva_porcentaje, Iva_valor, Razon_social, Direccion, Telefono, Ci_ruc, Id_Caja, Id_Cierre, Id_Turno, Numero, Serie,
                ClaveAccesoFactElectronica, Id_EstadoFE, Id_TipoFactura, Id_TipoAmbiente, RUC_Emisor, RazonSocial_Emisor, NombreComercial_Emisor, DirMatriz_Emisor, DirEstablecimiento_Emisor, CodEstablecimiento_Emisor,
                ResolContribEsp_Emisor, ObligadoContab_Emisor, Recaudador, NombreCaja, ValorBaseIva, Codigo_TipoIdCliente) VALUES ('{0}', {1}, {2}, {3}, {4},
                {5}, '{6}', '{7}', '{8}', '{9}', {10}, {11}, {12}, {13}, {14}, '{15}', {16}, {17}, {18}, {19}, '{20}', '{21}', '{22}', '{23}', {24}, '{25}', {26}, '{27}', '{28}', {29}, '{30}');",
                (FechaHora == null) ? "null" : Convert.ToDateTime(FechaHora.ToString()).ToString(SqlServer.FormatofechaHora), Estado ? 1: 0, Valor.ToString().Replace(",", SqlServer.SigFloatSql),
                Cantidad.ToString().Replace(",", SqlServer.SigFloatSql), IvaPorcentaje.ToString().Replace(",", SqlServer.SigFloatSql), IvaValor.ToString().Replace(",", SqlServer.SigFloatSql),
                RazonSocial, Direccion, Telefono, CIRuc, IdCaja, IdCierre, IdTurno, Numero, Serie, ClaveAccesoFactElectronica, IdEstadoFE == 0 ? "NULL" : IdEstadoFE.ToString(),
                IdTipoFactura == 0 ? "NULL" : IdTipoFactura.ToString(), IdTipoAmbiente == 0 ? "NULL" : IdTipoAmbiente.ToString(), RUCEmisor, RazonSocialEmisor, NombreComercialEmisor,
                DirMatrizEmisor, DirEstablecimientoEmisor, CodEstablecimientoEmisor, ResolContribEspEmisor, ObligadoContabEmisor ? 1 : 0, Recaudador, NombreCaja, ValorBaseIva.ToString().Replace(",", SqlServer.SigFloatSql), Codigo_TipoIdCliente);
        }
        #endregion

        #region Update Data Method
        public string Update()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate());
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;

        }

        private string GetSQLUpdate()
        {
            return String.Format(@"UPDATE FACTURA_BOLETO SET Fecha_hora = '{0}', Estado = {1}, Valor = {2}, Cantidad = {3}, Iva_porcentaje = {4}, Iva_valor = {5}, Razon_social = '{6}',
                Direccion = '{7}', Telefono = '{8}', Ci_ruc = '{9}', Id_Caja = '{10}', Id_Cierre = '{11}', Id_Turno = '{12}', Numero = '{13}', Serie = '{14}', ClaveAccesoFactElectronica = '{15}',
                Id_EstadoFE = '{16}', Id_TipoFactura = '{17}', Id_TipoAmbiente = '{18}', RUC_Emisor = '{19}', RazonSocial_Emisor = '{20}', NombreComercial_Emisor = '{21}',
                DirMatriz_Emisor = '{22}', DirEstablecimiento_Emisor = '{23}', CodEstablecimiento_Emisor = '{24}', ResolContribEsp_Emisor = '{25}', ObligadoContab_Emisor = '{26}',
                Recaudador = '{27}', NombreCaja = '{28}', ValorBaseIva = {29}, Codigo_TipoIdCliente= '{30}' WHERE Id_factura_boleto = {31}",
                (FechaHora == null) ? "null" : "'" + Convert.ToDateTime(FechaHora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Estado ? 1 : 0,
                Valor.ToString().Replace(",", SqlServer.SigFloatSql), Cantidad.ToString().Replace(",", SqlServer.SigFloatSql), IvaPorcentaje.ToString().Replace(",", SqlServer.SigFloatSql),
                IvaValor.ToString().Replace(",", SqlServer.SigFloatSql), RazonSocial, Direccion, Telefono, CIRuc, IdCaja, IdCierre, IdTurno, Numero, Serie, ClaveAccesoFactElectronica,
                IdEstadoFE == 0 ? "NULL" : IdEstadoFE.ToString(), IdTipoFactura == 0 ? "NULL" : IdTipoFactura.ToString(), IdTipoAmbiente == 0 ? "NULL" : IdTipoAmbiente.ToString(), RUCEmisor,
                RazonSocialEmisor, NombreComercialEmisor, DirMatrizEmisor, DirEstablecimientoEmisor, CodEstablecimientoEmisor, ResolContribEspEmisor, ObligadoContabEmisor ? 1 : 0,
                Recaudador, NombreCaja, ValorBaseIva.ToString().Replace(",", SqlServer.SigFloatSql), Codigo_TipoIdCliente, IdFacturaBoleto);
        }
        #endregion

        #region Delete Data Method
        public string Delete(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete(long ID)
        {
            return String.Format("DELETE FROM FACTURA_BOLETO WHERE Id_factura_boleto = {0};", ID);
        }
        #endregion

        #region Funciones
        static public long Next_NumeroFactura(string ruc, string establecimiento, string numerodeserie)
        {
            //string sql = SqlServer.GetFormatoSQLNEXT("Numero", "Factura", string.Format("where  serie ='{0}'", numerodeserie));
            string sql = string.Format("EXEC ObtenerProximaFactura {0}, {1}, {2}", ruc, establecimiento, numerodeserie);
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        public DataTable Get_factura_concepto(long id_factura)
        {
            string sql = String.Format("SELECT Id_Detalle_Fact_Parqueo, Id_factura_boleto, Nombre, Valor, Cantidad, Iva, Id_Concepto_Cuenta, ((Valor * Cantidad) + Iva) Total " +
                "FROM DETALLE_FACT_BOLETO WHERE Id_factura_boleto = {0}", id_factura);
            return SqlServer.EXEC_SELECT(sql);
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
        ~FacturaBoleto()
        {
            this.Dispose(false);
        }
        #endregion

    }
}