using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
     public class tbdipdi
    {
        private Decimal timeStamp;
        public virtual Decimal TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }
        private Decimal serialNo;
        public virtual Decimal SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }
        private String queueId;
        public virtual String QueueId
        {
            get { return queueId; }
            set { queueId = value; }
        }
        private String header;
        public virtual String Header
        {
            get { return header; }
            set { header = value; }
        }
        private String data;
        public virtual String Data
        {
            get { return data; }
            set { data = value; }
        }
        private String status;
        public virtual String Status
        {
            get { return status; }
            set { status = value; }
        }
        private String processTime;
        public virtual String ProcessTime
        {
            get { return processTime; }
            set { processTime = value; }
        }
        private String description;
        public virtual String Description
        {
            get { return description; }
            set { description = value; }
        }
        
    }
}
