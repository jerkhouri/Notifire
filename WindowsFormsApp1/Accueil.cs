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


namespace WindowsFormsApp1
{
    public partial class Accueil : Form
    {
        private static readonly HttpClient client = new HttpClient();
        //VARAIBLE CACHE
        public string pathFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Notifire");
        //VARIABLE GLOBALES SUR L'INCIDENT
        public static string Id_Incident = null;
        public static string Name_Incident = null;
        public static string Message_Incident = null;
        public static DateTime Date_Create_Incident;
        public static string Component_id_Incident = null;
        public static bool Never_notif = true;

        public Accueil()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            CreateCache();            
        }

        private void NotifStartup(string titre, string commentaire, string picture)
        {

            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.ThreadOptions = PSThreadOptions.UseCurrentThread;
            rs.Open();

            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;

            ps.AddScript("Import-Module BurntToast");
            ps.AddScript("New-BurntToastNotification -Text " + titre + ", \"" + commentaire + "\" -AppLogo \"" + picture +"\"");
            ps.Invoke();


            rs.Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            var titre = "Notifire";
            var commentaire = "C'est ici que vous recevrez les notifications d'incident !";
            //var picture = Directory.GetCurrentDirectory() + "\\Resources\\RobotAa.png";
            var picture = ModuleC.pathPitcure + "RobotAa.png";


            NotifStartup(titre, commentaire, picture);
            this.Hide();
        }

        private void CreateCache()
        {            
            if (!File.Exists(pathFile))
            {
                System.IO.Directory.CreateDirectory(pathFile);
                pathFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Notifire\\Cache.txt");
                if (!File.Exists(pathFile))
                {
                    new StreamWriter(pathFile);
                }
            }
        }

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

        //NOTIFICATION 


        //TIMER TOUTES LES 10 SECONDES
        private void timer1_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show(Directory.GetCurrentDirectory() + "\\Resources");
            VerifNews();
        }

        //VERIFS NEWS
        async void VerifNews()
        {
            var responseString = await client.GetStringAsync(ModuleC.urlIncident);

            try
            {
                ParseToJson(responseString); //CONTENU DE LA PAGE HTML DANS httpResponseBody     

                //var message = JsonConvert.DeserializeObject<dynamic>(ParseToJson(httpResponseBody));

                VerifAndGo();

                //ParseToJson(httpResponseBody);       //ON APPEL LA VARIABLE toJSON
            }
            catch
            {
                //MessageBox.Show("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);

                //Double click on the Package.appxmanifest file in your project. & Click on the "Capabilities" tab.Add the Private Networks capability to your project.
            }
        }


        //PARSER JSON
        public void ParseToJson(string HttpString) //https://www.youtube.com/watch?v=CjoAYslTKX0   //Fonction qui renvoie une string et qui permet de recuperer des infos precices ici tous les incidents ouverts
        {

            try
            {

                var message = JsonConvert.DeserializeObject<dynamic>(HttpString);


                //var Messagebox = new MessageDialog(message.meta.pagination.total.ToString()).ShowAsync();

                foreach (var num in message.data)
                {
                    Id_Incident = num.id.ToString();
                    Name_Incident = num.name.ToString();
                    Message_Incident = num.message.ToString();
                    Component_id_Incident = num.component_id.ToString();
                    Date_Create_Incident = DateCreater(num.created_at.ToString());
                }
                //var Messagebox = new MessageDialog("Name = "+ Name + " & message = " + Message + "& date de debut d'incident ="+ Date_Create).ShowAsync();                   



                //Recupération de tous les incidents Agences et sieges

                //Filtre par UO a venir

                //Puis envoie de la notifs filtrer sur le poste (go sur une fonction)

            }
            catch (Exception ex)
            {
                MessageBox.Show("We had a problem: " + ex.Message);
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

        //VERIFY IN THE CACHE
        public void VerifAndGo()
        {
            string contenuFile = System.IO.File.ReadAllText(pathFile);


            if (!contenuFile.Contains(Id_Incident))
            {
                //Create dataFile.txt in LocalCacheFolder and write “My text” to it 
                var txt = contenuFile + Id_Incident + ";";
                System.IO.File.WriteAllText(pathFile, txt);                          
                

                Notif(Name_Incident, Message_Incident, Component_id_Incident); //Generation de la notif
            }
        }

        //NOTIF
        private void Notif(string title_Incident, string content_Incident, string compenent_Incident)
        {
            //Declaration des variables
            string title = title_Incident;
            string content = content_Incident;
            string picture = ModuleC.pathPitcure + "warning.png";
            string signature = "Via Notifonder";
            int compenent = Convert.ToInt16(compenent_Incident);

            switch (compenent)
            {
                case 8: //Slack icone
                    picture = ModuleC.pathPitcure + "Slack_Icon.png";
                    signature = "Slack";
                    break;
                case 9: //Aramis Icone
                    picture = ModuleC.pathPitcure + "logoAa.png";
                    signature = "Site Web Aramis";
                    break;
                case 10: //Zendesk Icone
                    picture = ModuleC.pathPitcure + "zendesk.png";
                    signature = "Zendesk";
                    break;
                case 11: //Redmine Icone
                    picture = ModuleC.pathPitcure + "redmine.png";
                    signature = "Redmine";
                    break;

            }

            title = title + " • " + signature;


            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.ThreadOptions = PSThreadOptions.UseCurrentThread;
            rs.Open();

            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;

            ps.AddScript("New-BurntToastNotification -Text \"" + title + "\", \"" + content + "\" -AppLogo \"" + picture + "\"");
            ps.Invoke();

            rs.Close();



            //DateTime localDate = DateTime.Now;
            //MessageBox.Show(localDate + "   " + Date_Create_Incident);
        }

    }
}

