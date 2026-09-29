namespace VetCare
{
    partial class FrmProducto_60MN
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
            components = new System.ComponentModel.Container();
            lblCodigo = new Label();
            txtCodigo = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblCategoria = new Label();
            cboCategoria = new ComboBox();
            lblProveedor = new Label();
            cboProveedor = new ComboBox();
            lblStockActual = new Label();
            nudStockActual = new NumericUpDown();
            lblStockMinimo = new Label();
            nudStockMinimo = new NumericUpDown();
            lblPrecioCosto = new Label();
            nudPrecioCosto = new NumericUpDown();
            lblPrecioVenta = new Label();
            nudPrecioVenta = new NumericUpDown();
            btnUsarSugerido = new Button();
            lblMargen = new Label();
            lblVencimiento = new Label();
            dtpVencimiento = new DateTimePicker();
            lblLote = new Label();
            txtLote = new TextBox();
            lblNota = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)nudStockActual).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStockMinimo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecioCosto).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecioVenta).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // lblCodigo
            //
            lblCodigo.AutoSize = true;
            lblCodigo.Location = new Point(24, 24);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(58, 20);
            lblCodigo.TabIndex = 0;
            lblCodigo.Text = "Código";
            //
            // txtCodigo
            //
            txtCodigo.Location = new Point(190, 21);
            txtCodigo.Name = "txtCodigo";
            txtCodigo.ReadOnly = true;
            txtCodigo.Size = new Size(200, 27);
            txtCodigo.TabIndex = 1;
            txtCodigo.TabStop = false;
            txtCodigo.Text = "(se genera al guardar)";
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(24, 64);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(73, 20);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre *";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(190, 61);
            txtNombre.MaxLength = 120;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(300, 27);
            txtNombre.TabIndex = 3;
            //
            // lblCategoria
            //
            lblCategoria.AutoSize = true;
            lblCategoria.Location = new Point(24, 104);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(83, 20);
            lblCategoria.TabIndex = 4;
            lblCategoria.Text = "Categoría *";
            //
            // cboCategoria
            //
            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoria.FormattingEnabled = true;
            cboCategoria.Location = new Point(190, 100);
            cboCategoria.Name = "cboCategoria";
            cboCategoria.Size = new Size(300, 28);
            cboCategoria.TabIndex = 5;
            cboCategoria.SelectedIndexChanged += PrecioBase_Changed;
            //
            // lblProveedor
            //
            lblProveedor.AutoSize = true;
            lblProveedor.Location = new Point(24, 144);
            lblProveedor.Name = "lblProveedor";
            lblProveedor.Size = new Size(86, 20);
            lblProveedor.TabIndex = 6;
            lblProveedor.Text = "Proveedor *";
            //
            // cboProveedor
            //
            cboProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProveedor.FormattingEnabled = true;
            cboProveedor.Location = new Point(190, 140);
            cboProveedor.Name = "cboProveedor";
            cboProveedor.Size = new Size(300, 28);
            cboProveedor.TabIndex = 7;
            //
            // lblStockActual
            //
            lblStockActual.AutoSize = true;
            lblStockActual.Location = new Point(24, 184);
            lblStockActual.Name = "lblStockActual";
            lblStockActual.Size = new Size(99, 20);
            lblStockActual.TabIndex = 8;
            lblStockActual.Text = "Stock actual *";
            //
            // nudStockActual
            //
            nudStockActual.Location = new Point(190, 181);
            nudStockActual.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudStockActual.Name = "nudStockActual";
            nudStockActual.Size = new Size(120, 27);
            nudStockActual.TabIndex = 9;
            nudStockActual.ThousandsSeparator = true;
            //
            // lblStockMinimo
            //
            lblStockMinimo.AutoSize = true;
            lblStockMinimo.Location = new Point(24, 224);
            lblStockMinimo.Name = "lblStockMinimo";
            lblStockMinimo.Size = new Size(109, 20);
            lblStockMinimo.TabIndex = 10;
            lblStockMinimo.Text = "Stock mínimo *";
            //
            // nudStockMinimo
            //
            nudStockMinimo.Location = new Point(190, 221);
            nudStockMinimo.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudStockMinimo.Name = "nudStockMinimo";
            nudStockMinimo.Size = new Size(120, 27);
            nudStockMinimo.TabIndex = 11;
            nudStockMinimo.ThousandsSeparator = true;
            //
            // lblPrecioCosto
            //
            lblPrecioCosto.AutoSize = true;
            lblPrecioCosto.Location = new Point(24, 264);
            lblPrecioCosto.Name = "lblPrecioCosto";
            lblPrecioCosto.Size = new Size(121, 20);
            lblPrecioCosto.TabIndex = 12;
            lblPrecioCosto.Text = "Precio de costo *";
            //
            // nudPrecioCosto
            //
            nudPrecioCosto.DecimalPlaces = 2;
            nudPrecioCosto.Location = new Point(190, 261);
            nudPrecioCosto.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            nudPrecioCosto.Name = "nudPrecioCosto";
            nudPrecioCosto.Size = new Size(150, 27);
            nudPrecioCosto.TabIndex = 13;
            nudPrecioCosto.ThousandsSeparator = true;
            nudPrecioCosto.ValueChanged += PrecioBase_Changed;
            //
            // lblPrecioVenta
            //
            lblPrecioVenta.AutoSize = true;
            lblPrecioVenta.Location = new Point(24, 304);
            lblPrecioVenta.Name = "lblPrecioVenta";
            lblPrecioVenta.Size = new Size(122, 20);
            lblPrecioVenta.TabIndex = 14;
            lblPrecioVenta.Text = "Precio de venta *";
            //
            // nudPrecioVenta
            //
            nudPrecioVenta.DecimalPlaces = 2;
            nudPrecioVenta.Location = new Point(190, 301);
            nudPrecioVenta.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            nudPrecioVenta.Name = "nudPrecioVenta";
            nudPrecioVenta.Size = new Size(150, 27);
            nudPrecioVenta.TabIndex = 15;
            nudPrecioVenta.ThousandsSeparator = true;
            nudPrecioVenta.ValueChanged += nudPrecioVenta_ValueChanged;
            //
            // btnUsarSugerido
            //
            btnUsarSugerido.Location = new Point(350, 298);
            btnUsarSugerido.Name = "btnUsarSugerido";
            btnUsarSugerido.Size = new Size(140, 32);
            btnUsarSugerido.TabIndex = 16;
            btnUsarSugerido.Text = "Usar sugerido";
            btnUsarSugerido.UseVisualStyleBackColor = true;
            btnUsarSugerido.Click += btnUsarSugerido_Click;
            //
            // lblMargen
            //
            lblMargen.ForeColor = Color.DimGray;
            lblMargen.Location = new Point(190, 334);
            lblMargen.Name = "lblMargen";
            lblMargen.Size = new Size(300, 24);
            lblMargen.TabIndex = 17;
            //
            // lblVencimiento
            //
            lblVencimiento.AutoSize = true;
            lblVencimiento.Location = new Point(24, 370);
            lblVencimiento.Name = "lblVencimiento";
            lblVencimiento.Size = new Size(150, 20);
            lblVencimiento.TabIndex = 18;
            lblVencimiento.Text = "Fecha de vencimiento *";
            //
            // dtpVencimiento
            //
            dtpVencimiento.Format = DateTimePickerFormat.Short;
            dtpVencimiento.Location = new Point(190, 366);
            dtpVencimiento.Name = "dtpVencimiento";
            dtpVencimiento.Size = new Size(150, 27);
            dtpVencimiento.TabIndex = 19;
            //
            // lblLote
            //
            lblLote.AutoSize = true;
            lblLote.Location = new Point(24, 410);
            lblLote.Name = "lblLote";
            lblLote.Size = new Size(48, 20);
            lblLote.TabIndex = 20;
            lblLote.Text = "Lote *";
            //
            // txtLote
            //
            txtLote.Location = new Point(190, 407);
            txtLote.MaxLength = 40;
            txtLote.Name = "txtLote";
            txtLote.Size = new Size(150, 27);
            txtLote.TabIndex = 21;
            //
            // lblNota
            //
            lblNota.ForeColor = Color.DimGray;
            lblNota.Location = new Point(24, 444);
            lblNota.Name = "lblNota";
            lblNota.Size = new Size(466, 44);
            lblNota.TabIndex = 22;
            //
            // btnAceptar
            //
            btnAceptar.Location = new Point(260, 494);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 34);
            btnAceptar.TabIndex = 23;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(380, 494);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 24;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmProducto_60MN
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(520, 546);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(lblNota);
            Controls.Add(txtLote);
            Controls.Add(lblLote);
            Controls.Add(dtpVencimiento);
            Controls.Add(lblVencimiento);
            Controls.Add(lblMargen);
            Controls.Add(btnUsarSugerido);
            Controls.Add(nudPrecioVenta);
            Controls.Add(lblPrecioVenta);
            Controls.Add(nudPrecioCosto);
            Controls.Add(lblPrecioCosto);
            Controls.Add(nudStockMinimo);
            Controls.Add(lblStockMinimo);
            Controls.Add(nudStockActual);
            Controls.Add(lblStockActual);
            Controls.Add(cboProveedor);
            Controls.Add(lblProveedor);
            Controls.Add(cboCategoria);
            Controls.Add(lblCategoria);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(txtCodigo);
            Controls.Add(lblCodigo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmProducto_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Producto";
            Load += FrmProducto_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)nudStockActual).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStockMinimo).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecioCosto).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecioVenta).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCodigo;
        private TextBox txtCodigo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblCategoria;
        private ComboBox cboCategoria;
        private Label lblProveedor;
        private ComboBox cboProveedor;
        private Label lblStockActual;
        private NumericUpDown nudStockActual;
        private Label lblStockMinimo;
        private NumericUpDown nudStockMinimo;
        private Label lblPrecioCosto;
        private NumericUpDown nudPrecioCosto;
        private Label lblPrecioVenta;
        private NumericUpDown nudPrecioVenta;
        private Button btnUsarSugerido;
        private Label lblMargen;
        private Label lblVencimiento;
        private DateTimePicker dtpVencimiento;
        private Label lblLote;
        private TextBox txtLote;
        private Label lblNota;
        private Button btnAceptar;
        private Button btnCancelar;
        private ErrorProvider errorProvider;
    }
}
