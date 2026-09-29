namespace VetCare
{
    partial class FrmIntegridad_60MN
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
            lblDescripcion = new Label();
            lblEstado = new Label();
            dgvInconsistencias = new DataGridView();
            colTabla = new DataGridViewTextBoxColumn();
            colTipo = new DataGridViewTextBoxColumn();
            colRegistro = new DataGridViewTextBoxColumn();
            colDetalle = new DataGridViewTextBoxColumn();
            btnVerificar = new Button();
            btnRecalcular = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvInconsistencias).BeginInit();
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
            lblTitulo.Text = "Integridad de datos";
            //
            // lblDescripcion
            //
            lblDescripcion.ForeColor = Color.DimGray;
            lblDescripcion.Location = new Point(12, 44);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(860, 44);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Verifica los dígitos verificadores horizontales (DVH, uno por fila) y verticales (DVV, uno por tabla) de Usuario, Familia, Patente, sus tablas de relación, Bitacora, Productos y MovimientoStock.";
            //
            // lblEstado
            //
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEstado.Location = new Point(12, 96);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(0, 23);
            lblEstado.TabIndex = 2;
            //
            // dgvInconsistencias
            //
            dgvInconsistencias.AllowUserToAddRows = false;
            dgvInconsistencias.AllowUserToDeleteRows = false;
            dgvInconsistencias.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvInconsistencias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInconsistencias.BackgroundColor = SystemColors.Window;
            dgvInconsistencias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInconsistencias.Columns.AddRange(new DataGridViewColumn[] { colTabla, colTipo, colRegistro, colDetalle });
            dgvInconsistencias.Location = new Point(12, 128);
            dgvInconsistencias.Name = "dgvInconsistencias";
            dgvInconsistencias.ReadOnly = true;
            dgvInconsistencias.RowHeadersVisible = false;
            dgvInconsistencias.RowHeadersWidth = 51;
            dgvInconsistencias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInconsistencias.Size = new Size(860, 330);
            dgvInconsistencias.TabIndex = 3;
            //
            // colTabla
            //
            colTabla.DataPropertyName = "Tabla";
            colTabla.FillWeight = 60F;
            colTabla.HeaderText = "Tabla";
            colTabla.MinimumWidth = 6;
            colTabla.Name = "colTabla";
            colTabla.ReadOnly = true;
            //
            // colTipo
            //
            colTipo.DataPropertyName = "Tipo";
            colTipo.FillWeight = 30F;
            colTipo.HeaderText = "Dígito";
            colTipo.MinimumWidth = 6;
            colTipo.Name = "colTipo";
            colTipo.ReadOnly = true;
            //
            // colRegistro
            //
            colRegistro.DataPropertyName = "Registro";
            colRegistro.FillWeight = 50F;
            colRegistro.HeaderText = "Registro (clave)";
            colRegistro.MinimumWidth = 6;
            colRegistro.Name = "colRegistro";
            colRegistro.ReadOnly = true;
            //
            // colDetalle
            //
            colDetalle.DataPropertyName = "Detalle";
            colDetalle.FillWeight = 200F;
            colDetalle.HeaderText = "Detalle";
            colDetalle.MinimumWidth = 6;
            colDetalle.Name = "colDetalle";
            colDetalle.ReadOnly = true;
            //
            // btnVerificar
            //
            btnVerificar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnVerificar.Location = new Point(542, 470);
            btnVerificar.Name = "btnVerificar";
            btnVerificar.Size = new Size(150, 34);
            btnVerificar.TabIndex = 4;
            btnVerificar.Text = "Verificar";
            btnVerificar.UseVisualStyleBackColor = true;
            btnVerificar.Click += btnVerificar_Click;
            //
            // btnRecalcular
            //
            btnRecalcular.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRecalcular.Location = new Point(702, 470);
            btnRecalcular.Name = "btnRecalcular";
            btnRecalcular.Size = new Size(170, 34);
            btnRecalcular.TabIndex = 5;
            btnRecalcular.Text = "Recalcular dígitos";
            btnRecalcular.UseVisualStyleBackColor = true;
            btnRecalcular.Click += btnRecalcular_Click;
            //
            // FrmIntegridad_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 516);
            Controls.Add(btnRecalcular);
            Controls.Add(btnVerificar);
            Controls.Add(dgvInconsistencias);
            Controls.Add(lblEstado);
            Controls.Add(lblDescripcion);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(700, 400);
            Name = "FrmIntegridad_60MN";
            Text = "Integridad de datos";
            Load += FrmIntegridad_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvInconsistencias).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblDescripcion;
        private Label lblEstado;
        private DataGridView dgvInconsistencias;
        private DataGridViewTextBoxColumn colTabla;
        private DataGridViewTextBoxColumn colTipo;
        private DataGridViewTextBoxColumn colRegistro;
        private DataGridViewTextBoxColumn colDetalle;
        private Button btnVerificar;
        private Button btnRecalcular;
    }
}
