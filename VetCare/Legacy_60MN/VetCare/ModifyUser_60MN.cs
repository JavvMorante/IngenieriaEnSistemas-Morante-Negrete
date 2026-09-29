using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace VetCare
{
    public partial class ModifyUser_60MN : Form
    {

        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN crypt = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
        BLL_60MN.UsuarioBLL_60MN usu = new BLL_60MN.UsuarioBLL_60MN();
        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();

        public string Dato { get; set; }

        public ModifyUser_60MN()
        {
            InitializeComponent();
        }
        public ModifyUser_60MN(string dato)
        {
            InitializeComponent();
            Dato = dato;
        }

        private void ModifyUser_60MN_Load(object sender, EventArgs e)
        {
            usu = usu.TraerDatosUsuariobyID(Convert.ToInt16(Dato));
           

            txtNombre.Text = usu.Nombre.ToString();
            txtDNI.Text = usu.Dni.ToString();
            txtEmail.Text = usu.Email.ToString();
            txtApellido.Text = usu.Apellido.ToString();
            txtUsuario.Text = usu._Usuario.ToString();

            if (usu.Habilitado.ToString() == "True")
            {
                chkHabilitado.Checked = usu.Habilitado;
            }
            else
            {
                chkHabilitado.Checked = false;
            }


        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ABMUsuarios mu = new ABMUsuarios();

            mu.cargar();
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // 1. VALIDACIÓN: Verificar que ningún campo de texto esté vacío o tenga solo espacios
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtDNI.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios. Por favor, complete la información.",
                                "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Corta la ejecución aquí para que no guarde
            }

            try
            {
                // 2. DETECTAR CAMBIO DE ESTADO (Habilitado / Deshabilitado)
                // 'usu' mantiene los datos que trajiste originalmente de la BD en el Load.
                bool estadoOriginal = usu.Habilitado;
                bool estadoNuevo = chkHabilitado.Checked;

                // Si el estado cambió, disparamos la pregunta correspondiente
                if (estadoOriginal != estadoNuevo)
                {
                    string mensajePregunta = "";

                    if (estadoOriginal == false && estadoNuevo == true)
                    {
                        // Pasó de NO chequeado a SI chequeado
                        mensajePregunta = $"¿Quiere dar de alta el usuario \"{txtUsuario.Text}\"?";
                    }
                    else if (estadoOriginal == true && estadoNuevo == false)
                    {
                        // Pasó de SI chequeado a NO chequeado
                        mensajePregunta = $"¿Quiere dar de baja el usuario \"{txtUsuario.Text}\"?";
                    }

                    // Mostramos el cuadro de confirmación
                    DialogResult respuesta = MessageBox.Show(mensajePregunta, "Confirmar cambio de estado",
                                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (respuesta == DialogResult.No)
                    {
                        return; // Si el usuario pone "No", cancelamos el guardado
                    }
                }

                // 3. EJECUTAR EL UPDATE DINÁMICO
                // Usamos el método que adaptamos antes pasándole los datos de los controles
                string result = usu.ModificarDatosUsuario(
                    usuarioid: usu.UsuarioID,
                    _Usuario: txtUsuario.Text.Trim(),
                    apellido: txtApellido.Text.Trim(),
                    nombre: txtNombre.Text.Trim(),
                    email: txtEmail.Text.Trim(),
                    dni: Convert.ToInt64(txtDNI.Text.Trim()),
                    habilitado: estadoNuevo
                );

                MessageBox.Show(result, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 4. REGISTRO EN BITÁCORA
                log.Criticidad = 3;
                log.Descripcion = $"{txtUsuario.Text} {txtApellido.Text} {txtNombre.Text} {txtEmail.Text} {txtDNI.Text} {estadoNuevo} {usu.UsuarioID}";
                log.FechayHora = DateTime.Now;
                log.NombreOperacion = "Modificar Usuario";

                log.IngresarDatoBitacora(crypt.Encriptar(log.NombreOperacion), crypt.Encriptar(log.Descripcion), log.Criticidad, usu.UsuarioID);

                // 5. CERRAR Y VOLVER
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar modificar el usuario: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
