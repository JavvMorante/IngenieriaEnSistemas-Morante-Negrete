namespace VetCare
{
    partial class FrmGestionUsuarios_60MN
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
            chkMostrarBajas = new CheckBox();
            dgvUsuarios = new DataGridView();
            colUsuario = new DataGridViewTextBoxColumn();
            colApellido = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colDni = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colFamilias = new DataGridViewTextBoxColumn();
            colPatentesIndividuales = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colIntentos = new DataGridViewTextBoxColumn();
            btnAnadir = new Button();
            btnModificar = new Button();
            btnDarBaja = new Button();
            btnReactivar = new Button();
            btnDesbloquear = new Button();
            btnBlanquear = new Button();
            btnPermisos = new Button();
            btnActualizar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(184, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de usuarios";
            //
            // chkMostrarBajas
            //
            chkMostrarBajas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkMostrarBajas.AutoSize = true;
            chkMostrarBajas.Location = new Point(620, 16);
            chkMostrarBajas.Name = "chkMostrarBajas";
            chkMostrarBajas.Size = new Size(212, 24);
            chkMostrarBajas.TabIndex = 1;
            chkMostrarBajas.Text = "Mostrar usuarios dados de baja";
            chkMostrarBajas.UseVisualStyleBackColor = true;
            chkMostrarBajas.CheckedChanged += chkMostrarBajas_CheckedChanged;
            //
            // dgvUsuarios
            //
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.BackgroundColor = SystemColors.Window;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { colUsuario, colApellido, colNombre, colDni, colEmail, colFamilias, colPatentesIndividuales, colEstado, colIntentos });
            dgvUsuarios.Location = new Point(12, 50);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.RowHeadersWidth = 51;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(820, 446);
            dgvUsuarios.TabIndex = 2;
            dgvUsuarios.CellDoubleClick += dgvUsuarios_CellDoubleClick;
            dgvUsuarios.DataBindingComplete += dgvUsuarios_DataBindingComplete;
            //
            // colUsuario
            //
            colUsuario.DataPropertyName = "NombreUsuario";
            colUsuario.FillWeight = 90F;
            colUsuario.HeaderText = "Usuario";
            colUsuario.MinimumWidth = 6;
            colUsuario.Name = "colUsuario";
            colUsuario.ReadOnly = true;
            //
            // colApellido
            //
            colApellido.DataPropertyName = "Apellido";
            colApellido.HeaderText = "Apellido";
            colApellido.MinimumWidth = 6;
            colApellido.Name = "colApellido";
            colApellido.ReadOnly = true;
            //
            // colNombre
            //
            colNombre.DataPropertyName = "Nombre";
            colNombre.HeaderText = "Nombre";
            colNombre.MinimumWidth = 6;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            //
            // colDni
            //
            colDni.DataPropertyName = "Dni";
            colDni.FillWeight = 70F;
            colDni.HeaderText = "DNI";
            colDni.MinimumWidth = 6;
            colDni.Name = "colDni";
            colDni.ReadOnly = true;
            //
            // colEmail
            //
            colEmail.DataPropertyName = "Email";
            colEmail.FillWeight = 150F;
            colEmail.HeaderText = "Mail";
            colEmail.MinimumWidth = 6;
            colEmail.Name = "colEmail";
            colEmail.ReadOnly = true;
            //
            // colFamilias
            //
            colFamilias.DataPropertyName = "Familias";
            colFamilias.FillWeight = 130F;
            colFamilias.HeaderText = "Familias";
            colFamilias.MinimumWidth = 6;
            colFamilias.Name = "colFamilias";
            colFamilias.ReadOnly = true;
            //
            // colPatentesIndividuales
            //
            colPatentesIndividuales.DataPropertyName = "PatentesIndividuales";
            colPatentesIndividuales.FillWeight = 60F;
            colPatentesIndividuales.HeaderText = "Patentes individuales";
            colPatentesIndividuales.MinimumWidth = 6;
            colPatentesIndividuales.Name = "colPatentesIndividuales";
            colPatentesIndividuales.ReadOnly = true;
            //
            // colEstado
            //
            colEstado.DataPropertyName = "Estado";
            colEstado.FillWeight = 70F;
            colEstado.HeaderText = "Estado";
            colEstado.MinimumWidth = 6;
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // colIntentos
            //
            colIntentos.DataPropertyName = "IntentosFallidos";
            colIntentos.FillWeight = 60F;
            colIntentos.HeaderText = "Intentos fallidos";
            colIntentos.MinimumWidth = 6;
            colIntentos.Name = "colIntentos";
            colIntentos.ReadOnly = true;
            //
            // btnAnadir
            //
            btnAnadir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAnadir.Location = new Point(846, 50);
            btnAnadir.Name = "btnAnadir";
            btnAnadir.Size = new Size(150, 36);
            btnAnadir.TabIndex = 3;
            btnAnadir.Tag = "SEG_USU_ALTA";
            btnAnadir.Text = "Añadir";
            btnAnadir.UseVisualStyleBackColor = true;
            btnAnadir.Click += btnAnadir_Click;
            //
            // btnModificar
            //
            btnModificar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnModificar.Location = new Point(846, 94);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(150, 36);
            btnModificar.TabIndex = 4;
            btnModificar.Tag = "SEG_USU_MODIF";
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            //
            // btnDarBaja
            //
            btnDarBaja.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDarBaja.Location = new Point(846, 138);
            btnDarBaja.Name = "btnDarBaja";
            btnDarBaja.Size = new Size(150, 36);
            btnDarBaja.TabIndex = 5;
            btnDarBaja.Tag = "SEG_USU_BAJA";
            btnDarBaja.Text = "Dar de baja";
            btnDarBaja.UseVisualStyleBackColor = true;
            btnDarBaja.Click += btnDarBaja_Click;
            //
            // btnReactivar
            //
            btnReactivar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReactivar.Location = new Point(846, 182);
            btnReactivar.Name = "btnReactivar";
            btnReactivar.Size = new Size(150, 36);
            btnReactivar.TabIndex = 6;
            btnReactivar.Tag = "SEG_USU_BAJA";
            btnReactivar.Text = "Reactivar";
            btnReactivar.UseVisualStyleBackColor = true;
            btnReactivar.Click += btnReactivar_Click;
            //
            // btnDesbloquear
            //
            btnDesbloquear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDesbloquear.Location = new Point(846, 226);
            btnDesbloquear.Name = "btnDesbloquear";
            btnDesbloquear.Size = new Size(150, 36);
            btnDesbloquear.TabIndex = 7;
            btnDesbloquear.Tag = "SEG_USU_DESBLOQ";
            btnDesbloquear.Text = "Desbloquear";
            btnDesbloquear.UseVisualStyleBackColor = true;
            btnDesbloquear.Click += btnDesbloquear_Click;
            //
            // btnBlanquear
            //
            btnBlanquear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBlanquear.Location = new Point(846, 270);
            btnBlanquear.Name = "btnBlanquear";
            btnBlanquear.Size = new Size(150, 36);
            btnBlanquear.TabIndex = 8;
            btnBlanquear.Tag = "SEG_USU_MODIF";
            btnBlanquear.Text = "Blanquear clave";
            btnBlanquear.UseVisualStyleBackColor = true;
            btnBlanquear.Click += btnBlanquear_Click;
            //
            // btnPermisos
            //
            btnPermisos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPermisos.Location = new Point(846, 314);
            btnPermisos.Name = "btnPermisos";
            btnPermisos.Size = new Size(150, 36);
            btnPermisos.TabIndex = 10;
            btnPermisos.Tag = "SEG_USU_MODIF";
            btnPermisos.Text = "Permisos";
            btnPermisos.UseVisualStyleBackColor = true;
            btnPermisos.Click += btnPermisos_Click;
            //
            // btnActualizar
            //
            btnActualizar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnActualizar.Location = new Point(846, 460);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(150, 36);
            btnActualizar.TabIndex = 9;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            //
            // FrmGestionUsuarios_60MN
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 508);
            Controls.Add(btnActualizar);
            Controls.Add(btnPermisos);
            Controls.Add(btnBlanquear);
            Controls.Add(btnDesbloquear);
            Controls.Add(btnReactivar);
            Controls.Add(btnDarBaja);
            Controls.Add(btnModificar);
            Controls.Add(btnAnadir);
            Controls.Add(dgvUsuarios);
            Controls.Add(chkMostrarBajas);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(800, 450);
            Name = "FrmGestionUsuarios_60MN";
            Text = "Gestión de usuarios";
            Load += FrmGestionUsuarios_60MN_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private CheckBox chkMostrarBajas;
        private DataGridView dgvUsuarios;
        private DataGridViewTextBoxColumn colUsuario;
        private DataGridViewTextBoxColumn colApellido;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colDni;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewTextBoxColumn colFamilias;
        private DataGridViewTextBoxColumn colPatentesIndividuales;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colIntentos;
        private Button btnAnadir;
        private Button btnModificar;
        private Button btnDarBaja;
        private Button btnReactivar;
        private Button btnDesbloquear;
        private Button btnBlanquear;
        private Button btnPermisos;
        private Button btnActualizar;
    }
}
