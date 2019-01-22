using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace WindowsFormsApp1
{
    public class Notif
    {
        public static void NotifStartup(string titre, string commentaire, string picture)
        {

            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.ThreadOptions = PSThreadOptions.UseCurrentThread;
            rs.Open();

            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;

            ps.AddScript("Import-Module BurntToast");
            ps.AddScript("New-BurntToastNotification -Text " + titre + ", \"" + commentaire + "\" -AppLogo \"" + picture + "\"");
            ps.Invoke();


            rs.Close();
        }

        public static void CreateNotif(string title_Incident, string content_Incident, string compenent_Incident)
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
                    break;
                case 9: //Aramis Icone
                    picture = ModuleC.pathPitcure + "logoAa.png";
                    break;
                case 10: //Zendesk Icone
                    picture = ModuleC.pathPitcure + "zendesk.png";
                    break;
                case 11: //Redmine Icone
                    picture = ModuleC.pathPitcure + "redmine.png";
                    break;
                case 12: //Gmail Icone
                    picture = ModuleC.pathPitcure + "Gmail.png";
                    break;
                case 13: //Avaya icone
                    picture = ModuleC.pathPitcure + "Avaya.png";
                    break;
                case 14: //Salesforce Icone
                    picture = ModuleC.pathPitcure + "Salesforce.png";
                    break;
                case 15: //Odigo Icone
                    picture = ModuleC.pathPitcure + "Odigo.png";
                    break;
                case 16: //Robusto Icone
                    picture = ModuleC.pathPitcure + "Robusto.png";
                    break;

            }

            if (ModuleC.Status_Incident == "1")
            {
                signature = "Nouveau";
            }
            else if (ModuleC.Status_Incident == "4")
            {
                signature = "Clôture";
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
        }
    }
}
