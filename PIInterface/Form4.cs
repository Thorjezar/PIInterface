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
    public partial class DatabaseSet : Form
    {
        public DatabaseSet()
        {
            try
            {
                InitializeComponent();
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("数据库设置页面初始化成功");

            }
            catch (Exception ez)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ez);
            }
        }

        public static string dataname;
        public static string name;
        private string password;
        private string link;


        /// <summary>
        /// 获取窗体传来的值
        /// </summary>
        private void Huoqu()
        {
            try
            {
                dataname = this.textBox1.Text.ToString().Trim();
                name = this.textBox2.Text.ToString().Trim();
                password = this.textBox3.Text.ToString().Trim();
                link = "Data Source=" + dataname + ";User Id=" + name + ";Password=" + password;

            }
            catch (Exception eq)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(eq);
            }
        }

        /// <summary>
        /// 给config文件写入
        /// </summary>
        /// <param name="l"></param>
        private void Intoconfig(string l)
        {
            try
            {
                //获取Configuration对象
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //根据Key读取<add>元素的Value
                string links = config.AppSettings.Settings["ConnectionString"].Value;
                //写入<add>元素的Value
                config.AppSettings.Settings["ConnectionString"].Value = l;
                //一定要记得保存，写不带参数的config.Save()也可以
                config.Save();
                //刷新，否则程序读取的还是之前的值（可能已装入内存）
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("将数据库配置写入config文件");
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
            catch (Exception ee)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ee);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Huoqu();
                if (!string.IsNullOrEmpty(dataname))
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (!string.IsNullOrEmpty(password))
                        {
                            
                                MessageBox.Show("连接的数据库名称为:" + dataname + "用户为：" + name);
                                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                                log.Info("设置数据库名称:" + dataname + " " + "用户名:" + name + " " + "密码:****");
                                Intoconfig(link);
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
                        MessageBox.Show("请输入用户");
                    }

                }
                else
                {
                    MessageBox.Show("请配置正确的数据库");
                }
            }
            catch(Exception ee)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ee);
            }
        }
    }
}
