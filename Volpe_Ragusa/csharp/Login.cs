namespace Volpe_Ragusa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string email = textBoxEmail.Text;
            string password = textBoxPassword.Text;
            if (email != "" && password != "")
            {
                //invia le cose a python
            }
        }

        private void linkRegistrazione_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //riporta alla pagina di registrazione
        }
    }
}
