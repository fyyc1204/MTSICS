using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTSICS_VO;
using NHibernate;

namespace MTSICS_DAO
{
    public class Login_DAO
    {
        public static IList<Login> CheckOut(String userno, String password)
        {
            IList<Login> ret = null;
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from Login as a where a.UserNo=:userno and a.PassWord=:password");
                query.SetString("userno", userno);
                query.SetString("password", password);
                ret = query.List<Login>();
            }

            return ret;
        }
        public static IList<Login> CheckOutByFlag()
        {
            IList<Login> ret = null;
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from Login as a where a.Flag=1");
               
                ret = query.List<Login>();
            }

            return ret;
        }
        public static void UpdateFlag(Login vo)
        {
            
            using (ISession session = SessionHelper.GetSession())
            {
                session.Update(vo);
                session.Flush();
            }

            
        }

        public static IList<Login> SelectUserName()
        {
            IList<Login> ret = null;
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from Login ");

                ret = query.List<Login>();
            }
                        
            return ret;
            
        }




        public static void ResetFlag(Login vo) 
        {

            using (ISession session = SessionHelper.GetSession())
            {
                session.Update(vo);
                session.Flush();
            }


        }
    }
}
