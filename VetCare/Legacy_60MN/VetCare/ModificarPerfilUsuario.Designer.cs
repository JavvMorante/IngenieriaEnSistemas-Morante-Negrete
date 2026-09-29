namespace VetCare
{
    partial class ModificarPerfilUsuario
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
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            listBox1 = new ListBox();
            btncrearperfil = new Button();
            button2 = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 48);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(575, 558);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Perfil de Usuario";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(6, 178);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(460, 345);
            textBox2.TabIndex = 3;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(6, 80);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(460, 27);
            textBox1.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 138);
            label2.Name = "label2";
            label2.Size = new Size(90, 20);
            label2.TabIndex = 1;
            label2.Text = "Descripcion:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 48);
            label1.Name = "label1";
            label1.Size = new Size(67, 20);
            label1.TabIndex = 0;
            label1.Text = "Nombre:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(listBox1);
            groupBox2.Location = new Point(621, 48);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(575, 558);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Operaciones";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(22, 48);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(530, 484);
            listBox1.TabIndex = 0;
            // 
            // btncrearperfil
            // 
            btncrearperfil.Location = new Point(544, 645);
            btncrearperfil.Name = "btncrearperfil";
            btncrearperfil.Size = new Size(143, 48);
            btncrearperfil.TabIndex = 2;
            btncrearperfil.Text = "Crear Perfil";
            btncrearperfil.UseVisualStyleBackColor = true;
            btncrearperfil.Click += btncrearperfil_Click;
            // 
            // button2
            // 
            button2.Location = new Point(964, 645);
            button2.Name = "button2";
            button2.Size = new Size(122, 48);
            button2.TabIndex = 3;
            button2.Text = "Cancelar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // ModificarPerfilUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 707);
            Controls.Add(button2);
            Controls.Add(btncrearperfil);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "ModificarPerfilUsuario";
            Text = "ModificarPerfilUsuario";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox textBox2;
        private TextBox textBox1;
        private Label label2;
        private Label label1;
        private GroupBox groupBox2;
        private ListBox listBox1;
        private Button btncrearperfil;
        private Button button2;
    }
}