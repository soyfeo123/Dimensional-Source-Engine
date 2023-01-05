using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DimensionalSourceSDK
{
    public partial class MainPanel : Form
    {
        public MainPanel()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void OnBtnMouseEnter(object sender, EventArgs e)
        {
            Label senderLabel = sender as Label;
            senderLabel.Font = new Font(senderLabel.Font.Name, senderLabel.Font.SizeInPoints, FontStyle.Underline);
        }

        private void OnBtnMouseLeave(object sender, EventArgs e)
        {
            Label senderLabel = sender as Label;
            senderLabel.Font = new Font(senderLabel.Font.Name, senderLabel.Font.SizeInPoints, FontStyle.Regular);
        }
    }
}
