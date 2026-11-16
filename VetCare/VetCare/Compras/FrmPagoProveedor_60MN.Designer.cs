namespace VetCare
{
    partial class FrmPagoProveedor_60MN
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
            DataGridViewCellStyle estiloImporte = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloFecha = new DataGridViewCellStyle();
            lblTitulo = new Label();
            dgvOrdenes = new DataGridView();
            colNumero = new DataGridViewTextBoxColumn();
            colAprobada = new DataGridViewTextBoxColumn();
            colProveedor = new DataGridViewTextBoxColumn();
            colCondicion = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            btnEnviar = new Button();
            grpPago = new GroupBox();
            lblLeyenda = new Label();
            lblBanco = new Label();
            cboBanco = new ComboBox();
            lblComprobante = new Label();
            txtComprobante = new TextBox();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            lblImporte = new Label();
            nudImporte = new NumericUpDown();
            lblObservaciones = new Label();
            txtObservaciones = new TextBox();
            btnRegistrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).BeginInit();
            grpPago.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudImporte).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(200, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Pagos a proveedores";
            //
            // dgvOrdenes
            //
            dgvOrdenes.AllowUserToAddRows = false;
            dgvOrdenes.AllowUserToDeleteRows = false;
            dgvOrdenes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenes.BackgroundColor = SystemColors.Window;
            dgvOrdenes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrdenes.Columns.AddRange(new DataGridViewColumn[] { colNumero, colAprobada, colProveedor, colCondicion, colTotal, colEstado });
            dgvOrdenes.Location = new Point(12, 48);
            dgvOrdenes.MultiSelect = false;
            dgvOrdenes.Name = "dgvOrdenes";
            dgvOrdenes.ReadOnly = true;
            dgvOrdenes.RowHeadersVisible = false;
            dgvOrdenes.RowHeadersWidth = 51;
            dgvOrdenes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrdenes.Size = new Size(1076, 230);
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
            // colAprobada
            //
            colAprobada.DataPropertyName = "FechaAprobacion";
            estiloFecha.Format = "dd/MM/yyyy";
            colAprobada.DefaultCellStyle = estiloFecha;
            colAprobada.FillWeight = 65F;
            colAprobada.HeaderText = "Aprobada";
            colAprobada.MinimumWidth = 6;
            colAprobada.Name = "colAprobada";
            colAprobada.ReadOnly = true;
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
            // colCondicion
            //
            colCondicion.DataPropertyName = "CondicionPago";
            colCondicion.FillWeight = 110F;
            colCondicion.HeaderText = "Condición de pago";
            colCondicion.MinimumWidth = 6;
            colCondicion.Name = "colCondicion";
            colCondicion.ReadOnly = true;
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
            // colEstado
            //
            colEstado.DataPropertyName = "EstadoTexto";
            colEstado.FillWeight = 70F;
            colEstado.HeaderText = "Estado";
            colEstado.MinimumWidth = 6;
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // btnEnviar
            //
            btnEnviar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEnviar.Location = new Point(12, 286);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(300, 34);
            btnEnviar.TabIndex = 2;
            btnEnviar.Tag = "CMP_PAGO";
            btnEnviar.Text = "Enviar orden al proveedor (PDF)...";
            btnEnviar.UseVisualStyleBackColor = true;
            btnEnviar.Click += btnEnviar_Click;
            //
            // grpPago
            //
            grpPago.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpPago.Controls.Add(btnRegistrar);
            grpPago.Controls.Add(txtObservaciones);
            grpPago.Controls.Add(lblObservaciones);
            grpPago.Controls.Add(nudImporte);
            grpPago.Controls.Add(lblImporte);
            grpPago.Controls.Add(dtpFecha);
            grpPago.Controls.Add(lblFecha);
            grpPago.Controls.Add(txtComprobante);
            grpPago.Controls.Add(lblComprobante);
            grpPago.Controls.Add(cboBanco);
            grpPago.Controls.Add(lblBanco);
            grpPago.Controls.Add(lblLeyenda);
            grpPago.Location = new Point(12, 330);
            grpPago.Name = "grpPago";
            grpPago.Size = new Size(1076, 236);
            grpPago.TabIndex = 3;
            grpPago.TabStop = false;
            grpPago.Text = "Registrar pago";
            //
            // lblLeyenda
            //
            lblLeyenda.ForeColor = Color.DimGray;
            lblLeyenda.Location = new Point(16, 28);
            lblLeyenda.Name = "lblLeyenda";
            lblLeyenda.Size = new Size(1040, 24);
            lblLeyenda.TabIndex = 0;
            lblLeyenda.Text = "El pago se hace por transferencia en el home banking del banco. Aquí solo se registra el comprobante.";
            //
            // lblBanco
            //
            lblBanco.AutoSize = true;
            lblBanco.Location = new Point(16, 66);
            lblBanco.Name = "lblBanco";
            lblBanco.Size = new Size(61, 20);
            lblBanco.TabIndex = 1;
            lblBanco.Text = "Banco *";
            //
            // cboBanco
            //
            cboBanco.Location = new Point(170, 62);
            cboBanco.Name = "cboBanco";
            cboBanco.Size = new Size(360, 28);
            cboBanco.TabIndex = 2;
            //
            // lblComprobante
            //
            lblComprobante.AutoSize = true;
            lblComprobante.Location = new Point(560, 66);
            lblComprobante.Name = "lblComprobante";
            lblComprobante.Size = new Size(137, 20);
            lblComprobante.TabIndex = 3;
            lblComprobante.Text = "N° comprobante *";
            //
            // txtComprobante
            //
            txtComprobante.CharacterCasing = CharacterCasing.Upper;
            txtComprobante.Location = new Point(714, 62);
            txtComprobante.Name = "txtComprobante";
            txtComprobante.Size = new Size(240, 27);
            txtComprobante.TabIndex = 4;
            //
            // lblFecha
            //
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(16, 106);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(129, 20);
            lblFecha.TabIndex = 5;
            lblFecha.Text = "Fecha del pago *";
            //
            // dtpFecha
            //
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(170, 102);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(150, 27);
            dtpFecha.TabIndex = 6;
            //
            // lblImporte
            //
            lblImporte.AutoSize = true;
            lblImporte.Location = new Point(560, 106);
            lblImporte.Name = "lblImporte";
            lblImporte.Size = new Size(88, 20);
            lblImporte.TabIndex = 7;
            lblImporte.Text = "Importe $ *";
            //
            // nudImporte
            //
            nudImporte.DecimalPlaces = 2;
            nudImporte.Location = new Point(714, 103);
            nudImporte.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            nudImporte.Name = "nudImporte";
            nudImporte.Size = new Size(180, 27);
            nudImporte.TabIndex = 8;
            nudImporte.ThousandsSeparator = true;
            //
            // lblObservaciones
            //
            lblObservaciones.AutoSize = true;
            lblObservaciones.Location = new Point(16, 146);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(105, 20);
            lblObservaciones.TabIndex = 9;
            lblObservaciones.Text = "Observaciones";
            //
            // txtObservaciones
            //
            txtObservaciones.Location = new Point(170, 142);
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Size = new Size(784, 27);
            txtObservaciones.TabIndex = 10;
            //
            // btnRegistrar
            //
            btnRegistrar.Location = new Point(804, 184);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(150, 34);
            btnRegistrar.TabIndex = 11;
            btnRegistrar.Tag = "CMP_PAGO";
            btnRegistrar.Text = "Registrar pago";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;
            //
            // FrmPagoProveedor_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 578);
            Controls.Add(grpPago);
            Controls.Add(btnEnviar);
            Controls.Add(dgvOrdenes);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(1000, 560);
            Name = "FrmPagoProveedor_60MN";
            Text = "Pagos a proveedores";
            Load += FrmPagoProveedor_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).EndInit();
            grpPago.ResumeLayout(false);
            grpPago.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudImporte).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private DataGridView dgvOrdenes;
        private DataGridViewTextBoxColumn colNumero;
        private DataGridViewTextBoxColumn colAprobada;
        private DataGridViewTextBoxColumn colProveedor;
        private DataGridViewTextBoxColumn colCondicion;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewTextBoxColumn colEstado;
        private Button btnEnviar;
        private GroupBox grpPago;
        private Label lblLeyenda;
        private Label lblBanco;
        private ComboBox cboBanco;
        private Label lblComprobante;
        private TextBox txtComprobante;
        private Label lblFecha;
        private DateTimePicker dtpFecha;
        private Label lblImporte;
        private NumericUpDown nudImporte;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private Button btnRegistrar;
    }
}
