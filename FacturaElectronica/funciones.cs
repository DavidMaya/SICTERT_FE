using System;
using System.IO;

namespace HRDA
{
	/// <summary>
	/// Description of funciones.
	/// </summary>
    public class funciones : IDisposable
	{
        private string DirectorioTrabajo;
        private string path;
        private bool ArchivoLog;

        public funciones(string dir, string pt, bool logf)
		{
            DirectorioTrabajo = dir;
            path = pt;
            ArchivoLog = logf;
        }
		
        public void Log(string Mensaje)
        {
            if (ArchivoLog)
            {
                // rotar logs
                try
                {
                    var FileSize = Convert.ToDecimal((new FileInfo(path)).Length);
                    if (FileSize > 250000000) // 250MB
                    {
                        string[] logFileList = Directory.GetFiles(DirectorioTrabajo, "log-backup-*.txt", SearchOption.TopDirectoryOnly);
                        if (logFileList.Length > 0)
                        {
                            Array.Sort(logFileList, 0, logFileList.Length);
                            Array.Reverse(logFileList);

                            foreach (string fname in logFileList)
                            {
                                int num = 0;
                                string sname = new FileInfo(fname).Name;
                                if (int.TryParse(sname.Substring(9, sname.Length - 13), out num))
                                    if (num > 8)
                                        File.Delete(sname);
                                    else
                                    {
                                        num++;
                                        File.Move(fname, fname.Substring(0, fname.Length - 5) + num.ToString() + ".txt");
                                    }
                            }
                        }
                        File.Move(path, path.Substring(0, path.Length - 4) + "-1" + path.Substring(path.Length - 4));
                    }
                }
                catch { }

                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(DateTime.Now.ToString() + " :: " + Mensaje);
                tw.Close();
            }
        }
        
        private string SQLValidoParaReportes(string SQLOriginal)
        {
            string SQL = "";
            for (int i = 0; i < SQLOriginal.Length; i++)
            {
                if (SQLOriginal[i].ToString() == ">")
                    SQL += "#62#";
                else
                {
                    if (SQLOriginal[i].ToString() == "<")
                        SQL += "#60#";
                    else
                    {
                        if (SQLOriginal[i].ToString() == "!")
                            SQL += "#33#";
                        else
                        {
                            if (SQLOriginal[i].ToString() == ",")
                                SQL += "#44#";
                            else
                            {
                                if (SQLOriginal[i].ToString() == "'")
                                    SQL += "#39#";
                                else
                                {
                                    if (SQLOriginal[i].ToString() == "*")
                                        SQL += "#42#";
                                    else
                                    {
                                        if (SQLOriginal[i].ToString() == "#")
                                            SQL += "#35#";
                                        else
                                        {
                                            if (SQLOriginal[i].ToString() == "+")
                                                SQL += "#43#";
                                            else
                                            {
                                                if (SQLOriginal[i].ToString() == "-")
                                                    SQL += "#45#";
                                                else
                                                {
                                                    if (SQLOriginal[i].ToString() == "/")
                                                        SQL += "#47#";
                                                    else
                                                    {
                                                        if (SQLOriginal[i].ToString() == "%")
                                                            SQL += "#37#";
                                                        else
                                                        {
                                                            if (Char.ToUpper(SQLOriginal[i]) == 13)
                                                                SQL += "#13#";
                                                            else
                                                            {
                                                                if (Char.ToUpper(SQLOriginal[i]) == 9)
                                                                    SQL += "#9#";
                                                                else
                                                                {
                                                                    if (Char.ToUpper(SQLOriginal[i]) == 10)
                                                                        SQL += "#10#";
                                                                    else
                                                                    {
                                                                        if (Char.ToUpper(SQLOriginal[i]).ToString() == "@")
                                                                            SQL += "#64#";
                                                                        else
                                                                        {
                                                                            if (Char.ToUpper(SQLOriginal[i]).ToString() == "~")
                                                                                SQL += "#126#";
                                                                            else
                                                                            {
                                                                                if (Char.ToUpper(SQLOriginal[i]).ToString() == "´")
                                                                                    SQL += "#180#";
                                                                                else
                                                                                {
                                                                                    if (Char.ToUpper(SQLOriginal[i]).ToString() == "¿")
                                                                                        SQL += "#191#";
                                                                                    else
                                                                                    {
                                                                                        if (Char.ToUpper(SQLOriginal[i]).ToString() == "?")
                                                                                            SQL += "#63#";
                                                                                        else
                                                                                        {
                                                                                            if (Char.ToUpper(SQLOriginal[i]).ToString() == "\\")
                                                                                                SQL += "#92#";
                                                                                            else
                                                                                            {
                                                                                                if (SQLOriginal[i].ToString() == "ñ")
                                                                                                    SQL += "#241#";
                                                                                                else
                                                                                                {
                                                                                                    if (SQLOriginal[i].ToString() == "Ñ")
                                                                                                        SQL += "#209#";
                                                                                                    else
                                                                                                    {
                                                                                                        if (SQLOriginal[i].ToString() == "[")
                                                                                                            SQL += "#91#";
                                                                                                        else
                                                                                                        {
                                                                                                            if (SQLOriginal[i].ToString() == "]")
                                                                                                                SQL += "#93#";
                                                                                                            else
                                                                                                                SQL += SQLOriginal[i].ToString();
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return this.TildesSQL(SQL);
        }

        private string TildesSQL(string SQLOriginal)
        {
            string SQL = "";
            for (int i = 0; i < SQLOriginal.Length; i++)
            {
                if (SQLOriginal[i].ToString() == "Á")
                    SQL += "#193#";
                else
                {
                    if (SQLOriginal[i].ToString() == "É")
                        SQL += "#201#";
                    else
                    {
                        if (SQLOriginal[i].ToString() == "Í")
                            SQL += "#205#";
                        else
                        {
                            if (SQLOriginal[i].ToString() == "Ó")
                                SQL += "#211#";
                            else
                            {
                                if (SQLOriginal[i].ToString() == "Ú")
                                    SQL += "#218#";
                                else
                                {
                                    if (SQLOriginal[i].ToString() == "á")
                                        SQL += "#225#";
                                    else
                                    {
                                        if (SQLOriginal[i].ToString() == "é")
                                            SQL += "#233#";
                                        else
                                        {
                                            if (SQLOriginal[i].ToString() == "í")
                                                SQL += "#237#";
                                            else
                                            {
                                                if (SQLOriginal[i].ToString() == "ó")
                                                    SQL += "#243#";
                                                else
                                                {
                                                    if (SQLOriginal[i].ToString() == "ú")
                                                        SQL += "#250#";
                                                    else
                                                    {
                                                        if (SQLOriginal[i].ToString() == "&")
                                                            SQL += "#38#";
                                                        else
                                                        {
                                                            if (SQLOriginal[i].ToString() == "|")
                                                                SQL += "#124#";
                                                            else
                                                                SQL += SQLOriginal[i].ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return SQL;
        }
        
        private int CamposBlobSize(string strFileName)
        {
            int FileSize;
            FileStream fs;
            try
            {
                fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
                FileSize = Convert.ToInt32(fs.Length);
                fs.Close();
                return FileSize;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private byte[] CamposBlobRaw(string strFileName)
        {
            byte[] rawData;
            UInt32 FileSize;
            FileStream fs;
            //try			{
            //MessageBox.Show(strFileName);
            fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
            FileSize = Convert.ToUInt32(fs.Length);
            rawData = new byte[FileSize];
            fs.Read(rawData, 0, (int)FileSize);
            fs.Close();
            return rawData;
        }

        #region metodo disopose

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
        ~funciones()
        {
            this.Dispose(false);
        }
        #endregion
	}
}
