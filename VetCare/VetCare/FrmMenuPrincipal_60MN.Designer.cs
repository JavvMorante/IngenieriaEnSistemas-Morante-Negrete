namespace VetCare
{
    partial class FrmMenuPrincipal_60MN
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
            menuPrincipal = new MenuStrip();
            mnuAdmin = new ToolStripMenuItem();
            mnuUsuarios = new ToolStripMenuItem();
            mnuPerfiles = new ToolStripMenuItem();
            mnuBackup = new ToolStripMenuItem();
            mnuRestore = new ToolStripMenuItem();
            mnuBitacoraEventos = new ToolStripMenuItem();
            mnuDigitosVerificadores = new ToolStripMenuItem();
            mnuBitacoraCambios = new ToolStripMenuItem();
            mnuMaestros = new ToolStripMenuItem();
            mnuClientes = new ToolStripMenuItem();
            mnuMascotas = new ToolStripMenuItem();
            mnuProductos = new ToolStripMenuItem();
            mnuProveedores = new ToolStripMenuItem();
            mnuPacientes = new ToolStripMenuItem();
            mnuHistoriasClinicas = new ToolStripMenuItem();
            mnuRegistrarAtencion = new ToolStripMenuItem();
            mnuCompras = new ToolStripMenuItem();
            mnuSolicitudReposicion = new ToolStripMenuItem();
            mnuOrdenesCompra = new ToolStripMenuItem();
            mnuAprobarOrdenes = new ToolStripMenuItem();
            mnuPagosProveedores = new ToolStripMenuItem();
            mnuRecepcionMercaderia = new ToolStripMenuItem();
            mnuUsuario = new ToolStripMenuItem();
            mnuReLogin = new ToolStripMenuItem();
            mnuCambiarClave = new ToolStripMenuItem();
            mnuLogout = new ToolStripMenuItem();
            mnuCambiarIdioma = new ToolStripMenuItem();
            mnuVentas = new ToolStripMenuItem();
            mnuCarrito = new ToolStripMenuItem();
            mnuVentasRegistradas = new ToolStripMenuItem();
            mnuReportes = new ToolStripMenuItem();
            mnuReporteAtenciones = new ToolStripMenuItem();
            mnuReporteCompras = new ToolStripMenuItem();
            mnuReporte2 = new ToolStripMenuItem();
            mnuAyuda = new ToolStripMenuItem();
            mnuAyudaEnLinea = new ToolStripMenuItem();
            mnuAcercaDe = new ToolStripMenuItem();
            barraEstado = new StatusStrip();
            lblEstadoUsuario = new ToolStripStatusLabel();
            lblEstadoFamilias = new ToolStripStatusLabel();
            lblEstadoIdioma = new ToolStripStatusLabel();
            lblEstadoIntegridad = new ToolStripStatusLabel();
            lblEstadoFecha = new ToolStripStatusLabel();
            menuPrincipal.SuspendLayout();
            barraEstado.SuspendLayout();
            SuspendLayout();
            //
            // menuPrincipal
            //
            menuPrincipal.BackColor = Color.FromArgb(0, 102, 102);
            menuPrincipal.Font = new Font("Segoe UI", 10F);
            menuPrincipal.ImageScalingSize = new Size(20, 20);
            menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuAdmin, mnuMaestros, mnuPacientes, mnuVentas, mnuCompras, mnuReportes, mnuUsuario, mnuAyuda });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Padding = new Padding(8, 4, 0, 4);
            menuPrincipal.Size = new Size(1182, 39);
            menuPrincipal.TabIndex = 0;
            menuPrincipal.Text = "menuPrincipal";
            //
            // mnuAdmin
            //
            mnuAdmin.DropDownItems.AddRange(new ToolStripItem[] { mnuUsuarios, mnuPerfiles, mnuBackup, mnuRestore, mnuBitacoraEventos, mnuBitacoraCambios, mnuDigitosVerificadores });
            mnuAdmin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mnuAdmin.ForeColor = Color.White;
            mnuAdmin.Name = "mnuAdmin";
            mnuAdmin.Padding = new Padding(14, 4, 14, 4);
            mnuAdmin.Size = new Size(95, 31);
            mnuAdmin.Text = "ADMIN";
            //
            // mnuUsuarios
            //
            mnuUsuarios.Font = new Font("Segoe UI", 10F);
            mnuUsuarios.Name = "mnuUsuarios";
            mnuUsuarios.Size = new Size(260, 28);
            mnuUsuarios.Tag = "SEG_USU_ALTA|SEG_USU_MODIF|SEG_USU_BAJA|SEG_USU_DESBLOQ";
            mnuUsuarios.Text = "Usuarios";
            mnuUsuarios.Click += mnuUsuarios_Click;
            //
            // mnuPerfiles
            //
            mnuPerfiles.Font = new Font("Segoe UI", 10F);
            mnuPerfiles.Name = "mnuPerfiles";
            mnuPerfiles.Size = new Size(260, 28);
            mnuPerfiles.Tag = "SEG_ROLES";
            mnuPerfiles.Text = "Perfiles";
            mnuPerfiles.Click += mnuPerfiles_Click;
            //
            // mnuBackup
            //
            mnuBackup.Font = new Font("Segoe UI", 10F);
            mnuBackup.Name = "mnuBackup";
            mnuBackup.Size = new Size(260, 28);
            mnuBackup.Tag = "SEG_BACKUP";
            mnuBackup.Text = "Backup";
            mnuBackup.Click += mnuBackupRestore_Click;
            //
            // mnuRestore
            //
            mnuRestore.Font = new Font("Segoe UI", 10F);
            mnuRestore.Name = "mnuRestore";
            mnuRestore.Size = new Size(260, 28);
            mnuRestore.Tag = "SEG_RESTORE";
            mnuRestore.Text = "Restore";
            mnuRestore.Click += mnuBackupRestore_Click;
            //
            // mnuBitacoraEventos
            //
            mnuBitacoraEventos.Font = new Font("Segoe UI", 10F);
            mnuBitacoraEventos.Name = "mnuBitacoraEventos";
            mnuBitacoraEventos.Size = new Size(260, 28);
            mnuBitacoraEventos.Tag = "SEG_BITACORA_EVENTOS";
            mnuBitacoraEventos.Text = "Bitácora de eventos";
            mnuBitacoraEventos.Click += mnuBitacoraEventos_Click;
            //
            // mnuDigitosVerificadores
            //
            mnuDigitosVerificadores.Font = new Font("Segoe UI", 10F);
            mnuDigitosVerificadores.Name = "mnuDigitosVerificadores";
            mnuDigitosVerificadores.Size = new Size(260, 28);
            mnuDigitosVerificadores.Tag = "SEG_INTEGRIDAD";
            mnuDigitosVerificadores.Text = "Dígitos verificadores";
            mnuDigitosVerificadores.Click += mnuDigitosVerificadores_Click;
            //
            // mnuMaestros
            //
            mnuMaestros.DropDownItems.AddRange(new ToolStripItem[] { mnuClientes, mnuMascotas, mnuProductos, mnuProveedores });
            mnuMaestros.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mnuMaestros.ForeColor = Color.White;
            mnuMaestros.Name = "mnuMaestros";
            mnuMaestros.Padding = new Padding(14, 4, 14, 4);
            mnuMaestros.Size = new Size(123, 31);
            mnuMaestros.Text = "MAESTROS";
            //
            // mnuProductos
            //
            mnuProductos.Font = new Font("Segoe UI", 10F);
            mnuProductos.Name = "mnuProductos";
            mnuProductos.Size = new Size(260, 28);
            mnuProductos.Tag = "STK_PROD_ALTA|STK_PROD_MODIF";
            mnuProductos.Text = "Productos";
            mnuProductos.Click += mnuProductos_Click;
            //
            // mnuClientes
            //
            mnuClientes.Font = new Font("Segoe UI", 10F);
            mnuClientes.Name = "mnuClientes";
            mnuClientes.Size = new Size(260, 28);
            mnuClientes.Tag = "PAC_CLIENTE_CONSULTA|PAC_CLIENTE_ALTA|PAC_CLIENTE_MODIF";
            mnuClientes.Text = "Clientes";
            mnuClientes.Click += mnuClientes_Click;
            //
            // mnuMascotas
            //
            mnuMascotas.Font = new Font("Segoe UI", 10F);
            mnuMascotas.Name = "mnuMascotas";
            mnuMascotas.Size = new Size(260, 28);
            mnuMascotas.Tag = "PAC_MASCOTA";
            mnuMascotas.Text = "Mascotas";
            mnuMascotas.Click += mnuMascotas_Click;
            //
            // mnuProveedores
            //
            mnuProveedores.Font = new Font("Segoe UI", 10F);
            mnuProveedores.Name = "mnuProveedores";
            mnuProveedores.Size = new Size(260, 28);
            mnuProveedores.Tag = "MAE_PROVEEDORES";
            mnuProveedores.Text = "Proveedores";
            mnuProveedores.Click += mnuProveedores_Click;
            //
            // mnuBitacoraCambios
            //
            mnuBitacoraCambios.Font = new Font("Segoe UI", 10F);
            mnuBitacoraCambios.Name = "mnuBitacoraCambios";
            mnuBitacoraCambios.Size = new Size(260, 28);
            mnuBitacoraCambios.Tag = "SEG_BITACORA_CAMBIOS";
            mnuBitacoraCambios.Text = "Bitácora de cambios";
            mnuBitacoraCambios.Click += mnuBitacoraCambios_Click;
            //
            // mnuPacientes
            //
            mnuPacientes.DropDownItems.AddRange(new ToolStripItem[] { mnuHistoriasClinicas, mnuRegistrarAtencion });
            mnuPacientes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mnuPacientes.ForeColor = Color.White;
            mnuPacientes.Name = "mnuPacientes";
            mnuPacientes.Padding = new Padding(14, 4, 14, 4);
            mnuPacientes.Size = new Size(122, 31);
            mnuPacientes.Text = "PACIENTES";
            //
            // mnuHistoriasClinicas
            //
            mnuHistoriasClinicas.Font = new Font("Segoe UI", 10F);
            mnuHistoriasClinicas.Name = "mnuHistoriasClinicas";
            mnuHistoriasClinicas.Size = new Size(260, 28);
            mnuHistoriasClinicas.Tag = "HC_CONSULTA|HC_ALTA|HC_MODIF";
            mnuHistoriasClinicas.Text = "Historias clínicas";
            mnuHistoriasClinicas.Click += mnuHistoriasClinicas_Click;
            //
            // mnuRegistrarAtencion
            //
            mnuRegistrarAtencion.Font = new Font("Segoe UI", 10F);
            mnuRegistrarAtencion.Name = "mnuRegistrarAtencion";
            mnuRegistrarAtencion.Size = new Size(260, 28);
            mnuRegistrarAtencion.Tag = "ATN_REGISTRAR";
            mnuRegistrarAtencion.Text = "Registrar atención";
            mnuRegistrarAtencion.Click += mnuRegistrarAtencion_Click;
            //
            // mnuCompras
            //
            mnuCompras.DropDownItems.AddRange(new ToolStripItem[] { mnuSolicitudReposicion, mnuOrdenesCompra, mnuAprobarOrdenes, mnuPagosProveedores, mnuRecepcionMercaderia });
            mnuCompras.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mnuCompras.ForeColor = Color.White;
            mnuCompras.Name = "mnuCompras";
            mnuCompras.Padding = new Padding(14, 4, 14, 4);
            mnuCompras.Size = new Size(115, 31);
            mnuCompras.Text = "COMPRAS";
            //
            // mnuSolicitudReposicion
            //
            mnuSolicitudReposicion.Font = new Font("Segoe UI", 10F);
            mnuSolicitudReposicion.Name = "mnuSolicitudReposicion";
            mnuSolicitudReposicion.Size = new Size(260, 28);
            mnuSolicitudReposicion.Tag = "CMP_SOLICITUD";
            mnuSolicitudReposicion.Text = "Solicitud de reposición";
            mnuSolicitudReposicion.Click += mnuSolicitudReposicion_Click;
            //
            // mnuOrdenesCompra
            //
            mnuOrdenesCompra.Font = new Font("Segoe UI", 10F);
            mnuOrdenesCompra.Name = "mnuOrdenesCompra";
            mnuOrdenesCompra.Size = new Size(260, 28);
            mnuOrdenesCompra.Tag = "CMP_ORDEN";
            mnuOrdenesCompra.Text = "Órdenes de compra";
            mnuOrdenesCompra.Click += mnuOrdenesCompra_Click;
            //
            // mnuAprobarOrdenes
            //
            mnuAprobarOrdenes.Font = new Font("Segoe UI", 10F);
            mnuAprobarOrdenes.Name = "mnuAprobarOrdenes";
            mnuAprobarOrdenes.Size = new Size(260, 28);
            mnuAprobarOrdenes.Tag = "CMP_APROBAR";
            mnuAprobarOrdenes.Text = "Aprobar órdenes";
            mnuAprobarOrdenes.Click += mnuAprobarOrdenes_Click;
            //
            // mnuPagosProveedores
            //
            mnuPagosProveedores.Font = new Font("Segoe UI", 10F);
            mnuPagosProveedores.Name = "mnuPagosProveedores";
            mnuPagosProveedores.Size = new Size(260, 28);
            mnuPagosProveedores.Tag = "CMP_PAGO";
            mnuPagosProveedores.Text = "Pagos a proveedores";
            mnuPagosProveedores.Click += mnuPagosProveedores_Click;
            //
            // mnuRecepcionMercaderia
            //
            mnuRecepcionMercaderia.Font = new Font("Segoe UI", 10F);
            mnuRecepcionMercaderia.Name = "mnuRecepcionMercaderia";
            mnuRecepcionMercaderia.Size = new Size(260, 28);
            mnuRecepcionMercaderia.Tag = "CMP_RECEPCION";
            mnuRecepcionMercaderia.Text = "Recepción de mercadería";
            mnuRecepcionMercaderia.Click += mnuRecepcionMercaderia_Click;
            //
            // mnuUsuario
            //
            mnuUsuario.DropDownItems.AddRange(new ToolStripItem[] { mnuReLogin, mnuCambiarClave, mnuLogout, mnuCambiarIdioma });
            mnuUsuario.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mnuUsuario.ForeColor = Color.White;
            mnuUsuario.Name = "mnuUsuario";
            mnuUsuario.Padding = new Padding(14, 4, 14, 4);
            mnuUsuario.Size = new Size(110, 31);
            mnuUsuario.Text = "USUARIO";
            //
            // mnuReLogin
            //
            mnuReLogin.Font = new Font("Segoe UI", 10F);
            mnuReLogin.Name = "mnuReLogin";
            mnuReLogin.Size = new Size(260, 28);
            mnuReLogin.Text = "Re-Login";
            mnuReLogin.Click += mnuReLogin_Click;
            //
            // mnuCambiarClave
            //
            mnuCambiarClave.Font = new Font("Segoe UI", 10F);
            mnuCambiarClave.Name = "mnuCambiarClave";
            mnuCambiarClave.Size = new Size(260, 28);
            mnuCambiarClave.Text = "Cambiar clave";
            mnuCambiarClave.Click += mnuCambiarClave_Click;
            //
            // mnuLogout
            //
            mnuLogout.Font = new Font("Segoe UI", 10F);
            mnuLogout.Name = "mnuLogout";
            mnuLogout.Size = new Size(260, 28);
            mnuLogout.Text = "Logout";
            mnuLogout.Click += mnuLogout_Click;
            //
            // mnuCambiarIdioma
            //
            mnuCambiarIdioma.Font = new Font("Segoe UI", 10F);
            mnuCambiarIdioma.Name = "mnuCambiarIdioma";
            mnuCambiarIdioma.Size = new Size(260, 28);
            mnuCambiarIdioma.Text = "Cambiar idioma";
            mnuCambiarIdioma.Click += mnuCambiarIdioma_Click;
            //
            // mnuVentas
            //
            mnuVentas.DropDownItems.AddRange(new ToolStripItem[] { mnuCarrito, mnuVentasRegistradas });
            mnuVentas.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mnuVentas.ForeColor = Color.White;
            mnuVentas.Name = "mnuVentas";
            mnuVentas.Padding = new Padding(14, 4, 14, 4);
            mnuVentas.Size = new Size(100, 31);
            mnuVentas.Text = "VENTAS";
            //
            // mnuCarrito
            //
            mnuCarrito.Font = new Font("Segoe UI", 10F);
            mnuCarrito.Name = "mnuCarrito";
            mnuCarrito.Size = new Size(260, 28);
            mnuCarrito.Tag = "VEN_CARRITO";
            mnuCarrito.Text = "Carrito";
            mnuCarrito.Click += mnuCarrito_Click;
            //
            // mnuVentasRegistradas
            //
            mnuVentasRegistradas.Font = new Font("Segoe UI", 10F);
            mnuVentasRegistradas.Name = "mnuVentasRegistradas";
            mnuVentasRegistradas.Size = new Size(260, 28);
            mnuVentasRegistradas.Tag = "VEN_CARRITO";
            mnuVentasRegistradas.Text = "Ventas registradas";
            mnuVentasRegistradas.Click += mnuVentasRegistradas_Click;
            //
            // mnuReportes
            //
            mnuReportes.DropDownItems.AddRange(new ToolStripItem[] { mnuReporteAtenciones, mnuReporteCompras, mnuReporte2 });
            mnuReportes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mnuReportes.ForeColor = Color.White;
            mnuReportes.Name = "mnuReportes";
            mnuReportes.Padding = new Padding(14, 4, 14, 4);
            mnuReportes.Size = new Size(117, 31);
            mnuReportes.Text = "REPORTES";
            //
            // mnuReporteAtenciones
            //
            mnuReporteAtenciones.Font = new Font("Segoe UI", 10F);
            mnuReporteAtenciones.Name = "mnuReporteAtenciones";
            mnuReporteAtenciones.Size = new Size(260, 28);
            mnuReporteAtenciones.Tag = "REP_REPORTES";
            mnuReporteAtenciones.Text = "Reporte de atenciones";
            mnuReporteAtenciones.Click += mnuReporteAtenciones_Click;
            //
            // mnuReporteCompras
            //
            mnuReporteCompras.Font = new Font("Segoe UI", 10F);
            mnuReporteCompras.Name = "mnuReporteCompras";
            mnuReporteCompras.Size = new Size(260, 28);
            mnuReporteCompras.Tag = "REP_COMPRAS";
            mnuReporteCompras.Text = "Reporte de compras";
            mnuReporteCompras.Click += mnuReporteCompras_Click;
            //
            // mnuReporte2
            //
            mnuReporte2.Font = new Font("Segoe UI", 10F);
            mnuReporte2.Name = "mnuReporte2";
            mnuReporte2.Size = new Size(260, 28);
            mnuReporte2.Tag = "REP_REPORTES";
            mnuReporte2.Text = "Reporte inteligente";
            mnuReporte2.Click += mnuReporte2_Click;
            //
            // mnuAyuda
            //
            mnuAyuda.DropDownItems.AddRange(new ToolStripItem[] { mnuAyudaEnLinea, mnuAcercaDe });
            mnuAyuda.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mnuAyuda.ForeColor = Color.White;
            mnuAyuda.Name = "mnuAyuda";
            mnuAyuda.Padding = new Padding(14, 4, 14, 4);
            mnuAyuda.Size = new Size(95, 31);
            mnuAyuda.Text = "AYUDA";
            //
            // mnuAyudaEnLinea
            //
            mnuAyudaEnLinea.Font = new Font("Segoe UI", 10F);
            mnuAyudaEnLinea.Name = "mnuAyudaEnLinea";
            mnuAyudaEnLinea.Size = new Size(260, 28);
            mnuAyudaEnLinea.Text = "Ayuda en línea (F1)";
            mnuAyudaEnLinea.Click += mnuAyudaEnLinea_Click;
            //
            // mnuAcercaDe
            //
            mnuAcercaDe.Font = new Font("Segoe UI", 10F);
            mnuAcercaDe.Name = "mnuAcercaDe";
            mnuAcercaDe.Size = new Size(260, 28);
            mnuAcercaDe.Text = "Acerca de VetCare";
            mnuAcercaDe.Click += mnuAcercaDe_Click;
            //
            // barraEstado
            //
            barraEstado.ImageScalingSize = new Size(20, 20);
            barraEstado.Items.AddRange(new ToolStripItem[] { lblEstadoUsuario, lblEstadoFamilias, lblEstadoIdioma, lblEstadoIntegridad, lblEstadoFecha });
            barraEstado.Location = new Point(0, 641);
            barraEstado.Name = "barraEstado";
            barraEstado.Size = new Size(1182, 26);
            barraEstado.TabIndex = 1;
            barraEstado.Text = "barraEstado";
            //
            // lblEstadoUsuario
            //
            lblEstadoUsuario.Name = "lblEstadoUsuario";
            lblEstadoUsuario.Padding = new Padding(0, 0, 12, 0);
            lblEstadoUsuario.Size = new Size(74, 20);
            lblEstadoUsuario.Text = "Usuario:";
            //
            // lblEstadoFamilias
            //
            lblEstadoFamilias.Name = "lblEstadoFamilias";
            lblEstadoFamilias.Padding = new Padding(0, 0, 12, 0);
            lblEstadoFamilias.Size = new Size(46, 20);
            lblEstadoFamilias.Text = "Familias:";
            //
            // lblEstadoIdioma
            //
            lblEstadoIdioma.Name = "lblEstadoIdioma";
            lblEstadoIdioma.Padding = new Padding(0, 0, 12, 0);
            lblEstadoIdioma.Size = new Size(70, 20);
            lblEstadoIdioma.Text = "Idioma:";
            //
            // lblEstadoIntegridad
            //
            lblEstadoIntegridad.ForeColor = Color.Firebrick;
            lblEstadoIntegridad.Name = "lblEstadoIntegridad";
            lblEstadoIntegridad.Size = new Size(0, 20);
            //
            // lblEstadoFecha
            //
            lblEstadoFecha.Name = "lblEstadoFecha";
            lblEstadoFecha.Size = new Size(977, 20);
            lblEstadoFecha.Spring = true;
            lblEstadoFecha.TextAlign = ContentAlignment.MiddleRight;
            //
            // FrmMenuPrincipal_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 240, 240);
            ClientSize = new Size(1182, 667);
            Controls.Add(barraEstado);
            Controls.Add(menuPrincipal);
            IsMdiContainer = true;
            MainMenuStrip = menuPrincipal;
            MinimumSize = new Size(900, 600);
            Name = "FrmMenuPrincipal_60MN";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "VetCare - Sistema de gestión veterinaria";
            WindowState = FormWindowState.Maximized;
            FormClosing += FrmMenuPrincipal_60MN_FormClosing;
            Load += FrmMenuPrincipal_60MN_Load;
            menuPrincipal.ResumeLayout(false);
            menuPrincipal.PerformLayout();
            barraEstado.ResumeLayout(false);
            barraEstado.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuPrincipal;
        private ToolStripMenuItem mnuAdmin;
        private ToolStripMenuItem mnuUsuarios;
        private ToolStripMenuItem mnuPerfiles;
        private ToolStripMenuItem mnuBackup;
        private ToolStripMenuItem mnuRestore;
        private ToolStripMenuItem mnuBitacoraEventos;
        private ToolStripMenuItem mnuDigitosVerificadores;
        private ToolStripMenuItem mnuBitacoraCambios;
        private ToolStripMenuItem mnuMaestros;
        private ToolStripMenuItem mnuClientes;
        private ToolStripMenuItem mnuMascotas;
        private ToolStripMenuItem mnuProductos;
        private ToolStripMenuItem mnuProveedores;
        private ToolStripMenuItem mnuPacientes;
        private ToolStripMenuItem mnuHistoriasClinicas;
        private ToolStripMenuItem mnuRegistrarAtencion;
        private ToolStripMenuItem mnuCompras;
        private ToolStripMenuItem mnuSolicitudReposicion;
        private ToolStripMenuItem mnuOrdenesCompra;
        private ToolStripMenuItem mnuAprobarOrdenes;
        private ToolStripMenuItem mnuPagosProveedores;
        private ToolStripMenuItem mnuRecepcionMercaderia;
        private ToolStripMenuItem mnuUsuario;
        private ToolStripMenuItem mnuReLogin;
        private ToolStripMenuItem mnuCambiarClave;
        private ToolStripMenuItem mnuLogout;
        private ToolStripMenuItem mnuCambiarIdioma;
        private ToolStripMenuItem mnuVentas;
        private ToolStripMenuItem mnuCarrito;
        private ToolStripMenuItem mnuVentasRegistradas;
        private ToolStripMenuItem mnuReportes;
        private ToolStripMenuItem mnuReporteAtenciones;
        private ToolStripMenuItem mnuReporteCompras;
        private ToolStripMenuItem mnuReporte2;
        private ToolStripMenuItem mnuAyuda;
        private ToolStripMenuItem mnuAyudaEnLinea;
        private ToolStripMenuItem mnuAcercaDe;
        private StatusStrip barraEstado;
        private ToolStripStatusLabel lblEstadoUsuario;
        private ToolStripStatusLabel lblEstadoFamilias;
        private ToolStripStatusLabel lblEstadoIdioma;
        private ToolStripStatusLabel lblEstadoIntegridad;
        private ToolStripStatusLabel lblEstadoFecha;
    }
}
