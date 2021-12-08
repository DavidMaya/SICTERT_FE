using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Publicidad
{
    public class HorarioPautaje : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long IdHorarioPautaje { get; set; }
        public string Nombre { get; set; }
        public TimeSpan HoraDesde { get; set; }
        public TimeSpan HoraHasta { get; set; }
        public bool Activo { get; set; }
        public int Minutos { 
            get
            {
                int diferencia = Convert.ToInt32((HoraHasta - HoraDesde).TotalMinutes);
                if (diferencia < 0)
                    return Convert.ToInt32(((DateTime.Now.AddDays(1) + HoraHasta) - (DateTime.Now + HoraDesde)).TotalMinutes);
                return diferencia;
            }
        }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public HorarioPautaje()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public HorarioPautaje(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Horario_Pautaje = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    IdHorarioPautaje = Convert.ToInt64(dt.Rows[0]["Id_Horario_Pautaje"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    HoraDesde = (TimeSpan)dt.Rows[0]["HoraDesde"];
                    HoraHasta = (TimeSpan)dt.Rows[0]["HoraHasta"];
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
            return String.Format(@"SELECT Id_Horario_Pautaje, Nombre, HoraDesde, HoraHasta, Activo, Fecha_Creacion, Usuario_Creacion, 
                Fecha_Modificacion, Usuario_Modificacion FROM HORARIO_PAUTAJE");
        }

        public static DataTable GetAllData()
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect());
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

        public static HorarioPautaje Get_HorarioPautaje(long id)
        {
            return new HorarioPautaje(id);
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
            return String.Format("INSERT INTO HORARIO_PAUTAJE (Nombre, HoraDesde, HoraHasta, Activo, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, " +
                "Usuario_Modificacion) VALUES ('{0}', '{1}', '{2}', '{3}', GETDATE(), {4}, {5}, {6}); ",
                SqlServer.ValidarTexto(Nombre), HoraDesde, HoraHasta, Activo, Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM HORARIO_PAUTAJE WHERE Id_Horario_Pautaje = {0};", ID);
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
            return String.Format("UPDATE HORARIO_PAUTAJE SET Nombre = '{1}', HoraDesde = '{2}', HoraHasta = '{3}', Activo = '{4}', Fecha_Creacion = {5}, " +
                "Usuario_Creacion = {6}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {7} WHERE Id_Horario_Pautaje = {0};", 
                IdHorarioPautaje, SqlServer.ValidarTexto(Nombre), HoraDesde, HoraHasta, Activo,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Horario_Pautaje) FROM HORARIO_PAUTAJE " +
                "WHERE Activo = 1 AND Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Horario_Pautaje <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_HoraDesde(TimeSpan Min)
        {
            return IsDuplicate_HoraDesde(0, Min);
        }

        public bool IsDuplicate_HoraDesde(long id, TimeSpan Min)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Horario_Pautaje) FROM HORARIO_PAUTAJE " +
                "WHERE HoraDesde = '{0}' {1}", Min, id != 0 ? "AND Id_Horario_Pautaje <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_HoraHasta(TimeSpan Max)
        {
            return IsDuplicate_HoraHasta(0, Max);
        }

        public bool IsDuplicate_HoraHasta(long id, TimeSpan Max)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Horario_Pautaje) FROM HORARIO_PAUTAJE " +
                "WHERE HoraHasta = '{0}' {1}", Max, id != 0 ? "AND Id_Horario_Pautaje <> " + id.ToString() : ""))) > 0;
        }

        public bool IsOverlap_Hora(TimeSpan Min, TimeSpan Max)
        {
            return IsOverlap_Hora(0, Min, Max);
        }

        public bool IsOverlap_Hora(long id, TimeSpan Min, TimeSpan Max)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Horario_Pautaje) FROM HORARIO_PAUTAJE " +
                "WHERE CASE WHEN '{0}' > HoraDesde THEN '{0}' ELSE HoraDesde END <= CASE WHEN '{1}' < HoraHasta THEN '{1}' ELSE HoraHasta END {2}", 
                Min, Max, id != 0 ? "AND Id_Horario_Pautaje <> " + id.ToString() : ""))) > 0;
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
        ~HorarioPautaje()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
