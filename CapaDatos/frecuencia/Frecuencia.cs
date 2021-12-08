using AccesoDatos;
using CapaDatos.Transportistas;
using System;
using System.Data;

namespace CapaDatos.Frecuencias
{
	public  partial class Frecuencia : SqlConnecion, IDisposable 
	{
		private long _Id_Tipo_Ruta;
		private long _Id_Tipo_Frecuencia;
		private long _Id_Cooperativa;
		private long _Id_Ciudad_Origen;
		private long _Id_Ciudad_Destino;
		private long _Id_Anden;
		private Tipo_Ruta _Tipo_Ruta;
		private Tipo_Frecuencia _Tipo_Frecuencia;
		private Cooperativa _Cooperativa;
		private Ciudad _Ciudad_Origen;
		private Ciudad _Ciudad_Destino;
		private Anden _Anden;

		#region Public Properties		
		public long Id_Frecuencia { get; set; }
        public TimeSpan Hora_Salida { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorIva { get; set; }
        public bool Iva { get; set; }
        public long Id_Tipo_Ruta 
		{
			get { return _Id_Tipo_Ruta; }
			set {
				_Id_Tipo_Ruta = value;
				_Tipo_Ruta = null;
			} 
		}
		public Tipo_Ruta Tipo_Ruta
		{
			get
			{
				if (_Tipo_Ruta != null && _Tipo_Ruta.Id_Tipo_Ruta != 0)
					return _Tipo_Ruta;
				else if (_Id_Tipo_Ruta != 0)
					return _Tipo_Ruta = new Tipo_Ruta(_Id_Tipo_Ruta);
				else
					return null;
			}
		}
		public long Id_Tipo_Frecuencia
		{
			get { return _Id_Tipo_Frecuencia; }
			set
			{
				_Id_Tipo_Frecuencia = value;
				_Tipo_Frecuencia = null;
			}
		}
		public Tipo_Frecuencia tipo_frecuencia
		{
			get
			{
				if (_Tipo_Frecuencia != null && _Tipo_Frecuencia.Id_Tipo_Frecuencia != 0)
					return _Tipo_Frecuencia;
				else if (_Id_Tipo_Frecuencia != 0)
					return _Tipo_Frecuencia = new Tipo_Frecuencia(_Id_Tipo_Frecuencia);
				else
					return null;
			}
		}
		public string Resolucion { get; set; }
        public long Id_Cooperativa
		{
			get { return _Id_Cooperativa; }
			set
			{
				_Id_Cooperativa = value;
				_Cooperativa = null;
			}
		}
		public Cooperativa cooperativa
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
		public long Id_Ciudad_Origen
		{
			get { return _Id_Ciudad_Origen; }
			set
			{
				_Id_Ciudad_Origen = value;
				_Ciudad_Origen = null;
			}
		}
		public Ciudad Ciudad_Origen
		{
			get
			{
				if (_Ciudad_Origen != null && _Ciudad_Origen.Id_Ciudad != 0)
					return _Ciudad_Origen;
				else if (_Id_Ciudad_Origen != 0)
					return _Ciudad_Origen = new Ciudad(_Id_Ciudad_Origen);
				else
					return null;
			}
		}
		public long Id_Ciudad_Destino
		{
			get { return _Id_Ciudad_Destino; }
			set
			{
				_Id_Ciudad_Destino = value;
				_Ciudad_Destino = null;
			}
		}
		public Ciudad Ciudad_Destino
		{
			get
			{
				if (_Ciudad_Destino != null && _Ciudad_Destino.Id_Ciudad != 0)
					return _Ciudad_Destino;
				else if (_Id_Ciudad_Destino != 0)
					return _Ciudad_Destino = new Ciudad(_Id_Ciudad_Destino);
				else
					return null;
			}
		}
		public long Tolerancia_Entrada { get; set; }
        public long Tiempo_Anden { get; set; }
        public long Tolerancia_Salida { get; set; }
        public long Tolerancia_Despues { get; set; }
        public string Dias { get; set; }
        public long Id_Anden
		{
			get { return _Id_Anden; }
			set
			{
				_Id_Anden = value;
				_Anden = null;
			}
		}
		public Anden anden
		{
			get
			{
				if (_Anden != null && _Anden.Id_Anden != 0)
					return _Anden;
				else if (_Id_Anden != 0)
					return _Anden = new Anden(_Id_Anden);
				else
					return null;
			}
		}
		public string CiudadOrigen { get; set; }
        public string CiudadDestino { get; set; }
        public string NumeroAnden { get; set; }
        public decimal ValorParqueo { get; set; }
		public bool ExtraNoProg { get; set; }
        public bool Activo { get; set; }
		public DateTime? Fecha_Creacion { get; set; }
		public string Usuario_Creacion { get; set; }
		public DateTime? Fecha_Modificacion { get; set; }
		public string Usuario_Modificacion { get; set; }
		#endregion

		public Frecuencia()
		{
		}
		
		/// <summary>
		/// Constructor para llenar la clase con los datos
		/// </summary>
		/// <param name="id"> codigo de la tabla </param>
		public Frecuencia(long id)
		{
			using (DataTable dt = GetAllData(String.Format("Id_Frecuencia = {0}", id)))
			 {	
				if(dt != null && dt.Rows.Count > 0)
				{            
					Id_Frecuencia = Convert.ToInt64( dt.Rows[0]["Id_Frecuencia"]);
					Hora_Salida = (TimeSpan)dt.Rows[0]["Hora_Salida"];
					Valor = Convert.ToDecimal( dt.Rows[0]["Valor"]);
					Id_Tipo_Ruta = Convert.ToInt64( dt.Rows[0]["Id_Tipo_Ruta"]);
					Id_Tipo_Frecuencia = Convert.ToInt64(dt.Rows[0]["Id_Tipo_Frecuencia"]);
					Resolucion = Convert.ToString(dt.Rows[0]["Resolucion"]);
					Id_Cooperativa = Convert.ToInt64( dt.Rows[0]["Id_Cooperativa"]);
					Id_Ciudad_Origen = Convert.ToInt64( dt.Rows[0]["Id_Ciudad_Origen"]);
					Id_Ciudad_Destino = Convert.ToInt64( dt.Rows[0]["Id_Ciudad_Destino"]);
					Tolerancia_Entrada = Convert.ToInt64( dt.Rows[0]["Tolerancia_Entrada"]);
					Tiempo_Anden = Convert.ToInt64( dt.Rows[0]["Tiempo_Anden"]);
					Tolerancia_Salida = Convert.ToInt64( dt.Rows[0]["Tolerancia_Salida"]);
					Tolerancia_Despues = Convert.ToInt64(dt.Rows[0]["Tolerancia_Despues"]);
                    Activo = Convert.ToBoolean(dt.Rows[0]["Activo"]);
                    Dias = Convert.ToString(dt.Rows[0]["Dias"]);
                    CiudadDestino = Ciudad.GetCiudad(Id_Ciudad_Destino).Nombre;
                    CiudadOrigen = Ciudad.GetCiudad(Id_Ciudad_Origen).Nombre;
					ExtraNoProg = Convert.ToBoolean(dt.Rows[0]["ExtraNoProg"]);
					Fecha_Creacion = valorDateTime(dt.Rows[0]["Fecha_Creacion"]);
					Usuario_Creacion = dt.Rows[0]["Usuario_Creacion"].ToString();
					Fecha_Modificacion = valorDateTime(dt.Rows[0]["Fecha_Modificacion"]);
					Usuario_Modificacion = dt.Rows[0]["Usuario_Modificacion"].ToString();
					using (Anden a = new Anden(Id_Ciudad_Destino, true))
					{
						this.Id_Anden = a.Id_Anden;
						this.NumeroAnden = a.Numero;
					}
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
			return String.Format("SELECT Id_Frecuencia, Hora_Salida, Valor, Id_Tipo_Ruta, Id_Tipo_Frecuencia, Resolucion, Id_Cooperativa, " +
                "Id_Ciudad_Origen, Id_Ciudad_Destino, Tolerancia_Entrada, Tiempo_Anden, Tolerancia_Salida, Tolerancia_Despues, Dias, ExtraNoProg, " +
				"Activo, Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion FROM FRECUENCIA");
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
		
		public static Frecuencia GetFrecuencia(long id)
		{
			return new Frecuencia(id);
		}
		
		static public long Next_Codigo ()
		{
			string sql=	SqlServer.GetFormatoSQLNEXT("Id_Frecuencia", "Frecuencia","" );
			return Convert.ToInt32(SqlServer.EXEC_SCALAR(sql));
		}        

		public string Insert()
		{
			string  _return =  SqlServer.EXEC_COMMAND(GetSQLInsert()) ;
			if (_return  == "OK")
				_return = SqlServer.MensajeDeGuardar;            
			return _return;
		}
	  
		public string GetSQLInsert()
		{
			return String.Format("INSERT INTO FRECUENCIA (Hora_Salida, Valor, Id_Tipo_Ruta, Id_Tipo_Frecuencia, Resolucion, Id_Cooperativa, " +
				"Id_Ciudad_Origen, Id_Ciudad_Destino, Tolerancia_Entrada, Tiempo_Anden, Tolerancia_Salida, Tolerancia_Despues, Dias, ExtraNoProg, Activo, " +
				"Fecha_Creacion, Usuario_Creacion, Fecha_Modificacion, Usuario_Modificacion) " +
				"VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', GETDATE(), {15}, {16}, {17}); ",
				Hora_Salida, Valor.ToString().Replace(",", SqlServer.SigFloatSql), Id_Tipo_Ruta, Id_Tipo_Frecuencia, SqlServer.ValidarTexto(Resolucion), 
				Id_Cooperativa, Id_Ciudad_Origen, Id_Ciudad_Destino, Tolerancia_Entrada, Tiempo_Anden, Tolerancia_Salida, Tolerancia_Despues, 
				SqlServer.ValidarTexto(Dias), ExtraNoProg, Activo, Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'",
				(Fecha_Modificacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Modificacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
				Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
		}        
	   
		public string Delete(long id)
		{        
			string _return = SqlServer.EXEC_COMMAND(GetSQLDelete(id)) ;
			if (_return == "OK")
				_return = SqlServer.MensajeDeEliminar;            
			return _return;
		}  
		
		public string GetSQLDelete(long ID)
		{
			return String.Format("DELETE FROM FRECUENCIA WHERE Id_Frecuencia = {0};",ID);
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
			return String.Format("UPDATE FRECUENCIA SET Hora_Salida = '{1}', Valor = '{2}', Id_Tipo_Ruta = '{3}', Resolucion = '{4}', " +
				"Id_Cooperativa = '{5}', Id_Ciudad_Origen = '{6}', Id_Ciudad_Destino = '{7}', Tolerancia_Entrada = '{8}', " +
				"Tiempo_Anden = '{9}', Tolerancia_Salida = '{10}', Tolerancia_Despues = '{11}', Dias = '{12}', Id_Tipo_Frecuencia = '{13}', " +
				"ExtraNoProg = '{14}', Activo = '{15}', Fecha_Creacion = {16}, Usuario_Creacion = {17}, Fecha_Modificacion = GETDATE(), " +
				"Usuario_Modificacion = {18} WHERE Id_Frecuencia = {0};", 
				Id_Frecuencia, Hora_Salida, Valor.ToString().Replace(",", SqlServer.SigFloatSql), Id_Tipo_Ruta, SqlServer.ValidarTexto(Resolucion), 
				Id_Cooperativa, Id_Ciudad_Origen, Id_Ciudad_Destino, Tolerancia_Entrada, Tiempo_Anden, Tolerancia_Salida, Tolerancia_Despues, 
				SqlServer.ValidarTexto(Dias), Id_Tipo_Frecuencia, ExtraNoProg, Activo,
				(Fecha_Creacion == null) ? "NULL" : "'" + Convert.ToDateTime(Fecha_Creacion.ToString()).ToString(SqlServer.FormatofechaHora) + "'",
				Usuario_Creacion == null ? "NULL" : "'" + Usuario_Creacion + "'", Usuario_Modificacion == null ? "NULL" : "'" + Usuario_Modificacion + "'");
		}

		public bool IsDuplicate_Frecuencia(TimeSpan hs, long dest, long coop)
		{
			return IsDuplicate_Frecuencia(0, hs, dest, coop);
		}

		public bool IsDuplicate_Frecuencia(long id, TimeSpan hs, long dest, long coop)
		{
			return Convert.ToInt16(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Frecuencia) FROM FRECUENCIA " +
				"WHERE Activo = 1 AND Hora_Salida = '{0}' AND Id_Ciudad_Destino = {1} AND Id_Cooperativa = {2} {3}", 
				hs, dest, coop, id != 0 ? "AND Id_Frecuencia <> " + id.ToString() : ""))) > 0;
		}

		public static int ContarFrecHastaHora(DateTime hs, int minutos)
		{
			int dia = (int)hs.DayOfWeek + 1;
			string hora = string.Format("Hora_Salida BETWEEN '{0}' AND '{1}' AND SUBSTRING(Dias, {2}, 1) = '1' AND ", 
				hs.TimeOfDay, hs.TimeOfDay.Add(TimeSpan.FromMinutes(minutos)), dia);
			if (hs.AddMinutes(minutos).Date > hs.Date)
			{
				// siguiente dia
				hora = string.Format("((Hora_Salida BETWEEN '{0}' AND '23:59:59' AND SUBSTRING(Dias, {1}, 1) = '1') OR " +
					"(Hora_Salida BETWEEN '00:00:00' AND '{2}' AND SUBSTRING(Dias, {3}, 1) = '1')) AND ",
					hs.TimeOfDay, dia, hs.TimeOfDay.Add(TimeSpan.FromMinutes(minutos)).Add(TimeSpan.FromDays(-1)), dia % 7 + 1);
			}
			return Convert.ToInt32(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Frecuencia) FROM FRECUENCIA WHERE Activo = 1 AND {0} ", hora)));
		}

		public static int ContarFrecHastaHoraDestino(DateTime hs, int minutos, long dest)
		{
			int dia = (int)hs.DayOfWeek + 1;
			string hora = string.Format("Hora_Salida BETWEEN '{0}' AND '{1}' AND SUBSTRING(Dias, {2}, 1) = '1' AND ", 
				hs.TimeOfDay, hs.TimeOfDay.Add(TimeSpan.FromMinutes(minutos)), dia);
			if (hs.AddMinutes(minutos).Date > hs.Date)
            {
				// siguiente dia
				hora = string.Format("((Hora_Salida BETWEEN '{0}' AND '23:59:59' AND SUBSTRING(Dias, {1}, 1) = '1') OR " +
					"(Hora_Salida BETWEEN '00:00:00' AND '{2}' AND SUBSTRING(Dias, {3}, 1) = '1')) AND ", 
					hs.TimeOfDay, dia, hs.TimeOfDay.Add(TimeSpan.FromMinutes(minutos)).Add(TimeSpan.FromDays(-1)), dia % 7 + 1);
            }
			return Convert.ToInt32(SqlServer.EXEC_SCALAR(string.Format("SELECT COUNT(Id_Frecuencia) FROM FRECUENCIA " +
				"WHERE Activo = 1 AND {0} Id_Ciudad_Destino = {1}", hora, dest)));
		}

		/// <summary>
		/// Este metodo permite seleccionar los datos del las unidades de trasporte que han comprado una frecuenca
		/// </summary>
		/// <param name="idfrecuencia"> identificador de la frecuencia</param>
		/// <returns></returns>
		public DataTable Get_Lista_unidades_Q_Compraron(long idfrecuencia)
		{
			string sql = string.Format(@"SELECT fv.id_frecuencia, ut.numero_disco, ut.placa FROM frecuencia_vendida fv
				INNER JOIN unidad_transporte ut ON ut.id_unidad_transporte = fv.id_unidad_transporte
				WHERE DATEDIFF(mi, fecha, GETDATE()) < (SELECT CAST(Valor AS INT) FROM CONFIGURACION_GLOBAL WHERE Configuracion = 'MINUTOS') 
				AND fv.id_frecuencia = {0}", idfrecuencia);

			return SqlServer.EXEC_SELECT(sql);
		}

		public string PermitirVenta(long Id_Frecuencia)
		{
			string sql = string.Format("UPDATE frecuencia_vendida SET habilitado = 1 WHERE Id_Frecuencia = {0} ", Id_Frecuencia);
			return SqlServer.EXEC_COMMAND(sql);
		}

		public static DataTable Get_listado_frecuenciasPorCooperativa(long id_cooperativa)
		{
			string sql = string.Format("EXEC ObtenerListaFrecuencias {0}", id_cooperativa);
			return SqlServer.EXEC_SELECT(sql);

		}

		public static DataTable Get_listado_frecuenciaslibresPorCooperativa(long id_cooperativa, long id_frecuencia)
        {
			string sql = string.Format("EXEC ObtenerListaFrecuenciasLibres {0}, {1}", id_cooperativa, id_frecuencia);
			return SqlServer.EXEC_SELECT(sql);
		}

		public static DataTable Get_listado_Reimprimir()
		{
			string sql = string.Format("EXEC ListaReimprimirFrecuencia");
			return SqlServer.EXEC_SELECT(sql);
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
			if (!this.disposed) {
				if (disposing) {
				}  
			}
			this.disposed = true;
		}

		/// <summary>
		/// Destructor de la instancia
		///  </summary>
		~Frecuencia ()
		{
		   this.Dispose(false);
		}
	#endregion
	}
}
