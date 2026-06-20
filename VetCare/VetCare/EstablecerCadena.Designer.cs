namespace VetCare
{
    partial class EstablecerCadena
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
            txtBase = new TextBox();
            txtServidor = new TextBox();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(36, 34);
            label1.Name = "label1";
            label1.Size = new Size(191, 20);
            label1.TabIndex = 0;
            label1.Text = "Ingrese los siguientes datos";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(36, 106);
            label2.Name = "label2";
            label2.Size = new Size(126, 20);
            label2.TabIndex = 1;
            label2.Text = "Nombre Servidor:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(36, 189);
            label3.Name = "label3";
            label3.Size = new Size(166, 20);
            label3.TabIndex = 2;
            label3.Text = "Nombre Base de Datos:";
            // 
            // txtBase
            // 
            txtBase.Location = new Point(208, 103);
            txtBase.Name = "txtBase";
            txtBase.Size = new Size(290, 27);
            txtBase.TabIndex = 3;
            // 
            // txtServidor
            // 
            txtServidor.Location = new Point(208, 182);
            txtServidor.Name = "txtServidor";
            txtServidor.Size = new Size(290, 27);
            txtServidor.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(170, 267);
            button1.Name = "button1";
            button1.Size = new Size(109, 43);
            button1.TabIndex = 5;
            button1.Text = "Probar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(396, 267);
            button2.Name = "button2";
            button2.Size = new Size(109, 43);
            button2.TabIndex = 6;
            button2.Text = "Cancelar";
            button2.UseVisualStyleBackColor = true;
            // 
            // EstablecerCadena
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 339);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(txtServidor);
            Controls.Add(txtBase);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "EstablecerCadena";
            Text = "EstablecerCadena";
            Load += EstablecerCadena_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtBase;
        private TextBox txtServidor;
        private Button button1;
        private Button button2;
    }
}