using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Classes
{
    public enum Roles 
    { 
        CallTaker,
        Nurse, 
        Doctor,
        None
    }

    public enum CallTypes
    {
        DoctorAdvice = 1,
        NurseAdvice = 2,
        TreatmentCentre = 3,
        HomeVisit = 4,
        CallAnswering = 5
    }

    public enum AddressType
    {
	    Home=1,
	    Current=2
    }

    public enum AppointmentStatus 
    {
        Arrived = 2,
        Triage = 4,
        UnderTreatment = 5,
        SentToTreatmentCentre = 6,
        SentToDoctor = 7,
        Closed = 8,
        WaitingToDespatch = 9,
        NotArrived = 10,
        Cancelled = 11,
        VerbalDespatched = 12,
        Fax = 13
    }

    public class GlobalData
    {
        public static Roles ObjRoles { get; set; }
        public static AppointmentStatus ObjAppointmentStatus { get; set; }
        public static string AppName { get; set; }
        public static string GPOOHLTConnectionString { get; set; }
        public static int UserID { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static decimal  Delay { get; set; }
        public static string RTCSeverIP { get; set; }
        public static int RTCServerPort { get; set; }
        public static string GetDate(string appTime)
        {
            DateTime appDate = Convert.ToDateTime(appTime); 
            appDate = appDate.AddYears(DateTime.Now.Year - appDate.Year);
            appDate = appDate.AddMonths(DateTime.Now.Month - appDate.Month);
            appDate = appDate.AddDays(DateTime.Now.Day - appDate.Day);
            return appDate.ToString();
        }
    }
}
