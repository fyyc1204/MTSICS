using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MTSICS
{
    public partial class MDIFORM : Form
    {
        public MDIFORM()
        {
            InitializeComponent();
        }

        private void MDIFORM_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
           // this.MaximizeBox = false;
           // this.MinimizeBox = false;
        
            
            
            
            MainFrm maifrm = new MainFrm();
            maifrm.MdiParent = this;
            maifrm.WindowState = FormWindowState.Maximized;
            maifrm.Dock = DockStyle.Fill;
            maifrm.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            UPLoad upload = new UPLoad();
            upload.MdiParent = this;
            upload.Dock = DockStyle.Fill;
            upload.Show();
            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!ExistsMdiChildrenlnstance("MainFrm"))
            {
                MainFrm maifrm = new MainFrm();
                maifrm.MdiParent = this;
                maifrm.WindowState = FormWindowState.Maximized;
                maifrm.Dock = DockStyle.Fill;
                maifrm.Show();
            }
        }


        private bool ExistsMdiChildrenlnstance(string MdiChildrenClassName)
        {
            foreach(Form childFrm in this.MdiChildren)
            {
                if (childFrm.Name == MdiChildrenClassName)
                { 
                if(childFrm.WindowState == FormWindowState.Minimized)
                    {
                        childFrm.WindowState = FormWindowState.Maximized;
                    
                    }
                  childFrm.Activate();
                 return true;
                
                }
            
            }
            return false;
        }

        private void contextMenuBar1_ItemClick(object sender, EventArgs e)
        {

        }

       
        private void menuStrip1_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            if (e.Item.Text.Length == 0 || e.Item.Text == "还原(&R)" || e.Item.Text == "最小化(&N)"||e.Item.Text == "关闭(&C)")

            {
                e.Item.Visible = false;
            }
        }

       


    }
}
