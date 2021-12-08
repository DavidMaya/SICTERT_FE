using AccesoDatos;
using CapaDatos.UsuariosPerfiles;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public class Cierre_caja : SqlConnecion, IDisposable
    {
        private long _Id_Caja;
        private long _Id_Usuario;
        private Caja _Caja;
        private Usuarios _Usuario;

        #region Public Properties
        public long Id_Cierre_Caja { get; set; }
        public long Id_Caja
        {
            get { return _Id_Caja; }
            set
            {
                _Id_Caja = value;
                _Caja = null;
            }
        }
        public Nullable<DateTime> Fecha_Hora_Apertura { get; set; }
        public Nullable<DateTime> Fecha_Hora_Cierre { get; set; }
        public string Detalle { get; set; }
        public decimal Total { get; set; }
        public long Id_Usuario
        {
            get { return _Id_Usuario; }
            set
            {
                _Id_Usuario = value;
                _Usuario = null;
            }
        }
        public Nullable<DateTime> Fecha_Cuadre { get; set; }
        public string Estado { get; set; }
        public int Turno { get; set; }
        public bool Caja_frecuencia { get; set; }
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
        public Usuarios usuario
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
        public decimal Total_Sistema { get; set; }
        public decimal Total_Contado { get; set; }
        #endregion

        public Cierre_caja()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Cierre_caja(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Cierre_Caja = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Cierre_Caja = Convert.ToInt64(dt.Rows[0]["Id_Cierre_Caja"]);
                    _Id_Caja = Convert.ToInt64(dt.Rows[0]["Id_Caja"]);
                    Fecha_Hora_Apertura = valorDateTime(dt.Rows[0]["Fecha_Hora_Apertura"]);
                    Fecha_Hora_Cierre = valorDateTime(dt.Rows[0]["Fecha_Hora_Cierre"]);
                    Detalle = Convert.ToString(dt.Rows[0]["Detalle"]);
                    Total = Convert.ToDecimal(dt.Rows[0]["Total"]);
                    _Id_Usuario = Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
                    Estado = Convert.ToString(dt.Rows[0]["Estado"]);
                    Fecha_Cuadre = valorDateTime(dt.Rows[0]["Fecha_Cuadre"]);
                    Turno = Convert.ToInt32(dt.Rows[0]["Turno"]);
                    Caja_frecuencia = Convert.ToBoolean(dt.Rows[0]["Caja_frecuencia"]);
                    Total_Sistema = Convert.ToDecimal(dt.Rows[0]["Total_Sistema"]);
                    Total_Contado = Convert.ToDecimal(dt.Rows[0]["Total_Contado"]);
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
            return String.Format("SELECT CC.Id_Cierre_Caja, CC.Id_Caja, CC.Fecha_Hora_Apertura, CC.Fecha_Hora_Cierre, CC.Detalle, CC.Total, CC.Id_Usuario, CC.Estado, CC.Fecha_Cuadre, CC.Turno, CC.Caja_frecuencia, CC.Total_Sistema, CC.Total_Contado FROM Cierre_caja CC");
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

        public DataTable GetFullData()
        {
            return SqlServer.EXEC_SELECT(GetSqlSelect());
        }

        public static Cierre_caja GetCierre_caja(long id)
        {
            return new Cierre_caja(id);
        }

        static public long Next_Codigo()
        {
            //string sql = SqlServer.GetFormatoSQLNEXT("Id_Cierre_Caja", "Cierre_caja", "");
            //return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
            string sql = "EXEC ObtenerProximoID 'Id_Cierre_Caja'";
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
            return String.Format("INSERT INTO Cierre_caja (Id_Cierre_Caja, Id_Caja, Fecha_Hora_Apertura, Fecha_Hora_Cierre, Detalle, Total, Id_Usuario, Estado, Fecha_Cuadre, Turno, Caja_frecuencia, Total_Sistema, Total_Contado) " +
                " VALUES( '{0}','{1}',{2},{3},'{4}','{5}','{6}','{7}',{8},'{9}','{10}','{11}','{12}'); ", Id_Cierre_Caja, Id_Caja, (Fecha_Hora_Apertura == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora_Apertura.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                (Fecha_Hora_Cierre == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora_Cierre.ToString()).ToString(SqlServer.FormatofechaHora) + "'", SqlServer.ValidarTexto(Detalle), Total.ToString().Replace(",", SqlServer.SigFloatSql), Id_Usuario, SqlServer.ValidarTexto(Estado), 
                (Fecha_Cuadre == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Cuadre.ToString()).ToString(SqlServer.FormatofechaHora) + "'", Turno, Caja_frecuencia, Total_Sistema.ToString().Replace(",", SqlServer.SigFloatSql), Total_Contado.ToString().Replace(",", SqlServer.SigFloatSql));
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
            return String.Format("DELETE FROM Cierre_caja WHERE Id_Cierre_Caja = {0};", ID);

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
            return String.Format("UPDATE Cierre_caja SET Id_Caja = '{1}', Fecha_Hora_Apertura = {2}, Fecha_Hora_Cierre = {3}," +
                " Detalle = '{4}', Total = '{5}', Id_Usuario = '{6}', Estado = '{7}', Fecha_Cuadre = {8}, Turno = '{9}'," +
                " Caja_frecuencia = '{10}', Total_Sistema = '{11}', Total_Contado = '{12}'" +
                " WHERE Id_Cierre_Caja = {0};", Id_Cierre_Caja, Id_Caja, 
                (Fecha_Hora_Apertura == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora_Apertura.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                (Fecha_Hora_Cierre == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Hora_Cierre.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                SqlServer.ValidarTexto(Detalle), Total.ToString().Replace(",", SqlServer.SigFloatSql), Id_Usuario, SqlServer.ValidarTexto(Estado), 
                (Fecha_Cuadre == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Cuadre.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                Turno, Caja_frecuencia, Total_Sistema.ToString().Replace(",", SqlServer.SigFloatSql), Total_Contado.ToString().Replace(",", SqlServer.SigFloatSql));
        }

        public decimal ObtenerValorCierreRecaudador(long ID)
        {
            try
            {
                return Convert.ToDecimal(SqlServer.EXEC_SCALAR("SELECT CAST(ISNULL(SUM(Cantidad * Valor), 0) AS DECIMAL(19,4) " +
                    "FROM DINERO_CIERRE WHERE Id_Cierre = " + ID.ToString()));
            }
            catch
            {
                return 0;
            }
        }

        public decimal ObtenerValorCierreRecaudador(long ID, int IDTPago)
        {
            try
            {
                return Convert.ToDecimal(SqlServer.EXEC_SCALAR(string.Format("SELECT CAST(SUM(ISNULL(dc.Valor * dc.Cantidad, 0)) AS DECIMAL(19,2)) FROM DINERO_CIERRE dc " +
                    "INNER JOIN DINERO d ON d.Id_Dinero = dc.Id_Dinero WHERE dc.Id_Cierre = {0} AND d.Id_Tipo_Pago = {1}", ID, IDTPago)));
            }
            catch
            {
                return 0;
            }
        }

        public decimal ObtenerValorPagosCierre(long ID)
        {
            try
            {
                switch (caja.tipo_caja.Codigo)
                {
                    case "FR":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(p.Valor, 0)) AS DECIMAL(19,2)) " +
                            "FROM PAGO p LEFT JOIN FACTURA f ON f.Id_factura = p.Id_Factura " +
                            "WHERE f.Id_Cierre = {0} AND f.Estado = 1", ID)));
                    case "TU":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(p.Valor, 0)) AS DECIMAL(19, 2)) " +
                            "FROM PAGO_TICKET p LEFT JOIN FACTURA_TICKET f ON f.id_factura_ticket = p.Id_Factura_Ticket " +
                            "WHERE f.Id_Cierre = {0} AND f.Estado = 1", ID)));
                    case "PP":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(p.Valor, 0)) AS DECIMAL(19, 2)) " +
                            "FROM PAGO_PARQUEO p LEFT JOIN FACTURA_PARQUEO f ON f.Id_factura_parqueo = p.Id_Factura_Parqueo " +
                            "WHERE f.Id_Cierre = {0} AND f.Estado = 1", ID)));
                    case "BT":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(p.Valor, 0)) AS DECIMAL(19, 2)) " +
                            "FROM PAGO_BOLETO p LEFT JOIN FACTURA_BOLETO f ON f.Id_factura_boleto = p.Id_Factura_Boleto " +
                            "WHERE f.Id_Cierre = {0} AND f.Estado = 1", ID)));
                    case "ADM":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(p.Valor, 0)) AS DECIMAL(19, 2)) " +
                            "FROM PAGO_RECAUDA p LEFT JOIN FACTURA_RECAUDA f ON f.Id_factura_recauda = p.Id_Factura_Recauda " +
                            "WHERE f.Id_Cierre = {0} AND f.Estado = 1", ID)));
                    default:
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public decimal ObtenerValorPagosCierre(long ID, int IDTPago)
        {
            try
            {
                //TODO: Cambiar para el resto.
                switch (caja.tipo_caja.Codigo)
                {
                    case "FR":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(p.Valor, 0)) AS DECIMAL(19,2)) " +
                            "FROM PAGO p LEFT JOIN FACTURA f ON f.Id_factura = p.Id_Factura " +
                            "WHERE f.Id_Cierre = {0} AND f.Estado = 1 AND p.Id_Tipo_Pago = {1}", ID, IDTPago)));
                    case "TU":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(p.Valor, 0)) AS DECIMAL(19, 2)) " +
                            "FROM PAGO_TICKET p LEFT JOIN FACTURA_TICKET f ON f.id_factura_ticket = p.Id_Factura_Ticket " +
                            "WHERE f.Id_Cierre = {0} AND f.Estado = 1 AND p.Id_Tipo_Pago = {1}", ID, IDTPago)));
                    case "PP":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(p.Valor, 0)) AS DECIMAL(19, 2)) " +
                            "FROM PAGO_PARQUEO p LEFT JOIN FACTURA_PARQUEO f ON f.Id_factura_parqueo = p.Id_Factura_Parqueo " +
                            "WHERE f.Id_Cierre = {0} AND f.Estado = 1 AND p.Id_Tipo_Pago = {1}", ID, IDTPago)));
                    case "BT":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(p.Valor, 0)) AS DECIMAL(19, 2)) " +
                            "FROM PAGO_BOLETO p LEFT JOIN FACTURA_BOLETO f ON f.Id_factura_boleto = p.Id_Factura_Boleto " +
                            "WHERE f.Id_Cierre = {0} AND f.Estado = 1 AND p.Id_Tipo_Pago = {1}", ID, IDTPago)));
                    case "ADM":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(p.Valor, 0)) AS DECIMAL(19, 2)) " +
                            "FROM PAGO_RECAUDA p LEFT JOIN FACTURA_RECAUDA f ON f.Id_factura_recauda = p.Id_Factura_Recauda " +
                            "WHERE f.Id_Cierre = {0} AND f.Estado = 1 AND p.Id_Tipo_Pago = {1}", ID, IDTPago)));
                    default:
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public decimal ObtenerValorFacturas(long ID)
        {
            try
            {
                switch (caja.tipo_caja.Codigo)
                {
                    case "FR":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(Valor_Total, 0)) AS DECIMAL(19, 2)) " +
                            "FROM FACTURA WHERE Estado = 1 AND Id_Cierre = {0}", ID)));
                    case "TU":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(Valor_Total, 0)) AS DECIMAL(19, 2)) " +
                            "FROM FACTURA_TICKET WHERE Estado = 1 AND Id_Cierre = {0}", ID)));
                    case "PP":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(Valor_Total, 0)) AS DECIMAL(19, 2)) " +
                            "FROM FACTURA_PARQUEO WHERE Estado = 1 AND Id_Cierre = {0}", ID)));
                    case "BT":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(Valor_Total, 0)) AS DECIMAL(19, 2)) " +
                            "FROM FACTURA_BOLETO WHERE Estado = 1 AND Id_Cierre = {0}", ID)));
                    case "ADM":
                        return Convert.ToDecimal(SqlServer.EXEC_SCALAR(
                            string.Format("SELECT CAST(SUM(ISNULL(Valor_Total, 0)) AS DECIMAL(19, 2)) " +
                            "FROM FACTURA_RECAUDA WHERE Estado = 1 AND Id_Cierre = {0}", ID)));
                    default:
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public decimal ObtenerValorFacturas(long ID, int IDTPago)
        {
            try
            {
                switch (caja.tipo_caja.Codigo)
                {
                    case "FR":
                        return Convert.ToDecimal(
                            SqlServer.EXEC_SCALAR(
                                string.Format("SELECT CAST(SUM(ISNULL(f.Valor_Total, 0)) AS DECIMAL(19, 2)) " +
                                "FROM FACTURA f INNER JOIN PAGO p ON p.Id_factura = f.Id_factura " +
                                "WHERE f.Estado = 1 AND f.Id_Cierre = {0} AND p.Id_Tipo_Pago = {1}", ID, IDTPago)));
                    case "TU":
                        return Convert.ToDecimal(
                            SqlServer.EXEC_SCALAR(
                                string.Format("SELECT CAST(SUM(ISNULL(f.Valor_Total, 0)) AS DECIMAL(19, 2)) " +
                                "FROM FACTURA_TICKET f INNER JOIN PAGO_TICKET p ON p.Id_Factura_Ticket = f.id_factura_ticket " +
                                "WHERE f.Estado = 1 AND f.Id_Cierre = {0} AND p.Id_Tipo_Pago = {1}", ID, IDTPago)));
                    case "PP":
                        return Convert.ToDecimal(
                            SqlServer.EXEC_SCALAR(
                                string.Format("SELECT CAST(SUM(ISNULL(f.Valor_Total, 0)) AS DECIMAL(19, 2)) " +
                                "FROM FACTURA_PARQUEO f INNER JOIN PAGO_PARQUEO p ON p.Id_Factura_Parqueo = f.Id_factura_parqueo " +
                                "WHERE f.Estado = 1 AND f.Id_Cierre = {0} AND p.Id_Tipo_Pago = {1}", ID, IDTPago)));
                    case "BT":
                        return Convert.ToDecimal(
                            SqlServer.EXEC_SCALAR(
                                string.Format("SELECT CAST(SUM(ISNULL(f.Valor_Total, 0)) AS DECIMAL(19, 2)) " +
                                "FROM FACTURA_BOLETO f INNER JOIN PAGO_BOLETO p ON p.Id_Factura_Boleto = f.Id_factura_boleto " +
                                "WHERE f.Estado = 1 AND f.Id_Cierre = {0} AND p.Id_Tipo_Pago = {1}", ID, IDTPago)));
                    case "ADM":
                        return Convert.ToDecimal(
                            SqlServer.EXEC_SCALAR(
                                string.Format("SELECT CAST(SUM(ISNULL(f.Valor_Total, 0)) AS DECIMAL(19, 2)) " +
                                "FROM FACTURA_RECAUDA f INNER JOIN PAGO_RECAUDA p ON p.Id_Factura_Recauda = f.Id_factura_recauda " +
                                "WHERE f.Estado = 1 AND f.Id_Cierre = {0} AND p.Id_Tipo_Pago = {1}", ID, IDTPago)));
                    default:
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public decimal ObtenerValorEspeciesValoradas(long ID)
        {
            try
            {
                return Convert.ToDecimal(SqlServer.EXEC_SCALAR("SELECT CAST(SUM(ISNULL(Valor, 0)) AS DECIMAL(19,2)) FROM ESPECIES_CAJA " +
                    "WHERE Estado = 3 AND Id_Cierre_Emision = " + ID.ToString()));
            }
            catch
            {
                return 0;
            }
        }

        public decimal ObtenerValorEspeciesValoradas(long ID, int IDTPago)
        {
            try
            {
                return Convert.ToDecimal(
                    SqlServer.EXEC_SCALAR(
                        String.Format("SELECT CAST(SUM(ISNULL(Valor, 0)) AS DECIMAL(19,2)) " +
                        "FROM ESPECIES_CAJA WHERE Estado = 3 AND Id_Cierre_Emision = {0} AND Id_Tipo_Pago = {1}", ID, IDTPago)));
            }
            catch
            {
                return 0;
            }
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
        ~Cierre_caja()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
