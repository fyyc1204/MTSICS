using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
    public class EY_Scale_Value_PK
    {
        private String regNo;

        public virtual String RegNo
        {
            get { return regNo; }
            set { regNo = value; }
        }
        private String checkNo;

        public virtual String CheckNo
        {
            get { return checkNo; }
            set { checkNo = value; }
        }

        public override bool Equals(object obj)
        {
            var temp = obj as EY_Scale_Value_PK;
            if (temp == null)
            {
                return false;
            }

            return temp.RegNo == this.regNo && temp.CheckNo == this.checkNo;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
