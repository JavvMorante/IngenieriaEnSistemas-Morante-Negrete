namespace VetCare
{
    partial class FrmBuscarCliente_60MN
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
            lblTitulo = new Label();
            lblCriterio = new Label();
            cboCriterio = new ComboBox();
            txtValor = new TextBox();
            btnBuscar = new Button();
            dgvClientes = new DataGridView();
            colDocumento = new DataGridViewTextBoxColumn();
            colApellido = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colTelefono = new DataGridViewTextBoxColumn();
            btnAceptar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(300, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Elija el cliente dueño de la mascota";
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
            cboCriterio.Size = new Size(180, 28);
            cboCriterio.TabIndex = 2;
            //
            // txtValor
            //
            txtValor.Location = new Point(290, 52);
            txtValor.MaxLength = 80;
            txtValor.Name = "txtValor";
            txtValor.Size = new Size(260, 27);
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
            // dgvClientes
            //
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;
            dgvClientes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.BackgroundColor = SystemColors.Window;
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClientes.Columns.AddRange(new DataGridViewColumn[] { colDocumento, colApellido, colNombre, colTelefono });
            dgvClientes.Location = new Point(12, 96);
            dgvClientes.MultiSelect = false;
            dgvClientes.Name = "dgvClientes";
            dgvClientes.ReadOnly = true;
            dgvClientes.RowHeadersVisible = false;
            dgvClientes.RowHeadersWidth = 51;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Size = new Size(676, 260);
            dgvClientes.TabIndex = 5;
            //
            // colDocumento
            //
            colDocumento.DataPropertyName = "Documento";
            colDocumento.HeaderText = "Documento";
            colDocumento.MinimumWidth = 6;
            colDocumento.Name = "colDocumento";
            colDocumento.ReadOnly = true;
            //
            // colApellido
            //
            colApellido.DataPropertyName = "Apellido";
            colApellido.HeaderText = "Apellido";
            colApellido.MinimumWidth = 6;
            colApellido.Name = "colApellido";
            colApellido.ReadOnly = true;
            //
            // colNombre
            //
            colNombre.DataPropertyName = "Nombre";
            colNombre.HeaderText = "Nombre";
            colNombre.MinimumWidth = 6;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            //
            // colTelefono
            //
            colTelefono.DataPropertyName = "Telefono";
            colTelefono.HeaderText = "Teléfono";
            colTelefono.MinimumWidth = 6;
            colTelefono.Name = "colTelefono";
            colTelefono.ReadOnly = true;
            //
            // btnAceptar
            //
            btnAceptar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAceptar.Enabled = false;
            btnAceptar.Location = new Point(458, 370);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 36);
            btnAceptar.TabIndex = 6;
            btnAceptar.Text = "Seleccionar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(578, 370);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 36);
            btnCancelar.TabIndex = 7;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmBuscarCliente_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(700, 418);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(dgvClientes);
            Controls.Add(btnBuscar);
            Controls.Add(txtValor);
            Controls.Add(cboCriterio);
            Controls.Add(lblCriterio);
            Controls.Add(lblTitulo);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(700, 380);
            Name = "FrmBuscarCliente_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Seleccionar cliente";
            Load += FrmBuscarCliente_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblCriterio;
        private ComboBox cboCriterio;
        private TextBox txtValor;
        private Button btnBuscar;
        private DataGridView dgvClientes;
        private DataGridViewTextBoxColumn colDocumento;
        private DataGridViewTextBoxColumn colApellido;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colTelefono;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}
