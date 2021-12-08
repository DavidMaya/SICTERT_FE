using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.Parqueo
{
    public partial class Rango_TiempoPP : SqlConnecion, IDisposable
    {
        private long _Id_Valor;
        private long _Id_Tipo_Tarifa;
        private long _Id_Recargo;
        private Concepto_cuenta _Valor;
        private Tipo_TarifaPP _Tipo_Tarifa;
        private Concepto_cuenta _Recargo;

        #region Public Properties
        public long Id_Tiempo_Parqueo { get; set; }
        public string Nombre { get; set; }
        public long Id_Valor
        {
            get { return _Id_Valor; }
            set { 
                _Id_Valor = value;
                _Valor = null;
            }
        }
        public long Id_Recargo
        {
            get { return _Id_Recargo; }
            set { 
                _Id_Recargo = value;
                _Recargo = null;
            }
        }
        public Concepto_cuenta Valor
        {
            get
            {
                if (_Valor != null && _Valor.Id_Concepto_Cuenta != 0)
                    return _Valor;
                else if (_Id_Valor != 0)
                    return _Valor = new Concepto_cuenta(_Id_Valor);
                else
                    return null;
            }
        }
        public Concepto_cuenta Recargo
        {
            get
            {
                if (_Recargo != null && _Recargo.Id_Concepto_Cuenta != 0)
                    return _Recargo;
                else if (_Id_Recargo != 0)
                    return _Recargo = new Concepto_cuenta(_Id_Recargo);
                else
                    return null;
            }
        }
        public long Id_Tipo_Tarifa
        {
            get { return _Id_Tipo_Tarifa; }
            set { 
                _Id_Tipo_Tarifa = value;
                _Tipo_Tarifa = null;
            }
        }
        public Tipo_TarifaPP TipoTarifa
        {
            get
            {
                if (_Tipo_Tarifa != null && _Tipo_Tarifa.Id_Tipo_Tarifa != 0)
                    return _Tipo_Tarifa;
                else if (_Id_Tipo_Tarifa != 0)
                    return _Tipo_Tarifa = new Tipo_TarifaPP(_Id_Tipo_Tarifa);
                else
                    return null;
            }
        }
        public int Min_Tiempo { get; set; }
        public int Max_Tiempo { get; set; }
        public bool Valor_x_Hora { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Rango_TiempoPP()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Rango_TiempoPP(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Tiempo_Parqueo = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Tiempo_Parqueo = Convert.ToInt64(dt.Rows[0]["Id_Tiempo_Parqueo"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Min_Tiempo = Convert.ToInt32(dt.Rows[0]["Min_Tiempo"]);
                    Max_Tiempo = Convert.ToInt32(dt.Rows[0]["Max_Tiempo"]);
                    _Id_Valor = Convert.ToInt64(dt.Rows[0]["Id_Valor"]);
                    _Id_Tipo_Tarifa = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Tarifa"]);
                    Valor_x_Hora = Convert.ToBoolean(dt.Rows[0]["Valor_x_Hora"]);
                    _Id_Valor = dt.Rows[0]["Id_Valor"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Valor"]) : 0;
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
            return String.Format("SELECT Id_Tiempo_Parqueo, Nombre, Min_Tiempo, Max_Tiempo, Id_Valor, Valor_x_Hora, Id_Tipo_Tarifa, Id_Recargo, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM TIEMPO_PARQUEO");
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

        public static Rango_TiempoPP GetRango_TiempoPP(long id)
        {
            return new Rango_TiempoPP(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Tiempo_Parqueo", "Tiempo_Parqueo", "");
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
            return String.Format("INSERT INTO TIEMPO_PARQUEO (Nombre, Min_Tiempo, Max_Tiempo, Id_Valor, Valor_x_Hora, Id_Recargo, Id_Tipo_Tarifa, " +
                "Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ('{0}', {1}, {2}, {3}, '{4}', {5}, {6}, GETDATE(), {7}, {8}, {9});", 
                SqlServer.ValidarTexto(Nombre), Min_Tiempo, Max_Tiempo, Id_Valor, Valor_x_Hora, Id_Recargo == 0 ? "NULL" : Id_Recargo.ToString(), 
                Id_Tipo_Tarifa, Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM TIEMPO_PARQUEO WHERE Id_Tiempo_Parqueo = {0};", ID);
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
            return String.Format("UPDATE TIEMPO_PARQUEO SET Nombre= '{1}', Min_Tiempo= {2}, Max_Tiempo= {3}, Id_Valor= {4}, Valor_x_Hora= '{5}', " +
                "Id_Recargo= {6}, Id_Tipo_Tarifa= {7}, Fecha_Creacion = {8}, Usuario_Creacion = {9}, Fecha_Modificacion = GETDATE(), " +
                "Usuario_Modificacion = {10} WHERE Id_Tiempo_Parqueo = {0};", Id_Tiempo_Parqueo, SqlServer.ValidarTexto(Nombre), 
                Min_Tiempo, Max_Tiempo, Id_Valor, Valor_x_Hora, Id_Recargo == 0 ? "NULL" : Id_Recargo.ToString(), Id_Tipo_Tarifa,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tiempo_Parqueo) FROM TIEMPO_PARQUEO " +
                "WHERE Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Tiempo_Parqueo <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_TiempoMin(int Min)
        {
            return IsDuplicate_TiempoMin(0, Min);
        }

        public bool IsDuplicate_TiempoMin(long id, int Min)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tiempo_Parqueo) FROM TIEMPO_PARQUEO " +
                "WHERE Min_Tiempo = {0} {1}", Min, id != 0 ? "AND Id_Tiempo_Parqueo <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_TiempoMax(int Max)
        {
            return IsDuplicate_TiempoMax(0, Max);
        }

        public bool IsDuplicate_TiempoMax(long id, int Max)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tiempo_Parqueo) FROM TIEMPO_PARQUEO " +
                "WHERE Max_Tiempo = {0} {1}", Max, id != 0 ? "AND Id_Tiempo_Parqueo <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Tiempo(int Num)
        {
            return IsDuplicate_Tiempo(0, Num);
        }

        public bool IsDuplicate_Tiempo(long id, int Num)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tiempo_Parqueo) FROM TIEMPO_PARQUEO " +
                "WHERE (Min_Tiempo = {0} OR Max_Tiempo = {0}) {1}", Num, id != 0 ? "AND Id_Tiempo_Parqueo <> " + id.ToString() : ""))) > 0;
        }

        public bool IsOverlap_Tiempo(int Min, int Max)
        {
            return IsOverlap_Tiempo(0, Min, Max);
        }

        public bool IsOverlap_Tiempo(long id, int Min, int Max)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Tiempo_Parqueo) FROM TIEMPO_PARQUEO " +
                "WHERE CASE WHEN {0} > Min_Tiempo THEN {0} ELSE Min_Tiempo END <= CASE WHEN {1} < Max_Tiempo THEN {1} ELSE Max_Tiempo END {2}", 
                Min, Max, id != 0 ? "AND Id_Tiempo_Parqueo <> " + id.ToString() : ""))) > 0;
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
        ~Rango_TiempoPP()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
