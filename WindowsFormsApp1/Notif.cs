using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;

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
            Console.WriteLine("New-BurntToastNotification -Text " + titre + ", \"" + commentaire + "\" -AppLogo \"" + picture + "\"");
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
                       
            using (WebClient client = new WebClient())
            {
                Console.WriteLine(ModuleC.urlPicture + compenent);
                Console.WriteLine(ModuleC.pathPitcure + compenent + ".png");
                client.DownloadFile(ModuleC.urlPicture + compenent, ModuleC.pathRoamingFile + "Pictures\\" + compenent+".png");
            }

            if (compenent != 10)
            {
                picture = ModuleC.pathRoamingFile + "Pictures\\" + compenent + ".png";
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

            Console.WriteLine("New-BurntToastNotification -Text \"" + title + "\", \"" + content + "\" -AppLogo \"" + picture + "\"");
            ps.AddScript("New-BurntToastNotification -Text \"" + title + "\", \"" + content + "\" -AppLogo \"" + picture + "\"");
            ps.Invoke();

            rs.Close();
        }
    }
}
