using AccesoDatos;
using CapaDatos.AreasEquipos;
using System;
using System.Data;

namespace CapaDatos.Transportistas
{
    public partial class Dispositivo_rfid : SqlConnecion, IDisposable
    {
        private long _Id_Unidad_Transporte;
        private Unidad_transporte _Unidad_Transporte;

        #region Public Properties
        public long Id_Dispostivo_RFID { get; set; }
        public string Tag { get; set; }
        public bool Estado { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public long Id_Unidad_Transporte
        {
            get { return _Id_Unidad_Transporte; }
            set
            {
                _Id_Unidad_Transporte = value;
                _Unidad_Transporte = null;
            }
        }
        public bool Habilita_Entrada { get; set; }
        public bool Habilita_Venta { get; set; }
        public bool Habilita_Salida { get; set; }
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
        public bool Habilita_Llegada { get; set; }
        public DateTime Valido_Desde { get; set; }
        public DateTime Valido_Hasta { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Dispositivo_rfid()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Dispositivo_rfid(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Dispostivo_RFID = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Dispostivo_RFID = Convert.ToInt64(dt.Rows[0]["Id_Dispostivo_RFID"]);
                    Tag = Convert.ToString(dt.Rows[0]["Tag"]);
                    Estado = Convert.ToBoolean(dt.Rows[0]["Estado"]);
                    Descripcion = Convert.ToString(dt.Rows[0]["Descripcion"]);
                    Observacion = Convert.ToString(dt.Rows[0]["Observacion"]);
                    _Id_Unidad_Transporte = Convert.ToInt64(dt.Rows[0]["Id_Unidad_Transporte"]);
                    Habilita_Entrada = Convert.ToBoolean(dt.Rows[0]["Habilita_Entrada"].ToString());
                    Habilita_Venta = Convert.ToBoolean(dt.Rows[0]["Habilita_Venta"]);
                    Habilita_Salida = Convert.ToBoolean(dt.Rows[0]["Habilita_Salida"]);
                    Habilita_Llegada = Convert.ToBoolean(dt.Rows[0]["Habilita_Llegada"]);
                    Valido_Desde = Convert.ToDateTime(dt.Rows[0]["Valido_Desde"]);
                    Valido_Hasta = Convert.ToDateTime(dt.Rows[0]["Valido_Hasta"]);
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
            return String.Format("SELECT Id_Dispostivo_RFID, Tag, Estado, Descripcion, Observacion, Id_Unidad_Transporte, Habilita_Entrada, Habilita_Venta, " +
                "Habilita_Salida, Habilita_Llegada, Valido_Desde, Valido_Hasta, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion " +
                "FROM DISPOSITIVO_RFID");
        }

        public static DataTable GetAllData(string Where)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + String.Format((Where.Length > 0) ? (" WHERE " + Where) : ""));
        }

        public static DataTable GetAllData(string Where, string Order)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + String.Format((Where.Length > 0) ? (" WHERE " + Where) : "") + 
                String.Format((Order.Length > 0) ? (" ORDER BY " + Order) : ""));
        }

        public static DataTable GetAllData(string Where, string Join, string Order)
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect() + " " + Join + " " + String.Format((Where.Length > 0) ? (" WHERE " + Where) : "") + 
                String.Format((Order.Length > 0) ? (" ORDER BY " + Order) : ""));
        }

        public static DataTable GetAllData()
        {
            return GetAllData("");
        }

        public static Dispositivo_rfid GetDispositivo_rfid(long id)
        {
            return new Dispositivo_rfid(id);
        }

        public static Dispositivo_rfid GetDispositivo_rfid_unidad(long id_unidad)
        {
            long rfid = 0;
            if (long.TryParse(SqlServer.EXEC_SCALAR("SELECT ISNULL((SELECT Id_Dispostivo_RFID FROM DISPOSITIVO_RFID " +
                "WHERE Id_Unidad_Transporte = " + id_unidad + "), 0)").ToString(), out rfid))
                return rfid != 0 ? new Dispositivo_rfid(rfid) : null;
            else
                return null;
        }

        public static Dispositivo_rfid GetDispositivo_rfid_tag(string tag_unidad)
        {
            long rfid = 0;
            if (long.TryParse(SqlServer.EXEC_SCALAR("SELECT ISNULL((SELECT Id_Dispostivo_RFID FROM DISPOSITIVO_RFID WHERE Tag = '" + 
                tag_unidad + "'), 0)").ToString(), out rfid))
                return rfid != 0 ? new Dispositivo_rfid(rfid) : null;
            else
                return null;
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Dispostivo_RFID", "DISPOSITIVO_RFID", "");
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
            return String.Format("INSERT INTO DISPOSITIVO_RFID (Tag, Estado, Descripcion, Observacion, Id_Unidad_Transporte, Habilita_Entrada, Habilita_Venta, " +
                "Habilita_Salida, Habilita_Llegada, Valido_Desde, Valido_Hasta, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', GETDATE(), {11}, {12}, {13}); ", 
                SqlServer.ValidarTexto(Tag), Estado, SqlServer.ValidarTexto(Descripcion), SqlServer.ValidarTexto(Observacion), Id_Unidad_Transporte, 
                Habilita_Entrada, Habilita_Venta, Habilita_Salida, Habilita_Llegada, Convert.ToDateTime(Valido_Desde.ToString()).ToString(SqlServer.FormatofechaInicio), 
                Convert.ToDateTime(Valido_Hasta.ToString()).ToString(SqlServer.FormatofechaFin), Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM DISPOSITIVO_RFID WHERE Id_Dispostivo_RFID = {0};", ID);
        }

        public string Update()
        {
            string _return = "", sql = GetSQLUpdate();

            // activar la unidad asociada
            if (Id_Unidad_Transporte != 0 && (Habilita_Entrada || Habilita_Llegada || Habilita_Salida) && Unidad_Transporte != null
                && Unidad_Transporte.Id_Unidad_Transporte != 0 && !Unidad_Transporte.Activo)
            {
                Unidad_Transporte.Activo = true;
                sql += Unidad_Transporte.GetSQLUpdate();
                _return = SqlServer.EXEC_TRANSACTION(sql);
            }
            else
            {
                _return = SqlServer.EXEC_COMMAND(sql);
                if (_return == "OK")
                    _return = SqlServer.MensajeDeActualizar;
            }
            return _return;
        }

        public string UpdateOpto()
        {
            try
            {
                // _ - caracter C (habilitar) o A (inhabilitar)
                // _ - espacio
                // ##### - tag
                // _ - espacio
                // dd/MM/yyyy - fecha de caducidad
                // _ - espacio
                // ###### - saldo * 100 (entero)

                // Preembarque: 
                string _return = string.Format("DELETE FROM {2} WHERE Instruccion LIKE '%{0}%'; INSERT INTO {2} (Instruccion) VALUES ('{1} {0} {3} {4}'); ",
                    Tag.PadLeft(5, '0'), Habilita_Entrada ? "C" : "A", SqlServer.EXEC_SCALAR("SELECT TOP 1 TablaOpto FROM EQUIPO WHERE Codigo LIKE 'BV0A%'").ToString(),
                    Valido_Hasta.ToString("dd/MM/yyyy"), Convert.ToInt32(ObtenerSaldoPrepago() * 100).ToString().PadLeft(6, '0'));

                //TblA tblA = new TblA();
                //tblA.Tag = Tag;
                //_return += tblA.Insert(Habilita_Entrada); // Preembarque

                // Salidas: 
                _return += string.Format("DELETE FROM {2} WHERE Instruccion LIKE '%{0}%'; INSERT INTO {2} (Instruccion) VALUES ('{1} {0} {3} {4}'); ",
                    Tag.PadLeft(5, '0'), Habilita_Salida ? "C" : "A", SqlServer.EXEC_SCALAR("SELECT TOP 1 TablaOpto FROM EQUIPO WHERE Codigo LIKE 'BV0C%'").ToString(),
                    Valido_Hasta.ToString("dd/MM/yyyy"), Convert.ToInt32(ObtenerSaldoPrepago() * 100).ToString().PadLeft(6, '0'));

                //TblC tblC = new TblC();
                //tblC.Tag = Tag;
                //_return += tblC.Insert(Habilita_Salida); // Salida de andenes

                // Llegadas: 
                _return += string.Format("DELETE FROM {2} WHERE Instruccion LIKE '%{0}%'; INSERT INTO {2} (Instruccion) VALUES ('{1} {0} {3} {4}'); ",
                    Tag.PadLeft(5, '0'), Habilita_Llegada ? "C" : "A", SqlServer.EXEC_SCALAR("SELECT TOP 1 TablaOpto FROM EQUIPO WHERE Codigo LIKE 'BU0D%'").ToString(),
                    Valido_Hasta.ToString("dd/MM/yyyy"), Convert.ToInt32(ObtenerSaldoPrepago() * 100).ToString().PadLeft(6, '0'));

                //TblD tblD = new TblD();
                //tblD.Tag = Tag;
                //_return += tblD.Insert(Habilita_Llegada); // Llegadas

                // Llegadas: 
                _return += string.Format("DELETE FROM {2} WHERE Instruccion LIKE '%{0}%'; INSERT INTO {2} (Instruccion) VALUES ('{1} {0} {3} {4}'); ",
                    Tag.PadLeft(5, '0'), Habilita_Llegada ? "C" : "A", SqlServer.EXEC_SCALAR("SELECT TOP 1 TablaOpto FROM EQUIPO WHERE Codigo LIKE 'BV0E%'").ToString(),
                    Valido_Hasta.ToString("dd/MM/yyyy"), Convert.ToInt32(ObtenerSaldoPrepago() * 100).ToString().PadLeft(6, '0'));

                //TblE tblE = new TblE();
                //tblD.Tag = Tag;
                //_return += tblE.Insert(Habilita_Llegada); // Llegadas

                // Anden
                //TblB tblB = new TblB();
                //tblB.Tag = Tag;
                //_return += tblB.Insert(Habilita_Venta); // Andenes

                // Andén desde bodega
                //TblH tblH = new TblH();
                //tblH.Tag = Tag;
                //_return += tblH.Insert(Habilita_Venta); // Andenes desde Bodega

                string resultado_ = SqlServer.EXEC_COMMAND(_return);
                if (resultado_ == "OK")
                    resultado_ = SqlServer.MensajeDeGuardar;

                return resultado_;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string InhabilitarTagAntiguo(string TAG)
        {
            try
            {
                // Preembarque: 
                string _return = string.Format("DELETE FROM {1} WHERE Instruccion LIKE '%{0}%'; INSERT INTO {1} (Instruccion) VALUES ('A {0} 01/01/2001 000000'); ",
                    TAG.PadLeft(5, '0'), SqlServer.EXEC_SCALAR("SELECT TOP 1 TablaOpto FROM EQUIPO WHERE Codigo LIKE 'BV0A%'").ToString());

                // Salidas: 
                _return += string.Format("DELETE FROM {1} WHERE Instruccion LIKE '%{0}%'; INSERT INTO {1} (Instruccion) VALUES ('A {0} 01/01/2001 000000'); ",
                    TAG.PadLeft(5, '0'), SqlServer.EXEC_SCALAR("SELECT TOP 1 TablaOpto FROM EQUIPO WHERE Codigo LIKE 'BV0C%'").ToString());

                // Llegadas: 
                _return += string.Format("DELETE FROM {1} WHERE Instruccion LIKE '%{0}%'; INSERT INTO {1} (Instruccion) VALUES ('A {0} 01/01/2001 000000'); ",
                    TAG.PadLeft(5, '0'), SqlServer.EXEC_SCALAR("SELECT TOP 1 TablaOpto FROM EQUIPO WHERE Codigo LIKE 'BU0D%'").ToString());

                // Llegadas: 
                _return += string.Format("DELETE FROM {1} WHERE Instruccion LIKE '%{0}%'; INSERT INTO {1} (Instruccion) VALUES ('A {0} 01/01/2001 000000'); ",
                    TAG.PadLeft(5, '0'), SqlServer.EXEC_SCALAR("SELECT TOP 1 TablaOpto FROM EQUIPO WHERE Codigo LIKE 'BV0E%'").ToString());

                return SqlServer.EXEC_TRANSACTION(_return);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public decimal ObtenerSaldoPrepago(string TAG = "")
        {
            try
            {
                return Convert.ToDecimal(SqlServer.EXEC_SCALAR("SELECT CAST(ISNULL(cp.Saldo, 0) AS DECIMAL(14, 2)) FROM DISPOSITIVO_RFID drf " +
                    "INNER JOIN UNIDAD_TRANSPORTE ut ON ut.Id_Unidad_Transporte = drf.Id_Unidad_Transporte " +
                    "LEFT OUTER JOIN CUENTA_PREPAGO_UTT cp ON cp.Id_Unidad_Transporte = ut.Id_Unidad_Transporte WHERE drf.Tag = '" +
                    (string.IsNullOrEmpty(TAG) ? Tag : TAG) + "'"));
            }
            catch
            {
                return 0;
            }
        }

        public string GetSQLUpdate()
        {
            return String.Format("UPDATE DISPOSITIVO_RFID SET Tag= '{1}', Estado= '{2}', Descripcion= '{3}', Observacion= '{4}', Id_Unidad_Transporte= '{5}', " +
                "Habilita_Entrada= '{6}', Habilita_Venta= '{7}', Habilita_Salida= '{8}', Habilita_Llegada= '{9}', Valido_Desde= '{10}', Valido_Hasta= '{11}', " +
                "Fecha_Creacion = {12}, Usuario_Creacion = {13}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {14} " +
                "WHERE Id_Dispostivo_RFID = {0};", Id_Dispostivo_RFID, SqlServer.ValidarTexto(Tag), Estado, SqlServer.ValidarTexto(Descripcion), 
                SqlServer.ValidarTexto(Observacion), Id_Unidad_Transporte, Habilita_Entrada, Habilita_Venta, Habilita_Salida, Habilita_Llegada, 
                Convert.ToDateTime(Valido_Desde.ToString()).ToString(SqlServer.FormatofechaInicio),
                Convert.ToDateTime(Valido_Hasta.ToString()).ToString(SqlServer.FormatofechaFin),
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Tag(string sTag)
        {
            return IsDuplicate_Tag(0, sTag);
        }

        public bool IsDuplicate_Tag(long id, string sTag)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Dispostivo_RFID) FROM DISPOSITIVO_RFID WHERE Tag = '{0}' {1}", 
                sTag, id != 0 ? "AND Id_Dispostivo_RFID <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_UTT(long UTT)
        {
            return IsDuplicate_UTT(0, UTT);
        }

        public bool IsDuplicate_UTT(long id, long UTT)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Dispostivo_RFID) FROM DISPOSITIVO_RFID " +
                "WHERE Id_Unidad_Transporte = '{0}' {1}", UTT, id != 0 ? "AND Id_Dispostivo_RFID <> " + id.ToString() : ""))) > 0;
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
        ~Dispositivo_rfid()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
