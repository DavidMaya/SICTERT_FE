using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.TasaUsuario
{
    public partial class Tipo_TarifaTU : SqlConnecion, IDisposable
    {
        private Concepto_cuenta _Valor;
        private long _Id_Concepto_Cuenta;

        #region Public Properties
        public long Id_Tipo_Tarifa { get; set; }
        public string Nombre { get; set; }
        public long Id_Concepto_Cuenta
        {
            get { return _Id_Concepto_Cuenta; }
            set
            {
                _Id_Concepto_Cuenta = value;
                _Valor = null;
            }
        }
        public string Codigo { get; set; }
        public string Prefijo { get; set; }
        public string Dias { get; set; }
        public Concepto_cuenta Valor
        {
            get
            {
                if (_Valor != null && _Valor.Id_Concepto_Cuenta != 0)
                    return _Valor;
                else if (_Id_Concepto_Cuenta != 0)
                    return _Valor = new Concepto_cuenta(_Id_Concepto_Cuenta);
                else
                    return null;
            }
        }
        public int Orden { get; set; }
        public bool Tercera_Edad { get; set; }
        public bool Discapacitado { get; set; }
        public bool Menor_Edad { get; set; }
        public bool Activo { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Tipo_TarifaTU()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Tipo_TarifaTU(long id)
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
                    Id_Concepto_Cuenta = Convert.ToInt64(dt.Rows[0]["Id_Concepto_Cuenta"]);
                    Tercera_Edad = Convert.ToBoolean(dt.Rows[0]["Tercera_Edad"]);
                    Discapacitado = Convert.ToBoolean(dt.Rows[0]["Discapacitado"]);
                    Menor_Edad = Convert.ToBoolean(dt.Rows[0]["Menor_Edad"]);
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
            return String.Format("SELECT Id_Tipo_Tarifa, Nombre, Codigo, Prefijo, Id_Concepto_Cuenta, Orden, Dias, Tercera_Edad, " +
                "Discapacitado, Menor_Edad, Activo, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM TIPO_TARIFA_TU");
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

        public static Tipo_TarifaTU GetTipoTarifaTU(long id)
        {
            return new Tipo_TarifaTU(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Tipo_Tarifa", "TIPO_TARIFA_TU", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        static public string Next_Prefijo()
        {
            return Convert.ToString(SqlServer.EXEC_SCALAR("SELECT CHAR(ASCII(ISNULL(MAX(Prefijo), '@')) + 1) FROM TIPO_TARIFA_TU"));
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
            return String.Format("INSERT INTO TIPO_TARIFA_TU (Nombre, Codigo, Prefijo, Id_Concepto_Cuenta, Orden, Dias, Tercera_Edad, Discapacitado, " +
                "Menor_Edad, Activo, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', GETDATE(), {10}, {11}, {12}); ",
                SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Codigo), SqlServer.ValidarTexto(Prefijo), Id_Concepto_Cuenta, Orden, Dias, 
                Tercera_Edad, Discapacitado, Menor_Edad, Activo, Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM TIPO_TARIFA_TU WHERE Id_Tipo_Tarifa = {0};", ID);
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
            return String.Format("UPDATE TIPO_TARIFA_TU SET Nombre = '{1}', Codigo = '{2}', Prefijo = '{3}', Id_Concepto_Cuenta = '{4}', Orden = '{5}', " +
                "Dias = '{6}', Tercera_Edad = '{7}', Discapacitado = '{8}', Menor_Edad = '{9}', Activo = '{10}', Fecha_Creacion = {11}, Usuario_Creacion = {12}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {13} WHERE Id_Tipo_Tarifa = {0};", 
                Id_Tipo_Tarifa, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Codigo), SqlServer.ValidarTexto(Prefijo), Id_Concepto_Cuenta, 
                Orden, Dias, Tercera_Edad, Discapacitado, Menor_Edad, Activo,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Prefijo(string Pref)
        {
            return IsDuplicate_Prefijo(0, Pref);
        }

        public bool IsDuplicate_Prefijo(long id, string Pref)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Tarifa) FROM TIPO_TARIFA_TU " +
                "WHERE Activo = 1 AND Prefijo = '{0}' {1}", Pref, id != 0 ? "AND Id_Tipo_Tarifa <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Tarifa) FROM TIPO_TARIFA_TU " +
                "WHERE Activo = 1 AND Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Tipo_Tarifa <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Codigo(string Cod)
        {
            return IsDuplicate_Codigo(0, Cod);
        }

        public bool IsDuplicate_Codigo(long id, string Cod)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tipo_Tarifa) FROM TIPO_TARIFA_TU " +
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
        ~Tipo_TarifaTU()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
