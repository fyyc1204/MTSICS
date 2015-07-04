using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTSICS_VO;
using NHibernate;
using System.Collections;
namespace MTSICS_DAO
{
    public class Batch_DAO
    {
        /// <summary>
        /// 更新Batch
        /// </summary>
        /// <param name="VO"></param>
        public static void Update(EY_Batch VO)
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
        public static void Save(EY_Batch VO)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                session.Save(VO);
                session.Flush();
            }
        }
        public static void Delete(EY_Batch VO)
        {
            using (ISession session = SessionHelper.GetSession())
            {

                session.Delete(VO);
                session.Flush();
            }
        }

        /// <summary>
        /// 按主键查询一个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static EY_Batch LoadOne(object id)
        {
            EY_Batch ret = null;
            using (ISession session = SessionHelper.GetSession())
            {
                ret = session.Load<EY_Batch>(id);
            }
            return ret;
        }

        /// <summary>
        /// 查询所有VO
        /// </summary>
        /// <returns></returns>
        public static IList<EY_Batch> LoadAll()
        {
            IList<EY_Batch> ret = null;
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from Batch as a");
                ret = query.List<EY_Batch>();
            }
            return ret;
        }

        /// <summary>
        /// 按照时间区间进行查询
        /// </summary>
        /// <param name="less"></param>
        /// <param name="great"></param>
        /// <returns></returns>
        public static IList<EY_Batch> LoadByTime(DateTime less, DateTime great)
        {
            IList<EY_Batch> ret = null;
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from EY_Batch as a where a.CreateTime>=:less and a.CreateTime<=:great");
                query.SetDateTime("less", less);
                query.SetDateTime("great", great);
                ret = query.List<EY_Batch>();
            }

            return ret;
        }
        public static IList create4Excel(DateTime less, DateTime great) 
        {
            IList ret = null;
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from Batch as a ,RegNo_DI as b where a.CreateTime>=:less and a.CreateTime<=:great and a.BatchNo=b.Batch");
                query.SetDateTime("less", less);
                query.SetDateTime("great", great);
                ret = query.List();
            }

            return ret;
        }
        ////按照指定日期查询
        //public static IList<Batch> LoadByTime1(String date)
        //{
        //    IList<Batch> ret = null;
        //    using (ISession session = SessionHelper.GetSession())
        //    {
        //        IQuery query = session.CreateQuery("from Batch as a where a.CreateTime like:date ");

        //        query.SetParameter("date", string.Format("%{0}%", date));
        //        ret = query.List<Batch>();
        //    }

        //    return ret;
        //}

        /// <summary>
        /// 自动生成批次编号  批号编码规则YF+年月+流水号(3) YF20131201000
        /// </summary>
        /// <returns></returns>
        public static String GetBatchNo()
        {
            String retBatchNo = null;
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("select max(a.BatchNo) from Batch as a");
                String maxId = query.UniqueResult<String>();
                if (maxId == "" || maxId == null)
                {
                    return "YF" + DateTime.Now.ToString("yyyyMMdd") + "001";
                }

                String seqNo="";
                if (DateTime.Now.ToString("yyyyMMdd") == maxId.Substring(2, 8))
                {
                    seqNo = maxId.Substring(10, 3);
                    int temp = Convert.ToInt32(seqNo) + 1;
                    if (temp.ToString().Length == 1)
                    {
                        seqNo = "00" + temp.ToString();
                    }
                    else if (temp.ToString().Length == 2)
                    {
                        seqNo = "0" + temp.ToString();
                    }
                    else
                    {
                        seqNo = temp.ToString();
                    }
                    retBatchNo = maxId.Substring(0, 10) + seqNo;
                }
                else
                {
                  retBatchNo=  "YF" + DateTime.Now.ToString("yyyyMMdd") + "001";
                }
                
            }
            return retBatchNo;
        }
    }
}
