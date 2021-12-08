using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class Dinero : SqlConnecion, IDisposable
    {
        private long _Id_Tipo_Pago;
        private Tipo_pago _Tipo_Pago;

        #region Public Properties
        public int Id_Dinero { get; set; }
        public decimal Valor { get; set; }
        public string Denominacion { get; set; }
        public bool Estado { get; set; }
        public long Id_Tipo_Pago
        {
            get { return _Id_Tipo_Pago; }
            set
            {
                _Id_Tipo_Pago = value;
                _Tipo_Pago = null;
            }
        }
        public Tipo_pago Tipo_Pago
        {
            get
            {
                if (_Tipo_Pago != null && _Tipo_Pago.Id_Tipo_Pago != 0)
                    return _Tipo_Pago;
                else if (_Id_Tipo_Pago != 0)
                    return _Tipo_Pago = new Tipo_pago(_Id_Tipo_Pago);
                else
                    return null;
            }
        }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Dinero()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Dinero(int id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Denominacion = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Dinero = Convert.ToInt32(dt.Rows[0]["Id_Dinero"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    Denominacion = Convert.ToString(dt.Rows[0]["Denominacion"]);
                    Estado = Convert.ToBoolean(dt.Rows[0]["Estado"]);
                    _Id_Tipo_Pago = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Pago"]);
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
            return String.Format("SELECT Id_Dinero, Valor, Denominacion, Estado, Id_Tipo_Pago, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion FROM DINERO");
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

        public static Dinero GetDinero(int id)
        {
            return new Dinero(id);
        }

        static public decimal Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Dinero", "DINERO", "");
            return Convert.ToInt32(SqlServer.EXEC_SCALAR(sql));
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
            return String.Format("INSERT INTO DINERO (Valor, Denominacion, Estado, Id_Tipo_Pago, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, " +
                "Usuario_Modificacion) VALUES ('{0}', '{1}', '{2}', '{3}', GETDATE(), {4}, {5}, {6}); ", 
                Valor.ToString().Replace(",", SqlServer.SigFloatSql), SqlServer.ValidarTexto(Denominacion), Estado, Id_Tipo_Pago,
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
                (Fecha_Modificacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Modificacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public string Delete(int id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete(int ID)
        {
            return String.Format("DELETE FROM Dinero WHERE Id_Dinero = {0};", ID.ToString().Replace(",", SqlServer.SigFloatSql));
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
            return String.Format("UPDATE DINERO SET Valor = '{1}', Denominacion = '{2}', Estado = '{3}', Id_Tipo_Pago = '{4}', Fecha_Creacion = {5}, " +
                "Usuario_Creacion = {6}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {7} WHERE Id_Dinero = {0};", 
                Id_Dinero, Valor.ToString().Replace(",", SqlServer.SigFloatSql), SqlServer.ValidarTexto(Denominacion), Estado, Id_Tipo_Pago,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Denominacion(string Denom)
        {
            return IsDuplicate_Denominacion(0, Denom);
        }

        public bool IsDuplicate_Denominacion(int id, string Denom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Dinero) FROM DINERO WHERE Estado = 1 AND Denominacion = '{0}' {1}", 
                Denom, id != 0 ? "AND Id_Dinero <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Valor(decimal Val)
        {
            return IsDuplicate_Valor(0, Val);
        }

        public bool IsDuplicate_Valor(int id, decimal Val)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Dinero) FROM DINERO WHERE Valor = '{0}' {1}",
                Val.ToString().Replace(",", SqlServer.SigFloatSql), id != 0 ? "AND Id_Dinero <> " + id.ToString() : ""))) > 0;
        }

        public static bool ActualizarActivo(long id_tipo_pago, bool activar)
        {
            return SqlServer.EXEC_COMMAND(string.Format("UPDATE DINERO SET Estado = {0} WHERE Id_Tipo_Pago = {1}", activar ? 1 : 0, id_tipo_pago)) == "OK";
        }

        #region Codigo nuevo

        #endregion

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
        ~Dinero()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
