using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTSICS_VO;
using NHibernate;
using System.Collections;

namespace MTSICS_DAO
{
    public class tbdipdo_DAO
    {
        public static void TBDIPDOSAVE(tbdipdo vo)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                
                session.Save(vo);
                session.Flush();
                session.Close();

            }
        }





    }
}
