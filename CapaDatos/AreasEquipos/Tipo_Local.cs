using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.AreasEquipos
{
    public partial class Tipo_Local : SqlConnecion, IDisposable
    {
        private long _Id_Grupo;
        private Grupos _Grupo;
        private long _Id_Grupo_Arr;
        private Grupos _Grupo_Arr;

        #region Public Properties
        public long Id_Tipo_Local { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public long Id_Grupo
        {
            get { return _Id_Grupo; }
            set
            {
                _Id_Grupo = value;
                _Grupo = null;
            }
        }
        public Grupos Grupo
        {
            get
            {
                if (_Grupo != null && _Grupo.Id_Grupo != 0)
                    return _Grupo;
                else if (_Id_Grupo != 0)
                    return _Grupo = new Grupos(_Id_Grupo);
                else
                    return null;
            }
        }
        public long Id_Grupo_Arr
        {
            get { return _Id_Grupo_Arr; }
            set
            {
                _Id_Grupo_Arr = value;
                _Grupo_Arr = null;
            }
        }
        public Grupos Grupo_Arr
        {
            get
            {
                if (_Grupo_Arr != null && _Grupo_Arr.Id_Grupo != 0)
                    return _Grupo_Arr;
                else if (_Id_Grupo_Arr != 0)
                    return _Grupo_Arr = new Grupos(_Id_Grupo_Arr);
                else
                    return null;
            }
        }
        public bool Activo { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Tipo_Local()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Tipo_Local(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Tipo_Local = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Tipo_Local = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Local"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Codigo = Convert.ToString(dt.Rows[0]["Codigo"]);
                    _Id_Grupo = Convert.ToInt64(dt.Rows[0]["Id_Grupo"]);
                    _Id_Grupo_Arr = Convert.ToInt64(dt.Rows[0]["Id_Grupo_Arr"]);
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
            return String.Format("SELECT Id_Tipo_Local, Nombre, Codigo, Id_Grupo, Id_Grupo_Arr, Activo, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion FROM TIPO_LOCALCOMERCIAL");
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

        public static Tipo_Local GetTipo_Local(long id)
        {
            return new Tipo_Local(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Tipo_Local", "TIPO_LOCALCOMERCIAL", "");
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
            return String.Format("INSERT INTO TIPO_LOCALCOMERCIAL (Nombre, Codigo, Id_Grupo, Id_Grupo_Arr, Activo, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', GETDATE(), {5}, {6}, {7}); ", 
                SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Codigo), Id_Grupo, Id_Grupo_Arr, Activo,
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
            return String.Format("DELETE FROM TIPO_LOCALCOMERCIAL WHERE Id_Tipo_Local = {0};", ID);
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
            return String.Format("UPDATE TIPO_LOCALCOMERCIAL SET Nombre = '{1}', Codigo = '{2}', Id_Grupo = '{3}', Id_Grupo_Arr = '{4}', Activo = '{5}', " +
                "Fecha_Creacion = {6}, Usuario_Creacion = {7}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {8} WHERE Id_Tipo_Local = {0};", 
                Id_Tipo_Local, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Codigo), Id_Grupo, Id_Grupo_Arr, Activo,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Local) FROM TIPO_LOCALCOMERCIAL " +
                "WHERE Activo = 1 AND Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Tipo_Local <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Codigo(string Cod)
        {
            return IsDuplicate_Codigo(0, Cod);
        }

        public bool IsDuplicate_Codigo(long id, string Cod)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Local) FROM TIPO_LOCALCOMERCIAL " +
                "WHERE Activo = 1 AND Codigo = '{0}' {1}", Cod, id != 0 ? "AND Id_Tipo_Local <> " + id.ToString() : ""))) > 0;
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
        ~Tipo_Local()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
