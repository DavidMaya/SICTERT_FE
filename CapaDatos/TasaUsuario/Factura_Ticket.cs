using AccesoDatos;
using CapaDatos.Frecuencias;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.TasaUsuario
{
    public partial class Factura_ticket : SqlConnecion, IDisposable
    {
        private long _Id_Frecuencia;
        private Frecuencia _Frecuencia;
        private long _Id_Destino;
        private Ciudad _Destino;
        private long _Id_Caja;
        private Caja _Caja;
        private long _Id_Cierre_Caja;
        private Cierre_caja _Cierre_Caja;
        private string _Codigo_TipoIdCliente;
        private Tipo_IdCliente _Tipo_IdCliente;

        #region Public Properties
        public long Id_Factura_Ticket { get; set; }
        public Nullable<DateTime> Fecha_hora { get; set; }
        public bool Estado { get; set; }
        public double Valor { get; set; }
        public decimal ValorBaseIva { get; set; }
        public decimal ValorTotal { get; set; }
        public int Cantidad { get; set; }
        public double Iva_Porcentaje { get; set; }
        public double Iva_Valor { get; set; }
        public string Razon_social { get; set; }
        public string ci_ruc { get; set; }
        public long Id_Caja { 
            get { return _Id_Caja; }
            set {
                _Id_Caja = value;
                _Caja = null;
            } 
        }
        public long Id_Cierre {
            get { return _Id_Cierre_Caja; }
            set {
                _Id_Cierre_Caja = value;
                _Cierre_Caja = null;
            } 
        }
        public Cierre_caja Cierre_Caja
        {
            get
            {
                if (_Cierre_Caja != null && _Cierre_Caja.Id_Cierre_Caja != 0)
                    return _Cierre_Caja;
                else if (_Id_Caja != 0)
                    return _Cierre_Caja = new Cierre_caja(_Id_Cierre_Caja);
                else
                    return null;
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
        public long Id_Frecuencia {
            get { return _Id_Frecuencia; }
            set {
                _Id_Frecuencia = value;
                _Frecuencia = null;
            } 
        }
        public long Id_Destino {
            get { return _Id_Destino; }
            set {
                _Id_Destino = value;
                _Destino = null;
            } 
        }
        public long Numero { get; set; }
        public string Serie { get; set; }
        public Frecuencia frecuencia { 
            get
            {
                if (_Frecuencia != null && _Frecuencia.Id_Frecuencia != 0)
                    return _Frecuencia;
                else if (_Id_Frecuencia != 0)
                    return _Frecuencia = new Frecuencia(_Id_Frecuencia);
                else
                    return null;
            }
        }
        public Ciudad Destino
        {
            get
            {
                if (_Destino != null && _Destino.Id_Ciudad != 0)
                    return _Destino;
                else if (_Id_Destino != 0)
                    return _Destino = new Ciudad(_Id_Destino);
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
        public string Direccion { get; set; }
        public string Telefono { get; set; }
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

        public Factura_ticket()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Factura_ticket(long id)
        {
            using (DataTable dt = GetAllData(String.Format("id_factura_ticket = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Factura_Ticket = Convert.ToInt64(dt.Rows[0]["id_factura_ticket"]);
                    Serie = Convert.ToString(dt.Rows[0]["Serie"]);
                    Fecha_hora = valorDateTime(dt.Rows[0]["fecha_hora"]);
                    Estado = Convert.ToBoolean(dt.Rows[0]["estado"]);
                    Valor = Convert.ToDouble(dt.Rows[0]["valor"]);
                    ValorBaseIva = Convert.ToDecimal(dt.Rows[0]["ValorBaseIva"]);
                    ValorTotal = Convert.ToDecimal(dt.Rows[0]["Valor_Total"]);
                    Cantidad = Convert.ToInt32(dt.Rows[0]["cantidad"]);
                    Iva_Porcentaje = Convert.ToDouble(dt.Rows[0]["iva_porcentaje"]);
                    Iva_Valor = Convert.ToDouble(dt.Rows[0]["iva_valor"]);
                    Razon_social = Convert.ToString(dt.Rows[0]["razon_social"]);
                    ci_ruc = Convert.ToString(dt.Rows[0]["ci_ruc"]);
                    Id_Caja = Convert.ToInt64(dt.Rows[0]["Id_Caja"]);
                    Id_Cierre = Convert.ToInt64(dt.Rows[0]["Id_Cierre"]);
                    Id_Frecuencia = Convert.ToInt64(dt.Rows[0]["Id_Frecuencia"]);
                    Id_Destino = Convert.ToInt64(dt.Rows[0]["Id_Destino"]);
                    Numero = Convert.ToInt64(dt.Rows[0]["numero"]);
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
                    Direccion = dt.Rows[0]["Direccion"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Direccion"]) : "";
                    Telefono = dt.Rows[0]["Telefono"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Telefono"]) : "";
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
            return String.Format(@"SELECT id_factura_ticket, fecha_hora, estado, valor, cantidad, iva_porcentaje, iva_valor, razon_social, ci_ruc, Id_Caja, Id_Cierre, Id_Frecuencia, Id_Destino, numero, 
                ClaveAccesoFactElectronica, Id_EstadoFE, Id_TipoFactura, Id_TipoAmbiente, RUC_Emisor, RazonSocial_Emisor, NombreComercial_Emisor, DirMatriz_Emisor, Codigo_TipoIdCliente, 
                DirEstablecimiento_Emisor, CodEstablecimiento_Emisor, ResolContribEsp_Emisor, ObligadoContab_Emisor, Recaudador, NombreCaja, Serie, ValorBaseIva, Valor_Total FROM FACTURA_TICKET");
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

        public static Factura_ticket GetFactura_ticket(long id)
        {
            return new Factura_ticket(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("id_factura_ticket", "Factura_ticket", "");
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
            return String.Format(@"INSERT INTO FACTURA_TICKET (fecha_hora, estado, valor, cantidad, iva_porcentaje, iva_valor, razon_social, ci_ruc, 
                Id_Caja, Id_Cierre, Id_Frecuencia, Id_Destino, numero, ClaveAccesoFactElectronica, Id_EstadoFE, Id_TipoFactura, Id_TipoAmbiente, 
                RUC_Emisor, RazonSocial_Emisor, NombreComercial_Emisor, DirMatriz_Emisor, DirEstablecimiento_Emisor, CodEstablecimiento_Emisor, 
                ResolContribEsp_Emisor, ObligadoContab_Emisor, Recaudador, NombreCaja, Serie, ValorBaseIVA, Direccion, Telefono, Codigo_TipoIdCliente) 
                VALUES ('{0}', {1}, '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', {14}, {15}, {16}, '{17}', 
                '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{30}', '{31}'); ", 
                (Fecha_hora == null) ? "null" : "'" + Convert.ToDateTime(Fecha_hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                Estado ? 1 : 0, Valor.ToString().Replace(",", SqlServer.SigFloatSql), Cantidad.ToString().Replace(",", SqlServer.SigFloatSql), 
                Iva_Porcentaje.ToString().Replace(",", SqlServer.SigFloatSql), Iva_Valor.ToString().Replace(",", SqlServer.SigFloatSql), 
                Razon_social, ci_ruc, Id_Caja, Id_Cierre, Id_Frecuencia, Id_Destino, Numero, ClaveAccesoFactElectronica, 
                Id_EstadoFE == 0 ? "NULL" : Id_EstadoFE.ToString(), Id_TipoFactura == 0 ? "NULL" : Id_TipoFactura.ToString(), 
                Id_TipoAmbiente == 0 ? "NULL" : Id_TipoAmbiente.ToString(), RUC_Emisor, RazonSocial_Emisor, NombreComercial_Emisor,
                DirMatriz_Emisor, DirEstablecimiento_Emisor, CodEstablecimiento_Emisor, ResolContribEsp_Emisor, ObligadoContab_Emisor, 
                Recaudador, NombreCaja, Serie, ValorBaseIva.ToString().Replace(",", SqlServer.SigFloatSql), Direccion, Telefono, Codigo_TipoIdCliente);
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
            return String.Format("DELETE FROM FACTURA_TICKET WHERE id_factura_ticket = {0};", ID);
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
            return String.Format(@"UPDATE FACTURA_TICKET SET fecha_hora= {1}, estado= '{2}', valor= '{3}', cantidad= '{4}', iva_porcentaje= '{5}', 
                iva_valor= '{6}', razon_social= '{7}', ci_ruc= '{8}', Id_Caja= '{9}', Id_Cierre= '{10}', Id_Frecuencia= '{11}', Id_Destino= '{12}', numero= '{13}', 
                ClaveAccesoFactElectronica= '{14}', Id_EstadoFE= {15}, Id_TipoFactura= {16}, Id_TipoAmbiente= {17}, RUC_Emisor= '{18}', RazonSocial_Emisor= '{19}', 
                NombreComercial_Emisor= '{20}', DirMatriz_Emisor= '{21}', DirEstablecimiento_Emisor= '{22}', CodEstablecimiento_Emisor= '{23}', 
                ResolContribEsp_Emisor= '{24}', ObligadoContab_Emisor= '{25}', Recaudador= '{26}' NombreCaja= '{27}', Serie= '{28}', ValorBaseIVA= '{29}', 
                Direccion= '{30}', Telefono= '{31}', Codigo_TipoIdCliente= '{32}' WHERE id_factura_ticket = {0};", Id_Factura_Ticket, 
                (Fecha_hora == null) ? "null" : "'" + Convert.ToDateTime(Fecha_hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                Estado ? 1 : 0, Valor.ToString().Replace(",", SqlServer.SigFloatSql), Cantidad, Iva_Porcentaje.ToString().Replace(",", SqlServer.SigFloatSql), 
                Iva_Valor.ToString().Replace(",", SqlServer.SigFloatSql), SqlServer.ValidarTexto(Razon_social), SqlServer.ValidarTexto(ci_ruc), Id_Caja, Id_Cierre, 
                Id_Frecuencia, Id_Destino, Numero, ClaveAccesoFactElectronica, Id_EstadoFE == 0 ? "NULL" : Id_EstadoFE.ToString(), 
                Id_TipoFactura == 0 ? "NULL" : Id_TipoFactura.ToString(), Id_TipoAmbiente == 0 ? "NULL" : Id_TipoAmbiente.ToString(), 
                RUC_Emisor, RazonSocial_Emisor, NombreComercial_Emisor, DirMatriz_Emisor, DirEstablecimiento_Emisor, CodEstablecimiento_Emisor, 
                ResolContribEsp_Emisor, ObligadoContab_Emisor, Recaudador, NombreCaja, Serie, ValorBaseIva.ToString().Replace(",", SqlServer.SigFloatSql),
                Direccion, Telefono, Codigo_TipoIdCliente);
        }

        static public long Next_NumeroTicket(string numerodeserie)
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Numero", "factura_ticket", string.Format("where  serie ='{0}'", numerodeserie));
            return Convert.ToInt32(SqlServer.EXEC_SCALAR(sql));
        }

        #region Codigo nuevo

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
        ~Factura_ticket()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
