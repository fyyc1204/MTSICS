using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTSICS_VO
{
    public class Login
    {
        private String userName;

        public virtual String UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private String passWord;

        public virtual String PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }
        private String flag;

        public virtual String Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        private String userno;

        public virtual String UserNo
        {
            get { return userno; }
            set { userno = value; }
        }

        private String banbie;

        public virtual String BanBie
        {
            get { return banbie; }
            set { banbie = value; }
        }

        //private int id ;

        //public virtual int Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
    }
}
