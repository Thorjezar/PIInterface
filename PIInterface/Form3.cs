using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;
using System.Text.RegularExpressions;
using System.Configuration;
using log4net;

namespace PIInterface
{
    public partial class UploadData : Form
    {
        public UploadData()
        {
            try
            {
                InitializeComponent();
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("数据上传页面初始化成功");

            }
            catch (Exception ez)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ez);
            }
        }

        #region 参数
        public static int linkstate;
        public static int pilink;
        private int jiange;
        private string jg;
        string hostName;
        string uid;
        string pwd;
        int piPort;
        string[] strin;
        PiHelper pi;
        string[] str;
        #endregion

        #region 行为动作
        private void label4_Click(object sender, EventArgs e)
        {
            string files = @"数据操作日志\";
            if (Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + files))
            {
                System.Diagnostics.Process.Start("explorer.exe", System.AppDomain.CurrentDomain.BaseDirectory + files);
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("查看数据操作日志");
            }
            else
            {
                DialogResult dr = MessageBox.Show("没有数据操作日志文件！");
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Debug("没有数据操作日志");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {   
                Application.Exit();

            }
            catch (Exception ea)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ea);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var reg = new Regex("^[0-9]*$");
            var str = textBox1.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 允许输入:数字、退格键(8)、全选(1)、复制(3)、粘贴(22)
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 &&
            e.KeyChar != 1 && e.KeyChar != 3 && e.KeyChar != 22)
            {
                e.Handled = true;
                MessageBox.Show("输入正确时间格式!");
            }
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Red;
            this.label4.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        }

        private void label4_MouseMove(object sender, MouseEventArgs e)
        {
            label4.ForeColor = Color.Blue;
            this.label4.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        }

        private void UploadData_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    this.WindowState = FormWindowState.Minimized;
                    notifyIcon1.Visible = true;
                    this.Hide();
                    log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                    log.Info("最小化取数客户端");
                    return;
                }
                else
                {
                    if (timer1.Enabled == true)
                    {
                        DialogResult result = MessageBox.Show("数据正在上传中，确定要退出程序吗？(退出可能会发生未知错误)", "警告消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result == DialogResult.OK)
                        {
                            e.Cancel = false;  //点击OK
                            timer1.Stop();
                            log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                            log.Info("退出程序");
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("退出程序", "警告消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result == DialogResult.OK)
                        {
                            e.Cancel = false;  //点击OK
                            timer1.Stop();
                            log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                            log.Info("退出程序");
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
            catch (Exception ea)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ea);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("还原取数客户端窗口");
                notifyIcon1.Visible = false;
                this.Show();
                WindowState = FormWindowState.Normal;
                this.Focus();
            }
            catch (Exception es)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(es);
            }
        }

        private void 还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Info("还原取数客户端窗口");
                notifyIcon1.Visible = false;
                this.Show();
                WindowState = FormWindowState.Normal;
                this.Focus();
            }
            catch (Exception es)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(es);
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();

            }
            catch (Exception ea)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ea);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("确定要停止吗？", "警告消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    if (timer1.Enabled == true)
                    {
                        timer1.Stop();
                        logs.writelog("停止上传");
                        log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                        log.Info("点击停止");
                    }
                    else
                    {
                        MessageBox.Show("没有开始上传");
                        return;
                    }
                }
            }
            catch (Exception eq)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(eq);
            }
        }

        private void UploadData_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                    log.Info("最小化取数客户端");
                    notifyIcon1.Visible = true;
                    this.Hide();
                }
            }
            catch (Exception ee)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ee);
            }
        }

        private void UploadData_Load(object sender, EventArgs e)
        {
            string dataname = DatabaseSet.dataname;
            string piname = PIset.ip;
            if (!string.IsNullOrEmpty(dataname))
            {
                if (!string.IsNullOrEmpty(piname))
                {
                    linkstate = 1;
                    pilink = 1;
                }
                else
                {
                    try
                    {
                        DialogResult dr = MessageBox.Show("没有设置PI参数！");
                        pilink = 0;

                    }
                    catch (Exception ee)
                    {
                        log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                        log.Debug(ee);
                    }
                }
            }
            else
            {
                try
                {
                    DialogResult dr = MessageBox.Show("没有配置数据库！");

                    linkstate = 0;

                }
                catch (Exception ea)
                {
                    log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                    log.Debug(ea);
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string[] newstr;
            newstr = pi.GetSnapData(strin, System.DateTime.Now.AddMinutes(2).ToString());
            for (int i = 0; i < newstr.Length; i++)
            {
                if (newstr[i].ToUpper() != str[i].ToUpper())//如果值有变化则进行更新
                {
                    string[] aa = newstr[i].Split(',');
                    //如果取上来的数据是错误或者是坏数据就置0
                    if (aa[1].Contains("bad") || aa[1].Contains("Sample"))
                    {
                        aa[1] = "0";
                    }
                    bool soap_result = OPCAdd(aa[0], aa[1], "0");
                    if (soap_result)
                    {
                        str[i] = newstr[i];
                    }
                }
            }
            GetData(newstr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                jg = this.textBox1.Text.ToString().Trim();
                if (timer1.Enabled == false)    //linkstate = 1;pilink = 1;                  
                {
                    if (linkstate == 1)
                    {
                        if (pilink == 1)
                        {
                            if (!string.IsNullOrEmpty(jg))
                            {
                                jiange = int.Parse(jg);
                                if (jiange < 1)
                                {
                                    DialogResult dr = MessageBox.Show("刷新时间不能小于1");
                                }
                                else
                                {
                                    bool rl;
                                    log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                                    log.Info("点击上传");
                                    logs.writelog("开始上传数据");
                                    rl = Startup();
                                    if (rl)
                                    {
                                        timer1.Interval = 1000 * 60 * jiange;
                                        timer1.Start();
                                    }
                                    else
                                    {
                                        DialogResult dr = MessageBox.Show("数据上传失败！错误原因请查看日志");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                DialogResult dr = MessageBox.Show("请设置时间间隔");
                            }
                        }
                        else
                        {
                            DialogResult dr = MessageBox.Show("PI数据库未配置！");
                        }
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("数据库未配置！");
                    }

                }
                else
                {
                    DialogResult dr = MessageBox.Show("正在上传，请勿重复操作！");
                }

            }
            catch (Exception ea)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ea);
            }
        }
        #endregion

        #region 数据操作

        /// <summary>
        /// 上传操作
        /// </summary>
        /// <returns></returns>
        private bool Startup()
        {
            try
            {
                hostName = ConfigurationManager.AppSettings["PIAddress"].ToString();
                uid = ConfigurationManager.AppSettings["UID"].ToString();
                pwd = ConfigurationManager.AppSettings["PWD"].ToString();
                piPort = Convert.ToInt16(ConfigurationManager.AppSettings["PIport"]);
                pi = new PiHelper(hostName, uid, pwd, piPort);

                strin = PIGetPointName(" dbm.DATASOURCE = 'P' and dbm.MINGC is not null ");
                
                str = pi.GetSnapData(strin, System.DateTime.Now.AddMinutes(2).ToString());
                GetData(str);
                for (int i = 0; i < str.Length; i++)
                {
                    string[] aa = str[i].Split(',');
                    //如果取上来的数据是错误或者是坏数据就置0
                    if (aa[1].Contains("bad") || aa[1].Contains("Sample"))
                    {
                        aa[1] = "0";
                    }
                    bool soap_result = OPCAdd(aa[0], aa[1], "0");
                    if (!soap_result)
                    {
                        logs.writelog("PI数据" + aa[0] + "上传失败!");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logs.writelog("错误:" + ex.Message.ToString());
                return false;

            }

        }

        /// <summary>
        /// 从别名表查询数据
        /// </summary>
        /// <param name="strwhere">查询条件</param>
        /// <returns></returns>
        public DataSet Select_dianm(string strwhere)
        {
            StringBuilder strSql = new StringBuilder();
            DataSet ds = new DataSet();
            try
            {
                strSql.Append(" select dy.mingc        dymingc,");
                strSql.Append(" dbm.mingc       mingc,");
                strSql.Append(" dbm.biem        biem,");
                strSql.Append(" dy.qy_dianyboid qy_dianyboid");
                strSql.Append(" from qy_dianbmb dbm, qy_dianyb dy");
                strSql.Append("  where dbm.qy_dianyb_fk = dy.qy_dianyboid");
                strSql.Append(" and dy.status = 1   ");
                if (strwhere != "")
                {
                    strSql.Append(" and " + strwhere);
                }
                ds = DbHelperOra.Query(strSql.ToString());
                return ds;
            }
            catch (Exception ee)
            {
                //写数据操作日志
                logs.writelog("查询别名表出错:" + ee);
                throw ee;
            }
        }

        /// <summary>
        /// 得到点名称
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public string[] PIGetPointName(string strWhere)
        {
            DataSet ds = new DataSet();
            string[] _results = null;
            ds = Select_dianm(strWhere);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string[] str = new string[ds.Tables[0].Rows.Count];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str[i] = ds.Tables[0].Rows[i]["mingc"].ToString();
                }
                _results = str;
            }
            return _results;
        }

        /// <summary>
        /// 得到PI数据
        /// </summary>
        /// <param name="data"></param>
        private void GetData(string[] data)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("tb");
            DataRow dr;
            dt.Columns.Add("名称", typeof(string));
            dt.Columns.Add("值", typeof(string));
            for (int i = 0; i < data.Length; i++)
            {
                string[] aa = data[i].Split(',');
                dr = dt.NewRow();
                dr["名称"] = aa[0];
                dr["值"] = aa[1];
                dt.Rows.Add(dr);
            }
            ds.Tables.Add(dt);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "tb";
        }

        /// <summary>
        /// 数据添加
        /// </summary>
        /// <param name="name">点名</param>
        /// <param name="shuj">数据</param>
        /// <param name="type">1:OPC,0:PI</param>
        /// <returns></returns>
        public bool OPCAdd(string name, string shuj, string type)
        {
            bool result = false;
            try
            {
                string biemid = GetIDbyBM(name);
                QY_DIANMB model = new QY_DIANMB();
                model.DATATIME = DateTime.Now;
                model.DATAVALUE = shuj;
                model.MINGC = name;
                if (biemid == "")
                {
                    model.QY_DIANBMB_FK = "0";
                }
                model.QY_DIANBMB_FK = biemid;
                //lock
                lock (this)
                {
                    int count = CheckCount(model.DATATIME.ToString("yyyy-MM-dd HH:mm:ss"), model.MINGC, model.DATAVALUE);
                    if (count == 0)
                    {
                        QY_DIANBMB dianbm = GetDataType(name);
                        if (dianbm.DATATYPE == "B")
                        {
                            if (Exists(name))
                            {
                                result = Update(model);
                            }
                            else
                            {
                                result = Add(model);
                            }
                        }
                        if (dianbm.DATATYPE == "S")
                        {
                            result = Add(model);
                        }
                        //if (type == "1") 取煤批次
                        //{
                        //    QUMPCAdd(name, shuj);
                        //}
                        //else
                        //{
                        //    RLPCAdd(name, shuj);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                logs.writelog("添加错误" + ex.Message);
                throw;
            }
            return result;
        }

        /// <summary>
        /// 获取别名表ID
        /// </summary>
        /// <param name="biem"></param>
        /// <returns></returns>
        public string GetIDbyBM(string biem)
        {
            DataTable dt = GetIDbyBm(biem).Tables[0];
            if (dt.Rows.Count != 0)
            {
                return dt.Rows[0]["QY_DIANBMBOID"].ToString();
            }
            return "";
        }

        /// <summary>
        /// 通过别名获取对应ID
        /// </summary>
        /// <param name="biem"></param>
        /// <returns></returns>
        public DataSet GetIDbyBm(string biem)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from QY_DIANBMB where MINGC='" + biem + "'");
            return DbHelperOra.Query(sql.ToString());
        }

        /// <summary>
        /// 获取是否有重复数据
        /// </summary>
        /// <param name="time"></param>
        /// <param name="mingc"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public int CheckCount(string time, string mingc, string val)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select mingc, datatime, datavalue from QY_DIANMB ");
            strSql.Append(" where mingc = '" + mingc + "' ");
            strSql.Append(" order by datatime desc ");
            DataTable dt = DbHelperOra.Query(strSql.ToString()).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                decimal pval = !string.IsNullOrEmpty(dt.Rows[0]["datavalue"].ToString()) ? Convert.ToDecimal(dt.Rows[0]["datavalue"]) : 0;
                decimal nval = Convert.ToDecimal(val);

                if (pval == nval)
                {
                    return 1;
                }

                DateTime pdt = !string.IsNullOrEmpty(dt.Rows[0]["datatime"].ToString()) ? Convert.ToDateTime(dt.Rows[0]["datatime"]) : DateTime.Now;
                DateTime ndt = Convert.ToDateTime(time);

                if ((ndt - pdt).Minutes <= 5)
                {
                    return 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// 获取点数据类型
        /// </summary>
        /// <param name="mingc"></param>
        /// <returns></returns>
        public QY_DIANBMB GetDataType(string mingc)
        {
            try
            {
                string sql = "select * from qy_dianbmb where mingc='" + mingc + "'";
                DataTable dt = DbHelperOra.Query(sql).Tables[0];
                QY_DIANBMB model = new QY_DIANBMB();
                if (dt.Rows.Count != 0)
                {
                    model.BEIZ = dt.Rows[0]["BEIZ"].ToString();
                    model.BIEM = dt.Rows[0]["BIEM"].ToString();
                    model.QY_DIANBMBOID = dt.Rows[0]["QY_DIANBMBOID"].ToString();
                    model.DATATYPE = dt.Rows[0]["DATATYPE"].ToString();
                    model.MINGC = dt.Rows[0]["MINGC"].ToString();
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logs.writelog("别名表GetDataType方法" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string mingc)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select count(1) from qy_dianmb");
                strSql.Append(" where mingc=:mingc ");
                OracleParameter[] parameters = {
					new OracleParameter(":mingc", OracleType.VarChar,50)};
                parameters[0].Value = mingc;
                return DbHelperOra.Exists(strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                logs.writelog("点名表Exists方法" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(QY_DIANMB model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update qy_dianmb set");
                strSql.Append(" MINGC=:MINGC,");
                strSql.Append(" DATATIME=:DATATIME,");
                strSql.Append(" DATAVALUE=:DATAVALUE");
                strSql.Append(" where QY_DIANBMB_FK=:QY_DIANBMB_FK");
                OracleParameter[] para ={
                    new OracleParameter(":QY_DIANBMB_FK",OracleType.VarChar,36),
                new OracleParameter(":MINGC",OracleType.VarChar,50),
                new OracleParameter(":DATATIME",OracleType.DateTime),
                new OracleParameter(":DATAVALUE",OracleType.VarChar,36)
            };
                para[0].Value = model.QY_DIANBMB_FK;
                para[1].Value = model.MINGC;
                para[2].Value = model.DATATIME;
                para[3].Value = model.DATAVALUE;
                int rows = DbHelperOra.ExecuteSql(strSql.ToString(), para);
                if (rows > 0)
                {
                    string los = "更新点名表数据: 点名:" + model.MINGC + " " + "时间:" + model.DATATIME + " " + "值:" + model.DATAVALUE;
                    logs.writelog(los);
                    return true;
                }
                else
                {
                    logs.writelog("点名表没有可以更新的数据");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logs.writelog("update点名表出错"+ex.Message );
                throw;
            }
        }

        /// <summary>
        /// 添加点名表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(QY_DIANMB model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QY_DIANMB(");
            strSql.Append("MINGC,DATATIME,DATAVALUE,QY_DIANBMB_FK)");
            strSql.Append(" values (");
            strSql.Append(":MINGC,:DATATIME,:DATAVALUE,:QY_DIANBMB_FK)");
            OracleParameter[] para ={
                new OracleParameter(":MINGC",OracleType.VarChar,50),
                new OracleParameter(":DATATIME",OracleType.DateTime),
                new OracleParameter(":DATAVALUE",OracleType.VarChar,36),
                new OracleParameter(":QY_DIANBMB_FK",OracleType.VarChar,36)
                                   };
            para[0].Value = model.MINGC;
            para[1].Value = model.DATATIME;
            para[2].Value = model.DATAVALUE;
            para[3].Value = model.QY_DIANBMB_FK;
            int row = DbHelperOra.ExecuteSql(strSql.ToString(), para);
            if (row > 0)
            {
                string los = "点名表新增数据: 点名:" + model.MINGC + " " + "时间:" + model.DATATIME + " " + "值:" + model.DATAVALUE + " " + "别名外键:" + model.QY_DIANBMB_FK;
                logs.writelog(los);
                return true;
            }
            else
            {
                logs.writelog("点名表新增数据失败");
                return false;
            }
        }
        #endregion

    }
}
