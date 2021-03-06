﻿using System;
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
    public partial class Accueil : Form
    {             
        public Accueil()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) //A l'ouverture du formula
        {
            ModuleC.firstLaunch = "yes";
        }       

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
            SetupNotifire();
            
        }       

        private void SetupNotifire()
        {
            Console.WriteLine("Go");
            string contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile); //on charge le fichier dans la variable contenuFile
            dynamic Json = JsonConvert.DeserializeObject<dynamic>(contenuFile);     //On deserialize la variable en json dynamic

            JObject rss = JObject.Parse(Json.ToString());            //on charge le json dans un objet
            
            if ((string)rss["Config"]["Done"] == "no"){ //si la sous section Done de Confid est egale à no alors
                SetupNotifire setupNotifire = new SetupNotifire(); //initialise un nouveau formulaire
                setupNotifire.ShowDialog();  //On met en pause le programme et affiche le formulaire
            }

            contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile);    //On relis le fichier car il a pu changer
            Json = JsonConvert.DeserializeObject<dynamic>(contenuFile);     //on redeserialize
            rss = JObject.Parse(Json.ToString());       //on recharge le json

            ModuleC.listYes = (JArray)rss["Config"]["listYes"]; //on met a jour la liste des app en yes

            var titre = "Notifire";
            var commentaire = "C'est ici que vous recevrez les notifications d'incident !";
            var picture = ModuleC.pathPitcure + "RobotAa.png";

            Notif.NotifStartup(titre, commentaire, picture); //on affiche la notif

            timer1.Start(); //on start le timer
            timer_change_type.Start();
        }

        //Propriétés du formulaires...
        private void immoToolStrip_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "https://docs.google.com/a/aramisauto.com/forms/d/e/1FAIpQLSc81fE_zGyhovE-rmtxDEayjZvZP-uFZiUzZMNe4JDQsEG46Q/viewform?c=0&w=1";
            p.Start();

        }

        private void ZendeskToolStrip_Click(object sender, EventArgs e)
        {
            Navigateur formNav = new Navigateur();

            ModuleC.nameUrl = "Contactez le service support";
            ModuleC.navUrl = "https://support.aramisauto.com/hc/fr/requests/new";
            formNav.Show();
        }

        private void incidentsToolStrip_Click(object sender, EventArgs e)
        {
            Navigateur formNav = new Navigateur();;
            ModuleC.nameUrl = "Page d'incidents";
            ModuleC.navUrl = ModuleC.urlCachet;
            formNav.Show();
        }

        private void infosToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            Form3 formInfos = new Form3();
            formInfos.Show();
        }

        private void rentrerCesHeuresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Navigateur formNav = new Navigateur(); ;
            ModuleC.nameUrl = "Redmine";
            ModuleC.navUrl = "https://redmine.aramisauto.com";
            formNav.Show();
        } 

        

        //TIMER TOUTES LES 10 SECONDES
        private void timer1_Tick_1(object sender, EventArgs e) //Timer de verif de maj incident
        {
            Console.WriteLine("-------");
            VerifNews();
        } 

        private void timer_change_type_Tick(object sender, EventArgs e) //on verifie si l'utilisateur a changer son type dans les parametre
        {
            string contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile); //on charge le fichier dans la variable contenuFile
            dynamic Json = JsonConvert.DeserializeObject<dynamic>(contenuFile);     //On deserialize la variable en json dynamic

            JObject rss = JObject.Parse(Json.ToString());            //on charge le json dans un objet

            if ((string)rss["Config"]["type"] == "siege")
            { //si la sous section Done de Confid est egale à no alors
                this.rentrerCesHeuresToolStripMenuItem.Visible = true;
            }
            else
            {
                this.rentrerCesHeuresToolStripMenuItem.Visible = false;
            }
        }

        //VERIFS NEWS
        private static readonly HttpClient client = new HttpClient();
        public async void VerifNews()
        {
            Console.WriteLine("VerifNews");
            var responseNew = await client.GetStringAsync(ModuleC.urlNewIncident);
            var reponseClot = await client.GetStringAsync(ModuleC.urlClotIncident);

            try
            {
                Console.WriteLine("new?");
                if (ParseToJson(responseNew))
                {
                    var reponseName = await client.GetStringAsync(ModuleC.GetUrlCompenent(ModuleC.Component_id_Incident));
                    ParseToJsonCompenent(reponseName);

                    VerifAndGo();
                }
                else
                {
                    Console.WriteLine("no new");
                }

                Console.WriteLine("");
                Console.WriteLine("clot?");
                if (ParseToJson(reponseClot))
                {
                    var reponseName = await client.GetStringAsync(ModuleC.GetUrlCompenent(ModuleC.Component_id_Incident));
                    ParseToJsonCompenent(reponseName);

                    VerifAndGo();
                }
                else
                {
                    Console.WriteLine("no clot");
                }



            }
            catch { }


            
        }


        //PARSER JSON
        public bool ParseToJson(string HttpString) //https://www.youtube.com/watch?v=CjoAYslTKX0   //Fonction qui renvoie une string et qui permet de recuperer des infos precices ici tous les incidents ouverts
        {    
            var message = JsonConvert.DeserializeObject<dynamic>(HttpString);

            if (message.data.Count != 0)
            {
                foreach (var num in message.data)
                {
                    ModuleC.Id_Incident = num.id.ToString();
                    ModuleC.Name_Incident = num.name.ToString();
                    ModuleC.Message_Incident = num.message.ToString();
                    ModuleC.Component_id_Incident = num.component_id.ToString();
                    ModuleC.Status_Incident = num.status.ToString();
                    ModuleC.Date_Create_Incident = DateCreater(num.created_at.ToString());
                }       

                return true;
            }
            else
            {                
                return false;
            }

        }

        public void ParseToJsonCompenent(string HttpString) //https://www.youtube.com/watch?v=CjoAYslTKX0   //Fonction qui renvoie une string et qui permet de recuperer des infos precices ici tous les incidents ouverts
        {
            var message = JsonConvert.DeserializeObject<dynamic>(HttpString);

            foreach (var num in message.data)
                {
                    ModuleC.Compenent_Name = num.name.ToString();
                }                           
        }

        
        //VERIFY IN THE CACHE
        public void VerifAndGo()
        {
            Console.WriteLine("Verif&go");
            string contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile); //On ecrit dans la variable contenuFile le contenu du fichier config dans appdata
            dynamic Json = JsonConvert.DeserializeObject<dynamic>(contenuFile); //Deserialization du fichier json

            JObject rss = JObject.Parse(Json.ToString()); //On rentre le contenu du JSON entier dans une variable
            JObject cache = (JObject)rss["Cache"];      //On choisis la partie de traitement du json (ici Cache)
                        
            Console.WriteLine(ModuleC.Compenent_Name);

            if (Json.Config.listYes.ToString().Contains(ModuleC.Compenent_Name) || Json.Config.listOblige.ToString().Contains(ModuleC.Compenent_Name)) //Verifie si present dans le conf app ok
            {
                Console.WriteLine(ModuleC.Compenent_Name + " Existe dans la liste des applis autorisées");

                if (ModuleC.Status_Incident == "1") //Si c'est un nouvel incident alors on traite de cache des nouveaux incidents
                {
                    if (!Json.Cache.New.ToString().Contains(ModuleC.Id_Incident)) //Si l'id de l'incident existe pas dans le cache alors
                    {
                        Console.WriteLine(ModuleC.Id_Incident + " n'éxiste pas dans le cache ID");

                        JArray cacheNewID = (JArray)cache["New"];  //On créer une liste contenant tous les id d'incidents
                        cacheNewID.Add(ModuleC.Id_Incident); //On ajoute notre nouvelle incident à la liste

                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(rss, Newtonsoft.Json.Formatting.Indented); //Variable contenant le json modifié

                        System.IO.File.WriteAllText(ModuleC.pathSetupFile, output); //Ecriture du nouveau json

                        Console.WriteLine("Creation Notif");
                        Notif.CreateNotif(ModuleC.Name_Incident, ModuleC.Message_Incident, ModuleC.Component_id_Incident); //Generation de la notif         

                    }
                    else
                    {
                        Console.WriteLine(ModuleC.Id_Incident + " est déja le cache ID");
                    }

                }
                else if (ModuleC.Status_Incident == "4") //Si c'est un hold incident alors on traite de cache des anciens incidents
                {
                    if (!Json.Cache.Hold.ToString().Contains(ModuleC.Id_Incident)) //Si l'id de l'incident existe pas dans le cache alors
                    {
                        Console.WriteLine(ModuleC.Id_Incident + " n'éxiste pas dans le cache ID");

                        JArray cacheHoldID = (JArray)cache["Hold"];  //On créer une liste contenant tous les id d'incidents
                        cacheHoldID.Add(ModuleC.Id_Incident); //On ajoute notre nouvelle incident à la liste

                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(rss, Newtonsoft.Json.Formatting.Indented); //Variable contenant le json modifié

                        System.IO.File.WriteAllText(ModuleC.pathSetupFile, output); //Ecriture du nouveau json

                        Console.WriteLine("Creation Notif");
                        Notif.CreateNotif(ModuleC.Name_Incident, ModuleC.Message_Incident, ModuleC.Component_id_Incident); //Generation de la notif         

                    }
                    else
                    {
                        Console.WriteLine(ModuleC.Id_Incident + " est déja le cache ID");
                    }
                }
            }
            else
            {
                Console.WriteLine(ModuleC.Compenent_Name + " ne fais pas partis des applis autorisé");
            }




            
            

            
            
        }


        //Create DATETIME
        public DateTime DateCreater(string hold_date)
        {
            var new_date = new DateTime(Convert.ToInt16(hold_date.Substring(0, 4)),     //Année
                                        Convert.ToInt16(hold_date.Substring(5, 2)),     //Mois
                                        Convert.ToInt16(hold_date.Substring(8, 2)),     //Jours
                                        Convert.ToInt16(hold_date.Substring(11, 2)),   //Heure
                                        Convert.ToInt16(hold_date.Substring(14, 2)),   //Minutes
                                        Convert.ToInt16(hold_date.Substring(17, 2)));  //Secondes
            return new_date;
        }        
    }
}

