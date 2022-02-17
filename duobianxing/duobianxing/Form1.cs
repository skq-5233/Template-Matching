using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace duobianxing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Random m = new Random(); //创建一个随机数产生器；
            Image<Bgr, byte> mat = new Image<Bgr, byte>(200, 200, new Bgr(255, 255, 255)); //创建一张白色的，尺寸为200*200的图片；
            Point[] pt = new Point[3];  //创建一个三点数组；
            Point[] pt1 = new Point[3];
            Point[] pt2 = new Point[3];
            for (int i=0;i<3;i++)
            {
                pt[i] = new Point(m.Next(0, 200), m.Next(0, 200));//对数组分别赋值；
                pt1[i] = new Point(m.Next(0, 200), m.Next(0, 200));
                pt2[i] = new Point(m.Next(0, 200), m.Next(0, 200));
            }
            Point[][] pts = new Point[2][];//创建多个数组；
            pts[0] = pt1;//对多数组赋值；
            pts[1] = pt2;
            mat.DrawPolyline(pt, true, new Bgr(0, 0, 255), 2);//绘制多边形；
            mat.DrawPolyline(pt, true, new Bgr(255, 0, 0), 3);//绘制多边形；
            Image image = null;
            imageBox1.Image = image;//显示图像；
        }
    }
}
