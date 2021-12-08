using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace AccesoDatos
{
    public partial class Impresion : SqlConnecion, IDisposable
    {
        #region Public Properties

        public long ID_IMP { get; set; }

        public long ID_CIERRE { get; set; }

        public string SQL_IMP { get; set; }

        public string IP_IMP { get; set; }

        public string REP_IMP { get; set; }

        public string EST_IMP { get; set; }

        public string INC_IMG { get; set; }

        public string DIR_IMG { get; set; }

        public bool Impresion_Texto { get; set; }

        public Nullable<DateTime> Fecha_Hora { get; set; }

        public Nullable<DateTime> Fecha_Hora_Imp { get; set; }

        public int PAG_IMP { get; set; }

        public string Nombre_Impresora { get; set; }

        #endregion

        public Impresion()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Impresion(long id)
        {
            using (DataTable dt = GetAllData(String.Format("ID_IMP = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    ID_IMP = Convert.ToInt64(dt.Rows[0]["ID_IMP"]);
                    ID_CIERRE = Convert.ToInt64(dt.Rows[0]["ID_CIERRE"]);
                    SQL_IMP = Convert.ToString(dt.Rows[0]["SQL_IMP"]);
                    IP_IMP = Convert.ToString(dt.Rows[0]["IP_IMP"]);
                    REP_IMP = Convert.ToString(dt.Rows[0]["REP_IMP"]);
                    EST_IMP = Convert.ToString(dt.Rows[0]["EST_IMP"]);
                    INC_IMG = Convert.ToString(dt.Rows[0]["INC_IMG"]);
                    DIR_IMG = Convert.ToString(dt.Rows[0]["DIR_IMG"]);
                    Impresion_Texto = Convert.ToBoolean(dt.Rows[0]["Impresion_Texto"]);
                    Fecha_Hora = valorDateTime(dt.Rows[0]["Fecha_Hora"]);
                    Fecha_Hora_Imp = valorDateTime(dt.Rows[0]["Fecha_Hora_Imp"]);
                    PAG_IMP = Convert.ToInt32(dt.Rows[0]["PAG_IMP"]);
                    Nombre_Impresora = Convert.ToString(dt.Rows[0]["Nombre_Impresora"]);
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
            return String.Format("SELECT ID_IMP, ID_CIERRE, SQL_IMP, IP_IMP, REP_IMP, EST_IMP, INC_IMG, IMG_IMP, DIR_IMG, FECHA_HORA, FECHA_HORA_IMP, PAG_IMP, Impresion_Texto, Nombre_Impresora FROM IMPRESION");
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

        public static Impresion GetImpresion(long id)
        {
            return new Impresion(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("ID_IMP", "Impresion", "");
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
            return String.Format("INSERT INTO IMPRESION (SQL_IMP, IP_IMP, REP_IMP, EST_IMP, INC_IMG, IMG_IMP, DIR_IMG, ID_CIERRE, FECHA_HORA, " +
                "FECHA_HORA_IMP, PAG_IMP, Impresion_Texto, Nombre_Impresora) VALUES( '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', {7}, " +
                "GETDATE(), NULL, NULL, '{8}', '{9}'); ", SqlServer.ValidarTexto(SQL_IMP), SqlServer.ValidarTexto(IP_IMP), 
                SqlServer.ValidarTexto(REP_IMP), SqlServer.ValidarTexto(EST_IMP), SqlServer.ValidarTexto(INC_IMG), null, 
                SqlServer.ValidarTexto(DIR_IMG), ID_CIERRE == 0 ? "NULL" : ID_CIERRE.ToString(), Impresion_Texto, SqlServer.ValidarTexto(Nombre_Impresora));
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
            return String.Format("DELETE FROM IMPRESION WHERE ID_IMP = {0};", ID);
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
            return String.Format("UPDATE IMPRESION SET SQL_IMP= '{1}', IP_IMP= '{2}', REP_IMP= '{3}', EST_IMP= '{4}', INC_IMG= '{5}', " +
                "IMG_IMP= '{6}', DIR_IMP= '{7}', ID_CIERRE= {8}, FECHA_HORA= {9}, FECHA_HORA_IMP= {10}, PAG_IMP= {11}, Impresion_Texto= {12}, " +
                "Nombre_Impresora= {13} WHERE ID_IMP = {0};", ID_IMP, SqlServer.ValidarTexto(SQL_IMP), SqlServer.ValidarTexto(IP_IMP), 
                SqlServer.ValidarTexto(REP_IMP), SqlServer.ValidarTexto(EST_IMP), SqlServer.ValidarTexto(INC_IMG), null, 
                SqlServer.ValidarTexto(DIR_IMG), ID_CIERRE, Convert.ToDateTime(Fecha_Hora.ToString()).ToString(SqlServer.FormatofechaHora), 
                Convert.ToDateTime(Fecha_Hora_Imp.ToString()).ToString(SqlServer.FormatofechaHora), PAG_IMP, Impresion_Texto, Nombre_Impresora);
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
        ~Impresion()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
