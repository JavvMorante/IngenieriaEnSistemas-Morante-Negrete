namespace VetCare
{
    partial class FrmAtencion_60MN
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
            lblPaciente = new Label();
            lblAlergias = new Label();
            lblProfesional = new Label();
            txtProfesional = new TextBox();
            grpSignos = new GroupBox();
            lblPeso = new Label();
            nudPeso = new NumericUpDown();
            lblTemperatura = new Label();
            nudTemperatura = new NumericUpDown();
            lblFrecuencia = new Label();
            nudFrecuencia = new NumericUpDown();
            lblMotivo = new Label();
            txtMotivo = new TextBox();
            lblDiagnostico = new Label();
            txtDiagnostico = new TextBox();
            btnRegistrar = new Button();
            btnMedicacion = new Button();
            btnInforme = new Button();
            btnCerrar = new Button();
            grpSignos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPeso).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudTemperatura).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFrecuencia).BeginInit();
            SuspendLayout();
            //
            // lblPaciente
            //
            lblPaciente.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPaciente.Location = new Point(20, 14);
            lblPaciente.Name = "lblPaciente";
            lblPaciente.Size = new Size(700, 24);
            lblPaciente.TabIndex = 0;
            //
            // lblAlergias
            //
            lblAlergias.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAlergias.ForeColor = Color.Firebrick;
            lblAlergias.Location = new Point(20, 40);
            lblAlergias.Name = "lblAlergias";
            lblAlergias.Size = new Size(700, 22);
            lblAlergias.TabIndex = 1;
            //
            // lblProfesional
            //
            lblProfesional.AutoSize = true;
            lblProfesional.Location = new Point(20, 74);
            lblProfesional.Name = "lblProfesional";
            lblProfesional.Size = new Size(84, 20);
            lblProfesional.TabIndex = 2;
            lblProfesional.Text = "Profesional";
            //
            // txtProfesional
            //
            txtProfesional.Location = new Point(150, 70);
            txtProfesional.Name = "txtProfesional";
            txtProfesional.ReadOnly = true;
            txtProfesional.Size = new Size(300, 27);
            txtProfesional.TabIndex = 3;
            txtProfesional.TabStop = false;
            //
            // grpSignos
            //
            grpSignos.Controls.Add(lblPeso);
            grpSignos.Controls.Add(nudPeso);
            grpSignos.Controls.Add(lblTemperatura);
            grpSignos.Controls.Add(nudTemperatura);
            grpSignos.Controls.Add(lblFrecuencia);
            grpSignos.Controls.Add(nudFrecuencia);
            grpSignos.Location = new Point(20, 108);
            grpSignos.Name = "grpSignos";
            grpSignos.Size = new Size(700, 74);
            grpSignos.TabIndex = 4;
            grpSignos.TabStop = false;
            grpSignos.Text = "Signos vitales";
            //
            // lblPeso
            //
            lblPeso.AutoSize = true;
            lblPeso.Location = new Point(14, 34);
            lblPeso.Name = "lblPeso";
            lblPeso.Size = new Size(70, 20);
            lblPeso.TabIndex = 0;
            lblPeso.Text = "Peso (kg)";
            //
            // nudPeso
            //
            nudPeso.DecimalPlaces = 2;
            nudPeso.Location = new Point(96, 30);
            nudPeso.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudPeso.Name = "nudPeso";
            nudPeso.Size = new Size(90, 27);
            nudPeso.TabIndex = 1;
            //
            // lblTemperatura
            //
            lblTemperatura.AutoSize = true;
            lblTemperatura.Location = new Point(212, 34);
            lblTemperatura.Name = "lblTemperatura";
            lblTemperatura.Size = new Size(121, 20);
            lblTemperatura.TabIndex = 2;
            lblTemperatura.Text = "Temperatura (°C)";
            //
            // nudTemperatura
            //
            nudTemperatura.DecimalPlaces = 1;
            nudTemperatura.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            nudTemperatura.Location = new Point(342, 30);
            nudTemperatura.Maximum = new decimal(new int[] { 45, 0, 0, 0 });
            nudTemperatura.Minimum = new decimal(new int[] { 30, 0, 0, 0 });
            nudTemperatura.Name = "nudTemperatura";
            nudTemperatura.Size = new Size(80, 27);
            nudTemperatura.TabIndex = 3;
            nudTemperatura.Value = new decimal(new int[] { 385, 0, 0, 65536 });
            //
            // lblFrecuencia
            //
            lblFrecuencia.AutoSize = true;
            lblFrecuencia.Location = new Point(450, 34);
            lblFrecuencia.Name = "lblFrecuencia";
            lblFrecuencia.Size = new Size(118, 20);
            lblFrecuencia.TabIndex = 4;
            lblFrecuencia.Text = "Frec. card. (lpm)";
            //
            // nudFrecuencia
            //
            nudFrecuencia.Location = new Point(586, 30);
            nudFrecuencia.Maximum = new decimal(new int[] { 400, 0, 0, 0 });
            nudFrecuencia.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            nudFrecuencia.Name = "nudFrecuencia";
            nudFrecuencia.Size = new Size(90, 27);
            nudFrecuencia.TabIndex = 5;
            nudFrecuencia.Value = new decimal(new int[] { 100, 0, 0, 0 });
            //
            // lblMotivo
            //
            lblMotivo.AutoSize = true;
            lblMotivo.Location = new Point(20, 194);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.Size = new Size(149, 20);
            lblMotivo.TabIndex = 5;
            lblMotivo.Text = "Motivo de consulta *";
            //
            // txtMotivo
            //
            txtMotivo.Location = new Point(20, 218);
            txtMotivo.MaxLength = 400;
            txtMotivo.Multiline = true;
            txtMotivo.Name = "txtMotivo";
            txtMotivo.ScrollBars = ScrollBars.Vertical;
            txtMotivo.Size = new Size(700, 64);
            txtMotivo.TabIndex = 6;
            //
            // lblDiagnostico
            //
            lblDiagnostico.AutoSize = true;
            lblDiagnostico.Location = new Point(20, 292);
            lblDiagnostico.Name = "lblDiagnostico";
            lblDiagnostico.Size = new Size(203, 20);
            lblDiagnostico.TabIndex = 7;
            lblDiagnostico.Text = "Diagnóstico e indicaciones *";
            //
            // txtDiagnostico
            //
            txtDiagnostico.Location = new Point(20, 316);
            txtDiagnostico.MaxLength = 2000;
            txtDiagnostico.Multiline = true;
            txtDiagnostico.Name = "txtDiagnostico";
            txtDiagnostico.ScrollBars = ScrollBars.Vertical;
            txtDiagnostico.Size = new Size(700, 110);
            txtDiagnostico.TabIndex = 8;
            //
            // btnRegistrar
            //
            btnRegistrar.Location = new Point(20, 442);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(160, 36);
            btnRegistrar.TabIndex = 9;
            btnRegistrar.Tag = "ATN_REGISTRAR";
            btnRegistrar.Text = "Registrar atención";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;
            //
            // btnMedicacion
            //
            btnMedicacion.Enabled = false;
            btnMedicacion.Location = new Point(190, 442);
            btnMedicacion.Name = "btnMedicacion";
            btnMedicacion.Size = new Size(180, 36);
            btnMedicacion.TabIndex = 10;
            btnMedicacion.Tag = "VEN_CARRITO";
            btnMedicacion.Text = "Agregar medicación";
            btnMedicacion.UseVisualStyleBackColor = true;
            btnMedicacion.Click += btnMedicacion_Click;
            //
            // btnInforme
            //
            btnInforme.Enabled = false;
            btnInforme.Location = new Point(380, 442);
            btnInforme.Name = "btnInforme";
            btnInforme.Size = new Size(180, 36);
            btnInforme.TabIndex = 11;
            btnInforme.Tag = "ATN_INFORME";
            btnInforme.Text = "Emitir informe";
            btnInforme.UseVisualStyleBackColor = true;
            btnInforme.Click += btnInforme_Click;
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(610, 442);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 36);
            btnCerrar.TabIndex = 12;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmAtencion_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(740, 494);
            Controls.Add(btnCerrar);
            Controls.Add(btnInforme);
            Controls.Add(btnMedicacion);
            Controls.Add(btnRegistrar);
            Controls.Add(txtDiagnostico);
            Controls.Add(lblDiagnostico);
            Controls.Add(txtMotivo);
            Controls.Add(lblMotivo);
            Controls.Add(grpSignos);
            Controls.Add(txtProfesional);
            Controls.Add(lblProfesional);
            Controls.Add(lblAlergias);
            Controls.Add(lblPaciente);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAtencion_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar atención";
            Load += FrmAtencion_60MN_Load;
            grpSignos.ResumeLayout(false);
            grpSignos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPeso).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudTemperatura).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFrecuencia).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPaciente;
        private Label lblAlergias;
        private Label lblProfesional;
        private TextBox txtProfesional;
        private GroupBox grpSignos;
        private Label lblPeso;
        private NumericUpDown nudPeso;
        private Label lblTemperatura;
        private NumericUpDown nudTemperatura;
        private Label lblFrecuencia;
        private NumericUpDown nudFrecuencia;
        private Label lblMotivo;
        private TextBox txtMotivo;
        private Label lblDiagnostico;
        private TextBox txtDiagnostico;
        private Button btnRegistrar;
        private Button btnMedicacion;
        private Button btnInforme;
        private Button btnCerrar;
    }
}
