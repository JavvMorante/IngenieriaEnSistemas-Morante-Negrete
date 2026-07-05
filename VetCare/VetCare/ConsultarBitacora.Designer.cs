namespace VetCare
{
    partial class ConsultarBitacora
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
            btnConsultar = new Button();
            dtphasta = new DateTimePicker();
            cmbUsuario = new ComboBox();
            cmbCriticidad = new ComboBox();
            label4 = new Label();
            dtpdesde = new DateTimePicker();
            label1 = new Label();
            label3 = new Label();
            label2 = new Label();
            dgvBitacora = new DataGridView();
            label5 = new Label();
            btnExpExcel = new Button();
            btnSalir = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBitacora).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnConsultar);
            groupBox1.Controls.Add(dtphasta);
            groupBox1.Controls.Add(cmbUsuario);
            groupBox1.Controls.Add(cmbCriticidad);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(dtpdesde);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new Point(38, 39);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(938, 178);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtros";
            // 
            // btnConsultar
            // 
            btnConsultar.Location = new Point(772, 59);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(160, 93);
            btnConsultar.TabIndex = 6;
            btnConsultar.Text = "Consultar Bitácora";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;
            // 
            // dtphasta
            // 
            dtphasta.Location = new Point(432, 58);
            dtphasta.Name = "dtphasta";
            dtphasta.Size = new Size(296, 27);
            dtphasta.TabIndex = 5;
            // 
            // cmbUsuario
            // 
            cmbUsuario.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUsuario.FormattingEnabled = true;
            cmbUsuario.Location = new Point(432, 124);
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.Size = new Size(193, 28);
            cmbUsuario.TabIndex = 2;
            // 
            // cmbCriticidad
            // 
            cmbCriticidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCriticidad.FormattingEnabled = true;
            cmbCriticidad.Location = new Point(23, 124);
            cmbCriticidad.Name = "cmbCriticidad";
            cmbCriticidad.Size = new Size(193, 28);
            cmbCriticidad.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(432, 101);
            label4.Name = "label4";
            label4.Size = new Size(139, 20);
            label4.TabIndex = 4;
            label4.Text = "Nombre de Usuario";
            // 
            // dtpdesde
            // 
            dtpdesde.Location = new Point(23, 58);
            dtpdesde.Name = "dtpdesde";
            dtpdesde.Size = new Size(296, 27);
            dtpdesde.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 35);
            label1.Name = "label1";
            label1.Size = new Size(91, 20);
            label1.TabIndex = 1;
            label1.Text = "Fecha desde";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(432, 35);
            label3.Name = "label3";
            label3.Size = new Size(89, 20);
            label3.TabIndex = 3;
            label3.Text = "Fecha Hasta";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 101);
            label2.Name = "label2";
            label2.Size = new Size(73, 20);
            label2.TabIndex = 2;
            label2.Text = "Criticidad";
            // 
            // dgvBitacora
            // 
            dgvBitacora.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBitacora.Location = new Point(38, 294);
            dgvBitacora.Name = "dgvBitacora";
            dgvBitacora.RowHeadersWidth = 51;
            dgvBitacora.Size = new Size(938, 297);
            dgvBitacora.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(38, 245);
            label5.Name = "label5";
            label5.Size = new Size(64, 20);
            label5.TabIndex = 2;
            label5.Text = "Bitácora";
            // 
            // btnExpExcel
            // 
            btnExpExcel.Location = new Point(370, 611);
            btnExpExcel.Name = "btnExpExcel";
            btnExpExcel.Size = new Size(201, 51);
            btnExpExcel.TabIndex = 7;
            btnExpExcel.Text = "Exportar a Excel";
            btnExpExcel.UseVisualStyleBackColor = true;
            btnExpExcel.Click += btnExpExcel_Click;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(775, 611);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(201, 51);
            btnSalir.TabIndex = 8;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // ConsultarBitacora
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1010, 690);
            Controls.Add(btnSalir);
            Controls.Add(btnExpExcel);
            Controls.Add(label5);
            Controls.Add(dgvBitacora);
            Controls.Add(groupBox1);
            Name = "ConsultarBitacora";
            Text = "ConsultarBitacora";
            Load += ConsultarBitacora_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBitacora).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox cmbUsuario;
        private DateTimePicker dateTimePicker3;
        private ComboBox cmbCriticidad;
        private Label label4;
        private DateTimePicker dtpdesde;
        private Label label1;
        private Label label3;
        private Label label2;
        private Button btnConsultar;
        private DateTimePicker dtphasta;
        private DataGridView dgvBitacora;
        private Label label5;
        private Button btnExpExcel;
        private Button btnSalir;
    }
}