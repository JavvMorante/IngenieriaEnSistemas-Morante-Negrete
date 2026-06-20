namespace VetCare
{
    partial class AsignarPerfilaUsuario
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
            cmbUsuario = new Label();
            label3 = new Label();
            label4 = new Label();
            comboBox1 = new ComboBox();
            cmbPerfilUsuario = new ComboBox();
            lstOperaciones = new ListBox();
            LstPerfilOperaciones = new ListBox();
            btnGuardar = new Button();
            btnAgregar = new Button();
            btnDesagregar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(104, 43);
            label1.Name = "label1";
            label1.Size = new Size(62, 20);
            label1.TabIndex = 0;
            label1.Text = "Usuario:";
            // 
            // cmbUsuario
            // 
            cmbUsuario.AutoSize = true;
            cmbUsuario.Location = new Point(104, 123);
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.Size = new Size(99, 20);
            cmbUsuario.TabIndex = 1;
            cmbUsuario.Text = "Perfil Usuario:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(104, 227);
            label3.Name = "label3";
            label3.Size = new Size(173, 20);
            label3.TabIndex = 2;
            label3.Text = "Operaciones del Sistema";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(561, 227);
            label4.Name = "label4";
            label4.Size = new Size(217, 20);
            label4.TabIndex = 3;
            label4.Text = "Operaciones Asignadas al Perfil";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(232, 40);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(576, 28);
            comboBox1.TabIndex = 4;
            // 
            // cmbPerfilUsuario
            // 
            cmbPerfilUsuario.FormattingEnabled = true;
            cmbPerfilUsuario.Location = new Point(232, 115);
            cmbPerfilUsuario.Name = "cmbPerfilUsuario";
            cmbPerfilUsuario.Size = new Size(576, 28);
            cmbPerfilUsuario.TabIndex = 5;
            // 
            // lstOperaciones
            // 
            lstOperaciones.FormattingEnabled = true;
            lstOperaciones.Location = new Point(104, 268);
            lstOperaciones.Name = "lstOperaciones";
            lstOperaciones.Size = new Size(338, 324);
            lstOperaciones.TabIndex = 6;
            // 
            // LstPerfilOperaciones
            // 
            LstPerfilOperaciones.FormattingEnabled = true;
            LstPerfilOperaciones.Location = new Point(561, 268);
            LstPerfilOperaciones.Name = "LstPerfilOperaciones";
            LstPerfilOperaciones.Size = new Size(338, 324);
            LstPerfilOperaciones.TabIndex = 7;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(499, 636);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(112, 57);
            btnGuardar.TabIndex = 8;
            btnGuardar.Text = "Confirmar";
            btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(457, 349);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(94, 29);
            btnAgregar.TabIndex = 9;
            btnAgregar.Text = "->";
            btnAgregar.UseVisualStyleBackColor = true;
            // 
            // btnDesagregar
            // 
            btnDesagregar.Location = new Point(457, 466);
            btnDesagregar.Name = "btnDesagregar";
            btnDesagregar.Size = new Size(94, 29);
            btnDesagregar.TabIndex = 10;
            btnDesagregar.Text = "<-";
            btnDesagregar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(815, 636);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(106, 57);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // AsignarPerfilaUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1009, 726);
            Controls.Add(btnCancelar);
            Controls.Add(btnDesagregar);
            Controls.Add(btnAgregar);
            Controls.Add(btnGuardar);
            Controls.Add(LstPerfilOperaciones);
            Controls.Add(lstOperaciones);
            Controls.Add(cmbPerfilUsuario);
            Controls.Add(comboBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(cmbUsuario);
            Controls.Add(label1);
            Name = "AsignarPerfilaUsuario";
            Text = "AsignarPerfilaUsuario";
            Load += AsignarPerfilaUsuario_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label cmbUsuario;
        private Label label3;
        private Label label4;
        private ComboBox comboBox1;
        private ComboBox cmbPerfilUsuario;
        private ListBox lstOperaciones;
        private ListBox LstPerfilOperaciones;
        private Button btnGuardar;
        private Button btnAgregar;
        private Button btnDesagregar;
        private Button btnCancelar;
    }
}