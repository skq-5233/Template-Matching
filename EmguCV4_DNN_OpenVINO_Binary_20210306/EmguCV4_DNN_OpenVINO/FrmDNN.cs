using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.Util.TypeEnum;
using Emgu.CV.Structure;
using Emgu.CV.Dnn;
using Emgu.CV.CvEnum;

namespace EmguCV4_DNN
{
    public partial class FrmDNN : Form
    {
    
        List<string> typeList = new List<string>();

        String defaultfilepath;
        Net net;                 //网络
        Int32 picturecount = 0;  //图片总数
        Int32[] classcount;      //各类图片计数
        Thread thread1;          //目录测试线程
        Int32 ComboBoxindex = 0;

        public FrmDNN()
        {
            InitializeComponent();

            label1.Text = "";
            defaultfilepath = "";
            comboBox1.SelectedIndex = 0;
        }
        
        //单张测试
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < classcount.Length; i++)
            {
                classcount[i] = 0;
            }

            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)  //当点击文件对话框的确定按钮时打开相应的文件
            {
                ImageProcessingOne(openFileDialog1.FileName);
                return;
            }
            else
            {
                return;
            }
        }

        //打开分类文件
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.FileName = "";

            if (openFileDialog2.ShowDialog() == DialogResult.OK)  //当点击文件对话框的确定按钮时打开相应的文件
            {
                StreamReader sr = new StreamReader(openFileDialog2.FileName);
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    typeList.Add(line);
                }

                if (typeList == null)
                {
                    label1.Text = "分类文件为空！";
                }
                else
                {
                    classcount = new Int32[typeList.Count];

                    label1.Text = "已打开分类文件";
                    button4.Enabled = true;
                }
            }
            else
            {
                return;
            }
        }

        //目录测试
        private void button3_Click(object sender, EventArgs e)
        {
            picturecount = 0;

            for (int i = 0; i < classcount.Length; i++)
            {
                classcount[i] = 0;
            }

            if (defaultfilepath != "")
            {
                folderBrowserDialog1.SelectedPath = defaultfilepath;
            }
            else
            {
                folderBrowserDialog1.SelectedPath = Application.StartupPath;
            }
  
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                defaultfilepath = folderBrowserDialog1.SelectedPath;
                thread1 = new Thread(new ThreadStart(ImageProcessingAll));//创建线程
                thread1.Start();
            }
        }

        //单张测试函数
        private void ImageProcessingOne(String filename)    //处理指定的单张图片
        {
            Mat img;  //待测试图片

            img = CvInvoke.Imread(filename, Emgu.CV.CvEnum.ImreadModes.AnyColor);

            if (img.IsEmpty)
            {
                label1.Text = "无法加载文件！";
                return;
            }

            //开始时间
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间

            //对输入图像数据进行处理
            Mat blob = DnnInvoke.BlobFromImage(img, 1.0f, new Size(224, 224), new MCvScalar(), true, false);

            //进行图像分类预测
            Mat prob;          

            net.SetInput(blob);
            prob = net.Forward();

            //得到最可能分类输出
            Mat probMat = prob.Reshape(1, 1);
            double minVal = 0;  //最小可能性
            double maxVal = 0;  //最大可能性
            Point minLoc = new Point();
            Point maxLoc = new Point();

            CvInvoke.MinMaxLoc(probMat, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            double classProb = maxVal;     //最大可能性
            Point classNumber = maxLoc;    //最大可能性位置

            string typeName = typeList[classNumber.X];
 
            stopwatch.Stop();  //  停止监视
            long timespan = stopwatch.ElapsedMilliseconds;  //获取当前实例测量得出的总时间

            classcount[classNumber.X]++;
            picturecount++;

            if(comboBox1.SelectedIndex == 1)    //保存NG图像
            {
                if(classNumber.X == 0)
                {
                    CvInvoke.Imwrite("result/NG/" + Path.GetFileName(filename), img);
                }
            }
            else if(comboBox1.SelectedIndex == 2)    //保存OK图像
            {
                if (classNumber.X == 1)
                {
                    CvInvoke.Imwrite("result/OK/" + Path.GetFileName(filename), img);
                }
            }

            label2.Text = "图片名称：" + Path.GetFileName(filename);
            label2.Refresh();

            label3.Text = "图片总数：" + picturecount.ToString();
            label3.Refresh();

            label4.Text = typeList[0] + "：" + classcount[0].ToString();
            label4.Refresh();

            label5.Text = typeList[1] + "：" + classcount[1].ToString();
            label5.Refresh();

            //检测内容
            pictureBox1.Image = BitmapExtension.ToBitmap(img);

            label1.Text = "图片测试结果为：" + typeName + "  可能性为：" + classProb.ToString("0.00000") + "  处理总时间为：" + timespan.ToString() + "ms";
            label1.Refresh();
        }

        //目录测试函数

        double nMax = 0;
        double nMin = 1;
        private void ImageProcessingAll()   //处理文件指定文件夹下所有图片
        {
            nMax = 0;
            nMin = 1;
            double dThreValue = Convert.ToDouble(ThreshTb.Text);

            Mat img;  //待测试图片
            DirectoryInfo folder;
            
            folder = new DirectoryInfo(defaultfilepath);
            
            //遍历文件
            foreach (FileInfo nextfile in folder.GetFiles())
            {
                Invoke((EventHandler)delegate { label2.Text = "图片名称：" + Path.GetFileName(nextfile.FullName); });
                Invoke((EventHandler)delegate { label2.Refresh(); });

                img = CvInvoke.Imread(nextfile.FullName, Emgu.CV.CvEnum.ImreadModes.AnyColor);

                if (img.IsEmpty)
                {
                    Invoke((EventHandler)delegate { label1.Text = "无法加载文件！"; });
                    Invoke((EventHandler)delegate { label1.Refresh(); });
                    return;
                }

                //开始时间
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();   //开始监视代码运行时间

                //对输入图像数据进行处理
                Mat blob = DnnInvoke.BlobFromImage(img, 1.0f, new Size(224, 224), new MCvScalar(), true, false);

                //进行图像种类预测
                Mat prob;
              
                net.SetInput(blob);      //设置输入数据
                prob = net.Forward();    //推理

                //得到最可能分类输出
                Mat probMat = prob.Reshape(1, 1);
                double minVal = 0;  //最小可能性
                double maxVal = 0;  //最大可能性
                Point minLoc = new Point();
                Point maxLoc = new Point();

                CvInvoke.MinMaxLoc(probMat, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                double classProb = maxVal;     //最大可能性
                Point classNumber = maxLoc;    //最大可能性位置



                if(nMin > maxVal)
                {
                    nMin = maxVal;
                }
                if(nMax < maxVal)
                {
                    nMax = maxVal;
                }

                string typeName = typeList[classNumber.X];

                stopwatch.Stop();  //  停止监视
                long timespan = stopwatch.ElapsedMilliseconds;  //获取当前实例测量得出的总时间

                classcount[classNumber.X]++;
                picturecount++;

                if (ComboBoxindex == 1)    //保存NG图像
                {
                    //if (classNumber.X == 0)
                    //{
                    //    CvInvoke.Imwrite("result/NG/" + Path.GetFileName(nextfile.FullName), img);
                    //}

                    if (maxVal < dThreValue)
                    {
                        CvInvoke.Imwrite("result/NG/" + Path.GetFileName(nextfile.FullName), img);
                    }


                }
                else if (ComboBoxindex == 2)    //保存OK图像
                {
                    //if (classNumber.X == 1)
                    //{
                    //    CvInvoke.Imwrite("result/OK/" + Path.GetFileName(nextfile.FullName), img);
                    //}
                    if (maxVal > dThreValue)
                    {
                        CvInvoke.Imwrite("result/OK/" + Path.GetFileName(nextfile.FullName), img);
                    }
                }

                Invoke((EventHandler)delegate { label3.Text = "图片总数：" + picturecount.ToString(); });
                Invoke((EventHandler)delegate { label3.Refresh(); });

                Invoke((EventHandler)delegate { label4.Text = typeList[0] + "：" + classcount[0].ToString(); });
                Invoke((EventHandler)delegate { label4.Refresh(); });

                Invoke((EventHandler)delegate { label5.Text = typeList[1] + "：" + classcount[1].ToString(); });
                Invoke((EventHandler)delegate { label5.Refresh(); });

                Invoke((EventHandler)delegate { MinLab.Text = "Min:" + nMin.ToString("0.00000"); });
                Invoke((EventHandler)delegate { MinLab.Refresh(); });

                Invoke((EventHandler)delegate { MaxLab.Text = "Max:" + nMax.ToString("0.00000"); });
                Invoke((EventHandler)delegate { MaxLab.Refresh(); });



                //if (timespan < 200)    //为便于观察处理结果，每隔200ms处理一张图片
                //{
                //    timeDelay(200 - timespan);
                //}

                //检测内容
                Invoke((EventHandler)delegate { pictureBox1.Image = BitmapExtension.ToBitmap(img); });
                Invoke((EventHandler)delegate { pictureBox1.Refresh(); });

                Invoke((EventHandler)delegate { label1.Text = "图片测试结果为：" + typeName + "  可能性为：" + classProb.ToString("0.00000") + "  处理总时间为：" + timespan.ToString() + "ms"; });
                Invoke((EventHandler)delegate { label1.Refresh(); });
            }
            
        }

        //打开模型文件
        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog3.FileName = "";
            Mat imgfirst;  //待测试图片

            if (openFileDialog3.ShowDialog() == DialogResult.OK)  //当点击文件对话框的确定按钮时打开相应的文件
            {
                // 加载网络
                String tf_pb_file = openFileDialog3.FileName;

                net = DnnInvoke.ReadNetFromTensorflow(tf_pb_file);

                net.SetPreferableBackend(Emgu.CV.Dnn.Backend.InferenceEngine);
                net.SetPreferableTarget(Target.Cpu);

                if (net.Empty)
                {
                    label1.Text = "请确认模型文件是否为空文件";
                    return;
                }

                //模型加载后第一次运行较慢，为准确测试运行时间提前推理一次
                imgfirst = new Mat(224, 224, DepthType.Cv8U, 3);
                //对输入图像数据进行处理
                Mat blobfirst = DnnInvoke.BlobFromImage(imgfirst, 1.0f, new Size(224, 224), new MCvScalar(), true, false);

                //进行图像种类预测
                net.SetInput(blobfirst);
                net.Forward();

                label1.Text = "已打开模型文件";
                button1.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
            }
        }

        //延时函数
        private void timeDelay(long iInterval)    
        {
            DateTime now = DateTime.Now;

            while (now.AddMilliseconds(iInterval) > DateTime.Now)
            {
            }
            return;
        }

        //保存图片选项
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxindex = comboBox1.SelectedIndex;
        }
    }
}
