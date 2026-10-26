namespace VetCare
{
    partial class FrmHistoriaClinica_60MN
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
            DataGridViewCellStyle estiloFechaEvolucion = new DataGridViewCellStyle();
            grpPaciente = new GroupBox();
            lblPaciente = new Label();
            lblNumero = new Label();
            lblEspecie = new Label();
            txtEspecie = new TextBox();
            lblRaza = new Label();
            txtRaza = new TextBox();
            lblNacimiento = new Label();
            dtpNacimiento = new DateTimePicker();
            lblPropietario = new Label();
            grpDatos = new GroupBox();
            lblPeso = new Label();
            nudPeso = new NumericUpDown();
            chkSinAlergias = new CheckBox();
            lblAlergias = new Label();
            txtAlergias = new TextBox();
            lblObservaciones = new Label();
            txtObservaciones = new TextBox();
            lblAlertaAlergias = new Label();
            btnModificar = new Button();
            btnGuardar = new Button();
            btnCancelarEdicion = new Button();
            btnNuevaAtencion = new Button();
            btnImprimir = new Button();
            btnCerrar = new Button();
            tabHistoria = new TabControl();
            tpAtenciones = new TabPage();
            dgvAtenciones = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colProfesional = new DataGridViewTextBoxColumn();
            colMotivo = new DataGridViewTextBoxColumn();
            colPeso = new DataGridViewTextBoxColumn();
            txtDetalle = new TextBox();
            btnVerInforme = new Button();
            btnMedicacion = new Button();
            tpEvolucion = new TabPage();
            dgvEvolucion = new DataGridView();
            colFechaEvolucion = new DataGridViewTextBoxColumn();
            colOrigen = new DataGridViewTextBoxColumn();
            colPesoEvolucion = new DataGridViewTextBoxColumn();
            colAlergiasEvolucion = new DataGridViewTextBoxColumn();
            colObservacionesEvolucion = new DataGridViewTextBoxColumn();
            colUsuarioEvolucion = new DataGridViewTextBoxColumn();
            grpPaciente.SuspendLayout();
            grpDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPeso).BeginInit();
            tabHistoria.SuspendLayout();
            tpAtenciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAtenciones).BeginInit();
            tpEvolucion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEvolucion).BeginInit();
            SuspendLayout();
            //
            // grpPaciente
            //
            grpPaciente.Controls.Add(lblPaciente);
            grpPaciente.Controls.Add(lblNumero);
            grpPaciente.Controls.Add(lblEspecie);
            grpPaciente.Controls.Add(txtEspecie);
            grpPaciente.Controls.Add(lblRaza);
            grpPaciente.Controls.Add(txtRaza);
            grpPaciente.Controls.Add(lblNacimiento);
            grpPaciente.Controls.Add(dtpNacimiento);
            grpPaciente.Controls.Add(lblPropietario);
            grpPaciente.Location = new Point(12, 8);
            grpPaciente.Name = "grpPaciente";
            grpPaciente.Size = new Size(976, 126);
            grpPaciente.TabIndex = 0;
            grpPaciente.TabStop = false;
            grpPaciente.Text = "Paciente";
            //
            // lblPaciente
            //
            lblPaciente.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPaciente.Location = new Point(14, 26);
            lblPaciente.Name = "lblPaciente";
            lblPaciente.Size = new Size(560, 24);
            lblPaciente.TabIndex = 0;
            //
            // lblNumero
            //
            lblNumero.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNumero.ForeColor = Color.FromArgb(0, 102, 102);
            lblNumero.Location = new Point(580, 26);
            lblNumero.Name = "lblNumero";
            lblNumero.Size = new Size(380, 24);
            lblNumero.TabIndex = 1;
            lblNumero.TextAlign = ContentAlignment.TopRight;
            //
            // lblEspecie
            //
            lblEspecie.AutoSize = true;
            lblEspecie.Location = new Point(14, 62);
            lblEspecie.Name = "lblEspecie";
            lblEspecie.Size = new Size(59, 20);
            lblEspecie.TabIndex = 2;
            lblEspecie.Text = "Especie";
            //
            // txtEspecie
            //
            txtEspecie.Location = new Point(90, 58);
            txtEspecie.MaxLength = 40;
            txtEspecie.Name = "txtEspecie";
            txtEspecie.ReadOnly = true;
            txtEspecie.Size = new Size(150, 27);
            txtEspecie.TabIndex = 3;
            //
            // lblRaza
            //
            lblRaza.AutoSize = true;
            lblRaza.Location = new Point(256, 62);
            lblRaza.Name = "lblRaza";
            lblRaza.Size = new Size(41, 20);
            lblRaza.TabIndex = 4;
            lblRaza.Text = "Raza";
            //
            // txtRaza
            //
            txtRaza.Location = new Point(310, 58);
            txtRaza.MaxLength = 60;
            txtRaza.Name = "txtRaza";
            txtRaza.ReadOnly = true;
            txtRaza.Size = new Size(170, 27);
            txtRaza.TabIndex = 5;
            //
            // lblNacimiento
            //
            lblNacimiento.AutoSize = true;
            lblNacimiento.Location = new Point(496, 62);
            lblNacimiento.Name = "lblNacimiento";
            lblNacimiento.Size = new Size(85, 20);
            lblNacimiento.TabIndex = 6;
            lblNacimiento.Text = "Nacimiento";
            //
            // dtpNacimiento
            //
            dtpNacimiento.Enabled = false;
            dtpNacimiento.Format = DateTimePickerFormat.Short;
            dtpNacimiento.Location = new Point(590, 58);
            dtpNacimiento.Name = "dtpNacimiento";
            dtpNacimiento.Size = new Size(130, 27);
            dtpNacimiento.TabIndex = 7;
            //
            // lblPropietario
            //
            lblPropietario.Location = new Point(14, 96);
            lblPropietario.Name = "lblPropietario";
            lblPropietario.Size = new Size(946, 22);
            lblPropietario.TabIndex = 8;
            //
            // grpDatos
            //
            grpDatos.Controls.Add(lblPeso);
            grpDatos.Controls.Add(nudPeso);
            grpDatos.Controls.Add(chkSinAlergias);
            grpDatos.Controls.Add(lblAlergias);
            grpDatos.Controls.Add(txtAlergias);
            grpDatos.Controls.Add(lblObservaciones);
            grpDatos.Controls.Add(txtObservaciones);
            grpDatos.Controls.Add(lblAlertaAlergias);
            grpDatos.Location = new Point(12, 140);
            grpDatos.Name = "grpDatos";
            grpDatos.Size = new Size(976, 150);
            grpDatos.TabIndex = 1;
            grpDatos.TabStop = false;
            grpDatos.Text = "Datos clínicos";
            //
            // lblPeso
            //
            lblPeso.AutoSize = true;
            lblPeso.Location = new Point(14, 32);
            lblPeso.Name = "lblPeso";
            lblPeso.Size = new Size(130, 20);
            lblPeso.TabIndex = 0;
            lblPeso.Text = "Peso actual (kg) *";
            //
            // nudPeso
            //
            nudPeso.DecimalPlaces = 2;
            nudPeso.Location = new Point(160, 28);
            nudPeso.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudPeso.Name = "nudPeso";
            nudPeso.Size = new Size(100, 27);
            nudPeso.TabIndex = 1;
            //
            // chkSinAlergias
            //
            chkSinAlergias.AutoSize = true;
            chkSinAlergias.Location = new Point(290, 30);
            chkSinAlergias.Name = "chkSinAlergias";
            chkSinAlergias.Size = new Size(197, 24);
            chkSinAlergias.TabIndex = 2;
            chkSinAlergias.Text = "Sin alergias registradas";
            chkSinAlergias.UseVisualStyleBackColor = true;
            chkSinAlergias.CheckedChanged += chkSinAlergias_CheckedChanged;
            //
            // lblAlergias
            //
            lblAlergias.AutoSize = true;
            lblAlergias.Location = new Point(14, 72);
            lblAlergias.Name = "lblAlergias";
            lblAlergias.Size = new Size(73, 20);
            lblAlergias.TabIndex = 3;
            lblAlergias.Text = "Alergias *";
            //
            // txtAlergias
            //
            txtAlergias.Location = new Point(160, 68);
            txtAlergias.MaxLength = 400;
            txtAlergias.Name = "txtAlergias";
            txtAlergias.Size = new Size(560, 27);
            txtAlergias.TabIndex = 4;
            //
            // lblObservaciones
            //
            lblObservaciones.AutoSize = true;
            lblObservaciones.Location = new Point(14, 110);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(105, 20);
            lblObservaciones.TabIndex = 5;
            lblObservaciones.Text = "Observaciones";
            //
            // txtObservaciones
            //
            txtObservaciones.Location = new Point(160, 106);
            txtObservaciones.MaxLength = 1000;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Size = new Size(800, 27);
            txtObservaciones.TabIndex = 6;
            //
            // lblAlertaAlergias
            //
            lblAlertaAlergias.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAlertaAlergias.ForeColor = Color.Firebrick;
            lblAlertaAlergias.Location = new Point(736, 26);
            lblAlertaAlergias.Name = "lblAlertaAlergias";
            lblAlertaAlergias.Size = new Size(224, 72);
            lblAlertaAlergias.TabIndex = 7;
            //
            // btnModificar
            //
            btnModificar.Location = new Point(12, 298);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(120, 34);
            btnModificar.TabIndex = 2;
            btnModificar.Tag = "HC_MODIF";
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(138, 298);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(170, 34);
            btnGuardar.TabIndex = 3;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelarEdicion
            //
            btnCancelarEdicion.Location = new Point(314, 298);
            btnCancelarEdicion.Name = "btnCancelarEdicion";
            btnCancelarEdicion.Size = new Size(110, 34);
            btnCancelarEdicion.TabIndex = 4;
            btnCancelarEdicion.Text = "Cancelar";
            btnCancelarEdicion.UseVisualStyleBackColor = true;
            btnCancelarEdicion.Click += btnCancelarEdicion_Click;
            //
            // btnNuevaAtencion
            //
            btnNuevaAtencion.Location = new Point(560, 298);
            btnNuevaAtencion.Name = "btnNuevaAtencion";
            btnNuevaAtencion.Size = new Size(140, 34);
            btnNuevaAtencion.TabIndex = 5;
            btnNuevaAtencion.Tag = "ATN_REGISTRAR";
            btnNuevaAtencion.Text = "Nueva atención";
            btnNuevaAtencion.UseVisualStyleBackColor = true;
            btnNuevaAtencion.Click += btnNuevaAtencion_Click;
            //
            // btnImprimir
            //
            btnImprimir.Location = new Point(706, 298);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(160, 34);
            btnImprimir.TabIndex = 6;
            btnImprimir.Tag = "HC_CONSULTA";
            btnImprimir.Text = "Imprimir historia";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(878, 298);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 34);
            btnCerrar.TabIndex = 7;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // tabHistoria
            //
            tabHistoria.Controls.Add(tpAtenciones);
            tabHistoria.Controls.Add(tpEvolucion);
            tabHistoria.Location = new Point(12, 342);
            tabHistoria.Name = "tabHistoria";
            tabHistoria.SelectedIndex = 0;
            tabHistoria.Size = new Size(976, 336);
            tabHistoria.TabIndex = 8;
            //
            // tpAtenciones
            //
            tpAtenciones.Controls.Add(dgvAtenciones);
            tpAtenciones.Controls.Add(txtDetalle);
            tpAtenciones.Controls.Add(btnVerInforme);
            tpAtenciones.Controls.Add(btnMedicacion);
            tpAtenciones.Location = new Point(4, 29);
            tpAtenciones.Name = "tpAtenciones";
            tpAtenciones.Padding = new Padding(3);
            tpAtenciones.Size = new Size(968, 303);
            tpAtenciones.TabIndex = 0;
            tpAtenciones.Text = "Atenciones";
            tpAtenciones.UseVisualStyleBackColor = true;
            //
            // dgvAtenciones
            //
            dgvAtenciones.AllowUserToAddRows = false;
            dgvAtenciones.AllowUserToDeleteRows = false;
            dgvAtenciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAtenciones.BackgroundColor = SystemColors.Window;
            dgvAtenciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAtenciones.Columns.AddRange(new DataGridViewColumn[] { colFecha, colProfesional, colMotivo, colPeso });
            dgvAtenciones.Location = new Point(6, 6);
            dgvAtenciones.MultiSelect = false;
            dgvAtenciones.Name = "dgvAtenciones";
            dgvAtenciones.ReadOnly = true;
            dgvAtenciones.RowHeadersVisible = false;
            dgvAtenciones.RowHeadersWidth = 51;
            dgvAtenciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAtenciones.Size = new Size(560, 290);
            dgvAtenciones.TabIndex = 0;
            dgvAtenciones.SelectionChanged += dgvAtenciones_SelectionChanged;
            //
            // colFecha
            //
            colFecha.DataPropertyName = "FechaHora";
            estiloFecha.Format = "dd/MM/yyyy HH:mm";
            colFecha.DefaultCellStyle = estiloFecha;
            colFecha.HeaderText = "Fecha y hora";
            colFecha.MinimumWidth = 6;
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
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
            colMotivo.FillWeight = 140F;
            colMotivo.HeaderText = "Motivo";
            colMotivo.MinimumWidth = 6;
            colMotivo.Name = "colMotivo";
            colMotivo.ReadOnly = true;
            //
            // colPeso
            //
            colPeso.DataPropertyName = "Peso";
            colPeso.FillWeight = 50F;
            colPeso.HeaderText = "Peso (kg)";
            colPeso.MinimumWidth = 6;
            colPeso.Name = "colPeso";
            colPeso.ReadOnly = true;
            //
            // txtDetalle
            //
            txtDetalle.Location = new Point(572, 6);
            txtDetalle.Multiline = true;
            txtDetalle.Name = "txtDetalle";
            txtDetalle.ReadOnly = true;
            txtDetalle.ScrollBars = ScrollBars.Vertical;
            txtDetalle.Size = new Size(390, 246);
            txtDetalle.TabIndex = 1;
            //
            // btnVerInforme
            //
            btnVerInforme.Location = new Point(572, 260);
            btnVerInforme.Name = "btnVerInforme";
            btnVerInforme.Size = new Size(190, 34);
            btnVerInforme.TabIndex = 2;
            btnVerInforme.Tag = "ATN_INFORME";
            btnVerInforme.Text = "Ver / imprimir informe";
            btnVerInforme.UseVisualStyleBackColor = true;
            btnVerInforme.Click += btnVerInforme_Click;
            //
            // btnMedicacion
            //
            btnMedicacion.Location = new Point(772, 260);
            btnMedicacion.Name = "btnMedicacion";
            btnMedicacion.Size = new Size(190, 34);
            btnMedicacion.TabIndex = 3;
            btnMedicacion.Tag = "VEN_CARRITO";
            btnMedicacion.Text = "Agregar medicación";
            btnMedicacion.UseVisualStyleBackColor = true;
            btnMedicacion.Click += btnMedicacion_Click;
            //
            // tpEvolucion
            //
            tpEvolucion.Controls.Add(dgvEvolucion);
            tpEvolucion.Location = new Point(4, 29);
            tpEvolucion.Name = "tpEvolucion";
            tpEvolucion.Padding = new Padding(3);
            tpEvolucion.Size = new Size(968, 303);
            tpEvolucion.TabIndex = 1;
            tpEvolucion.Text = "Evolución (valores anteriores)";
            tpEvolucion.UseVisualStyleBackColor = true;
            //
            // dgvEvolucion
            //
            dgvEvolucion.AllowUserToAddRows = false;
            dgvEvolucion.AllowUserToDeleteRows = false;
            dgvEvolucion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEvolucion.BackgroundColor = SystemColors.Window;
            dgvEvolucion.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEvolucion.Columns.AddRange(new DataGridViewColumn[] { colFechaEvolucion, colOrigen, colPesoEvolucion, colAlergiasEvolucion, colObservacionesEvolucion, colUsuarioEvolucion });
            dgvEvolucion.Dock = DockStyle.Fill;
            dgvEvolucion.Location = new Point(3, 3);
            dgvEvolucion.Name = "dgvEvolucion";
            dgvEvolucion.ReadOnly = true;
            dgvEvolucion.RowHeadersVisible = false;
            dgvEvolucion.RowHeadersWidth = 51;
            dgvEvolucion.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvolucion.Size = new Size(962, 297);
            dgvEvolucion.TabIndex = 0;
            //
            // colFechaEvolucion
            //
            colFechaEvolucion.DataPropertyName = "FechaHora";
            estiloFechaEvolucion.Format = "dd/MM/yyyy HH:mm";
            colFechaEvolucion.DefaultCellStyle = estiloFechaEvolucion;
            colFechaEvolucion.HeaderText = "Fecha y hora";
            colFechaEvolucion.MinimumWidth = 6;
            colFechaEvolucion.Name = "colFechaEvolucion";
            colFechaEvolucion.ReadOnly = true;
            //
            // colOrigen
            //
            colOrigen.DataPropertyName = "Origen";
            colOrigen.FillWeight = 70F;
            colOrigen.HeaderText = "Origen";
            colOrigen.MinimumWidth = 6;
            colOrigen.Name = "colOrigen";
            colOrigen.ReadOnly = true;
            //
            // colPesoEvolucion
            //
            colPesoEvolucion.DataPropertyName = "PesoActual";
            colPesoEvolucion.FillWeight = 50F;
            colPesoEvolucion.HeaderText = "Peso (kg)";
            colPesoEvolucion.MinimumWidth = 6;
            colPesoEvolucion.Name = "colPesoEvolucion";
            colPesoEvolucion.ReadOnly = true;
            //
            // colAlergiasEvolucion
            //
            colAlergiasEvolucion.DataPropertyName = "Alergias";
            colAlergiasEvolucion.HeaderText = "Alergias";
            colAlergiasEvolucion.MinimumWidth = 6;
            colAlergiasEvolucion.Name = "colAlergiasEvolucion";
            colAlergiasEvolucion.ReadOnly = true;
            //
            // colObservacionesEvolucion
            //
            colObservacionesEvolucion.DataPropertyName = "Observaciones";
            colObservacionesEvolucion.FillWeight = 140F;
            colObservacionesEvolucion.HeaderText = "Observaciones";
            colObservacionesEvolucion.MinimumWidth = 6;
            colObservacionesEvolucion.Name = "colObservacionesEvolucion";
            colObservacionesEvolucion.ReadOnly = true;
            //
            // colUsuarioEvolucion
            //
            colUsuarioEvolucion.DataPropertyName = "Usuario";
            colUsuarioEvolucion.FillWeight = 60F;
            colUsuarioEvolucion.HeaderText = "Usuario";
            colUsuarioEvolucion.MinimumWidth = 6;
            colUsuarioEvolucion.Name = "colUsuarioEvolucion";
            colUsuarioEvolucion.ReadOnly = true;
            //
            // FrmHistoriaClinica_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1000, 690);
            Controls.Add(tabHistoria);
            Controls.Add(btnCerrar);
            Controls.Add(btnImprimir);
            Controls.Add(btnNuevaAtencion);
            Controls.Add(btnCancelarEdicion);
            Controls.Add(btnGuardar);
            Controls.Add(btnModificar);
            Controls.Add(grpDatos);
            Controls.Add(grpPaciente);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmHistoriaClinica_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Historia clínica";
            Load += FrmHistoriaClinica_60MN_Load;
            grpPaciente.ResumeLayout(false);
            grpPaciente.PerformLayout();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPeso).EndInit();
            tabHistoria.ResumeLayout(false);
            tpAtenciones.ResumeLayout(false);
            tpAtenciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAtenciones).EndInit();
            tpEvolucion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEvolucion).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpPaciente;
        private Label lblPaciente;
        private Label lblNumero;
        private Label lblEspecie;
        private TextBox txtEspecie;
        private Label lblRaza;
        private TextBox txtRaza;
        private Label lblNacimiento;
        private DateTimePicker dtpNacimiento;
        private Label lblPropietario;
        private GroupBox grpDatos;
        private Label lblPeso;
        private NumericUpDown nudPeso;
        private CheckBox chkSinAlergias;
        private Label lblAlergias;
        private TextBox txtAlergias;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private Label lblAlertaAlergias;
        private Button btnModificar;
        private Button btnGuardar;
        private Button btnCancelarEdicion;
        private Button btnNuevaAtencion;
        private Button btnImprimir;
        private Button btnCerrar;
        private TabControl tabHistoria;
        private TabPage tpAtenciones;
        private DataGridView dgvAtenciones;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colProfesional;
        private DataGridViewTextBoxColumn colMotivo;
        private DataGridViewTextBoxColumn colPeso;
        private TextBox txtDetalle;
        private Button btnVerInforme;
        private Button btnMedicacion;
        private TabPage tpEvolucion;
        private DataGridView dgvEvolucion;
        private DataGridViewTextBoxColumn colFechaEvolucion;
        private DataGridViewTextBoxColumn colOrigen;
        private DataGridViewTextBoxColumn colPesoEvolucion;
        private DataGridViewTextBoxColumn colAlergiasEvolucion;
        private DataGridViewTextBoxColumn colObservacionesEvolucion;
        private DataGridViewTextBoxColumn colUsuarioEvolucion;
    }
}
