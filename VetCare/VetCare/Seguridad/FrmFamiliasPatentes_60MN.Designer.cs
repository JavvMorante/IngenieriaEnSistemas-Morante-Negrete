namespace VetCare
{
    partial class FrmFamiliasPatentes_60MN
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
            lblListaFamilias = new Label();
            lstFamilias = new ListBox();
            btnNuevaFamilia = new Button();
            btnEliminarFamilia = new Button();
            lblNombreFamilia = new Label();
            txtNombreFamilia = new TextBox();
            lblPatentes = new Label();
            clbPatentes = new CheckedListBox();
            lblSubfamilias = new Label();
            clbSubfamilias = new CheckedListBox();
            lblArbol = new Label();
            tvwArbol = new TreeView();
            lblUsuariosFamilia = new Label();
            btnGuardar = new Button();
            btnDescartar = new Button();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(165, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Familias y patentes";
            //
            // lblListaFamilias
            //
            lblListaFamilias.AutoSize = true;
            lblListaFamilias.Location = new Point(12, 52);
            lblListaFamilias.Name = "lblListaFamilias";
            lblListaFamilias.Size = new Size(200, 20);
            lblListaFamilias.TabIndex = 1;
            lblListaFamilias.Text = "Familias (tipos de usuario)";
            //
            // lstFamilias
            //
            lstFamilias.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lstFamilias.FormattingEnabled = true;
            lstFamilias.IntegralHeight = false;
            lstFamilias.Location = new Point(12, 76);
            lstFamilias.Name = "lstFamilias";
            lstFamilias.Size = new Size(240, 382);
            lstFamilias.TabIndex = 2;
            lstFamilias.SelectedIndexChanged += lstFamilias_SelectedIndexChanged;
            //
            // btnNuevaFamilia
            //
            btnNuevaFamilia.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNuevaFamilia.Location = new Point(12, 468);
            btnNuevaFamilia.Name = "btnNuevaFamilia";
            btnNuevaFamilia.Size = new Size(115, 34);
            btnNuevaFamilia.TabIndex = 3;
            btnNuevaFamilia.Text = "Nueva familia";
            btnNuevaFamilia.UseVisualStyleBackColor = true;
            btnNuevaFamilia.Click += btnNuevaFamilia_Click;
            //
            // btnEliminarFamilia
            //
            btnEliminarFamilia.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEliminarFamilia.Location = new Point(137, 468);
            btnEliminarFamilia.Name = "btnEliminarFamilia";
            btnEliminarFamilia.Size = new Size(115, 34);
            btnEliminarFamilia.TabIndex = 4;
            btnEliminarFamilia.Text = "Eliminar familia";
            btnEliminarFamilia.UseVisualStyleBackColor = true;
            btnEliminarFamilia.Click += btnEliminarFamilia_Click;
            //
            // lblNombreFamilia
            //
            lblNombreFamilia.AutoSize = true;
            lblNombreFamilia.Location = new Point(270, 52);
            lblNombreFamilia.Name = "lblNombreFamilia";
            lblNombreFamilia.Size = new Size(111, 20);
            lblNombreFamilia.TabIndex = 5;
            lblNombreFamilia.Text = "Nombre de la familia";
            //
            // txtNombreFamilia
            //
            txtNombreFamilia.Location = new Point(270, 76);
            txtNombreFamilia.MaxLength = 100;
            txtNombreFamilia.Name = "txtNombreFamilia";
            txtNombreFamilia.Size = new Size(320, 27);
            txtNombreFamilia.TabIndex = 6;
            //
            // lblPatentes
            //
            lblPatentes.AutoSize = true;
            lblPatentes.Location = new Point(270, 114);
            lblPatentes.Name = "lblPatentes";
            lblPatentes.Size = new Size(227, 20);
            lblPatentes.TabIndex = 7;
            lblPatentes.Text = "Patentes de la familia";
            //
            // clbPatentes
            //
            clbPatentes.CheckOnClick = true;
            clbPatentes.FormattingEnabled = true;
            clbPatentes.IntegralHeight = false;
            clbPatentes.Location = new Point(270, 138);
            clbPatentes.Name = "clbPatentes";
            clbPatentes.Size = new Size(320, 222);
            clbPatentes.TabIndex = 8;
            clbPatentes.ItemCheck += clb_ItemCheck;
            //
            // lblSubfamilias
            //
            lblSubfamilias.AutoSize = true;
            lblSubfamilias.Location = new Point(270, 370);
            lblSubfamilias.Name = "lblSubfamilias";
            lblSubfamilias.Size = new Size(210, 20);
            lblSubfamilias.TabIndex = 9;
            lblSubfamilias.Text = "Familias incluidas";
            //
            // clbSubfamilias
            //
            clbSubfamilias.CheckOnClick = true;
            clbSubfamilias.FormattingEnabled = true;
            clbSubfamilias.IntegralHeight = false;
            clbSubfamilias.Location = new Point(270, 394);
            clbSubfamilias.Name = "clbSubfamilias";
            clbSubfamilias.Size = new Size(320, 108);
            clbSubfamilias.TabIndex = 10;
            clbSubfamilias.ItemCheck += clb_ItemCheck;
            //
            // lblArbol
            //
            lblArbol.AutoSize = true;
            lblArbol.Location = new Point(610, 52);
            lblArbol.Name = "lblArbol";
            lblArbol.Size = new Size(206, 20);
            lblArbol.TabIndex = 11;
            lblArbol.Text = "Árbol de permisos de la familia";
            //
            // tvwArbol
            //
            tvwArbol.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tvwArbol.Location = new Point(610, 76);
            tvwArbol.Name = "tvwArbol";
            tvwArbol.Size = new Size(376, 382);
            tvwArbol.TabIndex = 12;
            //
            // lblUsuariosFamilia
            //
            lblUsuariosFamilia.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblUsuariosFamilia.AutoSize = true;
            lblUsuariosFamilia.ForeColor = Color.DimGray;
            lblUsuariosFamilia.Location = new Point(610, 468);
            lblUsuariosFamilia.Name = "lblUsuariosFamilia";
            lblUsuariosFamilia.Size = new Size(0, 20);
            lblUsuariosFamilia.TabIndex = 13;
            //
            // btnGuardar
            //
            btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGuardar.Location = new Point(756, 508);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 34);
            btnGuardar.TabIndex = 14;
            btnGuardar.Text = "Aceptar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnDescartar
            //
            btnDescartar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDescartar.Location = new Point(876, 508);
            btnDescartar.Name = "btnDescartar";
            btnDescartar.Size = new Size(110, 34);
            btnDescartar.TabIndex = 15;
            btnDescartar.Text = "Cancelar";
            btnDescartar.UseVisualStyleBackColor = true;
            btnDescartar.Click += btnDescartar_Click;
            //
            // FrmFamiliasPatentes_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(998, 554);
            Controls.Add(btnDescartar);
            Controls.Add(btnGuardar);
            Controls.Add(lblUsuariosFamilia);
            Controls.Add(tvwArbol);
            Controls.Add(lblArbol);
            Controls.Add(clbSubfamilias);
            Controls.Add(lblSubfamilias);
            Controls.Add(clbPatentes);
            Controls.Add(lblPatentes);
            Controls.Add(txtNombreFamilia);
            Controls.Add(lblNombreFamilia);
            Controls.Add(btnEliminarFamilia);
            Controls.Add(btnNuevaFamilia);
            Controls.Add(lstFamilias);
            Controls.Add(lblListaFamilias);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(1016, 601);
            Name = "FrmFamiliasPatentes_60MN";
            Text = "Familias y patentes";
            Load += FrmFamiliasPatentes_60MN_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblListaFamilias;
        private ListBox lstFamilias;
        private Button btnNuevaFamilia;
        private Button btnEliminarFamilia;
        private Label lblNombreFamilia;
        private TextBox txtNombreFamilia;
        private Label lblPatentes;
        private CheckedListBox clbPatentes;
        private Label lblSubfamilias;
        private CheckedListBox clbSubfamilias;
        private Label lblArbol;
        private TreeView tvwArbol;
        private Label lblUsuariosFamilia;
        private Button btnGuardar;
        private Button btnDescartar;
    }
}
