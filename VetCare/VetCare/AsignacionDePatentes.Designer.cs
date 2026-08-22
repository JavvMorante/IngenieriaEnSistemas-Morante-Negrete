namespace VetCare
{
    partial class AsignacionDePatentes
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
            btSeleccionar = new Button();
            btCancelar = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 31);
            label1.Name = "label1";
            label1.Size = new Size(152, 20);
            label1.TabIndex = 0;
            label1.Text = "Seleccione un usuario";
            // 
            // cmbUsuario
            // 
            cmbUsuario.FormattingEnabled = true;
            cmbUsuario.Location = new Point(28, 87);
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.Size = new Size(352, 28);
            cmbUsuario.TabIndex = 1;
            // 
            // btSeleccionar
            // 
            btSeleccionar.Location = new Point(428, 87);
            btSeleccionar.Name = "btSeleccionar";
            btSeleccionar.Size = new Size(94, 29);
            btSeleccionar.TabIndex = 2;
            btSeleccionar.Text = "Seleccionar";
            btSeleccionar.UseVisualStyleBackColor = true;
            btSeleccionar.Click += btSeleccionar_Click;
            // 
            // btCancelar
            // 
            btCancelar.Location = new Point(286, 156);
            btCancelar.Name = "btCancelar";
            btCancelar.Size = new Size(94, 29);
            btCancelar.TabIndex = 3;
            btCancelar.Text = "Cancelar";
            btCancelar.UseVisualStyleBackColor = true;
            btCancelar.Click += btCancelar_Click;
            // 
            // AsignacionDePatentes
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(552, 201);
            Controls.Add(btCancelar);
            Controls.Add(btSeleccionar);
            Controls.Add(cmbUsuario);
            Controls.Add(label1);
            Name = "AsignacionDePatentes";
            Text = "AsignacionDePatentes";
            Load += AsignacionDePatentes_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cmbUsuario;
        private Button btSeleccionar;
        private Button btCancelar;
    }
}