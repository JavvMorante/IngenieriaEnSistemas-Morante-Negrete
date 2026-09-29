namespace VetCare
{
    partial class FrmLogin_60MN
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
            lblTitulo = new Label();
            pnlIdioma = new Panel();
            lblIdioma = new Label();
            cboIdioma = new ComboBox();
            lblSubtitulo = new Label();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            lblClave = new Label();
            txtClave = new TextBox();
            btnAceptar = new Button();
            btnCancelar = new Button();
            lblMensaje = new Label();
            errorProvider = new ErrorProvider(components);
            pnlIdioma.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(0, 102, 102);
            lblTitulo.Location = new Point(28, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(131, 41);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "VetCare";
            //
            // pnlIdioma
            //
            pnlIdioma.BackColor = Color.FromArgb(224, 240, 240);
            pnlIdioma.BorderStyle = BorderStyle.FixedSingle;
            pnlIdioma.Controls.Add(lblIdioma);
            pnlIdioma.Controls.Add(cboIdioma);
            pnlIdioma.Location = new Point(32, 66);
            pnlIdioma.Name = "pnlIdioma";
            pnlIdioma.Size = new Size(334, 48);
            pnlIdioma.TabIndex = 1;
            //
            // lblIdioma
            //
            lblIdioma.AutoSize = true;
            lblIdioma.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblIdioma.Location = new Point(8, 13);
            lblIdioma.Name = "lblIdioma";
            lblIdioma.Size = new Size(129, 20);
            lblIdioma.TabIndex = 0;
            lblIdioma.Text = "Cambio de idioma";
            //
            // cboIdioma
            //
            cboIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            cboIdioma.FormattingEnabled = true;
            cboIdioma.Location = new Point(170, 9);
            cboIdioma.Name = "cboIdioma";
            cboIdioma.Size = new Size(152, 28);
            cboIdioma.TabIndex = 1;
            cboIdioma.SelectedIndexChanged += cboIdioma_SelectedIndexChanged;
            //
            // lblSubtitulo
            //
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.ForeColor = Color.DimGray;
            lblSubtitulo.Location = new Point(32, 128);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(99, 20);
            lblSubtitulo.TabIndex = 2;
            lblSubtitulo.Text = "Iniciar sesión";
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.BackColor = Color.FromArgb(0, 102, 102);
            lblUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUsuario.ForeColor = Color.White;
            lblUsuario.Location = new Point(32, 163);
            lblUsuario.MinimumSize = new Size(96, 27);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Padding = new Padding(4, 3, 0, 0);
            lblUsuario.Size = new Size(96, 27);
            lblUsuario.TabIndex = 3;
            lblUsuario.Text = "Usuario";
            //
            // txtUsuario
            //
            txtUsuario.Location = new Point(136, 163);
            txtUsuario.MaxLength = 50;
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(230, 27);
            txtUsuario.TabIndex = 4;
            //
            // lblClave
            //
            lblClave.AutoSize = true;
            lblClave.BackColor = Color.FromArgb(0, 102, 102);
            lblClave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClave.ForeColor = Color.White;
            lblClave.Location = new Point(32, 200);
            lblClave.MinimumSize = new Size(96, 27);
            lblClave.Name = "lblClave";
            lblClave.Padding = new Padding(4, 3, 0, 0);
            lblClave.Size = new Size(96, 27);
            lblClave.TabIndex = 5;
            lblClave.Text = "Contraseña";
            //
            // txtClave
            //
            txtClave.Location = new Point(136, 200);
            txtClave.MaxLength = 100;
            txtClave.Name = "txtClave";
            txtClave.Size = new Size(230, 27);
            txtClave.TabIndex = 6;
            txtClave.UseSystemPasswordChar = true;
            //
            // btnAceptar
            //
            btnAceptar.BackColor = Color.FromArgb(0, 102, 102);
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAceptar.ForeColor = Color.White;
            btnAceptar.Location = new Point(136, 292);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 34);
            btnAceptar.TabIndex = 7;
            btnAceptar.Text = "Entrar";
            btnAceptar.UseVisualStyleBackColor = false;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(256, 292);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Salir";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // lblMensaje
            //
            lblMensaje.ForeColor = Color.Firebrick;
            lblMensaje.Location = new Point(32, 236);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new Size(334, 48);
            lblMensaje.TabIndex = 9;
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmLogin_60MN
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(404, 346);
            Controls.Add(lblMensaje);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(txtClave);
            Controls.Add(lblClave);
            Controls.Add(txtUsuario);
            Controls.Add(lblUsuario);
            Controls.Add(lblSubtitulo);
            Controls.Add(pnlIdioma);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmLogin_60MN";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "VetCare - Iniciar sesión";
            Load += FrmLogin_60MN_Load;
            pnlIdioma.ResumeLayout(false);
            pnlIdioma.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Panel pnlIdioma;
        private Label lblIdioma;
        private ComboBox cboIdioma;
        private Label lblSubtitulo;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblClave;
        private TextBox txtClave;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblMensaje;
        private ErrorProvider errorProvider;
    }
}
