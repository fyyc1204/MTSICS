using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTSICS_VO;
using NHibernate;
using System.Collections;

namespace MTSICS_DAO
{
    public  class EY_MatrTable_DAO
    {
        public static void Update(EY_MatrTable VO)
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
        public static void Save(EY_MatrTable VO)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                session.Save(VO);
                session.Flush();
            }
        }
        public static void Delete(EY_MatrTable VO)
        {
            using (ISession session = SessionHelper.GetSession())
            {

                session.Delete(VO);
                session.Flush();
            }
        }

        //获取水分调整值
        public static IList<EY_MatrTable> GetMatrTableWaterValue(String matrname)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from EY_MatrTable where matrName=:matrName");
                query.SetString("matrName", matrname);
                return query.List<EY_MatrTable>();
            }
        }
       
        //获取料名

        public static IList<EY_MatrTable> GetMatrName(String matrNo)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from EY_MatrTable where matrNo=:matrNo order by matrNo");
                query.SetString("matrNo", matrNo);
                return query.List<EY_MatrTable>();
            }
        }


        /// <summary>
        /// 按主键查询一个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static EY_MatrTable LoadOne(object id)
        {
            EY_MatrTable ret = null;
            using (ISession session = SessionHelper.GetSession())
            {
                ret = session.Load<EY_MatrTable>(id);
            }
            return ret;
        }

      
       

     







    }
}
