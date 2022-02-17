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
using System.Runtime.Serialization;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Template_Match
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> image;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "JPEG;BMP;PNG;JPG|*.JPEG;*.BMP;*.PNG;*.JPG|ALL|*.*";

            OpenFileDialog.RestoreDirectory = true;
            OpenFileDialog.FilterIndex = 1;

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    image = new Image<Bgr, byte>(OpenFileDialog.FileName);
                    pictureBox1.Image = image.ToBitmap();

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("图像格式错误！");
                }

            }
        }
    }
}
