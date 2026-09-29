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
            mnuMaestros = new ToolStripMenuItem();
            mnuProductos = new ToolStripMenuItem();
            mnuMovimientosStock = new ToolStripMenuItem();
            mnuClientes = new ToolStripMenuItem();
            mnuProveedores = new ToolStripMenuItem();
            mnuBitacoraCambios = new ToolStripMenuItem();
            mnuUsuario = new ToolStripMenuItem();
            mnuReLogin = new ToolStripMenuItem();
            mnuCambiarClave = new ToolStripMenuItem();
            mnuLogout = new ToolStripMenuItem();
            mnuCambiarIdioma = new ToolStripMenuItem();
            mnuVentas = new ToolStripMenuItem();
            mnuCarrito = new ToolStripMenuItem();
            mnuFacturar = new ToolStripMenuItem();
            mnuDespachar = new ToolStripMenuItem();
            mnuCompras = new ToolStripMenuItem();
            mnuGestionCompras = new ToolStripMenuItem();
            mnuReportes = new ToolStripMenuItem();
            mnuReporte1 = new ToolStripMenuItem();
            mnuReporte2 = new ToolStripMenuItem();
            mnuReporte3 = new ToolStripMenuItem();
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
            menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuAdmin, mnuMaestros, mnuUsuario, mnuVentas, mnuCompras, mnuReportes, mnuAyuda });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Padding = new Padding(8, 4, 0, 4);
            menuPrincipal.Size = new Size(1182, 39);
            menuPrincipal.TabIndex = 0;
            menuPrincipal.Text = "menuPrincipal";
            //
            // mnuAdmin
            //
            mnuAdmin.DropDownItems.AddRange(new ToolStripItem[] { mnuUsuarios, mnuPerfiles, mnuBackup, mnuRestore, mnuBitacoraEventos, mnuDigitosVerificadores });
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
            mnuBackup.Click += mnuPendiente_Click;
            //
            // mnuRestore
            //
            mnuRestore.Font = new Font("Segoe UI", 10F);
            mnuRestore.Name = "mnuRestore";
            mnuRestore.Size = new Size(260, 28);
            mnuRestore.Tag = "SEG_RESTORE";
            mnuRestore.Text = "Restore";
            mnuRestore.Click += mnuPendiente_Click;
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
            mnuMaestros.DropDownItems.AddRange(new ToolStripItem[] { mnuProductos, mnuMovimientosStock, mnuClientes, mnuProveedores, mnuBitacoraCambios });
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
            mnuProductos.Tag = "STK_PROD_ALTA|STK_PROD_MODIF|STK_MOVIMIENTO";
            mnuProductos.Text = "Productos";
            mnuProductos.Click += mnuProductos_Click;
            //
            // mnuMovimientosStock
            //
            mnuMovimientosStock.Font = new Font("Segoe UI", 10F);
            mnuMovimientosStock.Name = "mnuMovimientosStock";
            mnuMovimientosStock.Size = new Size(260, 28);
            mnuMovimientosStock.Tag = "STK_MOVIMIENTO";
            mnuMovimientosStock.Text = "Movimientos de stock";
            mnuMovimientosStock.Click += mnuMovimientosStock_Click;
            //
            // mnuClientes
            //
            mnuClientes.Font = new Font("Segoe UI", 10F);
            mnuClientes.Name = "mnuClientes";
            mnuClientes.Size = new Size(260, 28);
            mnuClientes.Tag = "MAE_CLIENTES";
            mnuClientes.Text = "Clientes";
            mnuClientes.Click += mnuPendiente_Click;
            //
            // mnuProveedores
            //
            mnuProveedores.Font = new Font("Segoe UI", 10F);
            mnuProveedores.Name = "mnuProveedores";
            mnuProveedores.Size = new Size(260, 28);
            mnuProveedores.Tag = "MAE_PROVEEDORES";
            mnuProveedores.Text = "Proveedores";
            mnuProveedores.Click += mnuPendiente_Click;
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
            mnuVentas.DropDownItems.AddRange(new ToolStripItem[] { mnuCarrito, mnuFacturar, mnuDespachar });
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
            mnuCarrito.Click += mnuPendiente_Click;
            //
            // mnuFacturar
            //
            mnuFacturar.Font = new Font("Segoe UI", 10F);
            mnuFacturar.Name = "mnuFacturar";
            mnuFacturar.Size = new Size(260, 28);
            mnuFacturar.Tag = "VEN_FACTURAR";
            mnuFacturar.Text = "Facturar";
            mnuFacturar.Click += mnuPendiente_Click;
            //
            // mnuDespachar
            //
            mnuDespachar.Font = new Font("Segoe UI", 10F);
            mnuDespachar.Name = "mnuDespachar";
            mnuDespachar.Size = new Size(260, 28);
            mnuDespachar.Tag = "VEN_DESPACHAR";
            mnuDespachar.Text = "Despachar";
            mnuDespachar.Click += mnuPendiente_Click;
            //
            // mnuCompras
            //
            mnuCompras.DropDownItems.AddRange(new ToolStripItem[] { mnuGestionCompras });
            mnuCompras.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mnuCompras.ForeColor = Color.White;
            mnuCompras.Name = "mnuCompras";
            mnuCompras.Padding = new Padding(14, 4, 14, 4);
            mnuCompras.Size = new Size(115, 31);
            mnuCompras.Text = "COMPRAS";
            //
            // mnuGestionCompras
            //
            mnuGestionCompras.Font = new Font("Segoe UI", 10F);
            mnuGestionCompras.Name = "mnuGestionCompras";
            mnuGestionCompras.Size = new Size(260, 28);
            mnuGestionCompras.Tag = "COM_COMPRAS";
            mnuGestionCompras.Text = "Compras";
            mnuGestionCompras.Click += mnuPendiente_Click;
            //
            // mnuReportes
            //
            mnuReportes.DropDownItems.AddRange(new ToolStripItem[] { mnuReporte1, mnuReporte2, mnuReporte3 });
            mnuReportes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mnuReportes.ForeColor = Color.White;
            mnuReportes.Name = "mnuReportes";
            mnuReportes.Padding = new Padding(14, 4, 14, 4);
            mnuReportes.Size = new Size(117, 31);
            mnuReportes.Text = "REPORTES";
            //
            // mnuReporte1
            //
            mnuReporte1.Font = new Font("Segoe UI", 10F);
            mnuReporte1.Name = "mnuReporte1";
            mnuReporte1.Size = new Size(260, 28);
            mnuReporte1.Tag = "REP_REPORTES";
            mnuReporte1.Text = "Reporte 1";
            mnuReporte1.Click += mnuPendiente_Click;
            //
            // mnuReporte2
            //
            mnuReporte2.Font = new Font("Segoe UI", 10F);
            mnuReporte2.Name = "mnuReporte2";
            mnuReporte2.Size = new Size(260, 28);
            mnuReporte2.Tag = "REP_REPORTES";
            mnuReporte2.Text = "Reporte 2";
            mnuReporte2.Click += mnuPendiente_Click;
            //
            // mnuReporte3
            //
            mnuReporte3.Font = new Font("Segoe UI", 10F);
            mnuReporte3.Name = "mnuReporte3";
            mnuReporte3.Size = new Size(260, 28);
            mnuReporte3.Tag = "REP_REPORTES";
            mnuReporte3.Text = "Reporte 3";
            mnuReporte3.Click += mnuPendiente_Click;
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
            mnuAyudaEnLinea.Click += mnuPendiente_Click;
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
        private ToolStripMenuItem mnuMaestros;
        private ToolStripMenuItem mnuProductos;
        private ToolStripMenuItem mnuMovimientosStock;
        private ToolStripMenuItem mnuClientes;
        private ToolStripMenuItem mnuProveedores;
        private ToolStripMenuItem mnuBitacoraCambios;
        private ToolStripMenuItem mnuUsuario;
        private ToolStripMenuItem mnuReLogin;
        private ToolStripMenuItem mnuCambiarClave;
        private ToolStripMenuItem mnuLogout;
        private ToolStripMenuItem mnuCambiarIdioma;
        private ToolStripMenuItem mnuVentas;
        private ToolStripMenuItem mnuCarrito;
        private ToolStripMenuItem mnuFacturar;
        private ToolStripMenuItem mnuDespachar;
        private ToolStripMenuItem mnuCompras;
        private ToolStripMenuItem mnuGestionCompras;
        private ToolStripMenuItem mnuReportes;
        private ToolStripMenuItem mnuReporte1;
        private ToolStripMenuItem mnuReporte2;
        private ToolStripMenuItem mnuReporte3;
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
