namespace VetCare
{
    partial class FrmMascotas_60MN
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
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblEspecie = new Label();
            cboEspecie = new ComboBox();
            lblRaza = new Label();
            txtRaza = new TextBox();
            lblSexo = new Label();
            rdoMacho = new RadioButton();
            rdoHembra = new RadioButton();
            lblNacimiento = new Label();
            dtpNacimiento = new DateTimePicker();
            btnAceptar = new Button();
            btnCancelar = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(24, 24);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(73, 20);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre *";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(180, 21);
            txtNombre.MaxLength = 60;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(250, 27);
            txtNombre.TabIndex = 1;
            //
            // lblEspecie
            //
            lblEspecie.AutoSize = true;
            lblEspecie.Location = new Point(24, 64);
            lblEspecie.Name = "lblEspecie";
            lblEspecie.Size = new Size(68, 20);
            lblEspecie.TabIndex = 2;
            lblEspecie.Text = "Especie *";
            //
            // cboEspecie
            //
            cboEspecie.FormattingEnabled = true;
            cboEspecie.Items.AddRange(new object[] { "Perro", "Gato", "Ave", "Conejo", "Hurón", "Reptil", "Otro" });
            cboEspecie.Location = new Point(180, 60);
            cboEspecie.MaxLength = 40;
            cboEspecie.Name = "cboEspecie";
            cboEspecie.Size = new Size(250, 28);
            cboEspecie.TabIndex = 3;
            //
            // lblRaza
            //
            lblRaza.AutoSize = true;
            lblRaza.Location = new Point(24, 104);
            lblRaza.Name = "lblRaza";
            lblRaza.Size = new Size(50, 20);
            lblRaza.TabIndex = 4;
            lblRaza.Text = "Raza *";
            //
            // txtRaza
            //
            txtRaza.Location = new Point(180, 101);
            txtRaza.MaxLength = 60;
            txtRaza.Name = "txtRaza";
            txtRaza.Size = new Size(250, 27);
            txtRaza.TabIndex = 5;
            //
            // lblSexo
            //
            lblSexo.AutoSize = true;
            lblSexo.Location = new Point(24, 144);
            lblSexo.Name = "lblSexo";
            lblSexo.Size = new Size(49, 20);
            lblSexo.TabIndex = 6;
            lblSexo.Text = "Sexo *";
            //
            // rdoMacho
            //
            rdoMacho.AutoSize = true;
            rdoMacho.Checked = true;
            rdoMacho.Location = new Point(180, 142);
            rdoMacho.Name = "rdoMacho";
            rdoMacho.Size = new Size(74, 24);
            rdoMacho.TabIndex = 7;
            rdoMacho.TabStop = true;
            rdoMacho.Text = "Macho";
            rdoMacho.UseVisualStyleBackColor = true;
            //
            // rdoHembra
            //
            rdoHembra.AutoSize = true;
            rdoHembra.Location = new Point(280, 142);
            rdoHembra.Name = "rdoHembra";
            rdoHembra.Size = new Size(81, 24);
            rdoHembra.TabIndex = 8;
            rdoHembra.Text = "Hembra";
            rdoHembra.UseVisualStyleBackColor = true;
            //
            // lblNacimiento
            //
            lblNacimiento.AutoSize = true;
            lblNacimiento.Location = new Point(24, 184);
            lblNacimiento.Name = "lblNacimiento";
            lblNacimiento.Size = new Size(149, 20);
            lblNacimiento.TabIndex = 9;
            lblNacimiento.Text = "Fecha de nacimiento *";
            //
            // dtpNacimiento
            //
            dtpNacimiento.Format = DateTimePickerFormat.Short;
            dtpNacimiento.Location = new Point(180, 180);
            dtpNacimiento.Name = "dtpNacimiento";
            dtpNacimiento.Size = new Size(150, 27);
            dtpNacimiento.TabIndex = 10;
            //
            // btnAceptar
            //
            btnAceptar.Location = new Point(200, 232);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 34);
            btnAceptar.TabIndex = 11;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(320, 232);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 12;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmMascotas_60MN
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(460, 286);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(dtpNacimiento);
            Controls.Add(lblNacimiento);
            Controls.Add(rdoHembra);
            Controls.Add(rdoMacho);
            Controls.Add(lblSexo);
            Controls.Add(txtRaza);
            Controls.Add(lblRaza);
            Controls.Add(cboEspecie);
            Controls.Add(lblEspecie);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmMascotas_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Mascota";
            Load += FrmMascotas_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblEspecie;
        private ComboBox cboEspecie;
        private Label lblRaza;
        private TextBox txtRaza;
        private Label lblSexo;
        private RadioButton rdoMacho;
        private RadioButton rdoHembra;
        private Label lblNacimiento;
        private DateTimePicker dtpNacimiento;
        private Button btnAceptar;
        private Button btnCancelar;
        private ErrorProvider errorProvider;
    }
}
