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
            DataExchange.writeCheckProtocol = true;

            DataExchange.blockNumber = blockNumberBox.Text;
            DataExchange.releaseDate = releaseDateBox.Text;

            blockNumberBox.Text = "";
            releaseDateBox.Text = "";

            Close();
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            DataExchange.writeCheckProtocol = false;
            Close();
        }
    }
}
