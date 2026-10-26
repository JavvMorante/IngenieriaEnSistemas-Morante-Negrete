namespace VetCare
{
    partial class FrmVisorXmlBitacora_60MN
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
            DataGridViewCellStyle estiloFecha = new DataGridViewCellStyle();
            lblArchivo = new Label();
            lblEstado = new Label();
            lblDetalle = new Label();
            lstErrores = new ListBox();
            tabContenido = new TabControl();
            tpXml = new TabPage();
            txtXml = new TextBox();
            tpEventos = new TabPage();
            dgvEventos = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colUsuario = new DataGridViewTextBoxColumn();
            colModulo = new DataGridViewTextBoxColumn();
            colEvento = new DataGridViewTextBoxColumn();
            colCriticidad = new DataGridViewTextBoxColumn();
            colDescripcion = new DataGridViewTextBoxColumn();
            btnImportar = new Button();
            btnCerrar = new Button();
            tabContenido.SuspendLayout();
            tpXml.SuspendLayout();
            tpEventos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEventos).BeginInit();
            SuspendLayout();
            //
            // lblArchivo
            //
            lblArchivo.AutoEllipsis = true;
            lblArchivo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblArchivo.Location = new Point(12, 10);
            lblArchivo.Name = "lblArchivo";
            lblArchivo.Size = new Size(976, 30);
            lblArchivo.TabIndex = 0;
            //
            // lblEstado
            //
            lblEstado.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEstado.Location = new Point(12, 44);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(976, 26);
            lblEstado.TabIndex = 1;
            //
            // lblDetalle
            //
            lblDetalle.Location = new Point(12, 74);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(976, 70);
            lblDetalle.TabIndex = 2;
            //
            // lstErrores
            //
            lstErrores.ForeColor = Color.Firebrick;
            lstErrores.FormattingEnabled = true;
            lstErrores.HorizontalScrollbar = true;
            lstErrores.Location = new Point(12, 148);
            lstErrores.Name = "lstErrores";
            lstErrores.Size = new Size(976, 104);
            lstErrores.TabIndex = 3;
            //
            // tabContenido
            //
            tabContenido.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabContenido.Controls.Add(tpXml);
            tabContenido.Controls.Add(tpEventos);
            tabContenido.Location = new Point(12, 260);
            tabContenido.Name = "tabContenido";
            tabContenido.SelectedIndex = 0;
            tabContenido.Size = new Size(976, 370);
            tabContenido.TabIndex = 4;
            //
            // tpXml
            //
            tpXml.Controls.Add(txtXml);
            tpXml.Location = new Point(4, 29);
            tpXml.Name = "tpXml";
            tpXml.Padding = new Padding(3);
            tpXml.Size = new Size(968, 337);
            tpXml.TabIndex = 0;
            tpXml.Text = "XML";
            tpXml.UseVisualStyleBackColor = true;
            //
            // txtXml
            //
            txtXml.BackColor = SystemColors.Window;
            txtXml.Dock = DockStyle.Fill;
            txtXml.Font = new Font("Consolas", 9.5F);
            txtXml.Location = new Point(3, 3);
            txtXml.MaxLength = 0;
            txtXml.Multiline = true;
            txtXml.Name = "txtXml";
            txtXml.ReadOnly = true;
            txtXml.ScrollBars = ScrollBars.Both;
            txtXml.Size = new Size(962, 331);
            txtXml.TabIndex = 0;
            txtXml.WordWrap = false;
            //
            // tpEventos
            //
            tpEventos.Controls.Add(dgvEventos);
            tpEventos.Location = new Point(4, 29);
            tpEventos.Name = "tpEventos";
            tpEventos.Padding = new Padding(3);
            tpEventos.Size = new Size(968, 337);
            tpEventos.TabIndex = 1;
            tpEventos.Text = "Eventos del archivo";
            tpEventos.UseVisualStyleBackColor = true;
            //
            // dgvEventos
            //
            dgvEventos.AllowUserToAddRows = false;
            dgvEventos.AllowUserToDeleteRows = false;
            dgvEventos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEventos.BackgroundColor = SystemColors.Window;
            dgvEventos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEventos.Columns.AddRange(new DataGridViewColumn[] { colFecha, colUsuario, colModulo, colEvento, colCriticidad, colDescripcion });
            dgvEventos.Dock = DockStyle.Fill;
            dgvEventos.Location = new Point(3, 3);
            dgvEventos.Name = "dgvEventos";
            dgvEventos.ReadOnly = true;
            dgvEventos.RowHeadersVisible = false;
            dgvEventos.RowHeadersWidth = 51;
            dgvEventos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEventos.Size = new Size(962, 331);
            dgvEventos.TabIndex = 0;
            //
            // colFecha
            //
            colFecha.DataPropertyName = "FechaHora";
            estiloFecha.Format = "dd/MM/yyyy HH:mm:ss";
            colFecha.DefaultCellStyle = estiloFecha;
            colFecha.FillWeight = 70F;
            colFecha.HeaderText = "Fecha y hora";
            colFecha.MinimumWidth = 6;
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colUsuario
            //
            colUsuario.DataPropertyName = "Usuario";
            colUsuario.FillWeight = 50F;
            colUsuario.HeaderText = "Usuario";
            colUsuario.MinimumWidth = 6;
            colUsuario.Name = "colUsuario";
            colUsuario.ReadOnly = true;
            //
            // colModulo
            //
            colModulo.DataPropertyName = "Modulo";
            colModulo.FillWeight = 50F;
            colModulo.HeaderText = "Módulo";
            colModulo.MinimumWidth = 6;
            colModulo.Name = "colModulo";
            colModulo.ReadOnly = true;
            //
            // colEvento
            //
            colEvento.DataPropertyName = "Evento";
            colEvento.FillWeight = 70F;
            colEvento.HeaderText = "Evento";
            colEvento.MinimumWidth = 6;
            colEvento.Name = "colEvento";
            colEvento.ReadOnly = true;
            //
            // colCriticidad
            //
            colCriticidad.DataPropertyName = "Criticidad";
            colCriticidad.FillWeight = 35F;
            colCriticidad.HeaderText = "Criticidad";
            colCriticidad.MinimumWidth = 6;
            colCriticidad.Name = "colCriticidad";
            colCriticidad.ReadOnly = true;
            //
            // colDescripcion
            //
            colDescripcion.DataPropertyName = "Descripcion";
            colDescripcion.FillWeight = 200F;
            colDescripcion.HeaderText = "Descripción";
            colDescripcion.MinimumWidth = 6;
            colDescripcion.Name = "colDescripcion";
            colDescripcion.ReadOnly = true;
            //
            // btnImportar
            //
            btnImportar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnImportar.Location = new Point(698, 638);
            btnImportar.Name = "btnImportar";
            btnImportar.Size = new Size(170, 36);
            btnImportar.TabIndex = 5;
            btnImportar.Text = "Importar eventos";
            btnImportar.UseVisualStyleBackColor = true;
            btnImportar.Click += btnImportar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(878, 638);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 36);
            btnCerrar.TabIndex = 6;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmVisorXmlBitacora_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1000, 686);
            Controls.Add(btnCerrar);
            Controls.Add(btnImportar);
            Controls.Add(tabContenido);
            Controls.Add(lstErrores);
            Controls.Add(lblDetalle);
            Controls.Add(lblEstado);
            Controls.Add(lblArchivo);
            MinimizeBox = false;
            MinimumSize = new Size(800, 560);
            Name = "FrmVisorXmlBitacora_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Bitácora en XML";
            Load += FrmVisorXmlBitacora_60MN_Load;
            tabContenido.ResumeLayout(false);
            tpXml.ResumeLayout(false);
            tpXml.PerformLayout();
            tpEventos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEventos).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblArchivo;
        private Label lblEstado;
        private Label lblDetalle;
        private ListBox lstErrores;
        private TabControl tabContenido;
        private TabPage tpXml;
        private TextBox txtXml;
        private TabPage tpEventos;
        private DataGridView dgvEventos;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colUsuario;
        private DataGridViewTextBoxColumn colModulo;
        private DataGridViewTextBoxColumn colEvento;
        private DataGridViewTextBoxColumn colCriticidad;
        private DataGridViewTextBoxColumn colDescripcion;
        private Button btnImportar;
        private Button btnCerrar;
    }
}
