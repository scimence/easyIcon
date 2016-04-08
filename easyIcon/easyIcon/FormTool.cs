using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easyIcon
{
    public class FormTool
    {
        /// <summary>
        /// 向列表中添加列表项
        /// </summary>
        public void addIteams(CheckedListBox checkList, string[] texts)
        {
            if (checkList.Items.Count > 0) checkList.Items.Clear();
            if(texts != null) foreach (string text in texts) checkList.Items.Add(text, false);
        }

        /// <summary>
        /// 设置控件picBox在父控件中，居中显示
        /// </summary>
        public void CenterOnParent(PictureBox picBox, Size parent)
        {
            Image pic = picBox.Image;

            // 若没有要显示的图像，则显示默认尺寸
            if (pic == null)
            {
                picBox.Width = 200;
                picBox.Height = 200;
            }
            // 居中显示较小的图像
            else if (pic.Width <= parent.Width && pic.Height <= parent.Height)
            {
                picBox.Width = pic.Width;
                picBox.Height = pic.Height;
            }
            // 拉伸显示较大的图像
            else
            {
                if (pic.Width >= pic.Height)
                {
                    picBox.Width = parent.Width;
                    picBox.Height = pic.Height * picBox.Width / pic.Width;
                }
                else
                {
                    picBox.Height = parent.Height;
                    picBox.Width = pic.Width * picBox.Height / pic.Height;
                }
            }

            // 居中显示pictureBox
            picBox.Left = (parent.Width - picBox.Width) / 2;
            picBox.Top = (parent.Height - picBox.Height) / 2;
        }

    }    
}
