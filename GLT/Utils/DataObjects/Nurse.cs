using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.DataObjects
{
    public class Nurse : PersonalDetails
    {
        public string Sending { get; set; }
        public string AppointmentTime { get; set; }
    }
}
