using AccesoDatos;
using CapaDatos.AreasEquipos;
using CapaDatos.Frecuencias;
using CapaDatos.Ingresos;
using System;
using System.Data;
using System.Dynamic;

namespace CapaDatos.Transportistas
{
    public partial class Unidad_transporte : SqlConnecion, IDisposable
    {
        private long _Id_Cliente_Final;
        private long _Id_Cooperativa;
        private long _Id_Area;
        private Cliente_Final _Cliente_final;
        private Cooperativa _Cooperativa;
        private Area _Area;
        private long _Id_Tipo_Bus;
        private Tipo_Bus _Tipo_Bus;

        #region Public Properties
        public long Id_Unidad_Transporte { get; set; }
        public long Numero_Disco { get; set; }
        public string Placa { get; set; }
        public long Id_Cliente_Final
        {
            get { return _Id_Cliente_Final; }
            set
            {
                _Id_Cliente_Final = value;
                _Cliente_final = null;
            }
        }
        public long Id_Cooperativa
        {
            get { return _Id_Cooperativa; }
            set
            {
                _Id_Cooperativa = value;
                _Cooperativa = null;
            }
        }
        public long Id_Area
        {
            get { return _Id_Area; }
            set
            {
                _Id_Area = value;
                _Area = null;
            }
        }
        public long Id_Tipo_Bus
        {
            get { return _Id_Tipo_Bus; }
            set
            {
                _Id_Tipo_Bus = value;
                _Tipo_Bus = null;
            }
        }
        public Cliente_Final Cliente_final
        {
            get
            {
                if (_Cliente_final != null && _Cliente_final.Id_Cliente_Final != 0)
                    return _Cliente_final;
                else if (_Id_Cliente_Final != 0)
                    return _Cliente_final = new Cliente_Final(_Id_Cliente_Final);
                else
                    return null;
            }
        }
        public Cooperativa Coop
        {
            get
            {
                if (_Cooperativa != null && _Cooperativa.Id_Cooperativa != 0)
                    return _Cooperativa;
                else if (_Id_Cooperativa != 0)
                    return _Cooperativa = new Cooperativa(_Id_Cooperativa);
                else
                    return null;
            }
        }
        public Area area
        {
            get
            {
                if (_Area != null && _Area.Id_Area != 0)
                    return _Area;
                else if (_Id_Area != 0)
                    return _Area = new Area(_Id_Area);
                else
                    return null;
            }
        }
        public Tipo_Bus Tipo_bus
        {
            get
            {
                if (_Tipo_Bus != null && _Tipo_Bus.Id_Tipo_Bus != 0)
                    return _Tipo_Bus;
                else if (_Id_Tipo_Bus != 0)
                    return _Tipo_Bus = new Tipo_Bus(_Id_Tipo_Bus);
                else
                    return null;
            }
        }
        public bool Comprofre { get; set; }
        //public String Cooperativa_nombre
        //{
        //    get { return _Cooperativa_nombre; }
        //    set { _Cooperativa_nombre = value; }
        //}
        public String Tag { get; set; }
        public Decimal Saldo { get; set; }
        public Decimal TotalMulta { get; set; }
        public bool Activo { get; set; }
        public DateTime? UltimaLlegada { get; set; }
        public DateTime? UltimoPasoArea { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Unidad_transporte()
        {
            Comprofre = false;
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Unidad_transporte(long id)
        {
            using (DataTable dt = SqlServer.EXEC_SELECT(String.Format("SELECT u.Id_Unidad_Transporte, Numero_Disco, Placa, Id_Cliente_Final, " +
                "Id_Cooperativa, Id_Area, ISNULL(Tag, '') AS Tag, Activo, COMPROFRE, Id_Tipo_Bus, UltimaLlegada, UltimoPasoArea, " +
                "ISNULL(cp.Saldo, 0) AS Saldo, u.Fecha_Creacion, u.Usuario_Creacion, u.Fecha_Modificacion, u.Usuario_Modificacion " +
                "FROM UNIDAD_TRANSPORTE u LEFT OUTER JOIN DISPOSITIVO_RFID r ON r.Id_Unidad_Transporte = u.Id_Unidad_Transporte " +
                "LEFT OUTER JOIN CUENTA_PREPAGO_UTT cp ON cp.Id_Unidad_Transporte = u.Id_Unidad_Transporte WHERE u.Id_Unidad_Transporte = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Unidad_Transporte = Convert.ToInt64(dt.Rows[0]["Id_Unidad_Transporte"]);
                    Numero_Disco = Convert.ToInt64(dt.Rows[0]["Numero_Disco"]);
                    Placa = Convert.ToString(dt.Rows[0]["Placa"]);
                    _Id_Cliente_Final = Convert.ToInt64(dt.Rows[0]["Id_Cliente_Final"]);
                    _Id_Cooperativa = Convert.ToInt64(dt.Rows[0]["Id_Cooperativa"]);
                    _Id_Area = Convert.ToInt64(dt.Rows[0]["Id_Area"]);
                    _Id_Tipo_Bus = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Bus"]);
                    Comprofre = Convert.ToBoolean(dt.Rows[0]["COMPROFRE"]);
                    this.Tag = Convert.ToString(dt.Rows[0]["Tag"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    UltimaLlegada = valorDateTime(dt.Rows[0]["UltimaLlegada"]);
                    UltimoPasoArea = valorDateTime(dt.Rows[0]["UltimoPasoArea"]);
                    Saldo = Convert.ToDecimal(dt.Rows[0]["Saldo"]);
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
                }
            }
        }

        public Unidad_transporte(long idcoop, long disco)
        {
            using (DataTable dt = SqlServer.EXEC_SELECT(String.Format("SELECT TOP 1 u.Id_Unidad_Transporte, Numero_Disco, Placa, Id_Cliente_Final, " +
                "Id_Cooperativa, Id_Area, ISNULL(Tag, '') AS Tag, Activo, COMPROFRE, Id_Tipo_Bus, UltimaLlegada, UltimoPasoArea, " +
                "ISNULL(cp.Saldo, 0) AS Saldo, u.Fecha_Creacion, u.Usuario_Creacion, u.Fecha_Modificacion, u.Usuario_Modificacion " +
                "FROM UNIDAD_TRANSPORTE u LEFT OUTER JOIN DISPOSITIVO_RFID r ON r.Id_Unidad_Transporte = u.Id_Unidad_Transporte " +
                "LEFT OUTER JOIN CUENTA_PREPAGO_UTT cp ON cp.Id_Unidad_Transporte = u.Id_Unidad_Transporte " +
                "WHERE u.Id_Cooperativa = {0} AND u.Numero_Disco = {1} ORDER BY Id_Unidad_Transporte DESC", idcoop, disco)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Unidad_Transporte = Convert.ToInt64(dt.Rows[0]["Id_Unidad_Transporte"]);
                    Numero_Disco = Convert.ToInt64(dt.Rows[0]["Numero_Disco"]);
                    Placa = Convert.ToString(dt.Rows[0]["Placa"]);
                    _Id_Cliente_Final = Convert.ToInt64(dt.Rows[0]["Id_Cliente_Final"]);
                    _Id_Cooperativa = Convert.ToInt64(dt.Rows[0]["Id_Cooperativa"]);
                    _Id_Area = Convert.ToInt64(dt.Rows[0]["Id_Area"]);
                    _Id_Tipo_Bus = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Bus"]);
                    Comprofre = Convert.ToBoolean(dt.Rows[0]["COMPROFRE"]);
                    this.Tag = Convert.ToString(dt.Rows[0]["Tag"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    UltimaLlegada = valorDateTime(dt.Rows[0]["UltimaLlegada"]);
                    UltimoPasoArea = valorDateTime(dt.Rows[0]["UltimoPasoArea"]);
                    Saldo = Convert.ToDecimal(dt.Rows[0]["Saldo"]);
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
                }
            }
        }

        public Unidad_transporte(string placa)
        {
            using (DataTable dt = SqlServer.EXEC_SELECT(String.Format("SELECT TOP 1 u.Id_Unidad_Transporte, Numero_Disco, Placa, Id_Cliente_Final, " +
                "Id_Cooperativa, Id_Area, ISNULL(Tag, '') AS Tag, Activo, COMPROFRE, Id_Tipo_Bus, UltimaLlegada, UltimoPasoArea, " +
                "ISNULL(cp.Saldo, 0) AS Saldo, u.Fecha_Creacion, u.Usuario_Creacion, u.Fecha_Modificacion, u.Usuario_Modificacion " +
                "FROM UNIDAD_TRANSPORTE u LEFT OUTER JOIN DISPOSITIVO_RFID r ON r.Id_Unidad_Transporte = u.Id_Unidad_Transporte " +
                "LEFT OUTER JOIN CUENTA_PREPAGO_UTT cp ON cp.Id_Unidad_Transporte = u.Id_Unidad_Transporte WHERE u.Placa = {0} " +
                "ORDER BY Id_Unidad_Transporte DESC", placa)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Unidad_Transporte = Convert.ToInt64(dt.Rows[0]["Id_Unidad_Transporte"]);
                    Numero_Disco = Convert.ToInt64(dt.Rows[0]["Numero_Disco"]);
                    Placa = Convert.ToString(dt.Rows[0]["Placa"]);
                    _Id_Cliente_Final = Convert.ToInt64(dt.Rows[0]["Id_Cliente_Final"]);
                    _Id_Cooperativa = Convert.ToInt64(dt.Rows[0]["Id_Cooperativa"]);
                    _Id_Area = Convert.ToInt64(dt.Rows[0]["Id_Area"]);
                    _Id_Tipo_Bus = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Bus"]);
                    Comprofre = Convert.ToBoolean(dt.Rows[0]["COMPROFRE"]);
                    this.Tag = Convert.ToString(dt.Rows[0]["Tag"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    UltimaLlegada = valorDateTime(dt.Rows[0]["UltimaLlegada"]);
                    UltimoPasoArea = valorDateTime(dt.Rows[0]["UltimoPasoArea"]);
                    Saldo = Convert.ToDecimal(dt.Rows[0]["Saldo"]);
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
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
            return String.Format("SELECT Id_Unidad_Transporte, Numero_Disco, Placa, Id_Cliente_Final, Id_Cooperativa, Id_Area, Activo, COMPROFRE, " +
                "Id_Tipo_Bus, UltimaLlegada, UltimoPasoArea, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion " +
                "FROM UNIDAD_TRANSPORTE");
        }

        public static DataTable GetAllData(long idCooperativa, string Where = null)
        {
            string sql = String.Format(@"SELECT t.Id_Unidad_Transporte, t.Numero_Disco, t.Placa, tp.CantAsientos, 
                '#' + CAST(t.Numero_Disco AS VARCHAR) + ' (' + t.Placa + ')' AS Disco_Placa
                FROM UNIDAD_TRANSPORTE t INNER JOIN TIPO_BUS tp ON tp.Id_Tipo_Bus = t.Id_Tipo_Bus WHERE t.Id_Cooperativa = {0} {1} AND t.Activo = 1
                ORDER BY t.Numero_Disco", idCooperativa, String.IsNullOrEmpty(Where) ? "" : " AND " + Where);

            return SqlServer.EXEC_SELECT(sql);
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

        public static Unidad_transporte GetUnidad_transporte(long id)
        {
            return new Unidad_transporte(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Unidad_Transporte", "UNIDAD_TRANSPORTE", "");
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
            return String.Format("INSERT INTO UNIDAD_TRANSPORTE (Numero_Disco, Placa, Id_Cliente_Final, Id_Cooperativa, Id_Area, Activo, COMPROFRE, " +
                "Id_Tipo_Bus, UltimaLlegada, UltimoPasoArea, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ({0}, '{1}', {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, GETDATE(), {10}, {11}, {12}); ", 
                Numero_Disco, SqlServer.ValidarTexto(Placa), Id_Cliente_Final, Id_Cooperativa, Id_Area, Activo ? 1 : 0, Comprofre ? 1 : 0, Id_Tipo_Bus, 
                (UltimaLlegada == null) ? "NULL" : "'" + Convert.ToDateTime(UltimaLlegada.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                (UltimoPasoArea == null) ? "NULL" : "'" + Convert.ToDateTime(UltimoPasoArea.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
                (Fecha_Modificacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Modificacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
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
            return String.Format("DELETE FROM UNIDAD_TRANSPORTE WHERE Id_Unidad_Transporte = {0};", ID);
        }

        public string Update()
        {
            string _return = "", sql = GetSQLUpdate();

            if (!Activo)
            {
                // deshabilitar el tag asociado en la misma transacción
                Dispositivo_rfid rfid = Dispositivo_rfid.GetDispositivo_rfid_unidad(Id_Unidad_Transporte);
                if (rfid != null)
                {
                    rfid.Habilita_Entrada = rfid.Habilita_Llegada = rfid.Habilita_Salida = rfid.Habilita_Venta = false;
                    sql += rfid.GetSQLUpdate();
                }
                _return = SqlServer.EXEC_TRANSACTION(sql);
                if (_return == "OK" || _return.Contains("correctamente") && rfid != null)
                    _return = rfid.UpdateOpto();
            }
            else
                _return = SqlServer.EXEC_COMMAND(sql);

            if (_return == "OK" || _return.Contains("correctamente"))
                _return = SqlServer.MensajeDeActualizar;
            return _return;
        }

        public string GetSQLUpdate()
        {
            return String.Format("UPDATE UNIDAD_TRANSPORTE SET Numero_Disco = {1}, Placa = '{2}', Id_Cliente_Final = {3}, Id_Cooperativa = {4}, Id_Area = {5}, " +
                "Activo = {6}, COMPROFRE = {7}, Id_Tipo_Bus = {8}, UltimaLlegada = {9}, UltimoPasoArea = {10}, Fecha_Creacion = {11}, Usuario_Creacion = {12}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {13} WHERE Id_Unidad_Transporte = {0};", 
                Id_Unidad_Transporte, Numero_Disco, SqlServer.ValidarTexto(Placa), Id_Cliente_Final, Id_Cooperativa, Id_Area, Activo ? 1 : 0, Comprofre ? 1 : 0, 
                Id_Tipo_Bus, (UltimaLlegada == null) ? "NULL" : "'" + Convert.ToDateTime(UltimaLlegada.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                (UltimoPasoArea == null) ? "NULL" : "'" + Convert.ToDateTime(UltimoPasoArea.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public DataTable Get_PreEmbarque()
        {
            string sql = String.Format(@"exec UTTsEnPreoperacional");
            return SqlServer.EXEC_SELECT(sql);
        }

        public Unidad_transporte ObtenerUnidadPorTAG(string id)
        {
            using (DataTable dt = SqlServer.EXEC_SELECT(String.Format("EXEC Buscar_unidad_porTag '{0}'", id.Trim())))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Unidad_Transporte = Convert.ToInt64(dt.Rows[0]["Id_Unidad_Transporte"]);
                    Numero_Disco = Convert.ToInt64(dt.Rows[0]["Numero_Disco"]);
                    Placa = Convert.ToString(dt.Rows[0]["Placa"]);
                    _Id_Cliente_Final = Convert.ToInt64(dt.Rows[0]["Id_Cliente_Final"]);
                    _Id_Cooperativa = Convert.ToInt64(dt.Rows[0]["Id_Cooperativa"]);
                    _Id_Area = Convert.ToInt64(dt.Rows[0]["Id_Area"]);
                    _Id_Tipo_Bus = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Bus"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    Tag = dt.Rows[0]["tag"].ToString();
                    Comprofre = Convert.ToBoolean(dt.Rows[0]["COMPROFRE"].ToString());
                    Saldo = Convert.ToDecimal(dt.Rows[0]["Saldo"]);
                    TotalMulta = TotalMultas();
                    UltimaLlegada = valorDateTime(dt.Rows[0]["UltimaLlegada"]);
                    UltimoPasoArea = valorDateTime(dt.Rows[0]["UltimoPasoArea"]);
                }
            }
            return this;
        }

        public Unidad_transporte ObtenerUnidadPorId(long id)
        {
            using (DataTable dt = SqlServer.EXEC_SELECT(String.Format("EXEC Buscar_unidad_porID {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Unidad_Transporte = Convert.ToInt64(dt.Rows[0]["Id_Unidad_Transporte"]);
                    Numero_Disco = Convert.ToInt64(dt.Rows[0]["Numero_Disco"]);
                    Placa = Convert.ToString(dt.Rows[0]["Placa"]);
                    _Id_Cliente_Final = Convert.ToInt64(dt.Rows[0]["Id_Cliente_Final"]);
                    _Id_Cooperativa = Convert.ToInt64(dt.Rows[0]["Id_Cooperativa"]);
                    _Id_Area = Convert.ToInt64(dt.Rows[0]["Id_Area"]);
                    _Id_Tipo_Bus = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Bus"]);
                    Comprofre = Convert.ToBoolean(dt.Rows[0]["COMPROFRE"].ToString());
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    Tag = dt.Rows[0]["tag"].ToString();
                    Saldo = Convert.ToDecimal(dt.Rows[0]["Saldo"]);
                    TotalMulta = TotalMultas();
                    UltimaLlegada = valorDateTime(dt.Rows[0]["UltimaLlegada"]);
                    UltimoPasoArea = valorDateTime(dt.Rows[0]["UltimoPasoArea"]);
                }
            }
            return this;
        }

        public bool TieneTagAsignado(long id)
        {
            long rfid = 0;
            if (long.TryParse(SqlServer.EXEC_SCALAR("SELECT ISNULL((SELECT Id_Dispostivo_RFID FROM DISPOSITIVO_RFID WHERE Id_Unidad_Transporte = " + 
                id + "), 0)").ToString(), out rfid))
                return rfid != 0;
            else
                return false;
        }

        public decimal TotalParqueo(long id_frecuencia, long IdUnidadTransporte)
        {
            string sql = String.Format(@"exec TotalParqueo {0} ,{1}", IdUnidadTransporte, id_frecuencia);
            DataTable dt = SqlServer.EXEC_SELECT(sql);
            return Math.Round(Convert.ToDecimal(dt.Rows[0]["SumaTotal"]), 2);
        }

        public static decimal GetSaldoPrepago(long ID)
        {
            string sql = string.Format("EXEC SaldoPrepago {0} ", ID);
            return Convert.ToDecimal(SqlServer.EXEC_SCALAR(sql));
        }

        private decimal TotalMultas()
        {
            string sql = String.Format(@"exec TotalMultas {0}", Id_Unidad_Transporte);
            DataTable dt = SqlServer.EXEC_SELECT(sql);
            return Convert.ToDecimal(dt.Rows[0]["SumaTotal"]);
        }

        public bool IsDuplicate_Placa(string Cod)
        {
            return IsDuplicate_Placa(0, Cod);
        }

        public bool IsDuplicate_Placa(long id, string Cod)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Unidad_Transporte) FROM UNIDAD_TRANSPORTE " +
                "WHERE Activo = 1 AND Placa = '{0}' {1}", Cod, id != 0 ? "AND Id_Unidad_Transporte <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Disco(long Oper, long Dis)
        {
            return IsDuplicate_Disco(0, Oper, Dis);
        }

        public bool IsDuplicate_Disco(long id, long Oper, long Dis)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Unidad_Transporte) FROM UNIDAD_TRANSPORTE " +
                "WHERE Activo = 1 AND Id_Cooperativa = {0} AND Numero_Disco = '{1}' {2}", Oper, Dis, id != 0 ? "AND Id_Unidad_Transporte <> " + 
                id.ToString() : ""))) > 0;
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
        ~Unidad_transporte()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
