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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "https://www.aramisauto.com/";
            p.Start();
        }
        
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private readonly KonamiSequence _konamiSequence = new KonamiSequence();

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void Form3_KeyUp(object sender, KeyEventArgs e)
        {
            if (_konamiSequence.IsCompletedBy(e.KeyCode))
                ShowNav();
        }

        private void ShowNav()
        {
            Process p = new Process();
            p.StartInfo.FileName = "https://fr.wikipedia.org/wiki/Code_Konami";
            p.Start();
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void btn_setting_Click(object sender, EventArgs e)
        {
            SetupNotifire formSetup = new SetupNotifire();
            ModuleC.firstLaunch = "no";
            formSetup.Show();
        }
    }
}
