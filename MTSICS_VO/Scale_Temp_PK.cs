using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
    public class Scale_Temp_PK
    {
        private String regNo;

        public virtual String RegNo
        {
            get { return regNo; }
            set { regNo = value; }
        }
        private String projectNo;

        public virtual String ProjectNo
        {
            get { return projectNo; }
            set { projectNo = value; }
        }

        public override bool Equals(object obj)
        {
            var temp = obj as Scale_Temp_PK;
            if (temp == null)
            {
                return false;
            }
            return temp.RegNo == this.regNo && temp.ProjectNo == this.projectNo;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
