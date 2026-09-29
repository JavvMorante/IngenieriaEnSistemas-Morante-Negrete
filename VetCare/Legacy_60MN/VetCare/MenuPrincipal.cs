using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VetCare
{
    public partial class MenuPrincipal : Form
    {
        private static MenuPrincipal miobj;
        IList<BLL_60MN.Componente> listaoperaciones = new List<BLL_60MN.Componente>();
        public int Usuarioid;
        ABMUsuarios abmusuario;
        BLL_60MN.IdiomaBLL_60MN idi = new BLL_60MN.IdiomaBLL_60MN();

        private static MenuPrincipal instance;

        public MenuPrincipal()
        {
            InitializeComponent();
            // Vinculamos el evento de cierre de forma manual para asegurar el fin de la app
            this.FormClosed += MenuPrincipal_FormClosed;
        }

        public static MenuPrincipal Instance
        {
            get
            {
                // Agregamos la validación IsDisposed por si el formulario se cerró y se vuelve a invocar
                if (instance == null || instance.IsDisposed)
                {
                    instance = new MenuPrincipal();
                }
                return instance;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            BLL_60MN.ManejadorPerfilUsuarioBLL_60MN mpu = new BLL_60MN.ManejadorPerfilUsuarioBLL_60MN();

            BLL_60MN.DirectorioPerfil Lo = new BLL_60MN.DirectorioPerfil("Login");

            listaoperaciones = Lo.obtenerhijos(Usuarioid);

            this.MapeoComponentes(listaoperaciones);






        }

        //CrearItem
        private ToolStripMenuItem CrearItem(string texto, Action accion)
        {
            return new ToolStripMenuItem(texto, null, (s, e) => accion());
        }
        //AbrirMdi
        private void AbrirMdi(Form form)
        {
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
            form.BringToFront();
        }

        public void MapeoComponentes(IList<BLL_60MN.Componente> listaoperaciones)
        {
            BLL_60MN.IdiomaBLL_60MN idi = new BLL_60MN.IdiomaBLL_60MN();
            string idioma = idi.CargarIdioma();

            System.Resources.ResourceManager rm;
            if (idioma == "Ingles")
            {
                rm = VetCare.Idioma.Ingles.ResourceManager;
            }
            else
            {
                rm = VetCare.Idioma.Espanol.ResourceManager;
            }

            LogOut logout = new LogOut();

            ToolStripDropDown drop = new ToolStripDropDown();
            drop.Text = "Menu";

            ToolStripDropDown dropnegocio = new ToolStripDropDown();
            dropnegocio.Text = "Menu";

            // Limpiamos los ítems previos para evitar duplicaciones por el Timer
            seguridadYProcesosToolStripMenuItem.DropDownItems.Clear();
            menuToolStripMenuItem.DropDownItems.Clear();

            foreach (var i in listaoperaciones)
            {
                if (i.Nombre == null) continue;
                string var = i.Nombre.ToString();

                // Buscamos dinámicamente la traducción en el archivo de recursos usando el nombre del componente.
                // Si no se encuentra, usamos el nombre técnico como fallback de seguridad.
                string textoTraducido = rm.GetString(var) ?? var;

                switch (var)
                {
                    case "hacerbackup":
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(textoTraducido, () =>
                            {
                                var f = new HacerBackUp();
                                ControlarVisibilidadHijo(f);
                            })
                        );
                        break;

                    case "abmusuario":
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(textoTraducido, () =>
                            {
                                var f = new ABMUsuarios();
                                ControlarVisibilidadHijo(f);
                            })
                        );
                        break;

                    case "abmperfilusuario" or "modificarperfilusuario":
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(textoTraducido, () =>
                            {
                                var f = new ABMPerfilesUsuario();
                                ControlarVisibilidadHijo(f);
                            })
                        );
                        break;

                    case "ConfigurarIdioma":
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(textoTraducido, () =>
                            {
                                var f = new ConfigurarIdioma();
                                ControlarVisibilidadHijo(f);
                            })
                        );
                        break;

                    case "abmfamilias":
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(textoTraducido, () =>
                            {
                                var f = new ABMFamilias();
                                ControlarVisibilidadHijo(f);
                            })
                        );
                        break;

                    case "hacerrestore":
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(textoTraducido, () =>
                            {
                                var f = new HacerRestore();
                                ControlarVisibilidadHijo(f);
                            })
                        );
                        break;

                    case "consultarbitacora":
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(textoTraducido, () =>
                            {
                                var f = new ConsultarBitacora();
                                ControlarVisibilidadHijo(f);
                            })
                        );
                        break;

                    case "digitosverificadores":
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(textoTraducido, () =>
                            {
                                var f = new DigitosVerificadores();
                                ControlarVisibilidadHijo(f);
                            })
                        );
                        break;

                    case "Asignacion_de_Patentes":
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(textoTraducido, () =>
                            {
                                var f = new AsignacionDePatentes();
                                ControlarVisibilidadHijo(f);
                            })
                        );
                        break;

                    case "BloquearDesbloquearOperacionesaUsuario":
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(textoTraducido, () =>
                            {
                                var f = new Bloquear_DesbloquearOperacionesaUsuario();
                                ControlarVisibilidadHijo(f);
                            })
                        );
                        break;

                    case "DesbloquearOperacionAUsuariocs":
                        // 1. Agrega el ítem de Desbloquear usando la traducción
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(textoTraducido, () =>
                            {
                                var f = new DesbloquearOperacionAUsuarios();
                                ControlarVisibilidadHijo(f);
                            })
                        );

                        // 2. Agrega el ítem secundario de Asignación de Patentes en el mismo case
                        // Buscamos la traducción específica de la clave de patentes en el archivo
                        string txtPatentes = rm.GetString("Asignacion_de_Patentes") ?? "Asignación de Patentes";
                        seguridadYProcesosToolStripMenuItem.DropDownItems.Add(
                            CrearItem(txtPatentes, () =>
                            {
                                var f = new AsignacionDePatentes();
                                ControlarVisibilidadHijo(f);
                            })
                        );
                        break;

                    default:
                        break;
                }
            }
        }

        private void ControlarVisibilidadHijo(Form formularioHijo)
        {
        
            GestionBox.Visible = false;
            btnSalida.Visible = false;

          
            formularioHijo.FormClosed += (sender, e) =>
            {
              
                if (this.MdiChildren.Length <= 1)
                {
                    GestionBox.Visible = true;
                    btnSalida.Visible = true;
                }
            };

            AbrirMdi(formularioHijo);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        // CONTROL AL CERRAR EL MENÚ: Cierra definitivamente la aplicación en segundo plano
        private void MenuPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsultarBitacora consultarBitacora = new ConsultarBitacora();
            consultarBitacora.Show();
            this.Hide();
        }

        private void gestionUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ABMUsuarios aBMUsuarios = new ABMUsuarios();
            aBMUsuarios.Show();
            this.Hide();
        }

        private void idiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurarIdioma configurarIdioma = new ConfigurarIdioma();
            configurarIdioma.Show();
            this.Hide();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void consultarBitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsultarBitacora consultarBitacora = new ConsultarBitacora();
            consultarBitacora.Show();
            this.Hide();

        }

        private void gestionUsuariosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ABMUsuarios aBMUsuarios = new ABMUsuarios();
            aBMUsuarios.Show();
            this.Hide();
        }

        private void gestionPermisosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void configuracionIdiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurarIdioma configurarIdioma = new ConfigurarIdioma();
            configurarIdioma.Show();
            this.Hide();
        }

        private void btnSalida_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}