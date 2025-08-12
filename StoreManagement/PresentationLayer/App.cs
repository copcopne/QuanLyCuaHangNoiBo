using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class App : Form
    {
        public App()
        {
            InitializeComponent();
            this.Shown += AppShown;
        }

        private void AppShown(object sender, EventArgs e)
        {
            if (BusinessLayer.AuthenticateBUS.CurrentUser == null)
            {
                using (var f = new LoginForm())
                {
                    var r = f.ShowDialog(this);
                    if (r != DialogResult.OK)
                    {
                        Close();
                        return;
                    }
                }
            }
        }

        private void App_Load(object sender, EventArgs e)
        {

        }
    }
}
