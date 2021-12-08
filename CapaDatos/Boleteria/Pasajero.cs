using AccesoDatos;
using System;
using System.Data;
using System.Linq;

namespace CapaDatos.Boleteria
{
    public class Pasajero: SqlConnecion, IDisposable
    {
        public long IdPasajero { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public bool TerceraEdad { get; set; }
        public bool Discapacitado { get; set; }
        public bool MenorEdad { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public string FechaNacimientoSQL { 
            get
            {
                return FechaNacimiento == null ? (String)null : Convert.ToDateTime(FechaNacimiento.ToString()).ToString(SqlServer.FormatofechaHora);
            }
        }

        public string FechaVerificacionSQL
        {
            get
            {
                return FechaVerificacion == null ? (String)null : Convert.ToDateTime(FechaVerificacion.ToString()).ToString(SqlServer.FormatofechaHora);
            }
        }

        public Pasajero() { }

        public Pasajero(long id)
        {
            string sql = String.Format(@"SELECT Id_Pasajero, Cedula, Nombre, Tercera_Edad, Discapacitado, Menor_Edad, Fecha_Nacimiento, 
                Fecha_Verificacion, Direccion, Telefono FROM PASAJERO WHERE Id_Pasajero = {0}", id);

            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                if(table.Rows.Count > 0)
                {
                    IdPasajero = Convert.ToInt64(table.Rows[0]["Id_Pasajero"]);
                    Cedula = table.Rows[0]["Cedula"] != DBNull.Value ? Convert.ToString(table.Rows[0]["Cedula"]) : "";
                    Nombre = Convert.ToString(table.Rows[0]["Nombre"]);
                    Direccion = table.Rows[0]["Direccion"] != DBNull.Value ? Convert.ToString(table.Rows[0]["Direccion"]) : "";
                    Telefono = table.Rows[0]["Telefono"] != DBNull.Value ? Convert.ToString(table.Rows[0]["Telefono"]) : "";
                    TerceraEdad = Convert.ToBoolean(table.Rows[0]["Tercera_Edad"]);
                    Discapacitado = Convert.ToBoolean(table.Rows[0]["Discapacitado"]);
                    MenorEdad = Convert.ToBoolean(table.Rows[0]["Menor_Edad"]);
                    FechaNacimiento = valorDateTime(table.Rows[0]["Fecha_Nacimiento"]);
                    FechaVerificacion = valorDateTime(table.Rows[0]["Fecha_Verificacion"]);
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

        #region Consultas
        private static string GetSqlSelect()
        {
            return String.Format(@"SELECT Id_Pasajero, Cedula, Nombre, Tercera_Edad, Discapacitado, Menor_Edad, Fecha_Nacimiento, 
                Fecha_Verificacion, Direccion, Telefono FROM PASAJERO");
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

        public static Pasajero GetPasajero(long id)
        {
            return new Pasajero(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Pasajero", "PASAJERO", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        static public long DataExists(string cedula, string nombre) {
            DataTable table = Pasajero.GetAllData(String.Format("Cedula = '{0}' AND Nombre = '{1}'", cedula, nombre));
            if (table.Rows.Count > 0)
                return Convert.ToInt64(table.Rows[0]["Id_Pasajero"]);
            else
                return 0;
        }
        #endregion

        #region Insert Data Method
        public string Insert()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }

        public string GetSQLInsert()
        {
            return String.Format(@"INSERT INTO PASAJERO (Cedula, Nombre, Tercera_Edad, Discapacitado, Menor_Edad, Fecha_Nacimiento, Fecha_Verificacion, Direccion, Telefono)
                VALUES ('{0}', '{1}', {2}, {3}, {4}, '{5}', '{6}', '{7}', '{8}')",
                SqlServer.ValidarTexto(Cedula), SqlServer.ValidarTexto(Nombre), TerceraEdad ? 1 : 0, Discapacitado ? 1 : 0, MenorEdad ? 1 :0,
                Convert.ToDateTime(FechaNacimiento.ToString()).ToString(SqlServer.Formatofecha),
                (FechaVerificacion == null) ? "null" : Convert.ToDateTime(FechaVerificacion.ToString()).ToString(SqlServer.Formatofecha), 
                SqlServer.ValidarTexto(Direccion), SqlServer.ValidarTexto(Telefono));

        }
        #endregion

        #region Update Data Method
        public string Update()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate());
            if (_return == "OK")
                _return = SqlServer.MensajeDeActualizar;
            return _return;

        }

        public string GetSQLUpdate()
        {
            return String.Format(@"UPDATE PASAJERO SET Cedula = {0}, Nombre = {1}, Tercera_Edad = {2}, Discapacitado = {3}, Menor_Edad = {4}, Fecha_Nacimiento = {5},
                Fecha_Verificacion = {6}, Direccion = '{7}', Telefono = '{8}' WHERE Id_Pasajero = {9}",
                SqlServer.ValidarTexto(Cedula), SqlServer.ValidarTexto(Nombre), TerceraEdad ? 1 : 0, Discapacitado ? 1 : 0, MenorEdad ? 1 : 0,
                Convert.ToDateTime(FechaNacimiento.ToString()).ToString(SqlServer.Formatofecha),
                Convert.ToDateTime(FechaVerificacion.ToString()).ToString(SqlServer.Formatofecha),
                SqlServer.ValidarTexto(Direccion), SqlServer.ValidarTexto(Telefono), IdPasajero);
        }
        #endregion

        #region Delete Update Method
        public string Delete(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }

        public string GetSQLDelete(long id)
        {
            return String.Format("DELETE FROM PASAJERO WHERE Id_Pasajero = {0};", id);

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
        ~Pasajero()
        {
            this.Dispose(false);
        }
        #endregion
    }
}