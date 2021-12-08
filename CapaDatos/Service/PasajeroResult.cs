using AccesoDatos;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Service
{
    public class PasajeroResult
    {
        [JsonProperty("Id_Pasajero", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long IdPasajero { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        [JsonProperty("Direccion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Direccion { get; set; }
        [JsonProperty("Telefono", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Telefono { get; set; }
        [JsonProperty("FechaHora", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FechaHora { get; set; }
        [JsonProperty("Destino", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Destino { get; set; }
        [JsonProperty("Tercera_Edad", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool TerceraEdad { get; set; }
        [JsonProperty("Discapacitado", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Discapacitado { get; set; }
        [JsonProperty("Menor_Edad", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool MenorEdad { get; set; }
        [JsonProperty("Fecha_Nacimiento", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? FechaNacimiento { get; set; }
        [JsonProperty("Fecha_Verificacion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? FechaVerificacion { get; set; }
        [JsonProperty("Id_Tarifa", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long IdTarifa { get; set; }
        [JsonProperty("Tarifa_Nombre")]
        public string TarifaNombre { get; set; }
        [JsonProperty("Tarifa_Valor")]
        public decimal TarifaValor { get; set; }
        [JsonProperty("Id_Transaccion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long IdTransaccion { get; set; }
        [JsonProperty("Codigo_Barra", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CodigoBarra { get; set; }
        [JsonProperty("Id_Detalle_Ticket", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long IdDetalleTicket { get; set; }
        [JsonProperty("Id_Caja", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long IdCaja { get; set; }
        [JsonProperty("Iva")]
        public bool Iva { get; set; }
        [JsonProperty("Porcentaje_Iva")]
        public decimal PorcentajeIva { get; set; }
        [JsonProperty("Tarifa_Imp_Iva")]
        public decimal TarifaImpIVA { get; set; }
        [JsonProperty("Actualizar_Datos", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ActualizarDatos { get; set; }
        [JsonProperty("Num_Asiento", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int NumAsiento { get; set; }
        public static string ConsultarPasajero(string Cedula, string Nombre)
        {
            using (SqlConnection connection = new SqlConnection(SqlServer.CadenaConexion))
            {
                using (SqlCommand command = new SqlCommand("ConsultarPasajero", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameter = null;

                    parameter = command.Parameters.Add("@Cedula", SqlDbType.VarChar);
                    parameter.Value = Cedula == null ? DBNull.Value.ToString() : Cedula;
                    parameter = command.Parameters.Add("@Nombre", SqlDbType.VarChar);
                    parameter.Value = Nombre;

                    parameter = command.Parameters.Add("@jsonResult", SqlDbType.VarChar, 100000);
                    parameter.Direction = ParameterDirection.Output;

                    parameter = command.Parameters.Add("return_value", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;

                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    command.ExecuteReader();

                    return command.Parameters["@jsonResult"].Value.ToString();
                    //var id = Int32.Parse(command.Parameters["return_value"].Value.ToString());
                }
            }
        }
    }
}
