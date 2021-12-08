using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Frecuencias
{
    public partial class Ciudad : SqlConnecion, IDisposable
    {
        private long _Id_Region;
        private RegionPais _Region;

        #region Public Properties
        public long Id_Ciudad { get; set; }
        public string Nombre { get; set; }
        public string Nombre_TU { get; set; }
        public string Nombre_BT { get; set; }
        public string Abreviatura { get; set; }
        public long Id_Region
        {
            get { return _Id_Region; }
            set
            {
                _Id_Region = value;
                _Region = null;
            }
        }
        public RegionPais region
        {
            get
            {
                if (_Region != null && _Region.Id_Region != 0)
                    return _Region;
                else if (_Id_Region != 0)
                    return _Region = new RegionPais(_Id_Region);
                else
                    return null;
            }
        }
        public short NroBoton { get; set; }
        public bool Visible_TU { get; set; }
        public bool Visible_BT { get; set; }
        public bool Origen { get; set; }
        public bool Destino { get; set; }
        public bool Activo { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Ciudad()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Ciudad(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Ciudad = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Ciudad = Convert.ToInt64(dt.Rows[0]["Id_Ciudad"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Nombre_TU = Convert.ToString(dt.Rows[0]["Nombre_TU"]);
                    Nombre_BT = Convert.ToString(dt.Rows[0]["Nombre_BT"]);
                    Abreviatura = Convert.ToString(dt.Rows[0]["Abreviatura"]);
                    _Id_Region = Convert.ToInt64(dt.Rows[0]["Id_Region"]);
                    NroBoton = Convert.ToInt16(dt.Rows[0]["NroBoton"]);
                    Visible_TU = Convert.ToBoolean(dt.Rows[0]["Visible_TU"]);
                    Visible_BT = Convert.ToBoolean(dt.Rows[0]["Visible_BT"]);
                    Origen = Convert.ToBoolean(dt.Rows[0]["Origen"]);
                    Destino = Convert.ToBoolean(dt.Rows[0]["Destino"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
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
            return String.Format("SELECT Id_Ciudad, Nombre, Nombre_TU, Abreviatura, Id_Region, NroBoton, Visible_TU, Activo, Nombre_BT, " +
                "Visible_BT, Origen, Destino, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM CIUDAD");
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

        public static Ciudad GetCiudad(long id)
        {
            return new Ciudad(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Ciudad", "Ciudad", "");
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
            return String.Format("INSERT INTO Ciudad (Nombre, Nombre_TU, Abreviatura, Id_Region, NroBoton, Visible_TU, Activo, Nombre_BT, " +
                "Visible_BT, Origen, Destino, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
                "VALUES('{0}', '{1}', '{2}', {3}, {4}, {5}, {6}, '{7}', {8}, {9}, {10}, GETDATE(), {11}, {12}, {13}); ", 
                SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Nombre_TU), SqlServer.ValidarTexto(Abreviatura), Id_Region, NroBoton, 
                Visible_TU ? 1 : 0, Activo ? 1 : 0, SqlServer.ValidarTexto(Nombre_BT), Visible_BT ? 1 : 0, Origen ? 1 : 0, Destino ? 1 : 0,
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
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
            return String.Format("DELETE FROM Ciudad WHERE Id_Ciudad = {0};", ID);
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
            return String.Format("UPDATE CIUDAD SET Nombre = '{1}', Nombre_TU = '{2}', Abreviatura = '{3}', Id_Region = {4}, NroBoton = {5}, " +
                "Visible_TU = {6}, Activo = {7}, Nombre_BT = '{8}', Visible_BT = {9}, Origen = {10}, Destino = {11}, Fecha_Creacion = {12}, " +
                "Usuario_Creacion = {13}, Fecha_Modificacion = GETDATE(), Usuario_Modificacion = {14} WHERE Id_Ciudad = {0};", 
                Id_Ciudad, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Nombre_TU), 
                SqlServer.ValidarTexto(Abreviatura), Id_Region, NroBoton, Visible_TU ? 1 : 0, Activo ? 1 : 0, 
                SqlServer.ValidarTexto(Nombre_BT), Visible_BT ? 1 : 0, Origen ? 1 : 0, Destino ? 1 : 0,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Ciudad) FROM CIUDAD WHERE Activo = 1 AND Nombre = '{0}' {1}", 
                Nom, id != 0 ? "AND Id_Ciudad <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_NombreTU(string Nom)
        {
            return IsDuplicate_NombreTU(0, Nom);
        }

        public bool IsDuplicate_NombreTU(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Ciudad) FROM CIUDAD WHERE Activo = 1 AND Nombre_TU = '{0}' {1}", 
                Nom, id != 0 ? "AND Id_Ciudad <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_NombreBT(string Nom)
        {
            return IsDuplicate_NombreBT(0, Nom);
        }

        public bool IsDuplicate_NombreBT(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Ciudad) FROM CIUDAD WHERE Activo = 1 AND Nombre_BT = '{0}' {1}", 
                Nom, id != 0 ? "AND Id_Ciudad <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Abreviatura(string Abrev)
        {
            return IsDuplicate_Abreviatura(0, Abrev);
        }

        public bool IsDuplicate_Abreviatura(long id, string Abrev)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Ciudad) FROM CIUDAD WHERE Activo = 1 AND Abreviatura = '{0}' {1}", 
                Abrev, id != 0 ? "AND Id_Ciudad <> " + id.ToString() : ""))) > 0;
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
        ~Ciudad()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
