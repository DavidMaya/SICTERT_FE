using CapaDatos.Frecuencias;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace AccesoDatos.Transportistas
{
    public partial class TarifaParqueoUTT : SqlConnecion, IDisposable
    {
        private long _Id_CC_PrimerasHoras;
        private Concepto_cuenta _CC_PrimerasHoras;
        private long _Id_CC_SiguientesHoras;
        private Concepto_cuenta _CC_SIguientesHoras;

        #region Public Properties
        public long Id_TarifaParqueo { get; set; }
        public string Nombre { get; set; }
        public long Id_CC_PrimerasHoras
        {
            get { return _Id_CC_PrimerasHoras; }
            set
            {
                _Id_CC_PrimerasHoras = value;
                _CC_PrimerasHoras = null;
            }
        }
        public Concepto_cuenta CC_PrimerasHoras
        {
            get
            {
                if (_CC_PrimerasHoras != null && _CC_PrimerasHoras.Id_Concepto_Cuenta != 0)
                    return _CC_PrimerasHoras;
                else if (_Id_CC_PrimerasHoras != 0)
                    return _CC_PrimerasHoras = new Concepto_cuenta(_Id_CC_PrimerasHoras);
                else
                    return null;
            }
        }
        public long Id_CC_SiguientesHoras
        {
            get { return _Id_CC_SiguientesHoras; }
            set
            {
                _Id_CC_SiguientesHoras = value;
                _CC_SIguientesHoras = null;
            }
        }
        public Concepto_cuenta CC_SIguientesHoras
        {
            get
            {
                if (_CC_SIguientesHoras != null && _CC_SIguientesHoras.Id_Concepto_Cuenta != 0)
                    return _CC_SIguientesHoras;
                else if (_Id_CC_SiguientesHoras != 0)
                    return _CC_SIguientesHoras = new Concepto_cuenta(_Id_CC_SiguientesHoras);
                else
                    return null;
            }
        }
        public int Horas { get; set; }
        public bool Activo { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public TarifaParqueoUTT()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public TarifaParqueoUTT(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_TarifaParqueo = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_TarifaParqueo = Convert.ToInt64(dt.Rows[0]["Id_TarifaParqueo"]);
                    _Id_CC_SiguientesHoras = Convert.ToInt64(dt.Rows[0]["Id_CC_SiguientesHoras"]);
                    _Id_CC_PrimerasHoras = Convert.ToInt64(dt.Rows[0]["Id_CC_PrimerasHoras"]);
                    Horas = Convert.ToInt32(dt.Rows[0]["Horas"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
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
            return String.Format("SELECT Id_TarifaParqueo, Nombre, Id_CC_PrimerasHoras, Id_CC_SiguientesHoras, Horas, Activo, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM TARIFA_PARQUEO_UTT");
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

        public static TarifaParqueoUTT GetTarifaParqueoUTT(long id)
        {
            return new TarifaParqueoUTT(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_TarifaParqueo", "TARIFA_PARQUEO_UTT", "");
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
            return String.Format("INSERT INTO TARIFA_PARQUEO_UTT (Nombre, Id_CC_PrimerasHoras, Id_CC_SiguientesHoras, Horas, Activo, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ('{0}', {1}, {2}, {3}, {4}, GETDATE(), {5}, {6}, {7});", 
                SqlServer.ValidarTexto(Nombre), Id_CC_PrimerasHoras, Id_CC_SiguientesHoras, Horas, Activo ? 1 : 0,
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
            return String.Format("DELETE FROM TARIFA_PARQUEO_UTT WHERE Id_TarifaParqueo = {0};", ID);
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
            return String.Format("UPDATE TARIFA_PARQUEO_UTT SET Nombre = '{1}', Id_CC_PrimerasHoras = {2}, Id_CC_SiguientesHoras = {3}, Horas = {4}, " +
                "Activo = {5}, Fecha_Creacion = {6}, Usuario_Creacion = {7}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {8} " +
                "WHERE Id_TarifaParqueo = {0};", 
                Id_TarifaParqueo, SqlServer.ValidarTexto(Nombre), Id_CC_PrimerasHoras, Id_CC_SiguientesHoras, Horas, Activo ? 1 : 0,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Nombre(string nom)
        {
            return IsDuplicate_Nombre(0, nom);
        }

        public bool IsDuplicate_Nombre(long id, string nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_TarifaParqueo) FROM TARIFA_PARQUEO_UTT WHERE Nombre = '{0}' {1} ", 
                nom, id != 0 ? "AND Id_TarifaParqueo <> " + id.ToString() : ""))) > 0;
        }

        public static int ContarTarifas(long id, bool act)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_TarifaParqueo) FROM TARIFA_PARQUEO_UTT WHERE Activo = {0} {1} ",
                act ? 1 : 0, id != 0 ? "AND Id_TarifaParqueo <> " + id.ToString() : "")));
        }

        public static int ContarTarifas(string ids, bool act)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_TarifaParqueo) FROM TARIFA_PARQUEO_UTT WHERE Activo = {0} {1} ",
                act ? 1 : 0, ids != null && ids.Length > 0 ? "AND Id_TarifaParqueo NOT IN (" + ids + ")" : "")));
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
        ~TarifaParqueoUTT()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
