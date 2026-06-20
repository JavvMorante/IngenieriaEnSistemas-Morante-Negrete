using VetCare.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VetCare
{
    public partial class DefaultForm : Form
    {
        public DefaultForm()
        {
            InitializeComponent();
        }

        private void DefaultForm_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Resources.backgray;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
    }
}
