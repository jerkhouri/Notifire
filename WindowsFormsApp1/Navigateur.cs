using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Navigateur : Form
    {
        public Navigateur()
        {
            InitializeComponent();
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            this.CenterToScreen(); //centrer le formulaire au milieu de l'ecran
            lbl_name_link.Text = ModuleC.nameUrl; //on charge les deux variables necessaire au fonctionnement du formulaire
            //webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(ModuleC.navUrl); //On charge le lien url
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack(); //revenir à la page precedente
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward(); //aller à l'avant
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();  //Rafraichir
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close(); //fermer le formulaire
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) { }
    }
}
