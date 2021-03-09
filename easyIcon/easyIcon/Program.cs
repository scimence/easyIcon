using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Text;
using SciUpdate;

namespace easyIcon
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //DllTool.LoadResourceDll();        // 载入依赖dll
            //DepensFile.load();
            _Main();
        }

        static void _Main()
        {
            SciUpdate.RunningExceptionTool.Run(call);   // 调用异常信息捕获类，进行异常信息的捕获
        }

        // 应用程序，入口逻辑
        public static void call(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new easyIconFun.mainForm());
            Form main = Sci.easyIconFunc.mainForm();
            if ( main != null) Application.Run(main);
            
        }
    }
}
