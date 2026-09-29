namespace VetCare
{
    partial class ImportarBitacora
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
            label1 = new Label();
            dgvBitacora = new DataGridView();
            btnImportar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvBitacora).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 41);
            label1.Name = "label1";
            label1.Size = new Size(64, 20);
            label1.TabIndex = 0;
            label1.Text = "Bitácora";
            // 
            // dgvBitacora
            // 
            dgvBitacora.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBitacora.Location = new Point(46, 89);
            dgvBitacora.Name = "dgvBitacora";
            dgvBitacora.RowHeadersWidth = 51;
            dgvBitacora.Size = new Size(867, 498);
            dgvBitacora.TabIndex = 1;
            // 
            // btnImportar
            // 
            btnImportar.Location = new Point(360, 634);
            btnImportar.Name = "btnImportar";
            btnImportar.Size = new Size(173, 60);
            btnImportar.TabIndex = 2;
            btnImportar.Text = "Importar Bitacora";
            btnImportar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(740, 634);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(173, 60);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Salir";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // ImportarBitacora
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(951, 716);
            Controls.Add(btnCancelar);
            Controls.Add(btnImportar);
            Controls.Add(dgvBitacora);
            Controls.Add(label1);
            Name = "ImportarBitacora";
            Text = "ImportarBitacora";
            ((System.ComponentModel.ISupportInitialize)dgvBitacora).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DataGridView dgvBitacora;
        private Button btnImportar;
        private Button btnCancelar;
    }
}