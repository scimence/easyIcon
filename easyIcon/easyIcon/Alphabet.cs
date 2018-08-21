using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication11
{
    /// <summary>
    /// 将任意字符串转化为全字母串
    /// </summary>
    public class Alphabet
    {
        public static void example()
        {
            String data = "test encode";
            string encode = Encode(data);
            string decode = Decode(encode);
            bool b = data.Equals(decode);
            bool b2 = b;
        }

        /// <summary>  
        /// 转码data为全字母串，并添加前缀  
        /// </summary>  
        public static string Encode(string data)
        {
            string str = data;
            if (!data.StartsWith("ALPHABETCODE@"))
            {
                str = "ALPHABETCODE@" + EncodeAlphabet(data);
            }
            return str;
        }


        /// <summary>  
        /// 解析字母串为原有串  
        /// </summary>  
        public static string Decode(string data)
        {
            string str = data;
            if (data.StartsWith("ALPHABETCODE@"))
            {
                str = DecodeAlphabet(data.Substring("ALPHABETCODE@".Length));
            }
            return str;
        }

        /// <summary>
        /// 获取文件对应的编码字符串
        /// </summary>
        public static string getFileData(string file)
        {
            byte[] bytes = File2Bytes(file);
            string data = ToStr(bytes);

            return data;
        }

        /// <summary>  
        /// 将文件转换为byte数组  
        /// </summary>  
        /// <param name="path">文件地址</param>  
        /// <returns>转换后的byte数组</returns>  
        public static byte[] File2Bytes(string path)
        {
            if (!File.Exists(path))
            {
                return new byte[0];
            }

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];

            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            return buff;
        }

        /// <summary>  
        /// 获取文件中的数据,自动判定编码格式
        /// </summary>  
        private static string fileToString(String filePath)
        {
            byte[] bytes = File2Bytes(filePath);
            string str = Encoding.UTF8.GetString(bytes);

            return str;
        }

        # region 字符串字母编码逻辑

        /// <summary>  
        /// 转化为字母字符串  
        /// </summary>  
        public static string EncodeAlphabet(string data)
        {
            byte[] B = Encoding.UTF8.GetBytes(data);
            return ToStr(B);
        }

        /// <summary>  
        /// 每个字节转化为两个字母  
        /// </summary>  
        private static string ToStr(byte[] B)
        {
            StringBuilder Str = new StringBuilder();
            foreach (byte b in B)
            {
                Str.Append(ToStr(b));
            }
            return Str.ToString();
        }

        private static string ToStr(byte b)
        {
            return "" + ToChar(b / 16) + ToChar(b % 16);
        }

        private static char ToChar(int n)
        {
            return (char)('a' + n);
        }

        /// <summary>  
        /// 解析字母字符串  
        /// </summary>  
        public static string DecodeAlphabet(string data)
        {
            byte[] B = ToBytes(data);
            return Encoding.UTF8.GetString(B);
        }

        /// <summary>  
        /// 解析字符串为Bytes数组
        /// </summary>  
        public static byte[] ToBytes(string data)
        {
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

        # endregion

    }

}