using AccesoDatos;
using CapaDatos.AreasEquipos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.AreasEquipos
{
    public partial class TarjetaAcceso : SqlConnecion, IDisposable
    {
        private string _Nombre;
        private long _Id_Area;
        private Area _Area;

        #region Public Properties
        public long Id_Tarjeta { get; set; }
        public string Serial { get; set; }
        public string Cedula { get; set; }
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value.ToUpper(); }
        }
        public DateTime Valida_Desde { get; set; }
        public DateTime Valida_Hasta { get; set; }
        public bool Activo { get; set; }
        public bool Seleccion { get; set; }
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

        public TarjetaAcceso()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public TarjetaAcceso(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Tarjeta = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Tarjeta = Convert.ToInt64(dt.Rows[0]["Id_Tarjeta"]);
                    Serial = Convert.ToString(dt.Rows[0]["Serial"]);
                    Cedula = Convert.ToString(dt.Rows[0]["Cedula"]);
                    _Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Valida_Desde = Convert.ToDateTime(dt.Rows[0]["Valida_Desde"]);
                    Valida_Hasta = Convert.ToDateTime(dt.Rows[0]["Valida_Hasta"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
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
            return String.Format("SELECT Id_Tarjeta, Serial, Cedula, Nombre, Valida_Desde, Valida_Hasta, Activo, Id_Area, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM TARJETA_ACCESO");
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

        public static TarjetaAcceso GetTarjeta_Acceso(long id)
        {
            return new TarjetaAcceso(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Tarjeta", "TARJETA_ACCESO", "");
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
            return String.Format("INSERT INTO TARJETA_ACCESO (Serial, Cedula, Nombre, Valida_Desde, Valida_Hasta, Activo, Id_Area, Fecha_Creacion, " +
                "Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, GETDATE(), {7}, {8}, {9});", 
                SqlServer.ValidarTexto(Serial), SqlServer.ValidarTexto(Cedula), SqlServer.ValidarTexto(Nombre),
                Convert.ToDateTime(Valida_Desde.ToString()).ToString(SqlServer.FormatofechaInicio), 
                Convert.ToDateTime(Valida_Hasta.ToString()).ToString(SqlServer.FormatofechaFin), Activo ? "1" : "0", 
                _Id_Area == 0 ? "NULL" : _Id_Area.ToString(), Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM TARJETA_ACCESO WHERE Id_Tarjeta = {0};", ID);
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
            return String.Format("UPDATE TARJETA_Acceso SET Serial= '{1}', Cedula= '{2}', Nombre= '{3}', Valida_Desde= '{4}', Valida_Hasta= '{5}', " +
                "Activo= '{6}', Id_Area = {7}, Fecha_Creacion = {8}, Usuario_Creacion = {9}, Fecha_Modificacion = GETDATE(), " +
                "Usuario_Modificacion = {10} WHERE Id_Tarjeta = {0};", 
                Id_Tarjeta, SqlServer.ValidarTexto(Serial), SqlServer.ValidarTexto(Cedula), SqlServer.ValidarTexto(Nombre), 
                Convert.ToDateTime(Valida_Desde.ToString()).ToString(SqlServer.FormatofechaInicio), 
                Convert.ToDateTime(Valida_Hasta.ToString()).ToString(SqlServer.FormatofechaFin), Activo ? "1" : "0", 
                _Id_Area == 0 ? "NULL" : _Id_Area.ToString(),
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Serial(string Ser)
        {
            return IsDuplicate_Serial(0, Ser);
        }

        public bool IsDuplicate_Serial(long id, string Ser)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tarjeta) FROM TARJETA_ACCESO WHERE Activo = 1 AND Serial = '{0}' {1}", Ser, id != 0 ? "AND Id_Tarjeta <> " + id.ToString() : ""))) > 0;
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
        ~TarjetaAcceso()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
