using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Reflection;
namespace MTSICS_DAO
{
    public class SessionHelper
    {
        public static readonly object synObj = new object();
        private static ISessionFactory sessionFactory;

        public static ISessionFactory Factory
        {
            get
            {
                if (sessionFactory == null)
                {
                    lock (synObj)
                    {
                        if (sessionFactory == null)
                        {
                            try
                            {
                                NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
                                cfg.Configure();
                                sessionFactory = cfg.BuildSessionFactory();
                            }
                             catch (Exception ex)
                            {
                                Exception ee = ex.InnerException;
                            }
                        }
                    }
                }
                return sessionFactory;
            }
        }

        public static ISession GetSession()
        {
            return Factory.OpenSession();
        }
        public static void CloseSession()
        {
             Factory.Close();
        }
    }
}
