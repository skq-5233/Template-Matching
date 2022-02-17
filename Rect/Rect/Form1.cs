using System;
using System.Drawing;
using System.Windows.Forms;


namespace Rect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Image<Bgr, byte> img = newImage<Bgr, byte>(200, 200, new Bgr(255, 255, 255));   //创建一张白色，尺寸为200*200的图片；
            Rectangle rect = new Rectangle(new Point(80, 80), new Size(40, 40));    //创建一个矩形，左上角坐标为（80,80），尺寸为40*40；
            CircleF circlef = new CircleF(new PointF(100, 100),40); //创建一个中心坐标点为（100,100），半径为40的圆形；
            string str = "Hello to EmguCv";
            Point str_location = new Point(0, 30);  //创建Point类型作为字符串的左上角坐标为（0,30）；
            img.Draw(rect, new Bgr(0, 255, 0), 2);  //指定参数绘画矩形；
            img.Draw(circlef, new Bgr(0, 0, 255), 3); //指定参数绘画圆形；
            img.Draw(str,str_location, Rect.FontFace.HersheyComolexSmall,1,new Bgr(255,0,0),3);//指定参数绘画字体；
            Image mat = null;
            imageBox1.Image = mat;//显示绘画后的图片；


        }

        public object Emgu { get; private set; }   

        private Image<Bgr, byte> newImage<T1, T2>(int v1, int v2, Bgr bgr)
        {
            throw new NotImplementedException();
        }
    }
}
