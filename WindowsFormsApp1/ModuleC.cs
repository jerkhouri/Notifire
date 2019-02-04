using System;
using System.IO;
using System.Collections.Generic;using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApp1
{
    public class ModuleC //Module utilisé par les autres classes!
    {
        //VARAIBLE Setup
        public static string pathSetupFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Notifire\\Config.json");
        public static string cache = null;
        public static string SetupDone = null;
        public static JArray listYes = null;
        public static string firstLaunch = null;

        //VARIABLE GLOBALES SUR L'INCIDENT
        public static string Id_Incident = null;
        public static string Name_Incident = null;
        public static string Message_Incident = null;
        public static DateTime Date_Create_Incident;
        public static string Component_id_Incident = null;
        public static string Compenent_Name = null;
        public static string Status_Incident = null;

        //Variable pour incident
        public static string navUrl;
        public static string nameUrl;
        public static string urlCachet = "http://192.168.253.10/";
        public static string urlNewIncident = urlCachet+"api/v1/incidents?status=1&per_page=1&sort=id&order=desc";
        public static string urlClotIncident = urlCachet + "api/v1/incidents?status=4&per_page=1&sort=id&order=desc";
        public static string pathPitcure = "C:\\Program Files\\AramisAuto Manufacture\\Notifire\\Ressources\\";

        public static string GetUrlCompenent (string compenentID)
        {
           return urlCachet + "api/v1/components?id="+ compenentID + "&per_page=1";
        }
    }
}
