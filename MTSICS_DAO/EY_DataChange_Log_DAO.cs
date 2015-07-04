using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTSICS_VO;
using NHibernate;
using System.Collections;

namespace MTSICS_DAO
{
    public class EY_DataChange_Log_DAO
    {



         /// <summary>
        /// 更新Batch
        /// </summary>
        /// <param name="VO"></param>
        public static void Update(EY_DataChange_Log VO)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                session.Update(VO);
                session.Flush();
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="VO"></param>
        public static void Save(EY_DataChange_Log VO)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                session.Save(VO);
                session.Flush();
            }
        }


        public static void Delete(EY_DataChange_Log VO)
        {
            using (ISession session = SessionHelper.GetSession())
            {

                session.Delete(VO);
                session.Flush();
            }
        }

        //public static IList<EY_DataChange_Log> Writelog(String matrname)
        //{
        //    using (ISession session = SessionHelper.GetSession())
        //    {
        //        IQuery query = session.CreateQuery("update EY_MatrTable set  where matrName=:matrName order by matrNo");
        //        query.SetString("matrName", matrname);
        //        return query.List<EY_MatrTable>();
        //    }
        //}
       

   
    }




}

