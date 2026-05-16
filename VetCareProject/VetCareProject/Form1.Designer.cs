namespace VetCareProject
{
    partial class Form1
    {
        /// <summary>
        /// Variable necesaria del diseñador.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar recursos usados.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados deben eliminarse; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método requerido para admitir el Diseñador.
        /// No modificar con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            menuStrip1 = new MenuStrip();
            menuToolStripMenuItem = new ToolStripMenuItem();
            administracionToolStripMenuItem = new ToolStripMenuItem();
            pacientesToolStripMenuItem = new ToolStripMenuItem();
            turnosToolStripMenuItem = new ToolStripMenuItem();
            historiasClinicasToolStripMenuItem = new ToolStripMenuItem();
            serviciosToolStripMenuItem = new ToolStripMenuItem();
            productosToolStripMenuItem = new ToolStripMenuItem();
            ventasToolStripMenuItem = new ToolStripMenuItem();
            reportesToolStripMenuItem = new ToolStripMenuItem();
            configuracionToolStripMenuItem = new ToolStripMenuItem();
            ayudaToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(669, 402);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { administracionToolStripMenuItem, pacientesToolStripMenuItem, turnosToolStripMenuItem, historiasClinicasToolStripMenuItem, serviciosToolStripMenuItem, productosToolStripMenuItem, ventasToolStripMenuItem, reportesToolStripMenuItem, configuracionToolStripMenuItem, ayudaToolStripMenuItem });
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(60, 24);
            menuToolStripMenuItem.Text = "Menu";
            // 
            // administracionToolStripMenuItem
            // 
            administracionToolStripMenuItem.Name = "administracionToolStripMenuItem";
            administracionToolStripMenuItem.Size = new Size(224, 26);
            administracionToolStripMenuItem.Text = "Administracion";
            // 
            // pacientesToolStripMenuItem
            // 
            pacientesToolStripMenuItem.Name = "pacientesToolStripMenuItem";
            pacientesToolStripMenuItem.Size = new Size(224, 26);
            pacientesToolStripMenuItem.Text = "Pacientes";
            // 
            // turnosToolStripMenuItem
            // 
            turnosToolStripMenuItem.Name = "turnosToolStripMenuItem";
            turnosToolStripMenuItem.Size = new Size(224, 26);
            turnosToolStripMenuItem.Text = "Turnos";
            // 
            // historiasClinicasToolStripMenuItem
            // 
            historiasClinicasToolStripMenuItem.Name = "historiasClinicasToolStripMenuItem";
            historiasClinicasToolStripMenuItem.Size = new Size(224, 26);
            historiasClinicasToolStripMenuItem.Text = "Historias Clinicas";
            // 
            // serviciosToolStripMenuItem
            // 
            serviciosToolStripMenuItem.Name = "serviciosToolStripMenuItem";
            serviciosToolStripMenuItem.Size = new Size(224, 26);
            serviciosToolStripMenuItem.Text = "Servicios";
            // 
            // productosToolStripMenuItem
            // 
            productosToolStripMenuItem.Name = "productosToolStripMenuItem";
            productosToolStripMenuItem.Size = new Size(224, 26);
            productosToolStripMenuItem.Text = "Productos";
            // 
            // ventasToolStripMenuItem
            // 
            ventasToolStripMenuItem.Name = "ventasToolStripMenuItem";
            ventasToolStripMenuItem.Size = new Size(224, 26);
            ventasToolStripMenuItem.Text = "Ventas";
            // 
            // reportesToolStripMenuItem
            // 
            reportesToolStripMenuItem.Name = "reportesToolStripMenuItem";
            reportesToolStripMenuItem.Size = new Size(224, 26);
            reportesToolStripMenuItem.Text = "Reportes";
            // 
            // configuracionToolStripMenuItem
            // 
            configuracionToolStripMenuItem.Name = "configuracionToolStripMenuItem";
            configuracionToolStripMenuItem.Size = new Size(224, 26);
            configuracionToolStripMenuItem.Text = "Configuracion";
            // 
            // ayudaToolStripMenuItem
            // 
            ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            ayudaToolStripMenuItem.Size = new Size(224, 26);
            ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // Form1
            // 
            BackColor = Color.DarkCyan;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(menuStrip1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem menuToolStripMenuItem;
        private ToolStripMenuItem administracionToolStripMenuItem;
        private ToolStripMenuItem pacientesToolStripMenuItem;
        private ToolStripMenuItem turnosToolStripMenuItem;
        private ToolStripMenuItem historiasClinicasToolStripMenuItem;
        private ToolStripMenuItem serviciosToolStripMenuItem;
        private ToolStripMenuItem productosToolStripMenuItem;
        private ToolStripMenuItem ventasToolStripMenuItem;
        private ToolStripMenuItem reportesToolStripMenuItem;
        private ToolStripMenuItem configuracionToolStripMenuItem;
        private ToolStripMenuItem ayudaToolStripMenuItem;
    }
}