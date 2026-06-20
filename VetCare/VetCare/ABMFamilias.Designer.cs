namespace VetCare
{
    partial class ABMFamilias
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
            dgvPerfilUsuario = new DataGridView();
            label1 = new Label();
            btAltaFamilia = new Button();
            btCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPerfilUsuario).BeginInit();
            SuspendLayout();
            // 
            // dgvPerfilUsuario
            // 
            dgvPerfilUsuario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPerfilUsuario.Location = new Point(38, 104);
            dgvPerfilUsuario.Name = "dgvPerfilUsuario";
            dgvPerfilUsuario.RowHeadersWidth = 51;
            dgvPerfilUsuario.Size = new Size(701, 256);
            dgvPerfilUsuario.TabIndex = 0;
            dgvPerfilUsuario.CellClick += dgvPerfilUsuario_CellClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(48, 34);
            label1.Name = "label1";
            label1.Size = new Size(131, 20);
            label1.TabIndex = 1;
            label1.Text = "Perfiles de Usuario";
            // 
            // btAltaFamilia
            // 
            btAltaFamilia.Location = new Point(417, 400);
            btAltaFamilia.Name = "btAltaFamilia";
            btAltaFamilia.Size = new Size(103, 29);
            btAltaFamilia.TabIndex = 2;
            btAltaFamilia.Text = "Alta Familia";
            btAltaFamilia.UseVisualStyleBackColor = true;
            btAltaFamilia.Click += btAltaFamilia_Click;
            // 
            // btCancelar
            // 
            btCancelar.Location = new Point(645, 400);
            btCancelar.Name = "btCancelar";
            btCancelar.Size = new Size(94, 29);
            btCancelar.TabIndex = 3;
            btCancelar.Text = "Cancelar";
            btCancelar.UseVisualStyleBackColor = true;
            btCancelar.Click += btCancelar_Click;
            // 
            // ABMFamilias
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btCancelar);
            Controls.Add(btAltaFamilia);
            Controls.Add(label1);
            Controls.Add(dgvPerfilUsuario);
            Name = "ABMFamilias";
            Text = "ABMFamilias";
            Load += ABMFamilias_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPerfilUsuario).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvPerfilUsuario;
        private Label label1;
        private Button btAltaFamilia;
        private Button btCancelar;
    }
}