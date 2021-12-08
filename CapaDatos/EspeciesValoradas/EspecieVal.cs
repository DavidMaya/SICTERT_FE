using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.EspeciesValoradas
{
    public partial class Especie_Caja : SqlConnecion, IDisposable
    {
        private long _Id_Tipo_Especie;
        private long _Id_Acta_Asigna;
        private long _Id_Acta_Devuelve;
        private long _Id_Caja;
        private long _Id_Tipo_Pago;
        private long _Id_Cierre;
        private Tipo_especie_val _Tipo_Especie;
        private Acta_EspecieVal _Acta_Asigna;
        private Acta_EspecieVal _Acta_Devuelve;
        private Caja _Caja;
        private Tipo_pago _Tipo_Pago;
        private Cierre_caja _Cierre_Caja;

        #region Public Properties
        public long Id_Especie_Reg { get; set; }
        public long Id_Tipo_Especie
        {
            get { return _Id_Tipo_Especie; }
            set { 
                _Id_Tipo_Especie = value;
                _Tipo_Especie = null;
            }
        }
        public long Numero { get; set; }
        public decimal Valor { get; set; }
        public long Id_Acta_Asigna
        {
            get { return _Id_Acta_Asigna; }
            set { 
                _Id_Acta_Asigna = value;
                _Acta_Asigna = null;
            }
        }
        public long Id_Acta_Devuelve
        {
            get { return _Id_Acta_Devuelve; }
            set { 
                _Id_Acta_Devuelve = value; 
                _Acta_Devuelve = null; 
            }
        }
        public long Id_Caja
        {
            get { return _Id_Caja; }
            set { 
                _Id_Caja = value; 
                _Caja = null; 
            }
        }
        public long Id_Tipo_Pago
        {
            get { return _Id_Tipo_Pago; }
            set { 
                _Id_Tipo_Pago = value;
                _Tipo_Pago = null;
            }
        }
        public short Estado { get; set; } // estados: 1: asignado (en caja) 2: devuelto (en admin) 3: emitido (en cliente)
        public DateTime? Fecha_Emision { get; set; }
        public long Id_Cierre
        {
            get { return _Id_Cierre; }
            set { 
                _Id_Cierre = value;
                _Cierre_Caja = null;
            }
        }
        public string Operadora { get; set; }
        public string Disco { get; set; }
        public string Placa { get; set; }
        public string Destino { get; set; }
        public string Cliente { get; set; }
        public DateTime? Fecha_Hora { get; set; }
        public string Observacion { get; set; }
        public Tipo_especie_val Tipo_Especie
        {
            get
            {
                if (_Tipo_Especie != null && _Tipo_Especie.Id_Tipo_Especie != 0)
                    return _Tipo_Especie;
                else if (_Id_Tipo_Especie != 0)
                    return _Tipo_Especie = new Tipo_especie_val(_Id_Tipo_Especie);
                else
                    return null;
            }
        }
        public Acta_EspecieVal Acta_Asigna
        {
            get
            {
                if (_Acta_Asigna != null && _Acta_Asigna.Id_Acta_EspVal != 0)
                    return _Acta_Asigna;
                else if (_Id_Acta_Asigna != 0)
                    return _Acta_Asigna = new Acta_EspecieVal(_Id_Acta_Asigna);
                else
                    return null;
            }
        }
        public Acta_EspecieVal Acta_Devuelve
        {
            get
            {
                if (_Acta_Devuelve != null && _Acta_Devuelve.Id_Acta_EspVal != 0)
                    return _Acta_Devuelve;
                else if (_Id_Acta_Devuelve != 0)
                    return _Acta_Devuelve = new Acta_EspecieVal(_Id_Tipo_Especie);
                else
                    return null;
            }
        }
        public Caja caja
        {
            get
            {
                if (_Caja != null && _Caja.Id_Caja != 0)
                    return _Caja;
                else if (_Id_Caja != 0)
                    return _Caja = new Caja(_Id_Caja);
                else
                    return null;
            }
        }
        public Tipo_pago Tipo_Pago
        {
            get
            {
                if (_Tipo_Pago != null && _Tipo_Pago.Id_Tipo_Pago != 0)
                    return _Tipo_Pago;
                else if (_Id_Tipo_Pago != 0)
                    return _Tipo_Pago = new Tipo_pago(_Id_Tipo_Pago);
                else
                    return null;
            }
        }
        #endregion

        public Especie_Caja()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Especie_Caja(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Especie_Reg = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Especie_Reg = Convert.ToInt64(dt.Rows[0]["Id_Especie_Reg"]);
                    _Id_Tipo_Especie = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Especie"]);
                    Numero = Convert.ToInt64(dt.Rows[0]["Numero"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    _Id_Acta_Asigna = Convert.ToInt64(dt.Rows[0]["Id_Acta_Asigna"]);
                    _Id_Acta_Devuelve = dt.Rows[0]["Id_Acta_Devuelve"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Acta_Devuelve"]) : 0;
                    _Id_Caja = Convert.ToInt64(dt.Rows[0]["Id_Caja"]);
                    Estado = Convert.ToInt16(dt.Rows[0]["Estado"]);
                    Fecha_Emision = dt.Rows[0]["Fecha_Emision"] != DBNull.Value ? valorDateTime(Convert.ToDateTime(dt.Rows[0]["Fecha_Emision"])) : null;
                    _Id_Cierre = dt.Rows[0]["Id_Cierre_Emision"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Cierre_Emision"]) : 0;
                    _Id_Tipo_Pago = dt.Rows[0]["Id_Tipo_Pago"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Tipo_Pago"]) : 0;
                    Operadora = dt.Rows[0]["Operadora"] != DBNull.Value ? dt.Rows[0]["Operadora"].ToString() : "";
                    Disco = dt.Rows[0]["Disco"] != DBNull.Value ? dt.Rows[0]["Disco"].ToString() : "";
                    Placa = dt.Rows[0]["Placa"] != DBNull.Value ? dt.Rows[0]["Placa"].ToString() : "";
                    Destino = dt.Rows[0]["Destino"] != DBNull.Value ? dt.Rows[0]["Destino"].ToString() : "";
                    Cliente = dt.Rows[0]["Cliente"] != DBNull.Value ? dt.Rows[0]["Cliente"].ToString() : "";
                    Fecha_Hora = dt.Rows[0]["Fecha_Hora"] != DBNull.Value ? valorDateTime(Convert.ToDateTime(dt.Rows[0]["Fecha_Hora"])) : null;
                    Observacion = dt.Rows[0]["Observacion"] != DBNull.Value ? dt.Rows[0]["Observacion"].ToString() : "";
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
            return String.Format("SELECT Id_Especie_Reg, Id_Tipo_Especie, Numero, Valor, Id_Acta_Asigna, Id_Acta_Devuelve, Id_Caja, Estado, Fecha_Emision, Id_Cierre_Emision, Operadora, Disco, Placa, Destino, Cliente, Fecha_hora, Observacion, Id_Tipo_Pago FROM ESPECIES_CAJA");
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

        public static Especie_Caja GetCaja(long id)
        {
            return new Especie_Caja(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Especie_Reg", "ESPECIES_CAJA", "");
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
            return String.Format("INSERT INTO ESPECIES_CAJA (Id_Tipo_Especie, Numero, Valor, Id_Acta_Asigna, Id_Acta_Devuelve, Id_Caja, Estado, Fecha_Emision, Id_Cierre_Emision, Operadora, Disco, " +
                "Placa, Destino, Cliente, Fecha_hora, Observacion, Id_Tipo_Pago) VALUES( '{0}','{1}','{2}','{3}',{4},'{5}','{6}',{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}); ",
                Id_Tipo_Especie, Numero, Valor.ToString().Replace(",", SqlServer.SigFloatSql), Id_Acta_Asigna, Id_Acta_Devuelve == 0 ? "NULL" : Id_Acta_Devuelve.ToString(), Id_Caja, Estado, 
                (Fecha_Emision == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Emision.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Id_Cierre == 0 ? "NULL" : Id_Cierre.ToString(),
                Operadora.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Operadora) + "'", Disco.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Disco) + "'", 
                Placa.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Placa) + "'", Destino.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Destino) + "'", 
                Cliente.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Cliente) + "'", (Fecha_Hora == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                Observacion.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Observacion) + "'", Id_Tipo_Pago == 0 ? "NULL" : Id_Tipo_Pago.ToString());
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
            return String.Format("DELETE FROM ESPECIES_CAJA WHERE Id_Especie_Reg = {0};", ID);
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
            return String.Format("UPDATE ESPECIES_CAJA SET Id_Tipo_Especie= '{1}',Numero= '{2}',Valor= '{3}',Id_Acta_Asigna= '{4}',Id_Acta_Devuelve= {5},Id_Caja= '{6}'," +
                " Estado= '{7}',Fecha_Emision= {8},Id_Cierre_Emision= {9},Operadora= {10},Disco= {11},Placa= {12},Destino= {13},Cliente= {14},Fecha_Hora= {15},Observacion= {16},Id_Tipo_Pago= {17}" +
                " WHERE Id_Especie_Reg = {0};", Id_Especie_Reg, Id_Tipo_Especie, Numero, Valor.ToString().Replace(",", SqlServer.SigFloatSql), Id_Acta_Asigna, Id_Acta_Devuelve == 0 ? "NULL" : Id_Acta_Devuelve.ToString(), Id_Caja, Estado, 
                (Fecha_Emision == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Emision.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Id_Cierre == 0 ? "NULL" : Id_Cierre.ToString(),
                Operadora.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Operadora) + "'", Disco.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Disco) + "'", Placa.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Placa) + "'", 
                Destino.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Destino) + "'", Cliente.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Cliente) + "'", 
                (Fecha_Hora == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Observacion.Length == 0 ? "NULL" : "'" + SqlServer.ValidarTexto(Observacion) + "'", 
                Id_Tipo_Pago == 0 ? "NULL" : Id_Tipo_Pago.ToString());
        }

        public string AgregarEmitida(long IDTPago)
        {
            // Generar registro en INGRESO_TURNO
            // El iva va en cero (cuarto parámetro)
            string ingreso = String.Format("EXEC InsertarIngresoTurno {0}, 'ESPV', {2}, 1, 0, {1};", 
                _Id_Cierre, Valor.ToString().Replace(",", SqlServer.SigFloatSql), IDTPago);
            string _return = SqlServer.EXEC_TRANSACTION(ingreso + GetSQLUpdate() + string.Format(" UPDATE CIERRE_CAJA SET Total_Sistema = Total_Sistema + {0}, " +
                "Total_Contado = CASE WHEN (SELECT Deposito FROM TIPO_PAGO WHERE Id_Tipo_Pago = {1}) = 1 THEN Total_Contado + {0} ELSE Total_Contado END " +
                "WHERE Id_Cierre_Caja = " + _Id_Cierre.ToString() + "; ", Valor.ToString().Replace(",", SqlServer.SigFloatSql), IDTPago));
            if (_return.Contains("correctamente"))
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }

        public string EliminarEmitida(long IDTPago)
        {
            long _id_cierre_caja = _Id_Cierre;
            Fecha_Emision = null;
            _Id_Cierre = 0;
            _Id_Tipo_Pago = 0;
            Operadora = "";
            Disco = "";
            Placa = "";
            Destino = "";
            Cliente = "";
            Fecha_Hora = null;
            Estado = 1;
            Observacion = "";

            // Generar registro en INGRESO_TURNO
            // Se insertan un valores negativos por ser una eliminación (tercer y cuarto parámetro)
            // El iva va en cero (quinto parámetro)
            string ingreso = String.Format("EXEC InsertarIngresoTurno {0}, 'ESPV', {2}, -1, 0, -{1};", 
                _id_cierre_caja, Valor.ToString().Replace(",", SqlServer.SigFloatSql), IDTPago);

            string _return = SqlServer.EXEC_TRANSACTION(ingreso + GetSQLUpdate() + string.Format(" UPDATE CIERRE_CAJA SET Total_Sistema = Total_Sistema - {0}, " +
                "Total_Contado = CASE WHEN (SELECT Deposito FROM TIPO_PAGO WHERE Id_Tipo_Pago = {1}) = 1 THEN Total_Contado - {0} ELSE Total_Contado END " +
                " WHERE Id_Cierre_Caja = " + _id_cierre_caja.ToString() + "; ", Valor.ToString().Replace(",", SqlServer.SigFloatSql), IDTPago));
            if (_return.Contains("correctamente"))
                _return = SqlServer.MensajeDeEliminar;
            return _return;
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
        ~Especie_Caja()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
