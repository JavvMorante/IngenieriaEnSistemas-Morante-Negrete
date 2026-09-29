namespace VetCare
{
    partial class FrmCambiarClave_60MN
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
            lblInfo = new Label();
            lblActual = new Label();
            txtActual = new TextBox();
            lblNueva = new Label();
            txtNueva = new TextBox();
            lblConfirmacion = new Label();
            txtConfirmacion = new TextBox();
            lblPolitica = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // lblInfo
            //
            lblInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblInfo.Location = new Point(24, 16);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(420, 24);
            lblInfo.TabIndex = 0;
            lblInfo.Text = "Ingrese su contraseña actual y la nueva contraseña.";
            //
            // lblActual
            //
            lblActual.AutoSize = true;
            lblActual.Location = new Point(24, 58);
            lblActual.Name = "lblActual";
            lblActual.Size = new Size(131, 20);
            lblActual.TabIndex = 1;
            lblActual.Text = "Contraseña actual";
            //
            // txtActual
            //
            txtActual.Location = new Point(214, 55);
            txtActual.MaxLength = 100;
            txtActual.Name = "txtActual";
            txtActual.Size = new Size(230, 27);
            txtActual.TabIndex = 2;
            txtActual.UseSystemPasswordChar = true;
            //
            // lblNueva
            //
            lblNueva.AutoSize = true;
            lblNueva.Location = new Point(24, 98);
            lblNueva.Name = "lblNueva";
            lblNueva.Size = new Size(129, 20);
            lblNueva.TabIndex = 3;
            lblNueva.Text = "Nueva contraseña";
            //
            // txtNueva
            //
            txtNueva.Location = new Point(214, 95);
            txtNueva.MaxLength = 100;
            txtNueva.Name = "txtNueva";
            txtNueva.Size = new Size(230, 27);
            txtNueva.TabIndex = 4;
            txtNueva.UseSystemPasswordChar = true;
            //
            // lblConfirmacion
            //
            lblConfirmacion.AutoSize = true;
            lblConfirmacion.Location = new Point(24, 138);
            lblConfirmacion.Name = "lblConfirmacion";
            lblConfirmacion.Size = new Size(184, 20);
            lblConfirmacion.TabIndex = 5;
            lblConfirmacion.Text = "Confirmar contraseña";
            //
            // txtConfirmacion
            //
            txtConfirmacion.Location = new Point(214, 135);
            txtConfirmacion.MaxLength = 100;
            txtConfirmacion.Name = "txtConfirmacion";
            txtConfirmacion.Size = new Size(230, 27);
            txtConfirmacion.TabIndex = 6;
            txtConfirmacion.UseSystemPasswordChar = true;
            //
            // lblPolitica
            //
            lblPolitica.ForeColor = Color.DimGray;
            lblPolitica.Location = new Point(24, 176);
            lblPolitica.Name = "lblPolitica";
            lblPolitica.Size = new Size(420, 44);
            lblPolitica.TabIndex = 7;
            lblPolitica.Text = "La contraseña debe tener al menos 8 caracteres e incluir mayúsculas, minúsculas y números.";
            //
            // btnAceptar
            //
            btnAceptar.Location = new Point(214, 232);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 34);
            btnAceptar.TabIndex = 8;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(334, 232);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 9;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmCambiarClave_60MN
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(474, 286);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(lblPolitica);
            Controls.Add(txtConfirmacion);
            Controls.Add(lblConfirmacion);
            Controls.Add(txtNueva);
            Controls.Add(lblNueva);
            Controls.Add(txtActual);
            Controls.Add(lblActual);
            Controls.Add(lblInfo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCambiarClave_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cambiar contraseña";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblInfo;
        private Label lblActual;
        private TextBox txtActual;
        private Label lblNueva;
        private TextBox txtNueva;
        private Label lblConfirmacion;
        private TextBox txtConfirmacion;
        private Label lblPolitica;
        private Button btnAceptar;
        private Button btnCancelar;
        private ErrorProvider errorProvider;
    }
}
