using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using CapaDatos.Parqueo;
using CapaDatos.Frecuencias;
using CapaDatos.Transportistas;
using CapaDatos.Ingresos;

namespace AccesoDatos
{
    public class Bitacora : SqlConnecion, IDisposable
    {
        private long _Id_Bitacora;
        private long _Id_Usuario;
        private long _Id_Modulo;
        private long _Id_Prioridad;
        private string _Descripcion;
        private long _Id_Equipo;  
      
        #region Public Properties
        public long Id_Bitacora
        {
            get { return _Id_Bitacora; }
            set { _Id_Bitacora = value; }
        }

        public long Id_Usuario
        {
            get { return _Id_Usuario; }
            set { _Id_Usuario = value; }
        }

      

        public long Id_Modulo
        {
            get { return _Id_Modulo; }
            set { _Id_Modulo = value; }
        }

        public long Id_Prioridad
        {
            get { return _Id_Prioridad; }
            set { _Id_Prioridad = value; }
        }

        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        public long Id_Equipo
        {
            get { return _Id_Equipo; }
            set { _Id_Equipo = value; }
        }
     
      
      

        #endregion
        public Bitacora()
        {
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
            return String.Format("SELECT Id_Bitacora,Id_Usuario,Fecha_Hora,Id_Modulo,Id_Prioridad,Descripcion,Id_Equipo FROM Bitacora");
        }

        public static string GetSqlSelect_factura(Factura fact, long modulo, long prioridad, long equipo, long usuario, string detalle_aux)
       {
 
            Caja caj = new Caja(fact.Id_Caja);

            string detalle = string.Format("Venta | ID = {5} | Caja = #{4} {3} | Recibo = {1}-{0} | Total = $ {2} | {6}", fact.Numero, fact.Serie, fact.Valor, caj.Nombre, caj.Numero_Caja, fact.Id_Factura, detalle_aux);
            return String.Format("INSERT INTO Bitacora ( Id_Usuario,Fecha_Hora,Id_Modulo,Id_Prioridad,Descripcion,Id_Equipo ) " +
                                      " VALUES( '{0}',GetDate(),{1},'{2}','{3}','{4}'); ", usuario,  modulo, prioridad, detalle, equipo);
        }

        public static string GetSqlSelect_factura_parqueo(Factura_Parqueo fact, long modulo, long prioridad, long equipo, long usuario, string detalle_aux)
        {

            Caja caj = new Caja(fact.Id_Caja);

            string detalle = string.Format("Venta | ID = {5} | Caja = #{4} {3} | Recibo = {1}-{0} | Total = $ {2} | {6}", fact.Numero, fact.Serie, fact.Valor, caj.Nombre, caj.Numero_Caja, fact.Id_Factura_Parqueo, detalle_aux);
            return String.Format("INSERT INTO Bitacora ( Id_Usuario,Fecha_Hora,Id_Modulo,Id_Prioridad,Descripcion,Id_Equipo ) " +
                                      " VALUES( '{0}',GetDate(),{1},'{2}','{3}','{4}'); ", usuario, modulo, prioridad, detalle, equipo);
        }

        public static string GetSqlCuentasPrepago(Recibo_pago cuentaPrepago, long modulo, long prioridad, long equipo, long usuario, Cooperativa coop)
        {

            Caja caj = new Caja(cuentaPrepago.Id_Caja);

            string detalle = string.Format("Abono Cuenta Prepago | Caja #{1} {0} | Valor = {2} | Cooperativa {3}", caj.Nombre ,caj.Numero_Caja, cuentaPrepago.Valor.ToString("{0:C}"), coop.Nombre);
            return String.Format("INSERT INTO Bitacora ( Id_Usuario,Fecha_Hora,Id_Modulo,Id_Prioridad,Descripcion,Id_Equipo ) " +
                                      " VALUES( '{0}',GetDate(),{1},'{2}','{3}','{4}'); ", usuario, modulo, prioridad, detalle, equipo);
        }

        public static string GetSql_reeimprimir( long  id_fact, long modulo, long prioridad, long equipo, long usuario)
        {
            string detalle = string.Format("Reimpresión de Recibo de Frecuencia | ID = {0};", id_fact);
            return String.Format("INSERT INTO Bitacora ( Id_Usuario,Fecha_Hora,Id_Modulo,Id_Prioridad,Descripcion,Id_Equipo ) " +
                                      " VALUES( '{0}',GetDate(),{1},'{2}','{3}','{4}'); ", usuario, modulo, prioridad, detalle, equipo);
  
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
      
        public string Insert()
        {
            string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
            if (_return == "OK")
                _return = SqlServer.MensajeDeGuardar;
            return _return;
        }

        public string GetSQLInsert()
        {
            return String.Format("INSERT INTO Bitacora ( Id_Usuario,Fecha_Hora,Id_Modulo,Id_Prioridad,Descripcion,Id_Equipo ) " +
                " VALUES( '{0}',GETDATE(),{1},'{2}','{3}',{4}); ", Id_Usuario,  Id_Modulo, Id_Prioridad, SqlServer.ValidarTexto(Descripcion), Id_Equipo == 0 ? "NULL" : Id_Equipo.ToString());

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
        ~Bitacora()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
