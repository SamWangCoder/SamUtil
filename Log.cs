using System;

namespace SamUtil
{
    public class Log
    {
        /// <summary>
        /// 最大的log文件大小(MB)
        /// </summary>
        public static readonly int MAX_LOG_SIZE_BY_MB = 5;
        /// <summary>
        /// 新行
        /// </summary>
        public static readonly string NEW_L = System.Environment.NewLine;
        /// <summary>
        /// 工作目录
        /// </summary>
        /// <returns></returns>
        public static readonly string LOG_WORK_PATH = @AppDomain.CurrentDomain.BaseDirectory + "Log/";
        /// <summary>
        /// 异常转string
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string Ex2String(Exception ex)
        {
            return ex.Message + NEW_L + ex.StackTrace;
        }
        /// <summary>
        /// 向指定文件写入log，log文件命名规则为前缀_Log_yyMMdd.txt
        /// </summary>
        /// <param name="now"></param>
        /// <param name="strContent"></param>
        /// <param name="strPrefix"></param>
        private static void AddContent(DateTime now, string strContent, string strPrefix)
        {
            try
            {
                if (!System.IO.Directory.Exists(LOG_WORK_PATH))
                {
                    System.IO.Directory.CreateDirectory(LOG_WORK_PATH);
                }
                string strFile = LOG_WORK_PATH + strPrefix + "Log_" + now.ToString("yyyyMMdd") + ".txt";
                strContent = "****" + now + "****" + NEW_L + strContent + NEW_L;

                if (System.IO.File.Exists(strFile))
                {
                    //大于5Mb则覆盖原有log文件
                    if (MAX_LOG_SIZE_BY_MB < (new System.IO.FileInfo(strFile)).Length / 1024 / 1024)
                    {
                        System.IO.File.WriteAllText(strFile, strContent);
                        return;
                    }
                }
                System.IO.File.AppendAllText(strFile, strContent);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 生成websocket或redis或生成sql语句log
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strPrefix"></param>
        public static void AddLog(string strContent, string strPrefix)
        {
            strPrefix = strPrefix.ToUpper() + "_";
            AddContent(DateTime.Now, strContent, strPrefix);
        }
    }
}
