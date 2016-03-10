using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPOOHLT
{
    class AppConfigReader
    {
        public static string CONN_STRING
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["GPOOHLTConnectionString"].ConnectionString; }
        }

        //public static string  GetDecriptedSconnectionString(string decriptedSconnectionString)
        //{
        //    string de_password = decriptedSconnectionString.Substring(decriptedSconnectionString.IndexOf("Password=") + "Password=".Length);
        //    de_password = de_password.Substring(0, de_password.LastIndexOf(";"));

        //    string en_password = Utils.Classes.DataCryptography.Singleton.Decrypt(de_password).ToString();

        //    string encriptedSconnectionString = decriptedSconnectionString.Replace(de_password, en_password);
        //    return encriptedSconnectionString;
        //}

        //public static string CONN_STRING
        //{

        //    get
        //    {
        //        string decriptedSconnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GPOOHLTConnectionString"].ConnectionString;
        //        string encriptedSconnectionString = AppConfigReader.GetDecriptedSconnectionString(decriptedSconnectionString);
        //        return encriptedSconnectionString;
        //    }
        //}

        public static string RTCSeverIP
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["RTCSeverIP"]; }
        }

        public static string RTCServerPort
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["RTCServerPort"]; }
        }

        public static string SystemNameAbrivation
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["SystemNameAbrivation"]; }
        }

        public static string SystemName
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["SystemName"]; }
        }

        public static string OrganizationNameAbrivation
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["OrganizationNameAbrivation"]; }
        }

        public static string OrganizationName
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["OrganizationName"]; }
        }

        public static string OrganizationAddress
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["OrganizationAddress"]; }
        }

        public static string OrganizationLogo
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["OrganizationLogo"]; }
        }
    }
}
