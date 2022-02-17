using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bianli
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Image<Bgr, Byte> img1 = new Image<Bgr, Byte>(320, 240,new Bgr(255, 0, 0));//创建一张大小为320*240的蓝色图像；
            Byte b1 = 255;
            for (int i = 20; i < 60; i++)
            {
                for (int j = 20; j < 60; j++)
                {
                    img1.Data[i, j, 0] = 0;//第一通道的某坐标赋值；
                    img1.Data[i, j, 1] = b1;//第二通道的某坐标赋值；
                    img1.Data[i, j, 2] = b1;//第三通道的某坐标赋值；
                }
            }
        }

        private class Bgr
        {
            private int v1;
            private int v2;
            private int v3;

            public Bgr(int v1, int v2, int v3) : this(v1, v2, v3)
            {
            }

            public Bgr(int v1, int v2, int v3)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.v3 = v3;
            }
        }
    }
}
