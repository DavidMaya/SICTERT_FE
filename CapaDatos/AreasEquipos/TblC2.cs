﻿using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.AreasEquipos
{
    public partial class TblC2 : SqlConnecion, IDisposable
    {
        #region Public Properties
        public long Id { get; set; }
        public string Tag { get; set; }
        #endregion

        public TblC2()
        { }

        public TblC2(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id = Convert.ToInt64(dt.Rows[0]["Id"]);
                    Tag = Convert.ToString(dt.Rows[0]["Instruccion"]);
                }
            }
            //throw new ApplicationException("Area does not exist.");
        }

        private static string GetSqlSelect()
        {
            return String.Format("SELECT Id, Instruccion FROM TblC2");
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

        public static TblC2 GetArea(long id)
        {
            return new TblC2(id);
        }

        public void DeleteAll()
        {
            DeleteAll("");
        }

        public string DeleteAll(string TAG)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDeleteAll(TAG));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDeleteAll(string TAG)
        {
            return "DELETE FROM TblC2 " + ((TAG != "") ? String.Format("WHERE Instruccion like '%{0}%';", TAG) : "");
        }

        public string Insert(bool Habilitar)
        {
            DeleteAll(Tag);
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert(Habilitar));
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }

        public string GetSQLInsert(bool Habilitar)
        {
            return String.Format("INSERT INTO TblC2 (Instruccion) " +
                " VALUES('{0}{1}'); ", Habilitar ? "C " : "A ", Tag.Trim());
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
            return String.Format("DELETE FROM TblC2 WHERE Id = {0};", ID);
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
        ~TblC2()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
