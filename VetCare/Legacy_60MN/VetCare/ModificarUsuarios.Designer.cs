namespace VetCare
{
    partial class ModificarUsuarios
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
            checkHabilitado = new CheckBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            txtusuario = new TextBox();
            txtdni = new TextBox();
            txtapellido = new TextBox();
            txtnombre = new TextBox();
            txtemail = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // checkHabilitado
            // 
            checkHabilitado.AutoSize = true;
            checkHabilitado.Location = new Point(155, 33);
            checkHabilitado.Name = "checkHabilitado";
            checkHabilitado.Size = new Size(102, 24);
            checkHabilitado.TabIndex = 0;
            checkHabilitado.Text = "Habilitado";
            checkHabilitado.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(33, 85);
            label1.Name = "label1";
            label1.Size = new Size(62, 20);
            label1.TabIndex = 1;
            label1.Text = "Usuario:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(33, 136);
            label2.Name = "label2";
            label2.Size = new Size(67, 20);
            label2.TabIndex = 2;
            label2.Text = "Nombre:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(33, 187);
            label3.Name = "label3";
            label3.Size = new Size(69, 20);
            label3.TabIndex = 3;
            label3.Text = "Apellido:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(33, 236);
            label4.Name = "label4";
            label4.Size = new Size(38, 20);
            label4.TabIndex = 4;
            label4.Text = "DNI:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(33, 289);
            label5.Name = "label5";
            label5.Size = new Size(49, 20);
            label5.TabIndex = 5;
            label5.Text = "Email:";
            // 
            // txtusuario
            // 
            txtusuario.Location = new Point(132, 85);
            txtusuario.Name = "txtusuario";
            txtusuario.Size = new Size(395, 27);
            txtusuario.TabIndex = 6;
            // 
            // txtdni
            // 
            txtdni.Location = new Point(132, 236);
            txtdni.Name = "txtdni";
            txtdni.Size = new Size(395, 27);
            txtdni.TabIndex = 12;
            txtdni.KeyPress += txtdni_KeyPress;
            // 
            // txtapellido
            // 
            txtapellido.Location = new Point(132, 187);
            txtapellido.Name = "txtapellido";
            txtapellido.Size = new Size(395, 27);
            txtapellido.TabIndex = 13;
            // 
            // txtnombre
            // 
            txtnombre.Location = new Point(132, 133);
            txtnombre.Name = "txtnombre";
            txtnombre.Size = new Size(395, 27);
            txtnombre.TabIndex = 14;
            // 
            // txtemail
            // 
            txtemail.Location = new Point(132, 286);
            txtemail.Name = "txtemail";
            txtemail.Size = new Size(395, 27);
            txtemail.TabIndex = 15;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(132, 382);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(125, 45);
            btnGuardar.TabIndex = 16;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(413, 382);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(114, 45);
            btnCancelar.TabIndex = 17;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // ModificarUsuarios
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(602, 450);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(txtemail);
            Controls.Add(txtnombre);
            Controls.Add(txtapellido);
            Controls.Add(txtdni);
            Controls.Add(txtusuario);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(checkHabilitado);
            Name = "ModificarUsuarios";
            Text = "ModificarUsuarios";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox checkHabilitado;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtusuario;
        private TextBox txtdni;
        private TextBox txtapellido;
        private TextBox txtnombre;
        private TextBox txtemail;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}