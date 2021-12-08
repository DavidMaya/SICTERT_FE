using AccesoDatos;
using CapaDatos.UsuariosPerfiles;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public class Envio_Cierre : SqlConnecion, IDisposable
    {
        private long _Id_Cierre;
        private Cierre_caja _Cierre;
        private long _Id_Usuario;
        private Usuarios _Usuario;

        #region Public Properties
        public long Id_EnvioCierre { get; set; }
        public long Id_Cierre
        {
            get { return _Id_Cierre; }
            set
            {
                _Id_Cierre = value;
                _Cierre = null;
            }
        }
        public Cierre_caja Cierre
        {
            get
            {
                if (_Cierre != null && _Cierre.Id_Cierre_Caja != 0)
                    return _Cierre;
                else if (_Id_Cierre != 0)
                    return _Cierre = new Cierre_caja(_Id_Cierre);
                else
                    return null;
            }
        }
        public int CodigoCaja { get; set; }
        public int CodigoRecaudador { get; set; }
        public bool Enviado { get; set; }
        public DateTime? FechaHoraEnvio { get; set; }
        public long Id_Usuario
        {
            get { return _Id_Usuario; }
            set
            {
                _Id_Usuario = value;
                _Usuario = null;
            }
        }
        public Usuarios Usuario
        {
            get
            {
                if (_Usuario != null && _Usuario.Id_Usuario != 0)
                    return _Usuario;
                else if (_Id_Usuario != 0)
                    return _Usuario = new Usuarios(_Id_Usuario);
                else
                    return null;
            }
        }
        #endregion

        public Envio_Cierre()
        { 
        }
        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Envio_Cierre(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Cierre_Caja = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_EnvioCierre = Convert.ToInt64(dt.Rows[0]["Id_EnvioCierre"]);
                    Id_Cierre = Convert.ToInt64(dt.Rows[0]["Id_Cierre_Caja"]);
                    CodigoCaja = Convert.ToInt32(dt.Rows[0]["CodigoCaja"]);
                    CodigoRecaudador = Convert.ToInt32(dt.Rows[0]["CodigoRecaudador"]);
                    Enviado = Convert.ToBoolean(dt.Rows[0]["Enviado"]);
                    FechaHoraEnvio = valorDateTime(dt.Rows[0]["FechaHoraEnvio"]);
                    _Id_Usuario = Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
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
            return String.Format("SELECT Id_EnvíoCierre, Id_Cierre_Caja, CodigoCaja, CodigoRecaudador, Enviado, FechaHoraEnvio, Id_Usuario FROM ENVIO_CIERRE");
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

        public static Envio_Cierre GetEnvio_Cierre(long id)
        {
            return new Envio_Cierre(id);
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
            return String.Format("INSERT INTO ENVIO_CIERRE (Id_Cierre_Caja, CodigoCaja, CodigoRecaudador, Enviado, FechaHoraEnvio, Id_Usuario) " +
                "VALUES ({0}, {1}, {2}, {3}, {4}, {5}); ", 
                Id_Cierre, CodigoCaja, CodigoRecaudador, Enviado ? 1 : 0,
                (FechaHoraEnvio == null) ? "NULL" : "'" + Convert.ToDateTime(FechaHoraEnvio.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                _Id_Usuario);
        }

        public string Delete(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete(long id)
        {
            return String.Format("DELETE FROM ENVIO_CIERRE WHERE Id_Cierre_Caja = {0};", id);
        }

        public string Update(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;
        }

        public string GetSQLUpdate(long id)
        {
            return String.Format("UPDATE ENVIO_CIERRE SET Id_Cierre_Caja = {1}, CodigoCaja = {2}, CodigoRecaudador = {3}, Enviado = {4}, " +
                "FechaHoraEnvio = {5}, Id_Usuario = {6} WHERE Id_EnvioCierre = {0}",
                Id_EnvioCierre, Id_Cierre, CodigoCaja, CodigoRecaudador, Enviado ? 1 : 0,
                (FechaHoraEnvio == null) ? "NULL" : "'" + Convert.ToDateTime(FechaHoraEnvio.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                _Id_Usuario);
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
        ~Envio_Cierre()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
