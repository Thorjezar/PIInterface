using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIInterface
{
    /// <summary>
    /// 点名表
    /// </summary>
    [Serializable]
    public class QY_DIANMB
    {
        private string mingc;

        /// <summary>
        /// 名称
        /// </summary>
        public string MINGC
        {
            get { return mingc; }
            set { mingc = value; }
        }
        private DateTime datatime;
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime DATATIME
        {
            get { return datatime; }
            set { datatime = value; }
        }
        private string datavalue;
        /// <summary>
        /// 值
        /// </summary>
        public string DATAVALUE
        {
            get { return datavalue; }
            set { datavalue = value; }
        }

        private string qy_dianbmb_fk;
        /// <summary>
        /// 点别名表外键
        /// </summary>
        public string QY_DIANBMB_FK
        {
            get { return qy_dianbmb_fk; }
            set { qy_dianbmb_fk = value; }
        }
    }
}
