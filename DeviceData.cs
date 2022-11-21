using System;
using System.Windows.Forms;

namespace PNTN_prov
{
    internal partial class DeviceData : Form
    {
        public DeviceData()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            DataExchange.WriteCheckProtocol = true;

            DataExchange.blockNumber = blockNumberBox.Text;
            DataExchange.releaseDate = releaseDateBox.Text;

            blockNumberBox.Text = string.Empty;
            releaseDateBox.Text = string.Empty;

            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DataExchange.WriteCheckProtocol = false;
            Close();
        }
    }
}
