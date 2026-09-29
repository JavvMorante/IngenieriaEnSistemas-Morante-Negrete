namespace VetCare
{
    partial class DesbloquearOperacionAUsuarios
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
            cmbUsuario = new ComboBox();
            groupBox1 = new GroupBox();
            chklistOperaciones = new CheckedListBox();
            btnDesbloquear = new Button();
            btnCancelar = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(47, 43);
            label1.Name = "label1";
            label1.Size = new Size(62, 20);
            label1.TabIndex = 0;
            label1.Text = "Usuario:";
            // 
            // cmbUsuario
            // 
            cmbUsuario.FormattingEnabled = true;
            cmbUsuario.Location = new Point(155, 43);
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.Size = new Size(362, 28);
            cmbUsuario.TabIndex = 1;
            cmbUsuario.SelectedValueChanged += cmbUsuario_SelectedValueChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chklistOperaciones);
            groupBox1.Location = new Point(56, 134);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(461, 292);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Operaciones bloqueadas";
            // 
            // chklistOperaciones
            // 
            chklistOperaciones.FormattingEnabled = true;
            chklistOperaciones.Location = new Point(16, 37);
            chklistOperaciones.Name = "chklistOperaciones";
            chklistOperaciones.Size = new Size(414, 224);
            chklistOperaciones.TabIndex = 0;
            // 
            // btnDesbloquear
            // 
            btnDesbloquear.Location = new Point(72, 457);
            btnDesbloquear.Name = "btnDesbloquear";
            btnDesbloquear.Size = new Size(116, 44);
            btnDesbloquear.TabIndex = 3;
            btnDesbloquear.Text = "Desbloquear";
            btnDesbloquear.UseVisualStyleBackColor = true;
            btnDesbloquear.Click += btnDesbloquear_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(390, 457);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(116, 44);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // DesbloquearOperacionAUsuarios
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(568, 525);
            Controls.Add(btnCancelar);
            Controls.Add(btnDesbloquear);
            Controls.Add(groupBox1);
            Controls.Add(cmbUsuario);
            Controls.Add(label1);
            Name = "DesbloquearOperacionAUsuarios";
            Text = "DesbloquearOperacionAUsuarios";
            Load += DesbloquearOperacionAUsuarios_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cmbUsuario;
        private GroupBox groupBox1;
        private CheckedListBox chklistOperaciones;
        private Button btnDesbloquear;
        private Button btnCancelar;
    }
}