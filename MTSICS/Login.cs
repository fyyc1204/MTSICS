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

namespace MTSICS
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            String userno=textBox1.Text;
            String password=textBox2.Text;
            MTSICS_VO.Login vo = new MTSICS_VO.Login();
            IList<MTSICS_VO.Login> userlist = new List<MTSICS_VO.Login>();
            vo.UserNo = userno;
            vo.PassWord = password;
            vo.Flag = "1";

            userlist= Login_DAO.CheckOut(userno, password);
           
            
            if (userlist.Count == 1)
            {
                vo.BanBie = userlist[0].BanBie;
                vo.UserName = userlist[0].UserName;
               // vo.Id = userlist[0].Id;
                Login_DAO.UpdateFlag(vo);
                //MessageBox.Show("登录成功");
                this.Visible = false;

                MDIFORM mdiparent = new MDIFORM();
                mdiparent.Show();
            }
            else
                MessageBox.Show("用户名或者密码错误");
            
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
