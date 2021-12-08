using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.Frecuencias
{
    public partial class Tipo_Frecuencia : SqlConnecion, IDisposable
    {
        private long _Id_Valor_Normal;
        private long _Id_Valor_Extra;
        private Concepto_cuenta _Valor_Normal;
        private Concepto_cuenta _Valor_Extra;

        #region Public Properties
        public long Id_Tipo_Frecuencia { get; set; }
        public string Nombre { get; set; }
        public long Id_Valor_Normal
        {
            get { return _Id_Valor_Normal; }
            set { 
                _Id_Valor_Normal = value;
                _Valor_Normal = null;
            }
        }
        public long Id_Valor_Extra
        {
            get { return _Id_Valor_Extra; }
            set { 
                _Id_Valor_Extra = value;
                _Valor_Extra = null;
            }
        }
        public Concepto_cuenta Valor_Normal
        {
            get
            {
                if (_Valor_Normal != null && _Valor_Normal.Id_Concepto_Cuenta != 0)
                    return _Valor_Normal;
                else if (_Id_Valor_Normal != 0)
                    return _Valor_Normal = new Concepto_cuenta(_Id_Valor_Normal);
                else
                    return null;
            }
        }
        public Concepto_cuenta Valor_Extra
        {
            get
            {
                if (_Valor_Extra != null && _Valor_Extra.Id_Concepto_Cuenta != 0)
                    return _Valor_Extra;
                else if (_Id_Valor_Extra != 0)
                    return _Valor_Extra = new Concepto_cuenta(_Id_Valor_Extra);
                else
                    return null;
            }
        }
        public string Prefijo { get; set; }
        public bool Activo { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Tipo_Frecuencia()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Tipo_Frecuencia(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Tipo_Frecuencia = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Tipo_Frecuencia = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Frecuencia"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    _Id_Valor_Normal = Convert.ToInt64(dt.Rows[0]["Id_Valor_Normal"]);
                    _Id_Valor_Extra = Convert.ToInt64(dt.Rows[0]["Id_Valor_Extra"]);
                    Prefijo = Convert.ToString(dt.Rows[0]["Prefijo"]);
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
            return String.Format("SELECT Id_Tipo_Frecuencia, Nombre, Id_Valor_Normal, Id_Valor_Extra, Prefijo, Activo, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM TIPO_FRECUENCIA");
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

        public static Tipo_Frecuencia GetTipo_frecuencia(long id)
        {
            return new Tipo_Frecuencia(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Tipo_Frecuencia", "Tipo_Frecuencia", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        static public string Next_Prefijo()
        {
            return Convert.ToString(SqlServer.EXEC_SCALAR("SELECT CHAR(ASCII(ISNULL(MAX(Prefijo), '@')) + 1) FROM TIPO_FRECUENCIA"));
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
            return String.Format("INSERT INTO TIPO_FRECUENCIA (Nombre, Id_Valor_Normal, Id_Valor_Extra, Prefijo, Activo, Fecha_Creacion, " +
                "Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', GETDATE(), {5}, {6}, {7});", 
                SqlServer.ValidarTexto(Nombre), Id_Valor_Normal, Id_Valor_Extra, Prefijo, Activo,
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
            return String.Format("DELETE FROM TIPO_FRECUENCIA WHERE Id_Tipo_Frecuencia = {0};", ID);
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
            return String.Format("UPDATE TIPO_FRECUENCIA SET Nombre = '{1}', Id_Valor_Normal = '{2}', Id_Valor_Extra = '{3}', Prefijo = '{4}', Activo = '{5}', " +
                "Fecha_Creacion = {6}, Usuario_Creacion = {7}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {8} WHERE Id_Tipo_Frecuencia = {0};", 
                Id_Tipo_Frecuencia, SqlServer.ValidarTexto(Nombre), Id_Valor_Normal, Id_Valor_Extra, Prefijo, Activo,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Prefijo(string Pref)
        {
            return IsDuplicate_Prefijo(0, Pref);
        }

        public bool IsDuplicate_Prefijo(long id, string Pref)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Frecuencia) FROM TIPO_FRECUENCIA " +
                "WHERE Activo = 1 AND Prefijo = '{0}' {1}", Pref, id != 0 ? "AND Id_Tipo_Frecuencia <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Frecuencia) FROM TIPO_FRECUENCIA " +
                "WHERE Activo = 1 AND Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Tipo_Frecuencia <> " + id.ToString() : ""))) > 0;
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
        ~Tipo_Frecuencia()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
