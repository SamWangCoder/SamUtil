using System;
using System.IO;

namespace SamUtil
{
    /// <summary>
    /// Pcm音频文件实用工具
    /// </summary>
    public class PcmFileUtil
    {
        /// <summary>
        /// 向一个二进制流中写入wav头部信息，并写入pcm剩余数据
        /// </summary>
        /// <param name="bw">二进制流</param>
        /// <param name="totalAudioLen">wav数据总长</param>
        /// <param name="totalDataLen">pcm数据总长</param>
        /// <param name="longSampleRate">采样率</param>
        /// <param name="channels">通道数量</param>
        /// <param name="sampleBits">每秒数据位数</param>
        /// <returns>返回头部数据</returns>
        private static byte[] WriteWavHeader(BinaryWriter bw, long totalAudioLen, long totalDataLen,
            long longSampleRate, int channels, long sampleBits)
        {
            byte[] header = new byte[44];
            //RIFF WAVE Chunk
            // RIFF标记占据四个字节
            header[0] = (byte)'R';
            header[1] = (byte)'I';
            header[2] = (byte)'F';
            header[3] = (byte)'F';
            //数据大小表示，由于原始数据为long型，通过四次计算得到长度
            header[4] = (byte)(totalDataLen & 0xff);
            header[5] = (byte)((totalDataLen >> 8) & 0xff);
            header[6] = (byte)((totalDataLen >> 16) & 0xff);
            header[7] = (byte)((totalDataLen >> 24) & 0xff);
            //WAVE标记占据四个字节
            header[8] = (byte)'W';
            header[9] = (byte)'A';
            header[10] = (byte)'V';
            header[11] = (byte)'E';
            //FMT Chunk
            // 'fmt '标记符占据四个字节
            header[12] = (byte)'f';
            header[13] = (byte)'m';
            header[14] = (byte)'t';
            header[15] = (byte)' ';//过渡字节
            //数据大小
            header[16] = 16; // 4 bytes: size of 'fmt ' chunk
            header[17] = 0;
            header[18] = 0;
            header[19] = 0;
            //编码方式 10H为PCM编码格式
            header[20] = 1; // format = 1
            header[21] = 0;
            //通道数
            header[22] = (byte)channels;
            header[23] = 0;
            //采样率，每个通道的播放速度
            header[24] = (byte)(longSampleRate & 0xff);
            header[25] = (byte)((longSampleRate >> 8) & 0xff);
            header[26] = (byte)((longSampleRate >> 16) & 0xff);
            header[27] = (byte)((longSampleRate >> 24) & 0xff);
            //音频数据传送速率,采样率*通道数*采样深度/8
            header[28] = (byte)((longSampleRate * channels * sampleBits / 8) & 0xff);
            header[29] = (byte)(((longSampleRate * channels * sampleBits / 8) >> 8) & 0xff);
            header[30] = (byte)(((longSampleRate * channels * sampleBits / 8) >> 16) & 0xff);
            header[31] = (byte)(((longSampleRate * channels * sampleBits / 8) >> 24) & 0xff);
            // 确定系统一次要处理多少个这样字节的数据，确定缓冲区，通道数*采样位数
            header[32] = (byte)(channels * sampleBits / 8);
            header[33] = 0;
            //每个样本的数据位数
            header[34] = 16;
            header[35] = 0;
            //Data chunk
            header[36] = (byte)'d';//data标记符
            header[37] = (byte)'a';
            header[38] = (byte)'t';
            header[39] = (byte)'a';
            //数据长度
            header[40] = (byte)(totalAudioLen & 0xff);
            header[41] = (byte)((totalAudioLen >> 8) & 0xff);
            header[42] = (byte)((totalAudioLen >> 16) & 0xff);
            header[43] = (byte)((totalAudioLen >> 24) & 0xff);

            bw.Write(header, 0, 44);
            return header;
        }

        /// <summary>
        /// 将指定地址的pcm文件转为流
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="longSampleRate">采样率</param>
        /// <param name="channels">通道数</param>
        /// <param name="sampleBits">采样位数</param>
        /// <returns></returns>
        public static Stream Pcm2Wav(string strPath, long longSampleRate, int channels, long sampleBits)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(strPath));
            return Pcm2Wav(br, longSampleRate, channels, sampleBits);
        }

        /// <summary>
        /// 将pcm的byte流转为wav流
        /// </summary>
        /// <param name="bsData"></param>
        /// <param name="longSampleRate">采样率</param>
        /// <param name="channels">通道数</param>
        /// <param name="sampleBits">采样位数</param>
        /// <returns></returns>
        public static Stream Pcm2Wav(byte[] bsData, long longSampleRate, int channels, long sampleBits)
        {
            BinaryReader br = new BinaryReader(IoUtil.BytesToStream(bsData));
            return Pcm2Wav(br, longSampleRate, channels, sampleBits);
        }

        /// <summary>
        /// 读取BinaryReader中pcm数据，转换为流
        /// </summary>
        /// <param name="br">流</param>
        /// <param name="longSampleRate">采样率</param>
        /// <param name="channels">通道数</param>
        /// <param name="sampleBits">采样位数</param>
        /// <returns></returns>
        private static Stream Pcm2Wav(BinaryReader br, long longSampleRate, int channels, long sampleBits)
        {
            BinaryWriter bw = new BinaryWriter(new MemoryStream());

            try
            {
                long totalAudioLen = br.BaseStream.Length;
                long totalDataLen = totalAudioLen + 36;
                //写wav头部
                WriteWavHeader(bw, totalAudioLen, totalDataLen, longSampleRate, channels, sampleBits);

                try
                {
                    while (true)
                    {
                        bw.Write(br.ReadByte());
                    }
                }
                catch (EndOfStreamException) { }
            }
            catch (IOException) { }
            finally
            {
                br.Close();
            }
            bw.Seek(0, SeekOrigin.Begin);
            return bw.BaseStream;
        }
    }
}
