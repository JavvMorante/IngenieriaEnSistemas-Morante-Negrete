namespace VetCare
{
    partial class ModifyUser_60MN
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
            btnCancelar = new Button();
            btnGuardar = new Button();
            txtEmail = new TextBox();
            txtDNI = new TextBox();
            txtApellido = new TextBox();
            txtNombre = new TextBox();
            txtUsuario = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            chkHabilitado = new CheckBox();
            SuspendLayout();
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(435, 387);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(94, 29);
            btnCancelar.TabIndex = 25;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(196, 387);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(94, 29);
            btnGuardar.TabIndex = 24;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(93, 277);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(436, 27);
            txtEmail.TabIndex = 23;
            // 
            // txtDNI
            // 
            txtDNI.Location = new Point(93, 230);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(436, 27);
            txtDNI.TabIndex = 22;
            txtDNI.KeyPress += txtDNI_KeyPress;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(93, 180);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(436, 27);
            txtApellido.TabIndex = 21;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(93, 133);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(436, 27);
            txtNombre.TabIndex = 20;
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(93, 92);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(436, 27);
            txtUsuario.TabIndex = 19;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 284);
            label5.Name = "label5";
            label5.Size = new Size(46, 20);
            label5.TabIndex = 18;
            label5.Text = "Email";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 233);
            label4.Name = "label4";
            label4.Size = new Size(35, 20);
            label4.TabIndex = 17;
            label4.Text = "DNI";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 180);
            label3.Name = "label3";
            label3.Size = new Size(66, 20);
            label3.TabIndex = 16;
            label3.Text = "Apellido";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 133);
            label2.Name = "label2";
            label2.Size = new Size(64, 20);
            label2.TabIndex = 15;
            label2.Text = "Nombre";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 92);
            label1.Name = "label1";
            label1.Size = new Size(59, 20);
            label1.TabIndex = 14;
            label1.Text = "Usuario";
            // 
            // chkHabilitado
            // 
            chkHabilitado.AutoSize = true;
            chkHabilitado.Location = new Point(104, 33);
            chkHabilitado.Name = "chkHabilitado";
            chkHabilitado.Size = new Size(102, 24);
            chkHabilitado.TabIndex = 13;
            chkHabilitado.Text = "Habilitado";
            chkHabilitado.UseVisualStyleBackColor = true;
            // 
            // ModifyUser_60MN
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 450);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(txtEmail);
            Controls.Add(txtDNI);
            Controls.Add(txtApellido);
            Controls.Add(txtNombre);
            Controls.Add(txtUsuario);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(chkHabilitado);
            Name = "ModifyUser_60MN";
            Text = "ModifyUser_60MN";
            Load += ModifyUser_60MN_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCancelar;
        private Button btnGuardar;
        private TextBox txtEmail;
        private TextBox txtDNI;
        private TextBox txtApellido;
        private TextBox txtNombre;
        private TextBox txtUsuario;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private CheckBox chkHabilitado;
    }
}