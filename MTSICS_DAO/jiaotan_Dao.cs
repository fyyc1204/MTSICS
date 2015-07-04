using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTSICS_VO;
using NHibernate;

namespace MTSICS_DAO
{
    public static class jiaotan_Dao
    {
        public static IList<jiaotan> GetPnType(String pn) 
        { 
            using (ISession session = SessionHelper.GetSession())
            {


                IQuery query = session.CreateQuery("from jiaotan  as a  where  a.Pn=:s ");
                query.SetParameter("s", pn);
                return query.List<jiaotan>();
            }
        }
    }
}
