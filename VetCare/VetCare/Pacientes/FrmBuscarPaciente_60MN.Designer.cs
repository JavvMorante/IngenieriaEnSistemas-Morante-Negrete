namespace VetCare
{
    partial class FrmBuscarPaciente_60MN
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
            lblProposito = new Label();
            lblCriterio = new Label();
            cboCriterio = new ComboBox();
            txtValor = new TextBox();
            btnBuscar = new Button();
            chkIncluirBajas = new CheckBox();
            dgvPacientes = new DataGridView();
            colPaciente = new DataGridViewTextBoxColumn();
            colEspecie = new DataGridViewTextBoxColumn();
            colRaza = new DataGridViewTextBoxColumn();
            colPropietario = new DataGridViewTextBoxColumn();
            colDocumento = new DataGridViewTextBoxColumn();
            colHistoria = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            btnAceptar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPacientes).BeginInit();
            SuspendLayout();
            //
            // lblProposito
            //
            lblProposito.AutoSize = true;
            lblProposito.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblProposito.Location = new Point(12, 12);
            lblProposito.Name = "lblProposito";
            lblProposito.Size = new Size(236, 28);
            lblProposito.TabIndex = 0;
            lblProposito.Text = "Busque y elija el paciente.";
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
            // dgvPacientes
            //
            dgvPacientes.AllowUserToAddRows = false;
            dgvPacientes.AllowUserToDeleteRows = false;
            dgvPacientes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPacientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPacientes.BackgroundColor = SystemColors.Window;
            dgvPacientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPacientes.Columns.AddRange(new DataGridViewColumn[] { colPaciente, colEspecie, colRaza, colPropietario, colDocumento, colHistoria, colEstado });
            dgvPacientes.Location = new Point(12, 96);
            dgvPacientes.MultiSelect = false;
            dgvPacientes.Name = "dgvPacientes";
            dgvPacientes.ReadOnly = true;
            dgvPacientes.RowHeadersVisible = false;
            dgvPacientes.RowHeadersWidth = 51;
            dgvPacientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPacientes.Size = new Size(876, 300);
            dgvPacientes.TabIndex = 6;
            dgvPacientes.DataBindingComplete += dgvPacientes_DataBindingComplete;
            //
            // colPaciente
            //
            colPaciente.DataPropertyName = "Nombre";
            colPaciente.HeaderText = "Paciente";
            colPaciente.MinimumWidth = 6;
            colPaciente.Name = "colPaciente";
            colPaciente.ReadOnly = true;
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
            colRaza.FillWeight = 70F;
            colRaza.HeaderText = "Raza";
            colRaza.MinimumWidth = 6;
            colRaza.Name = "colRaza";
            colRaza.ReadOnly = true;
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
            colDocumento.HeaderText = "Documento";
            colDocumento.MinimumWidth = 6;
            colDocumento.Name = "colDocumento";
            colDocumento.ReadOnly = true;
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
            // colEstado
            //
            colEstado.DataPropertyName = "Estado";
            colEstado.FillWeight = 60F;
            colEstado.HeaderText = "Estado";
            colEstado.MinimumWidth = 6;
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // btnAceptar
            //
            btnAceptar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAceptar.Enabled = false;
            btnAceptar.Location = new Point(658, 410);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 36);
            btnAceptar.TabIndex = 7;
            btnAceptar.Text = "Seleccionar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(778, 410);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 36);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmBuscarPaciente_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(900, 458);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(dgvPacientes);
            Controls.Add(chkIncluirBajas);
            Controls.Add(btnBuscar);
            Controls.Add(txtValor);
            Controls.Add(cboCriterio);
            Controls.Add(lblCriterio);
            Controls.Add(lblProposito);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(760, 400);
            Name = "FrmBuscarPaciente_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Seleccionar paciente";
            Load += FrmBuscarPaciente_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPacientes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblProposito;
        private Label lblCriterio;
        private ComboBox cboCriterio;
        private TextBox txtValor;
        private Button btnBuscar;
        private CheckBox chkIncluirBajas;
        private DataGridView dgvPacientes;
        private DataGridViewTextBoxColumn colPaciente;
        private DataGridViewTextBoxColumn colEspecie;
        private DataGridViewTextBoxColumn colRaza;
        private DataGridViewTextBoxColumn colPropietario;
        private DataGridViewTextBoxColumn colDocumento;
        private DataGridViewTextBoxColumn colHistoria;
        private DataGridViewTextBoxColumn colEstado;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}
