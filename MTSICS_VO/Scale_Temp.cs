using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
    public class Scale_Temp
    {
        private Scale_Temp_PK id;

        public virtual Scale_Temp_PK Id
        {
            get { return id; }
            set { id = value; }
        }

        private int indexNo;

        public virtual int IndexNo
        {
            get { return indexNo; }
            set { indexNo = value; }
        }
        private decimal scaleNum;

        public virtual decimal ScaleNum
        {
            get { return scaleNum; }
            set { scaleNum = value; }
        }
        private DateTime scaleDateTime;

        public virtual DateTime ScaleDateTime
        {
            get { return scaleDateTime; }
            set { scaleDateTime = value; }
        }
    }
}
