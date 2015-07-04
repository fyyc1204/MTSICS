using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
    public class tbdipdo
    {
        private Decimal TIMESTAMP;
        public virtual Decimal TimeStamp
        {
            get { return TIMESTAMP; }
            set { TIMESTAMP = value; }
        }
        private Decimal SERIALNO;
        public virtual Decimal SerialNo
        {
            get { return SERIALNO; }
            set { SERIALNO = value; }
        }
        private String QUEUEID;
        public virtual String QueueId
        {
            get { return QUEUEID; }
            set { QUEUEID = value; }
        }
        private String HEADER;
        public virtual String Header
        {
            get { return HEADER; }
            set { HEADER = value; }
        }
        private String DATA;
        public virtual String Data
        {
            get { return DATA; }
            set { DATA = value; }
        }
        private String STATUS;
        public virtual String Status
        {
            get { return STATUS; }
            set { STATUS = value; }
        }
        private String PROCESSTIME;
        public virtual String ProcessTime
        {
            get { return PROCESSTIME; }
            set { PROCESSTIME = value; }
        }
        private String DESCRIPTION;
        public virtual String Description
        {
            get { return DESCRIPTION; }
            set { DESCRIPTION = value; }
        }
         


    }
}
