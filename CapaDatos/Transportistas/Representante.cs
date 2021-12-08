using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Transportistas
{
    public partial class Propietario_socio : SqlConnecion, IDisposable
    {
        private string _Nombre;
        private string _Razon_Social;

        #region Public Properties
        public long Id_Propietario_Socio { get; set; }
        public string CI_Ruc { get; set; }
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value.ToUpper(); }
        }
        public string Razon_Social
        {
            get { return _Razon_Social; }
            set { _Razon_Social = value.ToUpper(); }
        }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public long Dias_Credito { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
        #endregion

        public Propietario_socio()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Propietario_socio(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Propietario_Socio = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Propietario_Socio = Convert.ToInt64(dt.Rows[0]["Id_Propietario_Socio"]);
                    CI_Ruc = (dt.Rows[0]["CI_Ruc"]) == null ? "": Convert.ToString(dt.Rows[0]["CI_Ruc"]);
                    _Nombre = (dt.Rows[0]["Nombre"]) == null ? "" : Convert.ToString(dt.Rows[0]["Nombre"]);
                    _Razon_Social = (dt.Rows[0]["Razon_Social"]) == null ? "" : Convert.ToString(dt.Rows[0]["Razon_Social"]);
                    Telefono = (dt.Rows[0]["Telefono"]) == null ? "" : Convert.ToString(dt.Rows[0]["Telefono"]);
                    Direccion = (dt.Rows[0]["Direccion"]) == null ? "" : Convert.ToString(dt.Rows[0]["Direccion"]);
                    Dias_Credito = (dt.Rows[0]["Dias_Credito"]) == null ? 0 : Convert.ToInt64(dt.Rows[0]["Dias_Credito"]);
                    Email = (dt.Rows[0]["Email"]) == null ? "" : Convert.ToString(dt.Rows[0]["Email"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
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
            return String.Format("SELECT Id_Propietario_Socio, CI_Ruc, Nombre, Razon_Social, Telefono, Direccion, Dias_Credito, Email, Activo FROM Propietario_socio");
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

        public static Propietario_socio GetPropietario_socio(long id)
        {
            return new Propietario_socio(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Propietario_Socio", "Propietario_socio", "");
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
            return String.Format("INSERT INTO Propietario_socio (CI_Ruc, Nombre, Razon_Social, Telefono, Direccion, Dias_Credito, Email, Activo) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}'); ", SqlServer.ValidarTexto(CI_Ruc), SqlServer.ValidarTexto(Nombre), 
                SqlServer.ValidarTexto(Razon_Social), SqlServer.ValidarTexto(Telefono), SqlServer.ValidarTexto(Direccion), Dias_Credito, 
                SqlServer.ValidarTexto(Email), Activo);
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
            return String.Format("DELETE FROM Propietario_socio WHERE Id_Propietario_Socio = {0};", ID);
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
            return String.Format("UPDATE Propietario_socio SET CI_Ruc = '{1}', Nombre = '{2}', Razon_Social = '{3}', Telefono = '{4}', Direccion = '{5}', " +
                "Dias_Credito = '{6}', Email = '{7}', Activo = '{8}' WHERE Id_Propietario_Socio = {0};", Id_Propietario_Socio, 
                SqlServer.ValidarTexto(CI_Ruc), SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Razon_Social), SqlServer.ValidarTexto(Telefono), 
                SqlServer.ValidarTexto(Direccion), Dias_Credito, SqlServer.ValidarTexto(Email), Activo);
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
        ~Propietario_socio()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
