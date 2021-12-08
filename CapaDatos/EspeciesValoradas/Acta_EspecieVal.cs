using AccesoDatos;
using CapaDatos.Ingresos;
using CapaDatos.UsuariosPerfiles;
using System;
using System.Data;

namespace CapaDatos.EspeciesValoradas
{
    public partial class Acta_EspecieVal : SqlConnecion, IDisposable
    {
        private long _Id_Tipo_Especie;
        private Tipo_especie_val _Tipo_Especie;
        private long _Id_Caja;
        private Caja _Caja;
        private long _Id_Usuario;
        private Usuarios _Usuario;
        private long _Id_Recaudador;
        private Usuarios _Recaudador;

        #region Public Properties
        public long Id_Acta_EspVal { get; set; }
        public long Id_Tipo_Especie
        {
            get { return _Id_Tipo_Especie; }
            set { 
                _Id_Tipo_Especie = value;
                _Tipo_Especie = null;
            }
        }
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
        public long Num_Acta { get; set; }
        public DateTime Fecha { get; set; }
        public long Num_Inicial { get; set; }
        public long Num_Final { get; set; }
        public long Id_Caja
        {
            get { return _Id_Caja; }
            set { 
                _Id_Caja = value;
                _Caja = null;
            }
        }
        public string Nombre_Caja { get; set; }
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
        public long Id_Usuario
        {
            get { return _Id_Usuario; }
            set { 
                _Id_Usuario = value;
                _Usuario = null;
            }
        }
        public string Nombre_Usuario { get; set; }
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
        public long Id_Recaudador
        {
            get { return _Id_Recaudador; }
            set { 
                _Id_Recaudador = value;
                _Recaudador = null;
            }
        }
        public string Nombre_Recaudador { get; set; }
        public Usuarios Recaudador
        {
            get
            {
                if (_Recaudador != null && _Recaudador.Id_Usuario != 0)
                    return _Recaudador;
                else if (_Id_Recaudador != 0)
                    return _Recaudador = new Usuarios(_Id_Recaudador);
                else
                    return null;
            }
        }
        public bool EsDevolucion { get; set; }
        public string Observacion { get; set; }
        public decimal Valor_Unitario { get; set; }
        public long Cantidad { get; set; }
        public decimal Valor_Total { get; set; }
        #endregion

        public Acta_EspecieVal()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Acta_EspecieVal(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Acta_EspVal = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Acta_EspVal = Convert.ToInt64(dt.Rows[0]["Id_Acta_EspVal"]);
                    _Id_Tipo_Especie = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Especie"]);
                    Fecha = Convert.ToDateTime(dt.Rows[0]["Fecha"]);
                    Num_Acta = Convert.ToInt64(dt.Rows[0]["Num_Acta"]);
                    Num_Inicial = Convert.ToInt64(dt.Rows[0]["Num_Inicial"]);
                    Num_Final = Convert.ToInt64(dt.Rows[0]["Num_Final"]);
                    _Id_Caja = Convert.ToInt64(dt.Rows[0]["Id_Caja"]);
                    Nombre_Caja = Convert.ToString(dt.Rows[0]["Caja"]);
                    _Id_Usuario = Convert.ToInt64(dt.Rows[0]["Id_Usuario"]);
                    Nombre_Usuario = Convert.ToString(dt.Rows[0]["Usuario"]);
                    _Id_Recaudador = Convert.ToInt64(dt.Rows[0]["Id_Recaudador"]);
                    Nombre_Recaudador = Convert.ToString(dt.Rows[0]["Recaudador"]);
                    EsDevolucion = Convert.ToBoolean(dt.Rows[0]["EsDevolucion"]);
                    Observacion = Convert.ToString(dt.Rows[0]["Observacion"]);
                    Valor_Unitario = Convert.ToDecimal(dt.Rows[0]["Valor_Unitario"]);
                    Cantidad = Convert.ToInt64(dt.Rows[0]["Cantidad"]);
                    Valor_Total = Convert.ToDecimal(dt.Rows[0]["Valor_Total"]);
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
            return String.Format("SELECT Id_Acta_EspVal,Id_Tipo_Especie,Fecha,Num_Acta,Num_Inicial,Num_Final,Id_Caja,Caja,Id_Usuario,Usuario,Id_Recaudador,Recaudador,EsDevolucion,Observacion,Valor_Unitario,Cantidad,Valor_Total FROM ACTA_ESPECIES");
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

        public static Acta_EspecieVal GetCaja(long id)
        {
            return new Acta_EspecieVal(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Acta_EspVal", "ACTA_ESPECIES", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        static public long Next_Num_Acta()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Num_Acta", "ACTA_ESPECIES", "");
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
            return String.Format("INSERT INTO ACTA_ESPECIES ( Id_Tipo_Especie,Fecha,Num_Acta,Num_Inicial,Num_Final,Id_Caja,Caja,Id_Usuario,Usuario," +
                "Id_Recaudador,Recaudador,EsDevolucion,Observacion,Valor_Unitario,Cantidad,Valor_Total) " +
                "VALUES( '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}' ); ",
                Id_Tipo_Especie, Convert.ToDateTime(Fecha.ToString()).ToString(SqlServer.FormatofechaHora), Num_Acta, Num_Inicial, Num_Final, Id_Caja, 
                Nombre_Caja, Id_Usuario, Nombre_Usuario, Id_Recaudador, Nombre_Recaudador, EsDevolucion, Observacion, 
                Valor_Unitario.ToString().Replace(",", SqlServer.SigFloatSql), Cantidad, Valor_Total.ToString().Replace(",", SqlServer.SigFloatSql));
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
            return String.Format("DELETE FROM ACTA_ESPECIES WHERE Id_Acta_EspVal = {0};", ID);
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
            return String.Format("UPDATE ACTA_ESPECIES SET Id_Tipo_Especie= '{1}', Fecha= '{2}', Num_Acta= '{3}', Num_Inicial= '{4}', " +
                "Num_Final= '{5}', Id_Caja= '{6}', Caja= '{7}', Id_Usuario= '{8}', Usuario= '{9}', Id_Recaudador= '{10}', Recaudador= '{11}', " +
                "EsDevolucion= '{12}', Observacion= '{13}', Valor_Unitario= '{14}', Cantidad= '{15}', Valor_Total= '{16}' WHERE Id_Acta_EspVal = {0};", 
                Id_Acta_EspVal, Id_Tipo_Especie, Convert.ToDateTime(Fecha.ToString()).ToString(SqlServer.FormatofechaHora), Num_Acta, Num_Inicial, 
                Num_Final, Id_Caja, caja, Id_Usuario, Usuario, Id_Recaudador, Recaudador, EsDevolucion, Observacion, 
                Valor_Unitario.ToString().Replace(",", SqlServer.SigFloatSql), Cantidad, Valor_Total.ToString().Replace(",", SqlServer.SigFloatSql));
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
        ~Acta_EspecieVal()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
