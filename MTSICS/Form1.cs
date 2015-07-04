using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MT_SICS;
using System.IO.Ports;
using NHibernate;
using NHibernate.Cfg;
using MTSICS_VO;
using MTSICS_DAO;
namespace MTSICS
{
    public partial class Form1 : Form
    {
        private MT_SICS.MT_SICS mtsics = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String portName = textBox2.Text;//端口名称
            int baudeRate =Convert.ToInt32(textBox3.Text);//波特率
            Parity parity = (Parity)Enum.Parse(typeof(Parity), textBox4.Text);//奇偶校验位
            int dataBits = Convert.ToInt32(textBox5.Text);
            StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), textBox6.Text);

            mtsics = new MT_SICS.MT_SICS(portName, baudeRate, parity, dataBits, stopBits);
            mtsics.OnDataStable+=new MT_SICS.MT_SICS.DataStableEventHandler(mtsics_OnDataStable);
            mtsics.Open();
            MessageBox.Show("串口打开成功");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mtsics.Close();
            MessageBox.Show("串口关闭成功");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mtsics.SendSeriesWeightRequest();
        }

        private void mtsics_OnDataStable(object sender, DataStableEventArgs args)
        {
            textBox1.BeginInvoke(new Action(() =>
            {
                textBox1.Text += args.OrgData;
            }));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("dasdasasdas");
            MessageBox.Show(sb.ToString());


        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            IList<RegNo_DI> dd = RegNo_DI_DAO.GetRegNo("");

        }
    }
}
