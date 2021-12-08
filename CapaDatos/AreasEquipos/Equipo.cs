using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.AreasEquipos
{
    public partial class Equipo : SqlConnecion, IDisposable
    {
        private long _Id_Area;
        private long _Id_Tipo_Equipo;
        private Area _Area;
        private Tipo_equipo _Tipo_equipo;

        #region Public Properties
        public long Id_Equipo { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public long Id_Area
        {
            get { return _Id_Area; }
            set
            {
                _Id_Area = value;
                _Area = null;
            }
        }
        public string IP { get; set; }
        public string Codigo { get; set; }
        public long Id_Tipo_Equipo
        {
            get { return _Id_Tipo_Equipo; }
            set { 
                _Id_Tipo_Equipo = value;
                _Tipo_equipo = null;
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
        public Tipo_equipo tipo_equipo
        {
            get {
                if (_Tipo_equipo != null && _Tipo_equipo.Id_Tipo_Equipo != 0)
                    return _Tipo_equipo;
                else if (_Id_Tipo_Equipo != 0)
                    return _Tipo_equipo = new Tipo_equipo(_Id_Tipo_Equipo);
                else
                    return null;
            }
        }
        public bool Activo { get; set; }
        public string URLAcceso { get; set; }
        public string TablaOpto { get; set; }
        public string Puerta { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Equipo()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Equipo(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Equipo = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Equipo = Convert.ToInt64(dt.Rows[0]["Id_Equipo"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Ubicacion = Convert.ToString(dt.Rows[0]["Ubicacion"]);
                    _Id_Area = Convert.ToInt64(dt.Rows[0]["Id_Area"]);
                    IP = Convert.ToString(dt.Rows[0]["IP"]);
                    _Id_Tipo_Equipo = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Equipo"]);
                    Codigo = Convert.ToString(dt.Rows[0]["Codigo"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    URLAcceso = dt.Rows[0]["URLAcceso"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["URLAcceso"]) : "";
                    TablaOpto = dt.Rows[0]["TablaOpto"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["TablaOpto"]) : "";
                    Puerta = dt.Rows[0]["Puerta"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Puerta"]) : "";
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
            return String.Format("SELECT Id_Equipo, Nombre, Ubicacion, Id_Area, IP, Id_Tipo_Equipo, Codigo, Activo, URLAcceso, TablaOpto, " +
                "Puerta, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM EQUIPO");
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

        public static Equipo GetEquipo(long id)
        {
            return new Equipo(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Equipo", "EQUIPO", "");
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
            return String.Format("INSERT INTO EQUIPO (Nombre, Ubicacion, Id_Area, IP, Id_Tipo_Equipo, Codigo, Activo, URLAcceso, TablaOpto, Puerta, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', GETDATE(), {10}, {11}, {12}); ", 
                SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Ubicacion), Id_Area, SqlServer.ValidarTexto(IP), Id_Tipo_Equipo, 
                SqlServer.ValidarTexto(Codigo), Activo, SqlServer.ValidarTexto(URLAcceso), TablaOpto, Puerta,
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
            return String.Format("DELETE FROM EQUIPO WHERE Id_Equipo = {0};", ID);
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
            return String.Format("UPDATE EQUIPO SET Nombre = '{1}', Ubicacion = '{2}', Id_Area = '{3}', IP = '{4}', Id_Tipo_Equipo = '{5}', " +
                "Codigo = '{6}', Activo = '{7}', URLAcceso = '{8}', TablaOpto = '{9}', Puerta = '{10}', Fecha_Creacion = {11}, Usuario_Creacion = {12}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {13} WHERE Id_Equipo = {0};", 
                Id_Equipo, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Ubicacion), Id_Area, SqlServer.ValidarTexto(IP), 
                Id_Tipo_Equipo, SqlServer.ValidarTexto(Codigo), Activo, SqlServer.ValidarTexto(URLAcceso), TablaOpto, Puerta,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public string quitarImpresoras(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(string.Format("DELETE FROM IMPRESORA WHERE id_equipo = {0}", id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Equipo) FROM EQUIPO WHERE Activo = 1 AND Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Equipo <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Codigo(string Cod)
        {
            return IsDuplicate_Codigo(0, Cod);
        }

        public bool IsDuplicate_Codigo(long id, string Cod)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Equipo) FROM EQUIPO WHERE Activo = 1 AND Codigo = '{0}' {1}", Cod, id != 0 ? "AND Id_Equipo <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_IP(string DirIP)
        {
            return IsDuplicate_IP(0, DirIP);
        }

        public bool IsDuplicate_IP(long id, string DirIP)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Equipo) FROM EQUIPO WHERE Activo = 1 AND IP = '{0}' {1}", DirIP, id != 0 ? "AND Id_Equipo <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Url(string Url)
        {
            return IsDuplicate_Url(0, Url);
        }

        public bool IsDuplicate_Url(long id, string Url)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Equipo) FROM EQUIPO WHERE Activo = 1 AND URLAcceso = '{0}' {1}", Url, id != 0 ? "AND Id_Equipo <> " + id.ToString() : ""))) > 0;
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
        ~Equipo()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
