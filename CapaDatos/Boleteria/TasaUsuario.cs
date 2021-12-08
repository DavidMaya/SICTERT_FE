using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CapaDatos.Boleteria
{
    public class TasaUsuario
    {
        #region Public Properties
        public long IdTipoTarifa { get; set; }
        public long IdConceptoCuenta { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public bool Iva { get; set; }
        public decimal ImpIVA { get; set; }
        public List<string> Dias { get; set; }
        public bool TerceraEdad { get; set; }
        public bool Discapacitado { get; set; }
        public bool MenorEdad { get; set; }
        #endregion

        #region Constructor
        public TasaUsuario()
        {

        }
        #endregion

        #region Funciones
        private static List<TasaUsuario> List()
        {
            List<TasaUsuario> tasasUsuario = new List<TasaUsuario>();
            string sql = String.Format(@"SELECT tt.Id_tipo_tarifa, tt.Id_concepto_cuenta, cc.Nombre, cc.Valor, cc.Iva, tt.Dias, tt.Tercera_Edad, tt.Discapacitado,
                tt.Menor_Edad, ImpIva = (SELECT Valor FROM CONFIGURACION_GLOBAL WHERE Configuracion = 'IVA')
                FROM TIPO_TARIFA_TU tt 
                INNER JOIN CONCEPTO_CUENTA cc 
                ON cc.Id_Concepto_Cuenta = tt.Id_concepto_cuenta
                WHERE tt.Activo = 1");

            using (DataTable table = SqlServer.EXEC_SELECT(sql))
            {
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        tasasUsuario.Add(new TasaUsuario
                        {
                            IdTipoTarifa = Convert.ToInt64(row["Id_tipo_tarifa"]),
                            IdConceptoCuenta = Convert.ToInt64(row["Id_concepto_cuenta"]),
                            Nombre = Convert.ToString(row["Nombre"]),
                            Valor = Convert.ToDecimal(row["Valor"]),
                            Iva = Convert.ToBoolean(row["Iva"]),
                            Dias = GetArrayString(Convert.ToString(row["Dias"])),
                            ImpIVA = Convert.ToDecimal(row["ImpIva"]),
                            TerceraEdad = Convert.ToBoolean(row["Tercera_Edad"]),
                            Discapacitado = Convert.ToBoolean(row["Discapacitado"]),
                            MenorEdad = Convert.ToBoolean(row["Menor_Edad"])
                        });
                    }
                }
            }

            return tasasUsuario;
        }

        public static List<TasaUsuario> GetDataObject(DateTime fecha)
        {
            //Filtrar por día de la semana que corresponda
            int dia = (int)Convert.ToDateTime(fecha).DayOfWeek;
            return List().Where(r => r.Dias[dia] == "1").ToList();
        }

        private static List<string> GetArrayString(string texto)
        {
            return texto.Select(c => c.ToString()).ToList();
        }
        #endregion
    }
}
