using Emgu.CV.Structure;  // add;
using Emgu.CV;           // add;
using Emgu.CV.CvEnum;    // add;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace first_demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  //在“创建图片”按钮的单击事件中写入代码；
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(446, 264, new Bgr(0, 0, 255)); //创建一张446*264尺寸大小的红色图像；
            this.imageBox1.Image = image.ToBitmap(); //在ImageBox1控件中显示所创建好的图像；(Bitmap位图文件，这里将image改为image.ToBitmap)；
        }

        private void button2_Click(object sender, EventArgs e)  // 在“打开本地图片”按钮的单击事件中写入代码；
        {
            OpenFileDialog op = new OpenFileDialog();  //实例化打开对话框；
            if (op.ShowDialog() == DialogResult.OK)
                {
                Mat scr = new Mat(op.FileName, Emgu.CV.CvEnum.LoadImageType.AnyColor); //指定路径加载图片；
                //imageBox1.Image = Image.FromFile("D:\\VS-Code\\Demo\first_demo\\VR.jpg");
                imageBox1.Image = scr.Bitmap; //显示加载完成的图片；(这里将scr改为scr.Bitmap)；
            }
        }
    }
}
