using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace PIInterface
{
    public partial class Main : Form
    {
        public Main()
        {
            try
            {
                InitializeComponent();
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                //log.Debug("debug");
                log.Info("主页面初始化成功");
                //log.Warn("warn");
                //log.Error("error");
                //log.Fatal("Fatal");
                //MessageBox.Show("生成系统日志成功"); 
            }
            catch (Exception ez)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ez);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("打开上传页面");
                UploadData updata = new UploadData();
                updata.Show();
                this.Visible = false;

            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("打开Oracle数据库设置页面");
                DatabaseSet set = new DatabaseSet();
                set.Show();
                this.Visible = false;

            }
            catch (Exception ee)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ee);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("退出程序");

            }
            catch (Exception ea)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ea);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("打开PI数据库配置页面");
                PIset setpi = new PIset();
                setpi.Show();
                this.Visible = false;


            }
            catch (Exception es)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(es);
            }
        }


    }
}
