namespace first_demo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.创建图片 = new System.Windows.Forms.Button();
            this.打开本地图片 = new System.Windows.Forms.Button();
            this.imageBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // 创建图片
            // 
            this.创建图片.Location = new System.Drawing.Point(12, 227);
            this.创建图片.Name = "创建图片";
            this.创建图片.Size = new System.Drawing.Size(81, 22);
            this.创建图片.TabIndex = 0;
            this.创建图片.Text = "创建图片";
            this.创建图片.UseVisualStyleBackColor = true;
            this.创建图片.Click += new System.EventHandler(this.button1_Click);
            // 
            // 打开本地图片
            // 
            this.打开本地图片.Location = new System.Drawing.Point(182, 226);
            this.打开本地图片.Name = "打开本地图片";
            this.打开本地图片.Size = new System.Drawing.Size(90, 23);
            this.打开本地图片.TabIndex = 1;
            this.打开本地图片.Text = "打开本地图片";
            this.打开本地图片.UseVisualStyleBackColor = true;
            this.打开本地图片.Click += new System.EventHandler(this.button2_Click);
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(28, 41);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(215, 140);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.打开本地图片);
            this.Controls.Add(this.创建图片);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button 创建图片;
        private System.Windows.Forms.Button 打开本地图片;
        private System.Windows.Forms.PictureBox imageBox1;
    }
}

