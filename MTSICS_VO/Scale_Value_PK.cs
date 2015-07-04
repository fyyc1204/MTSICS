using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
    public class Scale_Value_PK
    {
         private String regNo;

        public virtual String RegNo
        {
            get { return regNo; }
            set { regNo = value; }
        }
        private String eleNo;

        public virtual String EleNo
        {
            get { return eleNo; }
            set { eleNo = value; }
        }

        public override bool Equals(object obj)
        {
            var temp = obj as Scale_Value_PK;
            if (temp == null)
            {
                return false;
            }

            return temp.RegNo == this.regNo && temp.eleNo == this.eleNo;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    
}
