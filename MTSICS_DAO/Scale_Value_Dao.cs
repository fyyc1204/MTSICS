using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using MTSICS_VO;
using System.Windows.Forms;

namespace MTSICS_DAO
{
    public static class Scale_Value_Dao

    {
        public static IList<Scale_Value> Create(Scale_Value Scale_Value_Vo){

            using (ISession session = SessionHelper.GetSession())
            {


                session.Save(Scale_Value_Vo);
                session.Flush();
            }
            return null;
        }
        public static Scale_Value LoadOne(object id)
        {
            Scale_Value ret = null;
            using (ISession session = SessionHelper.GetSession())
            {
                ret = session.Load<Scale_Value>(id);
            }
            return ret;
        }
        public static IList<Scale_Value> GetRegNo(Scale_Value_PK id)//查询用
        {
            using (ISession session = SessionHelper.GetSession())
            {
                
                
                IQuery query = session.CreateQuery("from Scale_Value  as a  where  a.Id = :s ");
                query.SetParameter("s", id);
                return query.List<Scale_Value>();
            }
        }
        public static IList<Scale_Value> GetRegNo1(Scale_Value_PK id)//插入资料用
        {
            using (ISession session = SessionHelper.GetSession())
            {


                IQuery query = session.CreateQuery("from Scale_Value  as a  where  a.Id = :s ");
                query.SetParameter("s", id);
                return query.List<Scale_Value>();
            }
        }
        public static IList<Scale_Value> GetAverage(Scale_Value_PK id)//求平行样的平均数用
        {
            using (ISession session = SessionHelper.GetSession())
            {


                IQuery query = session.CreateQuery(" from Scale_Value  as a  where  a.Id = :s ");
                query.SetParameter("s",id);
                return query.List<Scale_Value>();
            }
        }
        //根据主键查
        public static IList<Scale_Value> GetRegNobyPK(String regno,String eleno)
        {
            using (ISession session = SessionHelper.GetSession())
            {

                IQuery query = session.CreateQuery("from Scale_Value  as a  where  a.RegNo=:s  and a.EleNo:t order by a.RegNo");
                query.SetString("s", regno);
                query.SetString("t", eleno);

                return query.List<Scale_Value>();
            }
        }
        //删除
        public static void Delete(Scale_Value VO)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                session.Delete(VO);
                session.Flush();
            }
        }
        public static IList<Scale_Value> Update(Scale_Value Scale_Value_Vo)
        {

            using (ISession session = SessionHelper.GetSession())
            {


                session.Update(Scale_Value_Vo);
                session.Flush();
            }
            return null;
        }

     
    }
}
