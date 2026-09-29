namespace VetCare
{
    partial class FrmPermisosUsuario_60MN
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
            lblTitulo = new Label();
            lblFamiliasUsuario = new Label();
            clbFamiliasUsuario = new CheckedListBox();
            lblPatentesUsuario = new Label();
            clbPatentesUsuario = new CheckedListBox();
            lblPermisosEfectivos = new Label();
            tvwPermisos = new TreeView();
            lblResumen = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(206, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Permisos del usuario";
            //
            // lblFamiliasUsuario
            //
            lblFamiliasUsuario.AutoSize = true;
            lblFamiliasUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFamiliasUsuario.Location = new Point(12, 52);
            lblFamiliasUsuario.Name = "lblFamiliasUsuario";
            lblFamiliasUsuario.Size = new Size(197, 20);
            lblFamiliasUsuario.TabIndex = 1;
            lblFamiliasUsuario.Text = "Familias (tipo de usuario)";
            //
            // clbFamiliasUsuario
            //
            clbFamiliasUsuario.CheckOnClick = true;
            clbFamiliasUsuario.FormattingEnabled = true;
            clbFamiliasUsuario.IntegralHeight = false;
            clbFamiliasUsuario.Location = new Point(12, 76);
            clbFamiliasUsuario.Name = "clbFamiliasUsuario";
            clbFamiliasUsuario.Size = new Size(360, 180);
            clbFamiliasUsuario.TabIndex = 2;
            clbFamiliasUsuario.ItemCheck += clb_ItemCheck;
            //
            // lblPatentesUsuario
            //
            lblPatentesUsuario.AutoSize = true;
            lblPatentesUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPatentesUsuario.Location = new Point(12, 268);
            lblPatentesUsuario.Name = "lblPatentesUsuario";
            lblPatentesUsuario.Size = new Size(341, 20);
            lblPatentesUsuario.TabIndex = 3;
            lblPatentesUsuario.Text = "Patentes individuales (además de las de su familia)";
            //
            // clbPatentesUsuario
            //
            clbPatentesUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            clbPatentesUsuario.CheckOnClick = true;
            clbPatentesUsuario.FormattingEnabled = true;
            clbPatentesUsuario.IntegralHeight = false;
            clbPatentesUsuario.Location = new Point(12, 292);
            clbPatentesUsuario.Name = "clbPatentesUsuario";
            clbPatentesUsuario.Size = new Size(360, 210);
            clbPatentesUsuario.TabIndex = 4;
            clbPatentesUsuario.ItemCheck += clb_ItemCheck;
            //
            // lblPermisosEfectivos
            //
            lblPermisosEfectivos.AutoSize = true;
            lblPermisosEfectivos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPermisosEfectivos.Location = new Point(392, 52);
            lblPermisosEfectivos.Name = "lblPermisosEfectivos";
            lblPermisosEfectivos.Size = new Size(229, 20);
            lblPermisosEfectivos.TabIndex = 5;
            lblPermisosEfectivos.Text = "Permisos efectivos del usuario";
            //
            // tvwPermisos
            //
            tvwPermisos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tvwPermisos.Location = new Point(392, 76);
            tvwPermisos.Name = "tvwPermisos";
            tvwPermisos.Size = new Size(490, 426);
            tvwPermisos.TabIndex = 6;
            //
            // lblResumen
            //
            lblResumen.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblResumen.AutoSize = true;
            lblResumen.ForeColor = Color.DimGray;
            lblResumen.Location = new Point(12, 516);
            lblResumen.Name = "lblResumen";
            lblResumen.Size = new Size(0, 20);
            lblResumen.TabIndex = 7;
            //
            // btnAceptar
            //
            btnAceptar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAceptar.Location = new Point(652, 510);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 34);
            btnAceptar.TabIndex = 8;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            //
            // btnCancelar
            //
            btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(772, 510);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 34);
            btnCancelar.TabIndex = 9;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmPermisosUsuario_60MN
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(894, 556);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(lblResumen);
            Controls.Add(tvwPermisos);
            Controls.Add(lblPermisosEfectivos);
            Controls.Add(clbPatentesUsuario);
            Controls.Add(lblPatentesUsuario);
            Controls.Add(clbFamiliasUsuario);
            Controls.Add(lblFamiliasUsuario);
            Controls.Add(lblTitulo);
            MinimizeBox = false;
            MinimumSize = new Size(820, 500);
            Name = "FrmPermisosUsuario_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Permisos del usuario";
            Load += FrmPermisosUsuario_60MN_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblFamiliasUsuario;
        private CheckedListBox clbFamiliasUsuario;
        private Label lblPatentesUsuario;
        private CheckedListBox clbPatentesUsuario;
        private Label lblPermisosEfectivos;
        private TreeView tvwPermisos;
        private Label lblResumen;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}
