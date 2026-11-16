namespace VetCare
{
    partial class FrmAprobarOrden_60MN
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
            DataGridViewCellStyle estiloAdvertencia = new DataGridViewCellStyle();
            lblTitulo = new Label();
            dgvOrdenes = new DataGridView();
            colNumero = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colProveedor = new DataGridViewTextBoxColumn();
            colUsuario = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colAdvertencias = new DataGridViewTextBoxColumn();
            lblDetalle = new Label();
            dgvDetalle = new DataGridView();
            colProducto = new DataGridViewTextBoxColumn();
            colCantidad = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            colCostoActual = new DataGridViewTextBoxColumn();
            colSubtotal = new DataGridViewTextBoxColumn();
            grpOrigen = new GroupBox();
            txtOrigen = new TextBox();
            lblMotivoRechazo = new Label();
            txtMotivoRechazo = new TextBox();
            btnAprobar = new Button();
            btnRechazar = new Button();
            lblResumen = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            grpOrigen.SuspendLayout();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(331, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Órdenes pendientes de aprobación";
            //
            // dgvOrdenes
            //
            dgvOrdenes.AllowUserToAddRows = false;
            dgvOrdenes.AllowUserToDeleteRows = false;
            dgvOrdenes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenes.BackgroundColor = SystemColors.Window;
            dgvOrdenes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrdenes.Columns.AddRange(new DataGridViewColumn[] { colNumero, colFecha, colProveedor, colUsuario, colTotal, colAdvertencias });
            dgvOrdenes.Location = new Point(12, 48);
            dgvOrdenes.MultiSelect = false;
            dgvOrdenes.Name = "dgvOrdenes";
            dgvOrdenes.ReadOnly = true;
            dgvOrdenes.RowHeadersVisible = false;
            dgvOrdenes.RowHeadersWidth = 51;
            dgvOrdenes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrdenes.Size = new Size(1126, 200);
            dgvOrdenes.TabIndex = 1;
            dgvOrdenes.SelectionChanged += dgvOrdenes_SelectionChanged;
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
            estiloFecha.Format = "dd/MM/yyyy HH:mm";
            colFecha.DefaultCellStyle = estiloFecha;
            colFecha.FillWeight = 75F;
            colFecha.HeaderText = "Fecha";
            colFecha.MinimumWidth = 6;
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colProveedor
            //
            colProveedor.DataPropertyName = "Proveedor";
            colProveedor.FillWeight = 140F;
            colProveedor.HeaderText = "Proveedor";
            colProveedor.MinimumWidth = 6;
            colProveedor.Name = "colProveedor";
            colProveedor.ReadOnly = true;
            //
            // colUsuario
            //
            colUsuario.DataPropertyName = "Usuario";
            colUsuario.FillWeight = 70F;
            colUsuario.HeaderText = "Registrada por";
            colUsuario.MinimumWidth = 6;
            colUsuario.Name = "colUsuario";
            colUsuario.ReadOnly = true;
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
            // colAdvertencias
            //
            colAdvertencias.DataPropertyName = "Advertencias";
            estiloAdvertencia.ForeColor = Color.DarkRed;
            colAdvertencias.DefaultCellStyle = estiloAdvertencia;
            colAdvertencias.FillWeight = 220F;
            colAdvertencias.HeaderText = "Advertencias";
            colAdvertencias.MinimumWidth = 6;
            colAdvertencias.Name = "colAdvertencias";
            colAdvertencias.ReadOnly = true;
            //
            // lblDetalle
            //
            lblDetalle.AutoSize = true;
            lblDetalle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDetalle.Location = new Point(12, 258);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(144, 20);
            lblDetalle.TabIndex = 2;
            lblDetalle.Text = "Detalle de la orden";
            //
            // dgvDetalle
            //
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.BackgroundColor = SystemColors.Window;
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalle.Columns.AddRange(new DataGridViewColumn[] { colProducto, colCantidad, colPrecio, colCostoActual, colSubtotal });
            dgvDetalle.Location = new Point(12, 282);
            dgvDetalle.MultiSelect = false;
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.ReadOnly = true;
            dgvDetalle.RowHeadersVisible = false;
            dgvDetalle.RowHeadersWidth = 51;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Size = new Size(730, 222);
            dgvDetalle.TabIndex = 3;
            //
            // colProducto
            //
            colProducto.DataPropertyName = "Producto";
            colProducto.FillWeight = 200F;
            colProducto.HeaderText = "Producto";
            colProducto.MinimumWidth = 6;
            colProducto.Name = "colProducto";
            colProducto.ReadOnly = true;
            //
            // colCantidad
            //
            colCantidad.DataPropertyName = "Cantidad";
            estiloNumero.Alignment = DataGridViewContentAlignment.MiddleRight;
            colCantidad.DefaultCellStyle = estiloNumero;
            colCantidad.FillWeight = 60F;
            colCantidad.HeaderText = "Cantidad";
            colCantidad.MinimumWidth = 6;
            colCantidad.Name = "colCantidad";
            colCantidad.ReadOnly = true;
            //
            // colPrecio
            //
            colPrecio.DataPropertyName = "PrecioUnitario";
            colPrecio.DefaultCellStyle = estiloImporte;
            colPrecio.FillWeight = 75F;
            colPrecio.HeaderText = "Precio unitario $";
            colPrecio.MinimumWidth = 6;
            colPrecio.Name = "colPrecio";
            colPrecio.ReadOnly = true;
            //
            // colCostoActual
            //
            colCostoActual.DataPropertyName = "PrecioCostoActual";
            colCostoActual.DefaultCellStyle = estiloImporte;
            colCostoActual.FillWeight = 75F;
            colCostoActual.HeaderText = "Costo actual $";
            colCostoActual.MinimumWidth = 6;
            colCostoActual.Name = "colCostoActual";
            colCostoActual.ReadOnly = true;
            //
            // colSubtotal
            //
            colSubtotal.DataPropertyName = "Subtotal";
            colSubtotal.DefaultCellStyle = estiloImporte;
            colSubtotal.FillWeight = 75F;
            colSubtotal.HeaderText = "Subtotal $";
            colSubtotal.MinimumWidth = 6;
            colSubtotal.Name = "colSubtotal";
            colSubtotal.ReadOnly = true;
            //
            // grpOrigen
            //
            grpOrigen.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            grpOrigen.Controls.Add(txtOrigen);
            grpOrigen.Location = new Point(754, 258);
            grpOrigen.Name = "grpOrigen";
            grpOrigen.Size = new Size(384, 246);
            grpOrigen.TabIndex = 4;
            grpOrigen.TabStop = false;
            grpOrigen.Text = "Origen";
            //
            // txtOrigen
            //
            txtOrigen.BackColor = SystemColors.Window;
            txtOrigen.Dock = DockStyle.Fill;
            txtOrigen.Multiline = true;
            txtOrigen.Name = "txtOrigen";
            txtOrigen.ReadOnly = true;
            txtOrigen.ScrollBars = ScrollBars.Vertical;
            txtOrigen.TabIndex = 0;
            txtOrigen.TabStop = false;
            //
            // lblMotivoRechazo
            //
            lblMotivoRechazo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblMotivoRechazo.AutoSize = true;
            lblMotivoRechazo.Location = new Point(12, 516);
            lblMotivoRechazo.Name = "lblMotivoRechazo";
            lblMotivoRechazo.Size = new Size(306, 20);
            lblMotivoRechazo.TabIndex = 5;
            lblMotivoRechazo.Text = "Motivo del rechazo (obligatorio para rechazar)";
            //
            // txtMotivoRechazo
            //
            txtMotivoRechazo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtMotivoRechazo.Location = new Point(12, 540);
            txtMotivoRechazo.Multiline = true;
            txtMotivoRechazo.Name = "txtMotivoRechazo";
            txtMotivoRechazo.Size = new Size(900, 56);
            txtMotivoRechazo.TabIndex = 6;
            //
            // btnAprobar
            //
            btnAprobar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAprobar.Location = new Point(924, 520);
            btnAprobar.Name = "btnAprobar";
            btnAprobar.Size = new Size(214, 34);
            btnAprobar.TabIndex = 7;
            btnAprobar.Tag = "CMP_APROBAR";
            btnAprobar.Text = "Aprobar";
            btnAprobar.UseVisualStyleBackColor = true;
            btnAprobar.Click += btnAprobar_Click;
            //
            // btnRechazar
            //
            btnRechazar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRechazar.Location = new Point(924, 562);
            btnRechazar.Name = "btnRechazar";
            btnRechazar.Size = new Size(214, 34);
            btnRechazar.TabIndex = 8;
            btnRechazar.Tag = "CMP_APROBAR";
            btnRechazar.Text = "Rechazar";
            btnRechazar.UseVisualStyleBackColor = true;
            btnRechazar.Click += btnRechazar_Click;
            //
            // lblResumen
            //
            lblResumen.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblResumen.ForeColor = Color.DimGray;
            lblResumen.Location = new Point(12, 604);
            lblResumen.Name = "lblResumen";
            lblResumen.Size = new Size(1126, 24);
            lblResumen.TabIndex = 9;
            lblResumen.Text = "Quien registró una orden no puede aprobarla ni rechazarla (separación de funciones).";
            //
            // FrmAprobarOrden_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1150, 636);
            Controls.Add(lblResumen);
            Controls.Add(btnRechazar);
            Controls.Add(btnAprobar);
            Controls.Add(txtMotivoRechazo);
            Controls.Add(lblMotivoRechazo);
            Controls.Add(grpOrigen);
            Controls.Add(dgvDetalle);
            Controls.Add(lblDetalle);
            Controls.Add(dgvOrdenes);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(1000, 620);
            Name = "FrmAprobarOrden_60MN";
            Text = "Aprobar órdenes de compra";
            Load += FrmAprobarOrden_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            grpOrigen.ResumeLayout(false);
            grpOrigen.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private DataGridView dgvOrdenes;
        private DataGridViewTextBoxColumn colNumero;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colProveedor;
        private DataGridViewTextBoxColumn colUsuario;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewTextBoxColumn colAdvertencias;
        private Label lblDetalle;
        private DataGridView dgvDetalle;
        private DataGridViewTextBoxColumn colProducto;
        private DataGridViewTextBoxColumn colCantidad;
        private DataGridViewTextBoxColumn colPrecio;
        private DataGridViewTextBoxColumn colCostoActual;
        private DataGridViewTextBoxColumn colSubtotal;
        private GroupBox grpOrigen;
        private TextBox txtOrigen;
        private Label lblMotivoRechazo;
        private TextBox txtMotivoRechazo;
        private Button btnAprobar;
        private Button btnRechazar;
        private Label lblResumen;
    }
}
