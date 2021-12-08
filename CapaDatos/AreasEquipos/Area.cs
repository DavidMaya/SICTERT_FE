using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.AreasEquipos
{
    public partial class Area : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long Id_Area { get; set; }
        public string Nombre { get; set; }
        public string Numero { get; set; }
        public bool AreaUTT { get; set; }
        public bool Activo { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Area()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Area(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Area = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Area = Convert.ToInt64(dt.Rows[0]["Id_Area"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Numero = Convert.ToString(dt.Rows[0]["Numero"]);
                    AreaUTT = Convert.ToBoolean(dt.Rows[0]["AreaUTT"]);
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
            return String.Format("SELECT Id_Area, Nombre, Numero, AreaUTT, Activo, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion FROM AREA");
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

        public static Area GetArea(long id)
        {
            return new Area(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Area", "AREA", "");
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
            return String.Format("INSERT INTO AREA (Nombre, Numero, AreaUTT, Activo, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "OUTPUT INSERTED.Id_Area VALUES( '{0}', '{1}', '{2}', '{3}', GETDATE(), {4}, {5}, {6}); ", 
                SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Numero), AreaUTT, Activo, Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
                (Fecha_Modificacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Modificacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public string Delete()
        {
            return Delete(Id_Area);
        }

        static public string Delete(long ID)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(ID));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete()
        {
            return GetSQLDelete(Id_Area);
        }

        static public string GetSQLDelete(long ID)
        {
            return String.Format("DELETE FROM AREA WHERE Id_Area = {0};", ID);
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
            return String.Format("UPDATE AREA SET Nombre = '{1}', Numero = '{2}', AreaUTT = '{3}', Activo = '{4}', Fecha_Creacion = {5}, " +
                "Usuario_Creacion = {6}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {7} WHERE Id_Area = {0};", 
                Id_Area, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Numero), AreaUTT, Activo,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Codigo(string Cod)
        {
            return IsDuplicate_Codigo(0, Cod);
        }

        public bool IsDuplicate_Codigo(long id, string Cod)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Area) FROM AREA WHERE Activo = 1 AND Numero = '{0}' {1}", 
                Cod, id != 0 ? "AND Id_Area <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Area) FROM AREA WHERE Activo = 1 AND Nombre = '{0}' {1}", 
                Nom, id != 0 ? "AND Id_Area <> " + id.ToString() : ""))) > 0;
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
        ~Area()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
