using System;
using System.Xml;

namespace SamUtil
{
    public class YshUtil
    {
        /// <summary>
        /// 获得Bin中指定文件的指定节点，仅获取符合条件的第一个节点
        /// </summary>
        /// <param name="strNode"></param>
        /// <returns></returns>
        public static XmlNode GetConfigNode(string strCfgFileName, string strNode)
        {
            try
            {
                XmlDocument Xd = new XmlDocument();
                string strDir = AppDomain.CurrentDomain.BaseDirectory;
                string strPath = string.Empty;

                if (strDir.ToUpper().EndsWith(System.IO.Path.DirectorySeparatorChar + "BIN"))
                {
                    strPath = strDir + System.IO.Path.DirectorySeparatorChar + strCfgFileName;
                }
                else
                {
                    if (System.IO.Directory.Exists(strDir + "Bin"))
                    {
                        strPath = strDir + System.IO.Path.DirectorySeparatorChar +
                            "Bin" + System.IO.Path.DirectorySeparatorChar + strCfgFileName;
                    }
                    else if (System.IO.Directory.Exists(strDir + "bin"))
                    {
                        strPath = strDir + System.IO.Path.DirectorySeparatorChar +
                            "bin" + System.IO.Path.DirectorySeparatorChar + strCfgFileName;
                    }
                }

                Xd.Load(strPath);
                return Xd.SelectSingleNode("//" + strNode.ToUpper());
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获得RdsWsConnCfg.xml中ws的配置
        /// 标准格式："ws://192.168.5.155:4000"
        /// </summary>
        /// <returns></returns>
        public static string GetWebsocketUrl()
        {
            string strWsUrl = "ws://{0}:{1}";
            try
            {
                XmlNode wsNode = GetConfigNode("RdsWsConnCfg.xml", "WEBSOCKET");
                string strIp = "";
                string strPort = "";
                foreach (XmlNode node in wsNode.ChildNodes)
                {
                    //sbCols.Append("'" + colNode.InnerText + "',");
                    switch (node.Name.ToUpper())
                    {
                        case "IPCFG":
                            strIp = node.InnerText;
                            break;
                        case "PORT":
                            strPort = node.InnerText;
                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrEmpty(strPort))
                        return string.Format(strWsUrl, string.IsNullOrEmpty(strIp) ? "127.0.0.1" : strIp, strPort);
                }
            }
            catch
            {
            }
            return "";
        }
    }
}
