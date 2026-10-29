namespace VetCare
{
    partial class FrmVentas_60MN
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
            DataGridViewCellStyle estiloTotal = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloUnidades = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloPrecio = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloSubtotal = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloCantidad = new DataGridViewCellStyle();
            lblTitulo = new Label();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            lblCliente = new Label();
            txtCliente = new TextBox();
            btnElegirCliente = new Button();
            btnQuitarCliente = new Button();
            btnBuscar = new Button();
            dgvVentas = new DataGridView();
            colNumero = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colCliente = new DataGridViewTextBoxColumn();
            colDocumento = new DataGridViewTextBoxColumn();
            colAtencion = new DataGridViewTextBoxColumn();
            colUsuario = new DataGridViewTextBoxColumn();
            colUnidades = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            lblResumen = new Label();
            lblDetalle = new Label();
            dgvDetalle = new DataGridView();
            colCodigo = new DataGridViewTextBoxColumn();
            colProducto = new DataGridViewTextBoxColumn();
            colLote = new DataGridViewTextBoxColumn();
            colCantidad = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            colSubtotal = new DataGridViewTextBoxColumn();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvVentas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(184, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Ventas registradas";
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
            // lblCliente
            //
            lblCliente.AutoSize = true;
            lblCliente.Location = new Point(414, 56);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(55, 20);
            lblCliente.TabIndex = 5;
            lblCliente.Text = "Cliente";
            //
            // txtCliente
            //
            txtCliente.Location = new Point(476, 52);
            txtCliente.Name = "txtCliente";
            txtCliente.ReadOnly = true;
            txtCliente.Size = new Size(300, 27);
            txtCliente.TabIndex = 6;
            txtCliente.TabStop = false;
            //
            // btnElegirCliente
            //
            btnElegirCliente.Location = new Point(784, 49);
            btnElegirCliente.Name = "btnElegirCliente";
            btnElegirCliente.Size = new Size(110, 34);
            btnElegirCliente.TabIndex = 7;
            btnElegirCliente.Text = "Elegir...";
            btnElegirCliente.UseVisualStyleBackColor = true;
            btnElegirCliente.Click += btnElegirCliente_Click;
            //
            // btnQuitarCliente
            //
            btnQuitarCliente.Location = new Point(900, 49);
            btnQuitarCliente.Name = "btnQuitarCliente";
            btnQuitarCliente.Size = new Size(110, 34);
            btnQuitarCliente.TabIndex = 8;
            btnQuitarCliente.Text = "Todos";
            btnQuitarCliente.UseVisualStyleBackColor = true;
            btnQuitarCliente.Click += btnQuitarCliente_Click;
            //
            // btnBuscar
            //
            btnBuscar.Location = new Point(1028, 49);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(110, 34);
            btnBuscar.TabIndex = 9;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // dgvVentas
            //
            dgvVentas.AllowUserToAddRows = false;
            dgvVentas.AllowUserToDeleteRows = false;
            dgvVentas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentas.BackgroundColor = SystemColors.Window;
            dgvVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVentas.Columns.AddRange(new DataGridViewColumn[] { colNumero, colFecha, colCliente, colDocumento, colAtencion, colUsuario, colUnidades, colTotal });
            dgvVentas.Location = new Point(12, 96);
            dgvVentas.MultiSelect = false;
            dgvVentas.Name = "dgvVentas";
            dgvVentas.ReadOnly = true;
            dgvVentas.RowHeadersVisible = false;
            dgvVentas.RowHeadersWidth = 51;
            dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentas.Size = new Size(1126, 240);
            dgvVentas.TabIndex = 10;
            dgvVentas.SelectionChanged += dgvVentas_SelectionChanged;
            //
            // colNumero
            //
            colNumero.DataPropertyName = "Numero";
            colNumero.FillWeight = 55F;
            colNumero.HeaderText = "Venta";
            colNumero.MinimumWidth = 6;
            colNumero.Name = "colNumero";
            colNumero.ReadOnly = true;
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
            // colCliente
            //
            colCliente.DataPropertyName = "Cliente";
            colCliente.FillWeight = 120F;
            colCliente.HeaderText = "Cliente";
            colCliente.MinimumWidth = 6;
            colCliente.Name = "colCliente";
            colCliente.ReadOnly = true;
            //
            // colDocumento
            //
            colDocumento.DataPropertyName = "DocumentoCliente";
            colDocumento.FillWeight = 80F;
            colDocumento.HeaderText = "Documento";
            colDocumento.MinimumWidth = 6;
            colDocumento.Name = "colDocumento";
            colDocumento.ReadOnly = true;
            //
            // colAtencion
            //
            colAtencion.DataPropertyName = "AtencionTexto";
            colAtencion.FillWeight = 110F;
            colAtencion.HeaderText = "Atención";
            colAtencion.MinimumWidth = 6;
            colAtencion.Name = "colAtencion";
            colAtencion.ReadOnly = true;
            //
            // colUsuario
            //
            colUsuario.DataPropertyName = "Usuario";
            colUsuario.FillWeight = 70F;
            colUsuario.HeaderText = "Usuario";
            colUsuario.MinimumWidth = 6;
            colUsuario.Name = "colUsuario";
            colUsuario.ReadOnly = true;
            //
            // colUnidades
            //
            colUnidades.DataPropertyName = "CantidadItems";
            estiloUnidades.Alignment = DataGridViewContentAlignment.MiddleRight;
            colUnidades.DefaultCellStyle = estiloUnidades;
            colUnidades.FillWeight = 55F;
            colUnidades.HeaderText = "Unidades";
            colUnidades.MinimumWidth = 6;
            colUnidades.Name = "colUnidades";
            colUnidades.ReadOnly = true;
            //
            // colTotal
            //
            colTotal.DataPropertyName = "Total";
            estiloTotal.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloTotal.Format = "N2";
            colTotal.DefaultCellStyle = estiloTotal;
            colTotal.FillWeight = 65F;
            colTotal.HeaderText = "Total $";
            colTotal.MinimumWidth = 6;
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            //
            // lblResumen
            //
            lblResumen.AutoSize = true;
            lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResumen.Location = new Point(12, 342);
            lblResumen.Name = "lblResumen";
            lblResumen.Size = new Size(0, 20);
            lblResumen.TabIndex = 11;
            //
            // lblDetalle
            //
            lblDetalle.AutoSize = true;
            lblDetalle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDetalle.Location = new Point(12, 372);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(131, 20);
            lblDetalle.TabIndex = 12;
            lblDetalle.Text = "Detalle de la venta";
            //
            // dgvDetalle
            //
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.BackgroundColor = SystemColors.Window;
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalle.Columns.AddRange(new DataGridViewColumn[] { colCodigo, colProducto, colLote, colCantidad, colPrecio, colSubtotal });
            dgvDetalle.Location = new Point(12, 396);
            dgvDetalle.MultiSelect = false;
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.ReadOnly = true;
            dgvDetalle.RowHeadersVisible = false;
            dgvDetalle.RowHeadersWidth = 51;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Size = new Size(1126, 150);
            dgvDetalle.TabIndex = 13;
            dgvDetalle.TabStop = false;
            //
            // colCodigo
            //
            colCodigo.DataPropertyName = "Codigo";
            colCodigo.FillWeight = 70F;
            colCodigo.HeaderText = "Código";
            colCodigo.MinimumWidth = 6;
            colCodigo.Name = "colCodigo";
            colCodigo.ReadOnly = true;
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
            // colLote
            //
            colLote.DataPropertyName = "Lote";
            colLote.FillWeight = 70F;
            colLote.HeaderText = "Lote";
            colLote.MinimumWidth = 6;
            colLote.Name = "colLote";
            colLote.ReadOnly = true;
            //
            // colCantidad
            //
            colCantidad.DataPropertyName = "Cantidad";
            estiloCantidad.Alignment = DataGridViewContentAlignment.MiddleRight;
            colCantidad.DefaultCellStyle = estiloCantidad;
            colCantidad.FillWeight = 55F;
            colCantidad.HeaderText = "Cantidad";
            colCantidad.MinimumWidth = 6;
            colCantidad.Name = "colCantidad";
            colCantidad.ReadOnly = true;
            //
            // colPrecio
            //
            colPrecio.DataPropertyName = "PrecioUnitario";
            estiloPrecio.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloPrecio.Format = "N2";
            colPrecio.DefaultCellStyle = estiloPrecio;
            colPrecio.FillWeight = 70F;
            colPrecio.HeaderText = "Precio unit.";
            colPrecio.MinimumWidth = 6;
            colPrecio.Name = "colPrecio";
            colPrecio.ReadOnly = true;
            //
            // colSubtotal
            //
            colSubtotal.DataPropertyName = "Subtotal";
            estiloSubtotal.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloSubtotal.Format = "N2";
            colSubtotal.DefaultCellStyle = estiloSubtotal;
            colSubtotal.FillWeight = 70F;
            colSubtotal.HeaderText = "Subtotal";
            colSubtotal.MinimumWidth = 6;
            colSubtotal.Name = "colSubtotal";
            colSubtotal.ReadOnly = true;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(1028, 556);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 36);
            btnCerrar.TabIndex = 14;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmVentas_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1150, 604);
            Controls.Add(btnCerrar);
            Controls.Add(dgvDetalle);
            Controls.Add(lblDetalle);
            Controls.Add(lblResumen);
            Controls.Add(dgvVentas);
            Controls.Add(btnBuscar);
            Controls.Add(btnQuitarCliente);
            Controls.Add(btnElegirCliente);
            Controls.Add(txtCliente);
            Controls.Add(lblCliente);
            Controls.Add(dtpHasta);
            Controls.Add(lblHasta);
            Controls.Add(dtpDesde);
            Controls.Add(lblDesde);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(900, 560);
            Name = "FrmVentas_60MN";
            Text = "Ventas registradas";
            Load += FrmVentas_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvVentas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Label lblCliente;
        private TextBox txtCliente;
        private Button btnElegirCliente;
        private Button btnQuitarCliente;
        private Button btnBuscar;
        private DataGridView dgvVentas;
        private DataGridViewTextBoxColumn colNumero;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colCliente;
        private DataGridViewTextBoxColumn colDocumento;
        private DataGridViewTextBoxColumn colAtencion;
        private DataGridViewTextBoxColumn colUsuario;
        private DataGridViewTextBoxColumn colUnidades;
        private DataGridViewTextBoxColumn colTotal;
        private Label lblResumen;
        private Label lblDetalle;
        private DataGridView dgvDetalle;
        private DataGridViewTextBoxColumn colCodigo;
        private DataGridViewTextBoxColumn colProducto;
        private DataGridViewTextBoxColumn colLote;
        private DataGridViewTextBoxColumn colCantidad;
        private DataGridViewTextBoxColumn colPrecio;
        private DataGridViewTextBoxColumn colSubtotal;
        private Button btnCerrar;
    }
}
