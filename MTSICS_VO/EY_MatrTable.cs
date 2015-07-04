using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
    public class EY_MatrTable
    {
        private String matrNo;

        public virtual String MatrNo
        {
            get { return matrNo; }
            set { matrNo = value; }
        }
        private String matrName;

        public virtual String MatrName
        {
            get { return matrName; }
            set { matrName = value; }
        }
        private int water_range_less;

        public virtual int Water_Range_Less
        {
            get { return water_range_less; }
            set { water_range_less = value; }
        }
        private int water_range_grater;

        public virtual int Water_Range_Grater
        {
            get { return water_range_grater; }
            set { water_range_grater = value; }
        }
        private Decimal water_value;

        public virtual Decimal Water_Value
        {
            get { return water_value; }
            set { water_value = value; }
        }
        private int id;

        public virtual int Id 
        {
            get { return id; }
            set { id = value; }
        }






    }
}
