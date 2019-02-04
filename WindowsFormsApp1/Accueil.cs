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
    public partial class Accueil : Form
    {             
        public Accueil()
        {
            InitializeComponent();
        }

        //Au lancement de l'application
        private void Form1_Load(object sender, EventArgs e) //A l'ouverture du formula
        {
            ModuleC.firstLaunch = "yes";
        }       
        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
            SetupNotifire();
        }       
        private void SetupNotifire() //PREMIER LANCEMENT DE L'APPLICATION
        {
            Console.WriteLine("Go");

            //Fichier Cache de paramêtre de l'application
            string contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile); //on charge le fichier dans la variable contenuFile
            dynamic Json = JsonConvert.DeserializeObject<dynamic>(contenuFile);     //On deserialize la variable en json dynamic

            JObject rss = JObject.Parse(Json.ToString());            //on charge le json dans un objet
            
            if ((string)rss["Config"]["Done"] == "no"){ //si la sous section Done de Confid est egale à no alors
                SetupNotifire setupNotifire = new SetupNotifire(); //initialise un nouveau formulaire
                setupNotifire.ShowDialog();  //On met en pause le programme et affiche le formulaire
            }

            //Une fois le formulaire fermé on recharge le fichier pour evaluer les changements

            contenuFile = System.IO.File.ReadAllText(ModuleC.pathSetupFile);    //On relis le fichier car il a pu changer
            Json = JsonConvert.DeserializeObject<dynamic>(contenuFile);     //on redeserialize
            rss = JObject.Parse(Json.ToString());       //on recharge le json

            ModuleC.listYes = (JArray)rss["Config"]["listYes"]; //on met a jour la liste des app en yes

            //On prepare la 1ère notification
            var title = "Notifire"; //titre
            var text = "C'est ici que vous recevrez les notifications d'incident !"; //texte
            var picture = ModuleC.pathPitcure + "RobotAa.png"; //image

            Notif.NotifStartup(title, text, picture); //on affiche la notif

            timer1.Start(); //on start le timer pour les incidents
            timer_change_type.Start(); //on start le timer 2 (pour la conf)
        }


        //Propriétés du formulaires...
        private void immoToolStrip_Click(object sender, EventArgs e) //Lorsque l'utilisateur clique sur le systray "immo"
        {
            Process p = new Process(); //on definit un nouvel objet process
            p.StartInfo.FileName = "https://docs.google.com/a/aramisauto.com/forms/d/e/1FAIpQLSc81fE_zGyhovE-rmtxDEayjZvZP-uFZiUzZMNe4JDQsEG46Q/viewform?c=0&w=1"; //On definie le liens
            p.Start(); //On lance le lien dans un nouvel onglet
        }
        private void ZendeskToolStrip_Click(object sender, EventArgs e) //Lorsque l'utilisateur clique sur le systray "Zendesk"
        {
            Navigateur formNav = new Navigateur(); //On definie une formulaire et le lance

            ModuleC.nameUrl = "Contactez le service support"; //Titre present sur le formulaire
            ModuleC.navUrl = "https://support.aramisauto.com/hc/fr/requests/new"; //url que l'on charge dans le formulaire
            formNav.Show(); //On affiche le formulaire
        }
        private void incidentsToolStrip_Click(object sender, EventArgs e) //Lorsque l'utilisateur clique sur le systray "Tableau de bord cachetHQ"
        {
            Navigateur formNav = new Navigateur(); //On definie une formulaire et le lance
            ModuleC.nameUrl = "Page d'incidents"; //Titre present sur le formulaire
            ModuleC.navUrl = ModuleC.urlCachet; //url que l'on charge dans le formulaire
            formNav.Show(); //On affiche le formulaire
        }
        private void infosToolStripMenuItem_Click(object sender, EventArgs e) //Lorsque l'utilisateur clique sur le systray "infos"
        {            
            Form3 formInfos = new Form3(); //On definie une formulaire et le lance
            formInfos.Show(); //On affiche le formulaire
        }         
        private void rentrerCesHeuresToolStripMenuItem_Click(object sender, EventArgs e) //Lorsque l'utilisateur clique sur le systray "redmine"
        {
            Navigateur formNav = new Navigateur(); //On definie une formulaire et le lance
            ModuleC.nameUrl = "Redmine"; //Titre present sur le formulaire
            ModuleC.navUrl = "https://redmine.aramisauto.com"; //url que l'on charge dans le formulaire
            formNav.Show(); //On affiche le formulaire
        } 
        


        //TIMER TOUTES LES 10 SECONDES
        private void timer1_Tick_1(object sender, EventArgs e) //Timer de verif de maj incident
        {
            Console.WriteLine("-------");
            VerifNews(); //on Charge la fonction verifnew (ligne 146)
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
            try
            {
                Console.WriteLine("VerifNews");
                var responseNew = await client.GetStringAsync(ModuleC.urlNewIncident); //Url lien nouveaux incidents
                var reponseClot = await client.GetStringAsync(ModuleC.urlClotIncident); //Url lien incidents clôts

                incidentsToolStrip.Enabled = true; //si le client communique avec le serveur cachetHQ
                notifyIcon1.Text = "Notifire - Service Opérationnel";

                Console.WriteLine("new?");
                if (ParseToJson(responseNew)) //Si il y a un incidents en cours
                {
                    var reponseName = await client.GetStringAsync(ModuleC.GetUrlCompenent(ModuleC.Component_id_Incident)); //On charge le contenu dans la variable
                    ParseToJsonCompenent(reponseName); //On parse la variable grace a la fonction ParseToCompenent (ligne 192)

                    VerifAndGo(); //Après avoir parser la variable et renseigné les bonnes variables pour créer une notif, on verifie si l'incident à déja été affiché à l'ecran (grace au cache)
                }
                else //si il n'y a aucun indicidents en cours
                {
                    Console.WriteLine("no new");
                }

                Console.WriteLine("");
                Console.WriteLine("clot?");
                if (ParseToJson(reponseClot)) //Si il y a un incidents Clot 
                {
                    var reponseName = await client.GetStringAsync(ModuleC.GetUrlCompenent(ModuleC.Component_id_Incident));  //On charge le contenu dans la variable
                    ParseToJsonCompenent(reponseName); //On parse la variable grace a la fonction ParseToCompenent (ligne 192)

                    VerifAndGo(); //Après avoir parser la variable et renseigné les bonnes variables pour créer une notif, on verifie si l'incident à déja été affiché à l'ecran (grace au cache)
                }
                else //si il n'y a aucun indicidents clot
                {
                    Console.WriteLine("no clot");
                }
            }
            catch {
                incidentsToolStrip.Enabled = false; //si le client ne communique pas avec le serveur cachetHQ
                notifyIcon1.Text = "Notfire - Service Indisponible";
            }            
        }


        //PARSER JSON
        public bool ParseToJson(string HttpString) //https://www.youtube.com/watch?v=CjoAYslTKX0   //Fonction qui renvoie une string et qui permet de recuperer des infos precices ici tous les incidents ouverts
        {    
            var message = JsonConvert.DeserializeObject<dynamic>(HttpString); //On deserialise le contenue du lien dans la variable message

            if (message.data.Count != 0) //Si il y a plus de 1 incident
            {
                foreach (var num in message.data) //On charge les differentes variables
                {
                    ModuleC.Id_Incident = num.id.ToString();
                    ModuleC.Name_Incident = num.name.ToString();
                    ModuleC.Message_Incident = num.message.ToString();
                    ModuleC.Component_id_Incident = num.component_id.ToString();
                    ModuleC.Status_Incident = num.status.ToString();
                    ModuleC.Date_Create_Incident = DateCreater(num.created_at.ToString());
                }       

                return true; //On retourne True pour dire qu'il y a eu un incident
            }
            else
            {                
                return false; //On retourne True pour dire qu'il n'y a pas d'incident
            }

        }

        public void ParseToJsonCompenent(string HttpString) //https://www.youtube.com/watch?v=CjoAYslTKX0   //Fonction qui renvoie une string et qui permet de recuperer des infos precices ici tous les incidents ouverts
        {
            var message = JsonConvert.DeserializeObject<dynamic>(HttpString); //On deserialise le contenue du lien dans la variable message

            foreach (var num in message.data) //On charge les nom du composant dans la variables
            {
                    ModuleC.Compenent_Name = num.name.ToString();
                }                           
        }

        
        //VERIFY IN THE CACHE AND SEND NOTIFICATION
        public void VerifAndGo() //Verification de doublons et envoie de notif
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
                    else //si l'incident est déja enregistré dans le cache
                    {
                        Console.WriteLine(ModuleC.Id_Incident + " est déja le cache ID"); //on affiche rien
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
                    else //si l'incident est déja enregistré dans le cache
                    {
                        Console.WriteLine(ModuleC.Id_Incident + " est déja le cache ID"); //on affiche rien
                    }
                }
            }
            else //Si l'application de l'incident n'est pas choisi par l'utilisateur
            {
                Console.WriteLine(ModuleC.Compenent_Name + " ne fais pas partis des applis autorisé");
            }   
        }


        //Create DATETIME
        public DateTime DateCreater(string hold_date) //Generation de la date.
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

