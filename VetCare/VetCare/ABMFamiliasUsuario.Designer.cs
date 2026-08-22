namespace VetCare
{
    partial class ABMPerfilesUsuario
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
            groupBox1 = new GroupBox();
            dgvPerfiles = new DataGridView();
            btAlta = new Button();
            btSalir = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPerfiles).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dgvPerfiles);
            groupBox1.Location = new Point(40, 43);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(719, 320);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Perfiles de Usuario";
            // 
            // dgvPerfiles
            // 
            dgvPerfiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPerfiles.Location = new Point(6, 26);
            dgvPerfiles.Name = "dgvPerfiles";
            dgvPerfiles.RowHeadersWidth = 51;
            dgvPerfiles.Size = new Size(707, 288);
            dgvPerfiles.TabIndex = 0;
            // 
            // btAlta
            // 
            btAlta.Location = new Point(400, 402);
            btAlta.Name = "btAlta";
            btAlta.Size = new Size(94, 29);
            btAlta.TabIndex = 1;
            btAlta.Text = "Alta";
            btAlta.UseVisualStyleBackColor = true;
            btAlta.Click += btAlta_Click;
            // 
            // btSalir
            // 
            btSalir.Location = new Point(659, 402);
            btSalir.Name = "btSalir";
            btSalir.Size = new Size(94, 29);
            btSalir.TabIndex = 2;
            btSalir.Text = "Salir";
            btSalir.UseVisualStyleBackColor = true;
            btSalir.Click += btSalir_Click;
            // 
            // ABMPerfilesUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btSalir);
            Controls.Add(btAlta);
            Controls.Add(groupBox1);
            Name = "ABMPerfilesUsuario";
            Text = "ABMPerfilesUsuario";
            Load += ABMFamiliasUsuario_Load;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPerfiles).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private DataGridView dgvPerfiles;
        private Button btAlta;
        private Button btSalir;
    }
}