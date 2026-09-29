namespace VetCare
{
    partial class AltaFamilia
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
            txtNombrePerfil = new TextBox();
            txtDescripcionPerfil = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            btAltaPerfil = new Button();
            btCancelar = new Button();
            SuspendLayout();
            // 
            // txtNombrePerfil
            // 
            txtNombrePerfil.Location = new Point(49, 62);
            txtNombrePerfil.Name = "txtNombrePerfil";
            txtNombrePerfil.Size = new Size(637, 27);
            txtNombrePerfil.TabIndex = 0;
            // 
            // txtDescripcionPerfil
            // 
            txtDescripcionPerfil.Location = new Point(49, 160);
            txtDescripcionPerfil.Name = "txtDescripcionPerfil";
            txtDescripcionPerfil.Size = new Size(637, 155);
            txtDescripcionPerfil.TabIndex = 2;
            txtDescripcionPerfil.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(49, 19);
            label1.Name = "label1";
            label1.Size = new Size(124, 20);
            label1.TabIndex = 3;
            label1.Text = "Nombre de perfil";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(49, 116);
            label2.Name = "label2";
            label2.Size = new Size(151, 20);
            label2.TabIndex = 4;
            label2.Text = "Descripcion del perfil";
            // 
            // btAltaPerfil
            // 
            btAltaPerfil.Location = new Point(350, 379);
            btAltaPerfil.Name = "btAltaPerfil";
            btAltaPerfil.Size = new Size(94, 29);
            btAltaPerfil.TabIndex = 5;
            btAltaPerfil.Text = "Alta Perfil";
            btAltaPerfil.UseVisualStyleBackColor = true;
            btAltaPerfil.Click += btAltaPerfil_Click;
            // 
            // btCancelar
            // 
            btCancelar.Location = new Point(592, 379);
            btCancelar.Name = "btCancelar";
            btCancelar.Size = new Size(94, 29);
            btCancelar.TabIndex = 6;
            btCancelar.Text = "Cancelar";
            btCancelar.UseVisualStyleBackColor = true;
            // 
            // AltaFamilia
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btCancelar);
            Controls.Add(btAltaPerfil);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtDescripcionPerfil);
            Controls.Add(txtNombrePerfil);
            Name = "AltaFamilia";
            Text = "AltaFamilia";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtNombrePerfil;
        private RichTextBox txtDescripcionPerfil;
        private Label label1;
        private Label label2;
        private Button btAltaPerfil;
        private Button btCancelar;
    }
}