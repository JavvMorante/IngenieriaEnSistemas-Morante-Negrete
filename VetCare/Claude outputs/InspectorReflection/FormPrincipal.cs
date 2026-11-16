using System;
using System.Windows.Forms;
using InspectorReflection.Inspeccion;
using InspectorReflection.Negocio;

namespace InspectorReflection
{
    public partial class FormPrincipal : Form
    {
        private readonly InspectorClase _inspector = new InspectorClase();

        public FormPrincipal()
        {
            InitializeComponent();
            dgvMiembros.AutoGenerateColumns = false;
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            Type tipo = typeof(Producto);

            lblClase.Text = "Clase inspeccionada: " + tipo.FullName;
            dgvMiembros.DataSource = _inspector.Inspeccionar(tipo);
        }
    }
}
/*
 * ¿Qué es Reflection (Reflexión)?
Es un mecanismo de .NET que permite examinar los metadatos de tipos, ensamblados y objetos en tiempo de ejecución. En este sistema se utilizó para obtener de forma dinámica los miembros (FieldInfo, PropertyInfo y MethodInfo) de la clase Producto sin necesidad de conocer o hardcodear sus nombres de antemano.

¿Cómo funciona el Patrón Chain of Responsibility (Cadena de Responsabilidad)?
Es un patrón de diseño comportamental que permite pasar solicitudes a lo largo de una cadena de manejadores. Cada eslabón evalúa si puede procesar el objeto recibido; si está capacitado lo maneja, y si no, delega la responsabilidad al siguiente eslabón de la cadena de forma desacoplada. Esto evita estructuras condicionales masivas (if-else o switch) en la interfaz gráfica.
*/