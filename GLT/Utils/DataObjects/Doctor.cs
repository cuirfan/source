using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.DataObjects
{
    public class Doctor : Nurse
    {
        public string Referral { get; set; }
        public string Medication { get; set; }
        
    }
}
