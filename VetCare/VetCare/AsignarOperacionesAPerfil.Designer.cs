namespace VetCare
{
    partial class AsignarOperacionesAPerfil
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
            lblNombreUsuario = new Label();
            label2 = new Label();
            label3 = new Label();
            ListOperaciones = new ListBox();
            ListPerfilOperaciones = new ListBox();
            btnAgregar = new Button();
            btnDesagregar = new Button();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // lblNombreUsuario
            // 
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.Location = new Point(38, 24);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(122, 20);
            lblNombreUsuario.TabIndex = 0;
            lblNombreUsuario.Text = "Nombre de Perfil";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(38, 87);
            label2.Name = "label2";
            label2.Size = new Size(173, 20);
            label2.TabIndex = 1;
            label2.Text = "Operaciones del Sistema";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(477, 87);
            label3.Name = "label3";
            label3.Size = new Size(217, 20);
            label3.TabIndex = 2;
            label3.Text = "Operaciones Asignadas al Perfil";
            // 
            // ListOperaciones
            // 
            ListOperaciones.FormattingEnabled = true;
            ListOperaciones.Location = new Point(38, 110);
            ListOperaciones.Name = "ListOperaciones";
            ListOperaciones.Size = new Size(326, 244);
            ListOperaciones.TabIndex = 3;
            // 
            // ListPerfilOperaciones
            // 
            ListPerfilOperaciones.FormattingEnabled = true;
            ListPerfilOperaciones.Location = new Point(477, 110);
            ListPerfilOperaciones.Name = "ListPerfilOperaciones";
            ListPerfilOperaciones.Size = new Size(326, 244);
            ListPerfilOperaciones.TabIndex = 5;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(393, 163);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(55, 29);
            btnAgregar.TabIndex = 6;
            btnAgregar.Text = "->";
            btnAgregar.UseVisualStyleBackColor = true;
            btnAgregar.Click += btnAgregar_Click;
            // 
            // btnDesagregar
            // 
            btnDesagregar.Location = new Point(393, 211);
            btnDesagregar.Name = "btnDesagregar";
            btnDesagregar.Size = new Size(55, 29);
            btnDesagregar.TabIndex = 7;
            btnDesagregar.Text = "<-";
            btnDesagregar.UseVisualStyleBackColor = true;
            btnDesagregar.Click += btnDesagregar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(415, 392);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(174, 46);
            btnGuardar.TabIndex = 8;
            btnGuardar.Text = "Guardar Configuracion";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(667, 392);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(136, 46);
            btnCancelar.TabIndex = 9;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // AsignarOperacionesAPerfil
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(847, 450);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(btnDesagregar);
            Controls.Add(btnAgregar);
            Controls.Add(ListPerfilOperaciones);
            Controls.Add(ListOperaciones);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lblNombreUsuario);
            Name = "AsignarOperacionesAPerfil";
            Text = "AsignarOperacionesAPerfil";
            Load += AsignarOperacionesAPerfil_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNombreUsuario;
        private Label label2;
        private Label label3;
        private ListBox ListOperaciones;
        private ListBox ListPerfilOperaciones;
        private Button btnAgregar;
        private Button btnDesagregar;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}