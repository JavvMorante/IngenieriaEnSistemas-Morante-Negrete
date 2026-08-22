namespace VetCare
{
    partial class CambiarConexionBasedeDatos
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
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(312, 51);
            label1.Name = "label1";
            label1.Size = new Size(166, 20);
            label1.TabIndex = 0;
            label1.Text = "Indicar Nueva Conexion";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(42, 88);
            label2.Name = "label2";
            label2.Size = new Size(67, 20);
            label2.TabIndex = 1;
            label2.Text = "Servidor:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(42, 151);
            label3.Name = "label3";
            label3.Size = new Size(62, 20);
            label3.TabIndex = 2;
            label3.Text = "Usuario:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(42, 214);
            label4.Name = "label4";
            label4.Size = new Size(48, 20);
            label4.TabIndex = 3;
            label4.Text = "Clave:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(42, 280);
            label5.Name = "label5";
            label5.Size = new Size(102, 20);
            label5.TabIndex = 4;
            label5.Text = "Nombre Base:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(194, 81);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(462, 27);
            textBox1.TabIndex = 5;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(194, 144);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(462, 27);
            textBox2.TabIndex = 6;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(194, 207);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(462, 27);
            textBox3.TabIndex = 7;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(194, 273);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(462, 27);
            textBox4.TabIndex = 8;
            // 
            // button1
            // 
            button1.Location = new Point(102, 361);
            button1.Name = "button1";
            button1.Size = new Size(136, 46);
            button1.TabIndex = 9;
            button1.Text = "Probar conexion";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(338, 361);
            button2.Name = "button2";
            button2.Size = new Size(115, 46);
            button2.TabIndex = 10;
            button2.Text = "Confirmar";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(546, 361);
            button3.Name = "button3";
            button3.Size = new Size(110, 46);
            button3.TabIndex = 11;
            button3.Text = "Salir";
            button3.UseVisualStyleBackColor = true;
            // 
            // CambiarConexionBasedeDatos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "CambiarConexionBasedeDatos";
            Text = "CambiarConexionBasedeDatos";
            Load += CambiarConexionBasedeDatos_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}