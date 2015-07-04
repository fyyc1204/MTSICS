using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
    public class EY_Scale_Value
    
    {
        private EY_Scale_Value_PK id;

        public virtual EY_Scale_Value_PK Id
        {
            get { return id; }
            set { id = value; }
        }
        private Decimal panwgt;
        public virtual Decimal PanWgt
        {
            get { return panwgt; }
            set { panwgt = value; }
        }
        private Decimal yangwgt;
        public virtual Decimal YangWgt
        {
            get { return yangwgt; }
            set { yangwgt = value; }
        }
        private Decimal hengwgt1;
        public virtual Decimal HengWgt1
        {
            get { return hengwgt1; } 
            set { hengwgt1 = value; }
        }
        private Decimal hengwgt2;
        public virtual Decimal HengWgt2
        {
            get { return hengwgt2; }
            set { hengwgt2 = value; }
        }
        private Decimal hengwgt3;
        public virtual Decimal HengWgt3
        {
            get { return hengwgt3; }
            set { hengwgt3 = value; }
        }
        private Decimal hengwgt4;
        public virtual Decimal HengWgt4
        {
            get { return hengwgt4; }
            set { hengwgt4 = value; }
        }
        private Decimal finalvalue;
        public virtual Decimal FinalValue
        {
            get { return finalvalue; }
            set { finalvalue = value; }
        }
        private String operator1;

        public virtual String Operator1
        {
            get { return operator1; }
            set { operator1 = value; }
        }
        private String operator2;
        public virtual String Operator2
        {
            get { return operator2; }
            set { operator2 = value; }
        }

        private int flag;
        public virtual int Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        private String toErpflag;
        public virtual String ToErpFlag
        {
            get { return toErpflag; }
            set { toErpflag = value; }
        }

        private String mem1;
        public virtual String Mem1
        {
            get { return mem1; }
            set { mem1 = value; }
        }

       

        private String matrname;
        public virtual String Matrname
        {
            get { return matrname; }
            set { matrname = value; }
        }

        private DateTime date;
        public virtual DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private String status;
        public virtual String Status
        {
            get { return status; }
            set { status = value; }
        }

        

       

    }
}

   
