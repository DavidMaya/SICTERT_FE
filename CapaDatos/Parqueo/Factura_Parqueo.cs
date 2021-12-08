using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.Parqueo
{
    public partial class Factura_Parqueo : SqlConnecion, IDisposable
    {
        private long _Id_Caja;
        private long _Id_Cierre;
        private Caja _Caja;
        private Cierre_caja _CierreCaja;
        private string _Codigo_TipoIdCliente;
        private Tipo_IdCliente _Tipo_IdCliente;

        #region Public Properties
        public long Id_Factura_Parqueo { get; set; }
        public string Serie { get; set; }
        public long Numero { get; set; }
        public Nullable<DateTime> Fecha_Hora { get; set; }
        public bool Estado { get; set; }
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorBaseIva { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal Iva_Porcentaje { get; set; }
        public decimal Iva_Valor { get; set; }
        public string Razon_Social { get; set; }
        public string CI_Ruc { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public long Id_Caja
        {
            get { return _Id_Caja; }
            set
            {
                _Id_Caja = value;
                _Caja = null;
            }
        }
        public long Id_Cierre
        {
            get { return _Id_Cierre; }
            set
            {
                _Id_Cierre = value;
                _CierreCaja = null;
            }
        }
        public Caja caja
        {
            get
            {
                if (_Caja != null && _Caja.Id_Caja != 0)
                    return _Caja;
                else if (_Id_Caja != 0)
                    return _Caja = new Caja(_Id_Caja);
                else
                    return null;
            }
        }
        public Cierre_caja CierreCaja
        {
            get
            {
                if (_CierreCaja != null && _CierreCaja.Id_Cierre_Caja != 0)
                    return _CierreCaja;
                else if (_Id_Cierre != 0)
                    return _CierreCaja = new Cierre_caja(_Id_Cierre);
                else
                    return null;
            }
        }
        public string ClaveAccesoFactElectronica { get; set; }
        public int Id_EstadoFE { get; set; }
        public int Id_TipoFactura { get; set; }
        public int Id_TipoAmbiente { get; set; }
        public string RUC_Emisor { get; set; }
        public string RazonSocial_Emisor { get; set; }
        public string NombreComercial_Emisor { get; set; }
        public string DirMatriz_Emisor { get; set; }
        public string DirEstablecimiento_Emisor { get; set; }
        public string CodEstablecimiento_Emisor { get; set; }
        public string ResolContribEsp_Emisor { get; set; }
        public bool ObligadoContab_Emisor { get; set; }
        public string Recaudador { get; set; }
        public string NombreCaja { get; set; }
        public string NumeroSerie
        {
            get
            {
                return CodEstablecimiento_Emisor.ToString().PadLeft(3, '0') + "-" + Serie.ToString().PadLeft(3, '0');
            }
        }
        public string NumeroCompleto
        {
            get
            {
                return CodEstablecimiento_Emisor.ToString().PadLeft(3, '0') + "-" + Serie.ToString().PadLeft(3, '0') + "-" +
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

        public Factura_Parqueo()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Factura_Parqueo(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Factura_Parqueo = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Factura_Parqueo = Convert.ToInt64(dt.Rows[0]["Id_Factura_Parqueo"]);
                    Serie = Convert.ToString(dt.Rows[0]["Serie"]);
                    Numero = Convert.ToInt64(dt.Rows[0]["Numero"]);
                    Fecha_Hora = valorDateTime(dt.Rows[0]["Fecha_Hora"]);
                    Estado = Convert.ToBoolean(dt.Rows[0]["Estado"]);
                    Cantidad = Convert.ToInt32(dt.Rows[0]["Cantidad"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    ValorBaseIva = Convert.ToDecimal(dt.Rows[0]["ValorBaseIva"]);
                    ValorTotal = Convert.ToDecimal(dt.Rows[0]["Valor_Total"]);
                    Iva_Porcentaje = Convert.ToDecimal(dt.Rows[0]["Iva_Porcentaje"]);
                    Iva_Valor = Convert.ToDecimal(dt.Rows[0]["Iva_Valor"]);
                    Razon_Social = Convert.ToString(dt.Rows[0]["Razon_Social"]);
                    CI_Ruc = Convert.ToString(dt.Rows[0]["CI_Ruc"]);
                    Direccion = Convert.ToString(dt.Rows[0]["Direccion"]);
                    Telefono = Convert.ToString(dt.Rows[0]["Telefono"]);
                    _Id_Caja = Convert.ToInt64(dt.Rows[0]["Id_Caja"]);
                    Id_Cierre = Convert.ToInt64(dt.Rows[0]["Id_Cierre"]);
                    ClaveAccesoFactElectronica = Convert.ToString(dt.Rows[0]["ClaveAccesoFactElectronica"]);
                    Id_EstadoFE = dt.Rows[0]["Id_EstadoFE"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Id_EstadoFE"]) : 0;
                    Id_TipoFactura = Convert.ToInt32(dt.Rows[0]["Id_TipoFactura"]);
                    Id_TipoAmbiente = Convert.ToInt32(dt.Rows[0]["Id_TipoAmbiente"]);
                    RUC_Emisor = Convert.ToString(dt.Rows[0]["RUC_Emisor"]);
                    RazonSocial_Emisor = Convert.ToString(dt.Rows[0]["RazonSocial_Emisor"]);
                    NombreComercial_Emisor = Convert.ToString(dt.Rows[0]["NombreComercial_Emisor"]);
                    DirMatriz_Emisor = Convert.ToString(dt.Rows[0]["DirMatriz_Emisor"]);
                    DirEstablecimiento_Emisor = Convert.ToString(dt.Rows[0]["DirEstablecimiento_Emisor"]);
                    CodEstablecimiento_Emisor = Convert.ToString(dt.Rows[0]["CodEstablecimiento_Emisor"]);
                    ResolContribEsp_Emisor = Convert.ToString(dt.Rows[0]["ResolContribEsp_Emisor"]);
                    ObligadoContab_Emisor = Convert.ToBoolean(dt.Rows[0]["ObligadoContab_Emisor"]);
                    Recaudador = Convert.ToString(dt.Rows[0]["Recaudador"]);
                    NombreCaja = Convert.ToString(dt.Rows[0]["NombreCaja"]);
                    _Codigo_TipoIdCliente = Convert.ToString(dt.Rows[0]["Codigo_TipoIdCliente"]);
                }
            }
        }

        public decimal GetValorConceptoFactura(string codigoConcepto)
        {
            string res = Convert.ToString(SqlServer.EXEC_SCALAR("SELECT ISNULL(fc.Valor, CAST(0 AS FLOAT)) AS 'Valor' FROM DETALLE_FACT_PARQUEO fc " +
                "INNER JOIN CONCEPTO_CUENTA cc ON cc.Id_Concepto_Cuenta = fc.Id_Concepto_Cuenta WHERE Id_Factura_Parqueo = " + Id_Factura_Parqueo.ToString() + " AND cc.Codigo = '" + codigoConcepto + "'"));
            decimal valor;
            if (res != null && res.Length > 0 && decimal.TryParse(res, out valor))
                return valor;
            else
            {
                valor = 0;
                return valor;
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
            return String.Format(@"SELECT Id_Factura_Parqueo, Serie, Numero, Fecha_Hora, Estado, Valor, Iva_Porcentaje, Iva_Valor, Razon_Social, CI_Ruc, Direccion, Telefono, Id_Caja, Id_Cierre, Cantidad, 
                ClaveAccesoFactElectronica, Id_EstadoFE, Id_TipoFactura, Id_TipoAmbiente, RUC_Emisor, RazonSocial_Emisor, NombreComercial_Emisor, DirMatriz_Emisor, Codigo_TipoIdCliente, 
                DirEstablecimiento_Emisor, CodEstablecimiento_Emisor, ResolContribEsp_Emisor, ObligadoContab_Emisor, Recaudador, NombreCaja, ValorBaseIva, Valor_Total FROM FACTURA_PARQUEO");
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

        public static Factura_Parqueo GetFactura_Parqueo(long id)
        {
            return new Factura_Parqueo(id);
        }

        //static public long Next_Codigo()
        //{
        //    //string sql = SqlServer.GetFormatoSQLNEXT("Id_Factura_Parqueo", "FACTURA_PARQUEO", "");
        //    //return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        //    string sql = "EXEC ObtenerProximoID 'Id_Factura_parqueo'";
        //    return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        //}

        public string Insert()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }

        public string GetSQLInsert()
        {
            return String.Format(@"INSERT INTO FACTURA_PARQUEO (Id_Factura_Parqueo, Serie, Numero, Fecha_Hora, Estado, Valor, Iva_Porcentaje, Iva_Valor, 
                Razon_Social, CI_Ruc, Direccion, Telefono, Id_Caja, Id_Cierre, Cantidad, ClaveAccesoFactElectronica, Id_EstadoFE, Id_TipoFactura, 
                Id_TipoAmbiente, RUC_Emisor, RazonSocial_Emisor, NombreComercial_Emisor, DirMatriz_Emisor, DirEstablecimiento_Emisor, CodEstablecimiento_Emisor, 
                ResolContribEsp_Emisor, ObligadoContab_Emisor, Recaudador, NombreCaja, ValorBaseIva, Codigo_TipoIdCliente) 
                VALUES ('{0}', '{1}', '{2}', {3}, '{4}', '{5}', {6}, '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', {16}, {17}, {18}, 
                '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}'); ", 
                Id_Factura_Parqueo, Serie, Numero, 
                (Fecha_Hora == null) ? "null" : "'" + Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Estado ? 1 : 0, 
                Valor.ToString().Replace(",",SqlServer.SigFloatSql), Iva_Porcentaje, Iva_Valor.ToString().Replace(",",SqlServer.SigFloatSql), Razon_Social, 
                CI_Ruc, Direccion, Telefono, Id_Caja, Id_Cierre, Cantidad, ClaveAccesoFactElectronica, Id_EstadoFE == 0 ? "NULL" : Id_EstadoFE.ToString(), 
                Id_TipoFactura == 0 ? "NULL" : Id_TipoFactura.ToString(), Id_TipoAmbiente == 0 ? "NULL" : Id_TipoAmbiente.ToString(), RUC_Emisor, 
                RazonSocial_Emisor, NombreComercial_Emisor, DirMatriz_Emisor, DirEstablecimiento_Emisor, CodEstablecimiento_Emisor, ResolContribEsp_Emisor, 
                ObligadoContab_Emisor, Recaudador, NombreCaja, ValorBaseIva.ToString().Replace(",", SqlServer.SigFloatSql), Codigo_TipoIdCliente);
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
            return String.Format("DELETE FROM FACTURA_PARQUEO WHERE Id_Factura = {0};", ID);
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
            return String.Format(@"UPDATE FACTURA_PARQUEO SET Serie = '{1}', Numero= '{2}', Fecha_Hora= {3}, Estado= '{4}', Valor= '{5}', Iva_Porcentaje= '{6}', 
                Iva_Valor= '{7}', Razon_Social= '{8}', CI_Ruc= '{9}', Direccion= '{10}', Telefono= '{11}', Id_Caja= '{12}', Id_Cierre= '{13}', Cantidad= '{14}', 
                ClaveAccesoFactElectronica= '{15}', Id_EstadoFE= {16}, Id_TipoFactura= {17}, Id_TipoAmbiente= {18}, RUC_Emisor= '{19}', RazonSocial_Emisor= '{20}', 
                NombreComercial_Emisor= '{21}', DirMatriz_Emisor= '{22}', DirEstablecimiento_Emisor= '{23}', CodEstablecimiento_Emisor= '{24}', 
                ResolContribEsp_Emisor= '{25}', ObligadoContab_Emisor= '{26}', Recaudador= '{27}', NombreCaja= '{28}', ValorBaseIVA= '{29}', Codigo_TipoIdCliente= '{30}' 
                WHERE Id_Factura_Parqueo = {0};", Id_Factura_Parqueo, Serie, Numero, 
                (Fecha_Hora == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Estado ? 1 : 0, 
                Valor.ToString().Replace(",",SqlServer.SigFloatSql), Iva_Porcentaje, Iva_Valor.ToString().Replace(",",SqlServer.SigFloatSql), Razon_Social, CI_Ruc, 
                Direccion, Telefono, Id_Caja, Id_Cierre, Cantidad, ClaveAccesoFactElectronica, Id_EstadoFE == 0 ? "NULL" : Id_EstadoFE.ToString(), 
                Id_TipoFactura == 0 ? "NULL" : Id_TipoFactura.ToString(), Id_TipoAmbiente == 0 ? "NULL" : Id_TipoAmbiente.ToString(), RUC_Emisor, RazonSocial_Emisor, 
                NombreComercial_Emisor, DirMatriz_Emisor, DirEstablecimiento_Emisor, CodEstablecimiento_Emisor, ResolContribEsp_Emisor, ObligadoContab_Emisor, 
                Recaudador, NombreCaja, ValorBaseIva.ToString().Replace(",", SqlServer.SigFloatSql), Codigo_TipoIdCliente);
        }

        static public long Next_NumeroFactura(string ruc, string establecimiento, string numerodeserie)
        {
            //string sql = SqlServer.GetFormatoSQLNEXT("Numero", "Factura", string.Format("where  serie ='{0}'", numerodeserie));
            string sql = string.Format("EXEC ObtenerProximaFactura '{0}', '{1}', '{2}'", ruc, establecimiento, numerodeserie);
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        static public long Next_NumeroFactura(string numerodeserie)
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Numero", "Factura_Parqueo", string.Format("WHERE serie = '{0}'", numerodeserie));
            return Convert.ToInt32(SqlServer.EXEC_SCALAR(sql));
        }

        public DataTable Get_factura_concepto(long id_factura)
        {
            string sql = String.Format("SELECT Id_Detalle_Fact_Parqueo, Id_Factura, Nombre, Valor, Cantidad, Iva, Id_Concepto_Cuenta, ((Valor * Cantidad) + Iva) Total " +
                "FROM DETALLE_FACT_PARQUEO WHERE Id_Factura_Parqueo = {0}", id_factura);
            return SqlServer.EXEC_SELECT(sql);
        }

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
        ~Factura_Parqueo()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
