namespace easyIcon
{
    partial class Register
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Register));
            this.label_machineCode = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label__SerialNum = new System.Windows.Forms.Label();
            this.textBox_SerialNum = new System.Windows.Forms.TextBox();
            this.label_MachineSerial = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_machineCode
            // 
            this.label_machineCode.AutoSize = true;
            this.label_machineCode.Location = new System.Drawing.Point(9, 14);
            this.label_machineCode.Name = "label_machineCode";
            this.label_machineCode.Size = new System.Drawing.Size(77, 12);
            this.label_machineCode.TabIndex = 1;
            this.label_machineCode.Text = "您的机器码：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(213, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "注册";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label__SerialNum
            // 
            this.label__SerialNum.AutoSize = true;
            this.label__SerialNum.Location = new System.Drawing.Point(9, 45);
            this.label__SerialNum.Name = "label__SerialNum";
            this.label__SerialNum.Size = new System.Drawing.Size(77, 12);
            this.label__SerialNum.TabIndex = 4;
            this.label__SerialNum.Text = "输入注册码：";
            // 
            // textBox_SerialNum
            // 
            this.textBox_SerialNum.Location = new System.Drawing.Point(87, 42);
            this.textBox_SerialNum.Name = "textBox_SerialNum";
            this.textBox_SerialNum.Size = new System.Drawing.Size(120, 21);
            this.textBox_SerialNum.TabIndex = 3;
            // 
            // label_MachineSerial
            // 
            this.label_MachineSerial.AutoSize = true;
            this.label_MachineSerial.Location = new System.Drawing.Point(88, 14);
            this.label_MachineSerial.Name = "label_MachineSerial";
            this.label_MachineSerial.Size = new System.Drawing.Size(119, 12);
            this.label_MachineSerial.TabIndex = 5;
            this.label_MachineSerial.Text = "XXXX-XXXX-XXXX-XXXX";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = global::easyIcon.Properties.Resources.price_5;
            this.pictureBox1.Location = new System.Drawing.Point(12, 96);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(271, 298);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label_MachineSerial);
            this.panel1.Controls.Add(this.label__SerialNum);
            this.panel1.Controls.Add(this.textBox_SerialNum);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label_machineCode);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(271, 78);
            this.panel1.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(289, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 382);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "获取注册码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(102, 352);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Mail: scimence@hotmail.com\r\nCopyright ©  2015 Scimence\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(233, 60);
            this.label2.TabIndex = 1;
            this.label2.Text = "银行转账：\r\n\r\n   招商银行 6225 8855 1919 7065 王忠愿\r\n\r\n短信 \"您的账号\" + 机器码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 108);
            this.label1.TabIndex = 0;
            this.label1.Text = "   1、付款： 支付宝付款（至187 5604 0776）\r\n\r\n   2、留言： 短信 \"您的支付宝名\" + 机器码\r\n\r\n至 187 5604 0776\r\n" +
    "  \r\n   3、反馈：我在收到信息后，会通过短信回复\r\n\r\n给您 注册码 （或通过联网方式，直接为您注册）";
            // 
            // Register
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 404);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Register";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "easyIcon 注册";
            this.Load += new System.EventHandler(this.Register_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_machineCode;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label__SerialNum;
        private System.Windows.Forms.TextBox textBox_SerialNum;
        private System.Windows.Forms.Label label_MachineSerial;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}