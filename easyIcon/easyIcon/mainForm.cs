using easyIcon.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easyIcon
{
    public partial class mainForm : Form
    {
        //------------------------------------------------------------------
        public static ToolsFunction F = new ToolsFunction();    //图像处理相关函数工具
        FormTool T = new FormTool();                            //界面特定的功能能函数
        string[] picFiles;          //各载入图像的文件名
        string[] picsName;          //各载入图像名称
        int style = 0;              //设置图像的蒙版样式
        //------------------------------------------------------------------

        public mainForm()
        {
            InitializeComponent();
        }

        // 界面初始化设置
        private void mainForm_Load(object sender, EventArgs e)
        {
            RegistrySet();

            
        }

        // 打开软件的存储目录
        private void MenuItem_Dir_Click(object sender, EventArgs e)
        {
            string Dir = F.getCurDir(getStyle(style)); // 获取文件导出路径
            System.Diagnostics.Process.Start("explorer.exe", "/e,/select, " + Dir);
        }

        // 预览模式
        private void MenuItem_preview_Click(object sender, EventArgs e)
        {
            bool check = MenuItem_preview.Checked;
            pictureBoxMain.BackgroundImage = check ? null : Properties.Resources.TransBg;
            pictureBoxMain.BorderStyle = check ? BorderStyle.None : BorderStyle.FixedSingle;
        }

        //仅允许输入数值,设置textBox的KeyPress事件为当前函数
        private void onlyNumberInput(object sender, KeyPressEventArgs e)
        {
            int key = Convert.ToInt32(e.KeyChar);
            e.Handled = (!(48 <= key && key <= 58 || key == 8)); //数字、 Backspace
        }

        // 高度随宽度变动
        private void textBox_width_TextChanged(object sender, EventArgs e)
        {
            textBox_height.Text = textBox_width.Text;
        }

        #region 最小化和关闭按钮

        // 最小化界面
        private void mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // 关闭主界面
        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region 控制无边框窗体的移动
        //using System.Runtime.InteropServices;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            //常量
            int WM_SYSCOMMAND = 0x0112;

            //窗体移动
            int SC_MOVE = 0xF010;
            int HTCAPTION = 0x0002;

            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        ////常量
        //int WM_SYSCOMMAND = 0x0112;
        ////改变窗体大小
        //int WMSZ_LEFT = 0xF001;
        //int WMSZ_RIGHT = 0xF002;
        //int WMSZ_TOP = 0xF003;
        //int WMSZ_TOPLEFT = 0xF004;
        //int WMSZ_TOPRIGHT = 0xF005;
        //int WMSZ_BOTTOM = 0xF006;
        //int WMSZ_BOTTOMLEFT = 0xF007;
        //int WMSZ_BOTTOMRIGHT = 0xF008;
        //ReleaseCapture();
        //SendMessage(this.Handle, WM_SYSCOMMAND, WMSZ_BOTTOM, 0);

        //SendMessage(this.Handle, WM_SYSCOMMAND, WMSZ_TOP, 0);
        #endregion

        #region 图像文件的载入

        //拖入文件
        private void mainForm_DragEnter(object sender, DragEventArgs e)
        {
            F.dragEnter(e);
        }

        //放下文件
        private void mainForm_DragDrop(object sender, DragEventArgs e)
        {
            string filesNames = F.dragDrop(e);  //拖入窗体的文件放下

            picFiles = filesNames.Split(';');   //载入的文件路径
            picFiles = F.getPicFiles(picFiles); //获取所有图像文件

            picsName = F.getNames(picFiles);    //获取载入的文件名称，不含路径
            T.addIteams(ListBox_Name, picsName);//添加载入的图像名到列表中

            // 默认显示载入的第一个有效图像文件
            if (ListBox_Name.Items.Count > 0)
            {
                ListBox_Name.SelectedIndex = 0;
                ListBox_Name.SetItemChecked(0, true);
            }
            else ListBox_Name_SelectedIndexChanged(null, null);

            if (picFiles == null) StatusTipInfo.Text = "请拖入若干个图像文件，或包含图像的文件夹！";
        }

        #endregion

        #region 自定义样式文件的载入

        //拖入文件
        private void PanelLeft_DragEnter(object sender, DragEventArgs e)
        {
            F.dragEnter(e);
        }

        //放下文件
        private void PanelLeft_DragDrop(object sender, DragEventArgs e)
        {
            string filesNames = F.dragDrop(e);          //拖入窗体的文件放下

            string[] picFiles = filesNames.Split(';');  //载入的文件路径
            picFiles = F.getPicFiles(picFiles);         //获取所有图像文件

            if (picFiles != null)                       //设置自定义蒙版样式
            {
                Button[] btns = { style_self, style_9, style_8, style_7, style_6, style_5, style_4, style_3, style_2, style_1 };
                int count = Math.Min(picFiles.Length, btns.Length);
                for (int i = 0; i < count; i++)
                {
                    Image pic = Bitmap.FromFile(picFiles[i]);
                    btns[i].BackgroundImage = pic;
                    if (btns[i].Image != null) showSelect();
                }
            }
        }

        #endregion

        #region 显示选中的图像

        // 在主界面中显示选中的图像
        private void showSelect()
        {   try
            {
                int index = ListBox_Name.SelectedIndex;
                Image pic = index == -1 ? null : Bitmap.FromFile(picFiles[index]);
                pic = getStyle(pic, style);     // 获取对应式样的图像
                pictureBoxMain.Image = pic;
            }
            catch (Exception) 
            { 
                pictureBoxMain.Image = null;
            }
        }

        // 获取pic对应蒙版式样的图像
        private Image getStyle(Image pic, int style)
        {
            if (pic == null) return null;
            else if (style == 0) return pic;

            Image[] mask = { style_0.BackgroundImage, style_1.BackgroundImage, style_2.BackgroundImage, style_3.BackgroundImage, 
                               style_4.BackgroundImage, style_5.BackgroundImage, style_6.BackgroundImage, style_7.BackgroundImage,
                               style_8.BackgroundImage, style_9.BackgroundImage, style_self.BackgroundImage };

            if (style < 0 || style > mask.Length) return pic;
            Image tmp = F.setPngMask(pic, mask[style]);

            return tmp;
        }

        #endregion

        #region 选择图像与蒙版样式

        // 显示选中的图像
        private void ListBox_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 居中显示选中的图像
            showSelect();
            T.CenterOnParent(pictureBoxMain, panelMain.Size);

            // 显示尺寸
            Image img = pictureBoxMain.Image;
            Size S = (img == null ? Size.Empty : img.Size);
            StatusSize.Text = S.Width + ", " + S.Height;
        }

        // 设置蒙版样式
        private void style_Click(object sender, EventArgs e)
        {
            int tmp = ((Button)sender).TabIndex;
            if (style != tmp)   // 设置新的样式
            {
                style = tmp;
                Button[] btns = { style_0, style_1, style_2, style_3, style_4, 
                                style_5, style_6, style_7, style_8, style_9, style_self };
                for (int i = 0; i < btns.Length; i++)
                    btns[i].Image = (i == style ? Resources._512_e : null);

                showSelect();
            }
        }

        #endregion

        #region 尺寸列表处理逻辑

        // 常用Icon尺寸
        private void MenuItem0_Click(object sender, EventArgs e)
        {
            string[] sizeData = { "48x48", "64x64", "96x96", "128x128", "160x160", "192x192", "224x224", "255x255" };
            ListBox_Size.Items.Clear();
            foreach (string size in sizeData) ListBox_Size.Items.Add(size);
        }

        // 向List列表中添加新的尺寸信息
        private void addSize(CheckedListBox checkList, int width, int height)
        {
            if (width > 0 && height > 0)
            {
                string size = width + "x" + height;
                if (!checkList.Items.Contains(size)) checkList.Items.Add(size);
            }
        }

        // 添加新的尺寸,并选中
        private void button_newSize_Click(object sender, EventArgs e)
        {
            try
            {
                int width = Int32.Parse(textBox_width.Text);
                int height = Int32.Parse(textBox_height.Text);

                addSize(ListBox_Size, width, height);
                ListBox_Size.SetItemChecked(ListBox_Size.Items.Count - 1, true);
            }
            catch (Exception) { }
        }

        // 添加当前尺寸
        private void MenuItem1_Click(object sender, EventArgs e)
        {
            Image img = pictureBoxMain.Image;
            Size S = (img == null ? Size.Empty : img.Size);
            addSize(ListBox_Size, S.Width, S.Height);
            ListBox_Size.SetItemChecked(ListBox_Size.Items.Count - 1, true);
            if(S != Size.Empty) StatusTipInfo.Text = "已添加当前图像尺寸，到尺寸列表";
        }

        // 删除选中尺寸
        private void MenuItem2_Click(object sender, EventArgs e)
        {
            object iteam = ListBox_Size.SelectedItem;
            if (iteam != null) ListBox_Size.Items.Remove(iteam);
        }

        // 删除所有尺寸
        private void MenuItem3_Click(object sender, EventArgs e)
        {
            ListBox_Size.Items.Clear();
        }

        // 记忆所有尺寸
        private void MenuItem4_Click(object sender, EventArgs e)
        {
            if (MenuItem4.Checked) F.RegistrySave(@"Scimence\easyIcon\Set", "导出尺寸", toString(ListBox_Size));
            else F.RegistryRemove(@"Scimence\easyIcon\Set");
        }

        //此函数用于从注册表中获取初始化时的尺寸设置信息
        private void RegistrySet()
        {
            ListBox_Size.Items.Clear();

            String value = F.RegistryStrValue(@"Scimence\easyIcon\Set", "导出尺寸");
            if (!value.Equals(""))
            {
                string[] sizeData = value.Split(',');
                foreach (string size in sizeData) ListBox_Size.Items.Add(size);
            }
        }

        // 获取List所有尺寸信息
        private string toString(CheckedListBox list)
        {
            string str = "";
            for (int i = 0; i < list.Items.Count; i++)
                str += (str.Equals("") ? "" : ",") + list.Items[i].ToString();
            return str;
        }

        #endregion

        #region 列表全选逻辑

        // 全选载入图像
        private void checkBoxAllFile_CheckedChanged(object sender, EventArgs e)
        {
            setCheck(ListBox_Name, checkBoxAllFile.Checked);
        }

        // 全选导出尺寸
        private void checkBoxAllSize_CheckedChanged(object sender, EventArgs e)
        {
            setCheck(ListBox_Size, checkBoxAllSize.Checked);
        }

        // 设置ListBox的选中状态为checkState
        private void setCheck(CheckedListBox list, bool checkState)
        {
            int count = list.Items.Count;
            for (int i = 0; i < count; i++)
                list.SetItemChecked(i, checkState);
        }

        #endregion

        #region 图像批量导出逻辑

        // 导出png
        private void MenuItem_savePng_Click(object sender, EventArgs e)
        {
            savePic("png");
        }

        // 导出icon
        private void MenuItem_saveIcon_Click(object sender, EventArgs e)
        {
            savePic("ico");
        }

        // 导出jpg
        private void MenuItem_saveJpg_Click(object sender, EventArgs e)
        {
            savePic("jpg");
        }

        // 保存图像逻辑， Ext = "png", "ico", "jpg"
        private void savePic(string Ext)
        {
            string[] files = getSelectFiles();      // 获取所有文件名
            Size[] sizes = getSelectSizes();        // 获取所有尺寸信息

            savePic(files, sizes, Ext);             // 保存所有图像信息
        }

        // 保存图像逻辑， Ext = "png", "ico", "jpg"
        private void savePic(string[] files, Size[] sizes, string Ext)
        {
            if (files == null) StatusTipInfo.Text = "请先设置和选中，要操作的图像"; // 提示信息
            else if (sizes == null) StatusTipInfo.Text = "请先设置和选中，要导出的尺寸";

            if (files == null) return;

            string Dir = F.getCurDir(getStyle(style) + "\\" + Ext);                 // 获取文件导出路径
            foreach (string file in files)
            {
                Image pic = Bitmap.FromFile(file);  // 获取图像
                pic = getStyle(pic, style);         // 设置蒙版式样
                string name = System.IO.Path.GetFileNameWithoutExtension(file);     // 获取文件名，不含拓展名

                if (sizes == null) return;

                foreach (Size size in sizes)
                {
                    // 生成对应尺寸的图像
                    Image tmp = F.shrinkTo(pic, size, MenuItem_Center.Checked);     // 尺寸缩放
                    if (Ext.Equals("jpg")) tmp = F.setBackColor(tmp, Color.White);  // jpg图像设置白色为背景色

                    // 获取尺寸对应的logo图像
                    if (!Register.IsRegUser())           // 若未注册，则添加logo
                    {
                        int width = size.Width / 3;
                        if (width < 55) width = (size.Width < 55 ? size.Width : 55);
                        int height = width * 64 / 200;
                        Image logo = F.shrinkTo(Resources.logo, new Size(width, height), false);      // 尺寸缩放
                        tmp = F.SetLogo(tmp, logo);
                    }

                    // 保存图像到路径
                    string PathName = Dir + name + "_" + size.Width + "x" + size.Height + "." + Ext;
                    if (Ext.Equals("ico")) IconTool.SaveToIcon(tmp, size, PathName);
                    else F.SaveToFile(tmp, PathName, true, null);
                }
            }

            string info = "成功导出" + (Ext.Equals("ico") ? "icon" : Ext) + "图像";
            F.MessageWithOpen(info, Dir);   // 提示信息
        }

        // 获取选中的所有图像文件
        private string[] getSelectFiles()
        {
            CheckedListBox list = ListBox_Name;
            if (list.CheckedItems.Count == 0) return null;

            List<string> files = new List<string>();
            for (int i = 0; i < list.Items.Count; i++)
                if (list.GetItemChecked(i)) files.Add(picFiles[i]);

            if (files.Count == 0) return null;
            else return files.ToArray();
        }

        // 获取选中的所有导出尺寸
        private Size[] getSelectSizes()
        {
            CheckedListBox list = ListBox_Size;
            if (list.CheckedItems.Count == 0) return null;

            List<Size> sizes = new List<Size>();
            for (int i = 0; i < list.Items.Count; i++)
            {
                if (list.GetItemChecked(i))
                {
                    string[] str = list.Items[i].ToString().Split('x');
                    Size size = new Size(Int32.Parse(str[0]), Int32.Parse(str[1]));
                    sizes.Add(size);
                }
            }

            if (sizes.Count == 0) return null;
            else return sizes.ToArray();
        }

        // 获取pic对应蒙版式样的图像
        private string getStyle(int style)
        {
            string[] names = { "原样式", "样式2", "样式2_5", "样式3", "样式3_5", 
                               "样式4", "样式5", "样式6", "样式7", "样式8", "自定义" };

            if (style < 0 || style > names.Length) return names[0];
            else return names[style];
        }

        #endregion

        #region 当前图像处理逻辑

        private void MenuItem_saveCurPng_Click(object sender, EventArgs e)
        {
            saveCurPic("png");
        }

        private void MenuItem_saveCurIcon_Click(object sender, EventArgs e)
        {
            saveCurPic("ico");
        }

        private void MenuItem_saveCurJpg_Click(object sender, EventArgs e)
        {
            saveCurPic("jpg");
        }

        // 保存图像逻辑， Ext = "png", "ico", "jpg"
        private void saveCurPic(string Ext)
        {
            int index = ListBox_Name.SelectedIndex;
            if (index != -1)
            {
                string[] files = { picFiles[index] };   // 当前选中的图像文件
                Size[] sizes = getSelectSizes();        // 获取所有尺寸信息
                if (sizes == null)
                {
                    Size S = pictureBoxMain.Image.Size;
                    sizes = new Size[] { S };           // 默认原尺寸
                    if (Ext.Equals("ico") && (S.Width > 255 || S.Height > 255))
                    {
                        StatusTipInfo.Text = "提示：Icon导出的最大尺寸为255x255";
                        return;
                    }
                }
                savePic(files, sizes, Ext);             // 保存所有图像信息
            }
        }

        #endregion

        private void 软件注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new Register().ShowDialog();

            //string CPUSerial = MachineInfo.GetCPUSerialNumber();            // "BFEB-FBFF-0003-06A9"
            //string HardDiskSerial = MachineInfo.GetHardDiskSerialNumber();  // "TEA55A3Q2P7WKR"
            //string BIOSSerial = MachineInfo.GetBIOSSerialNumber();          // "FMFTFW1"
            //string LocalMac = MachineInfo.GetLocalMac();                    // "60:36:DD:5A:EF:51"

            //string machineSerial = MachineInfo.MachineSerial(); // "XRUM-LYKS-4R2P-QP6H"    机器码
            //string Serial = MachineInfo.RegSerial();            // "6JSO-F2CM-4LQJ-JN8P"    注册码

            if (!Register.IsRegUser()) new Register().ShowDialog();  // 未注册，则打开注册界面
            else
            {
                MessageBox.Show("您已是注册用户 正版授权\nCopyright ©  2015 Scimence");
                //F.RegistryRemove(@"Scimence\easyIcon\Set", "Serial");
                //MessageBox.Show("您已完成注册，可以正常使用了！");
            }
        }

        // 打开软件的使用说明页面
        private void MenuItem_wiki(object sender, EventArgs e)
        {
            F.RegistryRemove(@"Scimence\easyIcon\Set", "Serial");
        }

        // 定时器，用于首次打开界面时控制信息的延时载入
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Register.needRegister())   // 网络控制开关，控制注册表项不显示
                MenuItem_pollCode.Visible = false;
            timer1.Stop();
        }

        // 使用说明界面
        private void MenuItem_wiki_Click(object sender, EventArgs e)
        {
            //F.RegistryRemove(@"Scimence\easyIcon\Set", "Serial");
            string url = "https://git.oschina.net/scimence/easyIcon/wikis/home";
            Process.Start(url);
        }

        // 软件下载界面
        private void FormIcon_Click(object sender, EventArgs e)
        {
            string url = "https://git.oschina.net/scimence/easyIcon/attach_files";
            Process.Start(url);
        }

        // 点击主界面时，跳转至推荐页面
        private void pictureBoxMain_Click(object sender, EventArgs e)
        {
            //string tmp = "http://store.cocos.com/stuff/show/178765.html";
            //tmp = Register.formatUrl(tmp);

            string url = Register.RecommendUrl();
            if (!url.Equals("")) Process.Start(url);
        }

    }
}
