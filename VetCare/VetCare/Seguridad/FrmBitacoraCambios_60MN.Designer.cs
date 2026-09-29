namespace VetCare
{
    partial class FrmBitacoraCambios_60MN
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
            DataGridViewCellStyle estiloCosto = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloVenta = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloVencimiento = new DataGridViewCellStyle();
            lblTitulo = new Label();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            lblCodigo = new Label();
            txtCodigo = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            btnBuscar = new Button();
            dgvCambios = new DataGridView();
            colAct = new DataGridViewCheckBoxColumn();
            colCodigo = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colOperacion = new DataGridViewTextBoxColumn();
            colFechaHora = new DataGridViewTextBoxColumn();
            colUsuario = new DataGridViewTextBoxColumn();
            colCategoria = new DataGridViewTextBoxColumn();
            colProveedor = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            colStockMinimo = new DataGridViewTextBoxColumn();
            colCosto = new DataGridViewTextBoxColumn();
            colVenta = new DataGridViewTextBoxColumn();
            colVencimiento = new DataGridViewTextBoxColumn();
            colLote = new DataGridViewTextBoxColumn();
            lblAyuda = new Label();
            btnComparar = new Button();
            btnRestaurar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCambios).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(283, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Bitácora de cambios - Productos";
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
            dtpDesde.Checked = false;
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(70, 52);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.ShowCheckBox = true;
            dtpDesde.Size = new Size(150, 27);
            dtpDesde.TabIndex = 2;
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(234, 56);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(47, 20);
            lblHasta.TabIndex = 3;
            lblHasta.Text = "Hasta";
            //
            // dtpHasta
            //
            dtpHasta.Checked = false;
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(288, 52);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.ShowCheckBox = true;
            dtpHasta.Size = new Size(150, 27);
            dtpHasta.TabIndex = 4;
            //
            // lblCodigo
            //
            lblCodigo.AutoSize = true;
            lblCodigo.Location = new Point(454, 56);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(58, 20);
            lblCodigo.TabIndex = 5;
            lblCodigo.Text = "Código";
            //
            // txtCodigo
            //
            txtCodigo.Location = new Point(518, 53);
            txtCodigo.MaxLength = 20;
            txtCodigo.Name = "txtCodigo";
            txtCodigo.Size = new Size(110, 27);
            txtCodigo.TabIndex = 6;
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(642, 56);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(64, 20);
            lblNombre.TabIndex = 7;
            lblNombre.Text = "Nombre";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(712, 53);
            txtNombre.MaxLength = 120;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(200, 27);
            txtNombre.TabIndex = 8;
            //
            // btnBuscar
            //
            btnBuscar.Location = new Point(926, 50);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(110, 34);
            btnBuscar.TabIndex = 9;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // dgvCambios
            //
            dgvCambios.AllowUserToAddRows = false;
            dgvCambios.AllowUserToDeleteRows = false;
            dgvCambios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCambios.BackgroundColor = SystemColors.Window;
            dgvCambios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCambios.Columns.AddRange(new DataGridViewColumn[] { colAct, colCodigo, colNombre, colOperacion, colFechaHora, colUsuario, colCategoria, colProveedor, colStock, colStockMinimo, colCosto, colVenta, colVencimiento, colLote });
            dgvCambios.Location = new Point(12, 96);
            dgvCambios.Name = "dgvCambios";
            dgvCambios.ReadOnly = true;
            dgvCambios.RowHeadersVisible = false;
            dgvCambios.RowHeadersWidth = 51;
            dgvCambios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCambios.Size = new Size(1126, 410);
            dgvCambios.TabIndex = 10;
            dgvCambios.DataBindingComplete += dgvCambios_DataBindingComplete;
            //
            // colAct
            //
            colAct.DataPropertyName = "Act";
            colAct.HeaderText = "Vigente";
            colAct.MinimumWidth = 6;
            colAct.Name = "colAct";
            colAct.ReadOnly = true;
            colAct.Width = 65;
            //
            // colCodigo
            //
            colCodigo.DataPropertyName = "Codigo";
            colCodigo.HeaderText = "Código";
            colCodigo.MinimumWidth = 6;
            colCodigo.Name = "colCodigo";
            colCodigo.ReadOnly = true;
            colCodigo.Width = 90;
            //
            // colNombre
            //
            colNombre.DataPropertyName = "Nombre";
            colNombre.HeaderText = "Nombre";
            colNombre.MinimumWidth = 6;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            colNombre.Width = 210;
            //
            // colOperacion
            //
            colOperacion.DataPropertyName = "Operacion";
            colOperacion.HeaderText = "Operación";
            colOperacion.MinimumWidth = 6;
            colOperacion.Name = "colOperacion";
            colOperacion.ReadOnly = true;
            colOperacion.Width = 110;
            //
            // colFechaHora
            //
            colFechaHora.DataPropertyName = "FechaHora";
            estiloFecha.Format = "dd/MM/yyyy HH:mm:ss";
            colFechaHora.DefaultCellStyle = estiloFecha;
            colFechaHora.HeaderText = "Fecha y hora";
            colFechaHora.MinimumWidth = 6;
            colFechaHora.Name = "colFechaHora";
            colFechaHora.ReadOnly = true;
            colFechaHora.Width = 150;
            //
            // colUsuario
            //
            colUsuario.DataPropertyName = "Usuario";
            colUsuario.HeaderText = "Usuario";
            colUsuario.MinimumWidth = 6;
            colUsuario.Name = "colUsuario";
            colUsuario.ReadOnly = true;
            colUsuario.Width = 90;
            //
            // colCategoria
            //
            colCategoria.DataPropertyName = "Categoria";
            colCategoria.HeaderText = "Categoría";
            colCategoria.MinimumWidth = 6;
            colCategoria.Name = "colCategoria";
            colCategoria.ReadOnly = true;
            colCategoria.Width = 120;
            //
            // colProveedor
            //
            colProveedor.DataPropertyName = "Proveedor";
            colProveedor.HeaderText = "Proveedor";
            colProveedor.MinimumWidth = 6;
            colProveedor.Name = "colProveedor";
            colProveedor.ReadOnly = true;
            colProveedor.Width = 160;
            //
            // colStock
            //
            colStock.DataPropertyName = "StockActual";
            colStock.HeaderText = "Stock";
            colStock.MinimumWidth = 6;
            colStock.Name = "colStock";
            colStock.ReadOnly = true;
            colStock.Width = 65;
            //
            // colStockMinimo
            //
            colStockMinimo.DataPropertyName = "StockMinimo";
            colStockMinimo.HeaderText = "Mínimo";
            colStockMinimo.MinimumWidth = 6;
            colStockMinimo.Name = "colStockMinimo";
            colStockMinimo.ReadOnly = true;
            colStockMinimo.Width = 70;
            //
            // colCosto
            //
            colCosto.DataPropertyName = "PrecioCosto";
            estiloCosto.Format = "N2";
            colCosto.DefaultCellStyle = estiloCosto;
            colCosto.HeaderText = "Costo";
            colCosto.MinimumWidth = 6;
            colCosto.Name = "colCosto";
            colCosto.ReadOnly = true;
            colCosto.Width = 90;
            //
            // colVenta
            //
            colVenta.DataPropertyName = "PrecioVenta";
            estiloVenta.Format = "N2";
            colVenta.DefaultCellStyle = estiloVenta;
            colVenta.HeaderText = "Venta";
            colVenta.MinimumWidth = 6;
            colVenta.Name = "colVenta";
            colVenta.ReadOnly = true;
            colVenta.Width = 90;
            //
            // colVencimiento
            //
            colVencimiento.DataPropertyName = "FechaVencimiento";
            estiloVencimiento.Format = "dd/MM/yyyy";
            colVencimiento.DefaultCellStyle = estiloVencimiento;
            colVencimiento.HeaderText = "Vencimiento";
            colVencimiento.MinimumWidth = 6;
            colVencimiento.Name = "colVencimiento";
            colVencimiento.ReadOnly = true;
            colVencimiento.Width = 105;
            //
            // colLote
            //
            colLote.DataPropertyName = "Lote";
            colLote.HeaderText = "Lote";
            colLote.MinimumWidth = 6;
            colLote.Name = "colLote";
            colLote.ReadOnly = true;
            colLote.Width = 90;
            //
            // lblAyuda
            //
            lblAyuda.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblAyuda.AutoSize = true;
            lblAyuda.ForeColor = Color.DimGray;
            lblAyuda.Location = new Point(12, 520);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(561, 20);
            lblAyuda.TabIndex = 11;
            lblAyuda.Text = "En verde, la versión vigente (Act = 1). Seleccione dos versiones con Ctrl para compararlas.";
            //
            // btnComparar
            //
            btnComparar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnComparar.Location = new Point(818, 514);
            btnComparar.Name = "btnComparar";
            btnComparar.Size = new Size(150, 34);
            btnComparar.TabIndex = 12;
            btnComparar.Text = "Comparar versiones";
            btnComparar.UseVisualStyleBackColor = true;
            btnComparar.Click += btnComparar_Click;
            //
            // btnRestaurar
            //
            btnRestaurar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRestaurar.Location = new Point(978, 514);
            btnRestaurar.Name = "btnRestaurar";
            btnRestaurar.Size = new Size(160, 34);
            btnRestaurar.TabIndex = 13;
            btnRestaurar.Tag = "STK_PROD_MODIF";
            btnRestaurar.Text = "Restaurar versión";
            btnRestaurar.UseVisualStyleBackColor = true;
            btnRestaurar.Click += btnRestaurar_Click;
            //
            // FrmBitacoraCambios_60MN
            //
            AcceptButton = btnBuscar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1150, 560);
            Controls.Add(btnRestaurar);
            Controls.Add(btnComparar);
            Controls.Add(lblAyuda);
            Controls.Add(dgvCambios);
            Controls.Add(btnBuscar);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(txtCodigo);
            Controls.Add(lblCodigo);
            Controls.Add(dtpHasta);
            Controls.Add(lblHasta);
            Controls.Add(dtpDesde);
            Controls.Add(lblDesde);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(1080, 450);
            Name = "FrmBitacoraCambios_60MN";
            Text = "Bitácora de cambios";
            Load += FrmBitacoraCambios_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCambios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Label lblCodigo;
        private TextBox txtCodigo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Button btnBuscar;
        private DataGridView dgvCambios;
        private DataGridViewCheckBoxColumn colAct;
        private DataGridViewTextBoxColumn colCodigo;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colOperacion;
        private DataGridViewTextBoxColumn colFechaHora;
        private DataGridViewTextBoxColumn colUsuario;
        private DataGridViewTextBoxColumn colCategoria;
        private DataGridViewTextBoxColumn colProveedor;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colStockMinimo;
        private DataGridViewTextBoxColumn colCosto;
        private DataGridViewTextBoxColumn colVenta;
        private DataGridViewTextBoxColumn colVencimiento;
        private DataGridViewTextBoxColumn colLote;
        private Label lblAyuda;
        private Button btnComparar;
        private Button btnRestaurar;
    }
}
