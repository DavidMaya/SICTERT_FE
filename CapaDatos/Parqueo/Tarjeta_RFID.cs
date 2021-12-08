using AccesoDatos;
using CapaDatos.AreasEquipos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.Parqueo
{
    public partial class Tarjeta_RFID : SqlConnecion, IDisposable
    {
        private long _Id_Cliente_Final;
        private long _Id_Factura_Parqueo;
        private long _Id_Tipo_Tarjeta;
        private long _Id_Area;
        private Factura_Parqueo _Factura;
        private Tipo_TarjetaRFID _Tipo_Tarjeta;
        private Area _Area;
        private Cliente_Final _Cliente_Final;

        #region Public Properties
        public long Id_Tarjeta_RFID { get; set; }
        public Cliente_Final Cliente_final
        {
            get
            {
                if (_Cliente_Final != null && _Cliente_Final.Id_Cliente_Final != 0)
                    return _Cliente_Final;
                else if (_Id_Cliente_Final != 0)
                    return _Cliente_Final = new Cliente_Final(_Id_Cliente_Final);
                else
                    return null;
            }
        }
        public long Id_Cliente_Final
        {
            get { return _Id_Cliente_Final; }
            set
            {
                _Id_Cliente_Final = value;
                _Cliente_Final = null;
            }
        }
        public string Id_tar { get; set; }
        public int Id_na { get; set; }
        public int Id_nt { get; set; }
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
        public DateTime Valida_Desde { get; set; }
        public DateTime Valida_Hasta { get; set; }
        public bool Anulada { get; set; }
        public long Id_Tipo_Tarjeta
        {
            get { return _Id_Tipo_Tarjeta; }
            set { 
                _Id_Tipo_Tarjeta = value;
                _Tipo_Tarjeta = null;
            }
        }
        public Tipo_TarjetaRFID TipoTarjeta
        {
            get
            {
                if (_Tipo_Tarjeta != null && _Tipo_Tarjeta.Id_Tipo_Tarjeta != 0)
                    return _Tipo_Tarjeta;
                else if (_Id_Tipo_Tarjeta != 0)
                    return _Tipo_Tarjeta = new Tipo_TarjetaRFID(_Id_Tipo_Tarjeta);
                else
                    return null;
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
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Tarjeta_RFID()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Tarjeta_RFID(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Tarjeta_RFID = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Tarjeta_RFID = Convert.ToInt64(dt.Rows[0]["Id_Tarjeta_RFID"]);
                    _Id_Cliente_Final = Convert.ToInt64(dt.Rows[0]["Id_Cliente_Final"]);
                    _Id_Factura_Parqueo = dt.Rows[0]["Id_Factura_Parqueo"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Factura_Parqueo"]) : 0;
                    Id_tar = dt.Rows[0]["Id_tar"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Id_tar"]) : "";
                    Id_na = dt.Rows[0]["Id_na"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Id_na"]) : 0;
                    Id_nt = dt.Rows[0]["Id_nt"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Id_nt"]) : 0;
                    Valida_Desde = Convert.ToDateTime(dt.Rows[0]["Valida_Desde"]);
                    Valida_Hasta = Convert.ToDateTime(dt.Rows[0]["Valida_Hasta"]);
                    Anulada = Convert.ToBoolean(dt.Rows[0]["Anulada"]);
                    _Id_Tipo_Tarjeta = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Tarjeta"]);
                    _Id_Area = dt.Rows[0]["Id_Area"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Area"]) : 0;
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
                }
            }
        }

        public Tarjeta_RFID(string codigo)
        {
            using (DataTable dt = GetAllData(String.Format("CAST(Id_tar AS VARCHAR(15)) = '{0}'", codigo)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Tarjeta_RFID = Convert.ToInt64(dt.Rows[0]["Id_Tarjeta_RFID"]);
                    _Id_Cliente_Final = Convert.ToInt64(dt.Rows[0]["Id_Cliente_Final"]);
                    _Id_Factura_Parqueo = dt.Rows[0]["Id_Factura_Parqueo"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Factura_Parqueo"]) : 0;
                    Id_tar = dt.Rows[0]["Id_tar"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Id_tar"]) : "";
                    Id_na = dt.Rows[0]["Id_na"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Id_na"]) : 0;
                    Id_nt = dt.Rows[0]["Id_nt"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Id_nt"]) : 0;
                    Valida_Desde = Convert.ToDateTime(dt.Rows[0]["Valida_Desde"]);
                    Valida_Hasta = Convert.ToDateTime(dt.Rows[0]["Valida_Hasta"]);
                    Anulada = Convert.ToBoolean(dt.Rows[0]["Anulada"]);
                    _Id_Tipo_Tarjeta = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Tarjeta"]);
                    _Id_Area = dt.Rows[0]["Id_Area"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Area"]) : 0;
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
            return String.Format("SELECT Id_Tarjeta_RFID, Id_Cliente_Final, Id_tar, Id_na, Id_nt, Id_Factura_Parqueo, Valida_Desde, Valida_Hasta, " +
                "Anulada, Id_Tipo_Tarjeta, Id_Area, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM TARJETA_RFID");
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

        public static Tarjeta_RFID GetTarjeta_RFID(long id)
        {
            return new Tarjeta_RFID(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Tarjeta_RFID", "Tarjeta_RFID", "");
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
            return String.Format("INSERT INTO TARJETA_RFID (Id_Cliente_Final, Id_tar, Id_Factura_Parqueo, Valida_Desde, Valida_Hasta, Anulada, " +
                "Id_Tipo_Tarjeta, Id_Area, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ({0}, {1}, {2}, '{3}', '{4}', '{5}', '{6}', {7}, GETDATE(), {8}, {9}, {10});", 
                Id_Cliente_Final, Id_tar.Length == 0 ? "NULL" : "'" + Id_tar + "'", Id_Factura_Parqueo == 0 ? "NULL" : Id_Factura_Parqueo.ToString(),
                Convert.ToDateTime(Valida_Desde.ToString()).ToString(SqlServer.FormatofechaHora), 
                Convert.ToDateTime(Valida_Hasta.ToString()).ToString(SqlServer.FormatofechaHora), Anulada ? "1" : "0", 
                Id_Tipo_Tarjeta, Id_Area == 0 ? "NULL" : Id_Area.ToString(), Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM TARJETA_RFID WHERE Id_Tarjeta_RFID = {0};", ID);
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
            return String.Format("UPDATE TARJETA_RFID SET  Id_Cliente_Final= '{1}', Id_tar= {2}, Id_Factura_Parqueo= {3}, Valida_Desde= '{4}', " +
                "Valida_Hasta= '{5}', Anulada= {6}, Id_Tipo_Tarjeta= '{7}', Id_Area = {8}, Fecha_Creacion = {9}, Usuario_Creacion = {10}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {11} WHERE Id_Tarjeta_RFID = {0};", 
                Id_Tarjeta_RFID, Id_Cliente_Final, Id_tar == "" ? "NULL" : "'" + Id_tar + "'", Id_Factura_Parqueo == 0 ? "NULL" : Id_Factura_Parqueo.ToString(),
                Convert.ToDateTime(Valida_Desde.ToString()).ToString(SqlServer.FormatofechaHora), 
                Convert.ToDateTime(Valida_Hasta.ToString()).ToString(SqlServer.FormatofechaHora), Anulada ? "1" : "0", 
                Id_Tipo_Tarjeta, Id_Area == 0 ? "NULL" : Id_Area.ToString(),
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
                Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
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
        ~Tarjeta_RFID()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
