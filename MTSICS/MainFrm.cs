    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using MTSICS_DAO;
    using MTSICS_VO;
    using System.IO.Ports;
    using NHibernate;
    using MT_SICS;
    using DevComponents.DotNetBar.Controls;
    using System.Data.SqlTypes;
    using System.Collections;
    using DevComponents.WinForms;
    namespace MTSICS
    {
        public partial class MainFrm : Form
        {
            private MT_SICS.MT_SICS mtsics = null;
            //private int fl = 0;//标志变量
            public MainFrm() 
            {
                InitializeComponent();
            }
            //刷新数据
            private void buttonX7_Click(object sender, EventArgs e)
            {
                RefleshData();
            }
            //获取PDI中数据在listview中显示
            private void RefleshData()
            {
                listViewEx1.Items.Clear();
                IList<tbdipdi> pdiList = new List<tbdipdi>();
                 pdiList = EY_RegNo_DI_DAO.GetRegNoInPDI("N");//拿到PDI里的数据
                IList<EY_RegNo_DI> temList = new List<EY_RegNo_DI>();
                EY_RegNo_DI temVo= new EY_RegNo_DI();
           
                foreach(tbdipdi vo in pdiList)
                  {
                
                    String[] ss= new String[3];
                    ss = vo.Data.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                                       
                    IList<EY_MatrTable> MatrNameList = EY_MatrTable_DAO.GetMatrName(ss[1].Trim());

                    if (MatrNameList.Count == 0)
                    {
                        vo.Status = "E";
                        EY_RegNo_DI_DAO.UpdatePDI(vo);

                        continue;
                    }

                    foreach (EY_MatrTable t in MatrNameList)
                    {
                        temVo.CName = t.MatrName;
                        
                    }
                                        
                    temVo.RegNo = ss[2].Trim();
                    temVo.RevDateTime=DateTime.Now;
                    temVo.Status = "N";
                    temVo.Batch = null;
                    temVo.CheckNo = ss[3].Trim();
                    temVo.MatrNo = ss[1].Trim();
                    IList<EY_RegNo_DI> List = new List<EY_RegNo_DI>();
                    List= EY_RegNo_DI_DAO.GetRegNo3(temVo.RegNo);
                    if (List.Count==0)
                    {
                        EY_RegNo_DI_DAO.Save(temVo);
                    }
                    vo.Status = "S";
                    EY_RegNo_DI_DAO.UpdatePDI(vo);
                  }
              //  RegNo_DI_DAO.Save(temList);
                IList<EY_RegNo_DI> regNoList = EY_RegNo_DI_DAO.GetRegNo("N");
                ShowListView(regNoList, listViewEx1);
            }


            /// <summary>
            /// 通过listview 空间显示案件编号信息
            /// </summary>
            /// <param name="regNoList"></param>
            /// <param name="listView"></param>
            private void ShowListView(IList<EY_RegNo_DI> regNoList,ListViewEx listView)
            {
                listView.Items.Clear();
                ListView.ListViewItemCollection collection = new ListView.ListViewItemCollection(listView);
                foreach (EY_RegNo_DI t in regNoList)
                {
                    ListViewItem lv = new ListViewItem(t.toArray());
                    
                    collection.Add(lv);
                }
            }

            private IList<EY_RegNo_DI> ListViewItems2Regno(ListView.CheckedListViewItemCollection collection)
            {
                IList<EY_RegNo_DI> ret = new List<EY_RegNo_DI>();



                foreach(ListViewItem row in collection)
                {
                    String regNo = row.Text;
                    string matrNo = EY_RegNo_DI_DAO.GetRegNo3(regNo)[0].MatrNo;
                    DateTime revTime = DateTime.Parse(row.SubItems[4].Text);
                    String status = row.SubItems[2].Text;
                    String cName = row.SubItems[3].Text;
                    String checkNo = row.SubItems[1].Text;
                    EY_RegNo_DI temp = new EY_RegNo_DI { RegNo=regNo,CheckNo=checkNo,MatrNo=matrNo,RevDateTime=revTime ,Status=status,CName=cName };
                    ret.Add(temp);
                    
                }
                return ret;
            }

       
            //放弃批号
            private void buttonX6_Click(object sender, EventArgs e)
            {
                
                if (this.listBox1.Text =="")
                {
                    MessageBox.Show("请选择一条批号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                String batchNo11 = this.listBox1.Text;
                IList<EY_RegNo_DI> regNoList11 = EY_RegNo_DI_DAO.GetRegNo1(batchNo11);
                
                if (EY_RegNo_DI_DAO.GetRegNo1(batchNo11).Count == 0)
                {
                    EY_Batch_DAO.Delete(Batch_DAO.LoadOne(batchNo11));

                    this.GetBatchNo();
                    EY_RegNo_DI_DAO.UpdateStatusBatch(regNoList11, "N");
                    RefleshData();
                    return;
                }
                IList<EY_Scale_Value> sss = EY_RegNo_DI_DAO.GetRegNo2(EY_RegNo_DI_DAO.GetRegNo1(this.listBox1.Text)[0].RegNo);
                if (sss[0].Status == "Y")
                {
                    MessageBox.Show("已产制报表,批号无法删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    int RowCount = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;

                    String batchNo = this.listBox1.Text;
                    IList<EY_RegNo_DI> regNoList =EY_RegNo_DI_DAO.GetRegNo1(batchNo);
             
                    // 在执行下面语句的时候，会更新RegNo_DI
                
                     for (int i = 0; i <regNoList.Count; i++)
                     {

                   
                    
                         String reg = regNoList[i].RegNo;
             
                         IList<EY_Scale_Value> list= new List<EY_Scale_Value>();
                        list=EY_RegNo_DI_DAO.GetRegNo2(reg);
                        
                        foreach(EY_Scale_Value t in list )
                        
                            {
                                

                                //if (t.Status == "N")
                                //{

                                    EY_Scale_Value_Dao.Delete(t);

                                    //MessageBox.Show("已产制报表,批号无法删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                   
                                    
                                //}
                                //else
                                //{
                                //    MessageBox.Show("已产制报表,无法删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //    break;
                                //}
                            }
                  
                    
             
                     }
                     EY_RegNo_DI_DAO.UpdateStatusBatch(regNoList, "N");
                     EY_Batch_DAO.Delete(Batch_DAO.LoadOne(batchNo));
          
                    this.RefleshData();
                    this.GetBatchNo();
                    this.dataGridViewX1.Rows.Clear();
                    for (int i = 0; i < regNoList.Count; i++)
                    {
                        if (regNoList[i].RegNo.Contains("_"))
                        {
                       
                            EY_RegNo_DI_DAO.delete(regNoList[i]);
                        }
                    }
                    RefleshData();


                
                }

                catch (Exception ex)
                {
                    MessageBox.Show("请选择一个批号再点击放弃批号");
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    SessionHelper.CloseSession();
                }
            }

            //关闭窗口
            private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
            {
               // MessageBox.Show("确认退出吗");
            }
            //挑选案件编号
            private void buttonX4_Click(object sender, EventArgs e)
            {
                if (listViewEx1.CheckedItems.Count == 0)
                {
                    MessageBox.Show("请勾选至少一项案件编号!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (comboBoxEx2_operator2.Text == "")
                {
                    MessageBox.Show("请选择质检员2", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                EY_Batch batchNo = new EY_Batch { BatchNo=EY_Batch_DAO.GetBatchNo(),CreateTime=DateTime.Now};
                IList<EY_RegNo_DI> checkVO = ListViewItems2Regno(listViewEx1.CheckedItems);
                batchNo.BatchChild = checkVO;
                int i = 0;
                foreach (EY_RegNo_DI t in checkVO)
                {
                
                    t.Batch = batchNo;
                    t.Status = "Y";
                   // t.MatrNo = checkVO[i].RegNo.Substring(1,;
                    t.CName=checkVO[i].CName;
                                       
                    i++;
                }
                EY_Batch_DAO.Save(batchNo);
                GetBatchNo();
                RefleshData();
                dataGridViewX1.Rows.Clear();
                DisplayDataGridView(checkVO);
             //   int RowCount = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;

             /*   for (int i = 0; i < RowCount; i++)
                {
                    Scale_Value Scale_Value_Vo = new Scale_Value();

                   // Scale_Value_Vo.RegNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[0].Value);
                 //   Scale_Value_Vo.EleNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[1].Value);
                    Scale_Value_PK spk = new Scale_Value_PK();
                    spk.EleNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[1].Value);
                    spk.RegNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[0].Value); 


                    Scale_Value_Vo.Id = spk;

                    Scale_Value_Vo.WgtOne = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[2].Value);
                    Scale_Value_Vo.WgtTwo = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[3].Value);

                    Scale_Value_Vo.WgtThree = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[4].Value);
                    Scale_Value_Vo.WgtFinal = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[5].Value);



                    Scale_Value_Dao.Create(Scale_Value_Vo);

                }*/
                saveIt();
                buttonX3.Focus();
            }

            private void DisplayDataGridView(IList<EY_RegNo_DI> batchReg)
            {
            
                foreach (EY_RegNo_DI t in batchReg)
                {
                    this.dataGridViewX1.Rows.Add(DateTime.Now ,t.RegNo,t.CheckNo,t.CName,null,null,null,null,null,null,null,textBoxX4.Text,comboBoxEx2_operator2.Text);
                }
            }
            // 显示称重后的值
            private void DisplayDataGridView1(IList<EY_Scale_Value> batchReg)
            {
                this.dataGridViewX1.Rows.Clear();
                int i = 0;
           
                foreach (EY_Scale_Value t in batchReg)
                {
                    this.dataGridViewX1.Rows.Add(t.Id);
                    this.dataGridViewX1.Rows[i].Cells[0].Value = t.Date;
                    this.dataGridViewX1.Rows[i].Cells[1].Value = t.Id.RegNo;
                    this.dataGridViewX1.Rows[i].Cells[2].Value = t.Id.CheckNo;
                    this.dataGridViewX1.Rows[i].Cells[3].Value = t.Matrname;
                    this.dataGridViewX1.Rows[i].Cells[4].Value = t.PanWgt;
                    this.dataGridViewX1.Rows[i].Cells[5].Value = t.YangWgt;
                    this.dataGridViewX1.Rows[i].Cells[6].Value = t.HengWgt1;
                    this.dataGridViewX1.Rows[i].Cells[7].Value = t.HengWgt2;
                    this.dataGridViewX1.Rows[i].Cells[8].Value = t.HengWgt3;
                    this.dataGridViewX1.Rows[i].Cells[9].Value = t.HengWgt4;
                    this.dataGridViewX1.Rows[i].Cells[10].Value = t.FinalValue;
                    this.dataGridViewX1.Rows[i].Cells[11].Value = t.Operator1;
                    this.dataGridViewX1.Rows[i].Cells[12].Value = t.Operator2;
                    this.dataGridViewX1.Rows[i].Cells[13].Value = t.Mem1;
                    i++;
                }
               // this.dataGridViewX1.Rows.Clear();
            }

            //按时间更新listbox1
            private void GetBatchNo()
            {
                IList<EY_Batch> allBatch = EY_Batch_DAO.LoadByTime(DateTime.Now.AddDays(-1),DateTime.Now);
                this.listBox1.Items.Clear();
                foreach (EY_Batch t in allBatch)
                {
                    listBox1.Items.Add(t.BatchNo);
                }
            }
            //按照指定日期查询
            private void GetBatchNo1(DateTime tem)
            {

                IList<EY_Batch> allBatch = Batch_DAO.LoadByTime(tem, tem.AddHours(24));
                this.listBox1.Items.Clear();
                foreach (EY_Batch t in allBatch)
                {
                    listBox1.Items.Add(t.BatchNo);
                }
            }

      
            //称重项目有变化时出发此事件

            //private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
            //{
            //    try
            //    {
               
            //        int RowCount = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Visible) - 1;
            //        IList<Scale_Value> ScaleList = new List<Scale_Value>();
            //        //   Scale_Value scale_vo = new Scale_Value();
            //        Scale_Value_PK spk = new Scale_Value_PK();
            //        //MessageBox.Show(listBox1.SelectedItem + "");
            //        String s = Convert.ToString(listBox1.SelectedItem);
            //        IList<RegNo_DI> list = new List<RegNo_DI>();
            //        list=RegNo_DI_DAO.GetRegNo1(s);
                
            //        for (int i = 0; i < RowCount; i++)
            //        {

            //            spk.RegNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[0].Value);
            //            spk.EleNo = this.comboBoxEx1.Text;
            //            String ele = Convert.ToString(dataGridViewX1.Rows[i].Cells[1].Value);
            //            ScaleList = Scale_Value_Dao.GetRegNo(spk);
            //            if (ScaleList.Count > 0)
            //            {
                        
            //                    dataGridViewX1.Rows[i].Cells[1].Value = ScaleList[0].Id.EleNo;
            //                    dataGridViewX1.Rows[i].Cells[2].Value = ScaleList[0].WgtOne;
            //                    dataGridViewX1.Rows[i].Cells[3].Value = ScaleList[0].WgtTwo;
            //                    dataGridViewX1.Rows[i].Cells[4].Value = ScaleList[0].WgtThree;
            //                    dataGridViewX1.Rows[i].Cells[5].Value = ScaleList[0].WgtFinal;
            //                    dataGridViewX1.Rows[i].Cells[6].Value = ScaleList[0].WgtFinalTwo;
            //                    dataGridViewX1.Rows[i].Cells[7].Value = ScaleList[0].WgtFinalThree;
            //                    dataGridViewX1.Rows[i].Cells[8].Value = ScaleList[0].WgtFinalFour;
                        

            //                    if (fl == 1&&ele!=spk.EleNo)
            //                    {
            //                        dataGridViewX1.Rows.RemoveAt(i);

            //                        i = i - 1;
            //                        RowCount = RowCount - 1;
            //                    }

            //            }
            //            else
            //            {
            //                if (Convert.ToString(dataGridViewX1.Rows[i].Cells[0].Value).Contains("_"))
            //                {
            //                    dataGridViewX1.Rows.RemoveAt(i);
            //                    i = i - 1;
            //                    RowCount = RowCount - 1;
                           
            //                }
            //                else
            //                {

            //                    dataGridViewX1.Rows[i].Cells[1].Value = this.comboBoxEx1.Text;
            //                    dataGridViewX1.Rows[i].Cells[2].Value = 0;
            //                    dataGridViewX1.Rows[i].Cells[3].Value = 0;
            //                    dataGridViewX1.Rows[i].Cells[4].Value = 0;
            //                    dataGridViewX1.Rows[i].Cells[5].Value = 0;
            //                    dataGridViewX1.Rows[i].Cells[6].Value = 0;
            //                    dataGridViewX1.Rows[i].Cells[7].Value = 0;
            //                    dataGridViewX1.Rows[i].Cells[8].Value = 0;
            //                }
            //            }
            //        }
            //        fl = 0;//用fl来控制是否是第一次带数据库资料
            //    }
                
            //    catch(Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);

            //    }
          
            //}
     
            //取得稳定值
            private void buttonX3_Click(object sender, EventArgs e)
            {
                 //int flag = 0;
                 //int Row = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;
                 int SelectRowcount = dataGridViewX1.SelectedRows.Count;
                 for (int i = 0; i < SelectRowcount; i++)
                 {   

                     
                     if (dataGridViewX1.SelectedRows[i].Cells[4].Value == null || Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[4].Value) == 0)
                     {
                         dataGridViewX1.SelectedRows[i].Cells[4].Value = lxLedControl1.Text.Trim();
                         break;
                     }
                     else if (dataGridViewX1.SelectedRows[i].Cells[5].Value == null || Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[5].Value) == 0)
                     {
                         dataGridViewX1.SelectedRows[i].Cells[5].Value = lxLedControl1.Text.Trim();
                         break;                     
                     }
                     else if (dataGridViewX1.SelectedRows[i].Cells[6].Value == null || Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[6].Value) == 0)
                     {
                         dataGridViewX1.SelectedRows[i].Cells[6].Value = lxLedControl1.Text.Trim();
                         break;
                     }
                     else if (dataGridViewX1.SelectedRows[i].Cells[7].Value == null || Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[7].Value) == 0)
                     {
                         dataGridViewX1.SelectedRows[i].Cells[7].Value = lxLedControl1.Text.Trim();
                         break;
                     }
                     else if (dataGridViewX1.SelectedRows[i].Cells[8].Value == null || Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[8].Value) == 0)
                     {
                         dataGridViewX1.SelectedRows[i].Cells[8].Value = lxLedControl1.Text.Trim();
                         break;
                     }
                     else if (dataGridViewX1.SelectedRows[i].Cells[9].Value == null || Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[9].Value) == 0)
                     {
                         dataGridViewX1.SelectedRows[i].Cells[9].Value = lxLedControl1.Text.Trim();

                         break;
                     }

                
                     // mtsics.SendSeriesWeightRequest();
                  


                 }

                 saveIt();



                //分割线
               // int Row = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;


           //     if (Row < 1)
           //     {
           //         MessageBox.Show("没有数据可以修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
           //         return;
           //     }



           //     int ss = EY_Scale_Value_Status();
           //     if (ss == 0)
           //     {
           //         try
           //         {
           //             // mtsics.SendStableWeightRequest();//取得稳定数值
           //             int flag = 0;
           //             int RowCount = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;
           //             for (int i = 0; i < RowCount; i++)
           //             {
           //                 //盘重
           //                 if (dataGridViewX1.Rows[i].Cells[4].Value == null || Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[4].Value) == 0)
           //                 {
           //                     dataGridViewX1.Rows[i].Cells[4].Value = lxLedControl1.Text.Trim();
           //                     break;

           //                 }
           //                 else
           //                 {
           //                     //样重
           //                     if (dataGridViewX1.Rows[i].Cells[5].Value == null || Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[5].Value) == 0)
           //                     {
           //                         dataGridViewX1.Rows[i].Cells[5].Value = lxLedControl1.Text.Trim();

           //                         break;
           //                     }

           //                     else
           //                     {
           //                         //恒重1
           //                         if (dataGridViewX1.Rows[i].Cells[6].Value == null || Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[6].Value) == 0)
           //                         {
           //                             dataGridViewX1.Rows[i].Cells[6].Value = lxLedControl1.Text.Trim();
           //                             break;

           //                         }
           //                         else
           //                         {
           //                             //恒重2
           //                             if (dataGridViewX1.Rows[i].Cells[7].Value == null || Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[7].Value) == 0)
           //                             {
           //                                 dataGridViewX1.Rows[i].Cells[7].Value = lxLedControl1.Text.Trim();
           //                                 break;

           //                             }
           //                             else
           //                             {
           //                                 //恒重3
           //                                 if (dataGridViewX1.Rows[i].Cells[8].Value == null || Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[8].Value) == 0)
           //                                 {
           //                                     dataGridViewX1.Rows[i].Cells[8].Value = lxLedControl1.Text.Trim();
           //                                     break;

           //                                 }

           //                                 else
           //                                 {
           //                                     if (dataGridViewX1.Rows[i].Cells[9].Value == null || Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[9].Value) == 0)
           //                                     {
           //                                         dataGridViewX1.Rows[i].Cells[9].Value = lxLedControl1.Text.Trim();
           //                                         if (i == RowCount - 1)
           //                                         {
           //                                             flag = 1;
           //                                         }


           //                                         break;

           //                                     }

           //                                 }

           //                             }


           //                         }



           //                     }

           //                 }


           //             }

           //             if (dataGridViewX1.Rows[RowCount - 1].Cells[9].Value != null && flag != 1 && Convert.ToDecimal(dataGridViewX1.Rows[RowCount - 1].Cells[9].Value) != 0)
           //             {
           //                 for (int i = 0; i < RowCount; i++)
           //                 {
           //                     if (dataGridViewX1.Rows[i].Cells[4].Value == null || Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[4].Value) == 0)
           //                     {
           //                         dataGridViewX1.Rows[i].Cells[4].Value = lxLedControl1.Text.Trim();
           //                         break;

           //                     }

           //                 }
           //             }
           //             // mtsics.SendSeriesWeightRequest();
           //             saveIt();


           //         }
           //         catch (Exception ex)
           //         {
           //             MessageBox.Show("取得数据前请先打开串口");
           //             MessageBox.Show(ex.Message);

           //         }
           //         finally
           //         {
           //             SessionHelper.CloseSession();
           //         }

           //     }
           //     else
           //     {
           //         MessageBox.Show("已产制报表，数据无法更改");
           //     }

            }

      
            //保存数据
            private void buttonX1_Click(object sender, EventArgs e)
            {
                try
                {
                    int RowCount = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;

                    for (int i = 0; i < RowCount; i++)
                    {
                        EY_Scale_Value EY_Scale_Value_Vo = new EY_Scale_Value();
                        EY_Scale_Value_PK spk = new EY_Scale_Value_PK();
                       // EY_Scale_Value_Vo.Date = Convert.ToDateTime(dataGridViewX1.Rows[i].Cells[0].Value);
                        EY_Scale_Value_Vo.Date = Convert.ToDateTime(dataGridViewX1.Rows[i].Cells[0].Value);
                        spk.RegNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[1].Value);
                        spk.CheckNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[2].Value);
                       // EY_Scale_Value_Vo.RegNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[1].Value);
                       // EY_Scale_Value_Vo.CheckNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[2].Value);
                        EY_Scale_Value_Vo.Id = spk;
                        EY_Scale_Value_Vo.Matrname = Convert.ToString(dataGridViewX1.Rows[i].Cells[3].Value);
                        EY_Scale_Value_Vo.PanWgt = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[4].Value);

                        EY_Scale_Value_Vo.YangWgt = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[5].Value);
                        EY_Scale_Value_Vo.HengWgt1 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[6].Value);
                        EY_Scale_Value_Vo.HengWgt2 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[7].Value);
                        EY_Scale_Value_Vo.HengWgt3 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[8].Value);
                        EY_Scale_Value_Vo.HengWgt4 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[9].Value);
                        EY_Scale_Value_Vo.FinalValue = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[10].Value);
                        EY_Scale_Value_Vo.Operator1 = Convert.ToString(dataGridViewX1.Rows[i].Cells[11].Value);
                        EY_Scale_Value_Vo.Operator2 = Convert.ToString(dataGridViewX1.Rows[i].Cells[12].Value);
                        EY_Scale_Value_Vo.Mem1 = Convert.ToString(dataGridViewX1.Rows[i].Cells[13].Value);
                        ////上传ERP标志位
                        //EY_Scale_Value_Vo.ToErpFlag = "0";
                        ////修改标志位
                        //EY_Scale_Value_Vo.Flag = "0";
                        ////产制报表后变Y后数据不可修改
                        //EY_Scale_Value_Vo.Status = "N";
                        
                    
                        if (EY_Scale_Value_Dao.GetRegNo1(EY_Scale_Value_Vo.Id).Count==0)

                            {

                                EY_Scale_Value_Vo.ToErpFlag = "0";
                                EY_Scale_Value_Vo.Flag = 0;
                                EY_Scale_Value_Vo.Status = "N";

                                EY_Scale_Value_Dao.Create(EY_Scale_Value_Vo);
                            
                         
                             }else
                            {
                                EY_Scale_Value_Dao.Update(EY_Scale_Value_Vo);

                            }

                    }
                    MessageBox.Show("保存数据成功");

                }
                catch(Exception ex)
                {
                    MessageBox.Show("该批次已保存，请勿重复点击");
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    SessionHelper.CloseSession();
                }

            }
            //后台保存数据用
            private void saveIt() 
            {


                int ss = EY_Scale_Value_Status();
                if (ss == 0)
                {
                    try
                    {
                        int RowCount = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;

                        for (int i = 0; i < RowCount; i++)
                        {
                            EY_Scale_Value EY_Scale_Value_Vo = new EY_Scale_Value();
                            EY_Scale_Value_PK spk = new EY_Scale_Value_PK();
                            // EY_Scale_Value_Vo.Date = Convert.ToDateTime(dataGridViewX1.Rows[i].Cells[0].Value);
                            EY_Scale_Value_Vo.Date = Convert.ToDateTime(dataGridViewX1.Rows[i].Cells[0].Value);
                            spk.RegNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[1].Value);
                            spk.CheckNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[2].Value);
                            // EY_Scale_Value_Vo.RegNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[1].Value);
                            // EY_Scale_Value_Vo.CheckNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[2].Value);
                            EY_Scale_Value_Vo.Id = spk;
                            EY_Scale_Value_Vo.Matrname = Convert.ToString(dataGridViewX1.Rows[i].Cells[3].Value);
                            EY_Scale_Value_Vo.PanWgt = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[4].Value);

                            EY_Scale_Value_Vo.YangWgt = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[5].Value);
                            EY_Scale_Value_Vo.HengWgt1 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[6].Value);
                            EY_Scale_Value_Vo.HengWgt2 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[7].Value);
                            EY_Scale_Value_Vo.HengWgt3 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[8].Value);
                            EY_Scale_Value_Vo.HengWgt4 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[9].Value);
                            EY_Scale_Value_Vo.FinalValue = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[10].Value);
                            EY_Scale_Value_Vo.Operator1 = Convert.ToString(dataGridViewX1.Rows[i].Cells[11].Value);
                            EY_Scale_Value_Vo.Operator2 = Convert.ToString(dataGridViewX1.Rows[i].Cells[12].Value);
                            EY_Scale_Value_Vo.Mem1 = Convert.ToString(dataGridViewX1.Rows[i].Cells[13].Value);
                            EY_Scale_Value_Vo.ToErpFlag = "0";
                            EY_Scale_Value_Vo.Flag = 0;
                            EY_Scale_Value_Vo.Status = "N";


                            if (EY_Scale_Value_Dao.GetRegNo1(EY_Scale_Value_Vo.Id).Count == 0)
                            {

                                //EY_Scale_Value_Vo.ToErpFlag = "0";
                                //EY_Scale_Value_Vo.Flag = "0";
                                //EY_Scale_Value_Vo.Status = "N";
                                EY_Scale_Value_Dao.Create(EY_Scale_Value_Vo);


                            }
                            else
                            {
                                EY_Scale_Value_Dao.Update(EY_Scale_Value_Vo);

                            }

                        }
                        // MessageBox.Show("保存数据成功");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("saveit方法报错");
                        MessageBox.Show(ex.Message);
                    }
                    //finally
                    //{
                    //    SessionHelper.CloseSession();
                    //}
                
                }


            }

            //判断数据是否产制报表，如果产制报表不可修改
            private  int  EY_Scale_Value_Status()
            {



                int flag = 0;
                EY_Scale_Value EY_Scale_Value = new EY_Scale_Value();
                EY_Scale_Value_PK spk = new EY_Scale_Value_PK();
               
                spk.RegNo = Convert.ToString(dataGridViewX1.CurrentRow.Cells[1].Value);
                spk.CheckNo = Convert.ToString(dataGridViewX1.CurrentRow.Cells[2].Value);
                EY_Scale_Value.Id = spk;

                IList<EY_Scale_Value> EY_Scale_value_Status = EY_Scale_Value_Dao.GetRegNo1(EY_Scale_Value.Id);
             foreach (EY_Scale_Value t in EY_Scale_value_Status)
             {
                 if (t.Status == "Y")
                 {
                    // MessageBox.Show("已产制报表，数据不可修改，请联系管理人员");
                     flag = 1;
                 }
            }
             return flag;
            }

            public void Writelog(string Oldvalue, string Newvalue, string matrname, EY_Scale_Value_PK idd)
            {

                IList<EY_Scale_Value> zzz = EY_Scale_Value_Dao.GetRegNo1(idd);
                foreach (EY_Scale_Value t in zzz)
                {

                    EY_DataChange_Log LOG = new EY_DataChange_Log();
                    LOG.OldValue = Oldvalue;
                    LOG.NewValue = Newvalue;
                    LOG.Operator1 = textBoxX4.Text;
                    LOG.Date = System.DateTime.Now;
                    LOG.RegNo = t.Id.RegNo;
                    LOG.CheckNo = t.Id.CheckNo;
                    LOG.MatrName = t.Matrname;
                    LOG.RowName = matrname;

                    EY_DataChange_Log_DAO.Save(LOG);


                }
            }



            //修改数据
            private void buttonX2_Click(object sender, EventArgs e)

            {

                int Row = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;


                if (Row < 1)
                {
                    MessageBox.Show("没有数据可以修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


              int ss = EY_Scale_Value_Status();
              if (ss == 0)
              {

                  try
                  {
                      int RowCount = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;

                      for (int i = 0; i < RowCount; i++)
                      {
                          EY_Scale_Value EY_Scale_Value_Vo = new EY_Scale_Value();
                          EY_Scale_Value_PK ssss = new EY_Scale_Value_PK();

                          ssss.RegNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[1].Value);
                          ssss.CheckNo = Convert.ToString(dataGridViewX1.Rows[i].Cells[2].Value);

                          EY_Scale_Value_Vo.Id = ssss;

                          IList<EY_Scale_Value> zzz = EY_Scale_Value_Dao.GetRegNo1(ssss);
                          foreach (EY_Scale_Value t in zzz)
                          {



                           EY_Scale_Value_Vo.Matrname = t.Matrname;
                           EY_Scale_Value_Vo.Date = t.Date;



                          EY_Scale_Value_Vo.PanWgt = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[4].Value);
                          if (EY_Scale_Value_Vo.PanWgt != t.PanWgt)
                          {
                            Writelog(Convert.ToString(t.PanWgt), Convert.ToString(EY_Scale_Value_Vo.PanWgt), "盘重",ssss);
                            EY_Scale_Value_Vo.Flag = t.Flag + 1;
                           }

                          EY_Scale_Value_Vo.YangWgt = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[5].Value);
                          if (EY_Scale_Value_Vo.YangWgt != t.YangWgt)
                          {
                              Writelog(Convert.ToString(t.YangWgt), Convert.ToString(EY_Scale_Value_Vo.YangWgt), "样重", ssss);
                              EY_Scale_Value_Vo.Flag = t.Flag + 1;
                          }
                          EY_Scale_Value_Vo.HengWgt1 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[6].Value);
                          if (EY_Scale_Value_Vo.HengWgt1 != t.HengWgt1)
                          {
                              Writelog(Convert.ToString(t.HengWgt1), Convert.ToString(EY_Scale_Value_Vo.HengWgt1), "恒重1", ssss);
                              EY_Scale_Value_Vo.Flag = t.Flag + 1;
                          }
                          EY_Scale_Value_Vo.HengWgt2 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[7].Value);
                          if (EY_Scale_Value_Vo.HengWgt2 != t.HengWgt2)
                          {
                              Writelog(Convert.ToString(t.HengWgt2), Convert.ToString(EY_Scale_Value_Vo.HengWgt2), "恒重2", ssss);
                              EY_Scale_Value_Vo.Flag = t.Flag + 1;
                          }
                          EY_Scale_Value_Vo.HengWgt3 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[8].Value);
                          if (EY_Scale_Value_Vo.HengWgt3 != t.HengWgt3)
                          {
                              Writelog(Convert.ToString(t.HengWgt3), Convert.ToString(EY_Scale_Value_Vo.HengWgt3), "恒重3", ssss);
                              EY_Scale_Value_Vo.Flag = t.Flag + 1;
                          }
                          EY_Scale_Value_Vo.HengWgt4 = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[9].Value);
                          if (EY_Scale_Value_Vo.HengWgt4 != t.HengWgt4)
                          {
                              Writelog(Convert.ToString(t.HengWgt4), Convert.ToString(EY_Scale_Value_Vo.HengWgt4), "恒重4", ssss);
                              EY_Scale_Value_Vo.Flag = t.Flag + 1;
                          }
                          EY_Scale_Value_Vo.FinalValue = Convert.ToDecimal(dataGridViewX1.Rows[i].Cells[10].Value);
                          if (EY_Scale_Value_Vo.FinalValue != t.FinalValue)
                          {
                              Writelog(Convert.ToString(t.FinalValue), Convert.ToString(EY_Scale_Value_Vo.FinalValue), "结果", ssss);
                              EY_Scale_Value_Vo.Flag = t.Flag + 1;
                          }
                          EY_Scale_Value_Vo.Operator1 = t.Operator1;
                          EY_Scale_Value_Vo.Operator2 = Convert.ToString(dataGridViewX1.Rows[i].Cells[12].Value);
                          if (EY_Scale_Value_Vo.Operator2 != t.Operator2)
                          {
                              Writelog(Convert.ToString(t.Operator2), Convert.ToString(EY_Scale_Value_Vo.Operator2), "备注", ssss);
                              EY_Scale_Value_Vo.Flag = t.Flag + 1;
                          }
                          EY_Scale_Value_Vo.Mem1 = Convert.ToString(dataGridViewX1.Rows[i].Cells[13].Value);
                          if (EY_Scale_Value_Vo.Mem1 != t.Mem1)
                          {
                              Writelog(Convert.ToString(t.Mem1 ), Convert.ToString(EY_Scale_Value_Vo.Mem1), "备注", ssss);
                              EY_Scale_Value_Vo.Flag = t.Flag + 1;
                          }
                          // EY_Scale_Value_Vo.ToErpFlag = "0";
                          //数据被修改过

                          

                               EY_Scale_Value_Vo.Status = t.Status;
                               EY_Scale_Value_Vo.ToErpFlag = t.ToErpFlag;
                               //EY_Scale_Value_Vo.Mem1 = t.Mem1;
                          }
                                                  

                          EY_Scale_Value_Dao.Update(EY_Scale_Value_Vo);

                      }

                      MessageBox.Show("修改数据成功");

                  }
                  catch (Exception ex)
                  {
                      MessageBox.Show(ex.Message);

                  }

              }

              else {

                  MessageBox.Show("已产制报表，无法修改数据，请联系管理员");
              } 

            }
            //("COM3", 9600, Parity.None, 8, StopBits.One);
            private void MainFrm_Load(object sender, EventArgs e)
            {
                this.RefleshData();
                this.GetBatchNo();
                //端口名称
                comboBoxEx1_com.SelectedIndex = 0;
                //波特率
                comboBoxEx2_baud.SelectedIndex = 2;
                //数据位
                comboBoxEx3_data.SelectedIndex = 1;
                //奇偶校验
                comboBoxEx4_check.SelectedIndex = 0;
                //停止位
                comboBoxEx5_stop.SelectedIndex = 0;
                MT_SICS.MT_SICS mt = new MT_SICS.MT_SICS();
                IList<MTSICS_VO.Login> userlist = new List<MTSICS_VO.Login>();
                userlist= Login_DAO.CheckOutByFlag();
                foreach(MTSICS_VO.Login t in userlist )
                        
                            {
                                this.textBoxX4.Text = t.UserName;
                            }
                
                IList<MTSICS_VO.Login> username = new List<MTSICS_VO.Login>();

                username = Login_DAO.SelectUserName();
                foreach (MTSICS_VO.Login t in username)
                {
                    this.comboBoxEx2_operator2.Items.Add(t.UserName);
                    
                }

                
               // mt.SendSeriesWeightRequest();
          
            }
            /// <summary>
            /// 查询组批称重
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void listBox1_MouseClick(object sender, MouseEventArgs e)
            {

                try
                {
                    //int RowCount = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;
                    IList<EY_Scale_Value> scale = new List<EY_Scale_Value>();
                    String batchNo = this.listBox1.Text;
                    IList<EY_RegNo_DI> regNoList = EY_RegNo_DI_DAO.GetRegNo1(batchNo);

                    for (int i = 0; i < regNoList.Count; i++)
                    {
                    
                        String reg = (EY_RegNo_DI_DAO.GetRegNo1(this.listBox1.Text))[i].RegNo;
                        IList<EY_Scale_Value> list= new List<EY_Scale_Value>();
                        list=EY_RegNo_DI_DAO.GetRegNo2(reg);
                  
                        foreach(EY_Scale_Value t in list )
                        
                            {
                                scale.Add(t);
                                            


                            }

                    }


                    this.DisplayDataGridView1(scale);
                   // fl = 1;
                }

                catch (Exception ex)
                {
                    MessageBox.Show("未选中批号");
                    MessageBox.Show(ex.Message);

                }
           
            }

            //打开串口
            private void button6_Click(object sender, EventArgs e)
            {
                try
                {
              
                    //String portName = comboBoxItem1.Items[comboBoxItem1.SelectedIndex] + "";//端口名称
                    ////MessageBox.Show(comboBoxItem2.Items[comboBoxItem2.SelectedIndex] + "");
                    //int baudeRate = Convert.ToInt32(comboBoxItem2.Items[comboBoxItem2.SelectedIndex] + "");//波特率
                    //Parity parity = (Parity)Enum.Parse(typeof(Parity), comboBoxItem3.Items[comboBoxItem3.SelectedIndex] + "");//奇偶校验位
                    //int dataBits = Convert.ToInt32(comboBoxItem5.Items[comboBoxItem5.SelectedIndex] + "");
                    //StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBoxItem4.Items[comboBoxItem4.SelectedIndex] + "");
                
                    mtsics = new MT_SICS.MT_SICS();
                    mtsics.OnDataStable += new MT_SICS.MT_SICS.DataStableEventHandler(mtsics_OnDataStable);
              
                    mtsics.Open();
                    MessageBox.Show("串口打开成功");
                    //MT_SICS.MT_SICS mt = new MT_SICS.MT_SICS();
                   // mtsics.SendSeriesWeightRequest();
                    timer1.Enabled = true;
                    //button6.Enabled = false;
                   // button7.Enabled = true;
                    buttonX3.Focus();
                }
                catch( Exception ex)
                {
                    MessageBox.Show("串口已打开或打开全口前输入信息不完整");
                    MessageBox.Show(ex.Message);

                }
            }
            //串口数据接收
            private void mtsics_OnDataStable(object sender, DataStableEventArgs args)
            {
                try
                {
                    lxLedControl1.BeginInvoke(new Action(() =>
                    {
                        //"S S 111111g/r/n"=""

                        //     12.5 g 
                       
                        
                        lxLedControl1.Text = args.OrgData.Substring(0, 1) + args.OrgData.Substring(2, 7);
                        label_danwei.Text = args.OrgData.Substring(9, 3).Trim();
                        
                    }));

                   

                }
                catch
                {
                    MessageBox.Show("重量异常");
                }
            }
            //关闭串口
            private void button7_Click(object sender, EventArgs e)
            {
                try
                {
                    mtsics.Close();
                    MessageBox.Show("串口关闭成功");
                    //button6.Enabled = true;
                   // button7.Enabled = false;
                }
                catch(Exception ex)
                {
                     MessageBox.Show("串口未打开，不需要关闭");
                     MessageBox.Show(ex.Message);

                }

            }

            /// <summary>
            /// 计算最终结果
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void buttonX5_Click(object sender, EventArgs e)
            {
                int Row = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;



                if (Row < 1)
                {
                    //MessageBox.Show("没有数据可以ji", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }




                Decimal finalvalue;

                int ss = EY_Scale_Value_Status();
                if (ss == 0)
                {

                    int rowcount = dataGridViewX1.SelectedRows.Count;
                    //int RowCount = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;
                    for (int i = 0; i < rowcount; i++)
                    {
                        Decimal watervalue = 0, Min0 = 0;


                        Decimal Min = Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[6].Value);
                        Decimal hengwgt2 = Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[7].Value);
                        Decimal Min1 = Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[8].Value);
                        Decimal hengwgt4 = Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[9].Value);
                        if (Min > 0)
                            Min0 = Min;
                        if (hengwgt2 < Min0 && hengwgt2 > 0)
                            Min0 = hengwgt2;
                        if (Min1 < Min0 && Min1 > 0)
                            Min0 = Min1;
                        if (hengwgt4 < Min0 && hengwgt4 > 0)
                            Min0 = hengwgt4;


                        Decimal final = (Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[4].Value) + Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[5].Value) - Min0) * 100 / Convert.ToDecimal(dataGridViewX1.SelectedRows[i].Cells[5].Value);


                        // IList<MTSICS_VO.EY_MatrTable> MatrTable = new List<EY_MatrTable>();
                        if (Convert.ToString(dataGridViewX1.SelectedRows[i].Cells[13].Value) != "DB")
                        {

                            string matrname = Convert.ToString(dataGridViewX1.SelectedRows[i].Cells[3].Value);
                            IList<EY_MatrTable> regNoList = EY_MatrTable_DAO.GetMatrTableWaterValue(matrname);
                            foreach (EY_MatrTable t in regNoList)
                            {
                                if (Convert.ToString(dataGridViewX1.SelectedRows[i].Cells[13].Value) == "XY")
                                {
                                    if (t.Water_Range_Less == 8 && t.Water_Range_Grater == 8)
                                        watervalue = t.Water_Value;

                                }

                                if ((t.Water_Range_Less == 0) && (t.Water_Range_Grater == 0))
                                {
                                    watervalue = t.Water_Value;

                                }

                                //if (final <= t.Water_Range_Less && final > t.Water_Range_Grater &&( t.Water_Range_Less < final && final <= t.Water_Range_Grater))
                                //    watervalue = t.Water_Value;
                                if (t.Water_Range_Less <= final && final < t.Water_Range_Grater)
                                    watervalue = t.Water_Value;
                                //if (final > t.Water_Range_Grater)
                                //    watervalue = t.Water_Value;



                            }

                            finalvalue = final + watervalue;
                        }
                        else
                        {
                            finalvalue = final;
                        }



                        dataGridViewX1.SelectedRows[i].Cells[10].Value = finalvalue;

                        EY_Scale_Value_PK SPL = new EY_Scale_Value_PK();

                        SPL.RegNo = Convert.ToString(dataGridViewX1.SelectedRows[i].Cells[1].Value);
                        SPL.CheckNo = Convert.ToString(dataGridViewX1.SelectedRows[i].Cells[2].Value);

                        //string regNo = Convert.ToString(dataGridViewX1.SelectedRows[i].Cells[6].Value);
                        //  EY_Scale_Value_Dao.Update_OnlyValue(SPL, finalvalue);

                        //IList<EY_Scale_Value> zz = EY_Scale_Value_Dao.GetRegNo1(SPL);
                        //foreach (EY_Scale_Value t in zz)
                        // t.FinalValue = finalvalue;

                        EY_Scale_Value_Dao.Update_OnlyValue(SPL, finalvalue);


                    }

                    //saveIt();
                }
                buttonX3.Focus();
            }
            
        
            /// <summary>
            /// 添加平行样
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void buttonX8_Click(object sender, EventArgs e)

            {
                try
                {
                    EY_Scale_Value_PK spk = new EY_Scale_Value_PK();
                    spk.RegNo = Convert.ToString(dataGridViewX1.SelectedCells[1].Value);
                    spk.CheckNo = Convert.ToString(dataGridViewX1.SelectedCells[2].Value);
                    EY_RegNo_DI temVo = new EY_RegNo_DI();

                    IList<EY_RegNo_DI> regCount = new List<EY_RegNo_DI>();

                    regCount = EY_RegNo_DI_DAO.GetRegNoCount(spk.RegNo);

                    dataGridViewX1.Rows.Insert(dataGridViewX1.CurrentCell.RowIndex + 1, 1);
                    int RowCount1 = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;
                    int a = regCount.Count + 1;
                    dataGridViewX1.Rows[dataGridViewX1.CurrentCell.RowIndex + 1].Cells[0].Value = System.DateTime.Now;
                    dataGridViewX1.Rows[dataGridViewX1.CurrentCell.RowIndex + 1].Cells[1].Value = dataGridViewX1.SelectedCells[1].Value + "_" + a;
                    dataGridViewX1.Rows[dataGridViewX1.CurrentCell.RowIndex + 1].Cells[2].Value = dataGridViewX1.SelectedCells[2].Value;
                    dataGridViewX1.Rows[dataGridViewX1.CurrentCell.RowIndex + 1].Cells[3].Value = dataGridViewX1.SelectedCells[3].Value;
                    IList<EY_RegNo_DI> regList = new List<EY_RegNo_DI>();
                    regList = EY_RegNo_DI_DAO.GetRegNo3(Convert.ToString(dataGridViewX1.SelectedCells[1].Value));
                    temVo.RegNo = dataGridViewX1.SelectedCells[1].Value + "_" + a;
                    temVo.RevDateTime = DateTime.Now;
                    temVo.Status = "Y";
                    temVo.Batch = regList[0].Batch;
                    temVo.CheckNo = regList[0].CheckNo;
                    temVo.MatrNo = regList[0].MatrNo;
                    temVo.CName = regList[0].CName;


                    EY_RegNo_DI_DAO.Save(temVo);

                }
                catch
                {
                    MessageBox.Show("必须指定案件接收编号才能使用此功能");
                }

                finally
                {
                    SessionHelper.CloseSession();
                }
            }
            //新增案件接收编号
            private void button3_Click(object sender, EventArgs e)
            {
                try
                {
                    EY_RegNo_DI temVo = new EY_RegNo_DI();

                    IList<EY_MatrTable> MatrNameList = EY_MatrTable_DAO.GetMatrName(textBoxX1_Number.Text.Trim().Substring(1, 8));

                    foreach (EY_MatrTable t in MatrNameList)
                    {
                        temVo.CName = t.MatrName;

                    }
                   
                    temVo.RegNo = textBoxX1_Number.Text.Trim();
                    temVo.RevDateTime = DateTime.Now;
                    temVo.Status = "N";
                    temVo.Batch = null;
                    temVo.MatrNo = textBoxX1_Number.Text.Trim().Substring(1,8);
                    temVo.CheckNo = "1111";

                    EY_RegNo_DI_DAO.Save(temVo);
                    RefleshData();
                }
                catch
                {
                    MessageBox.Show("长度不够，至少9位,格式：Z+料号+***** 例如：Z11001001***");
                }
            }
            //删除案件接收编号
            private void button1_Click(object sender, EventArgs e)
            {
                
                EY_RegNo_DI temVo = new EY_RegNo_DI();
                IList<EY_RegNo_DI> list = new List<EY_RegNo_DI>();
           
              IList<EY_RegNo_DI> checkVO = ListViewItems2Regno(listViewEx1.CheckedItems);
              if (checkVO.Count == 0)
              {
                  MessageBox.Show("至少选中一条案件接收编号");
              }
              else
              {
                  MessageBox.Show(checkVO[0].RegNo);

                  foreach (EY_RegNo_DI t in checkVO)
                  {
                      EY_RegNo_DI_DAO.delete(t);

                  }

                  RefleshData();
              }

            }

            //查询
            private void button4_Click(object sender, EventArgs e)
            {
                //DateTime tem = dateTimePicker2.Value;
                //MessageBox.Show(tem.ToString().Substring(0,9) + " 00:00:00");
                //String ss = tem.ToString().Substring(0, 9).Trim() + " 00:00:00";
                //tem = DateTime.Parse(ss);
                //GetBatchNo1(tem);
            }
       
            //获取数据库数据，产制报表
            private DataTable GetData()
            {
                DataTable table = new DataTable("table1");

                //使用自动增量时必须设置为SqlInt64
                //table.Columns.Add("id", typeof(SqlInt64));
                //table.Columns["id"].AutoIncrement = true;

               // table.Columns.Add("id", typeof(SqlInt32));
                //设置主键，主键可以是一个或者多个DataColumns对象组成的数组
                DataColumn[] key = new DataColumn[2];
              //  key[0] = table.Columns["id"];
              //  table.PrimaryKey = key;
                //SqlString相当于SQL SERVER类型中的nvarchar
                MessageBox.Show("报表产制中，产制完毕会弹出，请稍候....");
                table.Columns.Add("日期", typeof(SqlDateTime));
                table.Columns.Add("案件接收编号", typeof(SqlString));
                
                 table.Columns.Add("检验批号", typeof(SqlString));
                table.Columns.Add("料名", typeof(SqlString));
                //key[0] = table.Columns["案件接收编号"];
                //key[1] = table.Columns["中文名称"];
                //table.PrimaryKey = key;
               
                //table.Columns.Add("料名", typeof(SqlString));
                table.Columns.Add("盘重", typeof(SqlDecimal));
                table.Columns.Add("样重", typeof(SqlDecimal));
                table.Columns.Add("恒重1", typeof(SqlDecimal));
                table.Columns.Add("恒重2", typeof(SqlDecimal));
                table.Columns.Add("恒重3", typeof(SqlDecimal));
                table.Columns.Add("恒重4", typeof(SqlDecimal));
                table.Columns.Add("结果", typeof(SqlDecimal));
                table.Columns.Add("检测员1", typeof(SqlString));
                table.Columns.Add("检测员2", typeof(SqlString));
                table.Columns.Add("备注", typeof(SqlString));
                //table.Columns.Add("年龄", typeof(SqlInt32));
                //table.Columns.Add("出生日期", typeof(SqlDateTime));
                DateTime tem = dateTimePicker2.Value;
                string year = tem.Year.ToString();
                string month = tem.Month.ToString();
                string day = tem.Day.ToString();
                string date = year + "/" + month + "/" + day;
                MessageBox.Show(date + " 00:00:00");
                String ss = date + " 00:00:00";
               // String ss = tem.ToString().Substring(0, 9).Trim() + " 00:00:00";
                tem = DateTime.Parse(ss);
                IList<EY_Batch> allBatch = EY_Batch_DAO.LoadByTime(tem, tem.AddHours(24));
                int i = 0;
                foreach (EY_Batch t in allBatch)
                {
                    IList<EY_RegNo_DI> reg = EY_RegNo_DI_DAO.GetRegNo1(t.BatchNo);
                    foreach (EY_RegNo_DI a in reg)
                    {
                        EY_Scale_Value_PK id = new EY_Scale_Value_PK();
                        id.RegNo = a.RegNo;
                        id.CheckNo = a.CheckNo;

                       // EY_Scale_Value_Dao.Update_Status(id , "Y");
                        IList<EY_Scale_Value> scale = EY_Scale_Value_Dao.GetRegNo(id);
                        foreach (EY_Scale_Value b in scale)
                        {
                            i++;
                            DataRow row = table.NewRow();
                         
                         //   row["id"] = 1 + i;
                            row["日期"] = b.Date;
                            row["案件接收编号"] = b.Id.RegNo;
                            row["检验批号"] = b.Id.CheckNo;

                                
                            row["料名"] = b.Matrname;

                            //IList<EY_MatrTable> MatrNameList = EY_MatrTable_DAO.GetMatrName(ss[1].Trim());

                            //foreach (EY_MatrTable s in MatrNameList)
                            //{
                            //     row["料名"] = s.MatrName;

                            //}
                            
                            row["盘重"] = b.PanWgt;
                            row["样重"] = b.YangWgt;
                            row["恒重1"] = b.HengWgt1;
                            row["恒重2"] = b.HengWgt2;
                            row["恒重3"] = b.HengWgt3;
                            row["恒重4"] = b.HengWgt4;
                            row["结果"] = b.FinalValue;
                            row["检测员1"] = b.Operator1 + "";


                            row["检测员2"] = b.Operator2+"";
                            row["备注"] = b.Mem1 + "";

                            table.Rows.Add(row);
                           
                        }
                        EY_Scale_Value_Dao.Update_Status(id, "Y");
                        
                    }
                }
              
               
                
              //  row["出生日期"] = new DateTime(1990, 10, 23);
                
               
                return table;

            }
          
            //产制报表
            private void button5_Click_1(object sender, EventArgs e)
              {
                Excel.Application xls_exp = null;
                //int rowindex = 1;
                int colindex = 0;
                //创建一个workbook,一个worksheet  
                Excel._Workbook xls_book = null;
                Excel._Worksheet xls_sheet = null;
                try
                {
                    xls_exp = new Excel.ApplicationClass();
                    xls_book = xls_exp.Workbooks.Add(true);
                    xls_sheet = (Excel._Worksheet)xls_book.ActiveSheet;
                    //C#创建Excel文件之取得数据  
                    DataTable aa = GetData();
                    //将所得到的表的列名,赋值给单元格  
                    foreach (DataColumn col in aa.Columns)
                    {
                        colindex++;
                        int i = 0;
                        i = aa.Rows.Count;
                     
                        xls_exp.Cells[1, colindex] = col.ColumnName;
                        //水平对齐  
                        xls_sheet.get_Range(xls_exp.Cells[1, colindex],
                       xls_exp.Cells[1, colindex]).HorizontalAlignment =
                       Excel.XlVAlign.xlVAlignCenter;
                        //C#创建Excel文件之垂直对齐  
                        xls_sheet.get_Range(xls_exp.Cells[1, colindex],
                        xls_exp.Cells[1, colindex]).VerticalAlignment =
                        Excel.XlVAlign.xlVAlignCenter;
                        //行高、列宽自适应  
                      //  xls_sheet.Cells.Rows.AutoFill(null,Excel.XlAutoFillType.xlFillDefault);

                        ((Excel.Range)xls_sheet.Columns["A:A", System.Type.Missing]).ColumnWidth = 22;
                        ((Excel.Range)xls_sheet.Columns["B:B", System.Type.Missing]).ColumnWidth = 14;
                        ((Excel.Range)xls_sheet.Columns["D:D", System.Type.Missing]).NumberFormatLocal = "0.0000";
                        ((Excel.Range)xls_sheet.Columns["E:E", System.Type.Missing]).NumberFormatLocal = "0.0000";
                        ((Excel.Range)xls_sheet.Columns["F:F", System.Type.Missing]).NumberFormatLocal = "0.0000";
                        ((Excel.Range)xls_sheet.Columns["G:G", System.Type.Missing]).NumberFormatLocal = "0.00";
                        ((Excel.Range)xls_sheet.Columns["H:H", System.Type.Missing]).NumberFormatLocal = "0.00";
                        ((Excel.Range)xls_sheet.Columns["I:I", System.Type.Missing]).NumberFormatLocal = "0.00";
                        ((Excel.Range)xls_sheet.Columns["J:J", System.Type.Missing]).NumberFormatLocal = "0.00";

                        xls_sheet.get_Range(xls_exp.Cells[1, colindex],
                       xls_exp.Cells[i+1, colindex]).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                     
                    }
              
                     int rowIndex=1;
                    int colIndex=0;
                         foreach(DataRow row in aa.Rows)
                 {
                     rowIndex++;
                     colIndex=0;
                     foreach(DataColumn col in aa.Columns)
                {
                         colIndex++;
                         xls_exp.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString();
                    }
                }
                        //不可见,即后台处理  
                        xls_exp.Visible = true;
                        xls_sheet.Protect("123");
                
                }

                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }  
                finally  
                {  
                 xls_exp.Quit();  
                }  
                }
            //退出程序
            private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)

            {
                IList<MTSICS_VO.Login> userlist = new List<MTSICS_VO.Login>();
                userlist = Login_DAO.CheckOutByFlag();
                MTSICS_VO.Login vo = new MTSICS_VO.Login();
                foreach (MTSICS_VO.Login t in userlist)
                {
                     vo.UserName= t.UserName;
                     vo.PassWord = t.PassWord;
                     vo.UserNo = t.UserNo;
                     vo.BanBie = t.BanBie;
                     //vo.Id = t.Id;
                     vo.Flag = "0";
                }
                Login_DAO.ResetFlag(vo);

                Application.Exit();
            }
            //采集仪表数据
            private void button_PRINT_Click(object sender, EventArgs e)
            {

                try
                {
                 
                                        
                    //MT_SICS.MT_SICS mt = new MT_SICS.MT_SICS();
                    mtsics.SendSeriesWeightRequest_E3000Y("1B70");
                  
                    buttonX3.Focus();
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show(ex.Message);

                }



            }
            //单位转换
            private void button_UNIT_Click(object sender, EventArgs e)
            {
                try
                {
                    mtsics.SendSeriesWeightRequest_E3000Y("1B73");

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }
            }
            //计数
            private void button_COUNT_Click(object sender, EventArgs e)
            {
                try
                {
                    mtsics.SendSeriesWeightRequest_E3000Y("1B72");

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }
            }
            //背光
            private void button_LIGHT_Click(object sender, EventArgs e)
            {
                try
                {
                    mtsics.SendSeriesWeightRequest_E3000Y("1B75");

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }
            }
            //去皮
            private void button_TARE_Click(object sender, EventArgs e)
            {
                try
                {
                    mtsics.SendSeriesWeightRequest_E3000Y("1B74");
                    System.Threading.Thread.Sleep(1000);
                    mtsics.SendSeriesWeightRequest_E3000Y("1B70");
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }
            }

            private void timer1_Tick(object sender, EventArgs e)
            {
                mtsics.SendSeriesWeightRequest_E3000Y("1B70");
            }

     
            //删除当前称重项目
            private void buttonX9_Click(object sender, EventArgs e)
            {

                if (MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                
                }
                        
                                
                
                if (dataGridViewX1.CurrentRow ==null)
                {
                   MessageBox.Show("请选择要删除的称重行","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    return;
                }
                int ss = EY_Scale_Value_Status(); 
                if (ss == 1)
                {
                    MessageBox.Show("已产制报表,无法删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }

                try
                {
                    int RowCount = dataGridViewX1.Rows.GetRowCount(DataGridViewElementStates.Displayed) - 1;

                    IList<EY_Scale_Value> list = new List<EY_Scale_Value>();
                    String reg = Convert.ToString(dataGridViewX1.CurrentRow.Cells[1].Value);

                    IList<EY_RegNo_DI> regNoList = EY_RegNo_DI_DAO.GetRegNo3(reg);

                    // 在执行下面语句的时候，会更新RegNo_DI


                    list = EY_RegNo_DI_DAO.GetRegNo2(reg);

                    foreach (EY_Scale_Value t in list)
                    {
                        EY_Scale_Value_Dao.Delete(t);
                    }
                    
                    EY_RegNo_DI_DAO.UpdateStatusBatch(regNoList, "N");

                    this.RefleshData();
                    this.GetBatchNo();
                    this.dataGridViewX1.CurrentRow.Visible = false;
                    for (int i = 0; i < regNoList.Count; i++)
                    {
                        if (regNoList[i].RegNo.Contains("_"))
                        {

                            EY_RegNo_DI_DAO.delete(regNoList[i]);
                        }
                    }
                   
                    RefleshData();



                }

                catch (Exception ex)
                {
                    MessageBox.Show("请选择一个正在称重的项目再点击删除");
                    MessageBox.Show(ex.Message);
                }
            }

            private void button6_Click_1(object sender, EventArgs e)
            {

            }

            private void button4_Click_1(object sender, EventArgs e)
            {

            }
            //打开串口
            private void buttonX14_Click(object sender, EventArgs e)
            {
                try
                {

                    String portName = comboBoxEx1_com.Items[comboBoxEx1_com.SelectedIndex] + "";//端口名称
                    MessageBox.Show(comboBoxEx1_com.Items[comboBoxEx1_com.SelectedIndex] + "");
                    int baudeRate = Convert.ToInt32(comboBoxEx2_baud.Items[comboBoxEx2_baud.SelectedIndex] + "");//波特率
                    Parity parity = (Parity)Enum.Parse(typeof(Parity), comboBoxEx4_check.Items[comboBoxEx4_check.SelectedIndex] + "");//奇偶校验位
                    int dataBits = Convert.ToInt32(comboBoxEx3_data.Items[comboBoxEx3_data.SelectedIndex] + "");
                    StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBoxEx5_stop.Items[comboBoxEx5_stop.SelectedIndex] + "");

                   // mtsics = new MT_SICS.MT_SICS();
                    mtsics = new MT_SICS.MT_SICS(portName, baudeRate, parity, dataBits, stopBits);
                    
                    mtsics.OnDataStable += new MT_SICS.MT_SICS.DataStableEventHandler(mtsics_OnDataStable);

                    mtsics.Open();
                    MessageBox.Show("串口打开成功");
                    //MT_SICS.MT_SICS mt = new MT_SICS.MT_SICS();
                    // mtsics.SendSeriesWeightRequest();
                    timer1.Enabled = true;
                    buttonX14.Enabled = false;
                    buttonX15.Enabled = true;
                    buttonX3.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("串口已打开或打开全口前输入信息不完整");
                    MessageBox.Show(ex.Message);

                }
            }

            private void buttonX15_Click(object sender, EventArgs e)
            {
                try
                {
                    mtsics.Close();
                    MessageBox.Show("串口关闭成功");
                    buttonX14.Enabled = true;
                    buttonX15.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("串口未打开，不需要关闭");
                    MessageBox.Show(ex.Message);

                }
            }
            //新增案件编号
            private void buttonX13_Click(object sender, EventArgs e)
            {
                try
                {
                    EY_RegNo_DI temVo = new EY_RegNo_DI();

                    IList<EY_MatrTable> MatrNameList = EY_MatrTable_DAO.GetMatrName(textBoxX1_Number.Text.Trim().Substring(1, 8));

                    foreach (EY_MatrTable t in MatrNameList)
                    {
                        temVo.CName = t.MatrName;

                    }

                    temVo.RegNo = textBoxX1_Number.Text.Trim();
                    temVo.RevDateTime = DateTime.Now;
                    temVo.Status = "N";
                    temVo.Batch = null;
                    temVo.MatrNo = textBoxX1_Number.Text.Trim().Substring(1, 8);
                    temVo.CheckNo = "1111";

                    EY_RegNo_DI_DAO.Save(temVo);
                    RefleshData();
                }
                catch
                {
                    MessageBox.Show("长度不够，至少9位,格式：Z+料号+***** 例如：Z11001001***");
                }
            }
            //删除案件编号
            private void buttonX12_Click(object sender, EventArgs e)
            {
                EY_RegNo_DI temVo = new EY_RegNo_DI();
                IList<EY_RegNo_DI> list = new List<EY_RegNo_DI>();

                IList<EY_RegNo_DI> checkVO = ListViewItems2Regno(listViewEx1.CheckedItems);
                if (checkVO.Count == 0)
                {
                    MessageBox.Show("至少选中一条案件接收编号");
                }
                else
                {
                    MessageBox.Show(checkVO[0].RegNo);

                    foreach (EY_RegNo_DI t in checkVO)
                    {
                        EY_RegNo_DI_DAO.delete(t);

                    }

                    RefleshData();
                }
            }
            //查询
            private void buttonX10_Click(object sender, EventArgs e)
            {
                DateTime tem = dateTimePicker2.Value;
                string year = tem.Year.ToString();
                string month = tem.Month.ToString();
                string day = tem.Day.ToString();
                string date = year + "/" + month + "/" + day;
                MessageBox.Show(date + " 00:00:00");
                String ss = date + " 00:00:00";
                tem = DateTime.Parse(ss);
                GetBatchNo1(tem);
            }

            private void buttonX11_Click(object sender, EventArgs e)
            {
                Excel.Application xls_exp = null;
                //int rowindex = 1;
                int colindex = 0;
                //创建一个workbook,一个worksheet  
                Excel._Workbook xls_book = null;
                Excel._Worksheet xls_sheet = null;
                try
                {
                    xls_exp = new Excel.ApplicationClass();
                    xls_book = xls_exp.Workbooks.Add(true);
                    xls_sheet = (Excel._Worksheet)xls_book.ActiveSheet;
                    //C#创建Excel文件之取得数据  
                    DataTable aa = GetData();
                    //将所得到的表的列名,赋值给单元格  
                    foreach (DataColumn col in aa.Columns)
                    {
                        colindex++;
                        int i = 0;
                        i = aa.Rows.Count;

                        xls_exp.Cells[1, colindex] = col.ColumnName;
                        //水平对齐  
                        xls_sheet.get_Range(xls_exp.Cells[1, colindex],
                       xls_exp.Cells[1, colindex]).HorizontalAlignment =
                       Excel.XlVAlign.xlVAlignCenter;
                        //C#创建Excel文件之垂直对齐  
                        xls_sheet.get_Range(xls_exp.Cells[1, colindex],
                        xls_exp.Cells[1, colindex]).VerticalAlignment =
                        Excel.XlVAlign.xlVAlignCenter;
                        //行高、列宽自适应  
                       // xls_sheet.Cells.Rows.AutoFill(null,Excel.XlAutoFillType.xlFillDefault);

                        ((Excel.Range)xls_sheet.Columns["A:A", System.Type.Missing]).ColumnWidth = 22;
                        ((Excel.Range)xls_sheet.Columns["B:B", System.Type.Missing]).ColumnWidth = 18;
                        ((Excel.Range)xls_sheet.Columns["C:C", System.Type.Missing]).ColumnWidth = 14;
                        ((Excel.Range)xls_sheet.Columns["D:D", System.Type.Missing]).ColumnWidth = 18;
                        ((Excel.Range)xls_sheet.Columns["E:E", System.Type.Missing]).NumberFormatLocal = "0.00";
                        ((Excel.Range)xls_sheet.Columns["F:F", System.Type.Missing]).NumberFormatLocal = "0.00";
                        ((Excel.Range)xls_sheet.Columns["G:G", System.Type.Missing]).NumberFormatLocal = "0.00";
                        ((Excel.Range)xls_sheet.Columns["H:H", System.Type.Missing]).NumberFormatLocal = "0.00";
                        ((Excel.Range)xls_sheet.Columns["I:I", System.Type.Missing]).NumberFormatLocal = "0.00";
                        ((Excel.Range)xls_sheet.Columns["J:J", System.Type.Missing]).NumberFormatLocal = "0.00";

                        xls_sheet.get_Range(xls_exp.Cells[1, colindex],
                       xls_exp.Cells[i + 1, colindex]).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                    }

                    int rowIndex = 1;
                    int colIndex = 0;
                    foreach (DataRow row in aa.Rows)
                    {
                        rowIndex++;
                        colIndex = 0;
                        foreach (DataColumn col in aa.Columns)
                        {
                            colIndex++;
                            xls_exp.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString();
                        }
                    }
                    //不可见,即后台处理  
                    xls_exp.Visible = true;
                    xls_sheet.Protect("123");

                }

                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
                finally
                {
                    xls_exp.Quit();
                }
            }

            private void groupPanel1_Click(object sender, EventArgs e)
            {

            }

            private void MainFrm_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
            {
                if(e.KeyChar == Convert.ToChar(13))
                {
                   // MessageBox.Show("enter");
                   // buttonX3_Click(sender, e);
                }
            }
     
        }
    }
