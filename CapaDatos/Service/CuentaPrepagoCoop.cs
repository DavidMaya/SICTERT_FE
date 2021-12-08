using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaDatos.Service
{
    public class CuentaPrepagoCoop : SqlConnecion, IDisposable
    {
        public long IdCuentaPrepago { get; set; }
        public long IdCooperativa { get; set; }
        public string Clave { get; set; }
        public bool Activo { get; set; }

        public CuentaPrepagoCoop() { }

        public CuentaPrepagoCoop(long id)
        {

        }

        #region Select Data Method
        private static string GetSqlSelect()
        {
            return String.Format(@"SELECT Id_CuentaPrepago, Id_Cooperativa, Clave, Activo FROM CUENTA_PREPAGO_COOP");
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

        public static CuentaPrepagoCoop RevisarCredeciales(string Cooperativa, string Clave)
        {
            string sql = String.Format(@"SELECT cpc.Id_Cooperativa, cpc.Id_CuentaPrepago, cpc.Activo
                FROM CUENTA_PREPAGO_COOP cpc INNER JOIN COOPERATIVA cc ON cpc.Id_Cooperativa = cc.Id_Cooperativa
                WHERE cc.Ruc = '{0}' AND cpc.Clave = '{1}'",
                Cooperativa, SqlServer.ValidarTexto(Clave));

            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                if (table.Rows.Count > 0)
                {
                    return new CuentaPrepagoCoop(){
                        IdCuentaPrepago = Convert.ToInt64(table.Rows[0]["Id_CuentaPrepago"]),
                        IdCooperativa = Convert.ToInt64(table.Rows[0]["Id_Cooperativa"]),
                        Activo = Convert.ToBoolean(table.Rows[0]["Activo"])
                    };
                }
            }

            return null;
        }
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
        ~CuentaPrepagoCoop()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
