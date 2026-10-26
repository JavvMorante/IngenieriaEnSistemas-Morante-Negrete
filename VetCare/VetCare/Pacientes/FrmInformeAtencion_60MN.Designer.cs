namespace VetCare
{
    partial class FrmInformeAtencion_60MN
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
            txtInforme = new TextBox();
            btnImprimir = new Button();
            btnGuardar = new Button();
            btnCerrar = new Button();
            SuspendLayout();
            //
            // txtInforme
            //
            txtInforme.BackColor = SystemColors.Window;
            txtInforme.Font = new Font("Consolas", 10F);
            txtInforme.Location = new Point(16, 16);
            txtInforme.Multiline = true;
            txtInforme.Name = "txtInforme";
            txtInforme.ReadOnly = true;
            txtInforme.ScrollBars = ScrollBars.Both;
            txtInforme.Size = new Size(668, 470);
            txtInforme.TabIndex = 0;
            txtInforme.WordWrap = false;
            //
            // btnImprimir
            //
            btnImprimir.Location = new Point(16, 500);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(190, 34);
            btnImprimir.TabIndex = 1;
            btnImprimir.Text = "Imprimir / PDF";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(216, 500);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(190, 34);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "Guardar como texto";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(574, 500);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 34);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmInformeAtencion_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(700, 550);
            Controls.Add(btnCerrar);
            Controls.Add(btnGuardar);
            Controls.Add(btnImprimir);
            Controls.Add(txtInforme);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmInformeAtencion_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Informe de atención";
            Load += FrmInformeAtencion_60MN_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtInforme;
        private Button btnImprimir;
        private Button btnGuardar;
        private Button btnCerrar;
    }
}
