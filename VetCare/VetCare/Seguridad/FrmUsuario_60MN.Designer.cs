namespace VetCare
{
    partial class FrmUsuario_60MN
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
            components = new System.ComponentModel.Container();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblDni = new Label();
            txtDni = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            lblFamilia = new Label();
            cboFamilia = new ComboBox();
            lblNota = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // lblApellido
            //
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(24, 24);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(73, 20);
            lblApellido.TabIndex = 0;
            lblApellido.Text = "Apellido *";
            //
            // txtApellido
            //
            txtApellido.Location = new Point(170, 21);
            txtApellido.MaxLength = 80;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(280, 27);
            txtApellido.TabIndex = 1;
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(24, 64);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(73, 20);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre *";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(170, 61);
            txtNombre.MaxLength = 80;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(280, 27);
            txtNombre.TabIndex = 3;
            //
            // lblDni
            //
            lblDni.AutoSize = true;
            lblDni.Location = new Point(24, 104);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(45, 20);
            lblDni.TabIndex = 4;
            lblDni.Text = "DNI *";
            //
            // txtDni
            //
            txtDni.Location = new Point(170, 101);
            txtDni.MaxLength = 8;
            txtDni.Name = "txtDni";
            txtDni.Size = new Size(140, 27);
            txtDni.TabIndex = 5;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(24, 144);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(48, 20);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "Mail *";
            //
            // txtEmail
            //
            txtEmail.Location = new Point(170, 141);
            txtEmail.MaxLength = 150;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(280, 27);
            txtEmail.TabIndex = 7;
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(24, 184);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(140, 20);
            lblUsuario.TabIndex = 8;
            lblUsuario.Text = "Nombre de usuario *";
            //
            // txtUsuario
            //
            txtUsuario.Location = new Point(170, 181);
            txtUsuario.MaxLength = 50;
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(200, 27);
            txtUsuario.TabIndex = 9;
            //
            // lblFamilia
            //
            lblFamilia.AutoSize = true;
            lblFamilia.Location = new Point(24, 224);
            lblFamilia.Name = "lblFamilia";
            lblFamilia.Size = new Size(41, 20);
            lblFamilia.TabIndex = 10;
            lblFamilia.Text = "Familia *";
            //
            // cboFamilia
            //
            cboFamilia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFamilia.FormattingEnabled = true;
            cboFamilia.Location = new Point(170, 221);
            cboFamilia.Name = "cboFamilia";
            cboFamilia.Size = new Size(280, 28);
            cboFamilia.TabIndex = 11;
            //
            // lblNota
            //
            lblNota.ForeColor = Color.DimGray;
            lblNota.Location = new Point(24, 262);
            lblNota.Name = "lblNota";
            lblNota.Size = new Size(426, 44);
            lblNota.TabIndex = 12;
            lblNota.Text = "Al aceptar, el sistema genera una contraseña inicial que el usuario deberá cambiar en su primer ingreso.";
            //
            // btnAceptar
            //
            btnAceptar.Location = new Point(220, 318);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 34);
            btnAceptar.TabIndex = 13;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(340, 318);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 14;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmUsuario_60MN
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(482, 372);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(lblNota);
            Controls.Add(cboFamilia);
            Controls.Add(lblFamilia);
            Controls.Add(txtUsuario);
            Controls.Add(lblUsuario);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(txtDni);
            Controls.Add(lblDni);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(txtApellido);
            Controls.Add(lblApellido);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmUsuario_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Usuario";
            Load += FrmUsuario_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblDni;
        private TextBox txtDni;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblFamilia;
        private ComboBox cboFamilia;
        private Label lblNota;
        private Button btnAceptar;
        private Button btnCancelar;
        private ErrorProvider errorProvider;
    }
}
