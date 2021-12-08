using AccesoDatos;
using CapaDatos.UsuariosPerfiles;
using System;
using System.Data;

namespace CapaDatos.Parqueo
{
    public partial class Parqueo_Publico : SqlConnecion, IDisposable
    {
        private long _Id_Factura_Parqueo;
        private long _Id_Anterior;
        private Parqueo_Publico _PP_Anterior;
        private long _Id_tipo_tarifa;
        private Factura_Parqueo _Factura;
        private Tipo_TarifaPP _Tipo_Tarifa;
        private long _Id_Usuario_Anula;
        private Usuarios _Usuario_Anula;

        #region Public Properties
        public long Id_Parqueo_Publico { get; set; }
        public long Id_Factura_Parqueo
        {
            get { return _Id_Factura_Parqueo; }
            set
            {
                _Id_Factura_Parqueo = value;
                _Factura = null;
            }
        }
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
        public string Codigo_Barra { get; set; }
        public decimal Valor { get; set; }
        public int Estado { get; set; }
        public decimal Valor_Tmp { get; set; }
        public decimal Valor_Iva { get; set; }
        public DateTime Fecha_Ingreso { get; set; }
        public DateTime? Fecha_Salida { get; set; }
        public bool Es_Tarjeta { get; set; }
        public long Id_Anterior
        {
            get { return _Id_Anterior; }
            set { 
                _Id_Anterior = value;
                _PP_Anterior = null;
            }
        }
        public Parqueo_Publico PP_Anterior
        {
            get {
                if (_PP_Anterior != null && _PP_Anterior.Id_Parqueo_Publico != 0)
                    return _PP_Anterior;
                else if (_Id_Anterior != 0)
                    return _PP_Anterior = new Parqueo_Publico(_Id_Anterior);
                else
                    return null;
            }
        }
        public int Tiempo_Gracia { get; set; }
        public int Tiempo_Gratis { get; set; }
        public DateTime Fecha_Max_Salida { get; set; }
        public string Observacion { get; set; }
        public long Id_tipo_tarifa
        {
            get { return _Id_tipo_tarifa; }
            set
            {
                _Id_tipo_tarifa = value;
                _Tipo_Tarifa = null;
            }
        }
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
        public long Id_Usuario_Anula
        {
            get { return _Id_Usuario_Anula; }
            set
            {
                _Id_Usuario_Anula = value;
                _Usuario_Anula = null;
            }
        }
        public Usuarios Usuario_Anula
        {
            get
            {
                if (_Usuario_Anula != null && _Usuario_Anula.Id_Usuario != 0)
                    return _Usuario_Anula;
                else if (_Id_Usuario_Anula != 0)
                    return _Usuario_Anula = new Usuarios(_Id_Usuario_Anula);
                else
                    return null;
            }
        }
        public DateTime? Fecha_Anula { get; set; }
        #endregion

        public Parqueo_Publico()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Parqueo_Publico(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Parqueo_Publico = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Parqueo_Publico = Convert.ToInt64(dt.Rows[0]["Id_parqueo_publico"]);
                    Fecha_Ingreso = Convert.ToDateTime(dt.Rows[0]["Fecha_ingreso"]);
                    Fecha_Salida = dt.Rows[0]["Fecha_salida"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dt.Rows[0]["Fecha_salida"]) : null;
                    _Id_Factura_Parqueo = dt.Rows[0]["Factura"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Factura"]) : 0;
                    Valor = dt.Rows[0]["Valor"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["Valor"]) : 0;
                    Codigo_Barra = dt.Rows[0]["Codigo_barra"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Codigo_barra"]) : "";
                    Es_Tarjeta = Convert.ToBoolean(dt.Rows[0]["Es_tarjeta"]);
                    Estado = Convert.ToInt32(dt.Rows[0]["Estado"]);
                    Valor_Tmp = Convert.ToDecimal(dt.Rows[0]["Valor_tmp"]);
                    Valor_Iva = dt.Rows[0]["Valor_IVA"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["Valor_IVA"]) : 0;
                    Tiempo_Gracia = Convert.ToInt32(dt.Rows[0]["Tiempo_gracia"]);
                    Tiempo_Gratis = Convert.ToInt32(dt.Rows[0]["Tiempo_gratis"]);
                    Fecha_Max_Salida = Convert.ToDateTime(dt.Rows[0]["Fecha_max_salida"]);
                    Observacion = dt.Rows[0]["Observacion"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Observacion"]) : "";
                    _Id_Anterior = dt.Rows[0]["Id_Anterior"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Anterior"]) : 0;
                    _Id_tipo_tarifa = dt.Rows[0]["Id_tipo_tarifa"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_tipo_tarifa"]) : 0;
                    _Id_Usuario_Anula = dt.Rows[0]["Id_Usuario_Anula"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Usuario_Anula"]) : 0;
                    Fecha_Anula = valorDateTime(dt.Rows[0]["Fecha_Anula"]);
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
            return String.Format("SELECT Id_parqueo_publico, Fecha_ingreso, Fecha_salida, Factura, Valor, Codigo_barra, Es_tarjeta, Estado, Valor_tmp, " +
                "Valor_IVA, Id_Anterior, Tiempo_gracia, Tiempo_gratis, Fecha_max_salida, Observacion, Id_tipo_tarifa, Id_Usuario_Anula, Fecha_Anula " +
                "FROM PARQUEO_PUBLICO");
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

        public static Parqueo_Publico GetParqueo_Publico(long id)
        {
            return new Parqueo_Publico(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Parqueo_Publico", "PARQUEO_PUBLICO", "");
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
            return String.Format("INSERT INTO PARQUEO_PUBLICO (Fecha_ingreso, Fecha_salida, Factura, Valor, Codigo_barra, Es_tarjeta, Estado, " +
                "Valor_tmp, Valor_IVA, Id_Anterior, Tiempo_gracia, Tiempo_gratis, Fecha_max_salida, Observacion, Id_tipo_tarifa, Id_Usuario_Anula, Fecha_Anula) " +
                "VALUES ('{0}', {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, '{12}', '{13}', {14}, {15}, {16});", 
                Convert.ToDateTime(Fecha_Ingreso.ToString()).ToString(SqlServer.FormatofechaHora), 
                Fecha_Salida != null ? "'" + Convert.ToDateTime(Fecha_Salida.ToString()).ToString(SqlServer.FormatofechaHora) + "'" : "NULL", 
                Id_Factura_Parqueo == 0 ? "NULL" : Id_Factura_Parqueo.ToString(), Valor.ToString().Replace(",", SqlServer.SigFloatSql), 
                Codigo_Barra.Length == 0 ? "NULL" : "'" + Codigo_Barra + "'", Es_Tarjeta ? "1" : "0", Estado, 
                Valor_Tmp.ToString().Replace(",", SqlServer.SigFloatSql), Valor_Iva.ToString().Replace(",", SqlServer.SigFloatSql), 
                Id_Anterior != 0 ? Id_Anterior.ToString() : "NULL", Tiempo_Gracia, Tiempo_Gratis, 
                Convert.ToDateTime(Fecha_Max_Salida.ToString()).ToString(SqlServer.FormatofechaHora), SqlServer.ValidarTexto(Observacion), 
                Id_tipo_tarifa == 0 ? "NULL" : Id_tipo_tarifa.ToString(), Id_Usuario_Anula != 0 ? Id_Usuario_Anula.ToString() : "NULL",
                Fecha_Anula != null ? "'" + Convert.ToDateTime(Fecha_Anula.ToString()).ToString(SqlServer.FormatofechaHora) + "'" : "NULL");
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
            return String.Format("DELETE FROM PARQUEO_PUBLICO WHERE Id_Parqueo_Publico = {0};", ID);
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
            return String.Format("UPDATE PARQUEO_PUBLICO SET Fecha_ingreso= '{1}', Fecha_salida= {2}, Factura= {3}, Valor= '{4}', Codigo_barra= {5}, " +
                "Es_tarjeta= {6}, Estado= {7}, Valor_tmp= {8}, Valor_IVA= {9}, Id_Anterior= {10}, Tiempo_gracia= {11}, Tiempo_gratis= {12}, " +
                "Fecha_max_salida= '{13}', Observacion= '{14}', Id_tipo_tarifa= {15}, Id_Usuario_Anula = {16}, Fecha_Anula = {17} " +
                "WHERE Id_Parqueo_Publico = {0};", 
                Id_Parqueo_Publico, Convert.ToDateTime(Fecha_Ingreso.ToString()).ToString(SqlServer.FormatofechaHora), 
                Fecha_Salida != null ? "'" + Convert.ToDateTime(Fecha_Salida.ToString()).ToString(SqlServer.FormatofechaHora) + "'" : "NULL",
                Id_Factura_Parqueo == 0 ? "NULL" : Id_Factura_Parqueo.ToString(), Valor.ToString().Replace(",", SqlServer.SigFloatSql), 
                Codigo_Barra.Length == 0 ? "NULL" : "'" + Codigo_Barra + "'", Es_Tarjeta ? "1" : "0", Estado, 
                Valor_Tmp.ToString().Replace(",", SqlServer.SigFloatSql), Valor_Iva.ToString().Replace(",", SqlServer.SigFloatSql), 
                Id_Anterior != 0 ? Id_Anterior.ToString() : "NULL", Tiempo_Gracia, Tiempo_Gratis,
                Convert.ToDateTime(Fecha_Max_Salida.ToString()).ToString(SqlServer.FormatofechaHora), SqlServer.ValidarTexto(Observacion), 
                Id_tipo_tarifa == 0 ? "NULL" : Id_tipo_tarifa.ToString(), Id_Usuario_Anula != 0 ? Id_Usuario_Anula.ToString() : "NULL",
                Fecha_Anula != null ? "'" + Convert.ToDateTime(Fecha_Anula.ToString()).ToString(SqlServer.FormatofechaHora) + "'" : "NULL");
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
        ~Parqueo_Publico()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
