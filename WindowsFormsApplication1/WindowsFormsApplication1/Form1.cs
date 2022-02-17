using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.Serialization;
using Emgu.CV.CvEnum;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private object img;
        private readonly object img1;
        private object temp;
        //private SerializationInfo temp;

        public Form1()
        {
            InitializeComponent();
        }

        //protected Image();
        //public Image(SerializationInfo info, StreamingContext context);

        //public Mat(string fileName, ImreadModes loadType = ImreadModes.Color); 
        //public Mat(Mat mat, Rectangle roi);



        private void button1_Click(object sender, EventArgs e)
            {
            {
                Image<Bgr, byte> img = new Image<Bgr, byte>("D:\\img\\HIGH\\Image0001.bmp");//加载图像；
                pictureBox1.Image = img.ToBitmap();//显示图像；

                //Image<Bgr, byte> img1 = new Image<Bgr, byte>("D:\\img\\HIGH\\Image0001.bmp");//加载图像；


                //Mat image1 = new Mat("‪C:\\Image0001.bmp", WindowsFormsApplication1.LoadImageType.AnyColor);//加载图像；

                //Image<Bgr, byte> temp = new Image<Bgr, byte>("C:\\Image0001_screen.bmp");//加载图像；
                //Mat temp = new Mat("‪C:\\Image0001_temp.bmp", WindowsFormsApplication1.LoadImageType.AnyColor);//加载temp图像；

                //Mat image = new Mat(image1, new Rectangle(new Point(80, 80), temp.Size));//提取image1图像的ROI,保存在image中；

                //Image<Bgr, byte> img = new Image<Bgr, byte>("img1,new Rectangle(new Point(80,80),temp.Size");//提取img1图像的ROI,保存在img中；
            }

                //temp.CopyTo(img, null);//将模板数据赋值给img；
                //pictureBox1.Image = img1.ToBitmap();//显示图像；
        }
    }

    public class bmp
    {
    }
}
