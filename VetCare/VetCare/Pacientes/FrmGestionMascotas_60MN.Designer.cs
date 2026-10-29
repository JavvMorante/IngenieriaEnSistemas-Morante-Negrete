namespace VetCare
{
    partial class FrmGestionMascotas_60MN
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
            dgvMascotas = new DataGridView();
            colMascota = new DataGridViewTextBoxColumn();
            colEspecie = new DataGridViewTextBoxColumn();
            colRaza = new DataGridViewTextBoxColumn();
            colSexo = new DataGridViewTextBoxColumn();
            colNacimiento = new DataGridViewTextBoxColumn();
            colPropietario = new DataGridViewTextBoxColumn();
            colDocumento = new DataGridViewTextBoxColumn();
            colHistoria = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            lblResultados = new Label();
            btnAnadir = new Button();
            btnModificar = new Button();
            btnBaja = new Button();
            btnReactivar = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvMascotas).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(97, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Mascotas";
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
            chkIncluirBajas.CheckedChanged += chkIncluirBajas_CheckedChanged;
            //
            // dgvMascotas
            //
            dgvMascotas.AllowUserToAddRows = false;
            dgvMascotas.AllowUserToDeleteRows = false;
            dgvMascotas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMascotas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMascotas.BackgroundColor = SystemColors.Window;
            dgvMascotas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMascotas.Columns.AddRange(new DataGridViewColumn[] { colMascota, colEspecie, colRaza, colSexo, colNacimiento, colPropietario, colDocumento, colHistoria, colEstado });
            dgvMascotas.Location = new Point(12, 96);
            dgvMascotas.MultiSelect = false;
            dgvMascotas.Name = "dgvMascotas";
            dgvMascotas.ReadOnly = true;
            dgvMascotas.RowHeadersVisible = false;
            dgvMascotas.RowHeadersWidth = 51;
            dgvMascotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMascotas.Size = new Size(1126, 380);
            dgvMascotas.TabIndex = 6;
            dgvMascotas.CellDoubleClick += dgvMascotas_CellDoubleClick;
            dgvMascotas.DataBindingComplete += dgvMascotas_DataBindingComplete;
            dgvMascotas.SelectionChanged += dgvMascotas_SelectionChanged;
            //
            // colMascota
            //
            colMascota.DataPropertyName = "Nombre";
            colMascota.HeaderText = "Mascota";
            colMascota.MinimumWidth = 6;
            colMascota.Name = "colMascota";
            colMascota.ReadOnly = true;
            //
            // colEspecie
            //
            colEspecie.DataPropertyName = "Especie";
            colEspecie.FillWeight = 70F;
            colEspecie.HeaderText = "Especie";
            colEspecie.MinimumWidth = 6;
            colEspecie.Name = "colEspecie";
            colEspecie.ReadOnly = true;
            //
            // colRaza
            //
            colRaza.DataPropertyName = "Raza";
            colRaza.FillWeight = 80F;
            colRaza.HeaderText = "Raza";
            colRaza.MinimumWidth = 6;
            colRaza.Name = "colRaza";
            colRaza.ReadOnly = true;
            //
            // colSexo
            //
            colSexo.DataPropertyName = "SexoTexto";
            colSexo.FillWeight = 55F;
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
            colNacimiento.FillWeight = 70F;
            colNacimiento.HeaderText = "Nacimiento";
            colNacimiento.MinimumWidth = 6;
            colNacimiento.Name = "colNacimiento";
            colNacimiento.ReadOnly = true;
            //
            // colPropietario
            //
            colPropietario.DataPropertyName = "Propietario";
            colPropietario.FillWeight = 130F;
            colPropietario.HeaderText = "Propietario";
            colPropietario.MinimumWidth = 6;
            colPropietario.Name = "colPropietario";
            colPropietario.ReadOnly = true;
            //
            // colDocumento
            //
            colDocumento.DataPropertyName = "DocumentoPropietario";
            colDocumento.FillWeight = 90F;
            colDocumento.HeaderText = "Documento";
            colDocumento.MinimumWidth = 6;
            colDocumento.Name = "colDocumento";
            colDocumento.ReadOnly = true;
            //
            // colHistoria
            //
            colHistoria.DataPropertyName = "NumeroHistoriaClinica";
            colHistoria.FillWeight = 75F;
            colHistoria.HeaderText = "Historia clínica";
            colHistoria.MinimumWidth = 6;
            colHistoria.Name = "colHistoria";
            colHistoria.ReadOnly = true;
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
            // lblResultados
            //
            lblResultados.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblResultados.AutoSize = true;
            lblResultados.ForeColor = SystemColors.GrayText;
            lblResultados.Location = new Point(12, 482);
            lblResultados.Name = "lblResultados";
            lblResultados.Size = new Size(0, 20);
            lblResultados.TabIndex = 7;
            //
            // btnAnadir
            //
            btnAnadir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAnadir.Location = new Point(12, 510);
            btnAnadir.Name = "btnAnadir";
            btnAnadir.Size = new Size(130, 36);
            btnAnadir.TabIndex = 8;
            btnAnadir.Tag = "PAC_MASCOTA";
            btnAnadir.Text = "Añadir";
            btnAnadir.UseVisualStyleBackColor = true;
            btnAnadir.Click += btnAnadir_Click;
            //
            // btnModificar
            //
            btnModificar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnModificar.Location = new Point(152, 510);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(130, 36);
            btnModificar.TabIndex = 9;
            btnModificar.Tag = "PAC_MASCOTA";
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            //
            // btnBaja
            //
            btnBaja.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnBaja.Location = new Point(292, 510);
            btnBaja.Name = "btnBaja";
            btnBaja.Size = new Size(130, 36);
            btnBaja.TabIndex = 10;
            btnBaja.Tag = "PAC_MASCOTA";
            btnBaja.Text = "Dar de baja";
            btnBaja.UseVisualStyleBackColor = true;
            btnBaja.Click += btnBaja_Click;
            //
            // btnReactivar
            //
            btnReactivar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnReactivar.Location = new Point(432, 510);
            btnReactivar.Name = "btnReactivar";
            btnReactivar.Size = new Size(130, 36);
            btnReactivar.TabIndex = 11;
            btnReactivar.Tag = "PAC_MASCOTA";
            btnReactivar.Text = "Reactivar";
            btnReactivar.UseVisualStyleBackColor = true;
            btnReactivar.Click += btnReactivar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(1028, 510);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 36);
            btnCerrar.TabIndex = 12;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmGestionMascotas_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1150, 558);
            Controls.Add(btnCerrar);
            Controls.Add(btnReactivar);
            Controls.Add(btnBaja);
            Controls.Add(btnModificar);
            Controls.Add(btnAnadir);
            Controls.Add(lblResultados);
            Controls.Add(dgvMascotas);
            Controls.Add(chkIncluirBajas);
            Controls.Add(btnBuscar);
            Controls.Add(txtValor);
            Controls.Add(cboCriterio);
            Controls.Add(lblCriterio);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(900, 500);
            Name = "FrmGestionMascotas_60MN";
            Text = "Gestión de mascotas";
            Load += FrmGestionMascotas_60MN_Load;
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
        private DataGridView dgvMascotas;
        private DataGridViewTextBoxColumn colMascota;
        private DataGridViewTextBoxColumn colEspecie;
        private DataGridViewTextBoxColumn colRaza;
        private DataGridViewTextBoxColumn colSexo;
        private DataGridViewTextBoxColumn colNacimiento;
        private DataGridViewTextBoxColumn colPropietario;
        private DataGridViewTextBoxColumn colDocumento;
        private DataGridViewTextBoxColumn colHistoria;
        private DataGridViewTextBoxColumn colEstado;
        private Label lblResultados;
        private Button btnAnadir;
        private Button btnModificar;
        private Button btnBaja;
        private Button btnReactivar;
        private Button btnCerrar;
    }
}
