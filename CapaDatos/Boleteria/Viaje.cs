using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaDatos.Boleteria
{
    public partial class Viaje : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long IdViaje { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string FechaSalidaSQL 
        {
            get
            {
                return FechaSalida == null ? 
                    (String)null :
                    Convert.ToDateTime(FechaSalida.ToString()).ToString(SqlServer.FormatofechaHora);
            }
        }
        public long IdFrecuencia { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public string CiudadOrigen { get; set; }
        public string CiudadDestino { get; set; }
        public long IdEstadoViaje { get; set; }
        public long IdUnidadTransporte { get; set; }
        public long IdTipoBus { get; set; }
        public long IdChofer { get; set; }
        public string Chofer { get; set; }
        #endregion

        #region Constructor
        public Viaje(){ }

        public Viaje(long id)
        {
            string sql = String.Format(@"SELECT Id_Viaje, Fecha, Id_Frecuencia, Hora_Salida, Ciudad_Origen, 
                Ciudad_Destino, Id_Estado_Viaje, Id_Unidad_Transporte, Id_Tipo_Bus, Id_Chofer, Chofer
                FROM VIAJE WHERE Id_Viaje = {0}", id);

            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                if (table.Rows.Count > 0)
                {
                    IdViaje = Convert.ToInt64(table.Rows[0]["Id_Viaje"]);
                    FechaSalida = ValorDateTime(table.Rows[0]["Fecha"]);
                    IdFrecuencia = Convert.ToInt64(table.Rows[0]["Id_Frecuencia"]);
                    HoraSalida = (TimeSpan)table.Rows[0]["Hora_Salida"];
                    CiudadOrigen = Convert.ToString(table.Rows[0]["Ciudad_Origen"]);
                    CiudadDestino = Convert.ToString(table.Rows[0]["Ciudad_Destino"]);
                    IdEstadoViaje = Convert.ToInt64(table.Rows[0]["Id_Estado_Viaje"]);
                    IdUnidadTransporte = table.Rows[0]["Id_Unidad_Transporte"] == DBNull.Value ? 0 : Convert.ToInt64(table.Rows[0]["Id_Unidad_Transporte"]);
                    IdTipoBus = table.Rows[0]["Id_Tipo_Bus"] == DBNull.Value ? 0 : Convert.ToInt64(table.Rows[0]["Id_Tipo_Bus"]);
                    IdChofer = table.Rows[0]["Id_Chofer"] == DBNull.Value ? 0 : Convert.ToInt64(table.Rows[0]["Id_Chofer"]);
                    Chofer = table.Rows[0]["Chofer"] == DBNull.Value ? "" : table.Rows[0]["Chofer"].ToString();
                }
            }
        }
        #endregion

        #region Funciones
        private Nullable<DateTime> ValorDateTime(object ValorFecha)
        {
            if (string.IsNullOrEmpty(ValorFecha.ToString()))
                return null;
            else
                return Convert.ToDateTime(ValorFecha);
        }
        #endregion

        #region Select Data Method
        private static string GetSqlSelect()
        {
            return String.Format(@"SELECT Id_Viaje, Fecha, Id_Frecuencia, Hora_Salida, Ciudad_Origen, 
                Ciudad_Destino, Id_Estado_Viaje, Id_Unidad_Transporte, Id_Tipo_Bus, Id_Chofer, Chofer
                FROM VIAJE");
        }

        public static DataTable GetAllData()
        {
            return GetAllData("");
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

        public static EstadoAsiento getEstadoAsiento(long id)
        {
            return new EstadoAsiento(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Estado_Asiento", "ESTADO_ASIENTO", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }
        #endregion

        #region Insert Data Method
        public string Insert()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }

        private string GetSQLInsert()
        {
            string sql = String.Format(@"INSERT INTO VIAJE (Fecha, Id_Frecuencia, Hora_Salida, Ciudad_Origen,
                Ciudad_Destino, Id_Estado_Viaje, Id_Unidad_Transporte, Id_Tipo_Bus, Id_Chofer, Chofer)
                VALUES ('{0}', {1}, '{2}', '{3}', '{4}', {5}, {6}, {7}, {8}, '{9}')",
                Convert.ToDateTime(FechaSalida.ToString()).ToString(SqlServer.Formatofecha), IdFrecuencia,
                Convert.ToDateTime(HoraSalida.ToString()).ToString(SqlServer.FormatoHora), CiudadOrigen, CiudadDestino, IdEstadoViaje,
                IdUnidadTransporte == 0 ? "NULL" : IdUnidadTransporte.ToString(), IdTipoBus == 0 ? "NULL" : IdTipoBus.ToString(),
                IdChofer == 0 ? "NULL" : IdChofer.ToString(), Chofer == "" ? "NULL" : Chofer);
            return sql;
        }
        #endregion

        #region Update Data Method
        public string Update()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate());
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;

        }
        public string GetSQLUpdate()
        {
            return String.Format(@"UPDATE VIAJE SET Fecha = '{0}', Id_Frecuencia = {1}, Hora_Salida = '{2}', Ciudad_Origen = '{3}', Ciudad_Destino = '{4}',
                Id_Estado_Viaje = {5}, Id_Unidad_Transporte = {6}, Id_Tipo_Bus = {7}, Id_Chofer = {8}, Chofer = '{9}' WHERE Id_Viaje = {10}",
               Convert.ToDateTime(FechaSalida.ToString()).ToString(SqlServer.Formatofecha), IdFrecuencia, HoraSalida, CiudadOrigen, CiudadDestino,
               IdEstadoViaje, IdUnidadTransporte == 0 ? "null" : IdUnidadTransporte.ToString(), IdTipoBus == 0 ? "null" : IdTipoBus.ToString(),
               IdChofer == 0 ? "NULL" : IdChofer.ToString(), Chofer == "" ? "NULL" : Chofer, IdViaje);
        }
        #endregion

        #region Delete Data Method
        public string Delete(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        private string GetSQLDelete(long id)
        {
            return String.Format("DELETE FROM VIAJE WHERE Id_Viaje = {0}", id);
        }
        #endregion

        #region Control Datos Duplicados

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
        ~Viaje()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
