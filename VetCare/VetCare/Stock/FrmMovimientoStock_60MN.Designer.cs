namespace VetCare
{
    partial class FrmMovimientoStock_60MN
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
            DataGridViewCellStyle estiloFecha = new DataGridViewCellStyle();
            lblTitulo = new Label();
            lblProducto = new Label();
            cboProducto = new ComboBox();
            lblInfoProducto = new Label();
            grpTipo = new GroupBox();
            rdoAjuste = new RadioButton();
            rdoEgreso = new RadioButton();
            rdoIngreso = new RadioButton();
            grpAjuste = new GroupBox();
            chkVencimiento = new CheckBox();
            rdoRestar = new RadioButton();
            rdoSumar = new RadioButton();
            lblCantidad = new Label();
            nudCantidad = new NumericUpDown();
            lblMotivo = new Label();
            txtMotivo = new TextBox();
            btnAceptar = new Button();
            btnCancelar = new Button();
            lblHistorial = new Label();
            dgvMovimientos = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colTipo = new DataGridViewTextBoxColumn();
            colCantidad = new DataGridViewTextBoxColumn();
            colAnterior = new DataGridViewTextBoxColumn();
            colResultante = new DataGridViewTextBoxColumn();
            colMotivo = new DataGridViewTextBoxColumn();
            errorProvider = new ErrorProvider(components);
            grpTipo.SuspendLayout();
            grpAjuste.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMovimientos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(209, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Movimientos de stock";
            //
            // lblProducto
            //
            lblProducto.AutoSize = true;
            lblProducto.Location = new Point(12, 58);
            lblProducto.Name = "lblProducto";
            lblProducto.Size = new Size(85, 20);
            lblProducto.TabIndex = 1;
            lblProducto.Text = "Producto *";
            //
            // cboProducto
            //
            cboProducto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProducto.FormattingEnabled = true;
            cboProducto.Location = new Point(110, 54);
            cboProducto.Name = "cboProducto";
            cboProducto.Size = new Size(560, 28);
            cboProducto.TabIndex = 2;
            cboProducto.SelectedIndexChanged += cboProducto_SelectedIndexChanged;
            //
            // lblInfoProducto
            //
            lblInfoProducto.ForeColor = Color.DarkSlateGray;
            lblInfoProducto.Location = new Point(110, 88);
            lblInfoProducto.Name = "lblInfoProducto";
            lblInfoProducto.Size = new Size(760, 24);
            lblInfoProducto.TabIndex = 3;
            //
            // grpTipo
            //
            grpTipo.Controls.Add(rdoAjuste);
            grpTipo.Controls.Add(rdoEgreso);
            grpTipo.Controls.Add(rdoIngreso);
            grpTipo.Location = new Point(12, 120);
            grpTipo.Name = "grpTipo";
            grpTipo.Size = new Size(300, 124);
            grpTipo.TabIndex = 4;
            grpTipo.TabStop = false;
            grpTipo.Text = "Tipo de movimiento";
            //
            // rdoAjuste
            //
            rdoAjuste.AutoSize = true;
            rdoAjuste.Location = new Point(16, 88);
            rdoAjuste.Name = "rdoAjuste";
            rdoAjuste.Size = new Size(71, 24);
            rdoAjuste.TabIndex = 2;
            rdoAjuste.Text = "Ajuste";
            rdoAjuste.UseVisualStyleBackColor = true;
            rdoAjuste.CheckedChanged += Tipo_CheckedChanged;
            //
            // rdoEgreso
            //
            rdoEgreso.AutoSize = true;
            rdoEgreso.Location = new Point(16, 58);
            rdoEgreso.Name = "rdoEgreso";
            rdoEgreso.Size = new Size(74, 24);
            rdoEgreso.TabIndex = 1;
            rdoEgreso.Text = "Egreso";
            rdoEgreso.UseVisualStyleBackColor = true;
            rdoEgreso.CheckedChanged += Tipo_CheckedChanged;
            //
            // rdoIngreso
            //
            rdoIngreso.AutoSize = true;
            rdoIngreso.Checked = true;
            rdoIngreso.Location = new Point(16, 28);
            rdoIngreso.Name = "rdoIngreso";
            rdoIngreso.Size = new Size(80, 24);
            rdoIngreso.TabIndex = 0;
            rdoIngreso.TabStop = true;
            rdoIngreso.Text = "Ingreso";
            rdoIngreso.UseVisualStyleBackColor = true;
            rdoIngreso.CheckedChanged += Tipo_CheckedChanged;
            //
            // grpAjuste
            //
            grpAjuste.Controls.Add(chkVencimiento);
            grpAjuste.Controls.Add(rdoRestar);
            grpAjuste.Controls.Add(rdoSumar);
            grpAjuste.Enabled = false;
            grpAjuste.Location = new Point(330, 120);
            grpAjuste.Name = "grpAjuste";
            grpAjuste.Size = new Size(540, 124);
            grpAjuste.TabIndex = 5;
            grpAjuste.TabStop = false;
            grpAjuste.Text = "Ajuste";
            //
            // chkVencimiento
            //
            chkVencimiento.AutoSize = true;
            chkVencimiento.Location = new Point(16, 88);
            chkVencimiento.Name = "chkVencimiento";
            chkVencimiento.Size = new Size(459, 24);
            chkVencimiento.TabIndex = 2;
            chkVencimiento.Text = "Ajuste por vencimiento: dar de baja el lote completo (stock a 0)";
            chkVencimiento.UseVisualStyleBackColor = true;
            chkVencimiento.CheckedChanged += chkVencimiento_CheckedChanged;
            //
            // rdoRestar
            //
            rdoRestar.AutoSize = true;
            rdoRestar.Location = new Point(16, 58);
            rdoRestar.Name = "rdoRestar";
            rdoRestar.Size = new Size(206, 24);
            rdoRestar.TabIndex = 1;
            rdoRestar.Text = "Restar (rotura, pérdida...)";
            rdoRestar.UseVisualStyleBackColor = true;
            //
            // rdoSumar
            //
            rdoSumar.AutoSize = true;
            rdoSumar.Checked = true;
            rdoSumar.Location = new Point(16, 28);
            rdoSumar.Name = "rdoSumar";
            rdoSumar.Size = new Size(236, 24);
            rdoSumar.TabIndex = 0;
            rdoSumar.TabStop = true;
            rdoSumar.Text = "Sumar (diferencia de inventario)";
            rdoSumar.UseVisualStyleBackColor = true;
            //
            // lblCantidad
            //
            lblCantidad.AutoSize = true;
            lblCantidad.Location = new Point(12, 262);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(79, 20);
            lblCantidad.TabIndex = 6;
            lblCantidad.Text = "Cantidad *";
            //
            // nudCantidad
            //
            nudCantidad.Location = new Point(110, 259);
            nudCantidad.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudCantidad.Name = "nudCantidad";
            nudCantidad.Size = new Size(120, 27);
            nudCantidad.TabIndex = 7;
            nudCantidad.ThousandsSeparator = true;
            //
            // lblMotivo
            //
            lblMotivo.AutoSize = true;
            lblMotivo.Location = new Point(12, 302);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.Size = new Size(68, 20);
            lblMotivo.TabIndex = 8;
            lblMotivo.Text = "Motivo *";
            //
            // txtMotivo
            //
            txtMotivo.Location = new Point(110, 299);
            txtMotivo.MaxLength = 150;
            txtMotivo.Name = "txtMotivo";
            txtMotivo.Size = new Size(560, 27);
            txtMotivo.TabIndex = 9;
            //
            // btnAceptar
            //
            btnAceptar.Location = new Point(640, 340);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 34);
            btnAceptar.TabIndex = 10;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.Location = new Point(760, 340);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "Cerrar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            //
            // lblHistorial
            //
            lblHistorial.AutoSize = true;
            lblHistorial.Location = new Point(12, 388);
            lblHistorial.Name = "lblHistorial";
            lblHistorial.Size = new Size(247, 20);
            lblHistorial.TabIndex = 12;
            lblHistorial.Text = "Últimos movimientos del producto";
            //
            // dgvMovimientos
            //
            dgvMovimientos.AllowUserToAddRows = false;
            dgvMovimientos.AllowUserToDeleteRows = false;
            dgvMovimientos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMovimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMovimientos.BackgroundColor = SystemColors.Window;
            dgvMovimientos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMovimientos.Columns.AddRange(new DataGridViewColumn[] { colFecha, colTipo, colCantidad, colAnterior, colResultante, colMotivo });
            dgvMovimientos.Location = new Point(12, 412);
            dgvMovimientos.Name = "dgvMovimientos";
            dgvMovimientos.ReadOnly = true;
            dgvMovimientos.RowHeadersVisible = false;
            dgvMovimientos.RowHeadersWidth = 51;
            dgvMovimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMovimientos.Size = new Size(858, 170);
            dgvMovimientos.TabIndex = 13;
            //
            // colFecha
            //
            colFecha.DataPropertyName = "FechaHora";
            estiloFecha.Format = "dd/MM/yyyy HH:mm";
            colFecha.DefaultCellStyle = estiloFecha;
            colFecha.FillWeight = 80F;
            colFecha.HeaderText = "Fecha y hora";
            colFecha.MinimumWidth = 6;
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colTipo
            //
            colTipo.DataPropertyName = "Tipo";
            colTipo.FillWeight = 50F;
            colTipo.HeaderText = "Tipo";
            colTipo.MinimumWidth = 6;
            colTipo.Name = "colTipo";
            colTipo.ReadOnly = true;
            //
            // colCantidad
            //
            colCantidad.DataPropertyName = "Cantidad";
            colCantidad.FillWeight = 45F;
            colCantidad.HeaderText = "Cantidad";
            colCantidad.MinimumWidth = 6;
            colCantidad.Name = "colCantidad";
            colCantidad.ReadOnly = true;
            //
            // colAnterior
            //
            colAnterior.DataPropertyName = "StockAnterior";
            colAnterior.FillWeight = 50F;
            colAnterior.HeaderText = "Stock anterior";
            colAnterior.MinimumWidth = 6;
            colAnterior.Name = "colAnterior";
            colAnterior.ReadOnly = true;
            //
            // colResultante
            //
            colResultante.DataPropertyName = "StockResultante";
            colResultante.FillWeight = 50F;
            colResultante.HeaderText = "Stock resultante";
            colResultante.MinimumWidth = 6;
            colResultante.Name = "colResultante";
            colResultante.ReadOnly = true;
            //
            // colMotivo
            //
            colMotivo.DataPropertyName = "Motivo";
            colMotivo.FillWeight = 160F;
            colMotivo.HeaderText = "Motivo";
            colMotivo.MinimumWidth = 6;
            colMotivo.Name = "colMotivo";
            colMotivo.ReadOnly = true;
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FrmMovimientoStock_60MN
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 596);
            Controls.Add(dgvMovimientos);
            Controls.Add(lblHistorial);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(txtMotivo);
            Controls.Add(lblMotivo);
            Controls.Add(nudCantidad);
            Controls.Add(lblCantidad);
            Controls.Add(grpAjuste);
            Controls.Add(grpTipo);
            Controls.Add(lblInfoProducto);
            Controls.Add(cboProducto);
            Controls.Add(lblProducto);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(900, 560);
            Name = "FrmMovimientoStock_60MN";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Movimientos de stock";
            Load += FrmMovimientoStock_60MN_Load;
            grpTipo.ResumeLayout(false);
            grpTipo.PerformLayout();
            grpAjuste.ResumeLayout(false);
            grpAjuste.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMovimientos).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblProducto;
        private ComboBox cboProducto;
        private Label lblInfoProducto;
        private GroupBox grpTipo;
        private RadioButton rdoAjuste;
        private RadioButton rdoEgreso;
        private RadioButton rdoIngreso;
        private GroupBox grpAjuste;
        private CheckBox chkVencimiento;
        private RadioButton rdoRestar;
        private RadioButton rdoSumar;
        private Label lblCantidad;
        private NumericUpDown nudCantidad;
        private Label lblMotivo;
        private TextBox txtMotivo;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblHistorial;
        private DataGridView dgvMovimientos;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colTipo;
        private DataGridViewTextBoxColumn colCantidad;
        private DataGridViewTextBoxColumn colAnterior;
        private DataGridViewTextBoxColumn colResultante;
        private DataGridViewTextBoxColumn colMotivo;
        private ErrorProvider errorProvider;
    }
}
