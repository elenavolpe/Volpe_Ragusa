using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Volpe_Ragusa.csharp
{
    public partial class Home : Form
    {
        string email;
        public Home(string email)
        {
            InitializeComponent();
            this.email = email;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Account account = new Account(email);
            account.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Scheda scheda = new Scheda(email);
            scheda.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 login=new Form1();
            login.Show();
        }
    }
}
