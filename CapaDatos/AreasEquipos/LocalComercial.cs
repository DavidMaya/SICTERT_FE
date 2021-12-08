using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.AreasEquipos
{
    public partial class LocalComercial : SqlConnecion, IDisposable
    {
        private long _Id_Area;
        private Area _Area;
        private long _Id_Tipo_Local;
        private Tipo_Local _Tipo_Local;
        private long _Id_Cliente_Final;
        private Cliente_Final _Cliente_Final;

        #region Public Properties
        public long Id_Local { get; set; }
        public string Nombre { get; set; }
        public string Numero { get; set; }
        public long Id_Area
        {
            get { return _Id_Area; }
            set
            {
                _Id_Area = value;
                _Area = null;
            }
        }
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
        public long Id_Tipo_Local
        {
            get { return _Id_Tipo_Local; }
            set
            {
                _Id_Tipo_Local = value;
                _Tipo_Local = null;
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
        public Tipo_Local Tipo_local
        {
            get
            {
                if (_Tipo_Local != null && _Tipo_Local.Id_Tipo_Local != 0)
                    return _Tipo_Local;
                else if (_Id_Tipo_Local != 0)
                    return _Tipo_Local = new Tipo_Local(_Id_Tipo_Local);
                else
                    return null;
            }
        }
        public DateTime? Fecha_Desde { get; set; }
        public DateTime? Fecha_Hasta { get; set; }
        public bool Activo { get; set; }
        public long Id_Grupo_Arr { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public LocalComercial()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public LocalComercial(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Local = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Local = Convert.ToInt64(dt.Rows[0]["Id_Local"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Numero = Convert.ToString(dt.Rows[0]["Numero"]);
                    _Id_Area = Convert.ToInt64(dt.Rows[0]["Id_Area"]);
                    _Id_Tipo_Local = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Local"]);
                    _Id_Cliente_Final = Convert.ToInt64(dt.Rows[0]["Id_Cliente_Final"]);
                    Fecha_Desde = valorDateTime(dt.Rows[0]["Fecha_Desde"]);
                    Fecha_Hasta = valorDateTime(dt.Rows[0]["Fecha_Hasta"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
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
            return String.Format("SELECT Id_Local, Nombre, Numero, Id_Area, Id_Tipo_Local, Id_Cliente_Final, Fecha_Desde, Fecha_Hasta, " +
                "Activo, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM LOCAL_COMERCIAL");
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

        public static LocalComercial GetLocalComercial(long id)
        {
            return new LocalComercial(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Local", "LOCAL_COMERCIAL", "");
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
            return String.Format("INSERT INTO LOCAL_COMERCIAL (Nombre, Numero, Id_Area, Id_Tipo_Local, Id_Cliente_Final, Fecha_Desde, Fecha_Hasta, Activo, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, '{7}', GETDATE(), {8}, {9}, {10}); ", 
                SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Numero), Id_Area, Id_Tipo_Local, Id_Cliente_Final,
                (Fecha_Desde == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Desde.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                (Fecha_Hasta == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hasta.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Activo,
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
            return String.Format("DELETE FROM LOCAL_COMERCIAL WHERE Id_Local = {0};", ID);
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
            return String.Format("UPDATE LOCAL_COMERCIAL SET Nombre = '{1}', Numero = '{2}', Id_Area = '{3}', Id_Tipo_Local = '{4}', " +
                "Id_Cliente_Final = '{5}', Fecha_Desde = {6}, Fecha_Hasta = {7}, Activo = '{8}', Fecha_Creacion = {9}, Usuario_Creacion = {10}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {11} WHERE Id_Local = {0};", 
                Id_Local, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Numero), Id_Area, Id_Tipo_Local, Id_Cliente_Final,
                (Fecha_Desde == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Desde.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                (Fecha_Hasta == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hasta.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Activo,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Local) FROM LOCAL_COMERCIAL WHERE Activo = 1 AND Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Local <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Numero(string Num)
        {
            return IsDuplicate_Numero(0, Num);
        }

        public bool IsDuplicate_Numero(long id, string Num)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Local) FROM LOCAL_COMERCIAL WHERE Activo = 1 AND Numero = '{0}' {1}", Num, id != 0 ? "AND Id_Local <> " + id.ToString() : ""))) > 0;
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
        ~LocalComercial()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
