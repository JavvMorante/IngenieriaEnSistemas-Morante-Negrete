namespace VetCare
{
    partial class Bloquear_DesbloquearOperacionesaUsuario
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
            chkListOperaciones = new CheckedListBox();
            btnConfirmar = new Button();
            btnCancelar = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(36, 44);
            label1.Name = "label1";
            label1.Size = new Size(62, 20);
            label1.TabIndex = 0;
            label1.Text = "Usuario:";
            // 
            // cmbUsuario
            // 
            cmbUsuario.FormattingEnabled = true;
            cmbUsuario.Location = new Point(170, 44);
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.Size = new Size(484, 28);
            cmbUsuario.TabIndex = 1;
            cmbUsuario.SelectedValueChanged += cmbUsuario_SelectedValueChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chkListOperaciones);
            groupBox1.Location = new Point(38, 96);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(616, 328);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Operaciones bloqueadas";
            // 
            // chkListOperaciones
            // 
            chkListOperaciones.FormattingEnabled = true;
            chkListOperaciones.Location = new Point(28, 41);
            chkListOperaciones.Name = "chkListOperaciones";
            chkListOperaciones.Size = new Size(559, 246);
            chkListOperaciones.TabIndex = 0;
            // 
            // btnConfirmar
            // 
            btnConfirmar.Location = new Point(113, 461);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(134, 47);
            btnConfirmar.TabIndex = 3;
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.UseVisualStyleBackColor = true;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(450, 461);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(134, 47);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // Bloquear_DesbloquearOperacionesaUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(701, 540);
            Controls.Add(btnCancelar);
            Controls.Add(btnConfirmar);
            Controls.Add(groupBox1);
            Controls.Add(cmbUsuario);
            Controls.Add(label1);
            Name = "Bloquear_DesbloquearOperacionesaUsuario";
            Text = "Bloquear_DesbloquearOperacionesaUsuario";
            Load += Bloquear_DesbloquearOperacionesaUsuario_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cmbUsuario;
        private GroupBox groupBox1;
        private CheckedListBox chkListOperaciones;
        private Button btnConfirmar;
        private Button btnCancelar;
    }
}