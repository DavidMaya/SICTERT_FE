using AccesoDatos;
using CapaDatos.UsuariosPerfiles;
using System;
using System.Data;

namespace CapaDatos.AreasEquipos
{
    public class Apertura_Barrera_PP : SqlConnecion, IDisposable
    {
        private long _Id_Usuario;
        private long _Id_Motivo;
        private long _Id_Equipo;
        private Equipo _Equipo;
        private Motivo_apertura_manual _Motivo;
        private Usuarios _Usuario;

        #region Public Properties
        public long Id_Apertura_Manual { get; set; }
        public long Id_Usuario
        {
            get { return _Id_Usuario; }
            set { 
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
        public long Id_Motivo
        {
            get { return _Id_Motivo; }
            set { 
                _Id_Motivo = value;
                _Motivo = null;
            }
        }
        public Motivo_apertura_manual Motivo
        {
            get
            {
                if (_Motivo != null && _Motivo.Id_Motivo_Apertura != 0)
                    return _Motivo;
                else if (_Id_Motivo != 0)
                    return _Motivo = new Motivo_apertura_manual(_Id_Motivo);
                else
                    return null;
            }
        }
        public string Observacion { get; set; }
        public long Id_Equipo
        {
            get { return _Id_Equipo; }
            set
            {
                _Id_Equipo = value;
                _Equipo = null;
            }
        }
        public Equipo equipo
        {
            get
            {
                if (_Equipo != null && _Equipo.Id_Equipo != 0)
                    return _Equipo;
                else if (_Id_Equipo != 0)
                    return _Equipo = new Equipo(_Id_Equipo);
                else
                    return null;
            }
        }
        public bool Es_Manual { get; set; }
        public string Ticket { get; set; }
        public long Id_Tran { get; set; }
        public DateTime Fecha_Hora { get; set; }
        public DateTime Fecha_Hora_Opto { get; set; }
        #endregion

        public Apertura_Barrera_PP()
        {
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
            return String.Format("SELECT Id_Apertura_Manual, Id_Equipo, Id_Motivo, Id_Usuario, Observacion, Fecha_Hora, Es_Manual, Ticket, Id_Tran, Fecha_Hora_Opto FROM Apertura_Barrera_PP");
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

        public string Insert()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }

        public string GetSQLInsert()
        {
            return String.Format("INSERT INTO Apertura_Barrera_PP (Id_Usuario, Fecha_Hora, Id_Motivo, Id_Tran, Observacion, Id_Equipo, Es_Manual, Ticket, Fecha_Hora_Opto ) " +
                " VALUES( '{0}', GETDATE(), {1}, {2}, '{3}', {4}, '{5}', '{6}', NULL); ", Id_Usuario, Id_Motivo, (Id_Tran != 0) ? Id_Tran.ToString() : "NULL",
                SqlServer.ValidarTexto(Observacion), Id_Equipo, (Es_Manual) ? 1 : 0, Ticket);
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
        ~Apertura_Barrera_PP()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
