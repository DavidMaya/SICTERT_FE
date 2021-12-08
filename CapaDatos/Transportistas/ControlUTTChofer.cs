using AccesoDatos;
using CapaDatos.Frecuencias;
using CapaDatos.UsuariosPerfiles;
using System;
using System.Data;

namespace CapaDatos.Transportistas
{
    public partial class ControlUTTChofer : SqlConnecion, IDisposable
    {
        private long _Id_Unidad_Transporte;
        private long _Id_Frecuencia;
        private long _Id_Chofer;
        private long _Id_Usuario;
        private long _Id_Agente;
        private Unidad_transporte _Unidad_Transporte;
        private Conductor _Chofer;
        private Frecuencia _Frecuencia;
        private Usuarios _Usuario;
        private AgenteTransito _Agente;

        #region Public Properties
        public long Id_Control { get; set; }
        public long Id_Unidad_Transporte
        {
            get { return _Id_Unidad_Transporte; }
            set
            {
                _Id_Unidad_Transporte = value;
                _Unidad_Transporte = null;
            }
        }
        public long Id_Frecuencia
        {
            get { return _Id_Frecuencia; }
            set
            {
                _Id_Frecuencia = value;
                _Frecuencia = null;
            }
        }
        public long Id_Chofer
        {
            get { return _Id_Chofer; }
            set
            {
                _Id_Chofer = value;
                _Chofer = null;
            }
        }
        public long Id_Agente
        {
            get { return _Id_Agente; }
            set
            {
                _Id_Agente = value;
                _Agente = null;
            }
        }
        public long Id_Usuario
        {
            get { return _Id_Usuario; }
            set
            {
                _Id_Usuario = value;
                _Usuario = null;
            }
        }
        public DateTime Fecha_Hora { get; set; }
        public bool AprobadoUTT { get; set; }
        public bool AprobadoChofer { get; set; }
        public bool Usado { get; set; }
        public string NombreAgente { get; set; }
        public Unidad_transporte Unidad_Transporte
        {
            get
            {
                if (_Unidad_Transporte != null && _Unidad_Transporte.Id_Unidad_Transporte != 0)
                    return _Unidad_Transporte;
                else if (_Id_Unidad_Transporte != 0)
                    return _Unidad_Transporte = new Unidad_transporte(_Id_Unidad_Transporte);
                else
                    return null;
            }
        }
        public Conductor Chofer
        {
            get
            {
                if (_Chofer != null && _Chofer.Id_Chofer != 0)
                    return _Chofer;
                else if (_Id_Chofer != 0)
                    return _Chofer = new Conductor(_Id_Chofer);
                else
                    return null;
            }
        }
        public AgenteTransito Agente
        {
            get
            {
                if (_Agente != null && _Agente.Id_Agente != 0)
                    return _Agente;
                else if (_Id_Agente != 0)
                    return _Agente = new AgenteTransito(_Id_Agente);
                else
                    return null;
            }
        }
        public Frecuencia Frec
        {
            get
            {
                if (_Frecuencia != null && _Frecuencia.Id_Frecuencia != 0)
                    return _Frecuencia;
                else if (_Id_Frecuencia != 0)
                    return _Frecuencia = new Frecuencia(_Id_Frecuencia);
                else
                    return null;
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
        public string NombreOperadora { get; set; }
        public string NumDisco { get; set; }
        public string NumPlaca { get; set; }
        public string NumTag { get; set; }
        public string NombreConductor { get; set; }
        public string CiudadDestino { get; set; }
        public string HoraSalida { get; set; }
        #endregion

        public ControlUTTChofer()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public ControlUTTChofer(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Control = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Control = Convert.ToInt64(dt.Rows[0]["Id_Control"]);
                    _Id_Unidad_Transporte = Convert.ToInt64(dt.Rows[0]["Id_Unidad_Transporte"]);
                    _Id_Frecuencia = dt.Rows[0]["Id_Frecuencia"] == DBNull.Value ? 0 : Convert.ToInt64(dt.Rows[0]["Id_Frecuencia"]);
                    _Id_Chofer = Convert.ToInt64(dt.Rows[0]["Id_Chofer"]);
                    _Id_Agente = Convert.ToInt64(dt.Rows[0]["Id_Agente"]);
                    _Id_Usuario = Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
                    Fecha_Hora = Convert.ToDateTime(dt.Rows[0]["Fecha_Hora"]);
                    AprobadoUTT = Convert.ToBoolean(dt.Rows[0]["AprobadoUTT"]);
                    AprobadoChofer = Convert.ToBoolean(dt.Rows[0]["AprobadoChofer"]);
                    Usado = Convert.ToBoolean(dt.Rows[0]["Usado"]);
                    NombreAgente = Convert.ToString(dt.Rows[0]["NombreAgente"]);
                    NombreOperadora = Convert.ToString(dt.Rows[0]["Operadora"]);
                    NumDisco = Convert.ToString(dt.Rows[0]["Disco"]);
                    NumPlaca = Convert.ToString(dt.Rows[0]["Placa"]);
                    NumTag = Convert.ToString(dt.Rows[0]["Tag"]);
                    NombreConductor = Convert.ToString(dt.Rows[0]["Conductor"]);
                    CiudadDestino = Convert.ToString(dt.Rows[0]["CiudadDestino"]);
                    HoraSalida = Convert.ToString(dt.Rows[0]["HoraSalida"]);
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
            return String.Format("SELECT Id_Control, Id_Unidad_Transporte, Id_Frecuencia, Id_Chofer, Id_Agente, Id_Usuario, Fecha_Hora, AprobadoUTT, " +
                "AprobadoChofer, Usado, NombreAgente, Operadora, Disco, Placa, Tag, Conductor, CiudadDestino, HoraSalida FROM CONTROL_UTT_CHOFER");
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

        public static Conductor GetConductor(long id)
        {
            return new Conductor(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Control", "CONTROL_UTT_CHOFER", "");
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
            return String.Format("INSERT INTO CONTROL_UTT_CHOFER (Id_Unidad_Transporte, Id_Frecuencia, Id_Chofer, Id_Usuario, Fecha_Hora, AprobadoUTT, " +
                "AprobadoChofer, Usado, Id_Agente, NombreAgente, Operadora, Disco, Placa, Tag, Conductor, CiudadDestino, HoraSalida) " +
                "VALUES('{0}', '{1}', '{2}', '{3}', GETDATE(), '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}'); ", 
                Id_Unidad_Transporte, Id_Frecuencia, Id_Chofer, Id_Usuario, AprobadoUTT, AprobadoChofer, Usado, Id_Agente, NombreAgente, NombreOperadora, 
                NumDisco, NumPlaca, NumTag, NombreConductor, CiudadDestino, HoraSalida);
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
            return String.Format("DELETE FROM CONTROL_UTT_CHOFER WHERE Id_Control = {0};", ID);
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
            return String.Format("UPDATE CONTROL_UTT_CHOFER SET Id_Unidad_Transporte = '{1}', Id_Frecuencia = '{2}', Id_Chofer = '{3}', " +
                "Id_Usuario = '{4}', Fecha_Hora = '{5}', AprobadoUTT = '{6}', AprobadoChofer = '{7}', Usado = '{8}', Id_Agente = '{9}', " +
                "NombreAgente = '{10}', Operadora = '{11}', Disco = '{12}', Placa = '{13}', Tag = '{14}', Conductor = '{15}', CiudadDestino = '{16}', " +
                "HoraSalida = '{17}' WHERE Id_Control = {0};", Id_Control, Id_Unidad_Transporte, Id_Frecuencia, Id_Chofer, Id_Usuario, 
                Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora), AprobadoUTT, AprobadoChofer, Usado, 
                Id_Agente, NombreAgente, NombreOperadora, NumDisco, NumPlaca, NumTag, NombreConductor, CiudadDestino, HoraSalida);
        }

        public static ControlUTTChofer BuscarControlUTT(long id_Unidad_Transporte)
        {
            string sql = string.Format("SELECT ISNULL((SELECT TOP 1 Id_Control FROM CONTROL_UTT_CHOFER WHERE Id_Unidad_Transporte = {0} AND " +
                "Usado = 0 AND DATEDIFF(mi, Fecha_Hora, GETDATE()) < (SELECT CAST(Valor AS INT) FROM Configuracion_Global WHERE Configuracion = 'MINUTOS') " +
                "ORDER BY Fecha_Hora DESC), 0)", id_Unidad_Transporte);
            long idcontrol = 0;

            if (long.TryParse(SqlServer.EXEC_SCALAR(sql).ToString(), out idcontrol))
                if (idcontrol != 0)
                    return new ControlUTTChofer(idcontrol);

            return null;
        }

        public static ControlUTTChofer BuscarControlChofer(long id_chofer)
        {
            string sql = string.Format("SELECT ISNULL((SELECT TOP 1 Id_Control FROM CONTROL_UTT_CHOFER WHERE Id_Chofer = {0} AND Usado = 0 AND " +
                "DATEDIFF(mi, Fecha_Hora, GETDATE()) < (SELECT CAST(Valor AS INT) FROM Configuracion_Global WHERE Configuracion = 'MINUTOS') " +
                "ORDER BY Fecha_Hora DESC), 0)", id_chofer);
            long idcontrol = 0;

            if (long.TryParse(SqlServer.EXEC_SCALAR(sql).ToString(), out idcontrol))
                if (idcontrol != 0)
                    return new ControlUTTChofer(idcontrol);

            return null;
        }

        public static ControlUTTChofer BuscarControlUTTChofer(long id_Unidad_Transporte, long id_conductor)
        {
            string sql = string.Format("SELECT ISNULL((SELECT TOP 1 Id_Control FROM CONTROL_UTT_CHOFER WHERE Id_Unidad_Transporte = {0} AND Id_Chofer = {1} AND Usado = 0 AND DATEDIFF(mi, Fecha_Hora, GETDATE()) < (SELECT CAST(Valor AS INT) FROM Configuracion_Global WHERE Configuracion = 'MINUTOS') ORDER BY Fecha_Hora DESC), 0)", id_Unidad_Transporte, id_conductor);
            long idcontrol = 0;

            if (long.TryParse(SqlServer.EXEC_SCALAR(sql).ToString(), out idcontrol))
                if (idcontrol != 0)
                    return new ControlUTTChofer(idcontrol);

            return null;
        }

        public string ObtenerDetalleControles(long id_control)
        {
            try
            {
                return SqlServer.EXEC_SCALAR(string.Format("SELECT dbo.concatena_detalle_control_utt({0})", id_control)).ToString();
            }
            catch
            {
                return "";
            }
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
        ~ControlUTTChofer()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
