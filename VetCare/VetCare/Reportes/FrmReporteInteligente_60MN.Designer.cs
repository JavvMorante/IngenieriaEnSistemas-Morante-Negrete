namespace VetCare
{
    partial class FrmReporteInteligente_60MN
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
            grpFiltros = new GroupBox();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            lblTop = new Label();
            numTop = new NumericUpDown();
            btnGenerar = new Button();
            pnlDemanda = new PanelGrafico_60MN();
            pnlTop = new PanelGrafico_60MN();
            lblResumen = new Label();
            btnImprimir = new Button();
            btnExportar = new Button();
            btnCerrar = new Button();
            grpFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTop).BeginInit();
            SuspendLayout();
            //
            // grpFiltros
            //
            grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpFiltros.Controls.Add(lblDesde);
            grpFiltros.Controls.Add(dtpDesde);
            grpFiltros.Controls.Add(lblHasta);
            grpFiltros.Controls.Add(dtpHasta);
            grpFiltros.Controls.Add(lblTop);
            grpFiltros.Controls.Add(numTop);
            grpFiltros.Controls.Add(btnGenerar);
            grpFiltros.Location = new Point(12, 8);
            grpFiltros.Name = "grpFiltros";
            grpFiltros.Size = new Size(1076, 72);
            grpFiltros.TabIndex = 0;
            grpFiltros.TabStop = false;
            grpFiltros.Text = "Período";
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(14, 32);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(50, 20);
            lblDesde.TabIndex = 0;
            lblDesde.Text = "Desde";
            //
            // dtpDesde
            //
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(76, 28);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(130, 27);
            dtpDesde.TabIndex = 1;
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(224, 32);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(46, 20);
            lblHasta.TabIndex = 2;
            lblHasta.Text = "hasta";
            //
            // dtpHasta
            //
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(280, 28);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(130, 27);
            dtpHasta.TabIndex = 3;
            //
            // lblTop
            //
            lblTop.AutoSize = true;
            lblTop.Location = new Point(436, 32);
            lblTop.Name = "lblTop";
            lblTop.Size = new Size(139, 20);
            lblTop.TabIndex = 4;
            lblTop.Text = "Productos del ranking";
            //
            // numTop
            //
            numTop.Location = new Point(590, 28);
            numTop.Name = "numTop";
            numTop.Size = new Size(70, 27);
            numTop.TabIndex = 5;
            numTop.TextAlign = HorizontalAlignment.Center;
            //
            // btnGenerar
            //
            btnGenerar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerar.Location = new Point(920, 26);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(140, 34);
            btnGenerar.TabIndex = 6;
            btnGenerar.Text = "Generar";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            //
            // pnlDemanda
            //
            pnlDemanda.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlDemanda.Location = new Point(12, 88);
            pnlDemanda.Name = "pnlDemanda";
            pnlDemanda.Size = new Size(1076, 270);
            pnlDemanda.TabIndex = 1;
            pnlDemanda.Paint += pnlDemanda_Paint;
            pnlDemanda.Resize += Panel_Resize;
            //
            // pnlTop
            //
            pnlTop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlTop.Location = new Point(12, 366);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1076, 222);
            pnlTop.TabIndex = 2;
            pnlTop.Paint += pnlTop_Paint;
            pnlTop.Resize += Panel_Resize;
            //
            // lblResumen
            //
            lblResumen.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResumen.Location = new Point(12, 600);
            lblResumen.Name = "lblResumen";
            lblResumen.Size = new Size(600, 26);
            lblResumen.TabIndex = 3;
            //
            // btnImprimir
            //
            btnImprimir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnImprimir.Enabled = false;
            btnImprimir.Location = new Point(658, 596);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(150, 34);
            btnImprimir.TabIndex = 4;
            btnImprimir.Text = "Imprimir / PDF";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            //
            // btnExportar
            //
            btnExportar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExportar.Enabled = false;
            btnExportar.Location = new Point(818, 596);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(150, 34);
            btnExportar.TabIndex = 5;
            btnExportar.Text = "Exportar CSV";
            btnExportar.UseVisualStyleBackColor = true;
            btnExportar.Click += btnExportar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.Location = new Point(978, 596);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 34);
            btnCerrar.TabIndex = 6;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmReporteInteligente_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 640);
            Controls.Add(btnCerrar);
            Controls.Add(btnExportar);
            Controls.Add(btnImprimir);
            Controls.Add(lblResumen);
            Controls.Add(pnlTop);
            Controls.Add(pnlDemanda);
            Controls.Add(grpFiltros);
            MinimumSize = new Size(880, 560);
            Name = "FrmReporteInteligente_60MN";
            Text = "Reporte inteligente";
            Load += FrmReporteInteligente_60MN_Load;
            grpFiltros.ResumeLayout(false);
            grpFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTop).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpFiltros;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Label lblTop;
        private NumericUpDown numTop;
        private Button btnGenerar;
        private PanelGrafico_60MN pnlDemanda;
        private PanelGrafico_60MN pnlTop;
        private Label lblResumen;
        private Button btnImprimir;
        private Button btnExportar;
        private Button btnCerrar;
    }
}
