namespace VetCare
{
    partial class FrmReporteCompras_60MN
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
            DataGridViewCellStyle estiloNumero = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloImporte = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloFecha = new DataGridViewCellStyle();
            lblTitulo = new Label();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            lblProveedor = new Label();
            cboProveedor = new ComboBox();
            lblEstado = new Label();
            cboEstado = new ComboBox();
            btnGenerar = new Button();
            dgvOrdenes = new DataGridView();
            colNumero = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colProveedor = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colPagado = new DataGridViewTextBoxColumn();
            colUltimaRecepcion = new DataGridViewTextBoxColumn();
            colPedidas = new DataGridViewTextBoxColumn();
            colRecibidas = new DataGridViewTextBoxColumn();
            colPendientes = new DataGridViewTextBoxColumn();
            lblPorProveedor = new Label();
            dgvTotales = new DataGridView();
            colTotProveedor = new DataGridViewTextBoxColumn();
            colTotOrdenes = new DataGridViewTextBoxColumn();
            colTotComprado = new DataGridViewTextBoxColumn();
            colTotPagado = new DataGridViewTextBoxColumn();
            colTotPendientes = new DataGridViewTextBoxColumn();
            lblTotales = new Label();
            btnExportarPdf = new Button();
            btnImprimir = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvTotales).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(187, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Reporte de compras";
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(12, 56);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(51, 20);
            lblDesde.TabIndex = 1;
            lblDesde.Text = "Desde";
            //
            // dtpDesde
            //
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(70, 52);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(130, 27);
            dtpDesde.TabIndex = 2;
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(214, 56);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(47, 20);
            lblHasta.TabIndex = 3;
            lblHasta.Text = "Hasta";
            //
            // dtpHasta
            //
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(268, 52);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(130, 27);
            dtpHasta.TabIndex = 4;
            //
            // lblProveedor
            //
            lblProveedor.AutoSize = true;
            lblProveedor.Location = new Point(414, 56);
            lblProveedor.Name = "lblProveedor";
            lblProveedor.Size = new Size(77, 20);
            lblProveedor.TabIndex = 5;
            lblProveedor.Text = "Proveedor";
            //
            // cboProveedor
            //
            cboProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProveedor.Location = new Point(498, 52);
            cboProveedor.Name = "cboProveedor";
            cboProveedor.Size = new Size(280, 28);
            cboProveedor.TabIndex = 6;
            //
            // lblEstado
            //
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(792, 56);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(54, 20);
            lblEstado.TabIndex = 7;
            lblEstado.Text = "Estado";
            //
            // cboEstado
            //
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Location = new Point(852, 52);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(170, 28);
            cboEstado.TabIndex = 8;
            //
            // btnGenerar
            //
            btnGenerar.Location = new Point(1032, 49);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(106, 34);
            btnGenerar.TabIndex = 9;
            btnGenerar.Text = "Generar";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            //
            // dgvOrdenes
            //
            dgvOrdenes.AllowUserToAddRows = false;
            dgvOrdenes.AllowUserToDeleteRows = false;
            dgvOrdenes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenes.BackgroundColor = SystemColors.Window;
            dgvOrdenes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrdenes.Columns.AddRange(new DataGridViewColumn[] { colNumero, colFecha, colProveedor, colEstado, colTotal, colPagado, colUltimaRecepcion, colPedidas, colRecibidas, colPendientes });
            dgvOrdenes.Location = new Point(12, 96);
            dgvOrdenes.MultiSelect = false;
            dgvOrdenes.Name = "dgvOrdenes";
            dgvOrdenes.ReadOnly = true;
            dgvOrdenes.RowHeadersVisible = false;
            dgvOrdenes.RowHeadersWidth = 51;
            dgvOrdenes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrdenes.Size = new Size(1126, 290);
            dgvOrdenes.TabIndex = 10;
            //
            // colNumero
            //
            colNumero.DataPropertyName = "Numero";
            colNumero.FillWeight = 60F;
            colNumero.HeaderText = "Orden";
            colNumero.MinimumWidth = 6;
            colNumero.Name = "colNumero";
            colNumero.ReadOnly = true;
            //
            // colFecha
            //
            colFecha.DataPropertyName = "FechaHora";
            estiloFecha.Format = "dd/MM/yyyy";
            colFecha.DefaultCellStyle = estiloFecha;
            colFecha.FillWeight = 65F;
            colFecha.HeaderText = "Fecha";
            colFecha.MinimumWidth = 6;
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colProveedor
            //
            colProveedor.DataPropertyName = "Proveedor";
            colProveedor.FillWeight = 150F;
            colProveedor.HeaderText = "Proveedor";
            colProveedor.MinimumWidth = 6;
            colProveedor.Name = "colProveedor";
            colProveedor.ReadOnly = true;
            //
            // colEstado
            //
            colEstado.DataPropertyName = "EstadoTexto";
            colEstado.FillWeight = 85F;
            colEstado.HeaderText = "Estado";
            colEstado.MinimumWidth = 6;
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // colTotal
            //
            colTotal.DataPropertyName = "Total";
            estiloImporte.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloImporte.Format = "N2";
            colTotal.DefaultCellStyle = estiloImporte;
            colTotal.FillWeight = 70F;
            colTotal.HeaderText = "Total $";
            colTotal.MinimumWidth = 6;
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            //
            // colPagado
            //
            colPagado.DataPropertyName = "ImportePagado";
            colPagado.DefaultCellStyle = estiloImporte;
            colPagado.FillWeight = 70F;
            colPagado.HeaderText = "Pagado $";
            colPagado.MinimumWidth = 6;
            colPagado.Name = "colPagado";
            colPagado.ReadOnly = true;
            //
            // colUltimaRecepcion
            //
            colUltimaRecepcion.DataPropertyName = "UltimaRecepcion";
            colUltimaRecepcion.DefaultCellStyle = estiloFecha;
            colUltimaRecepcion.FillWeight = 70F;
            colUltimaRecepcion.HeaderText = "Última recepción";
            colUltimaRecepcion.MinimumWidth = 6;
            colUltimaRecepcion.Name = "colUltimaRecepcion";
            colUltimaRecepcion.ReadOnly = true;
            //
            // colPedidas
            //
            colPedidas.DataPropertyName = "UnidadesPedidas";
            estiloNumero.Alignment = DataGridViewContentAlignment.MiddleRight;
            colPedidas.DefaultCellStyle = estiloNumero;
            colPedidas.FillWeight = 55F;
            colPedidas.HeaderText = "Pedidas";
            colPedidas.MinimumWidth = 6;
            colPedidas.Name = "colPedidas";
            colPedidas.ReadOnly = true;
            //
            // colRecibidas
            //
            colRecibidas.DataPropertyName = "UnidadesRecibidas";
            colRecibidas.DefaultCellStyle = estiloNumero;
            colRecibidas.FillWeight = 55F;
            colRecibidas.HeaderText = "Recibidas";
            colRecibidas.MinimumWidth = 6;
            colRecibidas.Name = "colRecibidas";
            colRecibidas.ReadOnly = true;
            //
            // colPendientes
            //
            colPendientes.DataPropertyName = "UnidadesPendientes";
            colPendientes.DefaultCellStyle = estiloNumero;
            colPendientes.FillWeight = 60F;
            colPendientes.HeaderText = "Por recibir";
            colPendientes.MinimumWidth = 6;
            colPendientes.Name = "colPendientes";
            colPendientes.ReadOnly = true;
            //
            // lblPorProveedor
            //
            lblPorProveedor.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblPorProveedor.AutoSize = true;
            lblPorProveedor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPorProveedor.Location = new Point(12, 396);
            lblPorProveedor.Name = "lblPorProveedor";
            lblPorProveedor.Size = new Size(169, 20);
            lblPorProveedor.TabIndex = 11;
            lblPorProveedor.Text = "Totales por proveedor";
            //
            // dgvTotales
            //
            dgvTotales.AllowUserToAddRows = false;
            dgvTotales.AllowUserToDeleteRows = false;
            dgvTotales.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTotales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTotales.BackgroundColor = SystemColors.Window;
            dgvTotales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTotales.Columns.AddRange(new DataGridViewColumn[] { colTotProveedor, colTotOrdenes, colTotComprado, colTotPagado, colTotPendientes });
            dgvTotales.Location = new Point(12, 420);
            dgvTotales.MultiSelect = false;
            dgvTotales.Name = "dgvTotales";
            dgvTotales.ReadOnly = true;
            dgvTotales.RowHeadersVisible = false;
            dgvTotales.RowHeadersWidth = 51;
            dgvTotales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTotales.Size = new Size(790, 160);
            dgvTotales.TabIndex = 12;
            //
            // colTotProveedor
            //
            colTotProveedor.DataPropertyName = "Proveedor";
            colTotProveedor.FillWeight = 170F;
            colTotProveedor.HeaderText = "Proveedor";
            colTotProveedor.MinimumWidth = 6;
            colTotProveedor.Name = "colTotProveedor";
            colTotProveedor.ReadOnly = true;
            //
            // colTotOrdenes
            //
            colTotOrdenes.DataPropertyName = "Ordenes";
            colTotOrdenes.DefaultCellStyle = estiloNumero;
            colTotOrdenes.FillWeight = 55F;
            colTotOrdenes.HeaderText = "Órdenes";
            colTotOrdenes.MinimumWidth = 6;
            colTotOrdenes.Name = "colTotOrdenes";
            colTotOrdenes.ReadOnly = true;
            //
            // colTotComprado
            //
            colTotComprado.DataPropertyName = "Comprado";
            colTotComprado.DefaultCellStyle = estiloImporte;
            colTotComprado.FillWeight = 80F;
            colTotComprado.HeaderText = "Comprado $";
            colTotComprado.MinimumWidth = 6;
            colTotComprado.Name = "colTotComprado";
            colTotComprado.ReadOnly = true;
            //
            // colTotPagado
            //
            colTotPagado.DataPropertyName = "Pagado";
            colTotPagado.DefaultCellStyle = estiloImporte;
            colTotPagado.FillWeight = 80F;
            colTotPagado.HeaderText = "Pagado $";
            colTotPagado.MinimumWidth = 6;
            colTotPagado.Name = "colTotPagado";
            colTotPagado.ReadOnly = true;
            //
            // colTotPendientes
            //
            colTotPendientes.DataPropertyName = "UnidadesPendientes";
            colTotPendientes.DefaultCellStyle = estiloNumero;
            colTotPendientes.FillWeight = 60F;
            colTotPendientes.HeaderText = "Por recibir";
            colTotPendientes.MinimumWidth = 6;
            colTotPendientes.Name = "colTotPendientes";
            colTotPendientes.ReadOnly = true;
            //
            // lblTotales
            //
            lblTotales.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblTotales.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotales.Location = new Point(814, 420);
            lblTotales.Name = "lblTotales";
            lblTotales.Size = new Size(324, 110);
            lblTotales.TabIndex = 13;
            //
            // btnExportarPdf
            //
            btnExportarPdf.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExportarPdf.Location = new Point(814, 546);
            btnExportarPdf.Name = "btnExportarPdf";
            btnExportarPdf.Size = new Size(158, 34);
            btnExportarPdf.TabIndex = 14;
            btnExportarPdf.Text = "Exportar PDF";
            btnExportarPdf.UseVisualStyleBackColor = true;
            btnExportarPdf.Click += btnExportarPdf_Click;
            //
            // btnImprimir
            //
            btnImprimir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnImprimir.Location = new Point(980, 546);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(158, 34);
            btnImprimir.TabIndex = 15;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            //
            // FrmReporteCompras_60MN
            //
            AcceptButton = btnGenerar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1150, 596);
            Controls.Add(btnImprimir);
            Controls.Add(btnExportarPdf);
            Controls.Add(lblTotales);
            Controls.Add(dgvTotales);
            Controls.Add(lblPorProveedor);
            Controls.Add(dgvOrdenes);
            Controls.Add(btnGenerar);
            Controls.Add(cboEstado);
            Controls.Add(lblEstado);
            Controls.Add(cboProveedor);
            Controls.Add(lblProveedor);
            Controls.Add(dtpHasta);
            Controls.Add(lblHasta);
            Controls.Add(dtpDesde);
            Controls.Add(lblDesde);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(1000, 580);
            Name = "FrmReporteCompras_60MN";
            Text = "Reporte de compras";
            Load += FrmReporteCompras_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvTotales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Label lblProveedor;
        private ComboBox cboProveedor;
        private Label lblEstado;
        private ComboBox cboEstado;
        private Button btnGenerar;
        private DataGridView dgvOrdenes;
        private DataGridViewTextBoxColumn colNumero;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colProveedor;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewTextBoxColumn colPagado;
        private DataGridViewTextBoxColumn colUltimaRecepcion;
        private DataGridViewTextBoxColumn colPedidas;
        private DataGridViewTextBoxColumn colRecibidas;
        private DataGridViewTextBoxColumn colPendientes;
        private Label lblPorProveedor;
        private DataGridView dgvTotales;
        private DataGridViewTextBoxColumn colTotProveedor;
        private DataGridViewTextBoxColumn colTotOrdenes;
        private DataGridViewTextBoxColumn colTotComprado;
        private DataGridViewTextBoxColumn colTotPagado;
        private DataGridViewTextBoxColumn colTotPendientes;
        private Label lblTotales;
        private Button btnExportarPdf;
        private Button btnImprimir;
    }
}
