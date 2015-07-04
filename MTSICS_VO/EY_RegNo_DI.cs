using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MTSICS_VO
{
        public class  EY_RegNo_DI
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

            private String matrNo;

            public virtual String MatrNo
            {
                get { return matrNo; }
                set { matrNo = value; }
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
                return new String[] { this.regNo,this.checkNo,this.status,this.cName,this.revDateTime.ToString()};
                
            }

            private EY_Batch batch;

            public virtual EY_Batch Batch
            {
                get { return batch; }
                set { batch = value; }
            }


        }
}

