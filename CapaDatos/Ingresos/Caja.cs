using AccesoDatos;
using CapaDatos.AreasEquipos;
using CapaDatos.Transportistas;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
    public partial class Caja : SqlConnecion, IDisposable
    {
        private long _Id_Equipo;
        private long _Id_Tipo_Caja;
        private Equipo _Equipo;
        private Tipo_caja _Tipo_caja;
        private long _Id_Cooperativa;
        private Cooperativa _Cooperativa;

        #region Public Properties
        public long Id_Caja { get; set; }
        public string Nombre { get; set; }
        public string Serie { get; set; }
        public long Ultima_Factura { get; set; }
        public long Numero_Autorizacion { get; set; }
        public long Numero_Factura_Inicial { get; set; }
        public long Numero_Factura_Final { get; set; }
        public Nullable<DateTime> Fecha_Autorizacion { get; set; }
        public Nullable<DateTime> Fecha_Caducidad { get; set; }
        public bool Impresion_Texto { get; set; }
        public bool Activo { get; set; }
        public long Id_Equipo
        {
            get { return _Id_Equipo; }
            set
            {
                _Id_Equipo = value;
                _Equipo = null;
            }
        }
        public string Licencia { get; set; }
        public long Id_Tipo_Caja
        {
            get { return _Id_Tipo_Caja; }
            set
            {
                _Id_Tipo_Caja = value;
                _Tipo_caja = null;
            }
        }
        public long Numero_Caja { get; set; }
        public int ConsecutivoTasa { get; set; }
        public short TablaTasa { get; set; }
        public Equipo equipo
        {
            get
            {
                if (_Equipo != null && _Equipo.Id_Equipo != 0)
                    return _Equipo;
                else if (_Id_Equipo != 0)
                    return _Equipo = new Equipo(_Id_Equipo);
                else
                    return null;
            }
        }
        public Tipo_caja tipo_caja
        {
            get
            {
                if (_Tipo_caja != null && _Tipo_caja.Id_Tipo_Caja != 0)
                    return _Tipo_caja;
                else if (_Id_Tipo_Caja != 0)
                    return _Tipo_caja = new Tipo_caja(_Id_Tipo_Caja);
                else
                    return null;
            }
        }
        public int Id_TipoFactura { get; set; }
        public int Id_TipoAmbiente { get; set; }
        public string RUC { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string DirMatriz { get; set; }
        public string DirEstablecimiento { get; set; }
        public string CodEstablecimiento { get; set; }
        public string ResolContribEsp { get; set; }
        public bool ObligadoContab { get; set; }
        public long Id_Cooperativa
        {
            get { return _Id_Cooperativa; }
            set
            {
                _Id_Cooperativa = value;
                _Cooperativa = null;
            }
        }
        public Cooperativa Coop
        {
            get
            {
                if (_Cooperativa != null && _Cooperativa.Id_Cooperativa != 0)
                    return _Cooperativa;
                else if (_Id_Cooperativa != 0)
                    return _Cooperativa = new Cooperativa(_Id_Cooperativa);
                else
                    return null;
            }
        }
        public string CertificadoP12 { get; set; }
        public string ClaveCertificadoP12 { get; set; }
        public bool AutoCierre { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public string Usuario_Modificacion { get; set; }
        #endregion

        public Caja()
        {
        }

        /// <summary>
        /// Constructor para llenar la clase con los datos
        /// </summary>
        /// <param name="id"> codigo de la tabla </param>
        public Caja(long id)
        {
            using (DataTable dt = GetAllData(String.Format("Id_Caja = {0}", id)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id_Caja = Convert.ToInt64(dt.Rows[0]["Id_Caja"]);
                    Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                    Serie = Convert.ToString(dt.Rows[0]["Serie"]);
                    Ultima_Factura = Convert.ToInt64(dt.Rows[0]["Ultima_Factura"]);
                    Numero_Autorizacion = Convert.ToInt64(dt.Rows[0]["Numero_Autorizacion"]);
                    Numero_Factura_Inicial = Convert.ToInt64(dt.Rows[0]["Numero_Factura_Inicial"]);
                    Numero_Factura_Final = Convert.ToInt64(dt.Rows[0]["Numero_Factura_Final"]);
                    Fecha_Autorizacion = valorDateTime(dt.Rows[0]["Fecha_Autorizacion"]);
                    Fecha_Caducidad = valorDateTime(dt.Rows[0]["Fecha_Caducidad"]);
                    Impresion_Texto = Convert.ToBoolean(dt.Rows[0]["Impresion_Texto"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    _Id_Equipo = Convert.ToInt64(dt.Rows[0]["Id_Equipo"]);
                    Licencia = Convert.ToString(dt.Rows[0]["Licencia"]);
                    _Id_Tipo_Caja = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Caja"]);
                    Numero_Caja = Convert.ToInt64(dt.Rows[0]["Numero_Caja"]);
                    ConsecutivoTasa = Convert.ToInt32(dt.Rows[0]["ConsecutivoTasa"]);
                    TablaTasa = Convert.ToInt16(dt.Rows[0]["TablaTasa"]);
                    RUC = Convert.ToString(dt.Rows[0]["RUC"]);
                    RazonSocial = Convert.ToString(dt.Rows[0]["RazonSocial"]);
                    NombreComercial = Convert.ToString(dt.Rows[0]["NombreComercial"]);
                    DirMatriz = Convert.ToString(dt.Rows[0]["DirMatriz"]);
                    DirEstablecimiento = Convert.ToString(dt.Rows[0]["DirEstablecimiento"]);
                    CodEstablecimiento = Convert.ToString(dt.Rows[0]["CodEstablecimiento"]);
                    ResolContribEsp = Convert.ToString(dt.Rows[0]["ResolContribEsp"]);
                    ObligadoContab = Convert.ToBoolean(dt.Rows[0]["ObligadoContab"]);
                    Id_TipoFactura = Convert.ToInt32(dt.Rows[0]["Id_TipoFactura"]);
                    Id_TipoAmbiente = Convert.ToInt32(dt.Rows[0]["Id_TipoAmbiente"]);
                    _Id_Cooperativa = dt.Rows[0]["Id_Cooperativa"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id_Cooperativa"]) : 0;
                    CertificadoP12 = Convert.ToString(dt.Rows[0]["CertificadoP12"]);
                    ClaveCertificadoP12 = Convert.ToString(dt.Rows[0]["ClaveCertificadoP12"]);
                    AutoCierre = Convert.ToBoolean(dt.Rows[0]["AutoCierre"]);
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
            return String.Format("SELECT Id_Caja, Nombre, Serie, Ultima_Factura, Numero_Autorizacion, Numero_Factura_Inicial, Numero_Factura_Final, " +
                "Fecha_Autorizacion, Fecha_Caducidad, Certificado, Id_Equipo, Licencia, Id_Tipo_Caja, Numero_Caja, ConsecutivoTasa, TablaTasa, " +
                "Impresion_Texto, Activo, RUC, RazonSocial, NombreComercial, DirMatriz, DirEstablecimiento, CodEstablecimiento, ResolContribEsp, " +
                "ObligadoContab, Id_TipoFactura, Id_TipoAmbiente, Id_Cooperativa, CertificadoP12, ClaveCertificadoP12, AutoCierre, Fecha_Creacion, " +
                "Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM CAJA");
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

        public static Caja GetCaja(long id)
        {
            return new Caja(id);
        }

        static public long Next_Codigo()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("Id_Caja", "CAJA", "");
            return Convert.ToInt64(SqlServer.EXEC_SCALAR(sql));
        }

        static public short Next_TablaTasa()
        {
            string sql = SqlServer.GetFormatoSQLNEXT("TablaTasa", "CAJA", "");
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(sql));
        }

        public string Insert()
        {
            if (TablaTasa == 0)
                TablaTasa = Next_TablaTasa();
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }

        public string GetSQLInsert()
        {
            return String.Format("INSERT INTO CAJA (Nombre, Serie, Ultima_Factura, Numero_Autorizacion, Numero_Factura_Inicial, Numero_Factura_Final, " +
                "Fecha_Autorizacion, Fecha_Caducidad, Certificado, Id_Equipo, Licencia, Id_Tipo_Caja, Numero_Caja, ConsecutivoTasa, Impresion_Texto, " +
                "Activo, RUC, RazonSocial, NombreComercial, DirMatriz, DirEstablecimiento, CodEstablecimiento, ResolContribEsp, ObligadoContab, " +
                "Id_TipoFactura, Id_TipoAmbiente, Id_Cooperativa, TablaTasa, CertificadoP12, ClaveCertificadoP12, AutoCierre, Fecha_Creacion, " +
                "Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, {7}, '{8}', '{9}', " +
                "'{10}', '{11}', '{12}', '{13}', '{14}', {15}, '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', {24}, {25}, {26}, {27}, " +
                "'{28}', '{29}', {30}, GETDATE(), {31}, {32}, {33}); ", 
                SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Serie), Ultima_Factura, Numero_Autorizacion, Numero_Factura_Inicial, Numero_Factura_Final, 
                (Fecha_Autorizacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Autorizacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                (Fecha_Caducidad == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Caducidad.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                null, Id_Equipo, SqlServer.ValidarTexto(Licencia), Id_Tipo_Caja, Numero_Caja, ConsecutivoTasa, Impresion_Texto, Activo ? 1 : 0, SqlServer.ValidarTexto(RUC), 
                SqlServer.ValidarTexto(RazonSocial), SqlServer.ValidarTexto(NombreComercial), SqlServer.ValidarTexto(DirMatriz), SqlServer.ValidarTexto(DirEstablecimiento), 
                SqlServer.ValidarTexto(CodEstablecimiento), SqlServer.ValidarTexto(ResolContribEsp), ObligadoContab, Id_TipoFactura == 0 ? "NULL" : Id_TipoFactura.ToString(), 
                Id_TipoAmbiente == 0 ? "NULL" : Id_TipoAmbiente.ToString(), Id_Cooperativa == 0 ? "NULL" : Id_Cooperativa.ToString(), TablaTasa, SqlServer.ValidarTexto(CertificadoP12),
                SqlServer.ValidarTexto(ClaveCertificadoP12), AutoCierre ? 1 : 0, Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
                (Fecha_Modificacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Modificacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public string quitarTiposPago(long id)
        {
            string _return = SqlServer.EXEC_COMMAND(string.Format("DELETE FROM CAJA_TIPOPAGO WHERE Id_Caja = {0}", id));
            if (_return == "OK")
                _return = SqlServer.MensajeDeEliminar;
            return _return;
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
            return String.Format("DELETE FROM CAJA WHERE Id_Caja = {0};", ID);

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
            return String.Format("UPDATE CAJA SET Nombre = '{1}', Serie = '{2}', Ultima_Factura = '{3}', Numero_Autorizacion = '{4}', " +
                "Numero_Factura_Inicial = '{5}', Numero_Factura_Final = '{6}', Fecha_Autorizacion = {7}, Fecha_Caducidad = {8}, " +
                "Certificado = {9}, Id_Equipo = '{10}', Licencia = '{11}', Id_Tipo_Caja = '{12}', Numero_Caja = '{13}', ConsecutivoTasa = '{14}', " +
                "Impresion_Texto = '{15}', Activo = {16}, RUC = '{17}', RazonSocial = '{18}', NombreComercial = '{19}', DirMatriz = '{20}', " +
                "DirEstablecimiento = '{21}', CodEstablecimiento = '{22}', ResolContribEsp = '{23}', ObligadoContab = '{24}', " +
                "Id_TipoFactura = {25}, Id_TipoAmbiente = {26}, Id_Cooperativa = {27}, TablaTasa = {28}, CertificadoP12 = '{29}', " +
                "ClaveCertificadoP12 = '{30}', AutoCierre = {31}, Fecha_Creacion = {32}, Usuario_Creacion = {33}, Fecha_Modificacion = GETDATE(), " +
                "Usuario_Modificacion = {34} WHERE Id_Caja = {0};", 
                Id_Caja, SqlServer.ValidarTexto(Nombre), SqlServer.ValidarTexto(Serie), Ultima_Factura, Numero_Autorizacion, 
                Numero_Factura_Inicial, Numero_Factura_Final, 
                (Fecha_Autorizacion == null) ? "null" : "'" + Convert.ToDateTime(Fecha_Autorizacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                (Fecha_Caducidad == null) ? "null" : "'" + Convert.ToDateTime(Fecha_Caducidad.ToString()).ToString(SqlServer.FormatofechaHora) + "'", 
                "NULL", Id_Equipo, SqlServer.ValidarTexto(Licencia), Id_Tipo_Caja, Numero_Caja, ConsecutivoTasa, Impresion_Texto, Activo ? 1 : 0, 
                SqlServer.ValidarTexto(RUC), SqlServer.ValidarTexto(RazonSocial), SqlServer.ValidarTexto(NombreComercial),
                SqlServer.ValidarTexto(DirMatriz), SqlServer.ValidarTexto(DirEstablecimiento), SqlServer.ValidarTexto(CodEstablecimiento), 
                SqlServer.ValidarTexto(ResolContribEsp), ObligadoContab, Id_TipoFactura == 0 ? "NULL" : Id_TipoFactura.ToString(), 
                Id_TipoAmbiente == 0 ? "NULL" : Id_TipoAmbiente.ToString(), Id_Cooperativa == 0 ? "NULL" : Id_Cooperativa.ToString(), 
                TablaTasa, SqlServer.ValidarTexto(CertificadoP12), SqlServer.ValidarTexto(ClaveCertificadoP12), AutoCierre ? 1 : 0,
                (Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
                Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
        }

        public string SQLUpdateUltimaFactura(long idCaja, long NumFactura)
        {
            return string.Format("UPDATE CAJA SET Ultima_Factura = {1} WHERE Id_Caja = {0} ;", idCaja, NumFactura);
        }

        public bool IsDuplicate_NumeroCaja(long Num, string Ruc)
        {
            return IsDuplicate_NumeroCaja(0, Num, Ruc);
        }

        public bool IsDuplicate_NumeroCaja(long id, long Num, string Ruc)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Caja) FROM CAJA " +
                "WHERE Activo = 1 AND Numero_Caja = '{0}' AND RUC = '{1}' {2}", Num, Ruc, id != 0 ? "AND Id_Caja <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Serie(string Ser, string Ruc)
        {
            return IsDuplicate_Serie(0, Ser, Ruc);
        }

        public bool IsDuplicate_Serie(long id, string Ser, string Ruc)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Caja) FROM CAJA " +
                "WHERE Activo = 1 AND Serie = '{0}' AND RUC = '{1}' {2}", Ser, Ruc, id != 0 ? "AND Id_Caja <> " + id.ToString() : ""))) > 0;
        }

        public bool IsDuplicate_Serie(long id, string Ser, string Ruc, string Est)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Caja) FROM CAJA " +
                "WHERE Activo = 1 AND Serie = '{0}' AND RUC = '{1}' AND CodEstablecimiento = '{2}' {3}",
                Ser, Ruc, Est, id != 0 ? "AND Id_Caja <> " + id.ToString() : ""))) > 0;
        }
        
        public bool IsDuplicate_Nombre(string Nom)
        {
            return IsDuplicate_Nombre(0, Nom);
        }

        public bool IsDuplicate_Nombre(long id, string Nom)
        {
            return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Caja) FROM CAJA " +
                "WHERE Activo = 1 AND Nombre = '{0}' {1}", Nom, id != 0 ? "AND Id_Caja <> " + id.ToString() : ""))) > 0;
        }

        public int ContarCajasActivas()
        {
            return ContarCajasActivas(0);
        }

        public int ContarCajasActivas(long id)
        {
            try 
            { 
                return Convert.ToInt32(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(c.Id_Caja) FROM CAJA c " +
                    "WHERE c.Activo = 1 {0}", id != 0 ? "AND c.Id_Caja <> " + id.ToString() : "")));
            }
            catch
            {
                return 0;
            }
        }

    public int ContarCajasActivas(string Cod)
        {
            return ContarCajasActivas(0, Cod);
        }

        public int ContarCajasActivas(long id, string Cod)
        {
            try
            {
                return Convert.ToInt32(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(c.Id_Caja) FROM CAJA c " +
                    "INNER JOIN TIPO_CAJA tc ON tc.Id_Tipo_Caja = c.Id_Tipo_Caja WHERE c.Activo = 1 AND tc.Codigo = '{0}' {1}", 
                    Cod, id != 0 ? "AND c.Id_Caja <> " + id.ToString() : "")));
            }
            catch
            {
                return 0;
            }
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
        ~Caja()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
