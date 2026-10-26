namespace VetCare
{
    partial class FrmCliente_60MN
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
            lblTipoDocumento = new Label();
            cboTipoDocumento = new ComboBox();
            lblNroDocumento = new Label();
            txtNroDocumento = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblDireccion = new Label();
            txtDireccion = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            grpMascotas = new GroupBox();
            lstMascotas = new ListBox();
            btnAgregarMascota = new Button();
            btnQuitarMascota = new Button();
            lblNota = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            errorProvider = new ErrorProvider(components);
            grpMascotas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // lblTipoDocumento
            //
            lblTipoDocumento.AutoSize = true;
            lblTipoDocumento.Location = new Point(24, 24);
            lblTipoDocumento.Name = "lblTipoDocumento";
            lblTipoDocumento.Size = new Size(133, 20);
            lblTipoDocumento.TabIndex = 0;
            lblTipoDocumento.Text = "Tipo de documento *";
            //
            // cboTipoDocumento
            //
            cboTipoDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoDocumento.FormattingEnabled = true;
            cboTipoDocumento.Location = new Point(190, 20);
            cboTipoDocumento.Name = "cboTipoDocumento";
            cboTipoDocumento.Size = new Size(100, 28);
            cboTipoDocumento.TabIndex = 1;
            //
            // lblNroDocumento
            //
            lblNroDocumento.AutoSize = true;
            lblNroDocumento.Location = new Point(24, 64);
            lblNroDocumento.Name = "lblNroDocumento";
            lblNroDocumento.Size = new Size(146, 20);
            lblNroDocumento.TabIndex = 2;
            lblNroDocumento.Text = "Número de documento *";
            //
            // txtNroDocumento
            //
            txtNroDocumento.Location = new Point(190, 61);
            txtNroDocumento.MaxLength = 15;
            txtNroDocumento.Name = "txtNroDocumento";
            txtNroDocumento.Size = new Size(160, 27);
            txtNroDocumento.TabIndex = 3;
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(24, 104);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(73, 20);
            lblNombre.TabIndex = 4;
            lblNombre.Text = "Nombre *";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(190, 101);
            txtNombre.MaxLength = 80;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(320, 27);
            txtNombre.TabIndex = 5;
            //
            // lblApellido
            //
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(24, 144);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(73, 20);
            lblApellido.TabIndex = 6;
            lblApellido.Text = "Apellido *";
            //
            // txtApellido
            //
            txtApellido.Location = new Point(190, 141);
            txtApellido.MaxLength = 80;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(320, 27);
            txtApellido.TabIndex = 7;
            //
            // lblDireccion
            //
            lblDireccion.AutoSize = true;
            lblDireccion.Location = new Point(24, 184);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new Size(83, 20);
            lblDireccion.TabIndex = 8;
            lblDireccion.Text = "Dirección *";
            //
            // txtDireccion
            //
            txtDireccion.Location = new Point(190, 181);
            txtDireccion.MaxLength = 150;
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(320, 27);
            txtDireccion.TabIndex = 9;
            //
            // lblTelefono
            //
            lblTelefono.AutoSize = true;
            lblTelefono.Location = new Point(24, 224);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(76, 20);
            lblTelefono.TabIndex = 10;
            lblTelefono.Text = "Teléfono *";
            //
            // txtTelefono
            //
            txtTelefono.Location = new Point(190, 221);
            txtTelefono.MaxLength = 30;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(200, 27);
            txtTelefono.TabIndex = 11;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(24, 264);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(46, 20);
            lblEmail.TabIndex = 12;
            lblEmail.Text = "Email";
            //
            // txtEmail
            //
            txtEmail.Location = new Point(190, 261);
            txtEmail.MaxLength = 150;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(320, 27);
            txtEmail.TabIndex = 13;
            //
            // grpMascotas
            //
            grpMascotas.Controls.Add(lstMascotas);
            grpMascotas.Controls.Add(btnAgregarMascota);
            grpMascotas.Controls.Add(btnQuitarMascota);
            grpMascotas.Location = new Point(24, 304);
            grpMascotas.Name = "grpMascotas";
            grpMascotas.Size = new Size(486, 150);
            grpMascotas.TabIndex = 14;
            grpMascotas.TabStop = false;
            grpMascotas.Text = "Mascotas a registrar";
            //
            // lstMascotas
            //
            lstMascotas.FormattingEnabled = true;
            lstMascotas.IntegralHeight = false;
            lstMascotas.Location = new Point(12, 28);
            lstMascotas.Name = "lstMascotas";
            lstMascotas.Size = new Size(310, 108);
            lstMascotas.TabIndex = 0;
            //
            // btnAgregarMascota
            //
            btnAgregarMascota.Location = new Point(334, 28);
            btnAgregarMascota.Name = "btnAgregarMascota";
            btnAgregarMascota.Size = new Size(140, 34);
            btnAgregarMascota.TabIndex = 1;
            btnAgregarMascota.Text = "Agregar mascota";
            btnAgregarMascota.UseVisualStyleBackColor = true;
            btnAgregarMascota.Click += btnAgregarMascota_Click;
            //
            // btnQuitarMascota
            //
            btnQuitarMascota.Location = new Point(334, 70);
            btnQuitarMascota.Name = "btnQuitarMascota";
            btnQuitarMascota.Size = new Size(140, 34);
            btnQuitarMascota.TabIndex = 2;
            btnQuitarMascota.Text = "Quitar";
            btnQuitarMascota.UseVisualStyleBackColor = true;
            btnQuitarMascota.Click += btnQuitarMascota_Click;
            //
            // lblNota
            //
            lblNota.ForeColor = Color.DimGray;
            lblNota.Location = new Point(24, 462);
            lblNota.Name = "lblNota";
            lblNota.Size = new Size(486, 40);
            lblNota.TabIndex = 15;
            lblNota.Text = "Si el cliente no trae mascota, puede registrarse ahora y agregarla después desde su ficha.";
            //
            // btnAceptar
            //
            btnAceptar.Location = new Point(280, 510);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 34);
            btnAceptar.TabIndex = 16;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(400, 510);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 17;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmCliente_60MN
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(540, 562);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(lblNota);
            Controls.Add(grpMascotas);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(txtTelefono);
            Controls.Add(lblTelefono);
            Controls.Add(txtDireccion);
            Controls.Add(lblDireccion);
            Controls.Add(txtApellido);
            Controls.Add(lblApellido);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(txtNroDocumento);
            Controls.Add(lblNroDocumento);
            Controls.Add(cboTipoDocumento);
            Controls.Add(lblTipoDocumento);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCliente_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cliente";
            Load += FrmCliente_60MN_Load;
            grpMascotas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTipoDocumento;
        private ComboBox cboTipoDocumento;
        private Label lblNroDocumento;
        private TextBox txtNroDocumento;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblDireccion;
        private TextBox txtDireccion;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblEmail;
        private TextBox txtEmail;
        private GroupBox grpMascotas;
        private ListBox lstMascotas;
        private Button btnAgregarMascota;
        private Button btnQuitarMascota;
        private Label lblNota;
        private Button btnAceptar;
        private Button btnCancelar;
        private ErrorProvider errorProvider;
    }
}
