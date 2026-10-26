namespace VetCare
{
    partial class FrmCarrito_60MN
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
            DataGridViewCellStyle estiloPrecio = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloVence = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloPrecioCarrito = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloSubtotal = new DataGridViewCellStyle();
            lblAtencion = new Label();
            cboAtencion = new ComboBox();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            dgvProductos = new DataGridView();
            colCodigo = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colLote = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            colVence = new DataGridViewTextBoxColumn();
            lblCantidad = new Label();
            nudCantidad = new NumericUpDown();
            btnAgregar = new Button();
            lblCarrito = new Label();
            dgvCarrito = new DataGridView();
            colProductoCarrito = new DataGridViewTextBoxColumn();
            colLoteCarrito = new DataGridViewTextBoxColumn();
            colCantidadCarrito = new DataGridViewTextBoxColumn();
            colPrecioCarrito = new DataGridViewTextBoxColumn();
            colSubtotal = new DataGridViewTextBoxColumn();
            btnQuitar = new Button();
            lblTotal = new Label();
            btnConfirmar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCarrito).BeginInit();
            SuspendLayout();
            //
            // lblAtencion
            //
            lblAtencion.AutoSize = true;
            lblAtencion.Location = new Point(16, 18);
            lblAtencion.Name = "lblAtencion";
            lblAtencion.Size = new Size(66, 20);
            lblAtencion.TabIndex = 0;
            lblAtencion.Text = "Atención";
            //
            // cboAtencion
            //
            cboAtencion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAtencion.FormattingEnabled = true;
            cboAtencion.Location = new Point(110, 14);
            cboAtencion.Name = "cboAtencion";
            cboAtencion.Size = new Size(774, 28);
            cboAtencion.TabIndex = 1;
            //
            // lblBuscar
            //
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(16, 58);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(70, 20);
            lblBuscar.TabIndex = 2;
            lblBuscar.Text = "Producto";
            //
            // txtBuscar
            //
            txtBuscar.Location = new Point(110, 54);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(320, 27);
            txtBuscar.TabIndex = 3;
            txtBuscar.KeyDown += txtBuscar_KeyDown;
            //
            // btnBuscar
            //
            btnBuscar.Location = new Point(440, 52);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(100, 31);
            btnBuscar.TabIndex = 4;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // dgvProductos
            //
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.BackgroundColor = SystemColors.Window;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Columns.AddRange(new DataGridViewColumn[] { colCodigo, colNombre, colLote, colStock, colPrecio, colVence });
            dgvProductos.Location = new Point(16, 92);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.RowHeadersWidth = 51;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(868, 200);
            dgvProductos.TabIndex = 5;
            dgvProductos.CellDoubleClick += dgvProductos_CellDoubleClick;
            dgvProductos.CellFormatting += dgvProductos_CellFormatting;
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
            colNombre.FillWeight = 160F;
            colNombre.HeaderText = "Nombre";
            colNombre.MinimumWidth = 6;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
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
            // colPrecio
            //
            colPrecio.DataPropertyName = "PrecioVenta";
            estiloPrecio.Format = "N2";
            colPrecio.DefaultCellStyle = estiloPrecio;
            colPrecio.FillWeight = 60F;
            colPrecio.HeaderText = "Precio";
            colPrecio.MinimumWidth = 6;
            colPrecio.Name = "colPrecio";
            colPrecio.ReadOnly = true;
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
            // lblCantidad
            //
            lblCantidad.AutoSize = true;
            lblCantidad.Location = new Point(16, 306);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(69, 20);
            lblCantidad.TabIndex = 6;
            lblCantidad.Text = "Cantidad";
            //
            // nudCantidad
            //
            nudCantidad.Location = new Point(110, 302);
            nudCantidad.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudCantidad.Name = "nudCantidad";
            nudCantidad.Size = new Size(90, 27);
            nudCantidad.TabIndex = 7;
            nudCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // btnAgregar
            //
            btnAgregar.Location = new Point(212, 299);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(170, 33);
            btnAgregar.TabIndex = 8;
            btnAgregar.Text = "Agregar al carrito";
            btnAgregar.UseVisualStyleBackColor = true;
            btnAgregar.Click += btnAgregar_Click;
            //
            // lblCarrito
            //
            lblCarrito.AutoSize = true;
            lblCarrito.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCarrito.Location = new Point(16, 346);
            lblCarrito.Name = "lblCarrito";
            lblCarrito.Size = new Size(59, 20);
            lblCarrito.TabIndex = 9;
            lblCarrito.Text = "Carrito";
            //
            // dgvCarrito
            //
            dgvCarrito.AllowUserToAddRows = false;
            dgvCarrito.AllowUserToDeleteRows = false;
            dgvCarrito.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCarrito.BackgroundColor = SystemColors.Window;
            dgvCarrito.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCarrito.Columns.AddRange(new DataGridViewColumn[] { colProductoCarrito, colLoteCarrito, colCantidadCarrito, colPrecioCarrito, colSubtotal });
            dgvCarrito.Location = new Point(16, 370);
            dgvCarrito.MultiSelect = false;
            dgvCarrito.Name = "dgvCarrito";
            dgvCarrito.ReadOnly = true;
            dgvCarrito.RowHeadersVisible = false;
            dgvCarrito.RowHeadersWidth = 51;
            dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCarrito.Size = new Size(868, 160);
            dgvCarrito.TabIndex = 10;
            //
            // colProductoCarrito
            //
            colProductoCarrito.DataPropertyName = "Producto";
            colProductoCarrito.FillWeight = 180F;
            colProductoCarrito.HeaderText = "Producto";
            colProductoCarrito.MinimumWidth = 6;
            colProductoCarrito.Name = "colProductoCarrito";
            colProductoCarrito.ReadOnly = true;
            //
            // colLoteCarrito
            //
            colLoteCarrito.DataPropertyName = "Lote";
            colLoteCarrito.FillWeight = 60F;
            colLoteCarrito.HeaderText = "Lote";
            colLoteCarrito.MinimumWidth = 6;
            colLoteCarrito.Name = "colLoteCarrito";
            colLoteCarrito.ReadOnly = true;
            //
            // colCantidadCarrito
            //
            colCantidadCarrito.DataPropertyName = "Cantidad";
            colCantidadCarrito.FillWeight = 50F;
            colCantidadCarrito.HeaderText = "Cantidad";
            colCantidadCarrito.MinimumWidth = 6;
            colCantidadCarrito.Name = "colCantidadCarrito";
            colCantidadCarrito.ReadOnly = true;
            //
            // colPrecioCarrito
            //
            colPrecioCarrito.DataPropertyName = "PrecioUnitario";
            estiloPrecioCarrito.Format = "N2";
            colPrecioCarrito.DefaultCellStyle = estiloPrecioCarrito;
            colPrecioCarrito.FillWeight = 60F;
            colPrecioCarrito.HeaderText = "Precio unit.";
            colPrecioCarrito.MinimumWidth = 6;
            colPrecioCarrito.Name = "colPrecioCarrito";
            colPrecioCarrito.ReadOnly = true;
            //
            // colSubtotal
            //
            colSubtotal.DataPropertyName = "Subtotal";
            estiloSubtotal.Format = "N2";
            colSubtotal.DefaultCellStyle = estiloSubtotal;
            colSubtotal.FillWeight = 60F;
            colSubtotal.HeaderText = "Subtotal";
            colSubtotal.MinimumWidth = 6;
            colSubtotal.Name = "colSubtotal";
            colSubtotal.ReadOnly = true;
            //
            // btnQuitar
            //
            btnQuitar.Location = new Point(16, 540);
            btnQuitar.Name = "btnQuitar";
            btnQuitar.Size = new Size(140, 34);
            btnQuitar.TabIndex = 11;
            btnQuitar.Text = "Quitar ítem";
            btnQuitar.UseVisualStyleBackColor = true;
            btnQuitar.Click += btnQuitar_Click;
            //
            // lblTotal
            //
            lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotal.Location = new Point(170, 544);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(320, 26);
            lblTotal.TabIndex = 12;

            //
            // btnConfirmar
            //
            btnConfirmar.Location = new Point(614, 540);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(150, 34);
            btnConfirmar.TabIndex = 13;
            btnConfirmar.Text = "Confirmar entrega";
            btnConfirmar.UseVisualStyleBackColor = true;
            btnConfirmar.Click += btnConfirmar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(774, 540);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 14;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            //
            // FrmCarrito_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(900, 590);
            Controls.Add(btnCancelar);
            Controls.Add(btnConfirmar);
            Controls.Add(lblTotal);
            Controls.Add(btnQuitar);
            Controls.Add(dgvCarrito);
            Controls.Add(lblCarrito);
            Controls.Add(btnAgregar);
            Controls.Add(nudCantidad);
            Controls.Add(lblCantidad);
            Controls.Add(dgvProductos);
            Controls.Add(btnBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            Controls.Add(cboAtencion);
            Controls.Add(lblAtencion);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCarrito_60MN";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Carrito de medicación";
            Load += FrmCarrito_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCarrito).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblAtencion;
        private ComboBox cboAtencion;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private DataGridView dgvProductos;
        private DataGridViewTextBoxColumn colCodigo;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colLote;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colPrecio;
        private DataGridViewTextBoxColumn colVence;
        private Label lblCantidad;
        private NumericUpDown nudCantidad;
        private Button btnAgregar;
        private Label lblCarrito;
        private DataGridView dgvCarrito;
        private DataGridViewTextBoxColumn colProductoCarrito;
        private DataGridViewTextBoxColumn colLoteCarrito;
        private DataGridViewTextBoxColumn colCantidadCarrito;
        private DataGridViewTextBoxColumn colPrecioCarrito;
        private DataGridViewTextBoxColumn colSubtotal;
        private Button btnQuitar;
        private Label lblTotal;
        private Button btnConfirmar;
        private Button btnCancelar;
    }
}
