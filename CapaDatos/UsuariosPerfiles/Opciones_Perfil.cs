using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.UsuariosPerfiles
{
    public partial class Opciones_Perfil : SqlConnecion, IDisposable
    {
        private long _Id_Perfil;
        private long _Id_Modulo;
        private Perfilusuario _Perfil;
        private Modulo _Modulo;

        #region Public Properties
        public long Id_Opciones_Perfil { get; set; }
        public long Id_Perfil
        {
            get { return _Id_Perfil; }
            set { 
                _Id_Perfil = value;
                _Perfil = null;
            }
        }
        public long Id_Modulo
        {
            get { return _Id_Modulo; }
            set { 
                _Id_Modulo = value;
                _Modulo = null;
            }
        }
        public Perfilusuario Perfil
        {
            get {
                if (_Perfil != null && _Perfil.Id_Perfil != 0)
                    return _Perfil;
                else if (_Id_Perfil != 0)
                    return _Perfil = new Perfilusuario(_Id_Perfil);
                else
                    return null;
            }
        }
        public Modulo Modulo
        {
            get
            {
                if (_Modulo != null && _Modulo.Id_Modulo != 0)
                    return _Modulo;
                else if (_Id_Modulo != 0)
                    return _Modulo = new Modulo(_Id_Modulo);
                else
                    return null;
            }
        }
        #endregion

        public Opciones_Perfil()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Opciones_Perfil(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Opciones_Perfil = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Opciones_Perfil = Convert.ToInt64(dt.Rows[0]["Id_Opciones_Perfil"]);
                    _Id_Perfil = Convert.ToInt64(dt.Rows[0]["Id_Perfil"]);
                    _Id_Modulo = Convert.ToInt64(dt.Rows[0]["Id_Modulo"]);
                }
            }
            //throw new ApplicationException("Opciones_perfil does not exist.");
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
            return String.Format("SELECT Id_Opciones_Perfil,Id_Perfil,Id_Modulo FROM OPCIONES_PERFIL");
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

        public static Opciones_Perfil GetOpciones_perfil(long id)
        {
            return new Opciones_Perfil(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Opciones_Perfil", "Opciones_perfil", "");
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
            return String.Format("INSERT INTO OPCIONES_PERFIL (Id_Perfil, Id_Modulo) VALUES( '{0}','{1}'); ", Id_Perfil, Id_Modulo);
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
            return String.Format("DELETE FROM OPCIONES_PERFIL WHERE Id_Opciones_Perfil = {0};", ID);
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
            return String.Format("UPDATE OPCIONES_PERFIL SET Id_Perfil= '{1}', Id_Modulo= '{2}' " +
                "WHERE Id_Opciones_Perfil = {0};", Id_Opciones_Perfil, Id_Perfil, Id_Modulo);
        }

        public bool IsDuplicate_Opcion(long Perf, long Mod)
        {
            return IsDuplicate_Opcion(0, Perf, Mod);
        }

        public bool IsDuplicate_Opcion(long id, long Perf, long Mod)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Opciones_Perfil) FROM OPCIONES_PERFIL WHERE Id_Perfil = '{0}' AND Id_Modulo = '{1}' {2}", Perf, Mod, id != 0 ? "AND Id_Opciones_Perfil <> " + id.ToString() : ""))) > 0;
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
        ~Opciones_Perfil()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
