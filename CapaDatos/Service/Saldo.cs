using AccesoDatos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CapaDatos.Service
{
    [DataContract]
    public class Saldo
    {
        public long IdCuentaPrepago { get; set; }
        public long IdCooperativa { get; set; }
        public long IdConceptoCuenta { get; set; }
        public long IdTipoTarifa { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public decimal Valor { get; set; }

        public Saldo()
        {

        }

        public static List<Saldo> GetAll(string Cooperativa, string Clave)
        {
            string sql = String.Format(@"SELECT cpc.Id_CuentaPrepago, cc.Id_Cooperativa, ttt.Id_concepto_cuenta, spc.Id_Tipo_Tarifa, ttt.Nombre, spc.Saldo
                FROM CUENTA_PREPAGO_COOP cpc INNER JOIN COOPERATIVA cc ON cpc.Id_Cooperativa = cc.Id_Cooperativa INNER JOIN SALDO_PREPAGO_COOP spc
                ON cpc.Id_CuentaPrepago = spc.Id_CuentaPrepago INNER JOIN TIPO_TARIFA_TU ttt ON spc.Id_Tipo_Tarifa = ttt.Id_tipo_tarifa
                WHERE cc.Ruc = '{0}' AND cpc.Clave = '{1}' AND cpc.Activo = 1 AND spc.Saldo > 0",
                Cooperativa, Clave);

            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                if (table.Rows.Count > 0)
                {
                    List<Saldo> saldos = new List<Saldo>();

                    foreach (DataRow cuenta in table.Rows)
                    {
                        saldos.Add(new Saldo()
                        {
                            IdCuentaPrepago = Convert.ToInt64(cuenta["Id_CuentaPrepago"]),
                            IdCooperativa = Convert.ToInt64(cuenta["Id_Cooperativa"]),
                            IdConceptoCuenta = Convert.ToInt64(cuenta["Id_concepto_cuenta"]),
                            IdTipoTarifa = Convert.ToInt64(cuenta["Id_Tipo_Tarifa"]),
                            Nombre = Convert.ToString(cuenta["Nombre"]),
                            Valor = Convert.ToDecimal(cuenta["Saldo"])
                        });
                    }

                    return saldos;
                }
                return null;
            }
        }
    }
}
