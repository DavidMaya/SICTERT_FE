using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CapaDatos.Service
{
    public class Result
    {
        public string Operadora { get; set; }
        public long ID { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Clave { get; set; }
        public int Estado { get; set; }
        [DataMember]
        public string Mensaje { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<PasajeroResult> Pasajeros { get; set; }
        [JsonProperty("SaldoTarifas")]
        public List<Saldo> SaldoTarifas { get; set; }

        public Result()
        {
            Mensaje = "";
            Pasajeros = new List<PasajeroResult>();
            SaldoTarifas = new List<Saldo>();
        }
    }
}
