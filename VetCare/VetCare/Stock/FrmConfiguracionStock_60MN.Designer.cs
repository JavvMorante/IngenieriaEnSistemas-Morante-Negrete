namespace VetCare
{
    partial class FrmConfiguracionStock_60MN
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
            DataGridViewCellStyle estiloMargen = new DataGridViewCellStyle();
            grpGeneral = new GroupBox();
            lblMargen = new Label();
            nudMargen = new NumericUpDown();
            lblStockMinimo = new Label();
            nudStockMinimo = new NumericUpDown();
            lblCategorias = new Label();
            dgvCategorias = new DataGridView();
            colCategoria = new DataGridViewTextBoxColumn();
            colMargenCategoria = new DataGridViewTextBoxColumn();
            lblAyuda = new Label();
            chkRecalcular = new CheckBox();
            chkAplicarMinimo = new CheckBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMargen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStockMinimo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCategorias).BeginInit();
            SuspendLayout();
            //
            // grpGeneral
            //
            grpGeneral.Controls.Add(lblMargen);
            grpGeneral.Controls.Add(nudMargen);
            grpGeneral.Controls.Add(lblStockMinimo);
            grpGeneral.Controls.Add(nudStockMinimo);
            grpGeneral.Location = new Point(16, 12);
            grpGeneral.Name = "grpGeneral";
            grpGeneral.Size = new Size(568, 112);
            grpGeneral.TabIndex = 0;
            grpGeneral.TabStop = false;
            grpGeneral.Text = "Parámetros generales";
            //
            // lblMargen
            //
            lblMargen.AutoSize = true;
            lblMargen.Location = new Point(16, 36);
            lblMargen.Name = "lblMargen";
            lblMargen.Size = new Size(185, 20);
            lblMargen.TabIndex = 0;
            lblMargen.Text = "Margen de ganancia (%)";
            //
            // nudMargen
            //
            nudMargen.DecimalPlaces = 2;
            nudMargen.Location = new Point(280, 32);
            nudMargen.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudMargen.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            nudMargen.Name = "nudMargen";
            nudMargen.Size = new Size(120, 27);
            nudMargen.TabIndex = 1;
            //
            // lblStockMinimo
            //
            lblStockMinimo.AutoSize = true;
            lblStockMinimo.Location = new Point(16, 74);
            lblStockMinimo.Name = "lblStockMinimo";
            lblStockMinimo.Size = new Size(254, 20);
            lblStockMinimo.TabIndex = 2;
            lblStockMinimo.Text = "Stock mínimo sugerido (unidades)";
            //
            // nudStockMinimo
            //
            nudStockMinimo.Location = new Point(280, 70);
            nudStockMinimo.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudStockMinimo.Name = "nudStockMinimo";
            nudStockMinimo.Size = new Size(120, 27);
            nudStockMinimo.TabIndex = 3;
            //
            // lblCategorias
            //
            lblCategorias.AutoSize = true;
            lblCategorias.Location = new Point(16, 134);
            lblCategorias.Name = "lblCategorias";
            lblCategorias.Size = new Size(197, 20);
            lblCategorias.TabIndex = 1;
            lblCategorias.Text = "Margen por categoría (%)";
            //
            // dgvCategorias
            //
            dgvCategorias.AllowUserToAddRows = false;
            dgvCategorias.AllowUserToDeleteRows = false;
            dgvCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategorias.BackgroundColor = SystemColors.Window;
            dgvCategorias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCategorias.Columns.AddRange(new DataGridViewColumn[] { colCategoria, colMargenCategoria });
            dgvCategorias.Location = new Point(16, 158);
            dgvCategorias.MultiSelect = false;
            dgvCategorias.Name = "dgvCategorias";
            dgvCategorias.RowHeadersVisible = false;
            dgvCategorias.RowHeadersWidth = 51;
            dgvCategorias.Size = new Size(568, 190);
            dgvCategorias.TabIndex = 2;
            dgvCategorias.CellValidating += dgvCategorias_CellValidating;
            dgvCategorias.DataError += dgvCategorias_DataError;
            //
            // colCategoria
            //
            colCategoria.DataPropertyName = "Nombre";
            colCategoria.FillWeight = 160F;
            colCategoria.HeaderText = "Categoría";
            colCategoria.MinimumWidth = 6;
            colCategoria.Name = "colCategoria";
            colCategoria.ReadOnly = true;
            //
            // colMargenCategoria
            //
            colMargenCategoria.DataPropertyName = "Margen";
            estiloMargen.Format = "N2";
            estiloMargen.NullValue = "";
            colMargenCategoria.DefaultCellStyle = estiloMargen;
            colMargenCategoria.HeaderText = "Margen (%)";
            colMargenCategoria.MinimumWidth = 6;
            colMargenCategoria.Name = "colMargenCategoria";
            //
            // lblAyuda
            //
            lblAyuda.ForeColor = SystemColors.GrayText;
            lblAyuda.Location = new Point(16, 352);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(568, 22);
            lblAyuda.TabIndex = 3;
            lblAyuda.Text = "Vacío = la categoría usa el margen general.";
            //
            // chkRecalcular
            //
            chkRecalcular.AutoSize = true;
            chkRecalcular.Location = new Point(16, 382);
            chkRecalcular.Name = "chkRecalcular";
            chkRecalcular.Size = new Size(392, 24);
            chkRecalcular.TabIndex = 4;
            chkRecalcular.Text = "Recalcular el precio de venta de los productos activos";
            chkRecalcular.UseVisualStyleBackColor = true;
            //
            // chkAplicarMinimo
            //
            chkAplicarMinimo.AutoSize = true;
            chkAplicarMinimo.Location = new Point(16, 412);
            chkAplicarMinimo.Name = "chkAplicarMinimo";
            chkAplicarMinimo.Size = new Size(415, 24);
            chkAplicarMinimo.TabIndex = 5;
            chkAplicarMinimo.Text = "Aplicar el stock mínimo sugerido a los productos activos";
            chkAplicarMinimo.UseVisualStyleBackColor = true;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(354, 452);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 34);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(474, 452);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 7;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            //
            // FrmConfiguracionStock_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(600, 500);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(chkAplicarMinimo);
            Controls.Add(chkRecalcular);
            Controls.Add(lblAyuda);
            Controls.Add(dgvCategorias);
            Controls.Add(lblCategorias);
            Controls.Add(grpGeneral);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmConfiguracionStock_60MN";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Configuración de stock";
            Load += FrmConfiguracionStock_60MN_Load;
            grpGeneral.ResumeLayout(false);
            grpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMargen).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStockMinimo).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCategorias).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpGeneral;
        private Label lblMargen;
        private NumericUpDown nudMargen;
        private Label lblStockMinimo;
        private NumericUpDown nudStockMinimo;
        private Label lblCategorias;
        private DataGridView dgvCategorias;
        private DataGridViewTextBoxColumn colCategoria;
        private DataGridViewTextBoxColumn colMargenCategoria;
        private Label lblAyuda;
        private CheckBox chkRecalcular;
        private CheckBox chkAplicarMinimo;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
