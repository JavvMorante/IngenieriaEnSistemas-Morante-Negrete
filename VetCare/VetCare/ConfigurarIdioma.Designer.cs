namespace VetCare
{
    partial class ConfigurarIdioma
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
            cmbIdioma = new ComboBox();
            btnConfirmar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(58, 37);
            label1.Name = "label1";
            label1.Size = new Size(242, 20);
            label1.TabIndex = 0;
            label1.Text = "Seleccionar Idioma para el Sistema";
            // 
            // cmbIdioma
            // 
            cmbIdioma.FormattingEnabled = true;
            cmbIdioma.Location = new Point(56, 99);
            cmbIdioma.Name = "cmbIdioma";
            cmbIdioma.Size = new Size(298, 28);
            cmbIdioma.TabIndex = 1;
            // 
            // btnConfirmar
            // 
            btnConfirmar.Location = new Point(58, 194);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(127, 49);
            btnConfirmar.TabIndex = 2;
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.UseVisualStyleBackColor = true;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(235, 194);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(119, 49);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // ConfigurarIdioma
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(445, 329);
            Controls.Add(btnCancelar);
            Controls.Add(btnConfirmar);
            Controls.Add(cmbIdioma);
            Controls.Add(label1);
            Name = "ConfigurarIdioma";
            Text = "ConfigurarIdioma";
            Load += ConfigurarIdioma_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cmbIdioma;
        private Button btnConfirmar;
        private Button btnCancelar;
    }
}