using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MTSICS_VO
{
    public class Scale_Value
    {
        private Scale_Value_PK id;

        public virtual Scale_Value_PK Id
        {
            get { return id; }
            set { id = value; }
        }
        private Decimal wgtOne;
        public virtual Decimal WgtOne
        {
            get { return wgtOne; }
            set { wgtOne = value; }
        }
        private Decimal wgtTwo;
        public virtual Decimal WgtTwo
        {
            get { return wgtTwo; }
            set { wgtTwo = value; }
        }
        private Decimal wgtThree;
        public virtual Decimal WgtThree
        {
            get { return wgtThree; } 
            set { wgtThree = value; }
        }
        private Decimal wgtFinal;
        public virtual Decimal WgtFinal
        {
            get { return wgtFinal; }
            set { wgtFinal = value; }
        }
        private Decimal wgtFinalTwo;
        public virtual Decimal WgtFinalTwo
        {
            get { return wgtFinalTwo; }
            set { wgtFinalTwo = value; }
        }
        private Decimal wgtFinalThree;
        public virtual Decimal WgtFinalThree
        {
            get { return wgtFinalThree; }
            set { wgtFinalThree = value; }
        }
        private Decimal wgtFinalFour;
        public virtual Decimal WgtFinalFour
        {
            get { return wgtFinalFour; }
            set { wgtFinalFour = value; }
        }
        private String createPer;

        public virtual String CreatePer
        {
            get { return createPer; }
            set { createPer = value; }
        }
      
    }
}
