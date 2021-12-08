namespace FacturaElectronica
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FacturaElectronicaProcInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.FacturaElectronicaInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // FacturaElectronicaProcInstaller
            // 
            this.FacturaElectronicaProcInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FacturaElectronicaProcInstaller.Password = null;
            this.FacturaElectronicaProcInstaller.Username = null;
            // 
            // FacturaElectronicaInstaller
            // 
            this.FacturaElectronicaInstaller.Description = "Servicio de Gestión de Facturas Electrónicas para SiCtert";
            this.FacturaElectronicaInstaller.DisplayName = "SiCtert FacturaElectronica";
            this.FacturaElectronicaInstaller.ServiceName = "SiCtert FacturaElectronica";
            this.FacturaElectronicaInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FacturaElectronicaInstaller,
            this.FacturaElectronicaProcInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FacturaElectronicaProcInstaller;
        private System.ServiceProcess.ServiceInstaller FacturaElectronicaInstaller;
    }
}