namespace VetCare
{
    partial class FrmCompararVersiones_60MN
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
            lblProducto = new Label();
            dgvDiferencias = new DataGridView();
            colCampo = new DataGridViewTextBoxColumn();
            colVersionA = new DataGridViewTextBoxColumn();
            colVersionB = new DataGridViewTextBoxColumn();
            lblReferencia = new Label();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvDiferencias).BeginInit();
            SuspendLayout();
            //
            // lblProducto
            //
            lblProducto.AutoSize = true;
            lblProducto.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblProducto.Location = new Point(12, 12);
            lblProducto.Name = "lblProducto";
            lblProducto.Size = new Size(79, 23);
            lblProducto.TabIndex = 0;
            lblProducto.Text = "Producto";
            //
            // dgvDiferencias
            //
            dgvDiferencias.AllowUserToAddRows = false;
            dgvDiferencias.AllowUserToDeleteRows = false;
            dgvDiferencias.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDiferencias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDiferencias.BackgroundColor = SystemColors.Window;
            dgvDiferencias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDiferencias.Columns.AddRange(new DataGridViewColumn[] { colCampo, colVersionA, colVersionB });
            dgvDiferencias.Location = new Point(12, 44);
            dgvDiferencias.Name = "dgvDiferencias";
            dgvDiferencias.ReadOnly = true;
            dgvDiferencias.RowHeadersVisible = false;
            dgvDiferencias.RowHeadersWidth = 51;
            dgvDiferencias.Size = new Size(676, 420);
            dgvDiferencias.TabIndex = 1;
            dgvDiferencias.DataBindingComplete += dgvDiferencias_DataBindingComplete;
            //
            // colCampo
            //
            colCampo.DataPropertyName = "Campo";
            colCampo.FillWeight = 70F;
            colCampo.HeaderText = "Campo";
            colCampo.MinimumWidth = 6;
            colCampo.Name = "colCampo";
            colCampo.ReadOnly = true;
            //
            // colVersionA
            //
            colVersionA.DataPropertyName = "VersionA";
            colVersionA.HeaderText = "Versión anterior";
            colVersionA.MinimumWidth = 6;
            colVersionA.Name = "colVersionA";
            colVersionA.ReadOnly = true;
            //
            // colVersionB
            //
            colVersionB.DataPropertyName = "VersionB";
            colVersionB.HeaderText = "Versión posterior";
            colVersionB.MinimumWidth = 6;
            colVersionB.Name = "colVersionB";
            colVersionB.ReadOnly = true;
            //
            // lblReferencia
            //
            lblReferencia.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblReferencia.AutoSize = true;
            lblReferencia.ForeColor = Color.DimGray;
            lblReferencia.Location = new Point(12, 480);
            lblReferencia.Name = "lblReferencia";
            lblReferencia.Size = new Size(280, 20);
            lblReferencia.TabIndex = 2;
            lblReferencia.Text = "Los campos resaltados cambiaron entre versiones.";
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.OK;
            btnCerrar.Location = new Point(578, 474);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 34);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmCompararVersiones_60MN
            //
            AcceptButton = btnCerrar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(700, 520);
            Controls.Add(btnCerrar);
            Controls.Add(lblReferencia);
            Controls.Add(dgvDiferencias);
            Controls.Add(lblProducto);
            MinimizeBox = false;
            Name = "FrmCompararVersiones_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Comparar versiones";
            ((System.ComponentModel.ISupportInitialize)dgvDiferencias).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblProducto;
        private DataGridView dgvDiferencias;
        private DataGridViewTextBoxColumn colCampo;
        private DataGridViewTextBoxColumn colVersionA;
        private DataGridViewTextBoxColumn colVersionB;
        private Label lblReferencia;
        private Button btnCerrar;
    }
}
