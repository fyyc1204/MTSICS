using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
    public class RegNo_Value
    {
        private RegNo_Value_PK id;

        public virtual RegNo_Value_PK Id
        {
            get { return id; }
            set { id = value; }
        }

        private decimal projectValue;

        public virtual decimal ProjectValue
        {
            get { return projectValue; }
            set { projectValue = value; }
        }
        private DateTime scaleDateTime;

        public virtual DateTime ScaleDateTime
        {
            get { return scaleDateTime; }
            set { scaleDateTime = value; }
        }



    }
}
