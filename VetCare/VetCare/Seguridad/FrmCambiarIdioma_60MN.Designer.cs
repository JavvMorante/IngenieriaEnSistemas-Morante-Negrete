namespace VetCare
{
    partial class FrmCambiarIdioma_60MN
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
            lblIdioma = new Label();
            cboIdioma = new ComboBox();
            lblNota = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            //
            // lblIdioma
            //
            lblIdioma.AutoSize = true;
            lblIdioma.Location = new Point(24, 28);
            lblIdioma.Name = "lblIdioma";
            lblIdioma.Size = new Size(54, 20);
            lblIdioma.TabIndex = 0;
            lblIdioma.Text = "Idioma";
            //
            // cboIdioma
            //
            cboIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            cboIdioma.FormattingEnabled = true;
            cboIdioma.Location = new Point(110, 24);
            cboIdioma.Name = "cboIdioma";
            cboIdioma.Size = new Size(230, 28);
            cboIdioma.TabIndex = 1;
            //
            // lblNota
            //
            lblNota.ForeColor = Color.DimGray;
            lblNota.Location = new Point(24, 66);
            lblNota.Name = "lblNota";
            lblNota.Size = new Size(316, 44);
            lblNota.TabIndex = 2;
            lblNota.Text = "El idioma elegido quedará predeterminado para el próximo inicio de sesión.";
            //
            // btnAceptar
            //
            btnAceptar.Location = new Point(110, 120);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 34);
            btnAceptar.TabIndex = 3;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(230, 120);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmCambiarIdioma_60MN
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(366, 172);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(lblNota);
            Controls.Add(cboIdioma);
            Controls.Add(lblIdioma);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCambiarIdioma_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cambiar idioma";
            Load += FrmCambiarIdioma_60MN_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblIdioma;
        private ComboBox cboIdioma;
        private Label lblNota;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}
