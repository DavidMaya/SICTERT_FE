using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.Transportistas
{
    public partial class Cooperativa : SqlConnecion, IDisposable
    {
        private long _Id_Cliente_Final;
        private Cliente_Final _Cliente_final;

        #region Public Properties
        public long Id_Cooperativa { get; set; }
        public string Ruc { get; set; }
        public string Nombre { get; set; }
        public string Razon_Social { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public long Id_Cliente_Final
        {
            get { return _Id_Cliente_Final; }
            set
            {
                _Id_Cliente_Final = value;
                _Cliente_final = null;
            }
        }
        public Cliente_Final Cliente_final
        {
            get
            {
                if (_Cliente_final != null && _Cliente_final.Id_Cliente_Final != 0)
                    return _Cliente_final;
                else if (_Id_Cliente_Final != 0)
                    return _Cliente_final = new Cliente_Final(_Id_Cliente_Final);
                else
                    return null;
            }
        }
        public string Email { get; set; }
        public bool Boleteria { get; set; }
        public bool Activo { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Cooperativa()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Cooperativa(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Cooperativa = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Cooperativa = Convert.ToInt64(dt.Rows[0]["Id_Cooperativa"]);
                    Ruc = dt.Rows[0]["Id_Cooperativa"] == null ? "" : Convert.ToString(dt.Rows[0]["Ruc"]);
                    Nombre = dt.Rows[0]["Nombre"] == null ? "" : Convert.ToString(dt.Rows[0]["Nombre"]);
                    Razon_Social = dt.Rows[0]["Razon_Social"] == null ? "" : Convert.ToString(dt.Rows[0]["Razon_Social"]);
                    Telefono = dt.Rows[0]["Telefono"] == null ? "" : Convert.ToString(dt.Rows[0]["Telefono"]);
                    Direccion = dt.Rows[0]["Direccion"] == null ? "" : Convert.ToString(dt.Rows[0]["Direccion"]);
                    Email = (dt.Rows[0]["Email"]) == null ? "" : Convert.ToString(dt.Rows[0]["Email"]);
                    Boleteria = Convert.ToBoolean(dt.Rows[0]["Boleteria"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    _Id_Cliente_Final = dt.Rows[0]["Id_Cliente_Final"] == null ? 0 : Convert.ToInt64(dt.Rows[0]["Id_Cliente_Final"]);
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
            return String.Format("SELECT Id_Cooperativa, Ruc, Nombre, Razon_Social, Telefono, Id_Cliente_Final, Direccion, Email, Boleteria, Activo, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM COOPERATIVA");
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

        public static Cooperativa GetCooperativa(long id)
        {
            return new Cooperativa(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Cooperativa", "COOPERATIVA", "");
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
            return String.Format("INSERT INTO COOPERATIVA (Ruc, Nombre, Razon_Social, Telefono, Id_Cliente_Final, Direccion, Email, Boleteria, Activo, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES('{0}', '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}', '{8}', GETDATE(), {9}, {10}, {11}); ", 
                Ruc, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Razon_Social), SqlServer.ValidarTexto(Telefono), 
                Id_Cliente_Final == 0 ? "NULL" : Id_Cliente_Final.ToString(), SqlServer.ValidarTexto(Direccion), SqlServer.ValidarTexto(Email), 
                Boleteria, Activo, Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM COOPERATIVA WHERE Id_Cooperativa = {0};", ID);
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
            return String.Format("UPDATE COOPERATIVA SET Ruc = '{1}', Nombre = '{2}', Razon_Social = '{3}', Telefono = '{4}', Id_Cliente_Final = {5}, " +
                "Direccion = '{6}', Email = '{7}', Boleteria = '{8}', Activo = '{9}', Fecha_Creacion = {10}, Usuario_Creacion = {11}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {12} WHERE Id_Cooperativa = {0};",
                Id_Cooperativa, SqlServer.ValidarTexto(Ruc), SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Razon_Social), 
                SqlServer.ValidarTexto(Telefono), Id_Cliente_Final == 0 ? "null" : Id_Cliente_Final.ToString(), SqlServer.ValidarTexto(Direccion), 
                SqlServer.ValidarTexto(Email), Boleteria, Activo,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Ruc(string sRuc)
        {
            return IsDuplicate_Ruc(0, sRuc);
        }

        public bool IsDuplicate_Ruc(long id, string sRuc)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Cooperativa) FROM COOPERATIVA WHERE Activo = 1 AND " +
                "Ruc = '{0}' {1}", sRuc, id != 0 ? "AND Id_Cooperativa <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Cooperativa) FROM COOPERATIVA WHERE Activo = 1 AND " +
                "Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Cooperativa <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_RazonSocial(string RSoc)
        {
            return IsDuplicate_RazonSocial(0, RSoc);
        }

        public bool IsDuplicate_RazonSocial(long id, string RSoc)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Cooperativa) FROM COOPERATIVA WHERE Activo = 1 AND " +
                "Razon_Social = '{0}' {1}", RSoc, id != 0 ? "AND Id_Cooperativa <> " + id.ToString() : ""))) > 0;
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
        ~Cooperativa()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
