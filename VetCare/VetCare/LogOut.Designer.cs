namespace VetCare
{
    partial class LogOut
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
            btn_salir = new Button();
            btn_cancelar = new Button();
            lblConfirm = new Label();
            SuspendLayout();
            // 
            // btn_salir
            // 
            btn_salir.Location = new Point(57, 135);
            btn_salir.Name = "btn_salir";
            btn_salir.Size = new Size(129, 77);
            btn_salir.TabIndex = 0;
            btn_salir.Text = "Salir";
            btn_salir.UseVisualStyleBackColor = true;
            btn_salir.Click += btn_salir_Click;
            // 
            // btn_cancelar
            // 
            btn_cancelar.Location = new Point(351, 135);
            btn_cancelar.Name = "btn_cancelar";
            btn_cancelar.Size = new Size(129, 77);
            btn_cancelar.TabIndex = 1;
            btn_cancelar.Text = "Volver al sistema";
            btn_cancelar.UseVisualStyleBackColor = true;
            btn_cancelar.Click += btn_cancelar_Click;
            // 
            // lblConfirm
            // 
            lblConfirm.AutoSize = true;
            lblConfirm.Location = new Point(154, 67);
            lblConfirm.Name = "lblConfirm";
            lblConfirm.Size = new Size(202, 20);
            lblConfirm.TabIndex = 2;
            lblConfirm.Text = "¿Esta seguro que desea salir?";
            // 
            // LogOut
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 255);
            Controls.Add(lblConfirm);
            Controls.Add(btn_cancelar);
            Controls.Add(btn_salir);
            Name = "LogOut";
            Text = "LogOut";
            Load += LogOut_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_salir;
        private Button btn_cancelar;
        private Label lblConfirm;
    }
}