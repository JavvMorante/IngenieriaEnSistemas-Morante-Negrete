namespace VetCare
{
    partial class FrmSolicitudReposicion_60MN
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
            DataGridViewCellStyle estiloAPedir = new DataGridViewCellStyle();
            lblTitulo = new Label();
            lblAyuda = new Label();
            dgvProductos = new DataGridView();
            colIncluir = new DataGridViewCheckBoxColumn();
            colCodigo = new DataGridViewTextBoxColumn();
            colProducto = new DataGridViewTextBoxColumn();
            colProveedorHabitual = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            colMinimo = new DataGridViewTextBoxColumn();
            colSugerido = new DataGridViewTextBoxColumn();
            colAPedir = new DataGridViewTextBoxColumn();
            lblAgregar = new Label();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            cboResultados = new ComboBox();
            btnAgregar = new Button();
            lblObservaciones = new Label();
            txtObservaciones = new TextBox();
            lblResumen = new Label();
            btnEnviar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(236, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Solicitud de reposición";
            //
            // lblAyuda
            //
            lblAyuda.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblAyuda.ForeColor = Color.DimGray;
            lblAyuda.Location = new Point(12, 46);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(1076, 24);
            lblAyuda.TabIndex = 1;
            lblAyuda.Text = "Productos en alerta de stock mínimo. Marque los que quiere pedir y ajuste la columna \"A pedir\".";
            //
            // dgvProductos
            //
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.BackgroundColor = SystemColors.Window;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Columns.AddRange(new DataGridViewColumn[] { colIncluir, colCodigo, colProducto, colProveedorHabitual, colStock, colMinimo, colSugerido, colAPedir });
            dgvProductos.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvProductos.Location = new Point(12, 76);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.RowHeadersWidth = 51;
            dgvProductos.Size = new Size(1076, 330);
            dgvProductos.TabIndex = 2;
            dgvProductos.CurrentCellDirtyStateChanged += dgvProductos_CurrentCellDirtyStateChanged;
            dgvProductos.DataError += dgvProductos_DataError;
            //
            // colIncluir
            //
            colIncluir.DataPropertyName = "Incluido";
            colIncluir.FillWeight = 35F;
            colIncluir.HeaderText = "Pedir";
            colIncluir.MinimumWidth = 6;
            colIncluir.Name = "colIncluir";
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
            // colProducto
            //
            colProducto.DataPropertyName = "Producto";
            colProducto.FillWeight = 170F;
            colProducto.HeaderText = "Producto";
            colProducto.MinimumWidth = 6;
            colProducto.Name = "colProducto";
            colProducto.ReadOnly = true;
            //
            // colProveedorHabitual
            //
            colProveedorHabitual.DataPropertyName = "ProveedorHabitual";
            colProveedorHabitual.FillWeight = 130F;
            colProveedorHabitual.HeaderText = "Proveedor habitual";
            colProveedorHabitual.MinimumWidth = 6;
            colProveedorHabitual.Name = "colProveedorHabitual";
            colProveedorHabitual.ReadOnly = true;
            //
            // colStock
            //
            colStock.DataPropertyName = "StockActual";
            estiloNumero.Alignment = DataGridViewContentAlignment.MiddleRight;
            colStock.DefaultCellStyle = estiloNumero;
            colStock.FillWeight = 50F;
            colStock.HeaderText = "Stock";
            colStock.MinimumWidth = 6;
            colStock.Name = "colStock";
            colStock.ReadOnly = true;
            //
            // colMinimo
            //
            colMinimo.DataPropertyName = "StockMinimo";
            colMinimo.DefaultCellStyle = estiloNumero;
            colMinimo.FillWeight = 50F;
            colMinimo.HeaderText = "Mínimo";
            colMinimo.MinimumWidth = 6;
            colMinimo.Name = "colMinimo";
            colMinimo.ReadOnly = true;
            //
            // colSugerido
            //
            colSugerido.DataPropertyName = "CantidadSugerida";
            colSugerido.DefaultCellStyle = estiloNumero;
            colSugerido.FillWeight = 55F;
            colSugerido.HeaderText = "Sugerido";
            colSugerido.MinimumWidth = 6;
            colSugerido.Name = "colSugerido";
            colSugerido.ReadOnly = true;
            //
            // colAPedir
            //
            colAPedir.DataPropertyName = "CantidadPedida";
            estiloAPedir.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloAPedir.BackColor = Color.LightYellow;
            estiloAPedir.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            colAPedir.DefaultCellStyle = estiloAPedir;
            colAPedir.FillWeight = 55F;
            colAPedir.HeaderText = "A pedir";
            colAPedir.MinimumWidth = 6;
            colAPedir.Name = "colAPedir";
            //
            // lblAgregar
            //
            lblAgregar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblAgregar.AutoSize = true;
            lblAgregar.Location = new Point(12, 422);
            lblAgregar.Name = "lblAgregar";
            lblAgregar.Size = new Size(127, 20);
            lblAgregar.TabIndex = 3;
            lblAgregar.Text = "Agregar producto";
            //
            // txtBuscar
            //
            txtBuscar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtBuscar.Location = new Point(150, 418);
            txtBuscar.MaxLength = 80;
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(260, 27);
            txtBuscar.TabIndex = 4;
            //
            // btnBuscar
            //
            btnBuscar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnBuscar.Location = new Point(418, 415);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(110, 34);
            btnBuscar.TabIndex = 5;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // cboResultados
            //
            cboResultados.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cboResultados.DropDownStyle = ComboBoxStyle.DropDownList;
            cboResultados.Location = new Point(538, 418);
            cboResultados.Name = "cboResultados";
            cboResultados.Size = new Size(420, 28);
            cboResultados.TabIndex = 6;
            //
            // btnAgregar
            //
            btnAgregar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAgregar.Location = new Point(968, 415);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(120, 34);
            btnAgregar.TabIndex = 7;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = true;
            btnAgregar.Click += btnAgregar_Click;
            //
            // lblObservaciones
            //
            lblObservaciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblObservaciones.AutoSize = true;
            lblObservaciones.Location = new Point(12, 466);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(105, 20);
            lblObservaciones.TabIndex = 8;
            lblObservaciones.Text = "Observaciones";
            //
            // txtObservaciones
            //
            txtObservaciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtObservaciones.Location = new Point(150, 462);
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Size = new Size(938, 60);
            txtObservaciones.TabIndex = 9;
            //
            // lblResumen
            //
            lblResumen.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResumen.Location = new Point(12, 538);
            lblResumen.Name = "lblResumen";
            lblResumen.Size = new Size(780, 40);
            lblResumen.TabIndex = 10;
            //
            // btnEnviar
            //
            btnEnviar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnEnviar.Location = new Point(808, 538);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(160, 34);
            btnEnviar.TabIndex = 11;
            btnEnviar.Tag = "CMP_SOLICITUD";
            btnEnviar.Text = "Enviar solicitud";
            btnEnviar.UseVisualStyleBackColor = true;
            btnEnviar.Click += btnEnviar_Click;
            //
            // btnCancelar
            //
            btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancelar.Location = new Point(978, 538);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 12;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            //
            // FrmSolicitudReposicion_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 586);
            Controls.Add(btnCancelar);
            Controls.Add(btnEnviar);
            Controls.Add(lblResumen);
            Controls.Add(txtObservaciones);
            Controls.Add(lblObservaciones);
            Controls.Add(btnAgregar);
            Controls.Add(cboResultados);
            Controls.Add(btnBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(lblAgregar);
            Controls.Add(dgvProductos);
            Controls.Add(lblAyuda);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(900, 560);
            Name = "FrmSolicitudReposicion_60MN";
            Text = "Solicitud de reposición";
            Load += FrmSolicitudReposicion_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblAyuda;
        private DataGridView dgvProductos;
        private DataGridViewCheckBoxColumn colIncluir;
        private DataGridViewTextBoxColumn colCodigo;
        private DataGridViewTextBoxColumn colProducto;
        private DataGridViewTextBoxColumn colProveedorHabitual;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colMinimo;
        private DataGridViewTextBoxColumn colSugerido;
        private DataGridViewTextBoxColumn colAPedir;
        private Label lblAgregar;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private ComboBox cboResultados;
        private Button btnAgregar;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private Label lblResumen;
        private Button btnEnviar;
        private Button btnCancelar;
    }
}
