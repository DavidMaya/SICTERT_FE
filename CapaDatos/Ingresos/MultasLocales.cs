using AccesoDatos;
using CapaDatos.AreasEquipos;
using CapaDatos.Recaudacion;
using CapaDatos.UsuariosPerfiles;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class MultaLocal : SqlConnecion, IDisposable
    {
        private long _Id_Local;
        private long _Id_Concepto_Cuenta;
        private Concepto_cuenta _Concepto_Cuenta;
        private LocalComercial _Local;
        private long _Id_Usuario;
        private Usuarios _Usuario;
        private long _Id_Factura;
        private FacturaRecauda _Factura;

        #region Public Properties
        public long Id_Multa { get; set; }
        public long Id_Usuario
        {
            get { return _Id_Usuario; }
            set
            {
                _Id_Usuario = value;
                _Usuario = null;
            }
        }
        public long Id_Local
        {
            get { return _Id_Local; }
            set { 
                _Id_Local = value;
                _Local = null;
            }
        }
        public string Hechos { get; set; }
        public string Responsable { get; set; }
        public string Actividad { get; set; }
        public string Tipificacion { get; set; }
        public string Medidas { get; set; }
        public string Observaciones { get; set; }
        public DateTime? Fecha_Hora { get; set; }
        public decimal Valor { get; set; }
        public int Estado { get; set; }
        public long Id_Concepto_Cuenta
        {
            get { return _Id_Concepto_Cuenta; }
            set
            {
                _Id_Concepto_Cuenta = value;
                _Concepto_Cuenta = null;
            }
        }
        public decimal IVA { get; set; }
        public LocalComercial Local
        {
            get
            {
                if (_Local != null && _Local.Id_Local != 0)
                    return _Local;
                else if (_Id_Local != 0)
                    return _Local = new LocalComercial(_Id_Local);
                else
                    return null;
            }
        }
        public long Num_Notif { get; set; }
        public bool Obligatorio { get; set; }
        public bool Pago { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public Concepto_cuenta Concepto_Cuenta
        {
            get
            {
                if (_Concepto_Cuenta != null && _Concepto_Cuenta.Id_Concepto_Cuenta != 0)
                    return _Concepto_Cuenta;
                else if (_Id_Concepto_Cuenta != 0)
                    return _Concepto_Cuenta = new Concepto_cuenta(_Id_Concepto_Cuenta);
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
        public FacturaRecauda factura
        {
            get
            {
                if (_Factura != null && _Factura.IdFacturaRecauda != 0)
                    return _Factura;
                else if (_Id_Factura != 0)
                    return _Factura = new FacturaRecauda(_Id_Factura);
                else
                    return null;
            }
        }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public MultaLocal()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public MultaLocal(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Multa = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Multa = Convert.ToInt64(dt.Rows[0]["Id_Multa"]);
                    _Id_Local = Convert.ToInt64(dt.Rows[0]["Id_Local"]);
                    Hechos = Convert.ToString(dt.Rows[0]["Detalle"]);
                    Responsable = Convert.ToString(dt.Rows[0]["Responsable"]);
                    Actividad = Convert.ToString(dt.Rows[0]["Actividad"]);
                    Tipificacion = dt.Rows[0]["Tipificacion"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Tipificacion"]) : "";
                    Medidas = Convert.ToString(dt.Rows[0]["Medidas"]);
                    Observaciones = dt.Rows[0]["Observaciones"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Observaciones"]) : "";
                    Fecha_Hora = valorDateTime(dt.Rows[0]["Fecha_Hora"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    Estado = Convert.ToInt32(dt.Rows[0]["Estado"]);
                    _Id_Concepto_Cuenta = Convert.ToInt64(dt.Rows[0]["Id_Concepto_Cuenta"]);
                    IVA = Convert.ToDecimal(dt.Rows[0]["IVA"]);
                    _Id_Usuario = Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
                    Num_Notif = dt.Rows[0]["Num_Notif"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Num_Notif"]) : 0;
                    _Id_Factura = dt.Rows[0]["Id_factura_recauda"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_factura_recauda"]) : 0;
                    Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
                    Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
                    Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
                    Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
                }
            }
            //throw new ApplicationException("Multa does not exist.");
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
            return String.Format("SELECT Id_Multa, Id_Local, Detalle, Fecha_Hora, Valor, Estado, Id_Concepto_Cuenta, IVA, Id_Usuario, Responsable, " +
                "Actividad, Tipificacion, Medidas, Observaciones, Num_Notif, Id_factura_recauda, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, " +
                "Usuario_Modificacion FROM MULTA_LOCAL");
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

        public static MultaLocal GetMulta(long id)
        {
            return new MultaLocal(id);
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
            return String.Format("INSERT INTO MULTA_LOCAL (Id_Local, Detalle, Fecha_Hora, Valor, Estado, Id_Concepto_Cuenta, IVA, Id_Usuario, " +
                "Responsable, Actividad, Tipificacion, Medidas, Observaciones, Num_Notif, Id_factura_recauda, Fecha_Creacion, Usuario_Creacion, " +
                "Fecha_Modificacion, Usuario_Modificacion) VALUES('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', " +
                "'{11}', '{12}', '{13}', {14}, GETDATE(), {15}, {16}, {17}); ",  
                Id_Local, SqlServer.ValidarTexto(Hechos), 
                (Fecha_Hora == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                Valor.ToString().Replace(",", SqlServer.SigFloatSql), Estado, Id_Concepto_Cuenta, IVA.ToString().Replace(",", SqlServer.SigFloatSql), 
                Id_Usuario, SqlServer.ValidarTexto(Responsable), SqlServer.ValidarTexto(Actividad), SqlServer.ValidarTexto(Tipificacion), 
                SqlServer.ValidarTexto(Medidas), SqlServer.ValidarTexto(Observaciones), Num_Notif,
                _Id_Factura == 0 ? "NULL" : _Id_Factura.ToString(), Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM MULTA_LOCAL WHERE Id_Multa = {0};", ID);
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
            return String.Format("UPDATE MULTA_LOCAL SET Id_Local= '{1}', Detalle= '{2}', Fecha_Hora= {3}, Valor= '{4}', Estado= '{5}', " +
                "Id_Concepto_Cuenta= '{6}', IVA= '{7}', Id_Usuario= '{8}', Responsable= '{9}', Actividad= '{10}', Tipificacion = '{11}', " +
                "Medidas= '{12}', Observaciones= '{13}', Num_Notif= '{14}', Id_factura_recauda= {15}, Fecha_Creacion = {16}, Usuario_Creacion = {17}, " +
                "Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {18} WHERE Id_Multa = {0};", 
                Id_Multa, Id_Local, SqlServer.ValidarTexto(Hechos), 
                (Fecha_Hora == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                Valor.ToString().Replace(",", SqlServer.SigFloatSql), Estado, Id_Concepto_Cuenta, 
                IVA.ToString().Replace(",", SqlServer.SigFloatSql), Id_Usuario, SqlServer.ValidarTexto(Responsable),
                SqlServer.ValidarTexto(Actividad), SqlServer.ValidarTexto(Tipificacion), SqlServer.ValidarTexto(Medidas), 
                SqlServer.ValidarTexto(Observaciones), Num_Notif, _Id_Factura == 0 ? "NULL" : _Id_Factura.ToString(),
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        /// <summary>
        /// get el sql para actualizar la tabla multas
        /// </summary>
        /// <param name="Id_Local"></param>
        /// <param name="idFactura"></param>
        /// <returns></returns>
        public string GetSQLUpdate_Estado(string id_multas, long idFactura)
        {
            return id_multas.Length > 0 ? String.Format("UPDATE MULTA_LOCAL SET Estado = 6, Id_Factura = {1} WHERE Id_Multa IN ({0});", id_multas, idFactura) : "";
        }

        /// <summary>
        /// Obtiene el listado de las multas del vehiculo
        /// Estados:
        /// 1 Nueva
        /// 2 En Proceso
        /// 3 Cobro Opcional
        /// 4 Cobro Obligatorio
        /// 5 Anulada
        /// 6 Pagada
        /// </summary>
        /// <param name="id_bus"></param>
        /// <returns></returns>
        public DataTable get_Deudas(long id_local)
        {
            string sql = String.Format("SELECT 1 AS Cancelar, M.Id_Multa, m.Id_Local, cc.Nombre Motivo, m.Detalle, dbo.fn_formatearFecha(m.Fecha_Hora) AS Fecha_Hora, m.valor AS Subtotal, m.IVA, m.IVA + m.Valor AS TOTAL, m.Estado, " +
                "CASE WHEN m.estado = 4 THEN 1 ELSE 0 END Obligado FROM MULTA_LOCAL m INNER JOIN CONCEPTO_CUENTA cc ON cc.Id_Concepto_Cuenta = m.Id_Concepto_Cuenta " +
                "WHERE m.Id_Local = {0} AND m.ESTADO IN (SELECT Id_Estado FROM ESTADO_MULTA WHERE Estado LIKE 'Cobro%')", id_local);
            return SqlServer.EXEC_SELECT(sql);
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
        ~MultaLocal()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
