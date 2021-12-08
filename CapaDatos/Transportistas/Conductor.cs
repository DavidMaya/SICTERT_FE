using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Transportistas
{
    public partial class Conductor : SqlConnecion, IDisposable
    {
        private int _Id_na; 
        private int _Id_nt;
        private long _Id_Licencia;
        private LicenciaConduccion _Licencia;

        #region Public Properties
        public long Id_Chofer { get; set; }
        public string Nombre
        {
            get { return Nombre1; }
            set { Nombre1 = value.ToUpper(); }
        }
        public string Cedula { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Id_tar { get; set; } // tarjeta: produccion = area * 65536 + numero
        public long Id_Licencia {
            get { return _Id_Licencia; }
            set {
                _Id_Licencia = value;
                _Licencia = null;
            } 
        }
        public LicenciaConduccion Licencia
        {
            get {
                if (_Licencia != null && _Licencia.Id_Licencia != 0)
                    return _Licencia;
                else if (_Id_Licencia != 0)
                    return _Licencia = new LicenciaConduccion(_Id_Licencia);
                else
                    return null;
            }
        }
        public int Id_na // area = tarjeta / 65536
        {
            get { return _Id_na; }
        }
        public int Id_nt // numero = tarjeta % 65536
        {
            get { return _Id_nt; }
        }
        public string Email { get; set; }
        public bool Activo { get; set; }

        public string Nombre1 { get; set; }
        public DateTime? FechaComprobacionDinardap { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Conductor()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Conductor(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Chofer = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Chofer = Convert.ToInt64(dt.Rows[0]["Id_Chofer"]);
                    Nombre1 = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Cedula = Convert.ToString(dt.Rows[0]["Cedula"]);
                    Direccion = Convert.ToString(dt.Rows[0]["Direccion"]);
                    Telefono = Convert.ToString(dt.Rows[0]["Telefono"]);
                    Id_tar = Convert.ToString(dt.Rows[0]["Id_tar"]);
                    Id_Licencia = Convert.ToInt64(dt.Rows[0]["Id_Licencia"]);
                    _Id_na = Convert.ToInt32(dt.Rows[0]["Id_na"]);
                    _Id_nt = Convert.ToInt32(dt.Rows[0]["Id_nt"]);
                    Email = (dt.Rows[0]["Email"]) == null ? "" : Convert.ToString(dt.Rows[0]["Email"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    FechaComprobacionDinardap = valorDateTime(dt.Rows[0]["FechaComprobacionDinardap"]);
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
                }
            }
        }

        public Conductor(string ci)
        {
            using (DataTable dt = GetAllData(String.Format("Cedula = '{0}'", ci)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Chofer = Convert.ToInt64(dt.Rows[0]["Id_Chofer"]);
                    Nombre1 = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Cedula = Convert.ToString(dt.Rows[0]["Cedula"]);
                    Direccion = Convert.ToString(dt.Rows[0]["Direccion"]);
                    Telefono = Convert.ToString(dt.Rows[0]["Telefono"]);
                    Id_tar = Convert.ToString(dt.Rows[0]["Id_tar"]);
                    Id_Licencia = Convert.ToInt64(dt.Rows[0]["Id_Licencia"]);
                    _Id_na = Convert.ToInt32(dt.Rows[0]["Id_na"]);
                    _Id_nt = Convert.ToInt32(dt.Rows[0]["Id_nt"]);
                    Email = (dt.Rows[0]["Email"]) == null ? "" : Convert.ToString(dt.Rows[0]["Email"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    FechaComprobacionDinardap = valorDateTime(dt.Rows[0]["FechaComprobacionDinardap"]);
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
            return String.Format("SELECT Id_Chofer, Nombre, Cedula, Direccion, Telefono, Id_tar, Id_Licencia, Id_na, Id_nt, Email, Activo, " +
                "FechaComprobacionDinardap, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM CONDUCTOR");
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

        public static Conductor GetConductor(long id)
        {
            return new Conductor(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Chofer", "CONDUCTOR", "");
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
            return String.Format("INSERT INTO CONDUCTOR (Nombre, Cedula, Direccion, Telefono, Id_tar, Id_Licencia, Email, Activo, FechaComprobacionDinardap, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', {8}, GETDATE(), {9}, {10}, {11}); ", 
                SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Cedula), SqlServer.ValidarTexto(Direccion), SqlServer.ValidarTexto(Telefono), 
                SqlServer.ValidarTexto(Id_tar), Id_Licencia, SqlServer.ValidarTexto(Email), Activo,
                (FechaComprobacionDinardap == null) ? "NULL" : "'" + Convert.ToDateTime(FechaComprobacionDinardap.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
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
            return String.Format("DELETE FROM CONDUCTOR WHERE Id_Chofer = {0};", ID);
        }

        public string Update()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate());
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;
        }

        public string GetSQLUpdate()
        {//quitar la ultima coma
            return String.Format("UPDATE CONDUCTOR SET Nombre = '{1}', Cedula = '{2}', Direccion = '{3}', Telefono = '{4}', Id_tar = '{5}', " +
                "Id_Licencia = '{6}', Email = '{7}', Activo = '{8}', FechaComprobacionDinardap = {9}, Fecha_Creacion = {10}, Usuario_Creacion = {11}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {12} WHERE Id_Chofer = {0};", 
                Id_Chofer, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Cedula), SqlServer.ValidarTexto(Direccion), SqlServer.ValidarTexto(Telefono), 
                SqlServer.ValidarTexto(Id_tar), Id_Licencia, SqlServer.ValidarTexto(Email), Activo,
                (FechaComprobacionDinardap == null) ? "NULL" : "'" + Convert.ToDateTime(FechaComprobacionDinardap.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Cedula(string sCed)
        {
            return IsDuplicate_Cedula(0, sCed);
        }

        public bool IsDuplicate_Cedula(long id, string sCed)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Chofer) FROM CONDUCTOR WHERE Cedula = '{0}' {1}", sCed, id != 0 ? "AND Id_Chofer <> " + id.ToString() : ""))) > 0;
        }

        public long ObtenerId(string ced)
        {
            try
            {
                return Convert.ToInt64(SqlServer.EXEC_SCALAR(string.Format("SELECT Id_Chofer FROM CONDUCTOR WHERE Cedula = '{0}'", ced)));
            }
            catch
            {
                return 0;
            }
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
        ~Conductor()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
