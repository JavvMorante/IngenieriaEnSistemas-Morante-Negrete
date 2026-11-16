namespace VetCare
{
    partial class FrmCotizacion_60MN
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
            DataGridViewCellStyle estiloCantidad = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloPrecio = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloImporte = new DataGridViewCellStyle();
            lblProveedor = new Label();
            cboProveedor = new ComboBox();
            lblNroCotizacion = new Label();
            txtNroCotizacion = new TextBox();
            lblPlazo = new Label();
            nudPlazo = new NumericUpDown();
            lblDias = new Label();
            lblCondicion = new Label();
            cboCondicion = new ComboBox();
            lblPrecios = new Label();
            dgvPrecios = new DataGridView();
            colProducto = new DataGridViewTextBoxColumn();
            colCantidad = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            colCostoActual = new DataGridViewTextBoxColumn();
            colSubtotal = new DataGridViewTextBoxColumn();
            lblTotal = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)nudPlazo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPrecios).BeginInit();
            SuspendLayout();
            //
            // lblProveedor
            //
            lblProveedor.AutoSize = true;
            lblProveedor.Location = new Point(24, 24);
            lblProveedor.Name = "lblProveedor";
            lblProveedor.Size = new Size(86, 20);
            lblProveedor.TabIndex = 0;
            lblProveedor.Text = "Proveedor *";
            //
            // cboProveedor
            //
            cboProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProveedor.Location = new Point(190, 20);
            cboProveedor.Name = "cboProveedor";
            cboProveedor.Size = new Size(380, 28);
            cboProveedor.TabIndex = 1;
            //
            // lblNroCotizacion
            //
            lblNroCotizacion.AutoSize = true;
            lblNroCotizacion.Location = new Point(24, 64);
            lblNroCotizacion.Name = "lblNroCotizacion";
            lblNroCotizacion.Size = new Size(118, 20);
            lblNroCotizacion.TabIndex = 2;
            lblNroCotizacion.Text = "N° cotización *";
            //
            // txtNroCotizacion
            //
            txtNroCotizacion.CharacterCasing = CharacterCasing.Upper;
            txtNroCotizacion.Location = new Point(190, 60);
            txtNroCotizacion.Name = "txtNroCotizacion";
            txtNroCotizacion.Size = new Size(200, 27);
            txtNroCotizacion.TabIndex = 3;
            //
            // lblPlazo
            //
            lblPlazo.AutoSize = true;
            lblPlazo.Location = new Point(24, 104);
            lblPlazo.Name = "lblPlazo";
            lblPlazo.Size = new Size(131, 20);
            lblPlazo.TabIndex = 4;
            lblPlazo.Text = "Plazo de entrega *";
            //
            // nudPlazo
            //
            nudPlazo.Location = new Point(190, 101);
            nudPlazo.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            nudPlazo.Name = "nudPlazo";
            nudPlazo.Size = new Size(90, 27);
            nudPlazo.TabIndex = 5;
            //
            // lblDias
            //
            lblDias.AutoSize = true;
            lblDias.Location = new Point(288, 104);
            lblDias.Name = "lblDias";
            lblDias.Size = new Size(36, 20);
            lblDias.TabIndex = 6;
            lblDias.Text = "días";
            //
            // lblCondicion
            //
            lblCondicion.AutoSize = true;
            lblCondicion.Location = new Point(24, 144);
            lblCondicion.Name = "lblCondicion";
            lblCondicion.Size = new Size(148, 20);
            lblCondicion.TabIndex = 7;
            lblCondicion.Text = "Condición de pago *";
            //
            // cboCondicion
            //
            cboCondicion.Location = new Point(190, 140);
            cboCondicion.Name = "cboCondicion";
            cboCondicion.Size = new Size(380, 28);
            cboCondicion.TabIndex = 8;
            //
            // lblPrecios
            //
            lblPrecios.AutoSize = true;
            lblPrecios.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPrecios.Location = new Point(24, 186);
            lblPrecios.Name = "lblPrecios";
            lblPrecios.Size = new Size(313, 20);
            lblPrecios.TabIndex = 9;
            lblPrecios.Text = "Precio unitario cotizado de cada producto";
            //
            // dgvPrecios
            //
            dgvPrecios.AllowUserToAddRows = false;
            dgvPrecios.AllowUserToDeleteRows = false;
            dgvPrecios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPrecios.BackgroundColor = SystemColors.Window;
            dgvPrecios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPrecios.Columns.AddRange(new DataGridViewColumn[] { colProducto, colCantidad, colPrecio, colCostoActual, colSubtotal });
            dgvPrecios.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvPrecios.Location = new Point(24, 212);
            dgvPrecios.MultiSelect = false;
            dgvPrecios.Name = "dgvPrecios";
            dgvPrecios.RowHeadersVisible = false;
            dgvPrecios.RowHeadersWidth = 51;
            dgvPrecios.Size = new Size(752, 240);
            dgvPrecios.TabIndex = 10;
            dgvPrecios.CellEndEdit += dgvPrecios_CellEndEdit;
            dgvPrecios.DataError += dgvPrecios_DataError;
            //
            // colProducto
            //
            colProducto.DataPropertyName = "Producto";
            colProducto.FillWeight = 190F;
            colProducto.HeaderText = "Producto";
            colProducto.MinimumWidth = 6;
            colProducto.Name = "colProducto";
            colProducto.ReadOnly = true;
            //
            // colCantidad
            //
            colCantidad.DataPropertyName = "Cantidad";
            estiloCantidad.Alignment = DataGridViewContentAlignment.MiddleRight;
            colCantidad.DefaultCellStyle = estiloCantidad;
            colCantidad.FillWeight = 60F;
            colCantidad.HeaderText = "Cantidad";
            colCantidad.MinimumWidth = 6;
            colCantidad.Name = "colCantidad";
            colCantidad.ReadOnly = true;
            //
            // colPrecio
            //
            colPrecio.DataPropertyName = "Precio";
            estiloPrecio.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloPrecio.BackColor = Color.LightYellow;
            estiloPrecio.Format = "N2";
            colPrecio.DefaultCellStyle = estiloPrecio;
            colPrecio.FillWeight = 80F;
            colPrecio.HeaderText = "Precio unitario $";
            colPrecio.MinimumWidth = 6;
            colPrecio.Name = "colPrecio";
            //
            // colCostoActual
            //
            colCostoActual.DataPropertyName = "CostoActual";
            estiloImporte.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloImporte.Format = "N2";
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
            colSubtotal.FillWeight = 80F;
            colSubtotal.HeaderText = "Subtotal $";
            colSubtotal.MinimumWidth = 6;
            colSubtotal.Name = "colSubtotal";
            colSubtotal.ReadOnly = true;
            //
            // lblTotal
            //
            lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotal.Location = new Point(24, 462);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(480, 28);
            lblTotal.TabIndex = 11;
            //
            // btnAceptar
            //
            btnAceptar.Location = new Point(546, 500);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 34);
            btnAceptar.TabIndex = 12;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(666, 500);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 13;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmCotizacion_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(800, 552);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(lblTotal);
            Controls.Add(dgvPrecios);
            Controls.Add(lblPrecios);
            Controls.Add(cboCondicion);
            Controls.Add(lblCondicion);
            Controls.Add(lblDias);
            Controls.Add(nudPlazo);
            Controls.Add(lblPlazo);
            Controls.Add(txtNroCotizacion);
            Controls.Add(lblNroCotizacion);
            Controls.Add(cboProveedor);
            Controls.Add(lblProveedor);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCotizacion_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cotización del proveedor";
            Load += FrmCotizacion_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)nudPlazo).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPrecios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblProveedor;
        private ComboBox cboProveedor;
        private Label lblNroCotizacion;
        private TextBox txtNroCotizacion;
        private Label lblPlazo;
        private NumericUpDown nudPlazo;
        private Label lblDias;
        private Label lblCondicion;
        private ComboBox cboCondicion;
        private Label lblPrecios;
        private DataGridView dgvPrecios;
        private DataGridViewTextBoxColumn colProducto;
        private DataGridViewTextBoxColumn colCantidad;
        private DataGridViewTextBoxColumn colPrecio;
        private DataGridViewTextBoxColumn colCostoActual;
        private DataGridViewTextBoxColumn colSubtotal;
        private Label lblTotal;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}
