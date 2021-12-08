using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AccesoDatos;

namespace CapaDatos.Service
{
    public class PersonaDinardap
    {
        private static int _LimiteMenorEdad;
        private static int _LimiteTerceraEdad;

        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool MenorEdad
        {
            get 
            {
                return DateTime.Today.AddTicks(-FechaNacimiento.Ticks).Year - 1 <= LimiteMenorEdad;
            }
        }
        public bool TerceraEdad 
        {
            get
            {
                return DateTime.Today.AddTicks(-FechaNacimiento.Ticks).Year - 1 >= LimiteTerceraEdad;
            }
        }
        public bool Discapacidad 
        { 
            get { return false; } 
        }
        public static int LimiteMenorEdad
        {
            get 
            { 
                if (_LimiteMenorEdad == 0)
                { 
                    // obtener el dato de la configuración
                    try
                    {
                        _LimiteMenorEdad = Convert.ToInt32(SqlServer.EXEC_SCALAR("SELECT Valor FROM CONFIGURACION_GLOBAL WHERE Configuracion = 'LimMenorEdad'"));
                    }
                    catch { }
                }
                return _LimiteMenorEdad;
            }
        }
        public static int LimiteTerceraEdad
        {
            get
            {
                if (_LimiteTerceraEdad == 0)
                {
                    // obtener el dato de la configuración
                    try
                    {
                        _LimiteTerceraEdad = Convert.ToInt32(SqlServer.EXEC_SCALAR("SELECT Valor FROM CONFIGURACION_GLOBAL WHERE Configuracion = 'LimMayorEdad'"));
                    }
                    catch { }
                }
                return _LimiteTerceraEdad;
            }
        }

        public static PersonaDinardap ParseFromJson(string json)
        {
            PersonaDinardap pd = null;

            JObject jo = JObject.Parse(json);
            if (jo.HasValues && ((JProperty)jo.First).Name == "DatosTramite")
            {
                JToken ic = jo.SelectToken("DatosTramite.InformacionCivil");
                if (ic != null && ic.HasValues)
                {
                    pd = new PersonaDinardap();
                    foreach (JToken d in ((JArray)ic))
                    {
                        switch (d.Value<string>("NombreCampo"))
                        {
                            case "CEDULA":
                                pd.Cedula = d.Value<string>("Valor");
                                break;
                            case "NOMBRE":
                                pd.Nombre = d.Value<string>("Valor");
                                break;
                            case "FECHANACIMIENTO":
                                DateTime fn;
                                if (DateTime.TryParseExact(d.Value<string>("Valor"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                                    pd.FechaNacimiento = fn;
                                break;
                        }
                    }
                }
            }
            return pd;
        }
    }
}
