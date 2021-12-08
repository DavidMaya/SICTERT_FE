using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Frecuencias
{
    public partial class Destino_Ruta : SqlConnecion, IDisposable
    {
        private long _Id_CiudadOrigen;
        private Ciudad _CiudadOrigen;
        private long _Id_CiudadDestino;
        private Ciudad _CiudadDestino;
        private long _Id_Destino;
        private Ciudad _Destino;

        #region Public Properties
        public long Id_Destino_Ruta { get; set; }
        public long Id_CiudadOrigen
        {
            get { return _Id_CiudadOrigen; }
            set
            {
                _Id_CiudadOrigen = value;
                _CiudadOrigen = null;
            }
        }
        public Ciudad CiudadOrigen
        {
            get
            {
                if (_CiudadOrigen != null && _CiudadOrigen.Id_Ciudad != 0)
                    return _CiudadOrigen;
                else if (_Id_CiudadOrigen != 0)
                    return _CiudadOrigen = new Ciudad(_Id_CiudadOrigen);
                else
                    return null;
            }
        }
        public long Id_CiudadDestino
        {
            get { return _Id_CiudadDestino; }
            set
            {
                _Id_CiudadDestino = value;
                _CiudadDestino = null;
            }
        }
        public Ciudad CiudadDestino
        {
            get
            {
                if (_CiudadDestino != null && _CiudadDestino.Id_Ciudad != 0)
                    return _CiudadDestino;
                else if (_Id_CiudadDestino != 0)
                    return _CiudadDestino = new Ciudad(_Id_CiudadDestino);
                else
                    return null;
            }
        }
        public long Id_Destino
        {
            get { return _Id_Destino; }
            set
            {
                _Id_Destino = value;
                _Destino = null;
            }
        }
        public Ciudad Destino
        {
            get
            {
                if (_Destino != null && _Destino.Id_Ciudad != 0)
                    return _Destino;
                else if (_Id_Destino != 0)
                    return _Destino = new Ciudad(_Id_Destino);
                else
                    return null;
            }
        }
        #endregion

        public Destino_Ruta()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Destino_Ruta(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Destino_Ruta = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Destino_Ruta = Convert.ToInt64(dt.Rows[0]["Id_Destino_Ruta"]);
                    _Id_CiudadOrigen = Convert.ToInt64(dt.Rows[0]["Id_Ciudad_Origen"]);
                    _Id_CiudadDestino = Convert.ToInt64(dt.Rows[0]["Id_Ciudad_Destino"]);
                    _Id_Destino = Convert.ToInt64(dt.Rows[0]["Id_Destino"]);
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
            return String.Format("SELECT Id_Destino_Ruta, Id_Ciudad_Origen, Id_Ciudad_Destino, Id_Destino FROM DESTINOS_EN_RUTA");
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

        public static Destino_Ruta GetDestino_Ruta(long id)
        {
            return new Destino_Ruta(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Destino_Ruta", "DESTINOS_EN_RUTA", "");
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
            return String.Format("INSERT INTO DESTINOS_EN_RUTA (Id_Ciudad_Origen, Id_Ciudad_Destino, Id_Destino) " +
                "VALUES ({0}, {1}, {2});", Id_CiudadOrigen, Id_CiudadDestino, Id_Destino);
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
            return String.Format("DELETE FROM DESTINOS_EN_RUTA WHERE Id_Destino_Ruta = {0};", ID);
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
            return String.Format("UPDATE DESTINOS_EN_RUTA SET Id_Ciudad_Origen = {1}, Id_Ciudad_Destino = {2}, Id_Destino = {3} " +
                "WHERE Id_Destino_Ruta = {0};", Id_Destino_Ruta, Id_Destino_Ruta, Id_CiudadOrigen, Id_CiudadDestino, Id_Destino);
        }

        public bool IsDuplicate_Destino(long origen, long destino, long intermedio)
        {
            return IsDuplicate_Destino(0, origen, destino, intermedio);
        }

        public bool IsDuplicate_Destino(long id, long origen, long destino, long intermedio)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Destino_Ruta) FROM DESTINOS_EN_RUTA " +
                "WHERE Id_Ciudad_Origen = {0} AND Id_Ciudad_Destino = {1} AND Id_Destino = {2} ", 
                origen, destino, intermedio, id != 0 ? "AND Id_Destino_Ruta <> " + id.ToString() : ""))) > 0;
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
        ~Destino_Ruta()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
