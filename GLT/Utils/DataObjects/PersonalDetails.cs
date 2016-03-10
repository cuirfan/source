using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.DataObjects
{
    public class PersonalDetails
    {
        public string Age { get; set; }
        public string strDOB { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string CallBackNumber { get; set; }
        public string MobileNumber { get; set; }
        public string OtherPhoneNumber { get; set; }
        public string OtherNumber { get; set; }
        public int PatientID { get; set; }
        public string HomeAddress { get; set; }
        public string HomeAddressPostcode { get; set; }
        public string CallTakerSymptoms { get; set; }
        public string Priority { get; set; }
        public string CaseType { get; set; }
        public string NurseSymptoms { get; set; }
        public string TreatmentCentre { get; set; }
        public string NurseOutcome { get; set; }
        public string DoctorOutcome { get; set; }
        public string Allergy { get; set; }
        //public string FileVersion { get; set; }
    }
}
