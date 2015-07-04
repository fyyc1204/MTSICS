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
using System.Net;

namespace MTSICS
{
    public partial class UPLoad : Form
    {
        public UPLoad()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UPLoad_Load(object sender, EventArgs e)
        {

            IList<EY_Scale_Value> EY_Scale_Value = EY_Scale_Value_Dao.GetScaleToErp( "0");

            displaydatagridview(EY_Scale_Value);
           

          

        }

        private  void displaydatagridview(IList<EY_Scale_Value> uploaddata)
          {

              this.dataGridViewX1.Rows.Clear();
              int i = 0;
              string status;
              string flag;

           foreach (EY_Scale_Value t in uploaddata)
            {
                this.dataGridViewX1.Rows.Add();
                this.dataGridViewX1.Rows[i].Cells[1].Value = t.Date;
                this.dataGridViewX1.Rows[i].Cells[2].Value = t.Id.RegNo;
                this.dataGridViewX1.Rows[i].Cells[3].Value = t.Id.CheckNo;
                this.dataGridViewX1.Rows[i].Cells[4].Value = t.Matrname;
                this.dataGridViewX1.Rows[i].Cells[5].Value = t.FinalValue; ;
                if(t.ToErpFlag == "1")
                     status = "已上传";
                else
                {
                status = "未上传";
                }
                this.dataGridViewX1.Rows[i].Cells[6].Value = status;
                if (t.Status == "Y")
                {
                    flag = "已产制";
                }
                else
                {
                    flag = "未产制";
                }
                this.dataGridViewX1.Rows[i].Cells[7].Value = flag;
                this.dataGridViewX1.Rows[i].Cells[8].Value = t.Operator1;
                this.dataGridViewX1.Rows[i].Cells[9].Value = t.Operator2;
                this.dataGridViewX1.Rows[i].Cells[10].Value = t.Mem1;
                
                i++;
                           
            }
    
    }





        //上传
        private void button1_Click(object sender, EventArgs e)
        {

          

        }
        //上传ERP
        private void buttonX1_Click(object sender, EventArgs e)
        {

            string mess = "";
            int k = 0;

            int count = Convert.ToInt32(this.dataGridViewX1.Rows.Count.ToString());

            for (int i = 0; i < count; i++)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridViewX1.Rows[i].Cells["Column2"];

                Boolean flag = Convert.ToBoolean(checkCell.Value);

                if (flag == true)
                {



                    if (this.dataGridViewX1.Rows[i].Cells[6].Value.ToString().Trim() == "未上传")
                    {
                        
                        string date = Convert.ToDateTime(this.dataGridViewX1.Rows[i].Cells[1].Value).ToString("yyyyMMddHHmmss").Substring(0,8).Trim();
                        string time = Convert.ToDateTime(this.dataGridViewX1.Rows[i].Cells[1].Value).ToString("yyyyMMddHHmmss").Substring(8, 6).Trim();
                        double water = Convert.ToDouble(this.dataGridViewX1.Rows[i].Cells[5].Value);
                        string str1 = water.ToString("0.00");
                        mess = "N" + "###" + this.dataGridViewX1.Rows[i].Cells[2].Value.ToString().Trim() + "###" + str1 + "###" + date + "###" + time;
                        tbdipdo tbdipdo = new tbdipdo();
                        tbdipdo.TimeStamp = Convert.ToDecimal(DateTime.Now.ToString("yyyyMMddHHmmss"));
                        tbdipdo.Status = "N";
                        tbdipdo.SerialNo = Convert.ToDecimal(k);

                        tbdipdo.Header = "192.168.100.92" + "PJKFPTC" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        tbdipdo.QueueId = "PJKFPTC";
                        tbdipdo.Data = mess;
                        tbdipdo_DAO.TBDIPDOSAVE(tbdipdo);

                        try
                        {
                            EY_Scale_Value_PK SPL = new EY_Scale_Value_PK();
                            SPL.RegNo = Convert.ToString(this.dataGridViewX1.Rows[i].Cells[2].Value);
                            SPL.CheckNo = Convert.ToString(this.dataGridViewX1.Rows[i].Cells[3].Value);
                            EY_Scale_Value_Dao.Update_ToErpFlag(SPL, "1");
                        
                        }
                        catch (Exception ex)

                        {
                            MessageBox.Show(ex.Message);
                        }

                        //EY_Scale_Value_PK SPL = new EY_Scale_Value_PK();
                        //SPL.RegNo = Convert.ToString(this.dataGridViewX1.Rows[i].Cells[2].Value);
                        //SPL.CheckNo = Convert.ToString(this.dataGridViewX1.Rows[i].Cells[3].Value);
                        //EY_Scale_Value_Dao.Update_ToErpFlag(SPL, "1");
                        k++;



                    }



                }


            }

            MessageBox.Show("上传成功" + k.ToString() + "笔数据");


            IList<EY_Scale_Value> EY_Scale_Value = EY_Scale_Value_Dao.GetScaleToErp("0");

            displaydatagridview(EY_Scale_Value);








        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {

        }
        //查询
        private void buttonX2_Click(object sender, EventArgs e)
        {
            int k = 0;
            if (checkBoxX1.Checked == true)
            {
                k = 1;
            }
            //this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //this.dateTimePicker1.CustomFormat = "yyyy年MM月dd日  ";
            //this.dateTimePicker1.ShowUpDown = true;
            DateTime tem = dateTimePicker1.Value;

            string year = tem.Year.ToString();
            string month =   tem.Month.ToString();
            string day = tem.Day.ToString();
            string date = year+"/"+month+"/"+day;
            //MessageBox.Show(tem.ToString().Substring(0, 9) + " 00:00:00");
            //String ss = tem.ToString().Substring(0, 10).Trim() + " 00:00:00";
            String ss = date + " 00:00:00";
            tem = DateTime.Parse(ss);

            this.displaydatagridview(EY_Scale_Value_Dao.SearchByTime(tem, tem.AddHours(24),k));





        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }




    }
}
