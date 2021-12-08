using CapaDatos.Frecuencias;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace AccesoDatos
{
    public partial class Tarifa_Viaje : SqlConnecion, IDisposable
    {
        private long _Id_CiudadOrigen;
        private Ciudad _CiudadOrigen;
        private long _Id_CiudadDestino;
        private Ciudad _CiudadDestino;
        private long _Id_TipoTarifaViaje;
        private Tipo_TarifaViaje _Tipo_TarifaViaje;
        private long _Id_Concepto_Cuenta;
        private Concepto_cuenta _Concepto_cuenta;

        #region Public Properties
        public long Id_TarifaViaje { get; set; }
        public long Id_CiudadOrigen
        {
            get { return _Id_CiudadOrigen; }
            set
            {
                _Id_CiudadOrigen = value;
                _CiudadOrigen = null;
            }
        }
        public Ciudad CiudadOrigen
        {
            get
            {
                if (_CiudadOrigen != null && _CiudadOrigen.Id_Ciudad != 0)
                    return _CiudadOrigen;
                else if (_Id_CiudadOrigen != 0)
                    return _CiudadOrigen = new Ciudad(_Id_CiudadOrigen);
                else
                    return null;
            }
        }
        public long Id_CiudadDestino
        {
            get { return _Id_CiudadDestino; }
            set
            {
                _Id_CiudadDestino = value;
                _CiudadDestino = null;
            }
        }
        public Ciudad CiudadDestino
        {
            get
            {
                if (_CiudadDestino != null && _CiudadDestino.Id_Ciudad != 0)
                    return _CiudadDestino;
                else if (_Id_CiudadDestino != 0)
                    return _CiudadDestino = new Ciudad(_Id_CiudadDestino);
                else
                    return null;
            }
        }
        public long Id_TipoTarifaViaje
        {
            get { return _Id_TipoTarifaViaje; }
            set
            {
                _Id_TipoTarifaViaje = value;
                _Tipo_TarifaViaje = null;
            }
        }
        public Tipo_TarifaViaje TipoTarifaViaje
        {
            get
            {
                if (_Tipo_TarifaViaje != null && _Tipo_TarifaViaje.Id_TipoTarifaViaje != 0)
                    return _Tipo_TarifaViaje;
                else if (_Id_TipoTarifaViaje != 0)
                    return _Tipo_TarifaViaje = new Tipo_TarifaViaje(_Id_TipoTarifaViaje);
                else
                    return null;
            }
        }
        public long Id_Concepto_Cuenta
        {
            get { return _Id_Concepto_Cuenta; }
            set
            {
                _Id_Concepto_Cuenta = value;
                _Concepto_cuenta = null;
            }
        }
        public Concepto_cuenta concepto_cuenta
        {
            get
            {
                if (_Concepto_cuenta != null && _Concepto_cuenta.Id_Concepto_Cuenta != 0)
                    return _Concepto_cuenta;
                else if (_Id_Concepto_Cuenta != 0)
                    return _Concepto_cuenta = new Concepto_cuenta(_Id_Concepto_Cuenta);
                else
                    return null;
            }
        }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Tarifa_Viaje()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Tarifa_Viaje(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_TarifaViaje = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_TarifaViaje = Convert.ToInt64(dt.Rows[0]["Id_TarifaViaje"]);
                    _Id_CiudadOrigen = Convert.ToInt64(dt.Rows[0]["Id_CiudadOrigen"]);
                    _Id_CiudadDestino = Convert.ToInt64(dt.Rows[0]["Id_CiudadDestino"]);
                    _Id_Concepto_Cuenta = Convert.ToInt64(dt.Rows[0]["Id_Concepto_Cuenta"]);
                    _Id_TipoTarifaViaje = Convert.ToInt64(dt.Rows[0]["Id_TipoTarifaViaje"]);
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
            return String.Format("SELECT Id_TarifaViaje, Id_CiudadOrigen, Id_CiudadDestino, Id_Concepto_Cuenta, Id_TipoTarifaViaje, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM TARIFA_VIAJE");
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

        public static Tarifa_Viaje GetTarifa_Viaje(long id)
        {
            return new Tarifa_Viaje(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_TarifaViaje", "TARIFA_VIAJE", "");
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
            return String.Format("INSERT INTO TARIFA_VIAJE (Id_CiudadOrigen, Id_CiudadDestino, Id_Concepto_Cuenta, Id_TipoTarifaViaje, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ({0}, {1}, {2}, {3}, GETDATE(), {4}, {5}, {6});", 
                Id_CiudadOrigen, Id_CiudadDestino, Id_Concepto_Cuenta, Id_TipoTarifaViaje, Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM TARIFA_VIAJE WHERE Id_TarifaViaje = {0};", ID);
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
            return String.Format("UPDATE TARIFA_VIAJE SET Id_CiudadOrigen = {1}, Id_CiudadDestino = {2}, Id_Concepto_Cuenta = {3}, Id_TipoTarifaViaje = {4}, " +
                "Fecha_Creacion = {5}, Usuario_Creacion = {6}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {7} WHERE Id_TarifaViaje = {0};", 
                Id_TarifaViaje, Id_CiudadOrigen, Id_CiudadDestino, Id_Concepto_Cuenta, Id_TipoTarifaViaje,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Viaje(long origen, long destino, long tarifa)
        {
            return IsDuplicate_Viaje(0, origen, destino, tarifa);
        }

        public bool IsDuplicate_Viaje(long id, long origen, long destino, long tarifa)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_TarifaViaje) FROM TARIFA_VIAJE " +
                "WHERE Id_CiudadOrigen = {0} AND Id_CiudadDestino = {1} AND Id_TipoTarifaViaje = {2} {3} ", 
                origen, destino, tarifa, id != 0 ? "AND Id_TarifaViaje <> " + id.ToString() : ""))) > 0;
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
        ~Tarifa_Viaje()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
