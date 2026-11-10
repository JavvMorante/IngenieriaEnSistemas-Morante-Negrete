namespace VetCare
{
    partial class FrmDeserializarBitacora_60MN
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
            lblArchivo = new Label();
            dgvEventos = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colUsuario = new DataGridViewTextBoxColumn();
            colModulo = new DataGridViewTextBoxColumn();
            colEvento = new DataGridViewTextBoxColumn();
            colCriticidad = new DataGridViewTextBoxColumn();
            colDescripcion = new DataGridViewTextBoxColumn();
            btnSalir = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvEventos).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(222, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Bitácora deserializada";
            //
            // lblArchivo
            //
            lblArchivo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblArchivo.AutoEllipsis = true;
            lblArchivo.Location = new Point(12, 48);
            lblArchivo.Name = "lblArchivo";
            lblArchivo.Size = new Size(1076, 24);
            lblArchivo.TabIndex = 1;
            //
            // dgvEventos
            //
            dgvEventos.AllowUserToAddRows = false;
            dgvEventos.AllowUserToDeleteRows = false;
            dgvEventos.AllowUserToResizeRows = false;
            dgvEventos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEventos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEventos.BackgroundColor = SystemColors.Window;
            dgvEventos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEventos.Columns.AddRange(new DataGridViewColumn[] { colFecha, colUsuario, colModulo, colEvento, colCriticidad, colDescripcion });
            dgvEventos.Location = new Point(12, 80);
            dgvEventos.MultiSelect = false;
            dgvEventos.Name = "dgvEventos";
            dgvEventos.ReadOnly = true;
            dgvEventos.RowHeadersVisible = false;
            dgvEventos.RowHeadersWidth = 51;
            dgvEventos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEventos.Size = new Size(1076, 440);
            dgvEventos.TabIndex = 2;
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
            colUsuario.FillWeight = 50F;
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
            // btnSalir
            //
            btnSalir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSalir.DialogResult = DialogResult.Cancel;
            btnSalir.Location = new Point(978, 530);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(110, 36);
            btnSalir.TabIndex = 3;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            //
            // FrmDeserializarBitacora_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnSalir;
            ClientSize = new Size(1100, 578);
            Controls.Add(btnSalir);
            Controls.Add(dgvEventos);
            Controls.Add(lblArchivo);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(800, 450);
            Name = "FrmDeserializarBitacora_60MN";
            Text = "Deserializar XML de bitácora";
            Load += FrmDeserializarBitacora_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvEventos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblArchivo;
        private DataGridView dgvEventos;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colUsuario;
        private DataGridViewTextBoxColumn colModulo;
        private DataGridViewTextBoxColumn colEvento;
        private DataGridViewTextBoxColumn colCriticidad;
        private DataGridViewTextBoxColumn colDescripcion;
        private Button btnSalir;
    }
}
