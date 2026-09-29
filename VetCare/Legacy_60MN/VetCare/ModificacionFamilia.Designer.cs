namespace VetCare
{
    partial class ModificacionFamilia
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
            txtDescripcionPerfil = new TextBox();
            txtNombrePerfil = new TextBox();
            btnModificarPerfil = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 38);
            label1.Name = "label1";
            label1.Size = new Size(125, 20);
            label1.TabIndex = 0;
            label1.Text = "Nombre de Perfil:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(32, 128);
            label2.Name = "label2";
            label2.Size = new Size(152, 20);
            label2.TabIndex = 1;
            label2.Text = "Descripcion del Perfil:";
            // 
            // txtDescripcionPerfil
            // 
            txtDescripcionPerfil.Location = new Point(32, 176);
            txtDescripcionPerfil.Multiline = true;
            txtDescripcionPerfil.Name = "txtDescripcionPerfil";
            txtDescripcionPerfil.Size = new Size(428, 135);
            txtDescripcionPerfil.TabIndex = 2;
            // 
            // txtNombrePerfil
            // 
            txtNombrePerfil.Location = new Point(32, 61);
            txtNombrePerfil.Name = "txtNombrePerfil";
            txtNombrePerfil.Size = new Size(428, 27);
            txtNombrePerfil.TabIndex = 3;
            // 
            // btnModificarPerfil
            // 
            btnModificarPerfil.Location = new Point(156, 339);
            btnModificarPerfil.Name = "btnModificarPerfil";
            btnModificarPerfil.Size = new Size(98, 48);
            btnModificarPerfil.TabIndex = 4;
            btnModificarPerfil.Text = "Modificar Perfil";
            btnModificarPerfil.UseVisualStyleBackColor = true;
            btnModificarPerfil.Click += btnModificarPerfil_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(362, 339);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(98, 48);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // ModificacionFamilia
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(498, 410);
            Controls.Add(btnCancelar);
            Controls.Add(btnModificarPerfil);
            Controls.Add(txtNombrePerfil);
            Controls.Add(txtDescripcionPerfil);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ModificacionFamilia";
            Text = "ModificacionFamilia";
            Load += ModificacionFamilia_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtDescripcionPerfil;
        private TextBox txtNombrePerfil;
        private Button btnModificarPerfil;
        private Button btnCancelar;
    }
}