using System.Xml;

namespace SamUtil
{
    class Utiles
    {
        /// <summary>
        /// 是否打印到屏幕
        /// </summary>
        public static bool DEBUG = false;
        /// <summary>
        /// 是否记录到文件
        /// </summary>
        public static bool DEBUG_LOG = false;
        /// <summary>
        /// 静态初始化
        /// </summary>
        static Utiles()
        {
            try
            {
                //加载Bin/ConnCfg.xml
                XmlDocument doc = new XmlDocument();
                doc.Load(System.@AppDomain.CurrentDomain.BaseDirectory + "Bin/ConnCfg.xml");
                XmlNode nodeConn = doc.GetElementsByTagName("CFG").Item(0);

                foreach (XmlNode node in nodeConn.ChildNodes)
                {
                    //读配置
                    switch (node.Name.ToUpper())
                    {
                        case "DEBUG":
                            DEBUG = ("1" == node.InnerText);
                            break;
                        case "DEBUG_LOG":
                            DEBUG_LOG = ("1" == node.InnerText);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.AddLog("SamUtiles\t" + Log.Ex2String(ex), "DEBUG");
            }
        }
        /// <summary>
        /// 自定义控制台
        /// </summary>
        /// <param name="strMsg"></param>
        public static void Console(string strMsg)
        {
            if (DEBUG)
            {
                System.Console.WriteLine(strMsg);
            }

            if (DEBUG_LOG)
            {
                Log.AddLog(strMsg, "DEBUG");
            }
        }
    }
}
