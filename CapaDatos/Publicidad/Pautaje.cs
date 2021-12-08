using AccesoDatos;
using CapaDatos.Ingresos;
using CapaDatos.Recaudacion;
using System;
using System.Data;

namespace CapaDatos.Publicidad
{
    public class Pautaje : SqlConnecion, IDisposable
    {
        private long _Id_ClienteFinal;
        private long _Id_HorarioPautaje;
        private long _Id_FacturaRecauda;
        private Cliente_Final _Cliente_final;
        private HorarioPautaje _HorarioPautaje;
        private FacturaRecauda _FacturaRecauda;

        #region Public Properties
        public long IdPautaje { get; set; }
        public long IdClienteFinal
        {
            get { return _Id_ClienteFinal; }
            set
            {
                _Id_ClienteFinal = value;
                _Cliente_final = null;
            }
        }
        public Cliente_Final Cliente_final
        {
            get
            {
                if (_Cliente_final != null && _Cliente_final.Id_Cliente_Final != 0)
                    return _Cliente_final;
                else if (_Id_ClienteFinal != 0)
                    return _Cliente_final = new Cliente_Final(_Id_ClienteFinal);
                else
                    return null;
            }
        }
        public long IdFacturaRecauda
        {
            get { return _Id_FacturaRecauda; }
            set
            {
                _Id_FacturaRecauda = value;
                _FacturaRecauda = null;
            }
        }
        public FacturaRecauda Factura_recauda
        {
            get
            {
                if (_FacturaRecauda != null && _FacturaRecauda.IdFacturaRecauda != 0)
                    return _FacturaRecauda;
                else if (_Id_FacturaRecauda != 0)
                    return _FacturaRecauda = new FacturaRecauda(_Id_FacturaRecauda);
                else
                    return null;
            }
        }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int CantidadMostrar { get; set; }
        public int CantidadMostrada { get; set; }
        public string Url { get; set; }
        public TimeSpan HoraDesde { get; set; }
        public TimeSpan HoraHasta { get; set; }
        public int CantidadTotal { get; set; }
        public int ContadorTotal { get; set; }
        public int Tipo { get; set; }
        public bool Activo { get; set; }
        public long IdHorarioPautaje
        {
            get { return _Id_HorarioPautaje; }
            set
            {
                _Id_HorarioPautaje = value;
                _HorarioPautaje = null;
            }
        }
        public HorarioPautaje Horario_pautaje
        {
            get
            {
                if (_HorarioPautaje != null && _HorarioPautaje.IdHorarioPautaje != 0)
                    return _HorarioPautaje;
                else if (_Id_HorarioPautaje != 0)
                    return _HorarioPautaje = new HorarioPautaje(_Id_HorarioPautaje);
                else
                    return null;
            }
        }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Pautaje() { }

        public Pautaje(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Pautaje = {0}", id)))
            {
                IdPautaje = Convert.ToInt64(dt.Rows[0]["Id_Pautaje"]);
                _Id_ClienteFinal = Convert.ToInt64(dt.Rows[0]["Id_ClienteFinal"]);
                _Id_FacturaRecauda = dt.Rows[0]["Id_FacturaRecauda"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_FacturaRecauda"]) : 0;
                FechaDesde = Convert.ToDateTime(dt.Rows[0]["FechaDesde"]);
                FechaHasta = Convert.ToDateTime(dt.Rows[0]["FechaHasta"]);
                CantidadMostrar = Convert.ToInt32(dt.Rows[0]["CantidadMostrar"]);
                CantidadMostrada = Convert.ToInt32(dt.Rows[0]["CantidadMostrada"]);
                Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                _Id_HorarioPautaje = dt.Rows[0]["Id_Horario_Pautaje"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Horario_Pautaje"]) : 0;
                Url = dt.Rows[0]["Url"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Url"]) : "";
                Tipo = dt.Rows[0]["Tipo"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Tipo"]) : -1;
                HoraDesde = (TimeSpan)dt.Rows[0]["HoraDesde"];
                HoraHasta = (TimeSpan)dt.Rows[0]["HoraHasta"];
                CantidadTotal = Convert.ToInt32(dt.Rows[0]["CantidadTotal"]);
                ContadorTotal = Convert.ToInt32(dt.Rows[0]["ContadorTotal"]);
                Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
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
            return String.Format(@"SELECT Id_Pautaje, Id_ClienteFinal, Id_FacturaRecauda, FechaDesde, FechaHasta, CantidadMostrar, CantidadMostrada, Activo,
                Id_Horario_Pautaje, Url, Tipo, HoraDesde, HoraHasta, CantidadTotal, ContadorTotal, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, 
                Usuario_Modificacion FROM PAUTAJE");
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

        public static HorarioPautaje GetHorario_Pautaje(long id)
        {
            return new HorarioPautaje(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Pautaje", "PAUTAJE", "");
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
            return String.Format(@"INSERT INTO PAUTAJE (Id_ClienteFinal, Id_FacturaRecauda, FechaDesde, FechaHasta, CantidadMostrar, CantidadMostrada, 
                Activo, Id_Horario_Pautaje, Url, Tipo, HoraDesde, HoraHasta, CantidadTotal, ContadorTotal, Fecha_Creacion, Usuario_Creacion, 
                Fecha_Modificacion, Usuario_Modificacion)
                VALUES ({0}, {1}, '{2}', '{3}', {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, GETDATE(), {14}, {15}, {16})",
                IdPautaje, IdFacturaRecauda == 0 ? "NULL" : IdFacturaRecauda.ToString(), Convert.ToDateTime(FechaDesde.ToString()).ToString(SqlServer.Formatofecha),
                Convert.ToDateTime(FechaHasta.ToString()).ToString(SqlServer.Formatofecha), CantidadMostrar, CantidadMostrada, Activo ? 1 : 0, 
                IdHorarioPautaje == 0 ? "NULL" : IdHorarioPautaje.ToString(), string.IsNullOrEmpty(Url) ? "NULL" : "'" + Url + "'", 
                Tipo, Convert.ToDateTime(HoraDesde.ToString()).ToString(SqlServer.FormatofechaHoraMinuto), 
                Convert.ToDateTime(HoraHasta.ToString()).ToString(SqlServer.FormatofechaHoraMinuto), CantidadTotal, ContadorTotal, 
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
            return String.Format("DELETE FROM PAUTAJE WHERE Id_Pautaje = {0};", ID);
        }

        public string Update(bool min = false)
        {
            string _return = SqlServer.EXEC_COMMAND(min ? GetSQLUpdateMin() : GetSQLUpdate());
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;
        }        

        public string GetSQLUpdate()
        {
            return String.Format(@"UPDATE PAUTAJE SET Id_ClienteFinal = {0}, Id_FacturaRecauda = {1}, FechaDesde = '{2}', FechaHasta = '{3}',
                CantidadMostrar = {4}, CantidadMostrada = {5}, Activo = {6}, Id_Horario_Pautaje = {7}, Url = {8}, Tipo = {9},
                HoraDesde = '{10}', HoraHasta = '{11}', CantidadTotal = {12}, ContadorTotal  = {13}, Fecha_Creacion = {14}, Usuario_Creacion = {15}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {16} WHERE Id_Pautaje = {17}",
                IdClienteFinal, IdFacturaRecauda == 0 ? "NULL" : IdFacturaRecauda.ToString(), 
                Convert.ToDateTime(FechaDesde.ToString()).ToString(SqlServer.Formatofecha),
                Convert.ToDateTime(FechaHasta.ToString()).ToString(SqlServer.Formatofecha), CantidadMostrar, CantidadMostrada, Activo ? 1 : 0,
                IdHorarioPautaje == 0 ? "NULL" : IdHorarioPautaje.ToString(), string.IsNullOrEmpty(Url) ? "NULL" : "'" + Url + "'", 
                Tipo, HoraDesde, HoraHasta, CantidadTotal, ContadorTotal,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'", 
                IdPautaje);
        }
        public string GetSQLUpdateMin()
        {
            return String.Format(@"UPDATE PAUTAJE SET Id_ClienteFinal = {0}, Id_FacturaRecauda = {1}, FechaDesde = '{2}', FechaHasta = '{3}',
                CantidadMostrar = {4}, Activo = {5}, Id_Horario_Pautaje = {6}, Url = {7}, Tipo = {8}, CantidadTotal = {9}, HoraDesde = '{10}', HoraHasta = '{11}'  
                WHERE Id_Pautaje = {12}",
                IdClienteFinal, IdFacturaRecauda == 0 ? "NULL" : IdFacturaRecauda.ToString(), Convert.ToDateTime(FechaDesde.ToString()).ToString(SqlServer.Formatofecha),
                Convert.ToDateTime(FechaHasta.ToString()).ToString(SqlServer.Formatofecha), CantidadMostrar, Activo ? 1 : 0,
                IdHorarioPautaje == 0 ? "NULL" : IdHorarioPautaje.ToString(), string.IsNullOrEmpty(Url) ? "NULL" : "'" + Url + "'", 
                Tipo, CantidadTotal, HoraDesde, HoraHasta, IdPautaje);
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
        ~Pautaje()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
