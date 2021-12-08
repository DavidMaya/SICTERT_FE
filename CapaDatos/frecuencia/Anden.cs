using AccesoDatos;
using CapaDatos.AreasEquipos;
using System;
using System.Data;

namespace CapaDatos.Frecuencias
{
    public partial class Anden : SqlConnecion, IDisposable
    {
        private long _Id_Ciudad;
        private long _Id_Area;
        private Ciudad _Ciudad;
        private Area _Area;

        #region Public Properties
        public long Id_Anden { get; set; }
        public string Numero { get; set; }
        public long Id_Ciudad
        {
            get { return _Id_Ciudad; }
            set { 
                _Id_Ciudad = value;
                _Ciudad = null;
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
        public Ciudad ciudad
        {
            get
            {
                if (_Ciudad != null && _Ciudad.Id_Ciudad != 0)
                    return _Ciudad;
                else if (_Id_Ciudad != 0)
                    return _Ciudad = new Ciudad(_Id_Ciudad);
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
        public bool Activo { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Anden()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Anden(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Anden = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Anden = Convert.ToInt64(dt.Rows[0]["Id_Anden"]);
                    Numero = Convert.ToString(dt.Rows[0]["Numero"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    _Id_Ciudad = Convert.ToInt64(dt.Rows[0]["Id_Ciudad"]);
                    _Id_Area = Convert.ToInt64(dt.Rows[0]["Id_Area"]);
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
                }
            }
        }

        public Anden(long id, bool ciudad)
        {
            using (DataTable dt = GetAllData(String.Format("{0} = {1} AND Activo = 1", ciudad ? "Id_Ciudad" : "Id_Anden", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Anden = Convert.ToInt64(dt.Rows[0]["Id_Anden"]);
                    Numero = Convert.ToString(dt.Rows[0]["Numero"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    _Id_Ciudad = Convert.ToInt64(dt.Rows[0]["Id_Ciudad"]);
                    _Id_Area = Convert.ToInt64(dt.Rows[0]["Id_Area"]);
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
            return String.Format("SELECT Id_Anden, Numero, Id_Ciudad, Id_Area, Activo, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM ANDEN");
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

        public static Anden GetAnden(long id)
        {
            return new Anden(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Anden", "Anden", "");
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
            return String.Format("INSERT INTO Anden (Numero, Id_Ciudad, Id_Area, Activo, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', GETDATE(), {4}, {5}, {6}); ", 
                SqlServer.ValidarTexto(Numero), Id_Ciudad, Id_Area, Activo, Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM ANDEN WHERE Id_Anden = {0};", ID);
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
            return String.Format("UPDATE ANDEN SET Numero = '{1}', Activo = '{2}', Id_Ciudad = '{3}', Id_Area = '{4}', Fecha_Creacion = {5}, Usuario_Creacion = {6}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {7} WHERE Id_Anden = {0};", 
                Id_Anden, SqlServer.ValidarTexto(Numero), Activo, Id_Ciudad, Id_Area,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Numero(string Num)
        {
            return IsDuplicate_Numero(0, Num);
        }

        public bool IsDuplicate_Numero(long id, string Num)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Anden) FROM ANDEN WHERE Activo = 1 AND Numero = '{0}' {1}", 
                Num, id != 0 ? "AND Id_Anden <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Destino(long Dest)
        {
            return IsDuplicate_Destino(0, Dest);
        }

        public bool IsDuplicate_Destino(long id, long Dest)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Anden) FROM ANDEN WHERE Activo = 1 AND Id_Ciudad = {0} {1}", 
                Dest, id != 0 ? "AND Id_Anden <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_NumeroDestino(string Num, long Dest)
        {
            return IsDuplicate_NumeroDestino(0, Num, Dest);
        }

        public bool IsDuplicate_NumeroDestino(long id, string Num, long Dest)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Anden) FROM ANDEN WHERE Activo = 1 AND Numero = '{0}' AND Id_Ciudad = {1} {2}", 
                Num, Dest, id != 0 ? "AND Id_Anden <> " + id.ToString() : ""))) > 0;
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
        ~Anden()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
