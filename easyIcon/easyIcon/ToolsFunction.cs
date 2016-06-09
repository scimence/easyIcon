﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easyIcon
{
    public class ToolsFunction
    {
        public static string ToolsName = "easyIcon";   //工具名称

        #region 网络数据的读取

        //从给定的网址中获取数据
        public string getWebData(string url)
        {
            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                string data = client.DownloadString(url);
                return data;
            }
            catch (Exception) { return ""; }
        }

        #endregion

        //=======================================================
        //保存数据到文件
        //=======================================================

        /// <summary>
        /// 保存数据data到文件，返回值为保存的文件名
        /// </summary>
        public string SaveToFile(String data, String name, bool mute)
        {
            return SaveToFile(data, "", name, mute);
        }

        /// <summary>
        /// 保存数据data到文件，返回值为保存的文件名,指定子目录名
        /// </summary>
        public string SaveToFile(String data, String subDir, String name, bool mute)
        {
            String filePath = "";

            //若未命名，则使用系统时间自动命名
            if (name == null || name.Trim().Equals("（重命名）") || name.Trim().Equals(""))
            {
                name = DateTime.Now.ToLongTimeString().Replace(":", ".");   //使用系统时间自动为文件命名
                filePath = SaveToFile(data, subDir, name, false);           //保存数据到文件
                return filePath;                                            //返回保存的文件名
            }

            try
            {
                filePath = SaveProcess(data, subDir, name);                 //保存数据并记录文件完整路径

                if (!mute) MessageWithOpen("成功导出数据!", filePath);
                return filePath;
            }
            catch (Exception)
            {
                MessageBox.Show("保存数据失败");
                return "";
            }
        }

        /// <summary>
        /// 保存数据data到文件处理过程，返回值为保存的文件名
        /// </summary>
        public String SaveProcess(String data, String name)
        {
            return SaveProcess(data, "", name);
        }

        /// <summary>
        /// 保存数据data到文件处理过程，返回值为保存的文件名,指定子目录名
        /// </summary>
        public String SaveProcess(String data, String subDir, String name)
        {
            string CurDir = System.AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.Date.ToString("yyyy_MM_dd") + "(" + ToolsName + @")导出\" + subDir;         //设置当前目录
            if (!System.IO.Directory.Exists(CurDir)) System.IO.Directory.CreateDirectory(CurDir);   //该路径不存在时，在当前文件目录下创建文件夹"导出.."

            //不存在该文件时先创建
            String filePath = CurDir + name + ".txt";
            System.IO.StreamWriter file1 = new System.IO.StreamWriter(filePath, false);     //文件已覆盖方式添加内容

            file1.Write(data);                                                              //保存数据到文件

            file1.Close();                                                                  //关闭文件
            file1.Dispose();                                                                //释放对象

            return filePath;
        }

        /// <summary>
        /// 保存数据data到原文件filePathName中
        /// </summary>
        public String SaveToNativeFile(String data, String filePathName, bool mute)
        {
            try
            {
                System.IO.StreamWriter file1 = new System.IO.StreamWriter(filePathName, false); //文件已覆盖方式添加内容
                file1.Write(data);                                                              //保存数据到文件

                file1.Close();                                                                  //关闭文件
                file1.Dispose();                                                                //释放对象

                if (!mute) this.MessageWithOpen("成功导出数据!", filePathName);
                return filePathName;
            }
            catch (Exception)
            {
                return SaveToFile(data, "", mute);          //若保存到原文件失败，则创建新文件进行保存
            }
        }

        //=======================================================
        //其他相关功能
        //=======================================================
        /// <summary>
        /// 文件拖进事件处理：
        /// </summary>
        public void dragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))    //判断拖来的是否是文件
                e.Effect = DragDropEffects.Link;                //是则将拖动源中的数据连接到控件
            else e.Effect = DragDropEffects.None;
        }
        /// <summary>
        /// 文件放下事件处理：
        /// 放下, 另外需设置对应控件的 AllowDrop = true; 
        /// 获取的文件名形如 "d:\1.txt;d:\2.txt"
        /// </summary>
        public string dragDrop(DragEventArgs e)
        {
            StringBuilder filesName = new StringBuilder("");
            Array file = (System.Array)e.Data.GetData(DataFormats.FileDrop);//将拖来的数据转化为数组存储

            foreach (object I in file)
            {
                string str = I.ToString();

                System.IO.FileInfo info = new System.IO.FileInfo(str);
                //若为目录，则获取目录下所有子文件名
                if ((info.Attributes & System.IO.FileAttributes.Directory) != 0)
                {
                    str = getAllFiles(str);
                    if (!str.Equals("")) filesName.Append((filesName.Length == 0 ? "" : ";") + str);
                }
                //若为文件，则获取文件名
                else if (System.IO.File.Exists(str))
                    filesName.Append((filesName.Length == 0 ? "" : ";") + str);
            }

            return filesName.ToString();
        }

        /// <summary>
        /// 判断path是否为目录
        /// </summary>
        public bool IsDirectory(String path)
        {
            System.IO.FileInfo info = new System.IO.FileInfo(path);
            return (info.Attributes & System.IO.FileAttributes.Directory) != 0;
        }

        /// <summary>
        /// 获取目录path下所有子文件名
        /// </summary>
        public string getAllFiles(String path)
        {
            StringBuilder str = new StringBuilder("");
            if (System.IO.Directory.Exists(path))
            {
                //所有子文件名
                string[] files = System.IO.Directory.GetFiles(path);
                foreach (string file in files)
                    str.Append((str.Length == 0 ? "" : ";") + file);

                //所有子目录名
                string[] Dirs = System.IO.Directory.GetDirectories(path);
                foreach (string dir in Dirs)
                {
                    string tmp = getAllFiles(dir);  //子目录下所有子文件名
                    if (!tmp.Equals("")) str.Append((str.Length == 0 ? "" : ";") + tmp);
                }
            }
            return str.ToString();
        }

        //=======================================================
        //图像的保存
        //=======================================================

        /// <summary>
        /// 保存图像pic到默认目录中，保存名称为name
        /// </summary>
        public void SaveToFile(Image pic, String name)
        {
            string CurDir = System.AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.Date.ToString("yyyy_MM_dd") + "(" + ToolsName + @")导出\";         //设置当前目录
            if (!System.IO.Directory.Exists(CurDir)) System.IO.Directory.CreateDirectory(CurDir);   //该路径不存在时，在当前文件目录下创建文件夹"导出.."

            SaveToFile(pic, CurDir + name, true);    //已替换方式保存图像
        }

        //保存图像pic到文件fileName中
        public void SaveToFile(Image pic, string fileName, bool replace)
        {
            SaveToFile(pic, fileName, replace, null);
        }
        //保存图像pic到文件fileName中，指定图像保存格式
        public void SaveToFile(Image pic, string fileName, bool replace, ImageFormat format)    //ImageFormat.Jpeg
        {
            //若图像已存在，则删除
            if (System.IO.File.Exists(fileName) && replace)
                System.IO.File.Delete(fileName);

            //若不存在则创建
            if (!System.IO.File.Exists(fileName))
            {
                if (format == null) format = getFormat(fileName);   //根据拓展名获取图像的对应存储类型

                if (format == ImageFormat.MemoryBmp) pic.Save(fileName);
                else pic.Save(fileName, format);                    //按给定格式保存图像
            }
        }

        //根据文件拓展名，获取对应的存储类型
        public ImageFormat getFormat(string filePath)
        {
            ImageFormat format = ImageFormat.MemoryBmp;
            String Ext = System.IO.Path.GetExtension(filePath).ToLower();

            if (Ext.Equals(".png")) format = ImageFormat.Png;
            else if (Ext.Equals(".jpg") || Ext.Equals(".jpeg")) format = ImageFormat.Jpeg;
            else if (Ext.Equals(".bmp")) format = ImageFormat.Bmp;
            else if (Ext.Equals(".gif")) format = ImageFormat.Gif;
            else if (Ext.Equals(".ico")) format = ImageFormat.Icon;
            else if (Ext.Equals(".emf")) format = ImageFormat.Emf;
            else if (Ext.Equals(".exif")) format = ImageFormat.Exif;
            else if (Ext.Equals(".tiff")) format = ImageFormat.Tiff;
            else if (Ext.Equals(".wmf")) format = ImageFormat.Wmf;
            else if (Ext.Equals(".memorybmp")) format = ImageFormat.MemoryBmp;

            return format;
        }

        //从files中获取所有的图像文件
        public string[] getPicFiles(string[] files)
        {
            string[] exts = { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".ico", ".emf", ".tiff", ".wmf", ".memorybmp" };
            List<string> Extention = new List<string>();
            foreach(string ext in exts)  Extention.Add(ext);
            
            string tmp = "";
            foreach (string file in files)
            {
                String Ext = System.IO.Path.GetExtension(file).ToLower();
                if (Extention.Contains(Ext)) tmp = tmp.Equals("") ? file : (tmp + ";" + file);
            }
            return tmp.Equals("") ? null : tmp.Split(';');
        }

        //获取文件对应的名称包含后缀名
        public string[] getNames(string[] files)
        {
            if (files == null) return null;

            //获取拖入的载入图像信息
            string[] Names = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                Names[i] = System.IO.Path.GetFileName(files[i]);    //获取文件名
            }

            return Names;
        }

        /// <summary>
        /// 保存icon到文件 subDir + name
        /// </summary>
        public string SaveToDirectory(Icon icon, string name, string subDir)
        {
            string CurDir = getCurDir(subDir) + name;

            System.IO.Stream stream = new System.IO.FileStream(CurDir, System.IO.FileMode.Create);
            icon.Save(stream);
            stream.Flush();
            stream.Close();

            return CurDir;
        }

        //保存图像pic到子目录subDir中，保存名称为name
        public String SaveToDirectory(Image pic, string name, string subDir)
        {
            return SaveToDirectory(pic, name, subDir, null);
        }
        //保存图像pic到子目录subDir中，保存名称为name
        public String SaveToDirectory(Image pic, string name, ImageFormat format)
        {
            return SaveToDirectory(pic, name, null, format);
        }
        public String SaveToDirectory(Image pic, string name, string subDir, ImageFormat format)
        {
            string CurDir = System.AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.Date.ToString("yyyy_MM_dd") + "(" + ToolsName + @")导出\" + (subDir != null ? (subDir + "\\") : "");         //设置当前目录
            if (!System.IO.Directory.Exists(CurDir)) System.IO.Directory.CreateDirectory(CurDir);   //该路径不存在时，在当前文件目录下创建文件夹"导出.."

            string fileName = CurDir + name;
            SaveToFile(pic, fileName, true, format);

            return CurDir;
        }

        /// <summary>
        /// 获取工具的默认路径
        /// </summary>
        public String getCurDir(string subDir)
        {
            string Dir = DateTime.Now.Date.ToString("yyyy_MM_dd") + "(" + ToolsName + @")导出\" + ((subDir != null && !subDir.Equals("")) ? (subDir + "\\") : "");
            string CurDir = System.AppDomain.CurrentDomain.BaseDirectory + Dir;                  //设置当前目录
            if (!System.IO.Directory.Exists(CurDir)) System.IO.Directory.CreateDirectory(CurDir);//该路径不存在时，在当前文件目录下创建文件夹"导出.."

            return CurDir;
        }


        //为图像pic添加包含透明度信息的png蒙版图像
        public Bitmap setPngMask(Image Pic, Image Mask)
        {
            return setPicMask(Pic, Mask, true);
        }

        //为图像pic的添加遮罩图像
        public Bitmap setMask(Image Pic, Image Mask)
        {
            return setPicMask(Pic, Mask, false);
        }

        //=======================================================
        //为图像pic的添加遮罩图像
        //=======================================================
        public Bitmap setPicMask(Image Pic, Image Mask, Boolean isPng)
        {
            if (Pic == null) return null;

            Bitmap pic = ToBitmap(Pic);
            if (Mask == null) return pic;

            Bitmap mask = ToBitmap(Mask);
            if (pic.Width != mask.Width || pic.Height != mask.Height)
                mask = shrinkTo(mask, new Rectangle(0, 0, Pic.Width, Pic.Height));

            Color C, C2;
            for (int i = 0; i < pic.Height; i++)
            {
                for (int j = 0; j < pic.Width; j++)
                {
                    C = pic.GetPixel(j, i);                    //读取原图像的RGB值
                    C2 = mask.GetPixel(j, i);                  //读取蒙板的透明度信息

                    if (isPng)
                    {
                        if (C2.A == 0) C = Color.FromArgb(0, 0, 0, 0);  // Color.Empty;
                        else
                        {
                            int A = (C.A * C2.A / byte.MaxValue);   //按比例值计算透明度信息
                            C = Color.FromArgb(A, C.R, C.G, C.B);   //清除透明度信息
                        }
                    }
                    else
                    {
                        if (C2.R == 0) C = Color.FromArgb(0, 0, 0, 0);  // Color.Empty;
                        else C = Color.FromArgb(C2.R, C.R, C.G, C.B);   //清除透明度信息
                    }
                    pic.SetPixel(j, i, C);
                }
            }

            return pic;
        }

        // 设置默认背景颜色值
        public Bitmap setBackColor(Image Pic, Color defaultC)
        {
            if (Pic == null) return null;

            Bitmap pic = ToBitmap(Pic);

            Color C;
            for (int i = 0; i < pic.Height; i++)
            {
                for (int j = 0; j < pic.Width; j++)
                {
                    C = pic.GetPixel(j, i);                    //读取原图像的RGB值
                    if (C.A == 0) pic.SetPixel(j, i, defaultC);
                }
            }

            return pic;
        }

        //在图像pic上添加logo
        public Bitmap SetLogo(Image pic, Image logo)
        {
            //创建图像
            Bitmap tmp = new Bitmap(pic.Width, pic.Height);//按指定大小创建位图

            //绘制
            Graphics g = Graphics.FromImage(tmp);          //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));           //清空

            g.DrawImage(pic, 0, 0);         //从pic的给定区域进行绘制
            g.DrawImage(logo, 0, 0);        //为图像添加logo

            return tmp;                     //返回构建的新图像
        }


        /// <summary>
        /// 获取图像Pic的10进制颜色值
        /// </summary>
        public String getPixelsData(Image Pic)
        {
            return getPixelsData(Pic, 10);
        }
        /// <summary>
        /// 获取图像Pic的颜色值，转化为toBase进制的串
        /// </summary>
        public String getPixelsData(Image Pic, int toBase)
        {
            if (Pic == null) return "";

            Bitmap pic = ToBitmap(Pic);

            StringBuilder str1 = new StringBuilder();

            for (int i = 0; i < pic.Height; i++)
            {
                for (int j = 0; j < pic.Width; j++)
                {
                    int n = pic.GetPixel(j, i).ToArgb();  //读取原图像的ARGB值
                    str1.Append(Convert.ToString(n, toBase) + (j < pic.Width - 1 ? "," : ""));
                }
                str1.Append(i < pic.Height - 1 ? "\r\n" : "");
            }

            return str1.ToString();
            //SaveProcess(tmp, "colorT");
        }

        //=======================================================
        //获取图像pic的遮罩图像
        //=======================================================

        //将png透明图像，转化为一张不含透明度的jpeg图像 和 一张仅含透明度的png图像
        public Bitmap[] getPicMask(Image pic)
        {
            Bitmap[] tmp = new Bitmap[2];
            tmp[0] = ToBitmap(pic);
            tmp[1] = ToBitmap(pic);

            tmp[0] = getRGB(tmp[0]);    //获取除透明度信息外的图像
            tmp[1] = getAlpha(tmp[1]);  //获取图像的透明度图像

            return tmp;
        }

        //获取图像对应的RGB图像，透明度数据清除
        public Bitmap getRGB(Bitmap pic)
        {
            Color C;
            for (int i = 0; i < pic.Height; i++)
            {
                for (int j = 0; j < pic.Width; j++)
                {
                    C = pic.GetPixel(j, i);
                    C = C.A == 0 ? Color.FromArgb(0, 0, 0, 0) : Color.FromArgb(0, C.R, C.G, C.B);   //清除透明度信息

                    pic.SetPixel(j, i, C);
                }
            }
            return pic;
        }

        //获取图像的遮罩图像，仅保留透明度信息
        public Bitmap getAlpha(Bitmap pic)
        {
            Color C;
            for (int i = 0; i < pic.Height; i++)
            {
                for (int j = 0; j < pic.Width; j++)
                {
                    C = pic.GetPixel(j, i);
                    C = Color.FromArgb(0, C.A, C.A, C.A);   //使用透明度信息生成遮罩图像

                    pic.SetPixel(j, i, C);
                }
            }
            return pic;
        }

        //=======================================================
        //图像剪裁、缩放，转化为鼠标光标
        //=======================================================
        /// <summary>
        /// 从图像pic中截取区域Rect构建新的图像
        /// </summary>
        public Bitmap GetRect(Image pic, Rectangle Rect)
        {
            //创建图像
            Rectangle drawRect = new Rectangle(0, 0, Rect.Width, Rect.Height);  //绘制整块区域
            Bitmap tmp = new Bitmap(drawRect.Width, drawRect.Height);           //按指定大小创建位图

            //绘制
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空
            g.DrawImage(pic, drawRect, Rect, GraphicsUnit.Pixel);   //从pic的给定区域进行绘制

            return tmp;     //返回构建的新图像
        }

        /// <summary>
        /// 从图像pic中截取区域Rect构建为drawRect大小的图像
        /// </summary>
        public Bitmap GetRectTo(Image pic, Rectangle Rect, Rectangle drawRect)
        {
            //创建图像
            Bitmap tmp = new Bitmap(drawRect.Width, drawRect.Height);           //按指定大小创建位图

            //绘制
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空
            g.DrawImage(pic, drawRect, Rect, GraphicsUnit.Pixel);   //从pic的给定区域进行绘制

            return tmp;     //返回构建的新图像
        }

        /// <summary>
        /// 按指定尺寸对图像pic进行非拉伸缩放
        /// </summary>
        public Bitmap shrinkTo(Image pic, Size S, Boolean cutting)
        {
            //创建图像
            Bitmap tmp = new Bitmap(S.Width, S.Height);     //按指定大小创建位图

            //绘制
            Graphics g = Graphics.FromImage(tmp);           //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));            //清空

            Boolean mode = (float)pic.Width / S.Width > (float)pic.Height / S.Height;   //zoom缩放
            if (cutting) mode = !mode;                      //裁切缩放

            //计算Zoom绘制区域             
            if (mode)
                S.Height = (int)((float)pic.Height * S.Width / pic.Width);
            else
                S.Width = (int)((float)pic.Width * S.Height / pic.Height);
            Point P = new Point((tmp.Width - S.Width) / 2, (tmp.Height - S.Height) / 2);

            g.DrawImage(pic, new Rectangle(P, S));

            return tmp;     //返回构建的新图像
        }

        /// <summary>
        /// 对图像pic进行缩放，缩放比例reSize
        /// </summary>
        public Bitmap shrinkTo(Image pic, float reSize)
        {
            Size S = new Size((int)(pic.Width * reSize), (int)(pic.Height * reSize));
            Rectangle Rect = new Rectangle(new Point(0, 0), S);

            return shrinkTo(pic, Rect);
        }
        /// <summary>
        /// 对图像pic进行缩放处理，缩放为Rect大小的新图像
        /// </summary>
        public Bitmap shrinkTo(Image pic, Rectangle Rect)
        {
            //创建图像
            Bitmap tmp = new Bitmap(Rect.Width, Rect.Height);                   //按指定大小创建位图
            Rectangle drawRect = new Rectangle(0, 0, Rect.Width, Rect.Height);  //绘制整块区域
            Rectangle srcRect = new Rectangle(0, 0, pic.Width, pic.Height);     //pic的整个区域

            //绘制
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空
            g.DrawImage(pic, drawRect, srcRect, GraphicsUnit.Pixel);//从pic的给定区域进行绘制

            return tmp;     //返回构建的新图像
        }

        //Image转化为Bitamap
        public Bitmap ToBitmap(Image pic)
        {
            //创建图像
            Bitmap tmp = new Bitmap(pic.Width, pic.Height);                //按指定大小创建位图
            Rectangle Rect = new Rectangle(0, 0, pic.Width, pic.Height);   //pic的整个区域

            //绘制
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空

            g.DrawImage(pic, Rect, Rect, GraphicsUnit.Pixel);       //从pic的给定区域进行绘制

            return tmp;     //返回构建的新图像
        }

        /// <summary>
        /// 转化图像pic为Icon
        /// </summary>
        public Icon ToIcon(Bitmap pic)
        {
            Icon icon = Icon.FromHandle(pic.GetHicon());
            return icon;
        }

        private Rectangle getClipRect(Image pic, float angle)
        {
            //对图像进行剪裁
            double angle1 = Math.Atan2(pic.Height, pic.Width);      //获取当前角度
            double angle2 = Math.PI - angle1;
            double num = Math.Sqrt(pic.Width * pic.Width + pic.Height * pic.Height);


            double width1 = num * Math.Abs(Math.Cos(angle1 + angle)), width2 = num * Math.Abs(Math.Cos(angle2 + angle));
            double height1 = num * Math.Abs(Math.Sin(angle1 + angle)), height2 = num * Math.Abs(Math.Sin(angle2 + angle));
            int w = width1 > width2 ? (int)width1 : (int)width2, h = height1 > height2 ? (int)height1 : (int)height2;

            Rectangle clipRect = new Rectangle(0, 0, w, h);

            return clipRect;
        }

        /// <summary>
        /// 图像沿Y轴翻转
        /// </summary>
        public Bitmap FlipY(Bitmap pic)
        {
            pic.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return pic;
        }

        /// <summary>
        /// 图像沿X轴翻转
        /// </summary>
        public Bitmap FlipX(Bitmap pic)
        {
            pic.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return pic;
        }

        /// <summary>
        /// 对图像进行任意角度的旋转
        /// </summary>
        public Bitmap KiRotate(Bitmap bmp, float angle)
        {
            return KiRotate(bmp, angle, Color.Transparent);
        }
        /// <summary>
        /// 任意角度旋转
        /// </summary>
        /// <param name="bmp">原始图Bitmap</param>
        /// <param name="angle">旋转角度</param>
        /// <param name="bkColor">背景色</param>
        /// <returns>输出Bitmap</returns>
        public Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 2;
            int h = bmp.Height + 2;

            PixelFormat pf;

            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();

            tmp.Dispose();

            return dst;
        }

        /// <summary>
        /// 从给定的图像创建鼠标光标
        /// </summary>
        public Cursor GetCursor(Bitmap pic)
        {
            try { return new Cursor(pic.GetHicon()); }         //从位图创建鼠标图标
            catch (Exception) { return Cursors.Default; }
        }

        /// <summary>
        /// 获取用图像pic，按指定大小width创建鼠标光标
        /// </summary>
        public Cursor GetCursor(Image pic, int width)
        {
            Bitmap icon = new Bitmap(width, width);             //按指定大小创建位图
            Graphics g = Graphics.FromImage(icon);              //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawImage(pic, 0, 0, icon.Width, icon.Height);    //绘制Image到位图

            //Bitmap icon = new Bitmap(tiles[toolsPostion.Y - 1]);

            try { return new Cursor(icon.GetHicon()); }         //从位图创建鼠标图标
            catch (Exception) { return Cursors.Default; }
        }

        //=======================================================
        // 子图的分割，子图依据图片中的透明区域自行分割
        //=======================================================

        /// <summary>
        /// 对图像pic进行图块分割，分割为一个个的矩形子图块区域
        /// 分割原理： 相邻的连续区域构成一个图块，透明区域为分割点
        /// </summary>
        public Rectangle[] GetRects(Bitmap pic)
        {
            List<Rectangle> Rects = new List<Rectangle>();
            Rectangle rect;

            bool[][] Colors = getColors(pic);           //获取图像对应的非透明像素点

            for (int i = 0; i < pic.Height; i++)
            {
                for (int j = 0; j < pic.Width; j++)
                {
                    if (Exist(Colors, i, j))
                    {
                        rect = GetRect(Colors, i, j);   //获取非透明像素点所在的矩形区域

                        if (rect.Width > 10 && rect.Height > 10)    //剔除尺寸小于10x10的子图区域
                            Rects.Add(rect);            //记录区域范围
                    }
                }
            }

            return Rects.ToArray();
        }

        //获取图像buildPic的所有子图区域的图像
        private Bitmap[] getSubPics(Image buildPic, Rectangle[] buildRects)
        {
            //创建载入图像图像
            Bitmap[] buildTiles = new Bitmap[buildRects.Length];
            for (int i = 0; i < buildRects.Length; i++)
            {
                buildTiles[i] = GetRect(buildPic, buildRects[i]);     //获取载入图像的图像
            }

            return buildTiles;
        }

        //判断所有像素点是否存在非透明像素
        public bool[][] getColors(Bitmap pic)
        { return getColors(pic, 0); }

        //判断所有像素点是否存在非透明像素,将颜色Trans视作透明像素点
        public bool[][] getColors(Bitmap pic, int Trans)
        {
            Color C;
            bool[][] has = new bool[pic.Height][];
            int count;

            for (int i = 0; i < pic.Height; i++)
            {
                has[i] = new bool[pic.Width];
                for (int j = 0; j < pic.Width; j++)
                {
                    C = pic.GetPixel(j, i);

                    if (C.ToArgb() == Trans) { has[i][j] = false; continue; }

                    //统计RGB值近似为0的数目
                    count = 0;
                    if (C.R < 4) count++;
                    if (C.G < 4) count++;
                    if (C.B < 4) count++;

                    //若透明度近似为0，视为透明像素。或RGB中有两个值近似为0且透明度很小，也视为透明像素。
                    if (C.A < 3 || (count >= 2 && C.A < 30)) has[i][j] = false;
                    else has[i][j] = true;
                }
            }

            return has;
        }

        //判断坐标处是否存在非透明像素值
        public bool Exist(bool[][] Colors, int x, int y)
        {
            if (x < 0 || y < 0 || x >= Colors.Length || y >= Colors[0].Length) return false;
            else return Colors[x][y];
        }

        //判定区域Rect右侧是否存在像素点
        public bool R_Exist(bool[][] Colors, Rectangle Rect)
        {
            if (Rect.Right >= Colors[0].Length || Rect.Left < 0) return false;
            for (int i = 0; i < Rect.Height; i++)
                if (Exist(Colors, Rect.Top + i, Rect.Right + 1)) return true;
            return false;
        }
        public bool D_Exist(bool[][] Colors, Rectangle Rect)
        {
            if (Rect.Bottom >= Colors.Length || Rect.Top < 0) return false;
            for (int i = 0; i < Rect.Width; i++)
                if (Exist(Colors, Rect.Bottom + 1, Rect.Left + i)) return true;
            return false;
        }
        public bool L_Exist(bool[][] Colors, Rectangle Rect)
        {
            if (Rect.Right >= Colors[0].Length || Rect.Left < 0) return false;
            for (int i = 0; i < Rect.Height; i++)
                if (Exist(Colors, Rect.Top + i, Rect.Left - 1)) return true;
            return false;
        }
        public bool U_Exist(bool[][] Colors, Rectangle Rect)
        {
            if (Rect.Bottom >= Colors.Length || Rect.Top < 0) return false;
            for (int i = 0; i < Rect.Width; i++)
                if (Exist(Colors, Rect.Top - 1, Rect.Left + i)) return true;
            return false;
        }

        //获取坐标所在图块的区域范围
        public Rectangle GetRect0(bool[][] Colors, int x, int y)
        {
            Rectangle Rect = new Rectangle(new Point(y, x), new Size(1, 1));   //创建所在区域

            bool flag;
            do
            {
                flag = false;

                while (R_Exist(Colors, Rect)) { Rect.Width++; flag = true; }
                while (D_Exist(Colors, Rect)) { Rect.Height++; flag = true; }
                while (L_Exist(Colors, Rect)) { Rect.Width++; Rect.X--; flag = true; }
                while (U_Exist(Colors, Rect)) { Rect.Height++; Rect.Y--; flag = true; }
            }
            while (flag == true);

            ////区域范围放大1个像素
            if (Rect.X > 0) { Rect.X--; Rect.Width++; }
            if (Rect.Y > 0) { Rect.Y--; Rect.Height++; }
            if (Rect.X + Rect.Width + 1 < Colors[0].Length) { Rect.Width++; }
            if (Rect.Y + Rect.Height + 1 < Colors.Length) { Rect.Height++; }

            //区域范围放大1个像素
            if (Rect.X > 0) { Rect.X--; Rect.Width++; }
            if (Rect.Y > 0) { Rect.Y--; Rect.Height++; }
            if (Rect.X + Rect.Width + 1 < Colors[0].Length) { Rect.Width++; }
            if (Rect.Y + Rect.Height + 1 < Colors.Length) { Rect.Height++; }

            //Rect = expend2pix(Colors, Rect);    //区域范围扩大2个像素
            clearRect(Colors, Rect);            //清空已选择的区域

            return Rect;
        }

        //获取坐标所在图块的区域范围
        public Rectangle GetRect(bool[][] Colors, int x, int y)
        {
            Rectangle Rect = new Rectangle(new Point(y, x), new Size(1, 1));   //创建所在区域

            bool flag;
            do
            {
                flag = false;

                while (R_Exist(Colors, Rect)) { Rect.Width++; flag = true; }
                while (D_Exist(Colors, Rect)) { Rect.Height++; flag = true; }
                while (L_Exist(Colors, Rect)) { Rect.Width++; Rect.X--; flag = true; }
                while (U_Exist(Colors, Rect)) { Rect.Height++; Rect.Y--; flag = true; }
            }
            while (flag == true);

            clearRect(Colors, Rect);            //清空已选择的区域

            Rect.Width++;
            Rect.Height++;

            return Rect;
        }

        //清空区域内的像素非透明标记
        public void clearRect(bool[][] Colors, Rectangle Rect)
        {
            for (int i = Rect.Top; i <= Rect.Bottom; i++)
                for (int j = Rect.Left; j <= Rect.Right; j++)
                    Colors[i][j] = false;
        }


        //=======================================================
        // png图像裁切，裁切掉图像中多余的空白区域
        //=======================================================

        public Rectangle GetMiniRect(Bitmap pic)
        { return GetMiniRect(pic, 0); }

        /// <summary>
        /// 获取图像pic的最小非透明像素矩形区域大小，自动剔除边缘透明区域
        /// 原理： 记录所有非透明像素点的最小和最大坐标
        /// </summary>
        public Rectangle GetMiniRect(Bitmap pic, int Trans)
        {
            Point P = new Point(-1, -1), Q = new Point(-1, -1);

            bool[][] Colors = getColors(pic, Trans);    //获取图像对应的非透明像素点

            for (int i = 0; i < pic.Height; i++)        //行遍历
            {
                for (int j = 0; j < pic.Width; j++)     //列遍历
                {
                    if (P.X != -1 && P.X <= i && i <= Q.X && P.Y <= j && j <= Q.Y) { j = Q.Y; continue; }  //跳过P与Q之间的列遍历
                    if (Exist(Colors, i, j))
                    {
                        if (P.X == -1) { P = new Point(i, j); Q = new Point(i, j); }    //首个非透明像素点

                        //最小区域左上点
                        if (P.X > i) P.X = i;           //X记录行位置
                        if (P.Y > j) P.Y = j;           //Y记录列位置

                        //最小区域右下点
                        if (Q.X < i) Q.X = i;
                        if (Q.Y < j) Q.Y = j;
                    }
                }
            }

            if (P.X == -1) return new Rectangle(0, 0, 0, 0);
            return new Rectangle(P.Y, P.X, Q.Y - P.Y, Q.X - P.X);
        }

        public Rectangle GetMiniLeftRect(Bitmap pic)
        { return GetMiniLeftRect(pic, 0); }

        /// <summary>
        /// 获取从左上点开始的图像pic的最小非透明像素矩形区域大小，仅剔除右侧或下侧边缘透明区域
        /// 原理： 记录所有非透明像素点的最小和最大坐标
        /// </summary>
        public Rectangle GetMiniLeftRect(Bitmap pic, int Trans)
        {
            Rectangle Rect = GetMiniRect(pic, Trans);  //获取最小非透明像素矩形区域

            Rect.Width += Rect.X;                      //转化为从左上点开始区域，仅剔除右下侧透明区域
            Rect.Height += Rect.Y;
            Rect.X = 0;
            Rect.Y = 0;

            return Rect;
        }

        //=======================================================
        // 注册表操作
        //=======================================================

        # region 注册表操作
        //设置软件开机启动项： RegistrySave(@"Microsoft\Windows\CurrentVersion\Run", "QQ", "C:\\windows\\system32\\QQ.exe");  

        /// <summary>
        /// 记录键值数据到注册表subkey = @"Scimence\Email\Set";
        /// </summary>
        public void RegistrySave(string subkey, string name, object value)
        {
            //设置一个具有写权限的键 访问键注册表"HKEY_CURRENT_USER\Software"
            Microsoft.Win32.RegistryKey keyCur = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", true);
            Microsoft.Win32.RegistryKey keySet = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\" + subkey, true);
            if (keySet == null) keySet = keyCur.CreateSubKey(subkey);   //键不存在时创建

            keySet.SetValue(name, value);   //保存键值数据
        }

        /// <summary>
        /// 获取注册表subkey下键name的字符串值
        /// <summary>
        public string RegistryStrValue(string subkey, string name)
        {
            object value = RegistryValue(subkey, name);
            return value == null ? "" : value.ToString();
        }

        /// <summary>
        /// 获取注册表subkey下键name的值
        /// <summary>
        public object RegistryValue(string subkey, string name)
        {
            Microsoft.Win32.RegistryKey keySet = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\" + subkey, true);
            return (keySet == null ? null : keySet.GetValue(name, null));
        }

        /// <summary>
        /// 判断注册表是否含有子键subkey
        /// <summary>
        public bool RegistryCotains(string subkey)
        {
            Microsoft.Win32.RegistryKey keySet = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\" + subkey, true);
            return (keySet != null);
        }

        /// <summary>
        /// 判断注册表subkey下是否含有name键值信息
        /// <summary>
        public bool RegistryCotains(string subkey, string name)
        {
            //设置一个具有写权限的键 访问键注册表"HKEY_CURRENT_USER\Software"
            Microsoft.Win32.RegistryKey keySet = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\" + subkey, true);

            if (keySet == null) return false;
            else return keySet.GetValueNames().Contains<string>(name);
        }

        /// <summary>
        /// 删除注册表subkey信息
        /// <summary>
        public void RegistryRemove(string subkey)
        {
            //设置一个具有写权限的键 访问键注册表"HKEY_CURRENT_USER\Software"
            Microsoft.Win32.RegistryKey keyCur = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", true);
            Microsoft.Win32.RegistryKey keySet = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\" + subkey, true);

            if (keySet != null) keyCur.DeleteSubKeyTree(subkey);      //删除注册表信息
        }

        /// <summary>
        /// 删除注册表subkey下的name键值信息
        /// <summary>
        public void RegistryRemove(string subkey, string name)
        {
            //设置一个具有写权限的键 访问键注册表"HKEY_CURRENT_USER\Software"
            Microsoft.Win32.RegistryKey keySet = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\" + subkey, true);
            if (keySet != null) keySet.DeleteValue(name, false);
        }

        # endregion


        //=======================================================
        // 提示信息
        //=======================================================

        /// <summary>
        /// 显示提示信息str,并选择是否打开文件目录Dir
        /// </summary>
        public void MessageWithOpen(String str, String Dir)
        {
            bool ok = (MessageBox.Show(str, "打开？", MessageBoxButtons.OKCancel) == DialogResult.OK);
            if (ok) System.Diagnostics.Process.Start("explorer.exe", "/e,/select, " + Dir);
        }

    }

}
