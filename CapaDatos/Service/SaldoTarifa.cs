using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CapaDatos.Service
{
    [DataContract]
    public class SaldoTarifa
    {
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public decimal Valor { get; set; }
    }
}
