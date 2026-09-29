namespace VetCare
{
    partial class DigitosVerificadores
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
            button1 = new Button();
            btnVerificar = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(51, 40);
            button1.Name = "button1";
            button1.Size = new Size(250, 90);
            button1.TabIndex = 0;
            button1.Text = "ReCalcular Digitos Verificadores";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnVerificar
            // 
            btnVerificar.Location = new Point(51, 167);
            btnVerificar.Name = "btnVerificar";
            btnVerificar.Size = new Size(250, 94);
            btnVerificar.TabIndex = 1;
            btnVerificar.Text = "Consultar Digitos Verificadores";
            btnVerificar.UseVisualStyleBackColor = true;
            btnVerificar.Click += btnVerificar_Click;
            // 
            // button3
            // 
            button3.Location = new Point(51, 294);
            button3.Name = "button3";
            button3.Size = new Size(250, 92);
            button3.TabIndex = 2;
            button3.Text = "Salir";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // DigitosVerificadores
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(355, 415);
            Controls.Add(button3);
            Controls.Add(btnVerificar);
            Controls.Add(button1);
            Name = "DigitosVerificadores";
            Text = "DigitosVerificadores";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button btnVerificar;
        private Button button3;
    }
}