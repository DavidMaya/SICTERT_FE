using AccesoDatos;
using CapaDatos.Frecuencias;
using CapaDatos.Ingresos;
using Newtonsoft.Json;
using System;
using System.Data;

namespace CapaDatos.Parqueo
{
    public partial class Factura_Parqueo_D : SqlConnecion, IDisposable
    {
        private long _Id_Factura_Parqueo;
        private long _Id_Concepto_Cuenta;
        private long _Id_tipo_tarifa;
        private long _Id_tipo_tarjeta;
        private Factura_Parqueo _Factura;
        private Concepto_cuenta _Concepto_Cuenta;
        private Tipo_TarifaPP _Tipo_Tarifa;
        private Tipo_TarjetaRFID _Tipo_Tarjeta;

        #region Public Properties
        [JsonIgnore]
        public long Id_Detalle_Fact_Parqueo { get; set; }
        [JsonIgnore]
        public long Id_Factura_Parqueo
        {
            get { return _Id_Factura_Parqueo; }
            set { 
                _Id_Factura_Parqueo = value;
                _Factura = null;
            }
        }
        [JsonIgnore]
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public decimal Iva { get; set; }
        public int Cantidad { get; set; }
        [JsonIgnore]
        public string Codigo_Barra { get; set; }
        [JsonIgnore]
        public bool Es_Tarjeta { get; set; }
        [JsonIgnore]
        public bool Estado { get; set; }
        public bool Recargo { get; set; }
        public long Id_Concepto_Cuenta
        {
            get { return _Id_Concepto_Cuenta; }
            set { 
                _Id_Concepto_Cuenta = value;
                _Concepto_Cuenta = null;
            }
        }
        [JsonIgnore]
        public Concepto_cuenta Concepto_Cuenta
        {
            get
            {
                if (_Concepto_Cuenta != null && _Concepto_Cuenta.Id_Concepto_Cuenta != 0)
                    return _Concepto_Cuenta;
                else if (_Id_Concepto_Cuenta != 0)
                    return _Concepto_Cuenta = new Concepto_cuenta(_Id_Concepto_Cuenta);
                else
                    return null;
            }
        }
        [JsonIgnore]
        public Factura_Parqueo Factura
        {
            get
            {
                if (_Factura != null && _Factura.Id_Factura_Parqueo != 0)
                    return _Factura;
                else if (_Id_Factura_Parqueo != 0)
                    return _Factura = new Factura_Parqueo(_Id_Factura_Parqueo);
                else
                    return null;
            }
        }
        [JsonIgnore]
        public long Id_tipo_tarifa
        {
            get { return _Id_tipo_tarifa; }
            set { 
                _Id_tipo_tarifa = value;
                _Tipo_Tarifa = null;
            }
        }
        [JsonIgnore]
        public Tipo_TarifaPP TipoTarifa
        {
            get
            {
                if (_Tipo_Tarifa != null && _Tipo_Tarifa.Id_Tipo_Tarifa != 0)
                    return _Tipo_Tarifa;
                else if (_Id_tipo_tarifa != 0)
                    return _Tipo_Tarifa = new Tipo_TarifaPP(_Id_tipo_tarifa);
                else
                    return null;
            }
        }
        [JsonIgnore]
        public long Id_tipo_tarjeta
        {
            get { return _Id_tipo_tarjeta; }
            set { 
                _Id_tipo_tarjeta = value;
                _Tipo_Tarjeta = null;
            }
        }
        [JsonIgnore]
        public Tipo_TarjetaRFID TipoTarjeta
        {
            get
            {
                if (_Tipo_Tarjeta != null && _Tipo_Tarjeta.Id_Tipo_Tarjeta != 0)
                    return _Tipo_Tarjeta;
                else if (_Id_tipo_tarjeta != 0)
                    return _Tipo_Tarjeta = new Tipo_TarjetaRFID(_Id_tipo_tarjeta);
                else
                    return null;
            }
        }
        [JsonIgnore]
        public string NombreTipoTarifa { get; set; }
        [JsonIgnore]
        public string NombreTipoTarjeta { get; set; }
        [JsonIgnore]
        public bool TiquetePerdido { get; set; }
        #endregion

        public Factura_Parqueo_D()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Factura_Parqueo_D(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Detalle_Fact_Parqueo = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Detalle_Fact_Parqueo = Convert.ToInt64(dt.Rows[0]["Id_Detalle_Fact_Parqueo"]);
                    _Id_Factura_Parqueo = Convert.ToInt64(dt.Rows[0]["Id_Factura_Parqueo"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    Iva = Convert.ToDecimal(dt.Rows[0]["Iva"]);
                    Cantidad = Convert.ToInt32(dt.Rows[0]["Cantidad"]);
                    Codigo_Barra = dt.Rows[0]["Codigo_Barra"].ToString();
                    Es_Tarjeta = Convert.ToBoolean(dt.Rows[0]["Es_Tarjeta"]);
                    _Id_Concepto_Cuenta = Convert.ToInt64(dt.Rows[0]["Id_Concepto_Cuenta"]);
                    Recargo = Convert.ToBoolean(dt.Rows[0]["Recargo"]);
                    _Id_tipo_tarifa = dt.Rows[0]["Id_tipo_tarifa"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_tipo_tarifa"]) : 0;
                    _Id_tipo_tarjeta = dt.Rows[0]["Id_tipo_tarjeta"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_tipo_tarjeta"]) : 0;
                    TiquetePerdido = Convert.ToBoolean(dt.Rows[0]["TiquetePerdido"]);
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
            return String.Format("SELECT Id_Detalle_Fact_Parqueo, Id_Factura_Parqueo, Nombre, Valor, Codigo_Barra, Es_Tarjeta, Estado, Id_Concepto_Cuenta, " +
                "Iva, Cantidad, Recargo, Id_tipo_tarifa, Id_tipo_tarjeta, TiquetePerdido FROM DETALLE_FACT_PARQUEO");
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

        public static Factura_Parqueo_D GetFactura_Parqueo_D(long id)
        {
            return new Factura_Parqueo_D(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Detalle_Fact_Parqueo", "DETALLE_FACT_PARQUEO", "");
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
            return String.Format("INSERT INTO DETALLE_FACT_PARQUEO (Id_Factura_Parqueo, Nombre, Valor, Codigo_Barra, Es_Tarjeta, Estado, " +
                "Id_Concepto_Cuenta, Iva, Cantidad, Recargo, Id_tipo_tarifa, Id_tipo_tarjeta, TiquetePerdido) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', {10}, {11}, {12}); ", Id_Factura_Parqueo, 
                SqlServer.ValidarTexto(Nombre), Valor.ToString().Replace(",",SqlServer.SigFloatSql), Codigo_Barra, Es_Tarjeta ? "1" : "0", 
                Estado ? "1" : "0", Id_Concepto_Cuenta, Iva, Cantidad, Recargo ? "1" : "0", Id_tipo_tarifa == 0 ? "NULL" : Id_tipo_tarifa.ToString(), 
                Id_tipo_tarjeta == 0 ? "NULL" : Id_tipo_tarjeta.ToString(), TiquetePerdido ? "1" : "0");
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
            return String.Format("DELETE FROM DETALLE_FACT_PARQUEO WHERE Id_Detalle_Fact_Parqueo = {0};", ID);
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
            return String.Format("UPDATE DETALLE_FACT_PARQUEO SET Id_Factura_Parqueo= '{1}', Nombre= '{2}', Valor= '{3}', Codigo_Barra= '{4}', " +
                "Es_Tarjeta= '{5}', Estado= '{6}', Id_Concepto_Cuenta= '{7}', Iva= '{8}', Cantidad= '{9}', Recargo= '{10}', Id_tipo_tarifa= {11}, " +
                "Id_tipo_tarjeta= {12}, TiquetePerdido= {13} WHERE Id_Detalle_Fact_Parqueo = {0};", 
                Id_Detalle_Fact_Parqueo, Id_Factura_Parqueo, SqlServer.ValidarTexto(Nombre), Valor.ToString().Replace(",", SqlServer.SigFloatSql), 
                Codigo_Barra, Es_Tarjeta ? "1" : "0", Estado ? "1" : "0", Id_Concepto_Cuenta, Iva, Cantidad, Recargo ? "1" : "0", 
                Id_tipo_tarifa == 0 ? "NULL" : Id_tipo_tarifa.ToString(), Id_tipo_tarjeta == 0 ? "NULL" : Id_tipo_tarjeta.ToString(), 
                TiquetePerdido ? "1" : "0");
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
        ~Factura_Parqueo_D()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
