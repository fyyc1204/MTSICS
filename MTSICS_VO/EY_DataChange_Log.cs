using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
   public class EY_DataChange_Log
    {

       private DateTime date;
       public virtual DateTime Date
       {
           get { return date; }
           set { date = value; }      
       }


       private String regno;
       public virtual String RegNo
       {
           get { return regno; }
           set { regno = value; }
       }
       private String checkNo;
       public virtual String CheckNo
       {
           get { return checkNo; }
           set { checkNo = value; }
       }
       private String matrname;
       public virtual String MatrName
       {
           get { return matrname; }
           set { matrname = value; }
       }
       private String rowname;
       public virtual String RowName
       {
           get { return rowname; }
           set { rowname = value; }
       }

       private String oldvalue;
       public virtual String OldValue
       {
           get { return oldvalue; }
           set { oldvalue = value; }
       }

       private String newvalue;
       public virtual String NewValue
       {
           get { return newvalue; }
           set { newvalue = value; }
       }
       private String operator1;
       public virtual String Operator1
       {
           get { return operator1; }
           set { operator1 = value; }
       }



    }


}
