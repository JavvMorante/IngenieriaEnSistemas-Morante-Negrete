namespace InspectorReflection
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.lblClase = new System.Windows.Forms.Label();
            this.dgvMiembros = new System.Windows.Forms.DataGridView();
            this.colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTipoDato = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCantidadParametros = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMiembros)).BeginInit();
            this.SuspendLayout();
            // 
            // lblClase
            // 
            this.lblClase.AutoSize = true;
            this.lblClase.Location = new System.Drawing.Point(16, 15);
            this.lblClase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClase.Name = "lblClase";
            this.lblClase.Size = new System.Drawing.Size(137, 16);
            this.lblClase.TabIndex = 0;
            this.lblClase.Text = "Clase inspeccionada:";
            // 
            // dgvMiembros
            // 
            this.dgvMiembros.AllowUserToAddRows = false;
            this.dgvMiembros.AllowUserToDeleteRows = false;
            this.dgvMiembros.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMiembros.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMiembros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMiembros.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNombre,
            this.colCategoria,
            this.colTipoDato,
            this.colCantidadParametros});
            this.dgvMiembros.Location = new System.Drawing.Point(16, 44);
            this.dgvMiembros.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvMiembros.Name = "dgvMiembros";
            this.dgvMiembros.ReadOnly = true;
            this.dgvMiembros.RowHeadersVisible = false;
            this.dgvMiembros.RowHeadersWidth = 51;
            this.dgvMiembros.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMiembros.Size = new System.Drawing.Size(880, 262);
            this.dgvMiembros.TabIndex = 1;
            // 
            // colNombre
            // 
            this.colNombre.DataPropertyName = "Nombre";
            this.colNombre.HeaderText = "Nombre";
            this.colNombre.MinimumWidth = 6;
            this.colNombre.Name = "colNombre";
            this.colNombre.ReadOnly = true;
            // 
            // colCategoria
            // 
            this.colCategoria.DataPropertyName = "Categoria";
            this.colCategoria.HeaderText = "Categoría";
            this.colCategoria.MinimumWidth = 6;
            this.colCategoria.Name = "colCategoria";
            this.colCategoria.ReadOnly = true;
            // 
            // colTipoDato
            // 
            this.colTipoDato.DataPropertyName = "TipoDato";
            this.colTipoDato.HeaderText = "Tipo de dato / retorno";
            this.colTipoDato.MinimumWidth = 6;
            this.colTipoDato.Name = "colTipoDato";
            this.colTipoDato.ReadOnly = true;
            // 
            // colCantidadParametros
            // 
            this.colCantidadParametros.DataPropertyName = "CantidadParametros";
            this.colCantidadParametros.HeaderText = "Cantidad de parámetros";
            this.colCantidadParametros.MinimumWidth = 6;
            this.colCantidadParametros.Name = "colCantidadParametros";
            this.colCantidadParametros.ReadOnly = true;
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 321);
            this.Controls.Add(this.dgvMiembros);
            this.Controls.Add(this.lblClase);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inspector de miembros y COR";
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMiembros)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClase;
        private System.Windows.Forms.DataGridView dgvMiembros;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipoDato;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCantidadParametros;
    }
}
