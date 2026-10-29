namespace VetCare
{
    partial class FrmBitacoraEventos_60MN
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.lblCriticidad = new System.Windows.Forms.Label();
            this.cboCriticidad = new System.Windows.Forms.ComboBox();
            this.lblModulo = new System.Windows.Forms.Label();
            this.cboModulo = new System.Windows.Forms.ComboBox();
            this.lblEvento = new System.Windows.Forms.Label();
            this.cboEvento = new System.Windows.Forms.ComboBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.cboUsuario = new System.Windows.Forms.ComboBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnExportarXml = new System.Windows.Forms.Button();
            this.btnImportarXml = new System.Windows.Forms.Button();
            this.btnVerXml = new System.Windows.Forms.Button();
            this.dgvEventos = new System.Windows.Forms.DataGridView();
            this.colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModulo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEvento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCriticidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDetalleUsuario = new System.Windows.Forms.Label();
            this.lblResultados = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEventos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(12, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(200, 28);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Bitácora de eventos";
            // 
            // lblDesde
            // 
            this.lblDesde.AutoSize = true;
            this.lblDesde.Location = new System.Drawing.Point(12, 56);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(51, 20);
            this.lblDesde.TabIndex = 1;
            this.lblDesde.Text = "Desde";
            // 
            // dtpDesde
            // 
            this.dtpDesde.Checked = false;
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(80, 52);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.ShowCheckBox = true;
            this.dtpDesde.Size = new System.Drawing.Size(160, 27);
            this.dtpDesde.TabIndex = 2;
            // 
            // lblHasta
            // 
            this.lblHasta.AutoSize = true;
            this.lblHasta.Location = new System.Drawing.Point(260, 56);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(47, 20);
            this.lblHasta.TabIndex = 3;
            this.lblHasta.Text = "Hasta";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Checked = false;
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(320, 52);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.ShowCheckBox = true;
            this.dtpHasta.Size = new System.Drawing.Size(160, 27);
            this.dtpHasta.TabIndex = 4;
            // 
            // lblCriticidad
            // 
            this.lblCriticidad.AutoSize = true;
            this.lblCriticidad.Location = new System.Drawing.Point(500, 56);
            this.lblCriticidad.Name = "lblCriticidad";
            this.lblCriticidad.Size = new System.Drawing.Size(73, 20);
            this.lblCriticidad.TabIndex = 5;
            this.lblCriticidad.Text = "Criticidad";
            // 
            // cboCriticidad
            // 
            this.cboCriticidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCriticidad.FormattingEnabled = true;
            this.cboCriticidad.Location = new System.Drawing.Point(580, 52);
            this.cboCriticidad.Name = "cboCriticidad";
            this.cboCriticidad.Size = new System.Drawing.Size(200, 28);
            this.cboCriticidad.TabIndex = 6;
            // 
            // lblModulo
            // 
            this.lblModulo.AutoSize = true;
            this.lblModulo.Location = new System.Drawing.Point(12, 96);
            this.lblModulo.Name = "lblModulo";
            this.lblModulo.Size = new System.Drawing.Size(61, 20);
            this.lblModulo.TabIndex = 7;
            this.lblModulo.Text = "Módulo";
            // 
            // cboModulo
            // 
            this.cboModulo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModulo.FormattingEnabled = true;
            this.cboModulo.Location = new System.Drawing.Point(80, 92);
            this.cboModulo.Name = "cboModulo";
            this.cboModulo.Size = new System.Drawing.Size(160, 28);
            this.cboModulo.TabIndex = 8;
            // 
            // lblEvento
            // 
            this.lblEvento.AutoSize = true;
            this.lblEvento.Location = new System.Drawing.Point(260, 96);
            this.lblEvento.Name = "lblEvento";
            this.lblEvento.Size = new System.Drawing.Size(54, 20);
            this.lblEvento.TabIndex = 9;
            this.lblEvento.Text = "Evento";
            // 
            // cboEvento
            // 
            this.cboEvento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEvento.FormattingEnabled = true;
            this.cboEvento.Location = new System.Drawing.Point(320, 92);
            this.cboEvento.Name = "cboEvento";
            this.cboEvento.Size = new System.Drawing.Size(160, 28);
            this.cboEvento.TabIndex = 10;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(500, 96);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(59, 20);
            this.lblUsuario.TabIndex = 11;
            this.lblUsuario.Text = "Usuario";
            // 
            // cboUsuario
            // 
            this.cboUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsuario.FormattingEnabled = true;
            this.cboUsuario.Location = new System.Drawing.Point(580, 92);
            this.cboUsuario.Name = "cboUsuario";
            this.cboUsuario.Size = new System.Drawing.Size(200, 28);
            this.cboUsuario.TabIndex = 12;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(800, 50);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(110, 34);
            this.btnBuscar.TabIndex = 13;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(800, 90);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(110, 34);
            this.btnLimpiar.TabIndex = 14;
            this.btnLimpiar.Text = "Limpiar filtros";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // btnExportarXml
            // 
            this.btnExportarXml.Location = new System.Drawing.Point(926, 50);
            this.btnExportarXml.Name = "btnExportarXml";
            this.btnExportarXml.Size = new System.Drawing.Size(162, 34);
            this.btnExportarXml.TabIndex = 15;
            this.btnExportarXml.Tag = "SEG_BITACORA_EXPORT";
            this.btnExportarXml.Text = "Exportar XML...";
            this.btnExportarXml.UseVisualStyleBackColor = true;
            //
            // btnImportarXml
            //
            this.btnImportarXml.Location = new System.Drawing.Point(926, 90);
            this.btnImportarXml.Name = "btnImportarXml";
            this.btnImportarXml.Size = new System.Drawing.Size(162, 34);
            this.btnImportarXml.TabIndex = 16;
            this.btnImportarXml.Tag = "SEG_BITACORA_IMPORT";
            this.btnImportarXml.Text = "Importar XML...";
            this.btnImportarXml.UseVisualStyleBackColor = true;
            //
            // btnVerXml
            //
            this.btnVerXml.Location = new System.Drawing.Point(926, 10);
            this.btnVerXml.Name = "btnVerXml";
            this.btnVerXml.Size = new System.Drawing.Size(162, 34);
            this.btnVerXml.TabIndex = 17;
            this.btnVerXml.Tag = "SEG_BITACORA_EXPORT|SEG_BITACORA_IMPORT";
            this.btnVerXml.Text = "Ver XML...";
            this.btnVerXml.UseVisualStyleBackColor = true;
            // 
            // dgvEventos
            // 
            this.dgvEventos.AllowUserToAddRows = false;
            this.dgvEventos.AllowUserToDeleteRows = false;
            this.dgvEventos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEventos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEventos.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvEventos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEventos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFecha,
            this.colUsuario,
            this.colModulo,
            this.colEvento,
            this.colCriticidad,
            this.colDescripcion});
            this.dgvEventos.Location = new System.Drawing.Point(12, 136);
            this.dgvEventos.MultiSelect = false;
            this.dgvEventos.Name = "dgvEventos";
            this.dgvEventos.ReadOnly = true;
            this.dgvEventos.RowHeadersVisible = false;
            this.dgvEventos.RowHeadersWidth = 51;
            this.dgvEventos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEventos.Size = new System.Drawing.Size(1076, 390);
            this.dgvEventos.TabIndex = 15;
            // 
            // colFecha
            // 
            this.colFecha.DataPropertyName = "FechaHora";
            dataGridViewCellStyle1.Format = "dd/MM/yyyy HH:mm:ss";
            this.colFecha.DefaultCellStyle = dataGridViewCellStyle1;
            this.colFecha.FillWeight = 70F;
            this.colFecha.HeaderText = "Fecha y hora";
            this.colFecha.MinimumWidth = 6;
            this.colFecha.Name = "colFecha";
            this.colFecha.ReadOnly = true;
            // 
            // colUsuario
            // 
            this.colUsuario.DataPropertyName = "Usuario";
            this.colUsuario.FillWeight = 55F;
            this.colUsuario.HeaderText = "Usuario";
            this.colUsuario.MinimumWidth = 6;
            this.colUsuario.Name = "colUsuario";
            this.colUsuario.ReadOnly = true;
            // 
            // colModulo
            // 
            this.colModulo.DataPropertyName = "Modulo";
            this.colModulo.FillWeight = 50F;
            this.colModulo.HeaderText = "Módulo";
            this.colModulo.MinimumWidth = 6;
            this.colModulo.Name = "colModulo";
            this.colModulo.ReadOnly = true;
            // 
            // colEvento
            // 
            this.colEvento.DataPropertyName = "Evento";
            this.colEvento.FillWeight = 70F;
            this.colEvento.HeaderText = "Evento";
            this.colEvento.MinimumWidth = 6;
            this.colEvento.Name = "colEvento";
            this.colEvento.ReadOnly = true;
            // 
            // colCriticidad
            // 
            this.colCriticidad.DataPropertyName = "Criticidad";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colCriticidad.DefaultCellStyle = dataGridViewCellStyle2;
            this.colCriticidad.FillWeight = 35F;
            this.colCriticidad.HeaderText = "Criticidad";
            this.colCriticidad.MinimumWidth = 6;
            this.colCriticidad.Name = "colCriticidad";
            this.colCriticidad.ReadOnly = true;
            // 
            // colDescripcion
            // 
            this.colDescripcion.DataPropertyName = "Descripcion";
            this.colDescripcion.FillWeight = 200F;
            this.colDescripcion.HeaderText = "Descripción";
            this.colDescripcion.MinimumWidth = 6;
            this.colDescripcion.Name = "colDescripcion";
            this.colDescripcion.ReadOnly = true;
            // 
            // lblDetalleUsuario
            // 
            this.lblDetalleUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDetalleUsuario.AutoSize = true;
            this.lblDetalleUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDetalleUsuario.Location = new System.Drawing.Point(12, 536);
            this.lblDetalleUsuario.Name = "lblDetalleUsuario";
            this.lblDetalleUsuario.Size = new System.Drawing.Size(0, 20);
            this.lblDetalleUsuario.TabIndex = 16;
            // 
            // lblResultados
            // 
            this.lblResultados.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultados.ForeColor = System.Drawing.Color.DimGray;
            this.lblResultados.Location = new System.Drawing.Point(788, 536);
            this.lblResultados.Name = "lblResultados";
            this.lblResultados.Size = new System.Drawing.Size(300, 20);
            this.lblResultados.TabIndex = 17;
            this.lblResultados.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FrmBitacoraEventos_60MN
            // 
            this.AcceptButton = this.btnBuscar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 568);
            this.Controls.Add(this.lblResultados);
            this.Controls.Add(this.lblDetalleUsuario);
            this.Controls.Add(this.dgvEventos);
            this.Controls.Add(this.btnVerXml);
            this.Controls.Add(this.btnImportarXml);
            this.Controls.Add(this.btnExportarXml);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.cboUsuario);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.cboEvento);
            this.Controls.Add(this.lblEvento);
            this.Controls.Add(this.cboModulo);
            this.Controls.Add(this.lblModulo);
            this.Controls.Add(this.cboCriticidad);
            this.Controls.Add(this.lblCriticidad);
            this.Controls.Add(this.dtpHasta);
            this.Controls.Add(this.lblHasta);
            this.Controls.Add(this.dtpDesde);
            this.Controls.Add(this.lblDesde);
            this.Controls.Add(this.lblTitulo);
            this.MinimumSize = new System.Drawing.Size(960, 450);
            this.Name = "FrmBitacoraEventos_60MN";
            this.Text = "Bitácora de eventos";
            this.Load += new System.EventHandler(this.FrmBitacoraEventos_60MN_Load);
            this.cboModulo.SelectedIndexChanged += new System.EventHandler(this.cboModulo_SelectedIndexChanged);
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            this.btnExportarXml.Click += new System.EventHandler(this.btnExportarXml_Click);
            this.btnImportarXml.Click += new System.EventHandler(this.btnImportarXml_Click);
            this.btnVerXml.Click += new System.EventHandler(this.btnVerXml_Click);
            this.dgvEventos.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvEventos_DataBindingComplete);
            this.dgvEventos.SelectionChanged += new System.EventHandler(this.dgvEventos_SelectionChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEventos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblTitulo;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Label lblCriticidad;
        private ComboBox cboCriticidad;
        private Label lblModulo;
        private ComboBox cboModulo;
        private Label lblEvento;
        private ComboBox cboEvento;
        private Label lblUsuario;
        private ComboBox cboUsuario;
        private Button btnBuscar;
        private Button btnLimpiar;
        private Button btnExportarXml;
        private Button btnImportarXml;
        private Button btnVerXml;
        private DataGridView dgvEventos;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colUsuario;
        private DataGridViewTextBoxColumn colModulo;
        private DataGridViewTextBoxColumn colEvento;
        private DataGridViewTextBoxColumn colCriticidad;
        private DataGridViewTextBoxColumn colDescripcion;
        private Label lblDetalleUsuario;
        private Label lblResultados;
    }
}
