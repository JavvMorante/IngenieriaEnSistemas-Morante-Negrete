namespace VetCare
{
    partial class FrmRecepcion_60MN
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
            DataGridViewCellStyle estiloNumero = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloEditable = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloEditableNumero = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloEditableFecha = new DataGridViewCellStyle();
            lblTitulo = new Label();
            lblOrden = new Label();
            txtOrden = new TextBox();
            btnBuscarOrden = new Button();
            lblDatosOrden = new Label();
            lblRemito = new Label();
            txtRemito = new TextBox();
            dgvDetalle = new DataGridView();
            colProducto = new DataGridViewTextBoxColumn();
            colPedido = new DataGridViewTextBoxColumn();
            colPendiente = new DataGridViewTextBoxColumn();
            colRecibido = new DataGridViewTextBoxColumn();
            colLote = new DataGridViewTextBoxColumn();
            colVencimiento = new DataGridViewTextBoxColumn();
            colObservacion = new DataGridViewTextBoxColumn();
            lblObservaciones = new Label();
            txtObservaciones = new TextBox();
            lblEstado = new Label();
            btnConfirmar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(244, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Recepción de mercadería";
            //
            // lblOrden
            //
            lblOrden.AutoSize = true;
            lblOrden.Location = new Point(12, 58);
            lblOrden.Name = "lblOrden";
            lblOrden.Size = new Size(98, 20);
            lblOrden.TabIndex = 1;
            lblOrden.Text = "N° de orden *";
            //
            // txtOrden
            //
            txtOrden.CharacterCasing = CharacterCasing.Upper;
            txtOrden.Location = new Point(130, 54);
            txtOrden.MaxLength = 20;
            txtOrden.Name = "txtOrden";
            txtOrden.Size = new Size(160, 27);
            txtOrden.TabIndex = 2;
            //
            // btnBuscarOrden
            //
            btnBuscarOrden.Location = new Point(298, 51);
            btnBuscarOrden.Name = "btnBuscarOrden";
            btnBuscarOrden.Size = new Size(110, 34);
            btnBuscarOrden.TabIndex = 3;
            btnBuscarOrden.Text = "Buscar";
            btnBuscarOrden.UseVisualStyleBackColor = true;
            btnBuscarOrden.Click += btnBuscarOrden_Click;
            //
            // lblDatosOrden
            //
            lblDatosOrden.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblDatosOrden.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDatosOrden.Location = new Point(420, 58);
            lblDatosOrden.Name = "lblDatosOrden";
            lblDatosOrden.Size = new Size(668, 24);
            lblDatosOrden.TabIndex = 4;
            //
            // lblRemito
            //
            lblRemito.AutoSize = true;
            lblRemito.Location = new Point(12, 98);
            lblRemito.Name = "lblRemito";
            lblRemito.Size = new Size(106, 20);
            lblRemito.TabIndex = 5;
            lblRemito.Text = "N° de remito *";
            //
            // txtRemito
            //
            txtRemito.CharacterCasing = CharacterCasing.Upper;
            txtRemito.Location = new Point(130, 94);
            txtRemito.Name = "txtRemito";
            txtRemito.Size = new Size(220, 27);
            txtRemito.TabIndex = 6;
            //
            // dgvDetalle
            //
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.BackgroundColor = SystemColors.Window;
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalle.Columns.AddRange(new DataGridViewColumn[] { colProducto, colPedido, colPendiente, colRecibido, colLote, colVencimiento, colObservacion });
            dgvDetalle.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvDetalle.Location = new Point(12, 134);
            dgvDetalle.MultiSelect = false;
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.RowHeadersVisible = false;
            dgvDetalle.RowHeadersWidth = 51;
            dgvDetalle.Size = new Size(1076, 300);
            dgvDetalle.TabIndex = 7;
            dgvDetalle.CellParsing += dgvDetalle_CellParsing;
            dgvDetalle.DataError += dgvDetalle_DataError;
            //
            // colProducto
            //
            colProducto.DataPropertyName = "Producto";
            colProducto.FillWeight = 190F;
            colProducto.HeaderText = "Producto";
            colProducto.MinimumWidth = 6;
            colProducto.Name = "colProducto";
            colProducto.ReadOnly = true;
            //
            // colPedido
            //
            colPedido.DataPropertyName = "Pedido";
            estiloNumero.Alignment = DataGridViewContentAlignment.MiddleRight;
            colPedido.DefaultCellStyle = estiloNumero;
            colPedido.FillWeight = 55F;
            colPedido.HeaderText = "Pedido";
            colPedido.MinimumWidth = 6;
            colPedido.Name = "colPedido";
            colPedido.ReadOnly = true;
            //
            // colPendiente
            //
            colPendiente.DataPropertyName = "Pendiente";
            colPendiente.DefaultCellStyle = estiloNumero;
            colPendiente.FillWeight = 60F;
            colPendiente.HeaderText = "Pendiente";
            colPendiente.MinimumWidth = 6;
            colPendiente.Name = "colPendiente";
            colPendiente.ReadOnly = true;
            //
            // colRecibido
            //
            colRecibido.DataPropertyName = "Recibido";
            estiloEditableNumero.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloEditableNumero.BackColor = Color.LightYellow;
            colRecibido.DefaultCellStyle = estiloEditableNumero;
            colRecibido.FillWeight = 60F;
            colRecibido.HeaderText = "Recibido";
            colRecibido.MinimumWidth = 6;
            colRecibido.Name = "colRecibido";
            //
            // colLote
            //
            colLote.DataPropertyName = "Lote";
            estiloEditable.BackColor = Color.LightYellow;
            colLote.DefaultCellStyle = estiloEditable;
            colLote.FillWeight = 75F;
            colLote.HeaderText = "Lote";
            colLote.MaxInputLength = 20;
            colLote.MinimumWidth = 6;
            colLote.Name = "colLote";
            //
            // colVencimiento
            //
            colVencimiento.DataPropertyName = "Vencimiento";
            estiloEditableFecha.BackColor = Color.LightYellow;
            estiloEditableFecha.Format = "dd/MM/yyyy";
            colVencimiento.DefaultCellStyle = estiloEditableFecha;
            colVencimiento.FillWeight = 75F;
            colVencimiento.HeaderText = "Vencimiento";
            colVencimiento.MaxInputLength = 10;
            colVencimiento.MinimumWidth = 6;
            colVencimiento.Name = "colVencimiento";
            //
            // colObservacion
            //
            colObservacion.DataPropertyName = "Observacion";
            colObservacion.DefaultCellStyle = estiloEditable;
            colObservacion.FillWeight = 150F;
            colObservacion.HeaderText = "Observación";
            colObservacion.MaxInputLength = 200;
            colObservacion.MinimumWidth = 6;
            colObservacion.Name = "colObservacion";
            //
            // lblObservaciones
            //
            lblObservaciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblObservaciones.AutoSize = true;
            lblObservaciones.Location = new Point(12, 450);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(105, 20);
            lblObservaciones.TabIndex = 8;
            lblObservaciones.Text = "Observaciones";
            //
            // txtObservaciones
            //
            txtObservaciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtObservaciones.Location = new Point(130, 446);
            txtObservaciones.MaxLength = 400;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Size = new Size(958, 27);
            txtObservaciones.TabIndex = 9;
            //
            // lblEstado
            //
            lblEstado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEstado.ForeColor = Color.DarkGreen;
            lblEstado.Location = new Point(12, 488);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(840, 48);
            lblEstado.TabIndex = 10;
            //
            // btnConfirmar
            //
            btnConfirmar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnConfirmar.Location = new Point(868, 488);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(220, 34);
            btnConfirmar.TabIndex = 11;
            btnConfirmar.Tag = "CMP_RECEPCION";
            btnConfirmar.Text = "Confirmar recepción";
            btnConfirmar.UseVisualStyleBackColor = true;
            btnConfirmar.Click += btnConfirmar_Click;
            //
            // FrmRecepcion_60MN
            //
            AcceptButton = btnBuscarOrden;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 548);
            Controls.Add(btnConfirmar);
            Controls.Add(lblEstado);
            Controls.Add(txtObservaciones);
            Controls.Add(lblObservaciones);
            Controls.Add(dgvDetalle);
            Controls.Add(txtRemito);
            Controls.Add(lblRemito);
            Controls.Add(lblDatosOrden);
            Controls.Add(btnBuscarOrden);
            Controls.Add(txtOrden);
            Controls.Add(lblOrden);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(960, 540);
            Name = "FrmRecepcion_60MN";
            Text = "Recepción de mercadería";
            Load += FrmRecepcion_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblOrden;
        private TextBox txtOrden;
        private Button btnBuscarOrden;
        private Label lblDatosOrden;
        private Label lblRemito;
        private TextBox txtRemito;
        private DataGridView dgvDetalle;
        private DataGridViewTextBoxColumn colProducto;
        private DataGridViewTextBoxColumn colPedido;
        private DataGridViewTextBoxColumn colPendiente;
        private DataGridViewTextBoxColumn colRecibido;
        private DataGridViewTextBoxColumn colLote;
        private DataGridViewTextBoxColumn colVencimiento;
        private DataGridViewTextBoxColumn colObservacion;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private Label lblEstado;
        private Button btnConfirmar;
    }
}
