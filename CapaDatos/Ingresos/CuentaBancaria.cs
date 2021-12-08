using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class CuentaBancaria : SqlConnecion, IDisposable
    {
        private long _Id_Banco;
        private Banco _Banco;

        #region Public Properties
        public long Id_CuentaBancaria { get; set; }
        public string Numero { get; set; }
        public long Id_Banco
        {
            get { return _Id_Banco; }
            set
            {
                _Id_Banco = value;
                _Banco = null;
            }
        }
        public Banco banco
        {
            get
            {
                if (_Banco != null && _Banco.Id_Banco != 0)
                    return _Banco;
                else if (_Id_Banco != 0)
                    return _Banco = new Banco(_Id_Banco);
                else
                    return null;
            }
        }
        public bool Activo { get; set; }
        public string CuentaContable { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public CuentaBancaria()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public CuentaBancaria(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_CuentaBancaria = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_CuentaBancaria = Convert.ToInt64(dt.Rows[0]["Id_CuentaBancaria"]);
                    Numero = Convert.ToString(dt.Rows[0]["Numero"]);
                    _Id_Banco = Convert.ToInt64(dt.Rows[0]["Id_Banco"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    CuentaContable = dt.Rows[0]["CuentaContable"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["CuentaContable"]) : "";
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
            return String.Format("SELECT Id_CuentaBancaria, Numero, Id_Banco, Activo, CuentaContable, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion FROM CUENTA_BANCARIA");
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

        public static CuentaBancaria GetCuenta_bancaria(long id)
        {
            return new CuentaBancaria(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_CuentaBancaria", "CUENTA_BANCARIA", "");
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
            return String.Format("INSERT INTO CUENTA_BANCARIA (Numero, Id_Banco, Activo, CuentaContable, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion) VALUES ('{0}', {1}, {2}, {3}, GETDATE(), {4}, {5}, {6}); ", 
                SqlServer.ValidarTexto(Numero), _Id_Banco, Activo ? "1" : "0", 
                CuentaContable != null ? "'" + SqlServer.ValidarTexto(CuentaContable) + "'" : "NULL",
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
            return String.Format("DELETE FROM CUENTA_BANCARIA WHERE Id_CuentaBancaria = {0};", ID);
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
            return String.Format("UPDATE CUENTA_BANCARIA SET Numero = '{1}', Id_Banco = {2}, Activo = {3}, CuentaContable = {4}, Fecha_Creacion = {5}, " +
                "Usuario_Creacion = {6}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {7} WHERE Id_CuentaBancaria = {0};", 
                Id_CuentaBancaria, SqlServer.ValidarTexto(Numero), _Id_Banco, Activo ? "1" : "0", 
                CuentaContable != null ? "'" + SqlServer.ValidarTexto(CuentaContable) + "'" : "NULL",
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Numero(string Num, long Id_Ban)
        {
            return IsDuplicate_Numero(0, Num, Id_Ban);
        }

        public bool IsDuplicate_Numero(long id, string Num, long Id_Ban)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_CuentaBancaria) FROM CUENTA_BANCARIA WHERE Numero = '{0}' AND Id_Banco = {1} {2}", SqlServer.ValidarTexto(Num), Id_Ban, id != 0 ? "AND Id_CuentaBancaria <> " + id.ToString() : ""))) > 0;
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
        ~CuentaBancaria()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
