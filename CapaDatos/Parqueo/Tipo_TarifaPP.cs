using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Parqueo
{
    public partial class Tipo_TarifaPP : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long Id_Tipo_Tarifa { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Prefijo { get; set; }
        public string Dias { get; set; }
        public int Orden { get; set; }
        public bool Activo { get; set; }
        public bool Feriados { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Tipo_TarifaPP()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Tipo_TarifaPP(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Tipo_Tarifa = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Tipo_Tarifa = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Tarifa"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Codigo = Convert.ToString(dt.Rows[0]["Codigo"]);
                    Prefijo = Convert.ToString(dt.Rows[0]["Prefijo"]);
                    Dias = Convert.ToString(dt.Rows[0]["Dias"]);
                    Orden = Convert.ToInt32(dt.Rows[0]["Orden"]);
                    Feriados = Convert.ToBoolean(dt.Rows[0]["Feriados"]);
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
            return String.Format("SELECT Id_Tipo_Tarifa, Nombre, Codigo, Prefijo, Orden, Dias, Feriados, Activo, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM TIPO_TARIFA_PP");
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

        public static Tipo_TarifaPP GetTipoTarifaPP(long id)
        {
            return new Tipo_TarifaPP(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Tipo_Tarifa", "TIPO_TARIFA_PP", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        static public string Next_Prefijo()
        {
            return Convert.ToString(SqlServer.EXEC_SCALAR("SELECT CHAR(ASCII(ISNULL(MAX(Prefijo), '@')) + 1) FROM TIPO_TARIFA_PP"));
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
            return String.Format("INSERT INTO TIPO_TARIFA_PP (Nombre, Codigo, Prefijo, Orden, Dias, Feriados, Activo, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', GETDATE(), {7}, {8}, {9}); ",
                SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Codigo), SqlServer.ValidarTexto(Prefijo), Orden, Dias, Feriados, Activo,
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
            return String.Format("DELETE FROM TIPO_TARIFA_PP WHERE Id_Tipo_Tarifa = {0};", ID);
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
            return String.Format("UPDATE TIPO_TARIFA_PP SET Nombre = '{1}', Codigo = '{2}', Prefijo = '{3}', Orden = '{4}', Dias = '{5}', " +
                "Feriados = '{6}', Activo = '{7}', Fecha_Creacion = {8}, Usuario_Creacion = {9}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {10} " +
                "WHERE Id_Tipo_Tarifa = {0};", 
                Id_Tipo_Tarifa, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Codigo), SqlServer.ValidarTexto(Prefijo), Orden, Dias, Feriados, Activo,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Prefijo(string Pref)
        {
            return IsDuplicate_Prefijo(0, Pref);
        }

        public bool IsDuplicate_Prefijo(long id, string Pref)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Tarifa) FROM TIPO_TARIFA_PP " +
                "WHERE Activo = 1 AND Prefijo = '{0}' {1}", Pref, id != 0 ? "AND Id_Tipo_Tarifa <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Tarifa) FROM TIPO_TARIFA_PP " +
                "WHERE Activo = 1 AND Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Tipo_Tarifa <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Codigo(string Cod)
        {
            return IsDuplicate_Codigo(0, Cod);
        }

        public bool IsDuplicate_Codigo(long id, string Cod)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Tarifa) FROM TIPO_TARIFA_PP " +
                "WHERE Activo = 1 AND Codigo = '{0}' {1}", Cod, id != 0 ? "AND Id_Tipo_Tarifa <> " + id.ToString() : ""))) > 0;
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
        ~Tipo_TarifaPP()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
