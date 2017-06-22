using System;
using System.Data;
using System.Text;
using PISDK;
using PISDKCommon;
using PITimeServer;

namespace PIInterface
{
     public class PiHelper
     {
         PISDK.Server server;
         PISDK.PISDK pisdk = null;
         string uid = null;
         string pwd = null;
         int piPort;
         string hostName = null;
         string piConnectionString = "";
         /// <summary>
         /// PiHelper构造函数
         /// </summary>
         /// <param name="hostName">pi服务器名</param>
         /// <param name="uid">登录服务器的用户id</param>
         /// <param name="pwd">登录密码</param>
         /// <param name="piPort">pi服务器端口</param>
         public PiHelper(string hostName, string uid, string pwd, int piPort)
         {
             this.hostName = hostName;
             this.uid = uid;
             this.pwd = pwd;
             this.piPort = piPort;
             piConnectionString = string.Format("UID={0};PWD={1};port={2};Host={3};", uid, pwd, piPort, hostName);
             try
             {
                 pisdk = new PISDK.PISDK();
                 foreach (PISDK.Server Iserver in pisdk.Servers)
                 {
                     if (Iserver.Name.Equals(hostName))
                     {
                         this.server = Iserver;

                         //初始化PIServer日志 by Nutk'Z 141111
                         //log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                         //log.Info("初始化PIServer成功");
                         logs.writelog("初始化PIServer成功");
                     }
                 }
             }
             catch (Exception ex)
             {
                 log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                 log.Error(ex);
             }
         }
         PiHelper()
         {
             if (this.server != null)
             {
                 this.server.Close();
             }
         }

         protected void Dispose(bool disposing)
         {
             //释放非托管资源
             if (this.server != null)
             {
                 System.Runtime.InteropServices.Marshal.ReleaseComObject(this.server);
             }
             if (this.pisdk != null)
             {
                 System.Runtime.InteropServices.Marshal.ReleaseComObject(this.pisdk);
             }
             GC.Collect();
         }

         public void Dispose()
         {
             this.Dispose(true);
             GC.ReRegisterForFinalize(this);
         }

         /// <summary>
         /// 获取实时数据
         /// </summary>
         /// <param name="tagName">查询条件</param>
         /// <returns></returns>
         public virtual string[] GetSnapData(string[] tagName, string startime)
         {
             //PIValue piValue = null;
             PIPoint[] pt = new PIPoint[tagName.Length];
             PISDK.DigitalState myDigState;
             string[] pivalue = new string[tagName.Length];
             if (this.server == null)
             {
                 //log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                 //log.Debug("PIServer未初始化");
                 logs.writelog("PIServer未初始化");
                 throw new Exception("piserver未初始化。");
                                  
             }
             PIValue sb;
             DateTime strtime;
             if (startime != "")
                 strtime = DateTime.Parse(startime);
             else
                 strtime = System.DateTime.Now;
             try
             {
                 if (!this.server.Connected)
                 {
                     this.server.Open(this.piConnectionString);
                 }
                 for (int i = 0; i < tagName.Length; i++)
                 {
                     pt[i] = this.server.PIPoints[tagName[i].ToString()];
                     sb = pt[i].Data.Snapshot;
                     if (sb.Value.GetType().IsCOMObject)
                     {
                         myDigState = (PISDK.DigitalState)sb.Value;
                         pivalue[i] = tagName[i] + "," + myDigState.Name;
                     }
                     else
                     {
                         pivalue[i] = tagName[i] + "," + sb.Value.ToString();
                     }
                 }
             }
             catch (Exception ex)
             {
                 //log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                 //log.Error(ex);
                 logs.writelog("获取实时数据发生错误:" + ex);
                 throw new Exception("获取实时数据发生错误:" + ex.Message);
             }
             return pivalue;
         }
         /// <summary>
         /// 获取极值
         /// </summary>
         /// <param name="tagName">查询条件</param>
         /// <param name="startTime">起始时间</param>
         /// <param name="endTime">结束时间</param>
         /// <param name="nv"></param>
         /// <param name="atc"></param>
         /// <param name="qs">极值的类型，年份，月份，天</param>
         /// <returns></returns>
         public virtual StringBuilder GetSummary(string tagName, DateTime startTime, DateTime endTime, out NamedValues nv, ArchiveSummaryTypeConstants atc, QueryStyle qs)
         {
             PointList pLst = null;
             if (this.server == null)
             {
                 //log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                 //log.Debug("PIServer未初始化");
                 logs.writelog("PIServer未初始化");
                 throw new Exception("piserver未初始化。");
             }
             StringBuilder sb = new StringBuilder();
             try
             {
                 PIData pData = null;
                 PIValue pValue = null;
                 switch (qs)
                 {
                     case QueryStyle.Year:
                         startTime = DateTime.Parse(startTime.Year.ToString() + "-01-01 00:00:00");
                         endTime = DateTime.Parse(endTime.Year.ToString() + "12-31 23:59:59");
                         break;
                     case QueryStyle.Month:
                         startTime = DateTime.Parse(startTime.Year.ToString() + "-" + startTime.Month.ToString() + "-01 00:00:00");
                         endTime = DateTime.Parse(endTime.Year.ToString() + "-" + endTime.Month.ToString() + "-31 23:59:59");
                         break;
                     default:
                         break;
                 }
                 PITime ptStart = new PITime();
                 ptStart.LocalDate = startTime;
                 PITime ptEnd = new PITime();
                 ptEnd.LocalDate = endTime;
                 if (!this.server.Connected)
                 {
                     this.server.Open(this.piConnectionString);
                 }
                 nv = null;
                 pLst = this.server.GetPoints(tagName, null);//此时pLst中不会存在数据，也就是客户端现在还没有数据
                 foreach (PIPoint point in pLst)
                 {
                     pData = point.Data;//取数据
                     pValue = pData.Summary(ptStart, ptEnd, atc, CalculationBasisConstants.cbTimeWeighted, new PIAsynchStatus());//取极值数据
                     sb.AppendFormat(point.Name + "|" + pValue.TimeStamp.LocalDate.ToString() + "|" + pValue.Value.ToString() + "!");
                 }
                 sb.Remove(sb.Length - 1, 1);
             }
             catch (Exception ex)
             {
                 //log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                 //log.Error(ex);
                 logs.writelog("获取极值发生错误:" + ex);
                 throw new Exception("获取极值发生错误:" + ex.Message);
             }
             return sb;
         }
         /// <summary>
         /// 查询间隔数据
         /// </summary>
         /// <param name="tagName">查询条件</param>
         /// <param name="startTime">起始时间</param>
         /// <param name="endTime">结束时间</param>
         /// <param name="sampleInterVal">间隔时间：1h，2h，3h，4h</param>
         /// <param name="fvc">过滤方式</param>
         /// <param name="filter">过滤</param>
         /// <returns></returns>
         public virtual DataTable GetInterpolatedValues(string[] tagName, DateTime startTime, DateTime endTime, object sampleInterVal, string filter)
         {
             if (this.server == null)
             {
                 //log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                 //log.Debug("PIServer未初始化");
                 logs.writelog("PIServer未初始化");
                 throw new Exception("PIserver未初始化。");
             }
             //PIValues pivalues = null;
             DataSet ds = new DataSet();
             //声明一个数据表，表名为LoadValue
             DataTable dt = new DataTable("LoadValue");
             //该表有3列，分别为piID，ptValue，ptTime
             dt.Columns.Add("ptID", typeof(System.String));
             dt.Columns.Add("ptValue", typeof(System.String));
             dt.Columns.Add("ptTime", typeof(DateTime));

             //StringBuilder sb = new StringBuilder();
             //由于历史数据可能过多，所以选择用datatable返回
             try
             {
                 if (!this.server.Connected)
                 {
                     this.server.Open(this.piConnectionString);
                 }
                 //分别对pointID取数
                 for (int i = 0; i < tagName.Length; i++)
                 {
                     //pisdk的pipoint为pointID对应的原子点名
                     PISDK.PIPoint piPoint = this.server.PIPoints[tagName[i].ToString()];
                     //取2min内的2个点                   
                     PISDK.PIValues piValues = piPoint.Data.InterpolatedValues(
                         System.DateTime.Now.AddMinutes(-2),
                         System.DateTime.Now.AddMinutes(-1),
                         3,
                         "1=1",
                         PISDK.FilteredViewConstants.fvShowFilteredState,
                         new PISDKCommon.PIAsynchStatus());
                     if (piValues.Count > 0)
                     {
                         //将每个点对应的数据和事件加入到数据表LoadValue中
                         for (int j = 1; j <= piValues.Count; j++)
                         {
                             DataRow dr = dt.NewRow();
                             dr["ptID"] = tagName[i];
                             dr["ptValue"] = piValues[j].Value.ToString();
                             dr["ptTime"] = piValues[j].TimeStamp.LocalDate;
                             dt.Rows.Add(dr);
                         }
                     }
                 }

             }
             catch (Exception ex)
             {
                 //log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                 //log.Error(ex);
                 logs.writelog("按照时间间隔查询数据发生错误:" + ex);
                 throw new Exception("按照时间间隔查询数据发生错误:" + ex.Message);
             }
             return dt;
         }
     }
}
