using System;
using System.Net;
using System.Net.NetworkInformation;

namespace SamUtil
{
    public class NetUtil
    {
        /// <summary>
        /// 端口转换
        /// 不在1000-65535触发异常
        /// </summary>
        /// <param name="iPort"></param>
        public static int GetPort(string port)
        {
            int iPort;
            if (!int.TryParse(port, out iPort))
            {
                throw new Exception("GetPort:" + port + " is not a number!");
            }

            if (!(iPort >= 1000 && iPort <= 65535))
            {
                throw new Exception("GetPort:" + iPort + "  Port must in 1000~65535!");
            }

            return iPort;
        }
        /// <summary>
        /// 端口合法性检查
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool CheckPort(string port)
        {
            try
            {
                GetPort(port);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 转换ip
        /// </summary>
        /// <param name="strIp"></param>
        public static IPAddress GetIPAddress(string strIp)
        {
            IPAddress ip;
            if (!IPAddress.TryParse(strIp, out ip))
            {
                throw new Exception("ip:" + strIp + "  Check Ip format!");
            }

            return ip;
        }
        /// <summary>
        /// ip检查
        /// </summary>
        /// <param name="strIp"></param>
        /// <returns></returns>
        public static bool CheckIPAddress(string strIp)
        {
            try
            {
                GetIPAddress(strIp);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 检查端口被占用
        /// </summary>
        /// <param name="iPort"></param>
        public static bool CheckPortInUse(int iPort)
        {
            bool inUse = false;
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == iPort)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }
    }
}
