using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.UsuariosPerfiles
{
    public partial class Licencia : SqlConnecion, IDisposable
    {
        private int _Cajas_FR;
        private int _Cajas_TU;
        private int _Cajas_PP;
        private int _Cajas_BT;
        private int _Cajas_ADM;

        #region Public Properties
        public long Id_Licencia { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public int Cajas_FR
        {
            get { return _Cajas_FR; }
            set { _Cajas_FR = value; }
        }
        public int Cajas_TU
        {
            get { return _Cajas_TU; }
            set { _Cajas_TU = value; }
        }
        public int Cajas_PP
        {
            get { return _Cajas_PP; }
            set { _Cajas_PP = value; }
        }
        public int Cajas_BT
        {
            get { return _Cajas_BT; }
            set { _Cajas_BT = value; }
        }
        public int Cajas_ADM
        {
            get { return _Cajas_ADM; }
            set { _Cajas_ADM = value; }
        }
        public DateTime? Fecha_Desde { get; set; }
        public DateTime? Fecha_Hasta { get; set; }
        #endregion

        public Licencia()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Licencia(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Licencia = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Licencia = Convert.ToInt64(dt.Rows[0]["Id_Licencia"]);
                    Nombre = Codificacion.DesencriptarCadena(Convert.ToString(dt.Rows[0]["Nombre"]));
                    Ubicacion = Codificacion.DesencriptarCadena(Convert.ToString(dt.Rows[0]["Ubicacion"]));
                    if (!int.TryParse(Codificacion.DesencriptarCadena(Convert.ToString(dt.Rows[0]["Cajas_FR"])), out _Cajas_FR))
                        _Cajas_FR = -1;
                    if (!int.TryParse(Codificacion.DesencriptarCadena(Convert.ToString(dt.Rows[0]["Cajas_TU"])), out _Cajas_TU))
                        _Cajas_TU = -1;
                    if (!int.TryParse(Codificacion.DesencriptarCadena(Convert.ToString(dt.Rows[0]["Cajas_PP"])), out _Cajas_PP))
                        _Cajas_PP = -1;
                    if (!int.TryParse(Codificacion.DesencriptarCadena(Convert.ToString(dt.Rows[0]["Cajas_BT"])), out _Cajas_BT))
                        _Cajas_BT = -1;
                    if (!int.TryParse(Codificacion.DesencriptarCadena(Convert.ToString(dt.Rows[0]["Cajas_ADM"])), out _Cajas_ADM))
                        _Cajas_ADM = -1;
                    DateTime fTemp;
                    if (DateTime.TryParse(Codificacion.DesencriptarCadena(Convert.ToString(dt.Rows[0]["Fecha_Desde"])), out fTemp))
                        Fecha_Desde = fTemp;
                    else
                        Fecha_Desde = null;
                    if (DateTime.TryParse(Codificacion.DesencriptarCadena(Convert.ToString(dt.Rows[0]["Fecha_Hasta"])), out fTemp))
                        Fecha_Hasta = fTemp;
                    else
                        Fecha_Hasta = null;
                }
            }
        }

        private static string GetSqlSelect()
        {
            return String.Format("SELECT Id_Licencia, Nombre, Ubicacion, Cajas_FR, Cajas_TU, Cajas_PP, Cajas_BT, Cajas_ADM, Fecha_Desde, Fecha_Hasta FROM Licencia");
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

        public static Modulo GetModulo(long id)
        {
            return new Modulo(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Licencia", "Licencia", "");
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
            return String.Format("INSERT INTO LICENCIA ( Nombre, Ubicacion, Cajas_FR, Cajas_TU, Cajas_PP, Cajas_BT, Cajas_ADM, Fecha_Desde, Fecha_Hasta ) " +
                " VALUES( '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}'); ", Codificacion.EncriptarCadena(Nombre), Codificacion.EncriptarCadena(Ubicacion),
                Codificacion.EncriptarCadena(Cajas_FR == -1 ? "Ilimitadas" : Cajas_FR.ToString()), 
                Codificacion.EncriptarCadena(Cajas_TU == -1 ? "Ilimitadas" : Cajas_TU.ToString()),
                Codificacion.EncriptarCadena(Cajas_PP == -1 ? "Ilimitadas" : Cajas_PP.ToString()), 
                Codificacion.EncriptarCadena(Cajas_BT == -1 ? "Ilimitadas" : Cajas_BT.ToString()),
                Codificacion.EncriptarCadena(Cajas_ADM == -1 ? "Ilimitadas" : Cajas_ADM.ToString()),
                Codificacion.EncriptarCadena(Fecha_Desde == null ? "Indefinida" : ((DateTime)Fecha_Desde).ToString("dd MMMM yyyy")),
                Codificacion.EncriptarCadena(Fecha_Hasta == null ? "Indefinida" : ((DateTime)Fecha_Hasta).ToString("dd MMMM yyyy")));
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
            return String.Format("DELETE FROM LICENCIA WHERE Id_Licencia = {0};", ID);
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
            return String.Format("UPDATE LICENCIA SET Nombre= '{1}', Ubicacion= '{2}', Cajas_FR= '{3}', Cajas_TU= '{4}', Cajas_PP= '{5}', " +
                "Cajas_BT= '{6}', Cajas_ADM= '{7}', Fecha_Desde= '{8}', Fecha_Hasta= '{9}' WHERE Id_Licencia = {0};", Id_Licencia, 
                Codificacion.EncriptarCadena(Nombre), Codificacion.EncriptarCadena(Ubicacion),
                Codificacion.EncriptarCadena(Cajas_FR == -1 ? "Ilimitadas" : Cajas_FR.ToString()), 
                Codificacion.EncriptarCadena(Cajas_TU == -1 ? "Ilimitadas" : Cajas_TU.ToString()),
                Codificacion.EncriptarCadena(Cajas_PP == -1 ? "Ilimitadas" : Cajas_PP.ToString()), 
                Codificacion.EncriptarCadena(Cajas_BT == -1 ? "Ilimitadas" : Cajas_BT.ToString()), 
                Codificacion.EncriptarCadena(Cajas_ADM == -1 ? "Ilimitadas" : Cajas_ADM.ToString()),
                Codificacion.EncriptarCadena(Fecha_Desde == null ? "Indefinida" : ((DateTime)Fecha_Desde).ToString("dd MMMM yyyy")),
                Codificacion.EncriptarCadena(Fecha_Hasta == null ? "Indefinida" : ((DateTime)Fecha_Hasta).ToString("dd MMMM yyyy")));
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
        ~Licencia()
        {
            this.Dispose(false);
        }
        #endregion
    }

    public partial class LicenciaGlobal
    {
        private static int _Cajas_FR = 0;
        private static int _Cajas_TU = 0;
        private static int _Cajas_PP = 0;
        private static int _Cajas_BT = 0;
        private static int _Cajas_ADM = 0;
        private static DateTime? _Fecha_Desde = null;
        private static DateTime? _Fecha_Hasta = null;
        private static bool _EspVal = false;
        private static bool _Publicidad = false;
        private static bool _Leido = false;

        public static int Cajas_FR
        {
            get
            {
                if (!_Leido)
                    obtenerLicencias();
                return _Cajas_FR;
            }
        }

        public static int Cajas_TU
        {
            get
            {
                if (!_Leido)
                    obtenerLicencias();
                return _Cajas_TU;
            }
        }

        public static int Cajas_PP
        {
            get
            {
                if (!_Leido)
                    obtenerLicencias();
                return _Cajas_PP;
            }
        }

        public static int Cajas_BT
        {
            get
            {
                if (!_Leido)
                    obtenerLicencias();
                return _Cajas_BT;
            }
        }

        public static int Cajas_ADM
        {
            get
            {
                if (!_Leido)
                    obtenerLicencias();
                return _Cajas_ADM;
            }
        }

        public static bool EspVal
        {
            get
            {
                if (!_Leido)
                    obtenerLicencias();
                return _EspVal;
            }
        }

        public static bool Publicidad
        {
            get
            {
                if (!_Leido)
                    obtenerLicencias();
                return _Publicidad;
            }
        }

        public static DateTime? Fecha_Desde
        {
            get
            {
                if (!_Leido)
                    obtenerLicencias();
                return _Fecha_Desde;
            }
        }

        public static DateTime? Fecha_Hasta
        {
            get
            {
                if (!_Leido)
                    obtenerLicencias();
                return _Fecha_Hasta;
            }
        }

        public static bool Leido
        {
            get { return _Leido; }
        }

        public static void obtenerLicencias()
        {
            if (!_Leido)
            {
                int iTemp = 0; DateTime iFecha; string sStr = "";

                DataTable dtLic = SqlServer.EXEC_SELECT("SELECT Id_Licencia, Nombre, Ubicacion, Cajas_FR, Cajas_TU, Cajas_PP, Cajas_BT, Cajas_ADM, Fecha_Desde, Fecha_Hasta, EspVal, Publicidad FROM Licencia");
                foreach (DataRow drFila in dtLic.Rows)
                {
                    sStr = Codificacion.DesencriptarCadena(drFila["Fecha_Desde"].ToString());
                    if (sStr.Equals("Indefinida", StringComparison.OrdinalIgnoreCase))
                        _Fecha_Desde = new DateTime(1900, 1, 1);
                    else
                        _Fecha_Desde = Convert.ToDateTime(sStr);
                    sStr = Codificacion.DesencriptarCadena(drFila["Fecha_Hasta"].ToString());
                    if (sStr.Equals("Indefinida", StringComparison.OrdinalIgnoreCase))
                        _Fecha_Hasta = new DateTime(9999, 12, 31, 23, 59, 59);
                    else
                        _Fecha_Hasta = Convert.ToDateTime(sStr);

                    if (_Fecha_Desde < DateTime.Today && _Fecha_Hasta > DateTime.Today)
                    {
                        try
                        {
                            sStr = Codificacion.DesencriptarCadena(drFila["Cajas_FR"].ToString());
                            if (sStr.StartsWith("*FR*") && sStr.EndsWith("*FR*"))
                                if (sStr.Contains("Ilimitadas"))
                                    _Cajas_FR = -1;
                                else if (int.TryParse(sStr.Substring(4, sStr.Length - 8), out iTemp))
                                {
                                    if (_Cajas_FR != -1)
                                        _Cajas_FR += iTemp;
                                }
                        }
                        catch { }

                        try
                        {
                            sStr = Codificacion.DesencriptarCadena(drFila["Cajas_TU"].ToString());
                            if (sStr.StartsWith("*TU*") && sStr.EndsWith("*TU*"))
                                if (sStr.Contains("Ilimitadas"))
                                    _Cajas_TU = -1;
                                else if (int.TryParse(sStr.Substring(4, sStr.Length - 8), out iTemp))
                                {
                                    if (_Cajas_TU != -1)
                                        _Cajas_TU += iTemp;
                                }
                        }
                        catch { }

                        try
                        {
                            sStr = Codificacion.DesencriptarCadena(drFila["Cajas_PP"].ToString());
                            if (sStr.StartsWith("*PP*") && sStr.EndsWith("*PP*"))
                                if (sStr.Contains("Ilimitadas"))
                                    _Cajas_PP = -1;
                                else if (int.TryParse(sStr.Substring(4, sStr.Length - 8), out iTemp))
                                {
                                    if (_Cajas_PP != -1)
                                        _Cajas_PP += iTemp;
                                }
                        }
                        catch { }

                        try
                        {
                            sStr = Codificacion.DesencriptarCadena(drFila["Cajas_BT"].ToString());
                            if (sStr.StartsWith("*BT*") && sStr.EndsWith("*BT*"))
                                if (sStr.Contains("Ilimitadas"))
                                    _Cajas_BT = -1;
                                else if (int.TryParse(sStr.Substring(4, sStr.Length - 8), out iTemp))
                                {
                                    if (_Cajas_BT != -1)
                                        _Cajas_BT += iTemp;
                                }
                        }
                        catch { }

                        try
                        {
                            sStr = Codificacion.DesencriptarCadena(drFila["Cajas_ADM"].ToString());
                            if (sStr.StartsWith("*ADM*") && sStr.EndsWith("*ADM*"))
                                if (sStr.Contains("Ilimitadas"))
                                    _Cajas_ADM = -1;
                                else if (int.TryParse(sStr.Substring(5, sStr.Length - 10), out iTemp))
                                {
                                    if (_Cajas_ADM != -1)
                                        _Cajas_ADM += iTemp;
                                }
                        }
                        catch { }

                        _EspVal |= Codificacion.DesencriptarCadena(drFila["EspVal"].ToString()) == "*EV*Sí*EV*";
                        _Publicidad |= Codificacion.DesencriptarCadena(drFila["Publicidad"].ToString()) == "*AD*Sí*AD*";
                    }
                }
                _Leido = true;
            }
        }

        public static void LimpiarMemoriaSesion()
        {
            _Leido = false;
        }
    }
}
