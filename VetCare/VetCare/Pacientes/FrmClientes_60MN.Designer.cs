namespace VetCare
{
    partial class FrmClientes_60MN
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
            DataGridViewCellStyle estiloNacimiento = new DataGridViewCellStyle();
            lblTitulo = new Label();
            lblCriterio = new Label();
            cboCriterio = new ComboBox();
            txtValor = new TextBox();
            btnBuscar = new Button();
            chkIncluirBajas = new CheckBox();
            dgvClientes = new DataGridView();
            colDocumento = new DataGridViewTextBoxColumn();
            colApellido = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colTelefono = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            grpFicha = new GroupBox();
            lblFicha = new Label();
            btnNuevoCliente = new Button();
            btnModificarCliente = new Button();
            btnBajaCliente = new Button();
            btnReactivarCliente = new Button();
            lblMascotas = new Label();
            dgvMascotas = new DataGridView();
            colMascota = new DataGridViewTextBoxColumn();
            colEspecie = new DataGridViewTextBoxColumn();
            colRaza = new DataGridViewTextBoxColumn();
            colSexo = new DataGridViewTextBoxColumn();
            colNacimiento = new DataGridViewTextBoxColumn();
            colHistoria = new DataGridViewTextBoxColumn();
            colEstadoMascota = new DataGridViewTextBoxColumn();
            btnAnadirMascota = new Button();
            btnModificarMascota = new Button();
            btnBajaMascota = new Button();
            btnHistoriaClinica = new Button();
            btnNuevaAtencion = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            grpFicha.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMascotas).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(195, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Clientes y mascotas";
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
            // chkIncluirBajas
            //
            chkIncluirBajas.AutoSize = true;
            chkIncluirBajas.Location = new Point(685, 54);
            chkIncluirBajas.Name = "chkIncluirBajas";
            chkIncluirBajas.Size = new Size(170, 24);
            chkIncluirBajas.TabIndex = 5;
            chkIncluirBajas.Text = "Incluir dados de baja";
            chkIncluirBajas.UseVisualStyleBackColor = true;
            //
            // dgvClientes
            //
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.BackgroundColor = SystemColors.Window;
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClientes.Columns.AddRange(new DataGridViewColumn[] { colDocumento, colApellido, colNombre, colTelefono, colEstado });
            dgvClientes.Location = new Point(12, 96);
            dgvClientes.MultiSelect = false;
            dgvClientes.Name = "dgvClientes";
            dgvClientes.ReadOnly = true;
            dgvClientes.RowHeadersVisible = false;
            dgvClientes.RowHeadersWidth = 51;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Size = new Size(760, 260);
            dgvClientes.TabIndex = 6;
            dgvClientes.DataBindingComplete += dgvClientes_DataBindingComplete;
            dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;
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
            // colEstado
            //
            colEstado.DataPropertyName = "Estado";
            colEstado.FillWeight = 60F;
            colEstado.HeaderText = "Estado";
            colEstado.MinimumWidth = 6;
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // grpFicha
            //
            grpFicha.Controls.Add(lblFicha);
            grpFicha.Location = new Point(786, 90);
            grpFicha.Name = "grpFicha";
            grpFicha.Size = new Size(352, 266);
            grpFicha.TabIndex = 7;
            grpFicha.TabStop = false;
            grpFicha.Text = "Ficha del cliente";
            //
            // lblFicha
            //
            lblFicha.Location = new Point(12, 26);
            lblFicha.Name = "lblFicha";
            lblFicha.Size = new Size(328, 228);
            lblFicha.TabIndex = 0;
            //
            // btnNuevoCliente
            //
            btnNuevoCliente.Location = new Point(12, 366);
            btnNuevoCliente.Name = "btnNuevoCliente";
            btnNuevoCliente.Size = new Size(124, 34);
            btnNuevoCliente.TabIndex = 8;
            btnNuevoCliente.Tag = "PAC_CLIENTE_ALTA";
            btnNuevoCliente.Text = "Nuevo cliente";
            btnNuevoCliente.UseVisualStyleBackColor = true;
            btnNuevoCliente.Click += btnNuevoCliente_Click;
            //
            // btnModificarCliente
            //
            btnModificarCliente.Location = new Point(142, 366);
            btnModificarCliente.Name = "btnModificarCliente";
            btnModificarCliente.Size = new Size(124, 34);
            btnModificarCliente.TabIndex = 9;
            btnModificarCliente.Tag = "PAC_CLIENTE_MODIF";
            btnModificarCliente.Text = "Modificar";
            btnModificarCliente.UseVisualStyleBackColor = true;
            btnModificarCliente.Click += btnModificarCliente_Click;
            //
            // btnBajaCliente
            //
            btnBajaCliente.Location = new Point(272, 366);
            btnBajaCliente.Name = "btnBajaCliente";
            btnBajaCliente.Size = new Size(124, 34);
            btnBajaCliente.TabIndex = 10;
            btnBajaCliente.Tag = "PAC_CLIENTE_MODIF";
            btnBajaCliente.Text = "Dar de baja";
            btnBajaCliente.UseVisualStyleBackColor = true;
            btnBajaCliente.Click += btnBajaCliente_Click;
            //
            // btnReactivarCliente
            //
            btnReactivarCliente.Location = new Point(402, 366);
            btnReactivarCliente.Name = "btnReactivarCliente";
            btnReactivarCliente.Size = new Size(124, 34);
            btnReactivarCliente.TabIndex = 11;
            btnReactivarCliente.Tag = "PAC_CLIENTE_MODIF";
            btnReactivarCliente.Text = "Reactivar";
            btnReactivarCliente.UseVisualStyleBackColor = true;
            btnReactivarCliente.Click += btnReactivarCliente_Click;
            //
            // lblMascotas
            //
            lblMascotas.AutoSize = true;
            lblMascotas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMascotas.Location = new Point(12, 414);
            lblMascotas.Name = "lblMascotas";
            lblMascotas.Size = new Size(156, 20);
            lblMascotas.TabIndex = 12;
            lblMascotas.Text = "Mascotas del cliente";
            //
            // dgvMascotas
            //
            dgvMascotas.AllowUserToAddRows = false;
            dgvMascotas.AllowUserToDeleteRows = false;
            dgvMascotas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMascotas.BackgroundColor = SystemColors.Window;
            dgvMascotas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMascotas.Columns.AddRange(new DataGridViewColumn[] { colMascota, colEspecie, colRaza, colSexo, colNacimiento, colHistoria, colEstadoMascota });
            dgvMascotas.Location = new Point(12, 438);
            dgvMascotas.MultiSelect = false;
            dgvMascotas.Name = "dgvMascotas";
            dgvMascotas.ReadOnly = true;
            dgvMascotas.RowHeadersVisible = false;
            dgvMascotas.RowHeadersWidth = 51;
            dgvMascotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMascotas.Size = new Size(980, 250);
            dgvMascotas.TabIndex = 13;
            dgvMascotas.CellDoubleClick += dgvMascotas_CellDoubleClick;
            dgvMascotas.DataBindingComplete += dgvMascotas_DataBindingComplete;
            //
            // colMascota
            //
            colMascota.DataPropertyName = "Nombre";
            colMascota.HeaderText = "Nombre";
            colMascota.MinimumWidth = 6;
            colMascota.Name = "colMascota";
            colMascota.ReadOnly = true;
            //
            // colEspecie
            //
            colEspecie.DataPropertyName = "Especie";
            colEspecie.HeaderText = "Especie";
            colEspecie.MinimumWidth = 6;
            colEspecie.Name = "colEspecie";
            colEspecie.ReadOnly = true;
            //
            // colRaza
            //
            colRaza.DataPropertyName = "Raza";
            colRaza.HeaderText = "Raza";
            colRaza.MinimumWidth = 6;
            colRaza.Name = "colRaza";
            colRaza.ReadOnly = true;
            //
            // colSexo
            //
            colSexo.DataPropertyName = "SexoTexto";
            colSexo.FillWeight = 60F;
            colSexo.HeaderText = "Sexo";
            colSexo.MinimumWidth = 6;
            colSexo.Name = "colSexo";
            colSexo.ReadOnly = true;
            //
            // colNacimiento
            //
            colNacimiento.DataPropertyName = "FechaNacimiento";
            estiloNacimiento.Format = "dd/MM/yyyy";
            colNacimiento.DefaultCellStyle = estiloNacimiento;
            colNacimiento.FillWeight = 80F;
            colNacimiento.HeaderText = "Nacimiento";
            colNacimiento.MinimumWidth = 6;
            colNacimiento.Name = "colNacimiento";
            colNacimiento.ReadOnly = true;
            //
            // colHistoria
            //
            colHistoria.DataPropertyName = "NumeroHistoriaClinica";
            colHistoria.FillWeight = 80F;
            colHistoria.HeaderText = "Historia clínica";
            colHistoria.MinimumWidth = 6;
            colHistoria.Name = "colHistoria";
            colHistoria.ReadOnly = true;
            //
            // colEstadoMascota
            //
            colEstadoMascota.DataPropertyName = "Estado";
            colEstadoMascota.FillWeight = 60F;
            colEstadoMascota.HeaderText = "Estado";
            colEstadoMascota.MinimumWidth = 6;
            colEstadoMascota.Name = "colEstadoMascota";
            colEstadoMascota.ReadOnly = true;
            //
            // btnAnadirMascota
            //
            btnAnadirMascota.Location = new Point(1004, 438);
            btnAnadirMascota.Name = "btnAnadirMascota";
            btnAnadirMascota.Size = new Size(134, 36);
            btnAnadirMascota.TabIndex = 14;
            btnAnadirMascota.Tag = "PAC_MASCOTA";
            btnAnadirMascota.Text = "Añadir mascota";
            btnAnadirMascota.UseVisualStyleBackColor = true;
            btnAnadirMascota.Click += btnAnadirMascota_Click;
            //
            // btnModificarMascota
            //
            btnModificarMascota.Location = new Point(1004, 482);
            btnModificarMascota.Name = "btnModificarMascota";
            btnModificarMascota.Size = new Size(134, 36);
            btnModificarMascota.TabIndex = 15;
            btnModificarMascota.Tag = "PAC_MASCOTA";
            btnModificarMascota.Text = "Modificar";
            btnModificarMascota.UseVisualStyleBackColor = true;
            btnModificarMascota.Click += btnModificarMascota_Click;
            //
            // btnBajaMascota
            //
            btnBajaMascota.Location = new Point(1004, 526);
            btnBajaMascota.Name = "btnBajaMascota";
            btnBajaMascota.Size = new Size(134, 36);
            btnBajaMascota.TabIndex = 16;
            btnBajaMascota.Tag = "PAC_MASCOTA";
            btnBajaMascota.Text = "Dar de baja";
            btnBajaMascota.UseVisualStyleBackColor = true;
            btnBajaMascota.Click += btnBajaMascota_Click;
            //
            // btnHistoriaClinica
            //
            btnHistoriaClinica.Location = new Point(1004, 596);
            btnHistoriaClinica.Name = "btnHistoriaClinica";
            btnHistoriaClinica.Size = new Size(134, 36);
            btnHistoriaClinica.TabIndex = 17;
            btnHistoriaClinica.Tag = "HC_ALTA|HC_CONSULTA";
            btnHistoriaClinica.Text = "Historia clínica";
            btnHistoriaClinica.UseVisualStyleBackColor = true;
            btnHistoriaClinica.Click += btnHistoriaClinica_Click;
            //
            // btnNuevaAtencion
            //
            btnNuevaAtencion.Location = new Point(1004, 640);
            btnNuevaAtencion.Name = "btnNuevaAtencion";
            btnNuevaAtencion.Size = new Size(134, 36);
            btnNuevaAtencion.TabIndex = 18;
            btnNuevaAtencion.Tag = "ATN_REGISTRAR";
            btnNuevaAtencion.Text = "Nueva atención";
            btnNuevaAtencion.UseVisualStyleBackColor = true;
            btnNuevaAtencion.Click += btnNuevaAtencion_Click;
            //
            // FrmClientes_60MN
            //
            AcceptButton = btnBuscar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1150, 700);
            Controls.Add(btnNuevaAtencion);
            Controls.Add(btnHistoriaClinica);
            Controls.Add(btnBajaMascota);
            Controls.Add(btnModificarMascota);
            Controls.Add(btnAnadirMascota);
            Controls.Add(dgvMascotas);
            Controls.Add(lblMascotas);
            Controls.Add(btnReactivarCliente);
            Controls.Add(btnBajaCliente);
            Controls.Add(btnModificarCliente);
            Controls.Add(btnNuevoCliente);
            Controls.Add(grpFicha);
            Controls.Add(dgvClientes);
            Controls.Add(chkIncluirBajas);
            Controls.Add(btnBuscar);
            Controls.Add(txtValor);
            Controls.Add(cboCriterio);
            Controls.Add(lblCriterio);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmClientes_60MN";
            Text = "Clientes";
            Load += FrmClientes_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            grpFicha.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMascotas).EndInit();
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
        private DataGridView dgvClientes;
        private DataGridViewTextBoxColumn colDocumento;
        private DataGridViewTextBoxColumn colApellido;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colTelefono;
        private DataGridViewTextBoxColumn colEstado;
        private GroupBox grpFicha;
        private Label lblFicha;
        private Button btnNuevoCliente;
        private Button btnModificarCliente;
        private Button btnBajaCliente;
        private Button btnReactivarCliente;
        private Label lblMascotas;
        private DataGridView dgvMascotas;
        private DataGridViewTextBoxColumn colMascota;
        private DataGridViewTextBoxColumn colEspecie;
        private DataGridViewTextBoxColumn colRaza;
        private DataGridViewTextBoxColumn colSexo;
        private DataGridViewTextBoxColumn colNacimiento;
        private DataGridViewTextBoxColumn colHistoria;
        private DataGridViewTextBoxColumn colEstadoMascota;
        private Button btnAnadirMascota;
        private Button btnModificarMascota;
        private Button btnBajaMascota;
        private Button btnHistoriaClinica;
        private Button btnNuevaAtencion;
    }
}
