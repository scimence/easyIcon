using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace easyIcon
{

    //在项目-》添加引用....里面引用System.Management  
    class MachineInfo
    {

        # region 获取计算机硬件信息

        /// <summary>
        /// 获取CPU序列号  
        /// </summary>
        public static string GetCPUSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Processor");
                string sCPUSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sCPUSerialNumber = mo["ProcessorId"].ToString().Trim();
                }
                return sCPUSerialNumber;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取硬盘序列号  
        /// </summary>
        public static string GetHardDiskSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                string sHardDiskSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sHardDiskSerialNumber = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return sHardDiskSerialNumber;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>  
        /// 获取主板序列号  
        /// </summary>
        public static string GetBIOSSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_BIOS");
                string sBIOSSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sBIOSSerialNumber = mo["SerialNumber"].ToString().Trim();
                }
                return sBIOSSerialNumber;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>  
        /// 主板编号  
        /// </summary>  
        public static string GetBoardID()
        {
            string st = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_BaseBoard");
            foreach (ManagementObject mo in mos.Get())
            {
                st = mo["SerialNumber"].ToString();
            }
            return st;
        }  
        /// <summary>  
        /// 获取本机的MAC;  
        /// </summary>  
        public static string GetLocalMac()
        {
            string mac = null;
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True")
                    mac = mo["MacAddress"].ToString();
            }
            return (mac);
        }

        # endregion

        //===============================================

        #region 自定义机器码、注册码生成算法

        private static string machineSerial;

        /// <summary>
        /// 根据硬件信息生成机器码
        /// </summary>
        public static string MachineSerial()
        {
            if (machineSerial != null) return machineSerial;

            string CPUSerial = MachineInfo.GetCPUSerialNumber();            // "BFEBFBFF000306A9"
            string HardDiskSerial = MachineInfo.GetHardDiskSerialNumber();  // "TEA55A3Q2P7WKR"
            string BoardID = MachineInfo.GetBoardID().Replace(".", "");     // "FMFTFW1CN1296631B059E"

            string serial = getTails(HardDiskSerial, 5) + getTails(CPUSerial, 5) + getTails(BoardID, 6); //"P7WKR306A91B059E"
            serial = lockLogic(serial, "SCIM-ENCE-EASY-ICON");// 软件附加码 // "XRUM-LYKS-4R2P-QP6H"

            machineSerial = serial;
            return serial;
        }

        /// <summary>
        /// 根据机器码生成注册码
        /// </summary>
        public static string RegSerial()
        {
            return lockLogic(MachineSerial(), "SCIM-ENCE-REGE-DITS");
        }

        /// <summary>
        /// 获取Data尾部len长度的串
        /// </summary>
        private static string getTails(string Data, int len)
        {
            return Data.Substring(Data.Length - len, len);
        }

        /// <summary>
        /// 对字符串Data用字符串Key进行加密
        /// </summary>
        private static string lockLogic(string Data, string Key)
        {
            Data = Data.Replace("-", "").ToUpper();
            Key = Key.Replace("-", "").ToUpper();

            char[] data = Data.ToCharArray();
            char[] key = Key.ToCharArray();

            string tmp = "";
            for (int i = 0; i < data.Length; i++)
            {
                tmp += AlpahLock(data[i], key[i % key.Length]);
            }
            return format(tmp);
        }

        /// <summary>
        /// 对字符C用K加密，转化为A-Z、0-9对应的字符
        /// </summary>
        private static char AlpahLock(char C, char K)
        {
            int n = (C + K) % 35;
            if (n < 26) return (char)(n + 'A'); // 0-25映射到 A-Z
            else return (char)(n - 26 + '1');   // 26-34映射到 1-9
        }

        /// <summary>
        /// 规整化字符串"BFEBFBFF000306A9"为"BFEB-FBFF-0003-06A9"
        /// </summary>
        public static string format(string Data)
        {
            Data = Data.Replace("-", "").ToUpper();
            char[] data = Data.ToCharArray();

            string tmp = "";
            for (int i = 0; i < data.Length; i++)
            {
                tmp += data[i];
                tmp += (((i + 1) % 4 == 0 && i < data.Length - 1) ? "-" : "");
            }
            return tmp;
        }

        #endregion

    }
}
