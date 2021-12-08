using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.Frecuencias
{
    public partial class Factura_concepto : SqlConnecion, IDisposable
    {
        private long _Id_Factura;
        private long _Id_Concepto_Cuenta;
        private Factura _Factura;
        private Concepto_cuenta _Concepto_Cuenta;

        #region Public Properties
        public long Id_Factura_Concepto { get; set; }
        public long Id_Factura
        {
            get { return _Id_Factura; }
            set { 
                _Id_Factura = value;
                _Factura = null;
            }
        }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public int Cantidad { get; set; }
        public decimal Iva { get; set; }
        public long Id_Concepto_Cuenta
        {
            get { return _Id_Concepto_Cuenta; }
            set { 
                _Id_Concepto_Cuenta = value;
                _Concepto_Cuenta = null;
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
        #endregion

        public Factura_concepto()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Factura_concepto(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Factura_Concepto = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Factura_Concepto = Convert.ToInt64(dt.Rows[0]["Id_Factura_Concepto"]);
                    _Id_Factura = Convert.ToInt64(dt.Rows[0]["Id_Factura"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    Cantidad = Convert.ToInt32(dt.Rows[0]["Cantidad"]);
                    Iva = Convert.ToDecimal(dt.Rows[0]["Iva"]);
                    _Id_Concepto_Cuenta = Convert.ToInt64(dt.Rows[0]["Id_Concepto_Cuenta"]);
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
            return String.Format("SELECT Id_Factura_Concepto,Id_Factura,Nombre,Valor,Cantidad,Iva,Id_Concepto_Cuenta FROM Factura_concepto");
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

        public static Factura_concepto GetFactura_concepto(long id)
        {
            return new Factura_concepto(id);
        }

        static public long Next_Codigo()
        {
            //string sql = SqlServer.GetFormatoSQLNEXT("Id_Factura_Concepto", "Factura_concepto", "");
            //return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
            string sql = "EXEC ObtenerProximoID 'Id_Factura_concepto'";
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
            //return String.Format("INSERT INTO Factura_concepto ( Id_Factura_Concepto,Id_Factura,Nombre,Valor,Cantidad,Iva,Id_Concepto_Cuenta ) " +
            //    " VALUES( '{0}','{1}','{2}','{3}','{4}','{5}','{6}'); ", Id_Factura_Concepto, Id_Factura, SqlServer.ValidarTexto(Nombre),
            //    Valor.ToString().Replace(",", SqlServer.SigFloatSql), Cantidad.ToString().Replace(",", SqlServer.SigFloatSql), Iva.ToString().Replace(",", SqlServer.SigFloatSql), Id_Concepto_Cuenta);
            return String.Format("INSERT INTO Factura_concepto (Id_Factura,Nombre,Valor,Cantidad,Iva,Id_Concepto_Cuenta) " +
                "VALUES( '{0}','{1}','{2}','{3}','{4}','{5}'); ", Id_Factura, SqlServer.ValidarTexto(Nombre), 
                Valor.ToString().Replace(",",SqlServer.SigFloatSql), Cantidad.ToString().Replace(",",SqlServer.SigFloatSql), 
                Iva.ToString().Replace(",",SqlServer.SigFloatSql), Id_Concepto_Cuenta);
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
            return String.Format("DELETE FROM Factura_concepto WHERE Id_Factura_Concepto = {0};", ID);
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
            return String.Format("UPDATE Factura_concepto SET Id_Factura= '{1}',Nombre= '{2}',Valor= '{3}',Cantidad= '{4}',Iva= '{5}',Id_Concepto_Cuenta= '{6}' " +
                "WHERE Id_Factura_Concepto = {0};", Id_Factura_Concepto, Id_Factura, SqlServer.ValidarTexto(Nombre), Valor.ToString().Replace(",",SqlServer.SigFloatSql), 
                Cantidad.ToString(), Iva.ToString().Replace(",",SqlServer.SigFloatSql), Id_Concepto_Cuenta);
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
        ~Factura_concepto()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
