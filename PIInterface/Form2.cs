using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using log4net;

namespace PIInterface
{
    public partial class PIset : Form
    {
        public PIset()
        {
            try
            {
                InitializeComponent();
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("PI数据库设置页面初始化成功");

            }
            catch (Exception ez)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ez);
            }
        }
        public static string ip;
        private static string port;
        private static string account;
        private static string password;

        private void Getvalue()
        {
            try
            {
                ip = this.textBox1.Text.ToString().Trim();
                port = this.textBox2.Text.ToString().Trim();
                account = this.textBox3.Text.ToString().Trim();
                password = this.textBox4.Text.ToString().Trim();

            }
            catch (Exception ew)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ew);
            }
        }
        private void writetoconfig(string ip,string port,string account,string password)
        {
            try
            {
                //获取Configuration对象
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //根据Key读取<add>元素的Value
                string links = config.AppSettings.Settings["ConnectionString"].Value;
                //写入<add>元素的Value
                    config.AppSettings.Settings["PIAddress"].Value = ip;
                    config.AppSettings.Settings["PIport"].Value = port;
                    config.AppSettings.Settings["UID"].Value = account;
                    config.AppSettings.Settings["PWD"].Value = password;
                //一定要记得保存，写不带参数的config.Save()也可以
                config.Save();
                //刷新，否则程序读取的还是之前的值（可能已装入内存）
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("将配置写入config文件");
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
                log.Info("返回主页面");
                this.Close();
                Main main = new Main();
                main.Visible = true;

            }
            catch (Exception ea)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ea);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Getvalue();
                if (!string.IsNullOrEmpty(ip))
                {
                    if (!string.IsNullOrEmpty(port))
                    {
                        if (!string.IsNullOrEmpty(account))
                        {
                            if (!string.IsNullOrEmpty(password))
                            {                              
                                    MessageBox.Show("取数IP:" + ip + "端口为：" + port + "取数帐号:" + account);
                                    log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                                    log.Info("设置ip地址:" + ip + " " + "端口:" + port + " " + "帐号:" + account + " " + "密码:*****");
                                    writetoconfig(ip, port, account, password);
                                    this.Visible = false;
                                    Main main = new Main();
                                    main.Visible = true;                             
                                
                            }
                            else
                            {
                                MessageBox.Show("请输入密码");
                            }
                        }
                        else
                        {
                            MessageBox.Show("请输入取数帐号"); 
                        }
                   
                    }
                    else
                    {
                        MessageBox.Show("请配置端口号");
                    }

                }
                else
                {
                    MessageBox.Show("请配置正确IP地址");
                }
            }
            catch (Exception ee)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ee);
            }
        }
    }
}
