using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.Caching;
using Microsoft.QueryStringDotNET; // QueryString.NET
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApp1
{
    public partial class SetupNotifire : Form
    {
        public SetupNotifire()
        {
            InitializeComponent();
        }

        private void SetupNotifire_Load(object sender, EventArgs e)
        {
            if (ModuleC.firstLaunch == "no")
            {
                btn_plustard.Enabled = false;
                btn_plustard.Visible = false;
                btn_leave.Enabled = true;
                btn_leave.Visible = true;

                label1.Text = "Paramètre de notification Notifire";
            }

            string contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile); //On ecrit dans la variable contenuFile le contenu du fichier config dans appdata
            dynamic Json = JsonConvert.DeserializeObject<dynamic>(contenuFile); //Deserialization du fichier json

            if (Json.Config.listYes.ToString().Contains("Zendesk")){
                disablePbx(pictureBox1);
            }
            if (Json.Config.listYes.ToString().Contains("Slack"))
            {
                disablePbx(pictureBox2);
            }
            if (Json.Config.listYes.ToString().Contains("Redmine"))
            {
                disablePbx(pictureBox3);
            }
            if (Json.Config.listYes.ToString().Contains("Site Web Aramis"))
            {
                disablePbx(pictureBox4);
            }        



        }

        private void ajoutYes(string nameApp)
        {
            string contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile); //On ecrit dans la variable contenuFile le contenu du fichier config dans appdata
            dynamic Json = JsonConvert.DeserializeObject<dynamic>(contenuFile); //Deserialization du fichier json

            JObject rss = JObject.Parse(Json.ToString()); //On rentre le contenu du JSON entier dans une variable
            JObject config = (JObject)rss["Config"];      //On choisis la partie de traitement du json (ici Cache)
            JArray configYes = (JArray)config["listYes"];  //On créer une liste contenant tous les id d'incidents
            
            configYes.Add(nameApp); //On ajoute notre nouvelle incident à la liste
            
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(rss, Newtonsoft.Json.Formatting.Indented); //Variable contenant le json modifié

            System.IO.File.WriteAllText(ModuleC.pathSetupFile, output); //Ecriture du nouveau json
        }

        private void ajoutRemove()
        {
            string contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile); //On ecrit dans la variable contenuFile le contenu du fichier config dans appdata
            dynamic Json = JsonConvert.DeserializeObject<dynamic>(contenuFile); //Deserialization du fichier json

            JObject rss = JObject.Parse(Json.ToString()); //On rentre le contenu du JSON entier dans une variable
            JObject config = (JObject)rss["Config"];      //On choisis la partie de traitement du json (ici Cache)
            JArray configYes = (JArray)config["listYes"];  //On créer une liste contenant tous les id d'incidents

            configYes.RemoveAll(); //On ajoute notre nouvelle incident à la liste

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(rss, Newtonsoft.Json.Formatting.Indented); //Variable contenant le json modifié

            System.IO.File.WriteAllText(ModuleC.pathSetupFile, output); //Ecriture du nouveau json
        }

        private void disablePbx (PictureBox x)
        {
            x.BorderStyle = BorderStyle.Fixed3D;
            x.Enabled = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)  //Zendesk
        {
            disablePbx(pictureBox1);
            ajoutYes("Zendesk");

        }

        private void pictureBox2_Click(object sender, EventArgs e)  //Slack
        {
            disablePbx(pictureBox2);
            ajoutYes("Slack");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            disablePbx(pictureBox3);
            ajoutYes("Redmine");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            disablePbx(pictureBox4);
            ajoutYes("Site Web Aramis");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            pictureBox5.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            pictureBox6.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            pictureBox7.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            pictureBox8.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            pictureBox9.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            pictureBox10.BorderStyle = BorderStyle.Fixed3D;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //on change la valeur de la conf en yes
            string contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile); 
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(contenuFile);
            jsonObj["Config"]["Done"] = "yes";
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(ModuleC.pathSetupFile, output);


            //on ferme le formulaire
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //on ferme le formulaire
            pictureBox1.Enabled = true;
            pictureBox1.BorderStyle = BorderStyle.None;

            pictureBox2.Enabled = true;
            pictureBox2.BorderStyle = BorderStyle.None;

            pictureBox3.Enabled = true;
            pictureBox3.BorderStyle = BorderStyle.None;

            pictureBox4.Enabled = true;
            pictureBox4.BorderStyle = BorderStyle.None;

            ajoutRemove();

            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Enabled = true;
            pictureBox1.BorderStyle = BorderStyle.None;

            pictureBox2.Enabled = true;
            pictureBox2.BorderStyle = BorderStyle.None;

            pictureBox3.Enabled = true;
            pictureBox3.BorderStyle = BorderStyle.None;

            pictureBox4.Enabled = true;
            pictureBox4.BorderStyle = BorderStyle.None;

            ajoutRemove();
        }

        private void btn_leave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
