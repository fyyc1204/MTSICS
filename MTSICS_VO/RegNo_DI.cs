using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
    public class RegNo_DI
    {
        private String regNo;

        public virtual String RegNo
        {
            get { return regNo; }
            set { regNo = value; }
        }
        private DateTime revDateTime;

        public virtual DateTime RevDateTime
        {
            get { return revDateTime; }
            set { revDateTime = value; }
        }
        private String status;

        public virtual String Status
        {
            get { return status; }
            set { status = value; }
        }
        private String cName;

        public virtual String CName
        {
            get { return cName; }
            set { cName = value; }
        }

        public virtual String[] toArray()
        {
            return new String[] { this.regNo,this.revDateTime.ToString(), this.status, this.cName};
        }

        private EY_Batch batch;

        public virtual EY_Batch Batch
        {
            get { return batch; }
            set { batch = value; }
        }


    }
}
