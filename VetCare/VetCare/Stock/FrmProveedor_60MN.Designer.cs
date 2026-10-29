namespace VetCare
{
    partial class FrmProveedor_60MN
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
            lblRazonSocial = new Label();
            txtRazonSocial = new TextBox();
            lblCuit = new Label();
            txtCuit = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblDireccion = new Label();
            txtDireccion = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblContacto = new Label();
            txtContacto = new TextBox();
            lblNota = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // lblRazonSocial
            //
            lblRazonSocial.AutoSize = true;
            lblRazonSocial.Location = new Point(24, 24);
            lblRazonSocial.Name = "lblRazonSocial";
            lblRazonSocial.Size = new Size(104, 20);
            lblRazonSocial.TabIndex = 0;
            lblRazonSocial.Text = "Razón social *";
            //
            // txtRazonSocial
            //
            txtRazonSocial.Location = new Point(190, 21);
            txtRazonSocial.Name = "txtRazonSocial";
            txtRazonSocial.Size = new Size(320, 27);
            txtRazonSocial.TabIndex = 1;
            //
            // lblCuit
            //
            lblCuit.AutoSize = true;
            lblCuit.Location = new Point(24, 64);
            lblCuit.Name = "lblCuit";
            lblCuit.Size = new Size(48, 20);
            lblCuit.TabIndex = 2;
            lblCuit.Text = "CUIT *";
            //
            // txtCuit
            //
            txtCuit.Location = new Point(190, 61);
            txtCuit.Name = "txtCuit";
            txtCuit.Size = new Size(180, 27);
            txtCuit.TabIndex = 3;
            //
            // lblTelefono
            //
            lblTelefono.AutoSize = true;
            lblTelefono.Location = new Point(24, 104);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(77, 20);
            lblTelefono.TabIndex = 4;
            lblTelefono.Text = "Teléfono *";
            //
            // txtTelefono
            //
            txtTelefono.Location = new Point(190, 101);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(180, 27);
            txtTelefono.TabIndex = 5;
            //
            // lblDireccion
            //
            lblDireccion.AutoSize = true;
            lblDireccion.Location = new Point(24, 144);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new Size(84, 20);
            lblDireccion.TabIndex = 6;
            lblDireccion.Text = "Dirección *";
            //
            // txtDireccion
            //
            txtDireccion.Location = new Point(190, 141);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(320, 27);
            txtDireccion.TabIndex = 7;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(24, 184);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(46, 20);
            lblEmail.TabIndex = 8;
            lblEmail.Text = "Email";
            //
            // txtEmail
            //
            txtEmail.Location = new Point(190, 181);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(320, 27);
            txtEmail.TabIndex = 9;
            //
            // lblContacto
            //
            lblContacto.AutoSize = true;
            lblContacto.Location = new Point(24, 224);
            lblContacto.Name = "lblContacto";
            lblContacto.Size = new Size(69, 20);
            lblContacto.TabIndex = 10;
            lblContacto.Text = "Contacto";
            //
            // txtContacto
            //
            txtContacto.Location = new Point(190, 221);
            txtContacto.Name = "txtContacto";
            txtContacto.Size = new Size(320, 27);
            txtContacto.TabIndex = 11;
            //
            // lblNota
            //
            lblNota.ForeColor = SystemColors.GrayText;
            lblNota.Location = new Point(24, 262);
            lblNota.Name = "lblNota";
            lblNota.Size = new Size(486, 44);
            lblNota.TabIndex = 12;
            //
            // btnAceptar
            //
            btnAceptar.Location = new Point(280, 314);
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
            btnCancelar.Location = new Point(400, 314);
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
            // FrmProveedor_60MN
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(540, 364);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(lblNota);
            Controls.Add(txtContacto);
            Controls.Add(lblContacto);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(txtDireccion);
            Controls.Add(lblDireccion);
            Controls.Add(txtTelefono);
            Controls.Add(lblTelefono);
            Controls.Add(txtCuit);
            Controls.Add(lblCuit);
            Controls.Add(txtRazonSocial);
            Controls.Add(lblRazonSocial);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmProveedor_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Proveedor";
            Load += FrmProveedor_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRazonSocial;
        private TextBox txtRazonSocial;
        private Label lblCuit;
        private TextBox txtCuit;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblDireccion;
        private TextBox txtDireccion;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblContacto;
        private TextBox txtContacto;
        private Label lblNota;
        private Button btnAceptar;
        private Button btnCancelar;
        private ErrorProvider errorProvider;
    }
}
