namespace VetCare
{
    partial class FrmOrdenCompra_60MN
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
            DataGridViewCellStyle estiloImporte = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloFecha = new DataGridViewCellStyle();
            lblTitulo = new Label();
            tabOrdenes = new TabControl();
            tpNueva = new TabPage();
            lblSolicitud = new Label();
            cboSolicitud = new ComboBox();
            dgvSolicitud = new DataGridView();
            colSolCodigo = new DataGridViewTextBoxColumn();
            colSolProducto = new DataGridViewTextBoxColumn();
            colSolPedido = new DataGridViewTextBoxColumn();
            colSolEnOrdenes = new DataGridViewTextBoxColumn();
            colSolAPedir = new DataGridViewTextBoxColumn();
            colSolCosto = new DataGridViewTextBoxColumn();
            lblCotizaciones = new Label();
            dgvCotizaciones = new DataGridView();
            colCotProveedor = new DataGridViewTextBoxColumn();
            colCotNumero = new DataGridViewTextBoxColumn();
            colCotTotal = new DataGridViewTextBoxColumn();
            colCotPlazo = new DataGridViewTextBoxColumn();
            colCotCondicion = new DataGridViewTextBoxColumn();
            colCotMasBarata = new DataGridViewCheckBoxColumn();
            btnCargarCotizacion = new Button();
            btnQuitarCotizacion = new Button();
            lblDetalle = new Label();
            dgvDetalle = new DataGridView();
            colDetProducto = new DataGridViewTextBoxColumn();
            colDetCantidad = new DataGridViewTextBoxColumn();
            colDetPrecio = new DataGridViewTextBoxColumn();
            colDetCosto = new DataGridViewTextBoxColumn();
            colDetSubtotal = new DataGridViewTextBoxColumn();
            lblMotivo = new Label();
            txtMotivo = new TextBox();
            btnGenerar = new Button();
            lblAviso = new Label();
            tpMisOrdenes = new TabPage();
            dgvMisOrdenes = new DataGridView();
            colOrdNumero = new DataGridViewTextBoxColumn();
            colOrdFecha = new DataGridViewTextBoxColumn();
            colOrdSolicitud = new DataGridViewTextBoxColumn();
            colOrdProveedor = new DataGridViewTextBoxColumn();
            colOrdTotal = new DataGridViewTextBoxColumn();
            colOrdEstado = new DataGridViewTextBoxColumn();
            colOrdMotivo = new DataGridViewTextBoxColumn();
            lblMotivoAnulacion = new Label();
            txtMotivoAnulacion = new TextBox();
            btnCorregir = new Button();
            btnAnular = new Button();
            tabOrdenes.SuspendLayout();
            tpNueva.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSolicitud).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCotizaciones).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            tpMisOrdenes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMisOrdenes).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(180, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Órdenes de compra";
            //
            // tabOrdenes
            //
            tabOrdenes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabOrdenes.Controls.Add(tpNueva);
            tabOrdenes.Controls.Add(tpMisOrdenes);
            tabOrdenes.Location = new Point(12, 48);
            tabOrdenes.Name = "tabOrdenes";
            tabOrdenes.SelectedIndex = 0;
            tabOrdenes.Size = new Size(1126, 648);
            tabOrdenes.TabIndex = 1;
            tabOrdenes.SelectedIndexChanged += tabOrdenes_SelectedIndexChanged;
            //
            // tpNueva
            //
            tpNueva.Controls.Add(lblAviso);
            tpNueva.Controls.Add(btnGenerar);
            tpNueva.Controls.Add(txtMotivo);
            tpNueva.Controls.Add(lblMotivo);
            tpNueva.Controls.Add(dgvDetalle);
            tpNueva.Controls.Add(lblDetalle);
            tpNueva.Controls.Add(btnQuitarCotizacion);
            tpNueva.Controls.Add(btnCargarCotizacion);
            tpNueva.Controls.Add(dgvCotizaciones);
            tpNueva.Controls.Add(lblCotizaciones);
            tpNueva.Controls.Add(dgvSolicitud);
            tpNueva.Controls.Add(cboSolicitud);
            tpNueva.Controls.Add(lblSolicitud);
            tpNueva.Location = new Point(4, 29);
            tpNueva.Name = "tpNueva";
            tpNueva.Padding = new Padding(3);
            tpNueva.Size = new Size(1118, 615);
            tpNueva.TabIndex = 0;
            tpNueva.Text = "Nueva orden";
            tpNueva.UseVisualStyleBackColor = true;
            //
            // lblSolicitud
            //
            lblSolicitud.AutoSize = true;
            lblSolicitud.Location = new Point(10, 16);
            lblSolicitud.Name = "lblSolicitud";
            lblSolicitud.Size = new Size(184, 20);
            lblSolicitud.TabIndex = 0;
            lblSolicitud.Text = "Solicitud de reposición *";
            //
            // cboSolicitud
            //
            cboSolicitud.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSolicitud.Location = new Point(204, 12);
            cboSolicitud.Name = "cboSolicitud";
            cboSolicitud.Size = new Size(460, 28);
            cboSolicitud.TabIndex = 1;
            cboSolicitud.SelectedIndexChanged += cboSolicitud_SelectedIndexChanged;
            //
            // dgvSolicitud
            //
            dgvSolicitud.AllowUserToAddRows = false;
            dgvSolicitud.AllowUserToDeleteRows = false;
            dgvSolicitud.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvSolicitud.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSolicitud.BackgroundColor = SystemColors.Window;
            dgvSolicitud.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSolicitud.Columns.AddRange(new DataGridViewColumn[] { colSolCodigo, colSolProducto, colSolPedido, colSolEnOrdenes, colSolAPedir, colSolCosto });
            dgvSolicitud.Location = new Point(10, 48);
            dgvSolicitud.MultiSelect = false;
            dgvSolicitud.Name = "dgvSolicitud";
            dgvSolicitud.ReadOnly = true;
            dgvSolicitud.RowHeadersVisible = false;
            dgvSolicitud.RowHeadersWidth = 51;
            dgvSolicitud.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSolicitud.Size = new Size(1096, 140);
            dgvSolicitud.TabIndex = 2;
            //
            // colSolCodigo
            //
            colSolCodigo.DataPropertyName = "Codigo";
            colSolCodigo.FillWeight = 60F;
            colSolCodigo.HeaderText = "Código";
            colSolCodigo.MinimumWidth = 6;
            colSolCodigo.Name = "colSolCodigo";
            colSolCodigo.ReadOnly = true;
            //
            // colSolProducto
            //
            colSolProducto.DataPropertyName = "Producto";
            colSolProducto.FillWeight = 200F;
            colSolProducto.HeaderText = "Producto";
            colSolProducto.MinimumWidth = 6;
            colSolProducto.Name = "colSolProducto";
            colSolProducto.ReadOnly = true;
            //
            // colSolPedido
            //
            colSolPedido.DataPropertyName = "CantidadPedida";
            estiloNumero.Alignment = DataGridViewContentAlignment.MiddleRight;
            colSolPedido.DefaultCellStyle = estiloNumero;
            colSolPedido.FillWeight = 60F;
            colSolPedido.HeaderText = "Solicitado";
            colSolPedido.MinimumWidth = 6;
            colSolPedido.Name = "colSolPedido";
            colSolPedido.ReadOnly = true;
            //
            // colSolEnOrdenes
            //
            colSolEnOrdenes.DataPropertyName = "CantidadEnOrdenes";
            colSolEnOrdenes.DefaultCellStyle = estiloNumero;
            colSolEnOrdenes.FillWeight = 60F;
            colSolEnOrdenes.HeaderText = "En órdenes";
            colSolEnOrdenes.MinimumWidth = 6;
            colSolEnOrdenes.Name = "colSolEnOrdenes";
            colSolEnOrdenes.ReadOnly = true;
            //
            // colSolAPedir
            //
            colSolAPedir.DataPropertyName = "PendienteDePedir";
            colSolAPedir.DefaultCellStyle = estiloNumero;
            colSolAPedir.FillWeight = 60F;
            colSolAPedir.HeaderText = "A pedir";
            colSolAPedir.MinimumWidth = 6;
            colSolAPedir.Name = "colSolAPedir";
            colSolAPedir.ReadOnly = true;
            //
            // colSolCosto
            //
            colSolCosto.DataPropertyName = "PrecioCostoActual";
            estiloImporte.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloImporte.Format = "N2";
            colSolCosto.DefaultCellStyle = estiloImporte;
            colSolCosto.FillWeight = 70F;
            colSolCosto.HeaderText = "Costo actual $";
            colSolCosto.MinimumWidth = 6;
            colSolCosto.Name = "colSolCosto";
            colSolCosto.ReadOnly = true;
            //
            // lblCotizaciones
            //
            lblCotizaciones.AutoSize = true;
            lblCotizaciones.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCotizaciones.Location = new Point(10, 198);
            lblCotizaciones.Name = "lblCotizaciones";
            lblCotizaciones.Size = new Size(173, 20);
            lblCotizaciones.TabIndex = 3;
            lblCotizaciones.Text = "Cotizaciones recibidas";
            //
            // dgvCotizaciones
            //
            dgvCotizaciones.AllowUserToAddRows = false;
            dgvCotizaciones.AllowUserToDeleteRows = false;
            dgvCotizaciones.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvCotizaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCotizaciones.BackgroundColor = SystemColors.Window;
            dgvCotizaciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCotizaciones.Columns.AddRange(new DataGridViewColumn[] { colCotProveedor, colCotNumero, colCotTotal, colCotPlazo, colCotCondicion, colCotMasBarata });
            dgvCotizaciones.Location = new Point(10, 222);
            dgvCotizaciones.MultiSelect = false;
            dgvCotizaciones.Name = "dgvCotizaciones";
            dgvCotizaciones.ReadOnly = true;
            dgvCotizaciones.RowHeadersVisible = false;
            dgvCotizaciones.RowHeadersWidth = 51;
            dgvCotizaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCotizaciones.Size = new Size(870, 140);
            dgvCotizaciones.TabIndex = 4;
            dgvCotizaciones.SelectionChanged += dgvCotizaciones_SelectionChanged;
            //
            // colCotProveedor
            //
            colCotProveedor.DataPropertyName = "Proveedor";
            colCotProveedor.FillWeight = 170F;
            colCotProveedor.HeaderText = "Proveedor";
            colCotProveedor.MinimumWidth = 6;
            colCotProveedor.Name = "colCotProveedor";
            colCotProveedor.ReadOnly = true;
            //
            // colCotNumero
            //
            colCotNumero.DataPropertyName = "NroCotizacion";
            colCotNumero.FillWeight = 80F;
            colCotNumero.HeaderText = "N° cotización";
            colCotNumero.MinimumWidth = 6;
            colCotNumero.Name = "colCotNumero";
            colCotNumero.ReadOnly = true;
            //
            // colCotTotal
            //
            colCotTotal.DataPropertyName = "Total";
            colCotTotal.DefaultCellStyle = estiloImporte;
            colCotTotal.FillWeight = 80F;
            colCotTotal.HeaderText = "Total $";
            colCotTotal.MinimumWidth = 6;
            colCotTotal.Name = "colCotTotal";
            colCotTotal.ReadOnly = true;
            //
            // colCotPlazo
            //
            colCotPlazo.DataPropertyName = "PlazoEntregaDias";
            colCotPlazo.DefaultCellStyle = estiloNumero;
            colCotPlazo.FillWeight = 55F;
            colCotPlazo.HeaderText = "Plazo (días)";
            colCotPlazo.MinimumWidth = 6;
            colCotPlazo.Name = "colCotPlazo";
            colCotPlazo.ReadOnly = true;
            //
            // colCotCondicion
            //
            colCotCondicion.DataPropertyName = "CondicionPago";
            colCotCondicion.FillWeight = 120F;
            colCotCondicion.HeaderText = "Condición de pago";
            colCotCondicion.MinimumWidth = 6;
            colCotCondicion.Name = "colCotCondicion";
            colCotCondicion.ReadOnly = true;
            //
            // colCotMasBarata
            //
            colCotMasBarata.DataPropertyName = "EsLaMasBarata";
            colCotMasBarata.FillWeight = 55F;
            colCotMasBarata.HeaderText = "Más barata";
            colCotMasBarata.MinimumWidth = 6;
            colCotMasBarata.Name = "colCotMasBarata";
            colCotMasBarata.ReadOnly = true;
            //
            // btnCargarCotizacion
            //
            btnCargarCotizacion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCargarCotizacion.Location = new Point(890, 222);
            btnCargarCotizacion.Name = "btnCargarCotizacion";
            btnCargarCotizacion.Size = new Size(216, 34);
            btnCargarCotizacion.TabIndex = 5;
            btnCargarCotizacion.Text = "Cargar cotización...";
            btnCargarCotizacion.UseVisualStyleBackColor = true;
            btnCargarCotizacion.Click += btnCargarCotizacion_Click;
            //
            // btnQuitarCotizacion
            //
            btnQuitarCotizacion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnQuitarCotizacion.Location = new Point(890, 262);
            btnQuitarCotizacion.Name = "btnQuitarCotizacion";
            btnQuitarCotizacion.Size = new Size(216, 34);
            btnQuitarCotizacion.TabIndex = 6;
            btnQuitarCotizacion.Text = "Quitar";
            btnQuitarCotizacion.UseVisualStyleBackColor = true;
            btnQuitarCotizacion.Click += btnQuitarCotizacion_Click;
            //
            // lblDetalle
            //
            lblDetalle.AutoSize = true;
            lblDetalle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDetalle.Location = new Point(10, 372);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(274, 20);
            lblDetalle.TabIndex = 7;
            lblDetalle.Text = "Detalle de la cotización seleccionada";
            //
            // dgvDetalle
            //
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.BackgroundColor = SystemColors.Window;
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalle.Columns.AddRange(new DataGridViewColumn[] { colDetProducto, colDetCantidad, colDetPrecio, colDetCosto, colDetSubtotal });
            dgvDetalle.Location = new Point(10, 396);
            dgvDetalle.MultiSelect = false;
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.ReadOnly = true;
            dgvDetalle.RowHeadersVisible = false;
            dgvDetalle.RowHeadersWidth = 51;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Size = new Size(1096, 112);
            dgvDetalle.TabIndex = 8;
            dgvDetalle.CellFormatting += dgvDetalle_CellFormatting;
            //
            // colDetProducto
            //
            colDetProducto.DataPropertyName = "Producto";
            colDetProducto.FillWeight = 220F;
            colDetProducto.HeaderText = "Producto";
            colDetProducto.MinimumWidth = 6;
            colDetProducto.Name = "colDetProducto";
            colDetProducto.ReadOnly = true;
            //
            // colDetCantidad
            //
            colDetCantidad.DataPropertyName = "Cantidad";
            colDetCantidad.DefaultCellStyle = estiloNumero;
            colDetCantidad.FillWeight = 60F;
            colDetCantidad.HeaderText = "Cantidad";
            colDetCantidad.MinimumWidth = 6;
            colDetCantidad.Name = "colDetCantidad";
            colDetCantidad.ReadOnly = true;
            //
            // colDetPrecio
            //
            colDetPrecio.DataPropertyName = "PrecioUnitario";
            colDetPrecio.DefaultCellStyle = estiloImporte;
            colDetPrecio.FillWeight = 75F;
            colDetPrecio.HeaderText = "Precio unitario $";
            colDetPrecio.MinimumWidth = 6;
            colDetPrecio.Name = "colDetPrecio";
            colDetPrecio.ReadOnly = true;
            //
            // colDetCosto
            //
            colDetCosto.DataPropertyName = "PrecioCostoActual";
            colDetCosto.DefaultCellStyle = estiloImporte;
            colDetCosto.FillWeight = 75F;
            colDetCosto.HeaderText = "Costo actual $";
            colDetCosto.MinimumWidth = 6;
            colDetCosto.Name = "colDetCosto";
            colDetCosto.ReadOnly = true;
            //
            // colDetSubtotal
            //
            colDetSubtotal.DataPropertyName = "Subtotal";
            colDetSubtotal.DefaultCellStyle = estiloImporte;
            colDetSubtotal.FillWeight = 75F;
            colDetSubtotal.HeaderText = "Subtotal $";
            colDetSubtotal.MinimumWidth = 6;
            colDetSubtotal.Name = "colDetSubtotal";
            colDetSubtotal.ReadOnly = true;
            //
            // lblMotivo
            //
            lblMotivo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblMotivo.AutoSize = true;
            lblMotivo.Location = new Point(10, 524);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.Size = new Size(140, 20);
            lblMotivo.TabIndex = 9;
            lblMotivo.Text = "Motivo de elección";
            //
            // txtMotivo
            //
            txtMotivo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtMotivo.Location = new Point(160, 520);
            txtMotivo.Name = "txtMotivo";
            txtMotivo.Size = new Size(720, 27);
            txtMotivo.TabIndex = 10;
            //
            // btnGenerar
            //
            btnGenerar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGenerar.Location = new Point(890, 516);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(216, 34);
            btnGenerar.TabIndex = 11;
            btnGenerar.Tag = "CMP_ORDEN";
            btnGenerar.Text = "Generar orden";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            //
            // lblAviso
            //
            lblAviso.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblAviso.ForeColor = Color.DarkRed;
            lblAviso.Location = new Point(10, 556);
            lblAviso.Name = "lblAviso";
            lblAviso.Size = new Size(1096, 50);
            lblAviso.TabIndex = 12;
            //
            // tpMisOrdenes
            //
            tpMisOrdenes.Controls.Add(btnAnular);
            tpMisOrdenes.Controls.Add(btnCorregir);
            tpMisOrdenes.Controls.Add(txtMotivoAnulacion);
            tpMisOrdenes.Controls.Add(lblMotivoAnulacion);
            tpMisOrdenes.Controls.Add(dgvMisOrdenes);
            tpMisOrdenes.Location = new Point(4, 29);
            tpMisOrdenes.Name = "tpMisOrdenes";
            tpMisOrdenes.Padding = new Padding(3);
            tpMisOrdenes.Size = new Size(1118, 615);
            tpMisOrdenes.TabIndex = 1;
            tpMisOrdenes.Text = "Mis órdenes";
            tpMisOrdenes.UseVisualStyleBackColor = true;
            //
            // dgvMisOrdenes
            //
            dgvMisOrdenes.AllowUserToAddRows = false;
            dgvMisOrdenes.AllowUserToDeleteRows = false;
            dgvMisOrdenes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMisOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMisOrdenes.BackgroundColor = SystemColors.Window;
            dgvMisOrdenes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMisOrdenes.Columns.AddRange(new DataGridViewColumn[] { colOrdNumero, colOrdFecha, colOrdSolicitud, colOrdProveedor, colOrdTotal, colOrdEstado, colOrdMotivo });
            dgvMisOrdenes.Location = new Point(10, 12);
            dgvMisOrdenes.MultiSelect = false;
            dgvMisOrdenes.Name = "dgvMisOrdenes";
            dgvMisOrdenes.ReadOnly = true;
            dgvMisOrdenes.RowHeadersVisible = false;
            dgvMisOrdenes.RowHeadersWidth = 51;
            dgvMisOrdenes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMisOrdenes.Size = new Size(1096, 536);
            dgvMisOrdenes.TabIndex = 0;
            dgvMisOrdenes.DataBindingComplete += dgvMisOrdenes_DataBindingComplete;
            //
            // colOrdNumero
            //
            colOrdNumero.DataPropertyName = "Numero";
            colOrdNumero.FillWeight = 60F;
            colOrdNumero.HeaderText = "Orden";
            colOrdNumero.MinimumWidth = 6;
            colOrdNumero.Name = "colOrdNumero";
            colOrdNumero.ReadOnly = true;
            //
            // colOrdFecha
            //
            colOrdFecha.DataPropertyName = "FechaHora";
            estiloFecha.Format = "dd/MM/yyyy HH:mm";
            colOrdFecha.DefaultCellStyle = estiloFecha;
            colOrdFecha.FillWeight = 75F;
            colOrdFecha.HeaderText = "Fecha";
            colOrdFecha.MinimumWidth = 6;
            colOrdFecha.Name = "colOrdFecha";
            colOrdFecha.ReadOnly = true;
            //
            // colOrdSolicitud
            //
            colOrdSolicitud.DataPropertyName = "SolicitudNumero";
            colOrdSolicitud.FillWeight = 60F;
            colOrdSolicitud.HeaderText = "Solicitud";
            colOrdSolicitud.MinimumWidth = 6;
            colOrdSolicitud.Name = "colOrdSolicitud";
            colOrdSolicitud.ReadOnly = true;
            //
            // colOrdProveedor
            //
            colOrdProveedor.DataPropertyName = "Proveedor";
            colOrdProveedor.FillWeight = 140F;
            colOrdProveedor.HeaderText = "Proveedor";
            colOrdProveedor.MinimumWidth = 6;
            colOrdProveedor.Name = "colOrdProveedor";
            colOrdProveedor.ReadOnly = true;
            //
            // colOrdTotal
            //
            colOrdTotal.DataPropertyName = "Total";
            colOrdTotal.DefaultCellStyle = estiloImporte;
            colOrdTotal.FillWeight = 70F;
            colOrdTotal.HeaderText = "Total $";
            colOrdTotal.MinimumWidth = 6;
            colOrdTotal.Name = "colOrdTotal";
            colOrdTotal.ReadOnly = true;
            //
            // colOrdEstado
            //
            colOrdEstado.DataPropertyName = "EstadoTexto";
            colOrdEstado.FillWeight = 85F;
            colOrdEstado.HeaderText = "Estado";
            colOrdEstado.MinimumWidth = 6;
            colOrdEstado.Name = "colOrdEstado";
            colOrdEstado.ReadOnly = true;
            //
            // colOrdMotivo
            //
            colOrdMotivo.DataPropertyName = "MotivoRechazo";
            colOrdMotivo.FillWeight = 180F;
            colOrdMotivo.HeaderText = "Motivo del rechazo o anulación";
            colOrdMotivo.MinimumWidth = 6;
            colOrdMotivo.Name = "colOrdMotivo";
            colOrdMotivo.ReadOnly = true;
            //
            // lblMotivoAnulacion
            //
            lblMotivoAnulacion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblMotivoAnulacion.AutoSize = true;
            lblMotivoAnulacion.Location = new Point(10, 568);
            lblMotivoAnulacion.Name = "lblMotivoAnulacion";
            lblMotivoAnulacion.Size = new Size(148, 20);
            lblMotivoAnulacion.TabIndex = 1;
            lblMotivoAnulacion.Text = "Motivo de anulación";
            //
            // txtMotivoAnulacion
            //
            txtMotivoAnulacion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtMotivoAnulacion.Location = new Point(166, 564);
            txtMotivoAnulacion.Name = "txtMotivoAnulacion";
            txtMotivoAnulacion.Size = new Size(520, 27);
            txtMotivoAnulacion.TabIndex = 2;
            //
            // btnCorregir
            //
            btnCorregir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCorregir.Location = new Point(696, 560);
            btnCorregir.Name = "btnCorregir";
            btnCorregir.Size = new Size(240, 34);
            btnCorregir.TabIndex = 3;
            btnCorregir.Tag = "CMP_ORDEN";
            btnCorregir.Text = "Corregir orden rechazada...";
            btnCorregir.UseVisualStyleBackColor = true;
            btnCorregir.Click += btnCorregir_Click;
            //
            // btnAnular
            //
            btnAnular.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAnular.Location = new Point(946, 560);
            btnAnular.Name = "btnAnular";
            btnAnular.Size = new Size(160, 34);
            btnAnular.TabIndex = 4;
            btnAnular.Tag = "CMP_ORDEN";
            btnAnular.Text = "Anular orden";
            btnAnular.UseVisualStyleBackColor = true;
            btnAnular.Click += btnAnular_Click;
            //
            // FrmOrdenCompra_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1150, 708);
            Controls.Add(tabOrdenes);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(1000, 700);
            Name = "FrmOrdenCompra_60MN";
            Text = "Órdenes de compra";
            Load += FrmOrdenCompra_60MN_Load;
            tabOrdenes.ResumeLayout(false);
            tpNueva.ResumeLayout(false);
            tpNueva.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSolicitud).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCotizaciones).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            tpMisOrdenes.ResumeLayout(false);
            tpMisOrdenes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMisOrdenes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private TabControl tabOrdenes;
        private TabPage tpNueva;
        private Label lblSolicitud;
        private ComboBox cboSolicitud;
        private DataGridView dgvSolicitud;
        private DataGridViewTextBoxColumn colSolCodigo;
        private DataGridViewTextBoxColumn colSolProducto;
        private DataGridViewTextBoxColumn colSolPedido;
        private DataGridViewTextBoxColumn colSolEnOrdenes;
        private DataGridViewTextBoxColumn colSolAPedir;
        private DataGridViewTextBoxColumn colSolCosto;
        private Label lblCotizaciones;
        private DataGridView dgvCotizaciones;
        private DataGridViewTextBoxColumn colCotProveedor;
        private DataGridViewTextBoxColumn colCotNumero;
        private DataGridViewTextBoxColumn colCotTotal;
        private DataGridViewTextBoxColumn colCotPlazo;
        private DataGridViewTextBoxColumn colCotCondicion;
        private DataGridViewCheckBoxColumn colCotMasBarata;
        private Button btnCargarCotizacion;
        private Button btnQuitarCotizacion;
        private Label lblDetalle;
        private DataGridView dgvDetalle;
        private DataGridViewTextBoxColumn colDetProducto;
        private DataGridViewTextBoxColumn colDetCantidad;
        private DataGridViewTextBoxColumn colDetPrecio;
        private DataGridViewTextBoxColumn colDetCosto;
        private DataGridViewTextBoxColumn colDetSubtotal;
        private Label lblMotivo;
        private TextBox txtMotivo;
        private Button btnGenerar;
        private Label lblAviso;
        private TabPage tpMisOrdenes;
        private DataGridView dgvMisOrdenes;
        private DataGridViewTextBoxColumn colOrdNumero;
        private DataGridViewTextBoxColumn colOrdFecha;
        private DataGridViewTextBoxColumn colOrdSolicitud;
        private DataGridViewTextBoxColumn colOrdProveedor;
        private DataGridViewTextBoxColumn colOrdTotal;
        private DataGridViewTextBoxColumn colOrdEstado;
        private DataGridViewTextBoxColumn colOrdMotivo;
        private Label lblMotivoAnulacion;
        private TextBox txtMotivoAnulacion;
        private Button btnCorregir;
        private Button btnAnular;
    }
}
