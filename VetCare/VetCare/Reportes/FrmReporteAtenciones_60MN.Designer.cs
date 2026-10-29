namespace VetCare
{
    partial class FrmReporteAtenciones_60MN
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
            DataGridViewCellStyle estiloFecha = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloNumero = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloNumero2 = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloPorcentaje = new DataGridViewCellStyle();
            grpFiltros = new GroupBox();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            lblProfesional = new Label();
            cboProfesional = new ComboBox();
            lblEspecie = new Label();
            cboEspecie = new ComboBox();
            btnGenerar = new Button();
            dgvAtenciones = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colHistoria = new DataGridViewTextBoxColumn();
            colPaciente = new DataGridViewTextBoxColumn();
            colEspecie = new DataGridViewTextBoxColumn();
            colPropietario = new DataGridViewTextBoxColumn();
            colProfesional = new DataGridViewTextBoxColumn();
            colMotivo = new DataGridViewTextBoxColumn();
            lblPorProfesional = new Label();
            dgvProfesionales = new DataGridView();
            colProfesionalTotal = new DataGridViewTextBoxColumn();
            colCantidadAtenciones = new DataGridViewTextBoxColumn();
            colPacientes = new DataGridViewTextBoxColumn();
            colPorcentaje = new DataGridViewTextBoxColumn();
            lblResumen = new Label();
            btnImprimir = new Button();
            btnExportar = new Button();
            btnCerrar = new Button();
            grpFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAtenciones).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProfesionales).BeginInit();
            SuspendLayout();
            //
            // grpFiltros
            //
            grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpFiltros.Controls.Add(lblDesde);
            grpFiltros.Controls.Add(dtpDesde);
            grpFiltros.Controls.Add(lblHasta);
            grpFiltros.Controls.Add(dtpHasta);
            grpFiltros.Controls.Add(lblProfesional);
            grpFiltros.Controls.Add(cboProfesional);
            grpFiltros.Controls.Add(lblEspecie);
            grpFiltros.Controls.Add(cboEspecie);
            grpFiltros.Controls.Add(btnGenerar);
            grpFiltros.Location = new Point(12, 8);
            grpFiltros.Name = "grpFiltros";
            grpFiltros.Size = new Size(1076, 72);
            grpFiltros.TabIndex = 0;
            grpFiltros.TabStop = false;
            grpFiltros.Text = "Filtros";
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(14, 32);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(50, 20);
            lblDesde.TabIndex = 0;
            lblDesde.Text = "Desde";
            //
            // dtpDesde
            //
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(72, 28);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(125, 27);
            dtpDesde.TabIndex = 1;
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(208, 32);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(46, 20);
            lblHasta.TabIndex = 2;
            lblHasta.Text = "hasta";
            //
            // dtpHasta
            //
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(260, 28);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(125, 27);
            dtpHasta.TabIndex = 3;
            //
            // lblProfesional
            //
            lblProfesional.AutoSize = true;
            lblProfesional.Location = new Point(400, 32);
            lblProfesional.Name = "lblProfesional";
            lblProfesional.Size = new Size(85, 20);
            lblProfesional.TabIndex = 4;
            lblProfesional.Text = "Veterinario";
            //
            // cboProfesional
            //
            cboProfesional.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProfesional.FormattingEnabled = true;
            cboProfesional.Location = new Point(490, 28);
            cboProfesional.Name = "cboProfesional";
            cboProfesional.Size = new Size(200, 28);
            cboProfesional.TabIndex = 5;
            //
            // lblEspecie
            //
            lblEspecie.AutoSize = true;
            lblEspecie.Location = new Point(706, 32);
            lblEspecie.Name = "lblEspecie";
            lblEspecie.Size = new Size(59, 20);
            lblEspecie.TabIndex = 6;
            lblEspecie.Text = "Especie";
            //
            // cboEspecie
            //
            cboEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEspecie.FormattingEnabled = true;
            cboEspecie.Location = new Point(770, 28);
            cboEspecie.Name = "cboEspecie";
            cboEspecie.Size = new Size(140, 28);
            cboEspecie.TabIndex = 7;
            //
            // btnGenerar
            //
            btnGenerar.Location = new Point(924, 26);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(140, 34);
            btnGenerar.TabIndex = 8;
            btnGenerar.Text = "Generar";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            //
            // dgvAtenciones
            //
            dgvAtenciones.AllowUserToAddRows = false;
            dgvAtenciones.AllowUserToDeleteRows = false;
            dgvAtenciones.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAtenciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAtenciones.BackgroundColor = SystemColors.Window;
            dgvAtenciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAtenciones.Columns.AddRange(new DataGridViewColumn[] { colFecha, colHistoria, colPaciente, colEspecie, colPropietario, colProfesional, colMotivo });
            dgvAtenciones.Location = new Point(12, 90);
            dgvAtenciones.MultiSelect = false;
            dgvAtenciones.Name = "dgvAtenciones";
            dgvAtenciones.ReadOnly = true;
            dgvAtenciones.RowHeadersVisible = false;
            dgvAtenciones.RowHeadersWidth = 51;
            dgvAtenciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAtenciones.Size = new Size(1076, 300);
            dgvAtenciones.TabIndex = 1;
            //
            // colFecha
            //
            colFecha.DataPropertyName = "FechaHora";
            estiloFecha.Format = "dd/MM/yyyy HH:mm";
            colFecha.DefaultCellStyle = estiloFecha;
            colFecha.FillWeight = 80F;
            colFecha.HeaderText = "Fecha";
            colFecha.MinimumWidth = 6;
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colHistoria
            //
            colHistoria.DataPropertyName = "NumeroHistoriaClinica";
            colHistoria.FillWeight = 60F;
            colHistoria.HeaderText = "Historia clínica";
            colHistoria.MinimumWidth = 6;
            colHistoria.Name = "colHistoria";
            colHistoria.ReadOnly = true;
            //
            // colPaciente
            //
            colPaciente.DataPropertyName = "Paciente";
            colPaciente.FillWeight = 70F;
            colPaciente.HeaderText = "Paciente";
            colPaciente.MinimumWidth = 6;
            colPaciente.Name = "colPaciente";
            colPaciente.ReadOnly = true;
            //
            // colEspecie
            //
            colEspecie.DataPropertyName = "Especie";
            colEspecie.FillWeight = 60F;
            colEspecie.HeaderText = "Especie";
            colEspecie.MinimumWidth = 6;
            colEspecie.Name = "colEspecie";
            colEspecie.ReadOnly = true;
            //
            // colPropietario
            //
            colPropietario.DataPropertyName = "Propietario";
            colPropietario.HeaderText = "Propietario";
            colPropietario.MinimumWidth = 6;
            colPropietario.Name = "colPropietario";
            colPropietario.ReadOnly = true;
            //
            // colProfesional
            //
            colProfesional.DataPropertyName = "Profesional";
            colProfesional.HeaderText = "Profesional";
            colProfesional.MinimumWidth = 6;
            colProfesional.Name = "colProfesional";
            colProfesional.ReadOnly = true;
            //
            // colMotivo
            //
            colMotivo.DataPropertyName = "Motivo";
            colMotivo.FillWeight = 160F;
            colMotivo.HeaderText = "Motivo";
            colMotivo.MinimumWidth = 6;
            colMotivo.Name = "colMotivo";
            colMotivo.ReadOnly = true;
            //
            // lblPorProfesional
            //
            lblPorProfesional.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblPorProfesional.AutoSize = true;
            lblPorProfesional.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPorProfesional.Location = new Point(12, 398);
            lblPorProfesional.Name = "lblPorProfesional";
            lblPorProfesional.Size = new Size(181, 20);
            lblPorProfesional.TabIndex = 2;
            lblPorProfesional.Text = "Atenciones por profesional";
            //
            // dgvProfesionales
            //
            dgvProfesionales.AllowUserToAddRows = false;
            dgvProfesionales.AllowUserToDeleteRows = false;
            dgvProfesionales.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProfesionales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProfesionales.BackgroundColor = SystemColors.Window;
            dgvProfesionales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProfesionales.Columns.AddRange(new DataGridViewColumn[] { colProfesionalTotal, colCantidadAtenciones, colPacientes, colPorcentaje });
            dgvProfesionales.Location = new Point(12, 422);
            dgvProfesionales.MultiSelect = false;
            dgvProfesionales.Name = "dgvProfesionales";
            dgvProfesionales.ReadOnly = true;
            dgvProfesionales.RowHeadersVisible = false;
            dgvProfesionales.RowHeadersWidth = 51;
            dgvProfesionales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProfesionales.Size = new Size(1076, 140);
            dgvProfesionales.TabIndex = 3;
            dgvProfesionales.TabStop = false;
            //
            // colProfesionalTotal
            //
            colProfesionalTotal.DataPropertyName = "Profesional";
            colProfesionalTotal.FillWeight = 200F;
            colProfesionalTotal.HeaderText = "Profesional";
            colProfesionalTotal.MinimumWidth = 6;
            colProfesionalTotal.Name = "colProfesionalTotal";
            colProfesionalTotal.ReadOnly = true;
            //
            // colCantidadAtenciones
            //
            colCantidadAtenciones.DataPropertyName = "Atenciones";
            estiloNumero.Alignment = DataGridViewContentAlignment.MiddleRight;
            colCantidadAtenciones.DefaultCellStyle = estiloNumero;
            colCantidadAtenciones.HeaderText = "Atenciones";
            colCantidadAtenciones.MinimumWidth = 6;
            colCantidadAtenciones.Name = "colCantidadAtenciones";
            colCantidadAtenciones.ReadOnly = true;
            //
            // colPacientes
            //
            colPacientes.DataPropertyName = "Pacientes";
            estiloNumero2.Alignment = DataGridViewContentAlignment.MiddleRight;
            colPacientes.DefaultCellStyle = estiloNumero2;
            colPacientes.HeaderText = "Pacientes";
            colPacientes.MinimumWidth = 6;
            colPacientes.Name = "colPacientes";
            colPacientes.ReadOnly = true;
            //
            // colPorcentaje
            //
            colPorcentaje.DataPropertyName = "Porcentaje";
            estiloPorcentaje.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloPorcentaje.Format = "0.0 '%'";
            colPorcentaje.DefaultCellStyle = estiloPorcentaje;
            colPorcentaje.HeaderText = "% del total";
            colPorcentaje.MinimumWidth = 6;
            colPorcentaje.Name = "colPorcentaje";
            colPorcentaje.ReadOnly = true;
            //
            // lblResumen
            //
            lblResumen.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResumen.Location = new Point(12, 576);
            lblResumen.Name = "lblResumen";
            lblResumen.Size = new Size(620, 26);
            lblResumen.TabIndex = 4;
            lblResumen.Text = "Elija el período y presione Generar.";
            //
            // btnImprimir
            //
            btnImprimir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnImprimir.Enabled = false;
            btnImprimir.Location = new Point(658, 572);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(150, 34);
            btnImprimir.TabIndex = 5;
            btnImprimir.Text = "Imprimir / PDF";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            //
            // btnExportar
            //
            btnExportar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExportar.Enabled = false;
            btnExportar.Location = new Point(818, 572);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(150, 34);
            btnExportar.TabIndex = 6;
            btnExportar.Text = "Exportar CSV";
            btnExportar.UseVisualStyleBackColor = true;
            btnExportar.Click += btnExportar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.Location = new Point(978, 572);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 34);
            btnCerrar.TabIndex = 7;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmReporteAtenciones_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 618);
            Controls.Add(btnCerrar);
            Controls.Add(btnExportar);
            Controls.Add(btnImprimir);
            Controls.Add(lblResumen);
            Controls.Add(dgvProfesionales);
            Controls.Add(lblPorProfesional);
            Controls.Add(dgvAtenciones);
            Controls.Add(grpFiltros);
            MinimumSize = new Size(880, 560);
            Name = "FrmReporteAtenciones_60MN";
            Text = "Reporte de atenciones";
            Load += FrmReporteAtenciones_60MN_Load;
            grpFiltros.ResumeLayout(false);
            grpFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAtenciones).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProfesionales).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpFiltros;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Label lblProfesional;
        private ComboBox cboProfesional;
        private Label lblEspecie;
        private ComboBox cboEspecie;
        private Button btnGenerar;
        private DataGridView dgvAtenciones;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colHistoria;
        private DataGridViewTextBoxColumn colPaciente;
        private DataGridViewTextBoxColumn colEspecie;
        private DataGridViewTextBoxColumn colPropietario;
        private DataGridViewTextBoxColumn colProfesional;
        private DataGridViewTextBoxColumn colMotivo;
        private Label lblPorProfesional;
        private DataGridView dgvProfesionales;
        private DataGridViewTextBoxColumn colProfesionalTotal;
        private DataGridViewTextBoxColumn colCantidadAtenciones;
        private DataGridViewTextBoxColumn colPacientes;
        private DataGridViewTextBoxColumn colPorcentaje;
        private Label lblResumen;
        private Button btnImprimir;
        private Button btnExportar;
        private Button btnCerrar;
    }
}
