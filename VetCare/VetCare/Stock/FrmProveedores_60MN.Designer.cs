namespace VetCare
{
    partial class FrmProveedores_60MN
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
            DataGridViewCellStyle estiloProductosActivos = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloStock = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloVencimiento = new DataGridViewCellStyle();
            lblTitulo = new Label();
            lblCriterio = new Label();
            cboCriterio = new ComboBox();
            txtValor = new TextBox();
            btnBuscar = new Button();
            chkIncluirBajas = new CheckBox();
            dgvProveedores = new DataGridView();
            colRazonSocial = new DataGridViewTextBoxColumn();
            colCuit = new DataGridViewTextBoxColumn();
            colTelefono = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colContacto = new DataGridViewTextBoxColumn();
            colProductosActivos = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            lblProductos = new Label();
            dgvProductos = new DataGridView();
            colCodigo = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colCategoria = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            colLote = new DataGridViewTextBoxColumn();
            colVencimiento = new DataGridViewTextBoxColumn();
            colEstadoProducto = new DataGridViewTextBoxColumn();
            btnNuevo = new Button();
            btnModificar = new Button();
            btnDarBaja = new Button();
            btnReactivar = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProveedores).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(124, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Proveedores";
            //
            // lblCriterio
            //
            lblCriterio.AutoSize = true;
            lblCriterio.Location = new Point(12, 55);
            lblCriterio.Name = "lblCriterio";
            lblCriterio.Size = new Size(78, 20);
            lblCriterio.TabIndex = 1;
            lblCriterio.Text = "Buscar por";
            //
            // cboCriterio
            //
            cboCriterio.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCriterio.FormattingEnabled = true;
            cboCriterio.Location = new Point(100, 51);
            cboCriterio.Name = "cboCriterio";
            cboCriterio.Size = new Size(160, 28);
            cboCriterio.TabIndex = 2;
            //
            // txtValor
            //
            txtValor.Location = new Point(270, 52);
            txtValor.MaxLength = 120;
            txtValor.Name = "txtValor";
            txtValor.Size = new Size(280, 27);
            txtValor.TabIndex = 3;
            //
            // btnBuscar
            //
            btnBuscar.Location = new Point(560, 49);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(110, 34);
            btnBuscar.TabIndex = 4;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // chkIncluirBajas
            //
            chkIncluirBajas.AutoSize = true;
            chkIncluirBajas.Location = new Point(685, 54);
            chkIncluirBajas.Name = "chkIncluirBajas";
            chkIncluirBajas.Size = new Size(170, 24);
            chkIncluirBajas.TabIndex = 5;
            chkIncluirBajas.Text = "Incluir dados de baja";
            chkIncluirBajas.UseVisualStyleBackColor = true;
            chkIncluirBajas.CheckedChanged += chkIncluirBajas_CheckedChanged;
            //
            // dgvProveedores
            //
            dgvProveedores.AllowUserToAddRows = false;
            dgvProveedores.AllowUserToDeleteRows = false;
            dgvProveedores.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProveedores.BackgroundColor = SystemColors.Window;
            dgvProveedores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProveedores.Columns.AddRange(new DataGridViewColumn[] { colRazonSocial, colCuit, colTelefono, colEmail, colContacto, colProductosActivos, colEstado });
            dgvProveedores.Location = new Point(12, 96);
            dgvProveedores.MultiSelect = false;
            dgvProveedores.Name = "dgvProveedores";
            dgvProveedores.ReadOnly = true;
            dgvProveedores.RowHeadersVisible = false;
            dgvProveedores.RowHeadersWidth = 51;
            dgvProveedores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProveedores.Size = new Size(1126, 230);
            dgvProveedores.TabIndex = 6;
            dgvProveedores.CellDoubleClick += dgvProveedores_CellDoubleClick;
            dgvProveedores.DataBindingComplete += dgv_DataBindingComplete;
            dgvProveedores.SelectionChanged += dgvProveedores_SelectionChanged;
            //
            // colRazonSocial
            //
            colRazonSocial.DataPropertyName = "RazonSocial";
            colRazonSocial.FillWeight = 160F;
            colRazonSocial.HeaderText = "Razón social";
            colRazonSocial.MinimumWidth = 6;
            colRazonSocial.Name = "colRazonSocial";
            colRazonSocial.ReadOnly = true;
            //
            // colCuit
            //
            colCuit.DataPropertyName = "CuitFormateado";
            colCuit.FillWeight = 80F;
            colCuit.HeaderText = "CUIT";
            colCuit.MinimumWidth = 6;
            colCuit.Name = "colCuit";
            colCuit.ReadOnly = true;
            //
            // colTelefono
            //
            colTelefono.DataPropertyName = "Telefono";
            colTelefono.FillWeight = 75F;
            colTelefono.HeaderText = "Teléfono";
            colTelefono.MinimumWidth = 6;
            colTelefono.Name = "colTelefono";
            colTelefono.ReadOnly = true;
            //
            // colEmail
            //
            colEmail.DataPropertyName = "Email";
            colEmail.FillWeight = 120F;
            colEmail.HeaderText = "Email";
            colEmail.MinimumWidth = 6;
            colEmail.Name = "colEmail";
            colEmail.ReadOnly = true;
            //
            // colContacto
            //
            colContacto.DataPropertyName = "Contacto";
            colContacto.HeaderText = "Contacto";
            colContacto.MinimumWidth = 6;
            colContacto.Name = "colContacto";
            colContacto.ReadOnly = true;
            //
            // colProductosActivos
            //
            colProductosActivos.DataPropertyName = "ProductosActivos";
            estiloProductosActivos.Alignment = DataGridViewContentAlignment.MiddleRight;
            colProductosActivos.DefaultCellStyle = estiloProductosActivos;
            colProductosActivos.FillWeight = 60F;
            colProductosActivos.HeaderText = "Productos activos";
            colProductosActivos.MinimumWidth = 6;
            colProductosActivos.Name = "colProductosActivos";
            colProductosActivos.ReadOnly = true;
            //
            // colEstado
            //
            colEstado.DataPropertyName = "Estado";
            colEstado.FillWeight = 55F;
            colEstado.HeaderText = "Estado";
            colEstado.MinimumWidth = 6;
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // lblProductos
            //
            lblProductos.AutoSize = true;
            lblProductos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProductos.Location = new Point(12, 336);
            lblProductos.Name = "lblProductos";
            lblProductos.Size = new Size(143, 20);
            lblProductos.TabIndex = 7;
            lblProductos.Text = "Productos que provee";
            //
            // dgvProductos
            //
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.BackgroundColor = SystemColors.Window;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Columns.AddRange(new DataGridViewColumn[] { colCodigo, colNombre, colCategoria, colStock, colLote, colVencimiento, colEstadoProducto });
            dgvProductos.Location = new Point(12, 360);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.RowHeadersWidth = 51;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(1126, 180);
            dgvProductos.TabIndex = 8;
            dgvProductos.TabStop = false;
            dgvProductos.DataBindingComplete += dgv_DataBindingComplete;
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
            // colNombre
            //
            colNombre.DataPropertyName = "Nombre";
            colNombre.FillWeight = 180F;
            colNombre.HeaderText = "Nombre";
            colNombre.MinimumWidth = 6;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            //
            // colCategoria
            //
            colCategoria.DataPropertyName = "CategoriaNombre";
            colCategoria.HeaderText = "Categoría";
            colCategoria.MinimumWidth = 6;
            colCategoria.Name = "colCategoria";
            colCategoria.ReadOnly = true;
            //
            // colStock
            //
            colStock.DataPropertyName = "StockActual";
            estiloStock.Alignment = DataGridViewContentAlignment.MiddleRight;
            colStock.DefaultCellStyle = estiloStock;
            colStock.FillWeight = 55F;
            colStock.HeaderText = "Stock";
            colStock.MinimumWidth = 6;
            colStock.Name = "colStock";
            colStock.ReadOnly = true;
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
            // colEstadoProducto
            //
            colEstadoProducto.DataPropertyName = "Estado";
            colEstadoProducto.FillWeight = 55F;
            colEstadoProducto.HeaderText = "Estado";
            colEstadoProducto.MinimumWidth = 6;
            colEstadoProducto.Name = "colEstadoProducto";
            colEstadoProducto.ReadOnly = true;
            //
            // btnNuevo
            //
            btnNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNuevo.Location = new Point(12, 552);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(130, 36);
            btnNuevo.TabIndex = 9;
            btnNuevo.Tag = "MAE_PROVEEDORES";
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnModificar
            //
            btnModificar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnModificar.Location = new Point(152, 552);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(130, 36);
            btnModificar.TabIndex = 10;
            btnModificar.Tag = "MAE_PROVEEDORES";
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            //
            // btnDarBaja
            //
            btnDarBaja.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDarBaja.Location = new Point(292, 552);
            btnDarBaja.Name = "btnDarBaja";
            btnDarBaja.Size = new Size(130, 36);
            btnDarBaja.TabIndex = 11;
            btnDarBaja.Tag = "MAE_PROVEEDORES";
            btnDarBaja.Text = "Dar de baja";
            btnDarBaja.UseVisualStyleBackColor = true;
            btnDarBaja.Click += btnDarBaja_Click;
            //
            // btnReactivar
            //
            btnReactivar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnReactivar.Location = new Point(432, 552);
            btnReactivar.Name = "btnReactivar";
            btnReactivar.Size = new Size(130, 36);
            btnReactivar.TabIndex = 12;
            btnReactivar.Tag = "MAE_PROVEEDORES";
            btnReactivar.Text = "Reactivar";
            btnReactivar.UseVisualStyleBackColor = true;
            btnReactivar.Click += btnReactivar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(1028, 552);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 36);
            btnCerrar.TabIndex = 13;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmProveedores_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1150, 600);
            Controls.Add(btnCerrar);
            Controls.Add(btnReactivar);
            Controls.Add(btnDarBaja);
            Controls.Add(btnModificar);
            Controls.Add(btnNuevo);
            Controls.Add(dgvProductos);
            Controls.Add(lblProductos);
            Controls.Add(dgvProveedores);
            Controls.Add(chkIncluirBajas);
            Controls.Add(btnBuscar);
            Controls.Add(txtValor);
            Controls.Add(cboCriterio);
            Controls.Add(lblCriterio);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(900, 560);
            Name = "FrmProveedores_60MN";
            Text = "Proveedores";
            Load += FrmProveedores_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProveedores).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblCriterio;
        private ComboBox cboCriterio;
        private TextBox txtValor;
        private Button btnBuscar;
        private CheckBox chkIncluirBajas;
        private DataGridView dgvProveedores;
        private DataGridViewTextBoxColumn colRazonSocial;
        private DataGridViewTextBoxColumn colCuit;
        private DataGridViewTextBoxColumn colTelefono;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewTextBoxColumn colContacto;
        private DataGridViewTextBoxColumn colProductosActivos;
        private DataGridViewTextBoxColumn colEstado;
        private Label lblProductos;
        private DataGridView dgvProductos;
        private DataGridViewTextBoxColumn colCodigo;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colCategoria;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colLote;
        private DataGridViewTextBoxColumn colVencimiento;
        private DataGridViewTextBoxColumn colEstadoProducto;
        private Button btnNuevo;
        private Button btnModificar;
        private Button btnDarBaja;
        private Button btnReactivar;
        private Button btnCerrar;
    }
}
