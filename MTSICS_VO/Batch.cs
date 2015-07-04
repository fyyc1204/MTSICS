using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
    public class Batch 
    {
        private String batchNo;

        public virtual String BatchNo
        {
            get { return batchNo; }
            set { batchNo = value; }
        }
        private DateTime createTime;

        public virtual DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
        private IList<RegNo_DI> batchChild=new List<RegNo_DI>();

        public virtual IList<RegNo_DI> BatchChild
        {
            get { return batchChild; }
            set { batchChild = value; }
        }


    }
}
