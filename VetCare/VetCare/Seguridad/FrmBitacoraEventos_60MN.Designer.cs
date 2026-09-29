namespace VetCare
{
    partial class FrmBitacoraEventos_60MN
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
            DataGridViewCellStyle estiloFecha = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloCriticidad = new DataGridViewCellStyle();
            lblTitulo = new Label();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            lblCriticidad = new Label();
            cboCriticidad = new ComboBox();
            lblModulo = new Label();
            cboModulo = new ComboBox();
            lblEvento = new Label();
            cboEvento = new ComboBox();
            lblUsuario = new Label();
            cboUsuario = new ComboBox();
            btnBuscar = new Button();
            btnLimpiar = new Button();
            dgvEventos = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colUsuario = new DataGridViewTextBoxColumn();
            colModulo = new DataGridViewTextBoxColumn();
            colEvento = new DataGridViewTextBoxColumn();
            colCriticidad = new DataGridViewTextBoxColumn();
            colDescripcion = new DataGridViewTextBoxColumn();
            lblDetalleUsuario = new Label();
            lblResultados = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvEventos).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(191, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Bitácora de eventos";
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(12, 56);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(51, 20);
            lblDesde.TabIndex = 1;
            lblDesde.Text = "Desde";
            //
            // dtpDesde
            //
            dtpDesde.Checked = false;
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(80, 52);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.ShowCheckBox = true;
            dtpDesde.Size = new Size(160, 27);
            dtpDesde.TabIndex = 2;
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(260, 56);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(47, 20);
            lblHasta.TabIndex = 3;
            lblHasta.Text = "Hasta";
            //
            // dtpHasta
            //
            dtpHasta.Checked = false;
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(320, 52);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.ShowCheckBox = true;
            dtpHasta.Size = new Size(160, 27);
            dtpHasta.TabIndex = 4;
            //
            // lblCriticidad
            //
            lblCriticidad.AutoSize = true;
            lblCriticidad.Location = new Point(500, 56);
            lblCriticidad.Name = "lblCriticidad";
            lblCriticidad.Size = new Size(72, 20);
            lblCriticidad.TabIndex = 5;
            lblCriticidad.Text = "Criticidad";
            //
            // cboCriticidad
            //
            cboCriticidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCriticidad.FormattingEnabled = true;
            cboCriticidad.Location = new Point(580, 52);
            cboCriticidad.Name = "cboCriticidad";
            cboCriticidad.Size = new Size(200, 28);
            cboCriticidad.TabIndex = 6;
            //
            // lblModulo
            //
            lblModulo.AutoSize = true;
            lblModulo.Location = new Point(12, 96);
            lblModulo.Name = "lblModulo";
            lblModulo.Size = new Size(62, 20);
            lblModulo.TabIndex = 7;
            lblModulo.Text = "Módulo";
            //
            // cboModulo
            //
            cboModulo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboModulo.FormattingEnabled = true;
            cboModulo.Location = new Point(80, 92);
            cboModulo.Name = "cboModulo";
            cboModulo.Size = new Size(160, 28);
            cboModulo.TabIndex = 8;
            cboModulo.SelectedIndexChanged += cboModulo_SelectedIndexChanged;
            //
            // lblEvento
            //
            lblEvento.AutoSize = true;
            lblEvento.Location = new Point(260, 96);
            lblEvento.Name = "lblEvento";
            lblEvento.Size = new Size(54, 20);
            lblEvento.TabIndex = 9;
            lblEvento.Text = "Evento";
            //
            // cboEvento
            //
            cboEvento.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEvento.FormattingEnabled = true;
            cboEvento.Location = new Point(320, 92);
            cboEvento.Name = "cboEvento";
            cboEvento.Size = new Size(160, 28);
            cboEvento.TabIndex = 10;
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(500, 96);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(59, 20);
            lblUsuario.TabIndex = 11;
            lblUsuario.Text = "Usuario";
            //
            // cboUsuario
            //
            cboUsuario.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUsuario.FormattingEnabled = true;
            cboUsuario.Location = new Point(580, 92);
            cboUsuario.Name = "cboUsuario";
            cboUsuario.Size = new Size(200, 28);
            cboUsuario.TabIndex = 12;
            //
            // btnBuscar
            //
            btnBuscar.Location = new Point(800, 50);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(110, 34);
            btnBuscar.TabIndex = 13;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // btnLimpiar
            //
            btnLimpiar.Location = new Point(800, 90);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(110, 34);
            btnLimpiar.TabIndex = 14;
            btnLimpiar.Text = "Limpiar filtros";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            //
            // dgvEventos
            //
            dgvEventos.AllowUserToAddRows = false;
            dgvEventos.AllowUserToDeleteRows = false;
            dgvEventos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEventos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEventos.BackgroundColor = SystemColors.Window;
            dgvEventos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEventos.Columns.AddRange(new DataGridViewColumn[] { colFecha, colUsuario, colModulo, colEvento, colCriticidad, colDescripcion });
            dgvEventos.Location = new Point(12, 136);
            dgvEventos.MultiSelect = false;
            dgvEventos.Name = "dgvEventos";
            dgvEventos.ReadOnly = true;
            dgvEventos.RowHeadersVisible = false;
            dgvEventos.RowHeadersWidth = 51;
            dgvEventos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEventos.Size = new Size(1076, 390);
            dgvEventos.TabIndex = 15;
            dgvEventos.DataBindingComplete += dgvEventos_DataBindingComplete;
            dgvEventos.SelectionChanged += dgvEventos_SelectionChanged;
            //
            // colFecha
            //
            colFecha.DataPropertyName = "FechaHora";
            estiloFecha.Format = "dd/MM/yyyy HH:mm:ss";
            colFecha.DefaultCellStyle = estiloFecha;
            colFecha.FillWeight = 70F;
            colFecha.HeaderText = "Fecha y hora";
            colFecha.MinimumWidth = 6;
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colUsuario
            //
            colUsuario.DataPropertyName = "Usuario";
            colUsuario.FillWeight = 55F;
            colUsuario.HeaderText = "Usuario";
            colUsuario.MinimumWidth = 6;
            colUsuario.Name = "colUsuario";
            colUsuario.ReadOnly = true;
            //
            // colModulo
            //
            colModulo.DataPropertyName = "Modulo";
            colModulo.FillWeight = 50F;
            colModulo.HeaderText = "Módulo";
            colModulo.MinimumWidth = 6;
            colModulo.Name = "colModulo";
            colModulo.ReadOnly = true;
            //
            // colEvento
            //
            colEvento.DataPropertyName = "Evento";
            colEvento.FillWeight = 70F;
            colEvento.HeaderText = "Evento";
            colEvento.MinimumWidth = 6;
            colEvento.Name = "colEvento";
            colEvento.ReadOnly = true;
            //
            // colCriticidad
            //
            colCriticidad.DataPropertyName = "Criticidad";
            estiloCriticidad.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colCriticidad.DefaultCellStyle = estiloCriticidad;
            colCriticidad.FillWeight = 35F;
            colCriticidad.HeaderText = "Criticidad";
            colCriticidad.MinimumWidth = 6;
            colCriticidad.Name = "colCriticidad";
            colCriticidad.ReadOnly = true;
            //
            // colDescripcion
            //
            colDescripcion.DataPropertyName = "Descripcion";
            colDescripcion.FillWeight = 200F;
            colDescripcion.HeaderText = "Descripción";
            colDescripcion.MinimumWidth = 6;
            colDescripcion.Name = "colDescripcion";
            colDescripcion.ReadOnly = true;
            //
            // lblDetalleUsuario
            //
            lblDetalleUsuario.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblDetalleUsuario.AutoSize = true;
            lblDetalleUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDetalleUsuario.Location = new Point(12, 536);
            lblDetalleUsuario.Name = "lblDetalleUsuario";
            lblDetalleUsuario.Size = new Size(0, 20);
            lblDetalleUsuario.TabIndex = 16;
            //
            // lblResultados
            //
            lblResultados.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblResultados.ForeColor = Color.DimGray;
            lblResultados.Location = new Point(788, 536);
            lblResultados.Name = "lblResultados";
            lblResultados.Size = new Size(300, 20);
            lblResultados.TabIndex = 17;
            lblResultados.TextAlign = ContentAlignment.TopRight;
            //
            // FrmBitacoraEventos_60MN
            //
            AcceptButton = btnBuscar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 568);
            Controls.Add(lblResultados);
            Controls.Add(lblDetalleUsuario);
            Controls.Add(dgvEventos);
            Controls.Add(btnLimpiar);
            Controls.Add(btnBuscar);
            Controls.Add(cboUsuario);
            Controls.Add(lblUsuario);
            Controls.Add(cboEvento);
            Controls.Add(lblEvento);
            Controls.Add(cboModulo);
            Controls.Add(lblModulo);
            Controls.Add(cboCriticidad);
            Controls.Add(lblCriticidad);
            Controls.Add(dtpHasta);
            Controls.Add(lblHasta);
            Controls.Add(dtpDesde);
            Controls.Add(lblDesde);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(960, 450);
            Name = "FrmBitacoraEventos_60MN";
            Text = "Bitácora de eventos";
            Load += FrmBitacoraEventos_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvEventos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Label lblCriticidad;
        private ComboBox cboCriticidad;
        private Label lblModulo;
        private ComboBox cboModulo;
        private Label lblEvento;
        private ComboBox cboEvento;
        private Label lblUsuario;
        private ComboBox cboUsuario;
        private Button btnBuscar;
        private Button btnLimpiar;
        private DataGridView dgvEventos;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colUsuario;
        private DataGridViewTextBoxColumn colModulo;
        private DataGridViewTextBoxColumn colEvento;
        private DataGridViewTextBoxColumn colCriticidad;
        private DataGridViewTextBoxColumn colDescripcion;
        private Label lblDetalleUsuario;
        private Label lblResultados;
    }
}
