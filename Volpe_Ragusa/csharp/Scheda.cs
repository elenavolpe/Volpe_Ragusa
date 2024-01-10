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
    public partial class Scheda : Form
    {
        public Scheda()
        {
            InitializeComponent();
        }

        //è sbagliato, non è al click
        private void labelHeader_Click(object sender, EventArgs e)
        {
            //fare in modo che aperta la sessione al posto di nome si veda il nome dell'utente
        }
    }
}
