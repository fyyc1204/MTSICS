using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using MTSICS_VO;
using System.Windows.Forms;
namespace MTSICS_DAO
{
    public static class EY_Scale_Value_Dao
    {
        public static IList<EY_Scale_Value> Create(EY_Scale_Value EY_Scale_Value_Vo)
        {

            using (ISession session = SessionHelper.GetSession())
            {


                session.Save(EY_Scale_Value_Vo);
                session.Flush();
            }
            return null;
        }
        public static EY_Scale_Value LoadOne(object id)
        {
            EY_Scale_Value ret = null;
            using (ISession session = SessionHelper.GetSession())
            {
                ret = session.Load<EY_Scale_Value>(id);
            }
            return ret;
        }
        public static IList<EY_Scale_Value> GetRegNo(EY_Scale_Value_PK id)//查询用
        {
            using (ISession session = SessionHelper.GetSession())
            {


                IQuery query = session.CreateQuery("from EY_Scale_Value  as a  where  a.Id = :s and a.FinalValue !='' and a.FinalValue != 0");
                query.SetParameter("s", id);
                return query.List<EY_Scale_Value>();
            }
        }
        public static IList<EY_Scale_Value> GetRegNo1(EY_Scale_Value_PK id)//插入资料用
        {
            using (ISession session = SessionHelper.GetSession())
            {


                IQuery query = session.CreateQuery("from EY_Scale_Value as a  where  a.Id = :s ");
                query.SetParameter("s", id);
                return query.List<EY_Scale_Value>();
            }
        }
        public static IList<EY_Scale_Value> GetAverage(EY_Scale_Value id)//求平行样的平均数用
        {
            using (ISession session = SessionHelper.GetSession())
            {


                IQuery query = session.CreateQuery(" from EY_Scale_Value  as a  where  a.Id = :s ");
                query.SetParameter("s", id);
                return query.List<EY_Scale_Value>();
            }
        }
        //根据主键查
        public static IList<EY_Scale_Value> GetRegNobyPK(String regno, String checkno)
        {
            using (ISession session = SessionHelper.GetSession())
            {

                IQuery query = session.CreateQuery("from EY_Scale_Value as a  where  a.ID.RegNo=:s and a.ID.CheckNo=:t order by a.ID.RegNo");
                query.SetString("s", regno);
                query.SetString("t", checkno);

                return query.List<EY_Scale_Value>();
            }
        }

        //查询未上传数据
        public static IList<EY_Scale_Value> GetScaleToErp(string flag)
        {
            using (ISession session = SessionHelper.GetSession())
            {

                IQuery query = session.CreateQuery("from EY_Scale_Value as a  where  a.ToErpFlag = :f");
                query.SetString("f", flag);
                return query.List<EY_Scale_Value>();
            }
        }


        //删除
        public static void Delete(EY_Scale_Value VO)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                session.Delete(VO);
                session.Flush();
            }
        }
        public static IList<EY_Scale_Value> Update(EY_Scale_Value EY_Scale_Value_Vo)
        {

            using (ISession session = SessionHelper.GetSession())
            {


                session.Update(EY_Scale_Value_Vo);
                session.Flush();
            }
            return null;
        }
        public  static IList<EY_Scale_Value> Update_OnlyValue(EY_Scale_Value_PK spl,decimal finalvalue)
        {

            using (ISession session = SessionHelper.GetSession())
            {

                IQuery query = session.CreateQuery("update  EY_Scale_Value as a set a.FinalValue=:t where  a.id= :s");
                query.SetParameter("s", spl);
                query.SetDecimal("t", finalvalue);
                query.ExecuteUpdate();
                

               // return query.List<EY_Scale_Value>();
                return null;
            }
        }


        public static IList<EY_Scale_Value> SearchByTime(DateTime less, DateTime great,int N)
        {
            IList<EY_Scale_Value> ret = null;
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from EY_Scale_Value as a where a.Date>=:less and a.Date<=:great and a.ToErpFlag=:N");
                query.SetDateTime("less", less);
                query.SetDateTime("great", great);
                query.SetInt32("N", N);
                ret = query.List<EY_Scale_Value>();
            }

            return ret;
        
        
        
        }

        public static  IList<EY_Scale_Value> Update_Status(EY_Scale_Value_PK spl  , string st)
        {

            using (ISession session = SessionHelper.GetSession())
            {

                IQuery query = session.CreateQuery("update  EY_Scale_Value as a set a.Status = :t where a.Id= :s and a.FinalValue !='' and a.FinalValue != 0");
                query.SetParameter("s", spl);
                query.SetString("t", st);
                //query.SetString("k", cell);
                query.ExecuteUpdate();


                // return query.List<EY_Scale_Value>();
               return null;
            }
        }

        public static void Update_ToErpFlag(EY_Scale_Value_PK spl, string st)
        {

            using (ISession session = SessionHelper.GetSession())
            {

                IQuery query = session.CreateQuery("update  EY_Scale_Value as a set a.ToErpFlag = :t where a.Id= :s ");
                query.SetParameter("s", spl);
                query.SetString("t", st);
                //query.SetString("k", cell);
                query.ExecuteUpdate();
            }
        }

    }
}
