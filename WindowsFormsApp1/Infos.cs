using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e){}

        private void label1_Click(object sender, EventArgs e){}

        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.Close(); //Ferme le formulaire
        }

        private void btn_setting_Click(object sender, EventArgs e) // Lorsque l'utilisateur clique sur le bouton paramètre
        {
            SetupNotifire formSetup = new SetupNotifire(); //on charge un nouveau formulaire
            ModuleC.firstLaunch = "no"; //On fait comprendre a l'outils que l'utilisateur n'est pas a son premier lancement d'application !
            formSetup.Show(); //on affiche le formulaire
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //Clique sur AramisAuto
        {
            Process p = new Process(); //Nouvel process dans objet
            p.StartInfo.FileName = "https://www.aramisauto.com/"; //on rentre le liens à start
            p.Start(); // on lance le lien dans un nouvel onglet
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e) { }





        /*KONAMISEQUENCE A VOIR POSSIBILITE DE FUN 
        Pour activer la konami sequence il faut:
        1- Appuyer sur le logo aramis
        2- Rentrer la sequence, pour rappel c'est :
            (fleche haut) + (fleche bas) +  (fleche haut) 
            + (fleche bas) + (fleche gauche) + (fleche droite) 
            + (fleche gauche) + (fleche droite) + (lettre B) + (lettre A) */

        private readonly KonamiSequence _konamiSequence = new KonamiSequence();

        //Click sur le logo AA
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        //konami sequence
        private void Form3_KeyUp(object sender, KeyEventArgs e)
        {
            if (_konamiSequence.IsCompletedBy(e.KeyCode))
                KonamiSequence(); //Lorsque le konami est complet
        }


        private void KonamiSequence() //fonction konami
        {
            Process p = new Process();
            p.StartInfo.FileName = "https://fr.wikipedia.org/wiki/Code_Konami";
            p.Start();
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e) { }
    }
}
