using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace AccesoDatos
{
    public class Conceptos
    {
        #region Public Properties
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal iva { get; set; }
        public decimal Subtotal { get; set; }
        public decimal SumaTotal { get; set; }
        #endregion

        public Conceptos()
        { 
       
        }
      
        public Conceptos Parqueo(long id_unidad_transporte, long id_frecuencia)
        {
            Conceptos con = new Conceptos();
            using (DataTable dt = SqlServer.EXEC_SELECT(String.Format("EXEC TotalParqueo {0}, {1}", id_unidad_transporte, id_frecuencia)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    con.Cantidad = Convert.ToInt16(dt.Rows[0]["Cantidad"]);
                    con.Subtotal = SqlServer.Decimal_validado(dt.Rows[0]["SubTotal"].ToString());
                    con.SumaTotal = SqlServer.Decimal_validado(dt.Rows[0]["SumaTotal"].ToString());
                    con.iva = SqlServer.Decimal_validado(dt.Rows[0]["Iva"].ToString());
                    con.Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                }
            }
            return con;
        }

        public Conceptos Frecuencia(long id_frecuencia, long id_unidad_transporte)
        {
            Conceptos con = new Conceptos();
            using (DataTable dt = SqlServer.EXEC_SELECT(String.Format("EXEC TotalFrecuencia {0}, {1}", id_frecuencia, id_unidad_transporte)))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    con.Cantidad = Convert.ToInt16(dt.Rows[0]["Cantidad"]);
                    con.Subtotal = SqlServer.Decimal_validado(dt.Rows[0]["SubTotal"].ToString());
                    con.SumaTotal = SqlServer.Decimal_validado(dt.Rows[0]["SumaTotal"].ToString());
                    con.iva = SqlServer.Decimal_validado(dt.Rows[0]["Iva"].ToString());
                    con.Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                }
            }
            return con;
        }
    }
}
