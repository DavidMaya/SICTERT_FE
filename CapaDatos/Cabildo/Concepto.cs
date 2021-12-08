﻿using AccesoDatos;
using CapaDatos.Ingresos;
using System;
using System.Data;

namespace CapaDatos.Cabildo
{
    public partial class Cabildo_Concepto : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long Id_Concepto_Cuenta { get; set; }

        public int Cabildo_Codigo { get; set; }

        #endregion

        public Cabildo_Concepto()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Cabildo_Concepto(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Concepto_Cuenta = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Concepto_Cuenta = Convert.ToInt64(dt.Rows[0]["Id_Concepto_Cuenta"]);
                    Cabildo_Codigo = Convert.ToInt32(dt.Rows[0]["Cabildo_Codigo"]);
                }
            }
            //throw new ApplicationException("Concepto_cuenta does not exist.");
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
            return String.Format("SELECT Id_Concepto_Cuenta, Cabildo_Codigo FROM CABILDO_CONCEPTO");
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

        public static Concepto_cuenta GetConcepto_cuenta(long id)
        {
            return new Concepto_cuenta(id);
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
            return String.Format("INSERT INTO CABILDO_CONCEPTO (Id_Concepto_Cuenta, Cabildo_Codigo) " +
                                 "VALUES ('{0}','{1}'); ", Id_Concepto_Cuenta, Cabildo_Codigo);
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
            return String.Format("DELETE FROM CABILDO_CONCEPTO WHERE Id_Concepto_Cuenta = {0};", ID);
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
            return String.Format("UPDATE CABILDO_CONCEPTO SET Cabildo_Codigo = '{1}' WHERE Id_Concepto_Cuenta = {0};", Id_Concepto_Cuenta, Cabildo_Codigo);
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
        ~Cabildo_Concepto()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
