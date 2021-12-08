using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public class Dinero_Cierre : SqlConnecion, IDisposable
    {
        private int _Id_Dinero;
        private long _Id_Cierre;
        private Dinero _Dinero;
        private Cierre_caja _Cierre;

        #region Public Properties
        public int Id_Dinero 
        {
            get { return _Id_Dinero; }
            set {
                _Id_Dinero = value;
                _Dinero = null;
            } 
        }
        public Dinero dinero
        {
            get
            {
                if (_Dinero != null && _Dinero.Id_Dinero != 0)
                    return _Dinero;
                else if (_Id_Dinero != 0)
                    return _Dinero = new Dinero(_Id_Dinero);
                else
                    return null;
            }
        }
        public decimal Valor { get; set; }
        public string Denominacion { get; set; }
        public decimal Subtotal { get; set; }
        public long Id_Cierre 
        {
            get { return _Id_Cierre; }
            set {
                _Id_Cierre = value;
                _Cierre = null;
            } 
        }
        public Cierre_caja Cierre
        {
            get
            {
                if (_Cierre != null && _Cierre.Id_Cierre_Caja != 0)
                    return _Cierre;
                else if (_Id_Cierre != 0)
                    return _Cierre = new Cierre_caja(_Id_Cierre);
                else
                    return null;
            }
        }
        public int Cantidad { get; set; }
        #endregion

        public Dinero_Cierre()
        { 
        }
        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Dinero_Cierre(int id_Dinero, long id_Cierre)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Cierre = {0} and Id_Dinero = '{1}'", id_Cierre, id_Dinero)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Dinero = Convert.ToInt32(dt.Rows[0]["Id_Dinero"]);
                    Id_Cierre = Convert.ToInt64(dt.Rows[0]["Id_Cierre"]);
                    Valor = Convert.ToDecimal(dt.Rows[0]["Valor"]);
                    Cantidad = Convert.ToInt32(dt.Rows[0]["Cantidad"]);
                    Denominacion = Convert.ToString(dt.Rows[0]["Denominacion"]);
                    Subtotal = Convert.ToDecimal(dt.Rows[0]["Subtotal"]);
                }
            }
        }

        private static string GetSqlSelect()
        {
            return String.Format("SELECT Id_Dinero, Valor, Id_Cierre, Cantidad, Denominacion, Subtotal FROM DINERO_CIERRE");
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

        public static Dinero_Cierre GetDinero_Cierre(int id_Dinero, long id_Cierre)
        {
            return new Dinero_Cierre(id_Dinero, id_Cierre);
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
            return String.Format("INSERT INTO DINERO_CIERRE (Id_Dinero, Valor, Id_Cierre, Cantidad, Denominacion, Subtotal) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}'); ", 
                Id_Dinero, Valor.ToString().Replace(",", SqlServer.SigFloatSql), Id_Cierre, Cantidad, Denominacion, 
                Subtotal.ToString().Replace(",", SqlServer.SigFloatSql));
        }

        public string Delete(long id_Cierre)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id_Cierre));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string Delete(int id_Dinero, long id_Cierre)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id_Dinero, id_Cierre));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete(long id_Cierre)
        {
            return String.Format("DELETE FROM DINERO_CIERRE WHERE Id_Cierre = {0};", id_Cierre);
        }

        public string GetSQLDelete(int id_Dinero, long id_Cierre)
        {
            return String.Format("DELETE FROM DINERO_CIERRE WHERE Id_Cierre = {0} AND Id_Dinero = {1};", id_Cierre, Id_Dinero);
        }

        public string Update(int id_Dinero, long id_Cierre)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate(id_Dinero, id_Cierre));
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;
        }

        public string GetSQLUpdate(int id_Dinero, long id_Cierre)
        {
            return String.Format("UPDATE DINERO_CIERRE SET Denominacion = '{2}', Valor = '{3}', Cantidad = '{4}', Subtotal = {5} " +
                "WHERE Id_Cierre = {0} and Id_Dinero = '{1}'", 
                id_Cierre, id_Dinero, Denominacion, Valor.ToString().Replace(",", SqlServer.SigFloatSql), Cantidad, 
                Subtotal.ToString().Replace(",", SqlServer.SigFloatSql));
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
        ~Dinero_Cierre()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
