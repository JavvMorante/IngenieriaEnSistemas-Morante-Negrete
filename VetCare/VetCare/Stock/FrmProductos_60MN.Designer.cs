namespace VetCare
{
    partial class FrmProductos_60MN
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
            DataGridViewCellStyle estiloCosto = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloVenta = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloVencimiento = new DataGridViewCellStyle();
            lblTitulo = new Label();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            lblCategoria = new Label();
            cboCategoria = new ComboBox();
            chkInactivos = new CheckBox();
            btnBuscar = new Button();
            dgvProductos = new DataGridView();
            colCodigo = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colCategoria = new DataGridViewTextBoxColumn();
            colProveedor = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            colStockMinimo = new DataGridViewTextBoxColumn();
            colCosto = new DataGridViewTextBoxColumn();
            colVenta = new DataGridViewTextBoxColumn();
            colVencimiento = new DataGridViewTextBoxColumn();
            colLote = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            lblAlertas = new Label();
            btnNuevo = new Button();
            btnModificar = new Button();
            btnDarBaja = new Button();
            btnReactivar = new Button();
            btnMovimiento = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(263, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Productos e insumos (Stock)";
            //
            // lblBuscar
            //
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(12, 56);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(52, 20);
            lblBuscar.TabIndex = 1;
            lblBuscar.Text = "Buscar";
            //
            // txtBuscar
            //
            txtBuscar.Location = new Point(72, 53);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Código, nombre o lote";
            txtBuscar.Size = new Size(260, 27);
            txtBuscar.TabIndex = 2;
            //
            // lblCategoria
            //
            lblCategoria.AutoSize = true;
            lblCategoria.Location = new Point(350, 56);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(74, 20);
            lblCategoria.TabIndex = 3;
            lblCategoria.Text = "Categoría";
            //
            // cboCategoria
            //
            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoria.FormattingEnabled = true;
            cboCategoria.Location = new Point(430, 52);
            cboCategoria.Name = "cboCategoria";
            cboCategoria.Size = new Size(200, 28);
            cboCategoria.TabIndex = 4;
            //
            // chkInactivos
            //
            chkInactivos.AutoSize = true;
            chkInactivos.Location = new Point(650, 55);
            chkInactivos.Name = "chkInactivos";
            chkInactivos.Size = new Size(183, 24);
            chkInactivos.TabIndex = 5;
            chkInactivos.Text = "Incluir dados de baja";
            chkInactivos.UseVisualStyleBackColor = true;
            //
            // btnBuscar
            //
            btnBuscar.Location = new Point(850, 50);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(110, 34);
            btnBuscar.TabIndex = 6;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // dgvProductos
            //
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.BackgroundColor = SystemColors.Window;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Columns.AddRange(new DataGridViewColumn[] { colCodigo, colNombre, colCategoria, colProveedor, colStock, colStockMinimo, colCosto, colVenta, colVencimiento, colLote, colEstado });
            dgvProductos.Location = new Point(12, 96);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.RowHeadersWidth = 51;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(1126, 404);
            dgvProductos.TabIndex = 7;
            dgvProductos.CellDoubleClick += dgvProductos_CellDoubleClick;
            dgvProductos.DataBindingComplete += dgvProductos_DataBindingComplete;
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
            colNombre.FillWeight = 170F;
            colNombre.HeaderText = "Nombre";
            colNombre.MinimumWidth = 6;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            //
            // colCategoria
            //
            colCategoria.DataPropertyName = "CategoriaNombre";
            colCategoria.FillWeight = 80F;
            colCategoria.HeaderText = "Categoría";
            colCategoria.MinimumWidth = 6;
            colCategoria.Name = "colCategoria";
            colCategoria.ReadOnly = true;
            //
            // colProveedor
            //
            colProveedor.DataPropertyName = "ProveedorNombre";
            colProveedor.FillWeight = 110F;
            colProveedor.HeaderText = "Proveedor";
            colProveedor.MinimumWidth = 6;
            colProveedor.Name = "colProveedor";
            colProveedor.ReadOnly = true;
            //
            // colStock
            //
            colStock.DataPropertyName = "StockActual";
            colStock.FillWeight = 45F;
            colStock.HeaderText = "Stock";
            colStock.MinimumWidth = 6;
            colStock.Name = "colStock";
            colStock.ReadOnly = true;
            //
            // colStockMinimo
            //
            colStockMinimo.DataPropertyName = "StockMinimo";
            colStockMinimo.FillWeight = 45F;
            colStockMinimo.HeaderText = "Mínimo";
            colStockMinimo.MinimumWidth = 6;
            colStockMinimo.Name = "colStockMinimo";
            colStockMinimo.ReadOnly = true;
            //
            // colCosto
            //
            colCosto.DataPropertyName = "PrecioCosto";
            estiloCosto.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloCosto.Format = "N2";
            colCosto.DefaultCellStyle = estiloCosto;
            colCosto.FillWeight = 60F;
            colCosto.HeaderText = "Costo";
            colCosto.MinimumWidth = 6;
            colCosto.Name = "colCosto";
            colCosto.ReadOnly = true;
            //
            // colVenta
            //
            colVenta.DataPropertyName = "PrecioVenta";
            estiloVenta.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloVenta.Format = "N2";
            colVenta.DefaultCellStyle = estiloVenta;
            colVenta.FillWeight = 60F;
            colVenta.HeaderText = "Venta";
            colVenta.MinimumWidth = 6;
            colVenta.Name = "colVenta";
            colVenta.ReadOnly = true;
            //
            // colVencimiento
            //
            colVencimiento.DataPropertyName = "FechaVencimiento";
            estiloVencimiento.Format = "dd/MM/yyyy";
            colVencimiento.DefaultCellStyle = estiloVencimiento;
            colVencimiento.FillWeight = 70F;
            colVencimiento.HeaderText = "Vencimiento";
            colVencimiento.MinimumWidth = 6;
            colVencimiento.Name = "colVencimiento";
            colVencimiento.ReadOnly = true;
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
            // colEstado
            //
            colEstado.DataPropertyName = "Estado";
            colEstado.FillWeight = 45F;
            colEstado.HeaderText = "Estado";
            colEstado.MinimumWidth = 6;
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // lblAlertas
            //
            lblAlertas.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblAlertas.AutoSize = true;
            lblAlertas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAlertas.ForeColor = Color.Firebrick;
            lblAlertas.Location = new Point(12, 516);
            lblAlertas.Name = "lblAlertas";
            lblAlertas.Size = new Size(0, 20);
            lblAlertas.TabIndex = 8;
            //
            // btnNuevo
            //
            btnNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnNuevo.Location = new Point(458, 510);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(130, 34);
            btnNuevo.TabIndex = 9;
            btnNuevo.Tag = "STK_PROD_ALTA";
            btnNuevo.Text = "Nuevo producto";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnModificar
            //
            btnModificar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnModificar.Location = new Point(594, 510);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(130, 34);
            btnModificar.TabIndex = 10;
            btnModificar.Tag = "STK_PROD_MODIF";
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            //
            // btnDarBaja
            //
            btnDarBaja.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDarBaja.Location = new Point(730, 510);
            btnDarBaja.Name = "btnDarBaja";
            btnDarBaja.Size = new Size(130, 34);
            btnDarBaja.TabIndex = 11;
            btnDarBaja.Tag = "STK_PROD_MODIF";
            btnDarBaja.Text = "Dar de baja";
            btnDarBaja.UseVisualStyleBackColor = true;
            btnDarBaja.Click += btnDarBaja_Click;
            //
            // btnReactivar
            //
            btnReactivar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnReactivar.Location = new Point(866, 510);
            btnReactivar.Name = "btnReactivar";
            btnReactivar.Size = new Size(130, 34);
            btnReactivar.TabIndex = 12;
            btnReactivar.Tag = "STK_PROD_MODIF";
            btnReactivar.Text = "Reactivar";
            btnReactivar.UseVisualStyleBackColor = true;
            btnReactivar.Click += btnReactivar_Click;
            //
            // btnMovimiento
            //
            btnMovimiento.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnMovimiento.Location = new Point(1002, 510);
            btnMovimiento.Name = "btnMovimiento";
            btnMovimiento.Size = new Size(136, 34);
            btnMovimiento.TabIndex = 13;
            btnMovimiento.Tag = "STK_MOVIMIENTO";
            btnMovimiento.Text = "Movimiento";
            btnMovimiento.UseVisualStyleBackColor = true;
            btnMovimiento.Click += btnMovimiento_Click;
            //
            // FrmProductos_60MN
            //
            AcceptButton = btnBuscar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1150, 556);
            Controls.Add(btnMovimiento);
            Controls.Add(btnReactivar);
            Controls.Add(btnDarBaja);
            Controls.Add(btnModificar);
            Controls.Add(btnNuevo);
            Controls.Add(lblAlertas);
            Controls.Add(dgvProductos);
            Controls.Add(btnBuscar);
            Controls.Add(chkInactivos);
            Controls.Add(cboCategoria);
            Controls.Add(lblCategoria);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(1100, 450);
            Name = "FrmProductos_60MN";
            Text = "Productos";
            Load += FrmProductos_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Label lblCategoria;
        private ComboBox cboCategoria;
        private CheckBox chkInactivos;
        private Button btnBuscar;
        private DataGridView dgvProductos;
        private DataGridViewTextBoxColumn colCodigo;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colCategoria;
        private DataGridViewTextBoxColumn colProveedor;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colStockMinimo;
        private DataGridViewTextBoxColumn colCosto;
        private DataGridViewTextBoxColumn colVenta;
        private DataGridViewTextBoxColumn colVencimiento;
        private DataGridViewTextBoxColumn colLote;
        private DataGridViewTextBoxColumn colEstado;
        private Label lblAlertas;
        private Button btnNuevo;
        private Button btnModificar;
        private Button btnDarBaja;
        private Button btnReactivar;
        private Button btnMovimiento;
    }
}
