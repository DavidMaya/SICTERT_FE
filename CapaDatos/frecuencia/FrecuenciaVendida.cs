using AccesoDatos;
using CapaDatos.Transportistas;
using System;
using System.Data;

namespace CapaDatos.Frecuencias
{
    public partial class Frecuencia_vendida : SqlConnecion, IDisposable
    {
        private long _Id_Unidad_Transporte;
        private long _Id_ControlUTTChofer;
        private long _Id_ControlBioUTT;
        private long _Id_Factura;
        private long _Id_Frecuencia;
        private long _Id_FrecAnterior;
        private long _Id_Chofer;
        private Unidad_transporte _Unidad_Transporte;
        private Factura _Factura;
        private Frecuencia _Frecuencia;
        private Frecuencia _FrecAnterior;
        private Conductor _Chofer;
        private ControlUTTChofer _ControlUTTChofer;
        private ControlBioseguridadUTT _ControlBioseguridadUTT;

        #region Public Properties
        public long Id_Frecuencia_Vendida { get; set; }
        public long Id_Unidad_Transporte
        {
            get { return _Id_Unidad_Transporte; }
            set
            {
                _Id_Unidad_Transporte = value;
                _Unidad_Transporte = null;
            }
        }
        public long Id_Factura
        {
            get { return _Id_Factura; }
            set
            {
                _Id_Factura = value;
                _Factura = null;
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
        public long Id_FrecActerior
        {
            get { return _Id_FrecAnterior; }
            set
            {
                _Id_FrecAnterior = value;
                _FrecAnterior = null;
            }
        }
        public decimal Valor { get; set; }
        public decimal ValorIva { get; set; }
        public Nullable<DateTime> Fecha { get; set; }
        public bool Anulado { get; set; }
        public long Id_Anden { get; set; }
        public bool Habilitado { get; set; }
        public bool Extra { get; set; }
        public decimal Saldoprepago { get; set; }
        public string Recaudador { get; set; }
        public long Id_Chofer
        {
            get { return _Id_Chofer; }
            set {
                _Id_Chofer = value;
                _Chofer = null;
            }
        }
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
        public Factura factura
        {
            get
            {
                if (_Factura != null && _Factura.Id_Factura != 0)
                    return _Factura;
                else if (_Id_Factura != 0)
                    return _Factura = new Factura(_Id_Factura);
                else
                    return null;
            }
        }
        public Frecuencia frecuencia
        {
            get {
                if (_Frecuencia != null && _Frecuencia.Id_Frecuencia != 0)
                    return _Frecuencia;
                else if (_Id_Frecuencia != 0)
                    return _Frecuencia = new Frecuencia(_Id_Frecuencia);
                else
                    return null;
            }
        }
        public Frecuencia frecAnterior
        {
            get
            {
                if (_FrecAnterior != null && _FrecAnterior.Id_Frecuencia != 0)
                    return _FrecAnterior;
                else if (_Id_FrecAnterior != 0)
                    return _FrecAnterior = new Frecuencia(_Id_FrecAnterior);
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
        public long Numero_Tarifa { get; set; }
        public long Id_ControlUTTChofer
        {
            get { return _Id_ControlUTTChofer; }
            set { 
                _Id_ControlUTTChofer = value;
                _ControlUTTChofer = null;
            }
        }
        public ControlUTTChofer control_utt_chofer
        {
            get
            {
                if (_ControlUTTChofer != null && _ControlUTTChofer.Id_Control != 0)
                    return _ControlUTTChofer;
                else if (_Id_ControlUTTChofer != 0)
                    return _ControlUTTChofer = new ControlUTTChofer(_Id_ControlUTTChofer);
                else
                    return null;
            }
        }
        public long Id_ControlBioUTT
        {
            get { return _Id_ControlBioUTT; }
            set
            {
                _Id_ControlBioUTT = value;
                _ControlBioseguridadUTT = null;
            }
        }
        public ControlBioseguridadUTT control_bio_utt
        {
            get
            {
                if (_ControlBioseguridadUTT != null && _ControlBioseguridadUTT.Id_Control != 0)
                    return _ControlBioseguridadUTT;
                else if (_Id_ControlBioUTT != 0)
                    return _ControlBioseguridadUTT = new ControlBioseguridadUTT(_Id_ControlBioUTT);
                else
                    return null;
            }
        }
        public string Tipo_Tarifa { get; set; }
        public string NombreOperadora { get; set; }
        public string NumDisco { get; set; }
        public string NumPlaca { get; set; }
        public string NumTag { get; set; }
        public string NombreConductor { get; set; }
        public string CiudadOrigen { get; set; }
        public string CiudadDestino { get; set; }
        public string HoraSalida { get; set; }
        public string NumAnden { get; set; }
        public DateTime FechaHoraSalida { get; set; }
        public DateTime FechaHoraAnden { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Frecuencia_vendida()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Frecuencia_vendida(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Frecuencia_Vendida = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Frecuencia_Vendida = Convert.ToInt64(dt.Rows[0]["Id_Frecuencia_Vendida"]);
                    _Id_Unidad_Transporte = Convert.ToInt64(dt.Rows[0]["Id_Unidad_Transporte"]);
                    _Id_Factura = Convert.ToInt64(dt.Rows[0]["Id_Factura"]);
                    _Id_Frecuencia = Convert.ToInt64(dt.Rows[0]["Id_Frecuencia"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["ValorIva"]);
                    Fecha = valorDateTime(dt.Rows[0]["fecha"]);
                    Anulado = Convert.ToBoolean(dt.Rows[0]["anulado"]);
                    Id_Anden = Convert.ToInt64(dt.Rows[0]["Id_Anden"]);
                    Habilitado = Convert.ToBoolean(dt.Rows[0]["Habilitado"]);
                    Saldoprepago = Convert.ToDecimal(dt.Rows[0]["Saldoprepago"]);
                    Recaudador = Convert.ToString(dt.Rows[0]["recaudador"]);
                    _Id_Chofer = Convert.ToInt64(dt.Rows[0]["Id_Chofer"].ToString().Length == 0 ? 0 : dt.Rows[0]["Id_Chofer"]);
                    Numero_Tarifa = dt.Rows[0]["Numero_Tarifa"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Numero_Tarifa"]) : 0;
                    Tipo_Tarifa = Convert.ToString(dt.Rows[0]["Tipo_Tarifa"]);
                    Extra = Convert.ToBoolean(dt.Rows[0]["Extra"]);
                    _Id_ControlUTTChofer = dt.Rows[0]["Id_ControlUTTChofer"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_ControlUTTChofer"]) : 0;
                    NombreOperadora = Convert.ToString(dt.Rows[0]["Operadora"]);
                    NumDisco = Convert.ToString(dt.Rows[0]["Disco"]);
                    NumPlaca = Convert.ToString(dt.Rows[0]["Placa"]);
                    NumTag = Convert.ToString(dt.Rows[0]["Tag"]);
                    NombreConductor = Convert.ToString(dt.Rows[0]["Conductor"]);
                    CiudadOrigen = Convert.ToString(dt.Rows[0]["CiudadOrigen"]);
                    CiudadDestino = Convert.ToString(dt.Rows[0]["CiudadDestino"]);
                    HoraSalida = Convert.ToString(dt.Rows[0]["HoraSalida"]);
                    NumAnden = Convert.ToString(dt.Rows[0]["Anden"]);
                    FechaHoraSalida = Convert.ToDateTime(dt.Rows[0]["FechaHoraSalida"]);
                    FechaHoraAnden = Convert.ToDateTime(dt.Rows[0]["FechaHoraAnden"]);
                    _Id_ControlBioUTT = dt.Rows[0]["Id_ControlBioUTT"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_ControlBioUTT"]) : 0;
                    _Id_FrecAnterior = dt.Rows[0]["Id_FrecAnterior"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_FrecAnterior"]) : 0;
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
            return String.Format("SELECT Id_Frecuencia_Vendida, Id_Unidad_Transporte, Id_Factura, Id_Frecuencia, Valor, ValorIva, fecha, anulado, " +
                "Id_Anden, Habilitado, Saldoprepago, FechaHoraSalida, FechaHoraAnden, Recaudador, Id_Chofer, Tipo_Tarifa, Numero_Tarifa, Extra, " +
                "Id_ControlUTTChofer, Operadora, Disco, Placa, Tag, Conductor, CiudadOrigen, CiudadDestino, HoraSalida, Anden, Id_ControlBioUTT, " +
                "Id_FrecAnterior, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM FRECUENCIA_VENDIDA");
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

        public static Frecuencia_vendida GetFrecuencia_vendida(long id)
        {
            return new Frecuencia_vendida(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Frecuencia_Vendida", "FRECUENCIA_VENDIDA", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        static public long Next_NumeroTarifa(string tipotarifa)
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Numero_Tarifa", "FRECUENCIA_VENDIDA", string.Format("WHERE Tipo_Tarifa ='{0}'", tipotarifa));
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
            return String.Format("INSERT INTO FRECUENCIA_VENDIDA (Id_Frecuencia_Vendida, Id_Unidad_Transporte, Id_Factura, Id_Frecuencia, Valor, " +
                "ValorIva, Fecha, Anulado, Id_Anden, Habilitado, Saldoprepago, Recaudador, Id_Chofer, Tipo_Tarifa, Numero_Tarifa, Extra, " +
                "Id_ControlUTTChofer, Operadora, Disco, Placa, Tag, Conductor, CiudadOrigen, CiudadDestino, HoraSalida, Anden, FechaHoraSalida, " +
                "FechaHoraAnden, Id_ControlBioUTT, Id_FrecAnterior, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, '{7}', '{8}', '{9}', '{10}', '{11}', {12}, '{13}', {14}, '{15}', {16}, " +
                "'{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', {28}, {29}, GETDATE(), {30}, {31}, {32}); ", 
                Id_Frecuencia_Vendida, Id_Unidad_Transporte, Id_Factura, Id_Frecuencia, 
                Valor.ToString().Replace(",", SqlServer.SigFloatSql), ValorIva.ToString().Replace(",", SqlServer.SigFloatSql), 
                (Fecha == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Anulado, 
                Id_Anden, Habilitado, Saldoprepago.ToString().Replace(",", SqlServer.SigFloatSql), 
                Recaudador, Id_Chofer == 0 ? "NULL" : Id_Chofer.ToString(), Tipo_Tarifa, Numero_Tarifa == 0 ? "NULL" : Numero_Tarifa.ToString(), 
                Extra, Id_ControlUTTChofer == 0 ? "NULL" : Id_ControlUTTChofer.ToString(), NombreOperadora, NumDisco, NumPlaca, 
                NumTag, NombreConductor, CiudadOrigen, CiudadDestino, HoraSalida, NumAnden, FechaHoraSalida.ToString(SqlServer.FormatofechaHora), 
                FechaHoraAnden.ToString(SqlServer.FormatofechaHora), Id_ControlBioUTT == 0 ? "NULL" : Id_ControlBioUTT.ToString(), 
                Id_FrecActerior == 0 ? "NULL" : Id_FrecActerior.ToString(), Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM FRECUENCIA_VENDIDA WHERE Id_Frecuencia_Vendida = {0};", ID);
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
            return String.Format("UPDATE FRECUENCIA_VENDIDA SET Id_Unidad_Transporte= '{1}', Id_Factura= '{2}', Id_Frecuencia= '{3}', Valor= '{4}', " +
                "ValorIva= '{5}', fecha= {6}, anulado= '{7}', Id_Anden= '{8}', Habilitado= '{9}', Saldoprepago= '{10}', recaudador= '{11}', " +
                "Id_Chofer= {12}, Tipo_Tarifa= '{13}', Numero_Tarifa= {14}, Extra= '{15}', Id_ControlUTTChofer= {16}, Operadora= '{17}', Disco= '{18}', " +
                "Placa= '{19}', Tag= '{20}', Conductor= '{21}', CiudadOrigen= '{22}', CiudadDestino= '{23}', HoraSalida= '{24}', Anden= '{25}', " +
                "FechaHoraSalida= '{26}', FechaHoraAnden= '{27}', Id_ControlBioUTT= {28}, Id_FrecActerior= {29}, Fecha_Creacion = {30}, " +
                "Usuario_Creacion = {31}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {32} " +
                "WHERE Id_Frecuencia_Vendida = {0};", Id_Frecuencia_Vendida, Id_Unidad_Transporte, Id_Factura, Id_Frecuencia, 
                Valor.ToString().Replace(",",SqlServer.SigFloatSql), ValorIva.ToString().Replace(",", SqlServer.SigFloatSql), 
                (Fecha == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                Anulado, Id_Anden, Habilitado, Saldoprepago.ToString().Replace(",", SqlServer.SigFloatSql), Recaudador, 
                Id_Chofer == 0 ? "NULL" : Id_Chofer.ToString(), Tipo_Tarifa, Numero_Tarifa == 0 ? "NULL" : Numero_Tarifa.ToString(), Extra, 
                Id_ControlUTTChofer == 0 ? "NULL" : Id_ControlUTTChofer.ToString(), NombreOperadora, NumDisco, NumPlaca, NumTag, NombreConductor, CiudadOrigen, 
                CiudadDestino, HoraSalida, NumAnden, FechaHoraSalida.ToString(SqlServer.FormatofechaHora), FechaHoraAnden.ToString(SqlServer.FormatofechaHora), 
                Id_ControlBioUTT == 0 ? "NULL" : Id_ControlBioUTT.ToString(), Id_FrecActerior == 0 ? "NULL" : Id_FrecActerior.ToString(),
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
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
        ~Frecuencia_vendida()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
