using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>CUS10 flujo 6.1: dos versiones de un mismo producto lado a lado.</summary>
    public partial class FrmCompararVersiones_60MN : FormBase_60MN
    {
        public FrmCompararVersiones_60MN()
        {
            InitializeComponent();
            dgvDiferencias.AutoGenerateColumns = false;
        }

        public FrmCompararVersiones_60MN(ProductoHistorial_60MN anterior, ProductoHistorial_60MN posterior, List<DiferenciaVersion_60MN> diferencias)
            : this()
        {
            lblProducto.Text = $"{anterior.Codigo} - {posterior.Nombre}";
            colVersionA.HeaderText = $"Versión {anterior.FechaHora:dd/MM/yyyy HH:mm:ss}";
            colVersionB.HeaderText = $"Versión {posterior.FechaHora:dd/MM/yyyy HH:mm:ss}";
            dgvDiferencias.DataSource = diferencias;
        }

        private void dgvDiferencias_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvDiferencias.Rows)
                if (fila.DataBoundItem is DiferenciaVersion_60MN diferencia && diferencia.Cambio)
                    fila.DefaultCellStyle.BackColor = Color.LightYellow;
        }
    }
}
