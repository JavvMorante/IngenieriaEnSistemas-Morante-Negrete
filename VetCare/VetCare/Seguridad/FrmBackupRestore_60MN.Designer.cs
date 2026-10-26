namespace VetCare
{
    partial class FrmBackupRestore_60MN
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
            lblTitulo = new Label();
            grpRespaldo = new GroupBox();
            lblCarpeta = new Label();
            txtCarpeta = new TextBox();
            btnExaminarCarpeta = new Button();
            lblArchivoNuevo = new Label();
            lblNotaCarpeta = new Label();
            btnRespaldar = new Button();
            grpRestaurar = new GroupBox();
            lblRespaldos = new Label();
            dgvRespaldos = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colArchivo = new DataGridViewTextBoxColumn();
            colTamanio = new DataGridViewTextBoxColumn();
            colUsuario = new DataGridViewTextBoxColumn();
            colRuta = new DataGridViewTextBoxColumn();
            lblArchivo = new Label();
            txtArchivo = new TextBox();
            btnExaminarArchivo = new Button();
            lblAdvertencia = new Label();
            btnRestaurar = new Button();
            btnCerrar = new Button();
            grpRespaldo.SuspendLayout();
            grpRestaurar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRespaldos).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitulo.Location = new Point(16, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(386, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Backup y restore de la base de datos";
            //
            // grpRespaldo
            //
            grpRespaldo.Controls.Add(lblCarpeta);
            grpRespaldo.Controls.Add(txtCarpeta);
            grpRespaldo.Controls.Add(btnExaminarCarpeta);
            grpRespaldo.Controls.Add(lblArchivoNuevo);
            grpRespaldo.Controls.Add(lblNotaCarpeta);
            grpRespaldo.Controls.Add(btnRespaldar);
            grpRespaldo.Location = new Point(16, 52);
            grpRespaldo.Name = "grpRespaldo";
            grpRespaldo.Size = new Size(868, 160);
            grpRespaldo.TabIndex = 1;
            grpRespaldo.TabStop = false;
            grpRespaldo.Tag = "SEG_BACKUP";
            grpRespaldo.Text = "Respaldar";
            //
            // lblCarpeta
            //
            lblCarpeta.AutoSize = true;
            lblCarpeta.Location = new Point(16, 34);
            lblCarpeta.Name = "lblCarpeta";
            lblCarpeta.Size = new Size(144, 20);
            lblCarpeta.TabIndex = 0;
            lblCarpeta.Text = "Carpeta de destino *";
            //
            // txtCarpeta
            //
            txtCarpeta.Location = new Point(176, 30);
            txtCarpeta.Name = "txtCarpeta";
            txtCarpeta.Size = new Size(560, 27);
            txtCarpeta.TabIndex = 1;
            //
            // btnExaminarCarpeta
            //
            btnExaminarCarpeta.Location = new Point(746, 28);
            btnExaminarCarpeta.Name = "btnExaminarCarpeta";
            btnExaminarCarpeta.Size = new Size(106, 31);
            btnExaminarCarpeta.TabIndex = 2;
            btnExaminarCarpeta.Text = "Examinar...";
            btnExaminarCarpeta.UseVisualStyleBackColor = true;
            btnExaminarCarpeta.Click += btnExaminarCarpeta_Click;
            //
            // lblArchivoNuevo
            //
            lblArchivoNuevo.Location = new Point(176, 64);
            lblArchivoNuevo.Name = "lblArchivoNuevo";
            lblArchivoNuevo.Size = new Size(676, 22);
            lblArchivoNuevo.TabIndex = 3;
            //
            // lblNotaCarpeta
            //
            lblNotaCarpeta.ForeColor = SystemColors.GrayText;
            lblNotaCarpeta.Location = new Point(16, 90);
            lblNotaCarpeta.Name = "lblNotaCarpeta";
            lblNotaCarpeta.Size = new Size(620, 60);
            lblNotaCarpeta.TabIndex = 4;
            lblNotaCarpeta.Text = "Puede elegir cualquier carpeta donde usted tenga permiso de escritura (también un pendrive). SQL Server genera el backup en su propia carpeta y VetCare lo copia a la elegida, verificando que la copia sea idéntica.";
            //
            // btnRespaldar
            //
            btnRespaldar.Location = new Point(652, 104);
            btnRespaldar.Name = "btnRespaldar";
            btnRespaldar.Size = new Size(200, 40);
            btnRespaldar.TabIndex = 5;
            btnRespaldar.Text = "Respaldar base de datos";
            btnRespaldar.UseVisualStyleBackColor = true;
            btnRespaldar.Click += btnRespaldar_Click;
            //
            // grpRestaurar
            //
            grpRestaurar.Controls.Add(lblRespaldos);
            grpRestaurar.Controls.Add(dgvRespaldos);
            grpRestaurar.Controls.Add(lblArchivo);
            grpRestaurar.Controls.Add(txtArchivo);
            grpRestaurar.Controls.Add(btnExaminarArchivo);
            grpRestaurar.Controls.Add(lblAdvertencia);
            grpRestaurar.Controls.Add(btnRestaurar);
            grpRestaurar.Location = new Point(16, 222);
            grpRestaurar.Name = "grpRestaurar";
            grpRestaurar.Size = new Size(868, 360);
            grpRestaurar.TabIndex = 2;
            grpRestaurar.TabStop = false;
            grpRestaurar.Tag = "SEG_RESTORE";
            grpRestaurar.Text = "Restaurar";
            //
            // lblRespaldos
            //
            lblRespaldos.AutoSize = true;
            lblRespaldos.Location = new Point(16, 30);
            lblRespaldos.Name = "lblRespaldos";
            lblRespaldos.Size = new Size(274, 20);
            lblRespaldos.TabIndex = 0;
            lblRespaldos.Text = "Backups disponibles (o elija un archivo .bak de cualquier carpeta con Examinar...)";
            //
            // dgvRespaldos
            //
            dgvRespaldos.AllowUserToAddRows = false;
            dgvRespaldos.AllowUserToDeleteRows = false;
            dgvRespaldos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRespaldos.BackgroundColor = SystemColors.Window;
            dgvRespaldos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRespaldos.Columns.AddRange(new DataGridViewColumn[] { colFecha, colArchivo, colTamanio, colUsuario, colRuta });
            dgvRespaldos.Location = new Point(16, 54);
            dgvRespaldos.MultiSelect = false;
            dgvRespaldos.Name = "dgvRespaldos";
            dgvRespaldos.ReadOnly = true;
            dgvRespaldos.RowHeadersVisible = false;
            dgvRespaldos.RowHeadersWidth = 51;
            dgvRespaldos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRespaldos.Size = new Size(836, 180);
            dgvRespaldos.TabIndex = 1;
            dgvRespaldos.SelectionChanged += dgvRespaldos_SelectionChanged;
            //
            // colFecha
            //
            colFecha.DataPropertyName = "Fecha";
            estiloFecha.Format = "dd/MM/yyyy HH:mm:ss";
            colFecha.DefaultCellStyle = estiloFecha;
            colFecha.FillWeight = 80F;
            colFecha.HeaderText = "Fecha y hora";
            colFecha.MinimumWidth = 6;
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colArchivo
            //
            colArchivo.DataPropertyName = "Archivo";
            colArchivo.FillWeight = 130F;
            colArchivo.HeaderText = "Archivo";
            colArchivo.MinimumWidth = 6;
            colArchivo.Name = "colArchivo";
            colArchivo.ReadOnly = true;
            //
            // colTamanio
            //
            colTamanio.DataPropertyName = "Tamanio";
            colTamanio.FillWeight = 50F;
            colTamanio.HeaderText = "Tamaño";
            colTamanio.MinimumWidth = 6;
            colTamanio.Name = "colTamanio";
            colTamanio.ReadOnly = true;
            //
            // colUsuario
            //
            colUsuario.DataPropertyName = "Usuario";
            colUsuario.FillWeight = 70F;
            colUsuario.HeaderText = "Usuario";
            colUsuario.MinimumWidth = 6;
            colUsuario.Name = "colUsuario";
            colUsuario.ReadOnly = true;
            //
            // colRuta
            //
            colRuta.DataPropertyName = "Ruta";
            colRuta.FillWeight = 160F;
            colRuta.HeaderText = "Ruta";
            colRuta.MinimumWidth = 6;
            colRuta.Name = "colRuta";
            colRuta.ReadOnly = true;
            //
            // lblArchivo
            //
            lblArchivo.AutoSize = true;
            lblArchivo.Location = new Point(16, 250);
            lblArchivo.Name = "lblArchivo";
            lblArchivo.Size = new Size(143, 20);
            lblArchivo.TabIndex = 2;
            lblArchivo.Text = "Archivo de backup *";
            //
            // txtArchivo
            //
            txtArchivo.Location = new Point(176, 246);
            txtArchivo.Name = "txtArchivo";
            txtArchivo.Size = new Size(560, 27);
            txtArchivo.TabIndex = 3;
            //
            // btnExaminarArchivo
            //
            btnExaminarArchivo.Location = new Point(746, 244);
            btnExaminarArchivo.Name = "btnExaminarArchivo";
            btnExaminarArchivo.Size = new Size(106, 31);
            btnExaminarArchivo.TabIndex = 4;
            btnExaminarArchivo.Text = "Examinar...";
            btnExaminarArchivo.UseVisualStyleBackColor = true;
            btnExaminarArchivo.Click += btnExaminarArchivo_Click;
            //
            // lblAdvertencia
            //
            lblAdvertencia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAdvertencia.ForeColor = Color.Firebrick;
            lblAdvertencia.Location = new Point(16, 286);
            lblAdvertencia.Name = "lblAdvertencia";
            lblAdvertencia.Size = new Size(620, 60);
            lblAdvertencia.TabIndex = 5;
            lblAdvertencia.Text = "Restaurar reemplaza la base completa: se pierden los cambios posteriores al backup (incluida la bitácora) y al terminar se cierra la sesión. Solo se aceptan backups de VetCareBD_60MN.";
            //
            // btnRestaurar
            //
            btnRestaurar.Location = new Point(652, 300);
            btnRestaurar.Name = "btnRestaurar";
            btnRestaurar.Size = new Size(200, 40);
            btnRestaurar.TabIndex = 6;
            btnRestaurar.Text = "Restaurar base de datos";
            btnRestaurar.UseVisualStyleBackColor = true;
            btnRestaurar.Click += btnRestaurar_Click;
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(774, 592);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 34);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmBackupRestore_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(900, 640);
            Controls.Add(btnCerrar);
            Controls.Add(grpRestaurar);
            Controls.Add(grpRespaldo);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmBackupRestore_60MN";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Backup / Restore";
            Load += FrmBackupRestore_60MN_Load;
            grpRespaldo.ResumeLayout(false);
            grpRespaldo.PerformLayout();
            grpRestaurar.ResumeLayout(false);
            grpRestaurar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRespaldos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private GroupBox grpRespaldo;
        private Label lblCarpeta;
        private TextBox txtCarpeta;
        private Button btnExaminarCarpeta;
        private Label lblArchivoNuevo;
        private Label lblNotaCarpeta;
        private Button btnRespaldar;
        private GroupBox grpRestaurar;
        private Label lblRespaldos;
        private DataGridView dgvRespaldos;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colArchivo;
        private DataGridViewTextBoxColumn colTamanio;
        private DataGridViewTextBoxColumn colUsuario;
        private DataGridViewTextBoxColumn colRuta;
        private Label lblArchivo;
        private TextBox txtArchivo;
        private Button btnExaminarArchivo;
        private Label lblAdvertencia;
        private Button btnRestaurar;
        private Button btnCerrar;
    }
}
