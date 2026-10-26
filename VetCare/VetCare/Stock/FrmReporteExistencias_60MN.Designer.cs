namespace VetCare
{
    partial class FrmReporteExistencias_60MN
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
            DataGridViewCellStyle estiloVence = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloCosto = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloValorizacion = new DataGridViewCellStyle();
            grpFiltros = new GroupBox();
            lblTipo = new Label();
            cboTipo = new ComboBox();
            lblCategoria = new Label();
            cboCategoria = new ComboBox();
            lblProveedor = new Label();
            cboProveedor = new ComboBox();
            chkVencimiento = new CheckBox();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            btnGenerar = new Button();
            dgvReporte = new DataGridView();
            colCodigo = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colCategoria = new DataGridViewTextBoxColumn();
            colProveedor = new DataGridViewTextBoxColumn();
            colLote = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            colMinimo = new DataGridViewTextBoxColumn();
            colVence = new DataGridViewTextBoxColumn();
            colCosto = new DataGridViewTextBoxColumn();
            colValorizacion = new DataGridViewTextBoxColumn();
            lblResumen = new Label();
            btnImprimir = new Button();
            btnExportar = new Button();
            btnCerrar = new Button();
            grpFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            SuspendLayout();
            //
            // grpFiltros
            //
            grpFiltros.Controls.Add(lblTipo);
            grpFiltros.Controls.Add(cboTipo);
            grpFiltros.Controls.Add(lblCategoria);
            grpFiltros.Controls.Add(cboCategoria);
            grpFiltros.Controls.Add(lblProveedor);
            grpFiltros.Controls.Add(cboProveedor);
            grpFiltros.Controls.Add(chkVencimiento);
            grpFiltros.Controls.Add(dtpDesde);
            grpFiltros.Controls.Add(lblHasta);
            grpFiltros.Controls.Add(dtpHasta);
            grpFiltros.Controls.Add(btnGenerar);
            grpFiltros.Location = new Point(12, 8);
            grpFiltros.Name = "grpFiltros";
            grpFiltros.Size = new Size(1076, 108);
            grpFiltros.TabIndex = 0;
            grpFiltros.TabStop = false;
            grpFiltros.Text = "Criterios";
            //
            // lblTipo
            //
            lblTipo.AutoSize = true;
            lblTipo.Location = new Point(14, 32);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(39, 20);
            lblTipo.TabIndex = 0;
            lblTipo.Text = "Tipo";
            //
            // cboTipo
            //
            cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipo.FormattingEnabled = true;
            cboTipo.Location = new Point(100, 28);
            cboTipo.Name = "cboTipo";
            cboTipo.Size = new Size(240, 28);
            cboTipo.TabIndex = 1;
            cboTipo.SelectedIndexChanged += cboTipo_SelectedIndexChanged;
            //
            // lblCategoria
            //
            lblCategoria.AutoSize = true;
            lblCategoria.Location = new Point(360, 32);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(74, 20);
            lblCategoria.TabIndex = 2;
            lblCategoria.Text = "Categoría";
            //
            // cboCategoria
            //
            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoria.FormattingEnabled = true;
            cboCategoria.Location = new Point(446, 28);
            cboCategoria.Name = "cboCategoria";
            cboCategoria.Size = new Size(210, 28);
            cboCategoria.TabIndex = 3;
            //
            // lblProveedor
            //
            lblProveedor.AutoSize = true;
            lblProveedor.Location = new Point(676, 32);
            lblProveedor.Name = "lblProveedor";
            lblProveedor.Size = new Size(77, 20);
            lblProveedor.TabIndex = 4;
            lblProveedor.Text = "Proveedor";
            //
            // cboProveedor
            //
            cboProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProveedor.FormattingEnabled = true;
            cboProveedor.Location = new Point(764, 28);
            cboProveedor.Name = "cboProveedor";
            cboProveedor.Size = new Size(296, 28);
            cboProveedor.TabIndex = 5;
            //
            // chkVencimiento
            //
            chkVencimiento.AutoSize = true;
            chkVencimiento.Location = new Point(14, 70);
            chkVencimiento.Name = "chkVencimiento";
            chkVencimiento.Size = new Size(165, 24);
            chkVencimiento.TabIndex = 6;
            chkVencimiento.Text = "Vencimiento desde";
            chkVencimiento.UseVisualStyleBackColor = true;
            chkVencimiento.CheckedChanged += chkVencimiento_CheckedChanged;
            //
            // dtpDesde
            //
            dtpDesde.Enabled = false;
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(190, 68);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(130, 27);
            dtpDesde.TabIndex = 7;
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(334, 72);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(46, 20);
            lblHasta.TabIndex = 8;
            lblHasta.Text = "hasta";
            //
            // dtpHasta
            //
            dtpHasta.Enabled = false;
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(390, 68);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(130, 27);
            dtpHasta.TabIndex = 9;
            //
            // btnGenerar
            //
            btnGenerar.Location = new Point(920, 64);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(140, 34);
            btnGenerar.TabIndex = 10;
            btnGenerar.Text = "Generar";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            //
            // dgvReporte
            //
            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.AllowUserToDeleteRows = false;
            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReporte.BackgroundColor = SystemColors.Window;
            dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReporte.Columns.AddRange(new DataGridViewColumn[] { colCodigo, colNombre, colCategoria, colProveedor, colLote, colStock, colMinimo, colVence, colCosto, colValorizacion });
            dgvReporte.Location = new Point(12, 124);
            dgvReporte.Name = "dgvReporte";
            dgvReporte.ReadOnly = true;
            dgvReporte.RowHeadersVisible = false;
            dgvReporte.RowHeadersWidth = 51;
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.Size = new Size(1076, 400);
            dgvReporte.TabIndex = 1;
            dgvReporte.DataBindingComplete += dgvReporte_DataBindingComplete;
            //
            // colCodigo
            //
            colCodigo.DataPropertyName = "Codigo";
            colCodigo.FillWeight = 60F;
            colCodigo.HeaderText = "Código";
            colCodigo.MinimumWidth = 6;
            colCodigo.Name = "colCodigo";
            colCodigo.ReadOnly = true;
            //
            // colNombre
            //
            colNombre.DataPropertyName = "Nombre";
            colNombre.FillWeight = 150F;
            colNombre.HeaderText = "Nombre";
            colNombre.MinimumWidth = 6;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            //
            // colCategoria
            //
            colCategoria.DataPropertyName = "CategoriaNombre";
            colCategoria.HeaderText = "Categoría";
            colCategoria.MinimumWidth = 6;
            colCategoria.Name = "colCategoria";
            colCategoria.ReadOnly = true;
            //
            // colProveedor
            //
            colProveedor.DataPropertyName = "ProveedorNombre";
            colProveedor.HeaderText = "Proveedor";
            colProveedor.MinimumWidth = 6;
            colProveedor.Name = "colProveedor";
            colProveedor.ReadOnly = true;
            //
            // colLote
            //
            colLote.DataPropertyName = "Lote";
            colLote.FillWeight = 60F;
            colLote.HeaderText = "Lote";
            colLote.MinimumWidth = 6;
            colLote.Name = "colLote";
            colLote.ReadOnly = true;
            //
            // colStock
            //
            colStock.DataPropertyName = "StockActual";
            colStock.FillWeight = 50F;
            colStock.HeaderText = "Stock";
            colStock.MinimumWidth = 6;
            colStock.Name = "colStock";
            colStock.ReadOnly = true;
            //
            // colMinimo
            //
            colMinimo.DataPropertyName = "StockMinimo";
            colMinimo.FillWeight = 50F;
            colMinimo.HeaderText = "Mínimo";
            colMinimo.MinimumWidth = 6;
            colMinimo.Name = "colMinimo";
            colMinimo.ReadOnly = true;
            //
            // colVence
            //
            colVence.DataPropertyName = "FechaVencimiento";
            estiloVence.Format = "dd/MM/yyyy";
            colVence.DefaultCellStyle = estiloVence;
            colVence.FillWeight = 70F;
            colVence.HeaderText = "Vencimiento";
            colVence.MinimumWidth = 6;
            colVence.Name = "colVence";
            colVence.ReadOnly = true;
            //
            // colCosto
            //
            colCosto.DataPropertyName = "PrecioCosto";
            estiloCosto.Format = "N2";
            colCosto.DefaultCellStyle = estiloCosto;
            colCosto.FillWeight = 60F;
            colCosto.HeaderText = "Costo";
            colCosto.MinimumWidth = 6;
            colCosto.Name = "colCosto";
            colCosto.ReadOnly = true;
            //
            // colValorizacion
            //
            colValorizacion.DataPropertyName = "Valorizacion";
            estiloValorizacion.Format = "N2";
            colValorizacion.DefaultCellStyle = estiloValorizacion;
            colValorizacion.FillWeight = 70F;
            colValorizacion.HeaderText = "Valorización";
            colValorizacion.MinimumWidth = 6;
            colValorizacion.Name = "colValorizacion";
            colValorizacion.ReadOnly = true;
            //
            // lblResumen
            //
            lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResumen.Location = new Point(12, 536);
            lblResumen.Name = "lblResumen";
            lblResumen.Size = new Size(600, 26);
            lblResumen.TabIndex = 2;
            //
            // btnImprimir
            //
            btnImprimir.Enabled = false;
            btnImprimir.Location = new Point(658, 532);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(150, 34);
            btnImprimir.TabIndex = 3;
            btnImprimir.Text = "Imprimir / PDF";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            //
            // btnExportar
            //
            btnExportar.Enabled = false;
            btnExportar.Location = new Point(818, 532);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(150, 34);
            btnExportar.TabIndex = 4;
            btnExportar.Text = "Exportar CSV";
            btnExportar.UseVisualStyleBackColor = true;
            btnExportar.Click += btnExportar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(978, 532);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 34);
            btnCerrar.TabIndex = 5;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmReporteExistencias_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 580);
            Controls.Add(btnCerrar);
            Controls.Add(btnExportar);
            Controls.Add(btnImprimir);
            Controls.Add(lblResumen);
            Controls.Add(dgvReporte);
            Controls.Add(grpFiltros);
            Name = "FrmReporteExistencias_60MN";
            Text = "Reporte de existencias";
            Load += FrmReporteExistencias_60MN_Load;
            grpFiltros.ResumeLayout(false);
            grpFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpFiltros;
        private Label lblTipo;
        private ComboBox cboTipo;
        private Label lblCategoria;
        private ComboBox cboCategoria;
        private Label lblProveedor;
        private ComboBox cboProveedor;
        private CheckBox chkVencimiento;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnGenerar;
        private DataGridView dgvReporte;
        private DataGridViewTextBoxColumn colCodigo;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colCategoria;
        private DataGridViewTextBoxColumn colProveedor;
        private DataGridViewTextBoxColumn colLote;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colMinimo;
        private DataGridViewTextBoxColumn colVence;
        private DataGridViewTextBoxColumn colCosto;
        private DataGridViewTextBoxColumn colValorizacion;
        private Label lblResumen;
        private Button btnImprimir;
        private Button btnExportar;
        private Button btnCerrar;
    }
}
