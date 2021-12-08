using AccesoDatos;
using System;
using System.Data;

namespace CapaDatos.Ingresos
{
	public  partial class Concepto_TipoPago : SqlConnecion, IDisposable
    {
        #region Public Properties		
        public long Id_Concepto_TipoPago { get; set; }

        public long Id_Tipo_Pago { get; set; }

        public long Id_Concepto_Cuenta { get; set; }
		public bool Activo { get; set; }
        #endregion

        public Concepto_TipoPago()
		{
		}
		
		/// <summary>
		/// Constructor para llenar la clase con los datos
		/// </summary>
		/// <param name="id"> codigo de la tabla </param>
		public Concepto_TipoPago(long id)
		{
			using (DataTable dt = GetAllData(String.Format("Id_Concepto_TipoPago = {0}", id)))
			{	
				if(dt.Rows.Count>0)
				{            
					Id_Concepto_TipoPago = Convert.ToInt64( dt.Rows[0]["Id_Concepto_TipoPago"]);
					Id_Tipo_Pago = Convert.ToInt64( dt.Rows[0]["Id_Tipo_Pago"]);
					Id_Concepto_Cuenta = Convert.ToInt64( dt.Rows[0]["Id_Concepto_Cuenta"]);
					Activo = true;
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
		
		private static string GetSqlSelect(){
			return String.Format("SELECT Id_Concepto_TipoPago, Id_Tipo_Pago, Id_Concepto_Cuenta FROM CONCEPTO_TIPOPAGO");
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

		public static Caja_tipopago GetCaja_tipopago(long id)
		{
			return new Caja_tipopago(id);
		}
		
		static public long Next_Codigo ()
		{
			string sql=	SqlServer.GetFormatoSQLNEXT("Id_Concepto_TipoPago", "CONCEPTO_TIPOPAGO", "" );		             
			return Convert.ToInt32(SqlServer.EXEC_SCALAR(sql));
		}        
		public string Insert()
		{
			string _return = SqlServer.EXEC_COMMAND(GetSQLInsert());
			if (_return  == "OK")
				_return = SqlServer.MensajeDeGuardar;            
			return _return;
		}
	  
		public string GetSQLInsert()
		{
			return  String.Format("INSERT INTO CONCEPTO_TIPOPAGO (Id_Tipo_Pago, Id_Concepto_Cuenta) VALUES( '{0}', '{1}'); ", 
				Id_Tipo_Pago, Id_Concepto_Cuenta);
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
			return String.Format("DELETE FROM CONCEPTO_TIPOPAGO WHERE Id_Concepto_TipoPago = {0};", ID);
		}
		
		public string Update()
		{  
			string _return = SqlServer.EXEC_COMMAND(GetSQLUpdate()) ;
			if (_return == "OK")
				_return = SqlServer.MensajeDeActualizar;           
			return _return;

		}
		public string GetSQLUpdate()
		{
			return String.Format("UPDATE CONCEPTO_TIPOPAGO SET Id_Tipo_Pago= '{1}', Id_Concepto_Cuenta= '{2}' WHERE Id_Concepto_TipoPago = {0};", 
				Id_Concepto_TipoPago, Id_Tipo_Pago, Id_Concepto_Cuenta);
		}

        #region Metodo Dispose

        /// <summary>
        /// Implementaci??n de IDisposable. No se sobreescribe.
        /// </summary>
        /// 
        private Boolean disposed;
		public void Dispose() {
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
		protected virtual void Dispose(bool disposing) {
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
		~Concepto_TipoPago ()
		{
		   this.Dispose(false);
		}
		#endregion
	}
}
