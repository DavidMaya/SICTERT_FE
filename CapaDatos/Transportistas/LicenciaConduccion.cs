using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Transportistas
{
    public partial class LicenciaConduccion : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long Id_Licencia { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public bool EsProfesional { get; set; }
        public bool Activo { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public LicenciaConduccion()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public LicenciaConduccion(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Licencia = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Licencia = Convert.ToInt64(dt.Rows[0]["Id_Licencia"]);
                    Tipo = Convert.ToString(dt.Rows[0]["Tipo"]);
                    Descripcion = Convert.ToString(dt.Rows[0]["Descripcion"]);
                    EsProfesional = Convert.ToBoolean(dt.Rows[0]["EsProfesional"]);
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
            return String.Format("SELECT Id_Licencia, Tipo, Descripcion, EsProfesional, Activo, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion FROM LICENCIA_CONDUCCION");
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

        public static LicenciaConduccion GetLicenciaConduccion(long id)
        {
            return new LicenciaConduccion(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Licencia", "LICENCIA_CONDUCCION", "");
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
            return String.Format("INSERT INTO LICENCIA_CONDUCCION (Tipo, Descripcion, EsProfesional, Activo, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion) VALUES ('{0}', '{1}', '{2}', '{3}', GETDATE(), {4}, {5}, {6}); ", 
                SqlServer.ValidarTexto(Tipo), SqlServer.ValidarTexto(Descripcion), EsProfesional, Activo,
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
            return String.Format("DELETE FROM LICENCIA_CONDUCCION WHERE Id_Licencia = {0};", ID);
        }

        public string Update()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate());
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;
        }

        public bool IsDuplicate_Tipo(string Num)
        {
            return IsDuplicate_Tipo(0, Num);
        }

        public string GetSQLUpdate()
        {
            return String.Format("UPDATE LICENCIA_CONDUCCION SET Tipo = '{1}', Descripcion = '{2}', EsProfesional = '{3}', Activo = '{4}', " +
                "Fecha_Creacion = {5}, Usuario_Creacion = {6}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {7} WHERE Id_Licencia = {0};", 
                Id_Licencia, SqlServer.ValidarTexto(Tipo), SqlServer.ValidarTexto(Descripcion), EsProfesional, Activo,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Tipo(long id, string Num)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Licencia) FROM LICENCIA_CONDUCCION " +
                "WHERE Activo = 1 AND Tipo = '{0}' {1}", Num, id != 0 ? "AND Id_Licencia <> " + id.ToString() : ""))) > 0;
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
        ~LicenciaConduccion()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
