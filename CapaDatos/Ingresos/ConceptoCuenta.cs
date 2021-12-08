using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class Concepto_cuenta : SqlConnecion, IDisposable
    {
        private long _Id_Grupo;
        private Grupos _Grupo;
        private string sIDsTipoPago;

        #region Public Properties
        public long Id_Concepto_Cuenta { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public long Id_Grupo
        {
            get { return _Id_Grupo; }
            set { 
                _Id_Grupo = value;
                _Grupo = null;
            }
        }
        public bool Iva { get; set; }
        public bool Estado { get; set; }
        public string CodigoGrupo { get; set; }
        public decimal PorcIva { get; set; }
        public decimal IvaValor { 
            get
            {
                return Iva ? Valor * (PorcIva / 100) : 0;
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
        public string IDsTipoPago { 
            get 
            {
                if (string.IsNullOrEmpty(sIDsTipoPago))
                    try
                    {
                        sIDsTipoPago = SqlServer.EXEC_SCALAR(string.Format("SELECT dbo.concatena_tipopago_concepto({0}, 1)", Id_Concepto_Cuenta)).ToString();
                    }
                    catch {}
                return sIDsTipoPago;
            }
            set
            {
                sIDsTipoPago = value;
            }
        }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Concepto_cuenta()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Concepto_cuenta(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Concepto_Cuenta = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Concepto_Cuenta = Convert.ToInt64(dt.Rows[0]["Id_Concepto_Cuenta"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    _Id_Grupo = Convert.ToInt64(dt.Rows[0]["Id_Grupo"]);
                    Iva = Convert.ToBoolean(dt.Rows[0]["Iva"]);
                    Estado = Convert.ToBoolean(dt.Rows[0]["Estado"]);
                    Codigo = Convert.ToString(dt.Rows[0]["Codigo"]);
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
            return String.Format("SELECT Id_Concepto_Cuenta, Nombre, Valor, Id_Grupo, Iva, Estado, Codigo, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion FROM CONCEPTO_CUENTA");
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

        public static Concepto_cuenta GetConcepto_cuenta(long id)
        {
            return new Concepto_cuenta(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Concepto_Cuenta", "CONCEPTO_CUENTA", "");
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
            return String.Format("INSERT INTO CONCEPTO_CUENTA (Nombre, Valor, Id_Grupo, Iva, Estado, Codigo, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', GETDATE(), {6}, {7}, {8}); ", 
                SqlServer.ValidarTexto(Nombre), Valor.ToString().Replace(",", SqlServer.SigFloatSql), Id_Grupo, Iva, Estado, SqlServer.ValidarTexto(Codigo),
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
                (Fecha_Modificacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Modificacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public string quitarTiposPago(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(string.Format("DELETE FROM CONCEPTO_TIPOPAGO WHERE Id_Concepto_Cuenta = {0}", id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
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
            return String.Format("DELETE FROM CONCEPTO_CUENTA WHERE Id_Concepto_Cuenta = {0};", ID);
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
            return String.Format("UPDATE CONCEPTO_CUENTA SET Nombre = '{1}', Valor = '{2}', Id_Grupo = '{3}', Iva = '{4}', Estado = '{5}', Codigo = '{6}', " +
                "Fecha_Creacion = {7}, Usuario_Creacion = {8}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {9} WHERE Id_Concepto_Cuenta = {0};", 
                Id_Concepto_Cuenta, SqlServer.ValidarTexto(Nombre), Valor.ToString().Replace(",", SqlServer.SigFloatSql), Id_Grupo, Iva, Estado, 
                SqlServer.ValidarTexto(Codigo),
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Codigo(string Cod)
        {
            return IsDuplicate_Codigo(0, Cod);
        }

        public bool IsDuplicate_Codigo(long id, string Cod)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Concepto_Cuenta) FROM CONCEPTO_CUENTA " +
                "WHERE Codigo = '{0}' {1}", Cod, id != 0 ? "AND Id_Concepto_Cuenta <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Concepto_Cuenta) FROM CONCEPTO_CUENTA " +
                "WHERE Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Concepto_Cuenta <> " + id.ToString() : ""))) > 0;
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
        ~Concepto_cuenta()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
