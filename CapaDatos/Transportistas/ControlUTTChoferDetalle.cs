using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Transportistas
{
    public partial class ControlUTTChoferDetalle : SqlConnecion, IDisposable
    {
        private long _Id_Control;
        private long _Id_ParamControl;
        private ControlUTTChofer _Control;
        private ParametroControl _Parametro;

        #region Public Properties
        public long Id_Control
        {
            get { return _Id_Control; }
            set { 
                _Id_Control = value;
                _Control = null;
            }
        }
        public long Id_ParamControl
        {
            get { return _Id_ParamControl; }
            set { 
                _Id_ParamControl = value;
                _Parametro = null;
            }
        }
        public ControlUTTChofer Control
        {
            get
            {
                if (_Control != null && _Control.Id_Control != 0)
                    return _Control;
                else if (_Id_Control != 0)
                    return _Control = new ControlUTTChofer(_Id_Control);
                else
                    return null;
            }
        }
        public ParametroControl Parametro
        {
            get
            {
                if (_Parametro != null && _Parametro.Id_ParametroControl != 0)
                    return _Parametro;
                else if (_Id_ParamControl != 0)
                    return _Parametro = new ParametroControl(_Id_ParamControl);
                else
                    return null;
            }
        }
        public bool Aprobado { get; set; }
        #endregion

        public ControlUTTChoferDetalle()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public ControlUTTChoferDetalle(long idC, long idPC)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Control = {0} AND Id_ParamControl = {1}", idC, idPC)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    _Id_Control = Convert.ToInt64(dt.Rows[0]["Id_Control"]);
                    _Id_ParamControl = Convert.ToInt64(dt.Rows[0]["Id_ParamControl"]);
                    Aprobado = Convert.ToBoolean(dt.Rows[0]["Aprobado"]);
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
            return String.Format("SELECT Id_Control, Id_ParamControl, Aprobado FROM CONTROL_UTT_CHOFER_DETALLE");
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
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Control", "CONTROL_UTT_CHOFER_DETALLE", "");
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
            return String.Format("INSERT INTO CONTROL_UTT_CHOFER_DETALLE (Id_Control, Id_ParamControl, Aprobado) " +
                "VALUES('{0}', '{1}', '{2}'); ", Id_Control, Id_ParamControl, Aprobado);
        }

        public string Delete(long id, long idPC)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id, idPC));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete(long ID, long idPC)
        {
            return String.Format("DELETE FROM CONTROL_UTT_CHOFER_DETALLE WHERE Id_Control = {0} AND Id_ParamControl = {1};", ID, idPC);
        }

        public string Update(long id, long idPC)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate(id, idPC));
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;
        }

        public string GetSQLUpdate(long ID, long idPC)
        {
            return String.Format("UPDATE CONTROL_UTT_CHOFER_DETALLE SET Aprobado = '{2}' " +
                "WHERE Id_Control = {0} AND Id_ParamControl = {1};", ID, idPC, Aprobado);
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
        ~ControlUTTChoferDetalle()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
