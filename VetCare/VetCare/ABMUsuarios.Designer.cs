namespace VetCare
{
    partial class ABMUsuarios
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
            dgvUsuarios = new DataGridView();
            btSalir = new Button();
            btnAltaUsuario = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(52, 26);
            label1.Name = "label1";
            label1.Size = new Size(65, 20);
            label1.TabIndex = 0;
            label1.Text = "Usuarios";
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(57, 74);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.RowHeadersWidth = 51;
            dgvUsuarios.Size = new Size(1115, 296);
            dgvUsuarios.TabIndex = 1;
            dgvUsuarios.CellClick += dgvUsuarios_CellClick;
            // 
            // btSalir
            // 
            btSalir.Location = new Point(1077, 446);
            btSalir.Name = "btSalir";
            btSalir.Size = new Size(94, 29);
            btSalir.TabIndex = 2;
            btSalir.Text = "Salir";
            btSalir.UseVisualStyleBackColor = true;
            btSalir.Click += btSalir_Click;
            // 
            // btnAltaUsuario
            // 
            btnAltaUsuario.Location = new Point(884, 446);
            btnAltaUsuario.Name = "btnAltaUsuario";
            btnAltaUsuario.Size = new Size(123, 29);
            btnAltaUsuario.TabIndex = 3;
            btnAltaUsuario.Text = "Alta Usuario";
            btnAltaUsuario.UseVisualStyleBackColor = true;
            btnAltaUsuario.Click += btnAltaUsuario_Click;
            // 
            // ABMUsuarios
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1196, 636);
            Controls.Add(btnAltaUsuario);
            Controls.Add(btSalir);
            Controls.Add(dgvUsuarios);
            Controls.Add(label1);
            Name = "ABMUsuarios";
            Text = "ABMUsuarios";
            Load += ABMUsuarios_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DataGridView dgvUsuarios;
        private Button btSalir;
        private Button btnAltaUsuario;
    }
}