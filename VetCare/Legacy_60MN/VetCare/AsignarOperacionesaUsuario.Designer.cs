namespace VetCare
{
    partial class AsignarOperacionesaUsuario
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            lblNombreUsuario = new Label();
            label3 = new Label();
            lblNombrePerfil = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            LstOperaciones = new ListBox();
            LstUsuarioOperaciones = new ListBox();
            btnGuardar = new Button();
            BtnAgregar = new Button();
            BtnDesagregar = new Button();
            button4 = new Button();
            cmbPerfiles = new ComboBox();
            BtnAsignarPerfil = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(38, 38);
            label1.Name = "label1";
            label1.Size = new Size(62, 20);
            label1.TabIndex = 0;
            label1.Text = "Usuario:";
            // 
            // lblNombreUsuario
            // 
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.Location = new Point(164, 38);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(59, 20);
            lblNombreUsuario.TabIndex = 1;
            lblNombreUsuario.Text = "Usuario";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(38, 86);
            label3.Name = "label3";
            label3.Size = new Size(45, 20);
            label3.TabIndex = 2;
            label3.Text = "Perfil:";
            // 
            // lblNombrePerfil
            // 
            lblNombrePerfil.AutoSize = true;
            lblNombrePerfil.Location = new Point(164, 86);
            lblNombrePerfil.Name = "lblNombrePerfil";
            lblNombrePerfil.Size = new Size(42, 20);
            lblNombrePerfil.TabIndex = 3;
            lblNombrePerfil.Text = "Perfil";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(594, 38);
            label5.Name = "label5";
            label5.Size = new Size(134, 20);
            label5.TabIndex = 4;
            label5.Text = "Perfiles de Usuario:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(38, 196);
            label6.Name = "label6";
            label6.Size = new Size(173, 20);
            label6.TabIndex = 5;
            label6.Text = "Operaciones del Sistema";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(594, 196);
            label7.Name = "label7";
            label7.Size = new Size(230, 20);
            label7.TabIndex = 6;
            label7.Text = "Operaciones Asignadas a Usuario";
            // 
            // LstOperaciones
            // 
            LstOperaciones.FormattingEnabled = true;
            LstOperaciones.Location = new Point(38, 230);
            LstOperaciones.Name = "LstOperaciones";
            LstOperaciones.Size = new Size(402, 304);
            LstOperaciones.TabIndex = 7;
            // 
            // LstUsuarioOperaciones
            // 
            LstUsuarioOperaciones.FormattingEnabled = true;
            LstUsuarioOperaciones.Location = new Point(594, 230);
            LstUsuarioOperaciones.Name = "LstUsuarioOperaciones";
            LstUsuarioOperaciones.Size = new Size(402, 304);
            LstUsuarioOperaciones.TabIndex = 8;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(346, 577);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(123, 55);
            btnGuardar.TabIndex = 9;
            btnGuardar.Text = "Guardar Configuracion";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // BtnAgregar
            // 
            BtnAgregar.Location = new Point(468, 300);
            BtnAgregar.Name = "BtnAgregar";
            BtnAgregar.Size = new Size(94, 29);
            BtnAgregar.TabIndex = 10;
            BtnAgregar.Text = "->";
            BtnAgregar.UseVisualStyleBackColor = true;
            BtnAgregar.Click += BtnAgregar_Click;
            // 
            // BtnDesagregar
            // 
            BtnDesagregar.Location = new Point(468, 400);
            BtnDesagregar.Name = "BtnDesagregar";
            BtnDesagregar.Size = new Size(94, 29);
            BtnDesagregar.TabIndex = 11;
            BtnDesagregar.Text = "<-";
            BtnDesagregar.UseVisualStyleBackColor = true;
            BtnDesagregar.Click += BtnDesagregar_Click;
            // 
            // button4
            // 
            button4.Location = new Point(594, 577);
            button4.Name = "button4";
            button4.Size = new Size(122, 55);
            button4.TabIndex = 12;
            button4.Text = "Cancelar";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // cmbPerfiles
            // 
            cmbPerfiles.FormattingEnabled = true;
            cmbPerfiles.Location = new Point(594, 78);
            cmbPerfiles.Name = "cmbPerfiles";
            cmbPerfiles.Size = new Size(316, 28);
            cmbPerfiles.TabIndex = 13;
            // 
            // BtnAsignarPerfil
            // 
            BtnAsignarPerfil.Location = new Point(944, 78);
            BtnAsignarPerfil.Name = "BtnAsignarPerfil";
            BtnAsignarPerfil.Size = new Size(122, 55);
            BtnAsignarPerfil.TabIndex = 14;
            BtnAsignarPerfil.Text = "Asignar Perfil";
            BtnAsignarPerfil.UseVisualStyleBackColor = true;
            BtnAsignarPerfil.Click += BtnAsignarPerfil_Click;
            // 
            // AsignarOperacionesaUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1078, 644);
            Controls.Add(BtnAsignarPerfil);
            Controls.Add(cmbPerfiles);
            Controls.Add(button4);
            Controls.Add(BtnDesagregar);
            Controls.Add(BtnAgregar);
            Controls.Add(btnGuardar);
            Controls.Add(LstUsuarioOperaciones);
            Controls.Add(LstOperaciones);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(lblNombrePerfil);
            Controls.Add(label3);
            Controls.Add(lblNombreUsuario);
            Controls.Add(label1);
            Name = "AsignarOperacionesaUsuario";
            Text = "AsignarOperacionesaUsuario";
            Load += AsignarOperacionesaUsuario_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label lblNombreUsuario;
        private Label label3;
        private Label lblNombrePerfil;
        private Label label5;
        private Label label6;
        private Label label7;
        private ListBox LstOperaciones;
        private ListBox LstUsuarioOperaciones;
        private Button btnGuardar;
        private Button BtnAgregar;
        private Button BtnDesagregar;
        private Button button4;
        private ComboBox cmbPerfiles;
        private Button BtnAsignarPerfil;
    }
}