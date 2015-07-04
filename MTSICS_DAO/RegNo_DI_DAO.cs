using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTSICS_VO;
using NHibernate;
using System.Windows.Forms;
namespace MTSICS_DAO
{
    public static class RegNo_DI_DAO
    {
        /// <summary>
        /// 根据状态获取从DI接受的案件接收编号
        /// </summary>
        /// <param name="status">状态 N=未被化验 Y=已经被化验</param>
        /// <returns></returns>
        public static IList<RegNo_DI> GetRegNo(String status)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from RegNo_DI as a where a.Status=:s order by a.RegNo");
                query.SetString("s", status);
                return query.List<RegNo_DI>();
            }
        }
        public static IList<RegNo_DI> GetRegNoCount(String reg)//模糊查询用
        {
            using (ISession session = SessionHelper.GetSession()) 
            {


                IQuery query = session.CreateQuery("from RegNo_DI  as a  where  a.RegNo like :s and status='Y'");
                query.SetParameter("s","%"+ reg+"%");
                return query.List<RegNo_DI>();
            }
        }
        public static  void   delete(RegNo_DI reg)//放弃批号时，删除平行样用
        {
            using (ISession session = SessionHelper.GetSession())
            {
               

                session.Delete(reg);
               session.Flush();
               
              
            }
        }

        //将数据从DI接收端查询出
        public static IList<tbdipdi> GetRegNoInPDI(String status)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from tbdipdi as a where a.Status=:s order by a.Data");
                query.SetString("s", status);
                return query.List<tbdipdi>();
            }
        }
        //从PDI表查处数据塞入新表
        public static void Save( RegNo_DI tem)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                session.Save(tem);
                session.Flush();
                session.Close();
            }
        }
        //更新PDI状态
        public static void UpdatePDI(tbdipdi tem)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                session.Update(tem);
                session.Flush();
            }
        }
        public static IList<RegNo_DI> GetRegNo1(String batchNo)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                IQuery query = session.CreateQuery("from RegNo_DI where batchNo=:batchNo order by RegNo");
                query.SetString("batchNo", batchNo);
                return query.List<RegNo_DI>();
            }
        }
        //查找称重资料以显示
        public static IList<Scale_Value> GetRegNo2(String regNo)
        {
            using (ISession session = SessionHelper.GetSession())
            {
                
                IQuery query = session.CreateQuery(" from Scale_Value where regNo=:regNo order by RegNo,EleNo");
                query.SetString("regNo", regNo);
                
                return query.List<Scale_Value>();
            }
        }
        public static IList<RegNo_DI> GetRegNo3(String regNo)
        {
            using (ISession session = SessionHelper.GetSession())
            {

                IQuery query = session.CreateQuery(" from RegNo_DI where regNo=:regNo order by RegNo");
                query.SetString("regNo", regNo);

                return query.List<RegNo_DI>();
            }
        }

        /// <summary>
        /// 批量更新案件编号资料
        /// </summary>
        /// <param name="regNo_Dis">案件资料 </param>
        public static void UpdateBatch(IList<RegNo_DI> regNo_Dis)
        {
            using (ISession session = SessionHelper.GetSession())
            {
          
              /*  using (ITransaction transaction = session.BeginTransaction())
                {
          
                    try
                    {
                        foreach (RegNo_DI rd in regNo_Dis)
                        {
                            MessageBox.Show(rd.Status);
                            session.Update(rd);
                            
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
              */
                foreach (RegNo_DI rd in regNo_Dis)
                {
                   
                    session.Update(rd);
                    session.Flush();

                }
            }
        }


        /// <summary>
        /// 批量更新案件资料的状态
        /// </summary>
        /// <param name="regno_Dis">案件资料</param>
        /// <param name="status">状态值</param>
        public static void UpdateStatusBatch(IList<RegNo_DI> regno_Dis, String status)
        {
            int i = 0;
            foreach (RegNo_DI rd in regno_Dis)
            {
                
                rd.Status = status;
                rd.Batch=null;
                rd.CName = regno_Dis[i].CName;
                i++;
                
                
             
                
            }

            UpdateBatch(regno_Dis);
        }

    }
}
