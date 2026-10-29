namespace VetCare
{
    partial class FrmAyuda_60MN
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
            lblIndice = new Label();
            lstTemas = new ListBox();
            lblTema = new Label();
            txtTexto = new TextBox();
            btnCerrar = new Button();
            SuspendLayout();
            //
            // lblIndice
            //
            lblIndice.AutoSize = true;
            lblIndice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblIndice.Location = new Point(12, 12);
            lblIndice.Name = "lblIndice";
            lblIndice.Size = new Size(113, 20);
            lblIndice.TabIndex = 0;
            lblIndice.Text = "Índice de temas";
            //
            // lstTemas
            //
            lstTemas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lstTemas.FormattingEnabled = true;
            lstTemas.IntegralHeight = false;
            lstTemas.Location = new Point(12, 38);
            lstTemas.Name = "lstTemas";
            lstTemas.Size = new Size(300, 420);
            lstTemas.TabIndex = 1;
            lstTemas.SelectedIndexChanged += lstTemas_SelectedIndexChanged;
            //
            // lblTema
            //
            lblTema.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTema.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTema.ForeColor = Color.FromArgb(0, 102, 102);
            lblTema.Location = new Point(326, 32);
            lblTema.Name = "lblTema";
            lblTema.Size = new Size(522, 56);
            lblTema.TabIndex = 2;
            //
            // txtTexto
            //
            txtTexto.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtTexto.BackColor = SystemColors.Window;
            txtTexto.BorderStyle = BorderStyle.None;
            txtTexto.Font = new Font("Segoe UI", 10F);
            txtTexto.Location = new Point(326, 96);
            txtTexto.Multiline = true;
            txtTexto.Name = "txtTexto";
            txtTexto.ReadOnly = true;
            txtTexto.ScrollBars = ScrollBars.Vertical;
            txtTexto.Size = new Size(522, 362);
            txtTexto.TabIndex = 3;
            txtTexto.TabStop = false;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(738, 470);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 34);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmAyuda_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(860, 516);
            Controls.Add(btnCerrar);
            Controls.Add(txtTexto);
            Controls.Add(lblTema);
            Controls.Add(lstTemas);
            Controls.Add(lblIndice);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(700, 420);
            Name = "FrmAyuda_60MN";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ayuda en línea - VetCare";
            Load += FrmAyuda_60MN_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblIndice;
        private ListBox lstTemas;
        private Label lblTema;
        private TextBox txtTexto;
        private Button btnCerrar;
    }
}
