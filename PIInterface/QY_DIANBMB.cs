using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIInterface
{
    public class QY_DIANBMB
    {
        private string qy_dianbmboid;
        /// <summary>
        /// ID
        /// </summary>
        public string QY_DIANBMBOID
        {
            get { return qy_dianbmboid; }
            set { qy_dianbmboid = value; }
        }

        private string biem;
        /// <summary>
        /// 别名
        /// </summary>
        public string BIEM
        {
            get { return biem; }
            set { biem = value; }
        }

        private string beiz;
        /// <summary>
        /// 备注
        /// </summary>
        public string BEIZ
        {
            get { return beiz; }
            set { beiz = value; }
        }

        private string mingc;
        /// <summary>
        ///  名称
        /// </summary>
        public string MINGC
        {
            get { return mingc; }
            set { mingc = value; }
        }

        private string datatype;
        /// <summary>
        /// 数据类型
        /// S:String类型
        /// B:Bool类型
        /// </summary>
        public string DATATYPE
        {
            get { return datatype; }
            set { datatype = value; }
        }

        private string datasource;
        /// <summary>
        /// 数据来源
        /// O:OPC
        /// P:PI
        /// </summary>
        public string DATASOURCE
        {
            get { return datasource; }
            set { datasource = value; }
        }
    }
}
