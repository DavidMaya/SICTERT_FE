using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CapaDatos.Boleteria
{
    public class PasajeroViaje: SqlServer, IDisposable
    {
        #region Public Properties
        public long IdPasajeroViaje { get; set; }
        public int NumeroAsiento { get; set; }
        public long IdEstadoAsiento { get; set; }
        public EstadoAsiento EstadoAsiento { get; set; }
        public long IdFacturaBoleto { get; set; }
        public decimal ValorBoleto { get; set; }
        public decimal IvaBoleto { get; set; }
        public decimal IvaValorBoleto { get; set; }
        public decimal ValorTasaUsuario { get; set; }
        public decimal IvaTasaUsuario { get; set; }
        public decimal IvaValorTasaUsuario { get; set; }
        public long IdViaje { get; set; }
        public long IdDestinoRuta { get; set; }
        public long IdTarifaViaje { get; set; }
        public string NombreDestinoRuta { get; set; }
        public long IdPasajero { get; set; }
        public string CedulaRuc { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public bool TerceraEdad { get; set; }
        public bool Discapacitado { get; set; }
        public bool MenorEdad { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public string FechaNacimientoSQL
        {
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
        public long IdCCBoleteria { get; set; }
        public string NombreCC { get; set; }
        public long IdCCTasaUsuario { get; set; }
        public string NombreTasaUsuario { get; set; }
        public string CodigoBarras { get; set; }
        public string ScriptTicket { get; set; }
        #endregion

        #region Constructor
        public PasajeroViaje() { }

        public PasajeroViaje(long id)
        {
            string sql = String.Format(@"SELECT Id_Pasajero_Viaje, Num_Asiento, Valor_Boleto, Iva_Boleto,
                Iva_Valor_Boleto, Valor_TasaUsuario, Iva_TasaUsuario, Iva_Valor_TasaUsuario, Id_Viaje,
                Id_Factura_Boleto, Nombre_Destino, Id_Pasajero, Cedula, Nombre, Tercera_Edad, Discapacitado,
                Menor_Edad, Id_Estado_Asiento, Id_Destino_Ruta, Id_TarifaViaje, Direccion, Telefono
                FROM PASAJERO_VIAJE WHERE Id_Pasajero_Viaje = {0}", id);

            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                if (table.Rows.Count > 0)
                {
                    IdPasajeroViaje = Convert.ToInt64(table.Rows[0]["Id_Pasajero_Viaje"]);
                    NumeroAsiento = Convert.ToInt32(table.Rows[0]["Num_Asiento"]);
                    IdEstadoAsiento = Convert.ToInt64(table.Rows[0]["Id_Estado_Asiento"]);
                    EstadoAsiento = new EstadoAsiento(IdEstadoAsiento);
                    IdFacturaBoleto = Convert.ToInt64(table.Rows[0]["Id_Factura_Boleto"]);
                    ValorBoleto = Convert.ToDecimal(table.Rows[0]["Valor_Boleto"]);
                    IvaBoleto = Convert.ToInt64(table.Rows[0]["Iva_Boleto"]);
                    IvaValorBoleto = Convert.ToDecimal(table.Rows[0]["Iva_Valor_Boleto"]);
                    ValorTasaUsuario = Convert.ToDecimal(table.Rows[0]["Valor_TasaUsuario"]);
                    IvaTasaUsuario = Convert.ToInt64(table.Rows[0]["Iva_TasaUsuario"]);
                    IvaValorTasaUsuario = Convert.ToDecimal(table.Rows[0]["Iva_Valor_TasaUsuario"]);
                    IdViaje = Convert.ToInt64(table.Rows[0]["Id_Viaje"]);
                    IdDestinoRuta = Convert.ToInt64(table.Rows[0]["Id_Destino_Ruta"]);
                    NombreDestinoRuta = Convert.ToString(table.Rows[0]["Nombre_Destino"]);
                    IdPasajero = Convert.ToInt64(table.Rows[0]["Id_Pasajero"]);
                    CedulaRuc = table.Rows[0]["Cedula"] != DBNull.Value ? Convert.ToString(table.Rows[0]["Cedula"]) : "";
                    Nombre = Convert.ToString(table.Rows[0]["Nombre"]);
                    Direccion = table.Rows[0]["Direccion"] != DBNull.Value ? Convert.ToString(table.Rows[0]["Direccion"]) : "";
                    Telefono = table.Rows[0]["Telefono"] != DBNull.Value ? Convert.ToString(table.Rows[0]["Telefono"]) : "";
                    TerceraEdad = Convert.ToBoolean(table.Rows[0]["Tercera_Edad"]);
                    Discapacitado = Convert.ToBoolean(table.Rows[0]["Discapacitado"]);
                    MenorEdad = Convert.ToBoolean(table.Rows[0]["Menor_Edad"]);
                    IdTarifaViaje = Convert.ToInt64(table.Rows[0]["Id_TarifaViaje"]);
                }
            }
        }
        #endregion

        #region Select Data Method
        private static string GetSqlSelect()
        {
            return String.Format(@"SELECT Id_Pasajero_Viaje, Num_Asiento, Valor_Boleto, Iva_Boleto,
                Iva_Valor_Boleto, Valor_TasaUsuario, Iva_TasaUsuario, Iva_Valor_TasaUsuario, Id_Viaje,
                Id_Factura_Boleto, Nombre_Destino, Id_Pasajero, Cedula, Nombre, Tercera_Edad, Discapacitado,
                Menor_Edad, Id_Estado_Asiento, Id_Destino_Ruta, Id_TarifaViaje, Direccion, Telefono
                FROM PASAJERO_VIAJE");
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

        public static PasajeroViaje GetFacturaBoleto(long id)
        {
            return new PasajeroViaje(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Pasajero_Viaje", "PASAJERO_VIAJE", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        public static List<PasajeroViaje> List(long idViaje)
        {
            List<PasajeroViaje> lista = new List<PasajeroViaje>();

            using (DataTable table = GetAllData(string.Format("Id_Viaje = {0}", idViaje)))
            {
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        lista.Add(new PasajeroViaje
                        {
                            IdPasajeroViaje = Convert.ToInt64(row["Id_Pasajero_Viaje"]),
                            NumeroAsiento = Convert.ToInt32(row["Num_Asiento"]),
                            IdEstadoAsiento = Convert.ToInt64(row["Id_Estado_Asiento"]),
                            IdFacturaBoleto = Convert.ToInt64(row["Id_Factura_Boleto"]),
                            ValorBoleto = Convert.ToDecimal(row["Valor_Boleto"]),
                            IvaBoleto = Convert.ToInt64(row["Iva_Boleto"]),
                            IvaValorBoleto = Convert.ToDecimal(row["Iva_Valor_Boleto"]),
                            ValorTasaUsuario = Convert.ToDecimal(row["Valor_TasaUsuario"]),
                            IvaTasaUsuario = Convert.ToInt64(row["Iva_TasaUsuario"]),
                            IvaValorTasaUsuario = Convert.ToDecimal(row["Iva_Valor_TasaUsuario"]),
                            IdViaje = Convert.ToInt64(row["Id_Viaje"]),
                            IdDestinoRuta = Convert.ToInt64(row["Id_Destino_Ruta"]),
                            NombreDestinoRuta = Convert.ToString(row["Nombre_Destino"]),
                            IdPasajero = Convert.ToInt64(row["Id_Pasajero"]),
                            CedulaRuc = row["Cedula"] != DBNull.Value ? Convert.ToString(row["Cedula"]) : "",
                            Nombre = Convert.ToString(row["Nombre"]),
                            TerceraEdad = Convert.ToBoolean(row["Tercera_Edad"]),
                            Discapacitado = Convert.ToBoolean(row["Discapacitado"]),
                            MenorEdad = Convert.ToBoolean(row["Menor_Edad"]),
                            EstadoAsiento = new EstadoAsiento(Convert.ToInt64(row["Id_Estado_Asiento"])),
                            IdTarifaViaje = Convert.ToInt64(row["Id_TarifaViaje"]),
                            Direccion = row["Direccion"] != DBNull.Value ? Convert.ToString(row["Direccion"]) : "",
                            Telefono = row["Telefono"] != DBNull.Value ? Convert.ToString(row["Telefono"]) : ""
                        });
                    }
                }
                return lista;
            }
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
        private string GetSQLInsert()
        {
            string sql = String.Format(@"INSERT INTO PASAJERO_VIAJE (Num_Asiento, Valor_Boleto, Iva_Boleto, Iva_Valor_Boleto,
                Valor_TasaUsuario, Iva_TasaUsuario, Iva_Valor_TasaUsuario, Id_Viaje, Id_Factura_Boleto, Nombre_Destino, Id_Pasajero, 
                Cedula, Nombre, Tercera_Edad, Discapacitado, Menor_Edad, Id_Estado_Asiento, Id_Destino_Ruta, Id_TarifaViaje, Direccion, Telefono)
                VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, '{9}', {10}, '{11}', '{12}', {13}, {14}, {15}, {16}, {17}, {18}, '{19}', '{20}')",
                NumeroAsiento, ValorBoleto.ToString().Replace(",", SqlServer.SigFloatSql), IvaBoleto,
                IvaValorBoleto.ToString().Replace(",", SqlServer.SigFloatSql), ValorTasaUsuario.ToString().Replace(",", SqlServer.SigFloatSql),
                IvaTasaUsuario, IvaValorTasaUsuario.ToString().Replace(",", SqlServer.SigFloatSql), IdViaje, IdFacturaBoleto, NombreDestinoRuta,
                IdPasajero, CedulaRuc, SqlServer.ValidarTexto(Nombre), TerceraEdad ? 1 : 0, Discapacitado ? 1 : 0, MenorEdad ? 1 : 0, IdEstadoAsiento, IdDestinoRuta,
                IdTarifaViaje, SqlServer.ValidarTexto(Direccion), SqlServer.ValidarTexto(Telefono));

            return sql;
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

        private string GetSQLUpdate()
        {
            return String.Format(@"UPDATE PASAJERO_VIAJE SET Num_Asiento = {0}, Valor_Boleto = {1}, Iva_Boleto = {2}, Iva_Valor_Boleto = {3},
                Valor_TasaUsuario = {4}, Iva_TasaUsuario = {5}, Iva_Valor_TasaUsuario = {6}, Id_Viaje = {7}, Id_Factura_Boleto = {8}, Nombre_Destino = {9},
                Id_Pasajero = {10}, Cedula = {11}, Nombre = {12}, Tercera_Edad = {13}, Discapacitado = {14}, Menor_Edad = {15}, Id_Estado_Asiento = {16},
                Id_Destino_Ruta = {17}, Id_TarifaViaje = {18}, Direccion = '{19}', Telefono = '{20}' WHERE Id_Pasajero_Viaje = {21}",
                NumeroAsiento, ValorBoleto.ToString().Replace(",", SqlServer.SigFloatSql), IvaBoleto,
                IvaValorBoleto.ToString().Replace(",", SqlServer.SigFloatSql), ValorTasaUsuario.ToString().Replace(",", SqlServer.SigFloatSql),
                IvaTasaUsuario, IvaValorTasaUsuario.ToString().Replace(",", SqlServer.SigFloatSql), IdViaje, IdFacturaBoleto, NombreDestinoRuta,
                IdPasajero, CedulaRuc, SqlServer.ValidarTexto(Nombre), TerceraEdad ? 1 : 0, Discapacitado ? 1 : 0, MenorEdad ? 1 : 0, IdEstadoAsiento, IdDestinoRuta,
                IdTarifaViaje, SqlServer.ValidarTexto(Direccion), SqlServer.ValidarTexto(Telefono), IdPasajeroViaje);
        }
        #endregion

        #region Delete Data Method
        public string Delete(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
        }
        private string GetSQLDelete(long id)
        {
            return String.Format("DELETE FROM PASAJERO_VIAJE WHERE Id_Pasajero_Viaje = {0};", id);
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
        ~PasajeroViaje()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
