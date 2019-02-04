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

        private void SetupNotifire_Load(object sender, EventArgs e) //Chargement du formulaire
        {
            if (ModuleC.firstLaunch == "no") //Si ce n'est pas la premiere fois que ce formulaire apparaît
            {
                //On cache plustard et on afficher le boutons quitter
                btn_plustard.Enabled = false; 
                btn_plustard.Visible = false;
                btn_leave.Enabled = true;
                btn_leave.Visible = true;

                label1.Text = "Paramètre de notification Notifire"; //On cange le titre 
            }

            string contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile); //On ecrit dans la variable contenuFile le contenu du fichier config dans appdata
            dynamic Json = JsonConvert.DeserializeObject<dynamic>(contenuFile); //Deserialization du fichier json

            //On gere l'affichage du formulaire

            if (Json.Config.listYes.ToString().Contains("Zendesk")){ //si le cache contiens "zendesk" on le grise pour montrer qu'il est déja selectionner
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
            if (Json.Config.type.ToString().Contains("siege"))
            {
                cbx_siege.Checked = true;
            }
            if (Json.Config.type.ToString().Contains("agence"))
            {
                cbx_agence.Checked = true;
            }
            if (Json.Config.type.ToString().Contains("autres"))
            {
                cbx_autres.Checked = true;
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

        private void pictureBox3_Click(object sender, EventArgs e)  //Redmine
        {
            disablePbx(pictureBox3);
            ajoutYes("Redmine");
        }

        private void pictureBox4_Click(object sender, EventArgs e) //AramisAuto
        { 
            disablePbx(pictureBox4);
            ajoutYes("Site Web Aramis");
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

        
        private void button1_Click(object sender, EventArgs e)  //Button valider
        {
            //on change la valeur de la conf en yes
            string contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile); 
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(contenuFile);
            jsonObj["Config"]["Done"] = "yes";
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(ModuleC.pathSetupFile, output);
            
            if (this.cbx_siege.Checked == true)
            {
                jsonObj["Config"]["type"] = "siege";
            }else if (this.cbx_agence.Checked == true)
            {
                jsonObj["Config"]["type"] = "agence";
            }
            else if (this.cbx_autres.Checked == true)
            {
                jsonObj["Config"]["type"] = "autres";
            }
            else
            {
                jsonObj["Config"]["type"] = "";
            }

            output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(ModuleC.pathSetupFile, output);

            //on ferme le formulaire
            this.Close();
        }    


        private void button2_Click(object sender, EventArgs e) //Button Demander plus tard
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

        private void button3_Click(object sender, EventArgs e) //Button Reinitialiser
        {
            pictureBox1.Enabled = true;
            pictureBox1.BorderStyle = BorderStyle.None;

            pictureBox2.Enabled = true;
            pictureBox2.BorderStyle = BorderStyle.None;

            pictureBox3.Enabled = true;
            pictureBox3.BorderStyle = BorderStyle.None;

            pictureBox4.Enabled = true;
            pictureBox4.BorderStyle = BorderStyle.None;

            cbx_siege.Checked = false;
            cbx_agence.Checked = false;
            cbx_autres.Checked = false;

            ajoutRemove();
        }

        private void btn_leave_Click(object sender, EventArgs e) //Button quitter
        {
            this.Close();
        }

        private void cbx_siege_CheckedChanged(object sender, EventArgs e) //CheckBox siege
        {
            if (cbx_siege.Checked == true)
            {
                cbx_agence.Checked = false;
                cbx_autres.Checked = false;
            }
        }

        private void cbx_agence_CheckedChanged(object sender, EventArgs e) //CheckBox Agence
        {
            if (cbx_agence.Checked == true)
            {

                cbx_siege.Checked = false;
                cbx_autres.Checked = false;
            }
        }

        private void cbx_autres_CheckedChanged(object sender, EventArgs e) //CheckBox Autres
        {
            if (cbx_autres.Checked == true)
            {
                cbx_siege.Checked = false;
                cbx_agence.Checked = false;
            }
        }
    }
}
