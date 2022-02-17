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
using Emgu.CV.Util;
using Emgu.CV.Cuda;
using System.IO;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Diagnostics;
using System.Threading;

namespace Emgu.CV.CvEnum
{

    public partial class Form1 : Form
    {
        //加载单幅图像；
        #region       
        //Mat img = new Mat("D:\\software\\VS-Code\\Demo\\Image0001.bmp");//输入原始图像；               
        //Mat temp = new Mat("‪D:\\software\\VS-Code\\Demo\\Image0001_temp.bmp");//输入匹配模板； 
        //Image<Bgr, Byte> result;        
        #endregion
        Image<Bgr, byte> temp;
        Image<Bgr, byte> match_img;
        //Mat match_img = new Mat();
        Mat convert_img = new Mat();
        Image<Bgr, Byte> match_img1;
        Image<Bgr, Byte> match_area_img;
        Image<Bgr, byte> match_area_img_1;
        Image<Gray, byte> match_area_img_out;//(定义一个灰度图)
        Image<Gray, byte> binary_img;
        //Image<Bgr, byte> contour_img; 

        Mat corrosion1 = new Mat();//创建Mat，存储处理后的图像；
        Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
        Mat corrosion_img = new Mat();//创建Mat，存储处理后的图像；
        Mat dilate_img = new Mat();//创建Mat，存储处理后的图像；
        Mat img7 = new Mat();//创建Mat，存储处理后的图像；
        Mat binary = new Mat();
        Mat contour_img = new Mat();
        Mat swell = new Mat();//创建Mat，存储处理后的图像；
        Mat canny_img = new Mat(); //创建dst，用来存储Canny算子处理后的图像；


        //(add,2021-1229,获取文件名);
        string dbf_File = string.Empty;
        OpenFileDialog OpenFileDialog = new OpenFileDialog();
        private int indexOfExcel_j = 1;
        FileStream filestream = null;

        XSSFWorkbook wk = null;
        ISheet isheet = null;

        IRow rowWrite = null;


        double max = 0, min = 0;//创建double极值；
        Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；

        //加载整个文件夹(2022-0106--start)；
        List<string> typeList = new List<string>();

        String defaultfilepath;
        Int32 picturecount = 0;  //图片总数
        //Int32[] classcount;      //各类图片计数

        Int32 classcount = 1;      //各类图片计数

        Thread thread1;
        Int32 ComboBoxindex = 0;
        //加载整个文件夹(2022-0106--end)；

        //模式切换选择（2022-0113）；
        bool auto_or_manual = true;

        public Form1()
        {
            InitializeComponent();

            defaultfilepath = "";
            label18.Text = "";
            //加载、显示图像；
            #region
            //Image<Bgr, byte> img = new Image<Bgr, byte>("D:\\img\\HIGH\\Image0002.bmp");//加载图像；
            //pictureBox1.Image = img.ToBitmap();//显示图像；
            //Image<Bgr, byte> temp = new Image<Bgr, byte>("D:\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Image_temp00.bmp");//加载模板图像；
            #endregion
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        //模式切换选择（2022-0113--start）；
        private void button4_Click(object sender, EventArgs e)
        {
            if(auto_or_manual)
            {
                groupBox3.Visible = false;
                groupBox1.Visible = true;
                auto_or_manual = !auto_or_manual;
            }
            else
            {
                groupBox3.Visible = true;
                groupBox1.Visible = false;
                auto_or_manual = !auto_or_manual;
            }
        }
        //模式切换选择（2022-0113--end）；


        private void LoadTemplate_Click(object sender, EventArgs e)
        {
            /*********手动模式下，测试单张图像--1(2022-0111--start)************/
            /************************************************************/
            /******************加载模板图像--1(2022-0111--start)*************/

            // 加载模板图像；
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "JPEG;BMP;PNG;JPG;GIF|*.JPEG;*.BMP;*.PNG;*.JPG;*.GIF|ALL|*.*";//（模板和加载图像尺寸不一致时，会报错）;


            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                //(add-2021-1228,保存处理后的图像至指定文件夹)
                dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                try
                {
                    temp = new Image<Bgr, Byte>(OpenFileDialog.FileName);
                    pictureBox1.Image = temp.ToBitmap();

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("图像格式错误！");
                }

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                //显示、保存图像；
                #region
                ////CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_temp.bmp", temp); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                #endregion

                /*********手动模式下，测试单张图像--1(2022-0111--end)************/
                /************************************************************/
                /******************加载模板图像--1(2022-0111--end)*************/
            }
        }

        private void Matchimage_Click(object sender, EventArgs e)
        {
            /*********手动模式下，测试单张图像--2(2022-0111--start)************/
            /************************************************************/
            /******加载待匹配图像并画出矩形框--2(2022-0111--start)*************/


            //string dbf_File = string.Empty;
            // 加载待匹配图像并画出矩形框；
            //OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "JPEG;BMP;PNG;JPG;GIF|*.JPEG;*.BMP;*.PNG;*.JPG;*.GIF|ALL|*.*";//（模板和加载图像尺寸不一致时，会报错）;

            OpenFileDialog.Multiselect = true;//(2021-1231)该值确定是否可以选择多个文件;

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {

                string[] files = OpenFileDialog.FileNames;//(2021-1231)该值确定是否可以选择多个文件;

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                try
                {
                    match_img = new Image<Bgr, Byte>(OpenFileDialog.FileName);
                    Mat result = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                    CvInvoke.MatchTemplate(match_img, temp, result, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；

                    //CvInvoke.Normalize(result, result, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                    //double max = 0, min = 0;//创建double极值；
                    //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
                    CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                    match_img1 = match_img.Copy(); //将原图match_img复制到match_img1中，对match_img1进行画矩形框，避免pictureBox3显示匹配区域出现边框；
                    CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
                    pictureBox2.Image = match_img1.ToBitmap();//显示找到模板图像的待搜索图像；

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("图像格式错误！");
                }

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                //显示、保存图像；
                #region
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_match_img1.bmp", match_img1); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                #endregion

                /*********手动模式下，测试单张图像--2(2022-0111--end)************/
                /************************************************************/
                /******加载待匹配图像并画出矩形框--2(2022-0111--end)*************/

            }
            //模板匹配；
            #region
            /*mat result = new mat(new size(image.width - temp.width + 1, image.height - temp.height + 1), depthtype.cv32f, 1);//创建mat存储输出匹配结果；
            cvinvoke.matchtemplate(image, temp, result, cvenum.templatematchingtype.ccoeff);//采用系数匹配法，数值越大越接近准确图像；
            cvinvoke.normalize(result, result, 255, 0, cvenum.normtype.minmax); //对数据进行（min,max;0-255）归一化；
            double max = 0, min = 0;//创建double极值；
            point max_loc = new point(0, 0), min_loc = new point(0, 0);//创建dpoint类型，表示极值的坐标；
            cvinvoke.minmaxloc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；        
            cvinvoke.rectangle(image, new rectangle(max_loc, temp.size), new mcvscalar(0, 0, 255), 3);//绘制矩形，匹配得到的结果；
            //picturebox1.image = temp.tobitmap();//显示模板图像；
            picturebox2.image = image.tobitmap();//显示找到模板图像的待搜索图像；
            //picturebox3.image = result.toimage<gray, byte>().tobitmap();//显示匹配结果，若 picturebox3.image = result显示为全黑，必须将类型转换为byte；*/
            #endregion
        }

        private void MatchingArea_Click(object sender, EventArgs e)
        {
            /*********手动模式下，测试单张图像--3(2022-0111--start)************/
            /************************************************************/
            /******获取匹配区域并打印匹配信息--3(2022-0111--start)*************************/


            // 获取匹配区域；
            Mat result = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
            CvInvoke.MatchTemplate(match_img, temp, result, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);//采用系数匹配法(CcorrNormed)，打印出匹配相似度信息,数值越大越接近准确图像；
            //CvInvoke.Normalize(result, result, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
            //double max = 0, min = 0;//创建double极值；
            Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
            CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

            //(add-2021-1228,保存处理后的图像至指定文件夹)
            dbf_File = OpenFileDialog.FileName;
            //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

            string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
            //(add-2021-1228,保存处理后的图像至指定文件夹)

            match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图match_img复制到match_area_img中，显示匹配区域；
            pictureBox3.Image = match_area_img.ToBitmap();//显示匹配区域；

            //显示、保存图像；
            #region
            //CvInvoke.Imshow("img", temp); //显示图片
            CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_match_area_img.bmp", match_area_img); //保存匹配结果图像；
            CvInvoke.WaitKey(0); //暂停按键等待
            #endregion

            //////datetime格式化；
            //DateTime dt = DateTime.Now;

            ////在Excel表中写入时间数据时，设置单元格格式，自定义格式yyyy-MM-dd HH:mm:ss
            //(label as TextBox).Text = dt.ToLocalTime().ToString();   //mm-dd才显示08-01，否则显示8-1
            ////fff个数表示小数点后显示的位数，这里精确到小数点后3位

            ////datetime格式化；
            DateTime dt = DateTime.Now;

            //打印匹配信息（2021-1228,保存文本信息至指定文件夹）；
            displab.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" + "图像名称：" + dbf_File2 + "\n" +
                            "\n" + "匹配信息: X= " +
                             max_loc.X + "," + " Y= " + max_loc.Y + ";" +
                            "\n" +
                            "最大相似度: " + max.ToString("f2") + ";" + "\n" +
                            "最小相似度: " + min.ToString("f2") + ";" + "\n";

            //（2021-1228,保存文本信息至指定文件夹）；
            string txt = displab.Text;
            string filename = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息.txt";   //文件名，可以带路径

            FileStream fs = new FileStream(filename, FileMode.Append);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);

            //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
            wr.Write(displab.Text);
            wr.Close();
            //（2021-1228,保存文本信息至指定文件夹）；
            #region
            //（add-2021-1229）将文本信息（dislab.Text）保存至Excel表格中；
            //string sContext = displab.Text;// 控件的内容
            //string sExcelPath = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息.xlsx"; //excel路径
            //int iSheet = 1;

            // 连接Excel
            //Application app = new Application();
            //WorkBook WB = app.Workbooks.Open(sExcelPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //WorkSheet WS = (Worksheet)WB.Worksheets[iSheet];
            //Range range = (Range)worksheet.Cells[iRow, iCol]; ////得到指定行列的单元格
            //range.Value2 = sContext; // 对单元格赋值

            //（add-2021-1229）将文本信息（dislab.Text）保存至Excel表格中；





            //向单元格中写数据(2021-1229,循环写入行、列数据，先定义行数，再定义列数);

            ////datetime格式化；
            //DateTime dt1 = DateTime.Now;

            ////在Excel表中写入时间数据时，设置单元格格式，自定义格式yyyy-MM-dd HH:mm:ss
            //string current_time = dt1.ToString("yyyy-MM-dd HH:mm:ss.fff");   //mm-dd才显示08-01，否则显示8-1
            ////fff个数表示小数点后显示的位数，这里精确到小数点后3位


            //(add-2021-1228,保存处理后的图像至指定文件夹)
            //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
            //(add-2021-1228,保存处理后的图像至指定文件夹)
            #endregion

            /*********手动模式下，测试单张图像--3(2022-0111--end)************/
            /************************************************************/
            /******获取匹配区域并打印匹配信息--3(2022-0111--end)*************************/

        }

        private void MatchDifference_Click(object sender, EventArgs e)
        {
            /*********手动模式下，测试单张图像--4(2022-0111--start)************/
            /************************************************************/
            /******模板与匹配区域差运算--4(2022-0111--start)*************************/

            // 模板与匹配区域差运算方法;
            #region 
            /*模板与匹配区域差运算；        
            Sqdiff = 0(平方差匹配，最好匹配为0)；
            SqdiffNormed = 1(归一化平方差匹配，最好结果为0)；
            Ccorr = 2(相关匹配法，数值越大效果越好）；
            CcorrNormed = 3(归一化相关匹配法，数值越大效果越好）；
            Coeff = 4(系数匹配法，数值越大效果越好）；
            CoeffNormed = 5(归一化系数匹配法，数值越大效果越好）;*/
            #endregion

            //(add-2021-1228,保存处理后的图像至指定文件夹)
            dbf_File = OpenFileDialog.FileName;
            //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

            string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
            //(add-2021-1228,保存处理后的图像至指定文件夹)



            Mat result = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
            CvInvoke.MatchTemplate(match_img, temp, result, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；

            //CvInvoke.Normalize(result, result, 255, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
            double max = 0, min = 0;//创建double极值；
            Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
            CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

            //打印labbel信息；
            #region
            /*displab.Text =
                "匹配信息: max value" + "\n" +
                max.ToString() + "\n" +
                "min value " + min.ToString() + "\n" +
                "min_loc value " + min_loc.ToString() + "\n" +
                "max_loc value " + max_loc.ToString();*/
            #endregion

            match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图img复制到img2中，显示匹配区域；
            //img3 = temp.Sub(img2);//模板图像temp-匹配区域imge2，进行差运算；  （取绝对值差值！）  
            //add(1220)
            //value = temp.Sub(img2);
            //img3 = new Image<Bgr, byte>();
            //Mat img3 = new Mat();//创建img3，存储处理后的图像；
            //CudaInvoke.Abs(temp,img3,null);
            //add(1220)
            match_area_img_1 = match_area_img.AbsDiff(temp);//(1220-add,temp\img2绝对值差值)；
            match_area_img_out = match_area_img_1.Convert<Gray, Byte>();//（将match_area_img_1转化为灰度图）;(image定义的图像通过Convert转化为灰度图);
            pictureBox4.Image = match_area_img_out.ToBitmap();//显示模板图像与匹配区域的差运算结果；

            //(2021-1230)
            binary_img = match_area_img_out;//将模板图像temp-匹配区域imge2的差运算结果（match_area_img_out）拷贝到binary_img;
            //(2021-1230)

            //(add-2021-1228,保存处理后的图像至指定文件夹)
            //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
            //(add-2021-1228,保存处理后的图像至指定文件夹)


            //显示、保存图像；
            #region
            ////CvInvoke.Imshow("img", temp); //显示图片
            CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_match_area_img_out.bmp", match_area_img_out); //保存匹配结果图像；
            CvInvoke.WaitKey(0); //暂停按键等待

            //pictureBox4.Image = result.ToImage<Gray, byte>().ToBitmap();//显示匹配结果，若 pictureBox3.Image = result显示为全黑，必须将类型转换为Byte；
            #endregion

            /*********手动模式下，测试单张图像--4(2022-0111--end)************/
            /************************************************************/
            /******模板与匹配区域差运算--4(2022-0111--end)*************************/
        }


        private void MatchingBinarization_Click(object sender, EventArgs e)
        {
            /*********手动模式下，测试单张图像--5(2022-0111--start)************/
            /************************************************************/
            /******模板与匹配区域差二值化运算--5(2022-0111--start)*************************/


            //模板与匹配区域差二值化结果；            
            #region
            //Mat result = new Mat(new Size(img.Width - temp.Width + 1, img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
            //CvInvoke.MatchTemplate(img, temp, result, Emgu.CV.CvEnum.TemplateMatchingType.Ccoeff);//采用系数匹配法，数值越大越接近准确图像；

            //CvInvoke.Normalize(result, result, 255, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
            //double max = 0, min = 0;//创建double极值；
            //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
            //CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

            //img2 = img.Copy(new Rectangle(max_loc, temp.Size)); //将原图img复制到img2中，显示匹配区域；
            //img3 = temp.Sub(img2);//模板图像temp-匹配区域imge2，进行差运算；           
            //pictureBox4.Image = img3.ToBitmap();//显示模板图像与匹配区域的差运算结果；
            #endregion
            //string转换到int类型；
            #region
            //try

            //{
            //    int count1 = Convert.ToInt32(textBox1.Text); //string类型转换int类型(count1为待装换的数值)；
            //}
            //catch (System.Exception ec)
            //{
            //    MessageBox.Show("格式错误!");
            //}
            #endregion

            //(2021-1230,error:OpenCV: !src.empty())
            //binary_img = match_area_img_out;//将模板图像temp-匹配区域imge2的差运算结果（match_area_img_out）拷贝到binary_img;
            //(2021-1230,error:OpenCV: !src.empty())


            //(add-2021-1228,保存处理后的图像至指定文件夹)
            dbf_File = OpenFileDialog.FileName;
            //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

            string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                                                                                     //(add-2021-1228,保存处理后的图像至指定文件夹)

            //二值化
            #region
            //Mat binary_img = new Mat("D:\\VS-Code\\Demo\\Emgu.CV.CvEnum\\MatchDifference.bmp");//指定图像；

            //match_area_img_1 = match_area_img.AbsDiff(temp);//(1220-add,|temp-img2|绝对值差值)；

            //Image<Bgr, byte> binary = new Image <Bgr, byte>;

            //阈值20(work)\(自适应调整阈值)Binary类型；
            #endregion

            //自适应阈值
            #region
            //CvInvoke.Threshold(binary_img, binary, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

            //region
            //int num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;                                           
            //CvInvoke.Threshold(binary_img, binary, num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            #endregion
            //try
            //{
            //binary_img = img3.Copy();//将模板图像temp-匹配区域imge2的差运算结果拷贝到binary_img;                
            //int count1 = Convert.ToInt32(textBox1.Text);  //string类型转换int类型
            int pixel_num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;

            //add(2021-1227,num>255,num=255;num<0,num=0);
            if (pixel_num > 255)
            {
                pixel_num = 255;
            }
            if (pixel_num <= 0)
            {
                pixel_num = 0;
            }
            //add(2021-1227,num>255,num=255;num<0,num=0);

            CvInvoke.Threshold(binary_img, binary, pixel_num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);//num自适应调节;

            pictureBox5.Image = binary.ToImage<Gray, byte>().ToBitmap();
            corrosion_img = binary;



            //(add-2021-1228,保存处理后的图像至指定文件夹)
            //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
            //(add-2021-1228,保存处理后的图像至指定文件夹)


            //显示、保存图像；
            #region
            ////CvInvoke.Imshow("img", temp); //显示图片
            CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_binary.bmp", binary); //保存匹配结果图像；
            CvInvoke.WaitKey(0); //暂停按键等待

            /*********手动模式下，测试单张图像--5(2022-0111--end)************/
            /************************************************************/
            /******模板与匹配区域差二值化运算--5(2022-0111--end)*************************/

            //}
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show("  格式错误，请输入整型！");

            //}
            //保存结果；
            #region          
            //CvInvoke.Imwrite("D:\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Binary_Thres-20-Gray.bmp", binary.ToImage<Gray, byte>()); //（保存结果图片(灰度图),dst彩色图）；
            ////CvInvoke.Imwrite("D:\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Binary_Thres-20-RGB.bmp", binary); //保存结果图片(彩色图)；
            //CvInvoke.WaitKey(0); //暂停按键等待
            #endregion
            //腐蚀
            #region
            //腐蚀
            //corrosion_img = binary_img.Copy();// 将二值化结果binary_img复制到corrosion_img；            
            //Mat corrosion_img = binary;
            //Mat corrosion = new Mat();//创建Mat，存储处理后的图像；
            //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3),new Point(-1,-1));//指定参数获得结构元素；
            //CvInvoke.Erode(corrosion_img, corrosion, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行腐蚀操作；
            //pictureBox5.Image = corrosion.ToImage<Bgr, byte>().ToBitmap();
            #endregion
        }
        //add(1213,switch)
        #region
        //private int count = 0;
        //add(1213)
        #endregion


        //add(2021-1228，自适应手动调整腐蚀次数，默认腐蚀次数为1)；
        private void erode_Click(object sender, EventArgs e)
        {
            /*********手动模式下，测试单张图像--6(2022-0111--start)************/
            /************************************************************/
            /******模板与匹配区域差二值化结果腐蚀运算--6(2022-0111--start)*************************/


            //腐蚀;Tab (代码整体左移)或 Shift+Tab(代码整体右移) ；
            #region
            //corrosion_img = binary_img.Copy();// 将二值化结果binary_img复制到corrosion_img;           

            //try
            //{
            //    //binary_img = img3.Copy();//将模板图像temp-匹配区域imge2的差运算结果拷贝到binary_img;                
            //    //int count1 = Convert.ToInt32(textBox1.Text); ; //string类型转换int类型
            //    int num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;
            //    CvInvoke.Threshold(binary_img, binary, num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);//num自适应调节;
            //    //pictureBox5.Image = binary.ToImage<Gray, byte>().ToBitmap();
            //}
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show("  格式错误，请输入整型！");

            //}     
            #endregion
            //switch (count % 2)
            //{
            //腐蚀（单击一次腐蚀按钮即腐蚀一次）；
            //case 0:

            //(add-2021-1228,保存处理后的图像至指定文件夹)
            dbf_File = OpenFileDialog.FileName;
            //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

            string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
            //(add-2021-1228,保存处理后的图像至指定文件夹)


            Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //指定参数进行形态学开（先腐蚀后膨胀）操作；
            //CvInvoke.MorphologyEx(corrosion_img,corrosion1,Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

            //new Point(-1, -1),表示结构元素中心，3，表示腐蚀迭代次数；

            //add(2021-1228，自适应手动调整腐蚀次数，默认腐蚀次数为1)；


            //(add-2021-1228,保存处理后的图像至指定文件夹)
            //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
            //(add-2021-1228,保存处理后的图像至指定文件夹)


            int erode_num = Convert.ToInt32(textBox2.Text);//实现string类型到int类型转换（默认腐蚀次数erode_num=1，点击一次腐蚀一次）；

            CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), erode_num, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作（自适应手动调整输入腐蚀次数）；

            //CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作（2：腐蚀2次）；

            pictureBox5.Image = corrosion1.ToImage<Gray, byte>().ToBitmap();
            corrosion_img = corrosion1;//（将第一次腐蚀的结果作为第二次腐蚀的输入，如此循环，一直腐蚀，直至消失）;

            dilate_img = corrosion1;//（将腐蚀结果corrosion1复制给dilate_img，作为膨胀的输入）;
                                    //break;

            //显示、保存图像；
            #region
            //CvInvoke.Imshow("img", temp); //显示图片
            CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_corrosion1.bmp", corrosion1); //保存匹配结果图像；
            CvInvoke.WaitKey(0); //暂停按键等待
            #endregion

            /*********手动模式下，测试单张图像--6(2022-0111--end)************/
            /************************************************************/
            /******模板与匹配区域差二值化结果腐蚀运算--6(2022-0111--end)*************************/


            //add(2021-1228，自适应手动调整腐蚀次数，默认腐蚀次数为1)；
            //二次腐蚀；
            #region
            //case 1:
            //Mat img7 = corrosion1;
            //        Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
            //        Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //        CvInvoke.Erode(img7, corrosion2, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作；
            //        //pictureBox5.Image.Dispose();
            //        pictureBox5.Image = corrosion2.ToImage<Gray, byte>().ToBitmap();
            //      break;
            //}
            //count++;
            //add(1213)
            #endregion

            //二次腐蚀；
            #region
            //Mat img7 = corrosion1;
            //Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
            //Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //CvInvoke.Erode(img7, corrosion2, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作；
            //pictureBox5.Image = corrosion2.ToImage<Bgr, byte>().ToBitmap();
            #endregion

            ////膨胀
            #region
            //Mat corrosion_img = binary;
            //Mat swell = new Mat();
            //Mat corrosion1 = new Mat();//创建Mat，存储处理后的图像；
            //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行腐蚀操作；

            //CvInvoke.Dilate(corrosion_img, swell, struct_element, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(128, 128, 128));

            //pictureBox5.Image = corrosion1.ToImage<Bgr, byte>().ToBitmap();

            //pictureBox5.Image = swell.ToImage<Bgr, byte>().ToBitmap();
            #endregion

        }

        private void dilate_Click(object sender, EventArgs e)
        {

            /*********手动模式下，测试单张图像--7(2022-0111--start)************/
            /************************************************************/
            /******模板与匹配区域差二值化结果膨胀运算--7(2022-0111--start)*************************/


            //膨胀（work,单击一次膨胀按钮即膨胀一次）；
            #region
            //corrosion_img = binary_img.Copy();// 将二值化结果binary_img复制到corrosion_img；            

            //try
            //{
            //    //binary_img = img3.Copy();//将模板图像temp-匹配区域imge2的差运算结果拷贝到binary_img;                
            //    //int count1 = Convert.ToInt32(textBox1.Text); ; //string类型转换int类型
            //    int num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;
            //    CvInvoke.Threshold(binary_img, binary, num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);//num自适应调节;
            //    //pictureBox5.Image = binary.ToImage<Gray, byte>().ToBitmap();
            //}
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show("  格式错误，请输入整型！");

            //}

            //add(1213)
            //add(1213)
            //Mat corrosion1 = new Mat();//创建Mat，存储处理后的图像；

            //switch (counti)
            //{

            //腐蚀；
            //case 0:
            //Mat corrosion_img = binary;
            //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            ////指定参数进行形态学开（先腐蚀后膨胀）操作；
            ////CvInvoke.MorphologyEx(corrosion_img,corrosion1,Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));
            //CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作；
            //pictureBox5.Image = corrosion1.ToImage<Gray, byte>().ToBitmap();
            //break;

            //二次腐蚀；               
            //Mat img7 = corrosion1;
            //Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
            //Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //CvInvoke.Erode(img7, corrosion2, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作；
            //pictureBox5.Image.Dispose();
            //pictureBox5.Image = corrosion2.ToImage<Gray, byte>().ToBitmap();

            //switch (counti % 2)
            //{
            // 膨胀；
            //case 0:
            #endregion

            //add(2021-1228，自适应手动调整膨胀次数，默认膨胀次数为1)；

            //形态学操作类型（add-2021-1228）
            #region
            //Erode = 0;(形态学腐蚀，消除边界点，使边界向内收缩，可消除没有意义的物体。使用CvInvoke.Erode)；
            //Dilate =1;(形态学膨胀，将与物体接触的所有背景点合并到该物体中，使边界向外扩张，可用来填补物体中的空洞。使用 CvInvoke.Dilate)；
            //open = 2;(形态学开操作，先腐蚀，后膨胀。即：OPEN(X)=D(E(X))，（使用CvInvoke.MorphologyEx（Emgu.CV.CvEnum.MorphOp.Open）作用：消除小物体，在纤细点处分离物体，位置和形状总是不变)；
            //Close = 3;(形态学闭操作，先膨胀，后腐蚀。即：CLOSE(X)=E(D(X)),（使用CvInvoke.MorphologyEx（Emgu.CV.CvEnum.MorphOp.Close））作用：可填平小孔，位置、形状不变。)
            //Gradient = 4;(形态学梯度操作，膨胀与腐蚀差。即：Grad(X)=Dilate(X)-Erode(X)。膨胀：扩大图像的边界，腐蚀：缩小图像的边界，膨胀-腐蚀：即为形态学梯度操作，可保存边缘轮廓)；
            //Tophat = 5;(形态学高帽，原始图像-开操作图像，即：Tophat(X)=X-Open（X）)；
            //Blackhat = 6;(形态学地帽，闭操作图像-原始图像，即：Blackhat(X)=Close(X)-X,结果是：输出图像大部分面积均为偏黑色，故称黑帽操作);
            //形态学操作类型（add-2021-1228）
            #endregion
            //(add-2021-1228,保存处理后的图像至指定文件夹)
            dbf_File = OpenFileDialog.FileName;
            //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

            string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
            //(add-2021-1228,保存处理后的图像至指定文件夹)

            //(add-2021-1228,保存处理后的图像至指定文件夹)
            //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
            //(add-2021-1228,保存处理后的图像至指定文件夹)

            Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //指定参数进行形态学开操作； //new Point(-1, -1),表示结构元素中心，3，表示形态学开操作中腐蚀迭代次数；
            //CvInvoke.MorphologyEx(corrosion_img, corrosion1, Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

            int dilate_num = Convert.ToInt32(textBox3.Text);//实现string类型到int类型转换（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；

            CvInvoke.Dilate(dilate_img, swell, struct_element, new Point(-1, -1), dilate_num, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；

            //CvInvoke.Dilate(dilate_img, swell, struct_element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作（2：膨胀2次）；
            pictureBox5.Image = swell.ToImage<Gray, byte>().ToBitmap();

            dilate_img = swell; //（将第一次膨胀的结果作为第二次膨胀的输入，如此循环，一直膨胀）;

            //显示、保存图像；
            #region
            //CvInvoke.Imshow("img", temp); //显示图片
            CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_swell.bmp", swell); //保存匹配结果图像；
            CvInvoke.WaitKey(0); //暂停按键等待
            #endregion


            /*********手动模式下，测试单张图像--7(2022-0111--end)************/
            /************************************************************/
            /******模板与匹配区域差二值化结果膨胀运算--7(2022-0111--end)*************************/


            //二次膨胀；
            #region
            //break;
            //case 1:
            //Mat img9 = corrosion2;
            //Mat swell2 = new Mat();//创建Mat，存储处理后的图像；
            //Mat struct_element3 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            ////指定参数进行形态学开操作；
            ////CvInvoke.MorphologyEx(img7, corrosion2, Emgu.CV.CvEnum.MorphOp.Open, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

            //CvInvoke.Dilate(img9, swell2, struct_element3, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作；
            //pictureBox5.Image = swell2.ToImage<Gray, byte>().ToBitmap();
            //break;

            //Mat corrosion_img = binary;
            //Mat corrosion1 = new Mat();//创建Mat，存储处理后的图像；
            //Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //CvInvoke.Erode(corrosion_img, corrosion1, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行腐蚀操作；
            //pictureBox5.Image = corrosion1.ToImage<Bgr, byte>().ToBitmap();

            //Mat img7 = corrosion1;
            //Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
            //Mat struct_element2 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //CvInvoke.Erode(img7, corrosion2, struct_element2, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行腐蚀操作；
            //pictureBox5.Image = corrosion2.ToImage<Bgr, byte>().ToBitmap();

            //}
            //counti++;
            #endregion
        }
        //add(2021-1228，自适应手动调整膨胀次数，默认膨胀次数为1)；

        //private int countj = 0;      


        private void BinarizedContour_Click(object sender, EventArgs e)
        {


            /*********手动模式下，测试单张图像--8(2022-0111--start)************/
            /************************************************************/
            /******模板与匹配区域差二值化轮廓阈值运算--8************************/
            /*****将两次文本信息打印拼接在一起，使用stream将信息写入Excel表中--8(2022-0111--start)*************************/


            //模板与匹配区域差二值化后的轮廓结果；
            #region
            //contour_img = binary_img.Copy();  // 将二值化结果binary_img复制到contour_img； 
            //开运算2（先腐蚀后膨胀）；
            //corrosion_img = binary_img.Copy();// 将二值化结果binary_img复制到corrosion_img；                       
            //try
            //{
            //    //binary_img = img3.Copy();//将模板图像temp-匹配区域imge2的差运算结果拷贝到binary_img;                
            //    //int count1 = Convert.ToInt32(textBox1.Text); ; //string类型转换int类型
            //    int num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;
            //    CvInvoke.Threshold(binary_img, binary, num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);//num自适应调节;
            //    //pictureBox5.Image = binary.ToImage<Gray, byte>().ToBitmap();
            //}
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show("  格式错误，请输入整型！");

            //}           

            //switch (countj % 2)
            //{
            //一次腐蚀；
            //case 0:
            //Mat corrosion_img = binary;
            //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作；
            //指定参数进行形态学开（先腐蚀后膨胀）操作；
            //CvInvoke.MorphologyEx(corrosion_img,corrosion1,Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));
            //pictureBox5.Image = corrosion1.ToImage<Gray, byte>().ToBitmap();
            //break;

            // 膨胀1；
            //Mat img7 = corrosion1;
            //Mat swell1 = new Mat();//创建Mat，存储处理后的图像；
            //Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //指定参数进行形态学开操作；
            //CvInvoke.MorphologyEx(corrosion_img, corrosion1, Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

            //CvInvoke.Dilate(img7, swell1, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行腐蚀操作；
            //pictureBox5.Image = swell1.ToImage<Gray, byte>().ToBitmap();
            #endregion

            // canny算子；
            #region
            //Mat corrosion_img = binary;
            //Mat corrosion1 = new Mat();//创建Mat，存储处理后的图像；
            //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            ////指定参数进行形态学开操作；
            //CvInvoke.MorphologyEx(corrosion_img, corrosion1, Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

            ////开运算2（先腐蚀后膨胀）；
            //Mat img7 = corrosion1;
            //Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
            //Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            ////指定参数进行形态学开操作；
            //CvInvoke.MorphologyEx(img7, corrosion2, Emgu.CV.CvEnum.MorphOp.Open, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));
            #endregion

            //(add-2021-1228,保存处理后的图像至指定文件夹)
            dbf_File = OpenFileDialog.FileName;
            //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

            string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
            //(add-2021-1228,保存处理后的图像至指定文件夹)

            //(add-2021-1228,显示图像名称)
            //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
            //(add-2021-1228,显示图像名称)

            contour_img = swell;

            CvInvoke.Canny(contour_img, canny_img, 120, 180); //指定阈值（第一阈值：120，第二阈值：180）进行Canny算子处理(这里是否需要Canny算子？？)；                       

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储查找到的轮廓；
            //add(1215，用于面积筛选)
            VectorOfVectorOfPoint use_contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储筛选后的轮廓；
            //add(1215，用于面积筛选)

            //(RetrType.External:提取最外层轮廓；RetrType.List:提取所有轮廓;Emgu.CV.CvEnum.RetrType.Ccomp检索所有轮廓，并将它们组织成两级层级结构：水平是组件的外部边界，二级约束边界的洞）；
            CvInvoke.FindContours(canny_img, contours, null, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);//查找dst所有轮廓并保存（contour_img<-->canny_img）；                                                            

            //add(2021-1217，面积筛选)
            //遍历完所有面积，打印总面积；
            double sum_area = 0;
            int ksize = contours.Size;//获取连通区域的个数；
            for (int i = 0; i < ksize; i++)//遍历每个连通轮廓；
            {
                VectorOfPoint contour = contours[i];//获取独立的连通轮廓；
                double area = CvInvoke.ContourArea(contour);//计算连通轮廓的面积；

                double area1 = Convert.ToDouble(textBox4.Text);//实现string类型到double类型的转换;
                //add(2021-1227,当输入面积小于0时，area==0);
                if (area1 <= 0)
                {
                    area1 = 0;
                }
                //add(2021-1227,当输入面积小于0时，num1==0);

                //add(2021-1227，当轮廓面积大于阈值面积时，才执行下列语句，实现面积相加，否则跳出条件语句）；
                if (area > area1)//对每一轮廓区域面积进行面积筛选；                                
                {

                    use_contours.Push(contour);//添加筛选后的连通轮廓；
                    sum_area += area;//遍历结果相加；

                }

            }
            //打印轮廓总面积信息；
            displab1.Text = "轮廓总面积: " + sum_area.ToString() + ";" + "\n" + "\n" + "\n";//打印遍历完的总面积（0044轮廓面积,当阈值选择30时，显示出错,试试RetrType.Ccomp）


            //打印匹配信息（2021-1228,保存文本信息至指定文件夹）；
            #region
            //displab.Text = dbf_File2 + "\n" +
            //                 "轮廓总面积:  " + sum_area.ToString();

            ////（2021-1228,保存文本信息至指定文件夹）；
            //string txt = displab1.Text;
            //string filename = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息1.txt";   //文件名，可以带路径

            //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
            //sw.Write(displab1.Text);
            //sw.Close();
            #endregion
            //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；
            string txt = displab1.Text;
            string filename = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息.txt";   //文件名，可以带路径

            FileStream fs = new FileStream(filename, FileMode.Append);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);

            //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
            wr.Write(displab1.Text);
            wr.Close();
            //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；

            //(add-2021-1231)用流创建或读取.xlsx文件（同时流关联了文件）

            filestream = new FileStream("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息.xlsx", FileMode.OpenOrCreate);
            //(add-2021-1231)用流创建或读取.xlsx文件（同时流关联了文件）


            //创建表和sheet;
            if (indexOfExcel_j == 1)
            {
                wk = new XSSFWorkbook();   //创建表对象wk
                isheet = wk.CreateSheet("Sheet1");   //在wk中创建sheet1
                //创建表头
                IRow rowtitle = null;
                rowtitle = isheet.CreateRow(0); //创建index=j的行

                ICell cellTitle0 = null;
                cellTitle0 = rowtitle.CreateCell(0);  //在index=j的行中创建index=0的单元格
                cellTitle0.SetCellValue("时间"); //给创建的单元格赋值string

                ICell cellTitle1 = null;
                cellTitle1 = rowtitle.CreateCell(1);  //在index=j的行中创建index=0的单元格
                cellTitle1.SetCellValue("图像名称"); //给创建的单元格赋值string

                ICell cellTitle2 = null;
                cellTitle2 = rowtitle.CreateCell(2);  //在index=j的行中创建index=0的单元格
                cellTitle2.SetCellValue("匹配信息坐标X"); //给创建的单元格赋值string

                ICell cellTitle3 = null;
                cellTitle3 = rowtitle.CreateCell(3);  //在index=j的行中创建index=0的单元格
                cellTitle3.SetCellValue("匹配信息坐标Y"); //给创建的单元格赋值string

                ICell cellTitle4 = null;
                cellTitle4 = rowtitle.CreateCell(4);  //在index=j的行中创建index=0的单元格
                cellTitle4.SetCellValue("最大相似度"); //给创建的单元格赋值string


                ICell cellTitle5 = null;
                cellTitle5 = rowtitle.CreateCell(5);  //在index=j的行中创建index=0的单元格
                cellTitle5.SetCellValue("最小相似度"); //给创建的单元格赋值string

                ICell cellTitle6 = null;
                cellTitle6 = rowtitle.CreateCell(6);  //在index=j的行中创建index=0的单元格
                cellTitle6.SetCellValue("轮廓总面积"); //给创建的单元格赋值strin
            }


            //向单元格中写数据(2021-1231,循环写入行、列数据，先定义行数，再定义列数，这里是2行6列);
            //for (int indexOfExcel_j = 1; indexOfExcel_j < 3; indexOfExcel_j++)//(j:行)；
            //{

            //IRow row2 = null;
            //row2 = isheet.CreateRow(indexOfExcel_j++); //创建index=j的行

            ////datetime格式化；
            DateTime dt = DateTime.Now;

            rowWrite = isheet.CreateRow(indexOfExcel_j++); //创建index=j的行

            ICell cell0 = null;
            cell0 = rowWrite.CreateCell(0);  //在index=j的行中创建index=0的单元格
            cell0.SetCellValue(dt.ToLocalTime().ToString()); //给创建的单元格赋值string

            ICell cell1 = null;
            cell1 = rowWrite.CreateCell(1);  //在index=j的行中创建index=0的单元格
            cell1.SetCellValue(dbf_File2); //给创建的单元格赋值string

            ICell cell2 = null;
            cell2 = rowWrite.CreateCell(2);  //在index=j的行中创建index=0的单元格
            cell2.SetCellValue("X:" + max_loc.X); //给创建的单元格赋值string

            ICell cell3 = null;
            cell3 = rowWrite.CreateCell(3);  //在index=j的行中创建index=0的单元格
            cell3.SetCellValue("Y:" + max_loc.Y); //给创建的单元格赋值string

            ICell cell4 = null;
            cell4 = rowWrite.CreateCell(4);  //在index=j的行中创建index=0的单元格
            cell4.SetCellValue(max.ToString("f2")); //给创建的单元格赋值string


            ICell cell5 = null;
            cell5 = rowWrite.CreateCell(5);  //在index=j的行中创建index=0的单元格
            cell5.SetCellValue(min.ToString("f2")); //给创建的单元格赋值string

            ICell cell6 = null;
            cell6 = rowWrite.CreateCell(6);  //在index=j的行中创建index=0的单元格
            cell6.SetCellValue(sum_area.ToString()); //给创建的单元格赋值string


            //通过流将表中写入的数据一次性写入文件中;
            wk.Write(filestream); //通过流filestream将表wk写入文件


            //(add,20211231)
            //filestream.Close(); //关闭文件流filestream
            //wk.Close(); //关闭Excel表对象wk


            Mat mask = contour_img.ToImage<Bgr, byte>().CopyBlank().Mat;//获取一张背景为黑色的图像，尺寸大小与contour_img一样(类型Bgr);
            CvInvoke.DrawContours(mask, use_contours, -1, new MCvScalar(0, 0, 255));//采用红色画笔在mask掩膜图像中画出所有轮廓；(MCvScalar是一个具有单元素到四元素间的一个数组)
            pictureBox6.Image = mask.ToImage<Bgr, byte>().ToBitmap();//显示轮廓区域图像；


            //显示、保存图像；
            #region
            //CvInvoke.Imshow("img", temp); //显示图片
            CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_mask.bmp", mask); //保存匹配结果图像；
            CvInvoke.WaitKey(0); //暂停按键等待
            #endregion

            /*********手动模式下，测试单张图像--8(2022-0111--end)************/
            /************************************************************/
            /******模板与匹配区域差二值化轮廓阈值运算--8************************/
            /*****将两次文本信息打印拼接在一起，使用stream将信息写入Excel表中--8(2022-0111--end)*************************/
        
        }



        private void LoadTemplate1_Click(object sender, EventArgs e)
        {
            /***********************自动模式下---1-Start；************************************/
            /***********************自动模式下---1-加载模板图像；************************************/
            /*****************(add,(2022-0111))**************************************/
            /******************(LoadTemplate_Click)**********************/


            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "JPEG;BMP;PNG;JPG;GIF|*.JPEG;*.BMP;*.PNG;*.JPG;*.GIF|ALL|*.*";//（模板和加载图像尺寸不一致时，会报错）;
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                //(add-2021-1228,保存处理后的图像至指定文件夹)
                dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                try
                {
                    temp = new Image<Bgr, Byte>(OpenFileDialog.FileName);
                    pictureBox1.Image = temp.ToBitmap();

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("图像格式错误！");
                }

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                //显示、保存图像；
                #region
                ////CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite(@"D:\SKQ\VS-Code\Demo\Emgu.CV.CvEnum\Emgu.CV.CvEnum\bin\Debug\test\temp_image_result\" + dbf_File2 + "_temp.bmp", temp); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                #endregion

                /***********************自动模式下---1-end；************************************/
                /***********************自动模式下---1-加载模板图像；************************************/
                /*****************(add,(2022-0111))**************************************/
                /******************(LoadTemplate_Click)**********************/
            }
        }

        private void Matchimage1_Click(object sender, EventArgs e)
        {
            //string dbf_File = string.Empty;

            /****************自动模式下---2-加载待匹配图像并画出矩形框--start；*********************/
            /**********************************（add,2022-0111);***********************/
            /**************************（Matchimage_Click）*****************/


            //OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "JPEG;BMP;PNG;JPG;GIF|*.JPEG;*.BMP;*.PNG;*.JPG;*.GIF|ALL|*.*";//（模板和加载图像尺寸不一致时，会报错）;

            OpenFileDialog.Multiselect = true;//(2021-1231)该值确定是否可以选择多个文件;

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {

                string[] files = OpenFileDialog.FileNames;//(2021-1231)该值确定是否可以选择多个文件;

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                try
                {
                    match_img = new Image<Bgr, Byte>(OpenFileDialog.FileName);
                    Mat result1 = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                    CvInvoke.MatchTemplate(match_img, temp, result1, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；

                    //CvInvoke.Normalize(result1, result1, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                    //double max = 0, min1 = 0;//创建double极值；
                    //Point max_loc = new Point(0, 0), min_loc1 = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
                    CvInvoke.MinMaxLoc(result1, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                    match_img1 = match_img.Copy(); //将原图match_img复制到match_img1中，对match_img1进行画矩形框，避免pictureBox3显示匹配区域出现边框；
                    CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
                    pictureBox2.Image = match_img1.ToBitmap();//显示找到模板图像的待搜索图像；

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("图像格式错误！");
                }

                //显示、保存图像；
                #region
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_match_img1.bmp", match_img1); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                #endregion

                /****************自动模式下---2-加载待匹配图像并画出矩形框--end；*********************/
                /**********************************（add,2022-0111);***********************/
                /**************************（Matchimage_Click）*****************/



                /********************自动模式下---3-获取匹配区域--start************************/
                /******************* (add,2022-0111,获取匹配区域并打印匹配信息*********/
                /*************相当于(MatchingArea_Click));*******/


                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)


                // 获取匹配区域；
                Mat result = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                CvInvoke.MatchTemplate(match_img, temp, result, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);//采用系数匹配法(CcorrNormed)，打印出匹配相似度信息,数值越大越接近准确图像；
                //CvInvoke.Normalize(result, result, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                //double max = 0, min = 0;//创建double极值；
                //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
                CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                //string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图match_img复制到match_area_img中，显示匹配区域；
                pictureBox3.Image = match_area_img.ToBitmap();//显示匹配区域；

                //显示、保存图像；
                #region
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_match_area_img.bmp", match_area_img); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                #endregion



                //////datetime格式化；
                //DateTime dt = DateTime.Now;

                ////在Excel表中写入时间数据时，设置单元格格式，自定义格式yyyy-MM-dd HH:mm:ss
                //(label as TextBox).Text = dt.ToLocalTime().ToString();   //mm-dd才显示08-01，否则显示8-1
                ////fff个数表示小数点后显示的位数，这里精确到小数点后3位

                ////datetime格式化；
                DateTime dt = DateTime.Now;

                //打印匹配信息（2021-1228,保存文本信息至指定文件夹）；
                displab2.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" + "图像名称：" + dbf_File2 + "\n" +
                                "\n" + "匹配信息: X= " +
                                 max_loc.X + "," + " Y= " + max_loc.Y + ";" +
                                "\n" +
                                "最大相似度: " + max.ToString("f2") + ";" + "\n" +
                                "最小相似度: " + min.ToString("f2") + ";" + "\n";

                //（2021-1228,保存文本信息至指定文件夹）；
                string txt = displab2.Text;
                string filename = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息.txt";   //文件名，可以带路径

                FileStream fs = new FileStream(filename, FileMode.Append);
                StreamWriter wr = null;
                wr = new StreamWriter(fs);

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                wr.Write(displab2.Text);
                wr.Close();
                //（2021-1228,保存文本信息至指定文件夹）；
                #region
                //（add-2021-1229）将文本信息（dislab.Text）保存至Excel表格中；
                //string sContext = displab.Text;// 控件的内容
                //string sExcelPath = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息.xlsx"; //excel路径
                //int iSheet = 1;

                // 连接Excel
                //Application app = new Application();
                //WorkBook WB = app.Workbooks.Open(sExcelPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //WorkSheet WS = (Worksheet)WB.Worksheets[iSheet];
                //Range range = (Range)worksheet.Cells[iRow, iCol]; ////得到指定行列的单元格
                //range.Value2 = sContext; // 对单元格赋值

                //（add-2021-1229）将文本信息（dislab.Text）保存至Excel表格中；



                //向单元格中写数据(2021-1229,循环写入行、列数据，先定义行数，再定义列数);

                ////datetime格式化；
                //DateTime dt1 = DateTime.Now;

                ////在Excel表中写入时间数据时，设置单元格格式，自定义格式yyyy-MM-dd HH:mm:ss
                //string current_time = dt1.ToString("yyyy-MM-dd HH:mm:ss.fff");   //mm-dd才显示08-01，否则显示8-1
                ////fff个数表示小数点后显示的位数，这里精确到小数点后3位


                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)
                #endregion
                /********************自动模式下---3-获取匹配区域--end************************/
                /******************* (add,2022-0111,获取匹配区域*********/
                /*************相当于(MatchingArea_Click));*******/



                /****************************自动模式下---4-进行差运算--start**********************/
                /***************************(add,2022-0111,进行差运算********/
                /*************相当于(MatchDifference_Click));********/

                // 模板与匹配区域差运算方法;
                #region 
                /*模板与匹配区域差运算；        
                Sqdiff = 0(平方差匹配，最好匹配为0)；
                SqdiffNormed = 1(归一化平方差匹配，最好结果为0)；
                Ccorr = 2(相关匹配法，数值越大效果越好）；
                CcorrNormed = 3(归一化相关匹配法，数值越大效果越好）；
                Coeff = 4(系数匹配法，数值越大效果越好）；
                CoeffNormed = 5(归一化系数匹配法，数值越大效果越好）;*/
                #endregion

                //(add-2021-1228,保存处理后的图像至指定文件夹)

                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);


                Mat result2 = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                CvInvoke.MatchTemplate(match_img, temp, result2, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；

                //CvInvoke.Normalize(result2, result2, 255, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                //double max = 0, min = 0;//创建double极值；
                //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
                CvInvoke.MinMaxLoc(result2, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                //打印labbel信息；
                #region
                /*displab.Text =
                    "匹配信息: max value" + "\n" +
                    max.ToString() + "\n" +
                    "min value " + min.ToString() + "\n" +
                    "min_loc value " + min_loc.ToString() + "\n" +
                    "max_loc value " + max_loc.ToString();*/
                #endregion

                match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图img复制到img2中，显示匹配区域；

                match_area_img_1 = match_area_img.AbsDiff(temp);//(1220-add,temp\img2绝对值差值)；
                match_area_img_out = match_area_img_1.Convert<Gray, Byte>();//（将match_area_img_1转化为灰度图）;(image定义的图像通过Convert转化为灰度图);
                pictureBox4.Image = match_area_img_out.ToBitmap();//显示模板图像与匹配区域的差运算结果；

                //(2021-1230)
                binary_img = match_area_img_out;//将模板图像temp-匹配区域imge2的差运算结果（match_area_img_out）拷贝到binary_img;

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)


                //显示、保存图像；
                #region
                ////CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_match_area_img_out.bmp", match_area_img_out); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待

                //pictureBox4.Image = result.ToImage<Gray, byte>().ToBitmap();//显示匹配结果，若 pictureBox3.Image = result显示为全黑，必须将类型转换为Byte；
                #endregion
                /****************************自动模式下---4-进行差运算--end**********************/
                /***************************(add,2022-0111,进行差运算********/
                /*************相当于(MatchDifference_Click));********/



                /********************自动模式下---5-进行二值化运算--start************************/
                /******************* (aadd,2022-0111,二值化）*********/
                /*************相当于(MatchingBinarization_Click));*******/

                //模板与匹配区域差二值化结果；            
                #region
                //Mat result = new Mat(new Size(img.Width - temp.Width + 1, img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                //CvInvoke.MatchTemplate(img, temp, result, Emgu.CV.CvEnum.TemplateMatchingType.Ccoeff);//采用系数匹配法，数值越大越接近准确图像；

                //CvInvoke.Normalize(result, result, 255, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                //double max = 0, min = 0;//创建double极值；
                //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
                //CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                //img2 = img.Copy(new Rectangle(max_loc, temp.Size)); //将原图img复制到img2中，显示匹配区域；
                //img3 = temp.Sub(img2);//模板图像temp-匹配区域imge2，进行差运算；           
                //pictureBox4.Image = img3.ToBitmap();//显示模板图像与匹配区域的差运算结果；
                #endregion
                //string转换到int类型；
                #region
                //try

                //{
                //    int count1 = Convert.ToInt32(textBox1.Text); //string类型转换int类型(count1为待装换的数值)；
                //}
                //catch (System.Exception ec)
                //{
                //    MessageBox.Show("格式错误!");
                //}
                #endregion

                //(2021-1230,error:OpenCV: !src.empty())
                //binary_img = match_area_img_out;//将模板图像temp-匹配区域imge2的差运算结果（match_area_img_out）拷贝到binary_img;
                //(2021-1230,error:OpenCV: !src.empty())


                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                /* string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File);*/ // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                //二值化
                #region
                //Mat binary_img = new Mat("D:\\VS-Code\\Demo\\Emgu.CV.CvEnum\\MatchDifference.bmp");//指定图像；

                //match_area_img_1 = match_area_img.AbsDiff(temp);//(1220-add,|temp-img2|绝对值差值)；

                //Image<Bgr, byte> binary = new Image <Bgr, byte>;

                //阈值20(work)\(自适应调整阈值)Binary类型；
                #endregion

                //自适应阈值
                #region
                //CvInvoke.Threshold(binary_img, binary, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

                //region
                //int num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;                                           
                //CvInvoke.Threshold(binary_img, binary, num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
                #endregion
                //try
                //{
                //binary_img = img3.Copy();//将模板图像temp-匹配区域imge2的差运算结果拷贝到binary_img;                
                //int count1 = Convert.ToInt32(textBox1.Text);  //string类型转换int类型
                int pixel_num = Convert.ToInt32(textBox8.Text);//实现string类型到int类型的转换;

                //add(2021-1227,num>255,num=255;num<0,num=0);
                if (pixel_num > 255)
                {
                    pixel_num = 255;
                }
                if (pixel_num <= 0)
                {
                    pixel_num = 0;
                }
                //add(2021-1227,num>255,num=255;num<0,num=0);

                CvInvoke.Threshold(binary_img, binary, pixel_num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);//num自适应调节;

                pictureBox5.Image = binary.ToImage<Gray, byte>().ToBitmap();
                corrosion_img = binary;



                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)


                //显示、保存图像；
                #region
                ////CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_binary.bmp", binary); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待


                //}
                //catch (System.Exception ex)
                //{
                //    MessageBox.Show("  格式错误，请输入整型！");

                //}
                //保存结果；
                #region          
                //CvInvoke.Imwrite("D:\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Binary_Thres-20-Gray.bmp", binary.ToImage<Gray, byte>()); //（保存结果图片(灰度图),dst彩色图）；
                ////CvInvoke.Imwrite("D:\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Binary_Thres-20-RGB.bmp", binary); //保存结果图片(彩色图)；
                //CvInvoke.WaitKey(0); //暂停按键等待
                #endregion
                //腐蚀
                #region
                //腐蚀
                //corrosion_img = binary_img.Copy();// 将二值化结果binary_img复制到corrosion_img；            
                //Mat corrosion_img = binary;
                //Mat corrosion = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3),new Point(-1,-1));//指定参数获得结构元素；
                //CvInvoke.Erode(corrosion_img, corrosion, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行腐蚀操作；
                //pictureBox5.Image = corrosion.ToImage<Bgr, byte>().ToBitmap();
                #endregion
                /********************自动模式下---5-进行二值化运算--end************************/
                /******************* (add,2022-0111,二值化）********************************/
                /*************相当于(MatchingBinarization_Click));***************************/



                /********************自动模式下---6-形态学腐蚀操作运算--start************************/
                /******************* add,2022-0111,腐蚀）****************************************/
                /*************相当于(erode_Click));*********************************************/



                //腐蚀;Tab (代码整体左移)或 Shift+Tab(代码整体右移) ；
                #region
                //corrosion_img = binary_img.Copy();// 将二值化结果binary_img复制到corrosion_img;           

                //try
                //{
                //    //binary_img = img3.Copy();//将模板图像temp-匹配区域imge2的差运算结果拷贝到binary_img;                
                //    //int count1 = Convert.ToInt32(textBox1.Text); ; //string类型转换int类型
                //    int num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;
                //    CvInvoke.Threshold(binary_img, binary, num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);//num自适应调节;
                //    //pictureBox5.Image = binary.ToImage<Gray, byte>().ToBitmap();
                //}
                //catch (System.Exception ex)
                //{
                //    MessageBox.Show("  格式错误，请输入整型！");

                //}     
                #endregion
                //switch (count % 2)
                //{
                //腐蚀（单击一次腐蚀按钮即腐蚀一次）；
                //case 0:

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                /*string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); */// for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)


                Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //指定参数进行形态学开（先腐蚀后膨胀）操作；
                #region
                //CvInvoke.MorphologyEx(corrosion_img,corrosion1,Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

                //new Point(-1, -1),表示结构元素中心，3，表示腐蚀迭代次数；

                //add(2021-1228，自适应手动调整腐蚀次数，默认腐蚀次数为1)；
                #endregion

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)


                int erode_num = Convert.ToInt32(textBox6.Text);//实现string类型到int类型转换（默认腐蚀次数erode_num=1，点击一次腐蚀一次）；

                CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), erode_num, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作（自适应手动调整输入腐蚀次数）；

                //CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作（2：腐蚀2次）；

                pictureBox5.Image = corrosion1.ToImage<Gray, byte>().ToBitmap();
                corrosion_img = corrosion1;//（将第一次腐蚀的结果作为第二次腐蚀的输入，如此循环，一直腐蚀，直至消失）;

                dilate_img = corrosion1;//（将腐蚀结果corrosion1复制给dilate_img，作为膨胀的输入）;
                //break;

                //显示、保存图像；
                #region
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_corrosion1.bmp", corrosion1); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                #endregion

                //add(2021-1228，自适应手动调整腐蚀次数，默认腐蚀次数为1)；
                //二次腐蚀；
                #region
                //case 1:
                //Mat img7 = corrosion1;
                //        Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
                //        Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //        CvInvoke.Erode(img7, corrosion2, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作；
                //        //pictureBox5.Image.Dispose();
                //        pictureBox5.Image = corrosion2.ToImage<Gray, byte>().ToBitmap();
                //      break;
                //}
                //count++;
                //add(1213)
                #endregion

                //二次腐蚀；
                #region
                //Mat img7 = corrosion1;
                //Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //CvInvoke.Erode(img7, corrosion2, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作；
                //pictureBox5.Image = corrosion2.ToImage<Bgr, byte>().ToBitmap();
                #endregion

                ////膨胀
                #region
                //Mat corrosion_img = binary;
                //Mat swell = new Mat();
                //Mat corrosion1 = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行腐蚀操作；

                //CvInvoke.Dilate(corrosion_img, swell, struct_element, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(128, 128, 128));

                //pictureBox5.Image = corrosion1.ToImage<Bgr, byte>().ToBitmap();

                //pictureBox5.Image = swell.ToImage<Bgr, byte>().ToBitmap();
                #endregion
                /********************自动模式下---6-形态学腐蚀操作运算--end************************/
                /******************* (add,2022-0111,腐蚀）****************************************/
                /*************相当于(erode_Click));**********************************************/



                /********************自动模式下---7-形态学膨胀操作运算--start************************/
                /******************* (add,2022-0111,膨胀）******************************************/
                /*************相当于(dilate_Click));***************************************/



                //膨胀（work,单击一次膨胀按钮即膨胀一次）；
                #region
                //corrosion_img = binary_img.Copy();// 将二值化结果binary_img复制到corrosion_img；            

                //try
                //{
                //    //binary_img = img3.Copy();//将模板图像temp-匹配区域imge2的差运算结果拷贝到binary_img;                
                //    //int count1 = Convert.ToInt32(textBox1.Text); ; //string类型转换int类型
                //    int num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;
                //    CvInvoke.Threshold(binary_img, binary, num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);//num自适应调节;
                //    //pictureBox5.Image = binary.ToImage<Gray, byte>().ToBitmap();
                //}
                //catch (System.Exception ex)
                //{
                //    MessageBox.Show("  格式错误，请输入整型！");

                //}

                //add(1213)
                //add(1213)
                //Mat corrosion1 = new Mat();//创建Mat，存储处理后的图像；

                //switch (counti)
                //{

                //腐蚀；
                //case 0:
                //Mat corrosion_img = binary;
                //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                ////指定参数进行形态学开（先腐蚀后膨胀）操作；
                ////CvInvoke.MorphologyEx(corrosion_img,corrosion1,Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));
                //CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作；
                //pictureBox5.Image = corrosion1.ToImage<Gray, byte>().ToBitmap();
                //break;

                //二次腐蚀；               
                //Mat img7 = corrosion1;
                //Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //CvInvoke.Erode(img7, corrosion2, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作；
                //pictureBox5.Image.Dispose();
                //pictureBox5.Image = corrosion2.ToImage<Gray, byte>().ToBitmap();

                //switch (counti % 2)
                //{
                // 膨胀；
                //case 0:
                #endregion

                //add(2021-1228，自适应手动调整膨胀次数，默认膨胀次数为1)；

                //形态学操作类型（add-2021-1228）
                #region
                //Erode = 0;(形态学腐蚀，消除边界点，使边界向内收缩，可消除没有意义的物体。使用CvInvoke.Erode)；
                //Dilate =1;(形态学膨胀，将与物体接触的所有背景点合并到该物体中，使边界向外扩张，可用来填补物体中的空洞。使用 CvInvoke.Dilate)；
                //open = 2;(形态学开操作，先腐蚀，后膨胀。即：OPEN(X)=D(E(X))，（使用CvInvoke.MorphologyEx（Emgu.CV.CvEnum.MorphOp.Open）作用：消除小物体，在纤细点处分离物体，位置和形状总是不变)；
                //Close = 3;(形态学闭操作，先膨胀，后腐蚀。即：CLOSE(X)=E(D(X)),（使用CvInvoke.MorphologyEx（Emgu.CV.CvEnum.MorphOp.Close））作用：可填平小孔，位置、形状不变。)
                //Gradient = 4;(形态学梯度操作，膨胀与腐蚀差。即：Grad(X)=Dilate(X)-Erode(X)。膨胀：扩大图像的边界，腐蚀：缩小图像的边界，膨胀-腐蚀：即为形态学梯度操作，可保存边缘轮廓)；
                //Tophat = 5;(形态学高帽，原始图像-开操作图像，即：Tophat(X)=X-Open（X）)；
                //Blackhat = 6;(形态学地帽，闭操作图像-原始图像，即：Blackhat(X)=Close(X)-X,结果是：输出图像大部分面积均为偏黑色，故称黑帽操作);
                //形态学操作类型（add-2021-1228）
                #endregion
                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                //string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //指定参数进行形态学开操作； //new Point(-1, -1),表示结构元素中心，3，表示形态学开操作中腐蚀迭代次数；
                //CvInvoke.MorphologyEx(corrosion_img, corrosion1, Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

                int dilate_num = Convert.ToInt32(textBox5.Text);//实现string类型到int类型转换（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；

                CvInvoke.Dilate(dilate_img, swell, struct_element1, new Point(-1, -1), dilate_num, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；

                //CvInvoke.Dilate(dilate_img, swell, struct_element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作（2：膨胀2次）；
                pictureBox5.Image = swell.ToImage<Gray, byte>().ToBitmap();

                dilate_img = swell; //（将第一次膨胀的结果作为第二次膨胀的输入，如此循环，一直膨胀）;

                //显示、保存图像；
                #region
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_swell.bmp", swell); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                #endregion

                //二次膨胀；
                #region
                //break;
                //case 1:
                //Mat img9 = corrosion2;
                //Mat swell2 = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element3 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                ////指定参数进行形态学开操作；
                ////CvInvoke.MorphologyEx(img7, corrosion2, Emgu.CV.CvEnum.MorphOp.Open, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

                //CvInvoke.Dilate(img9, swell2, struct_element3, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作；
                //pictureBox5.Image = swell2.ToImage<Gray, byte>().ToBitmap();
                //break;

                //Mat corrosion_img = binary;
                //Mat corrosion1 = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //CvInvoke.Erode(corrosion_img, corrosion1, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行腐蚀操作；
                //pictureBox5.Image = corrosion1.ToImage<Bgr, byte>().ToBitmap();

                //Mat img7 = corrosion1;
                //Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element2 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //CvInvoke.Erode(img7, corrosion2, struct_element2, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行腐蚀操作；
                //pictureBox5.Image = corrosion2.ToImage<Bgr, byte>().ToBitmap();

                //}
                //counti++;
                #endregion
                /********************自动模式下---7-形态学膨胀操作运算--end************************/
                /******************* (add,2022-0111,膨胀）******************************************/
                /*************相当于(dilate_Click));***************************************/


                /********************自动模式下---8-获取二值化轮廓结果并打印轮廓面积信息及将匹配信息写入Excel表中--start************************/
                /******************* (add,2022-0111,获取二值化轮廓信息）**************************************/
                /*************相当于(BinarizedContour_Click));**********************************************/


                //模板与匹配区域差二值化后的轮廓结果；
                #region
                //contour_img = binary_img.Copy();  // 将二值化结果binary_img复制到contour_img； 
                //开运算2（先腐蚀后膨胀）；
                //corrosion_img = binary_img.Copy();// 将二值化结果binary_img复制到corrosion_img；                       
                //try
                //{
                //    //binary_img = img3.Copy();//将模板图像temp-匹配区域imge2的差运算结果拷贝到binary_img;                
                //    //int count1 = Convert.ToInt32(textBox1.Text); ; //string类型转换int类型
                //    int num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;
                //    CvInvoke.Threshold(binary_img, binary, num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);//num自适应调节;
                //    //pictureBox5.Image = binary.ToImage<Gray, byte>().ToBitmap();
                //}
                //catch (System.Exception ex)
                //{
                //    MessageBox.Show("  格式错误，请输入整型！");

                //}           

                //switch (countj % 2)
                //{
                //一次腐蚀；
                //case 0:
                //Mat corrosion_img = binary;
                //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作；
                //指定参数进行形态学开（先腐蚀后膨胀）操作；
                //CvInvoke.MorphologyEx(corrosion_img,corrosion1,Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));
                //pictureBox5.Image = corrosion1.ToImage<Gray, byte>().ToBitmap();
                //break;

                // 膨胀1；
                //Mat img7 = corrosion1;
                //Mat swell1 = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //指定参数进行形态学开操作；
                //CvInvoke.MorphologyEx(corrosion_img, corrosion1, Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

                //CvInvoke.Dilate(img7, swell1, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行腐蚀操作；
                //pictureBox5.Image = swell1.ToImage<Gray, byte>().ToBitmap();
                #endregion

                // canny算子；
                #region
                //Mat corrosion_img = binary;
                //Mat corrosion1 = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                ////指定参数进行形态学开操作；
                //CvInvoke.MorphologyEx(corrosion_img, corrosion1, Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

                ////开运算2（先腐蚀后膨胀）；
                //Mat img7 = corrosion1;
                //Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                ////指定参数进行形态学开操作；
                //CvInvoke.MorphologyEx(img7, corrosion2, Emgu.CV.CvEnum.MorphOp.Open, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));
                #endregion

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                /*string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File);*/ // for getting only MyFile
                                                                                             //(add-2021-1228,保存处理后的图像至指定文件夹)

                //(add-2021-1228,显示图像名称)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,显示图像名称)

                contour_img = swell;

                CvInvoke.Canny(contour_img, canny_img, 120, 180); //指定阈值（第一阈值：120，第二阈值：180）进行Canny算子处理(这里是否需要Canny算子？？)；                       

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储查找到的轮廓；
                //add(1215，用于面积筛选)
                VectorOfVectorOfPoint use_contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储筛选后的轮廓；
                //add(1215，用于面积筛选)

                //(RetrType.External:提取最外层轮廓；RetrType.List:提取所有轮廓;Emgu.CV.CvEnum.RetrType.Ccomp检索所有轮廓，并将它们组织成两级层级结构：水平是组件的外部边界，二级约束边界的洞）；
                CvInvoke.FindContours(canny_img, contours, null, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);//查找dst所有轮廓并保存（contour_img<-->canny_img）；                                                            

                //add(2021-1217，面积筛选)
                //遍历完所有面积，打印总面积；
                double sum_area = 0;
                int ksize = contours.Size;//获取连通区域的个数；
                for (int i = 0; i < ksize; i++)//遍历每个连通轮廓；
                {
                    VectorOfPoint contour = contours[i];//获取独立的连通轮廓；
                    double area = CvInvoke.ContourArea(contour);//计算连通轮廓的面积；

                    double area1 = Convert.ToDouble(textBox4.Text);//实现string类型到double类型的转换;
                                                                   //add(2021-1227,当输入面积小于0时，area==0);
                    if (area1 <= 0)
                    {
                        area1 = 0;
                    }
                    //add(2021-1227,当输入面积小于0时，num1==0);

                    //add(2021-1227，当轮廓面积大于阈值面积时，才执行下列语句，实现面积相加，否则跳出条件语句）；
                    if (area > area1)//对每一轮廓区域面积进行面积筛选；                                
                    {

                        use_contours.Push(contour);//添加筛选后的连通轮廓；
                        sum_area += area;//遍历结果相加；

                    }

                }
                //打印轮廓总面积信息；
                displab3.Text = "轮廓总面积: " + sum_area.ToString() + ";" + "\n" + "\n" + "\n";//打印遍历完的总面积（0044轮廓面积,当阈值选择30时，显示出错,试试RetrType.Ccomp）


                //打印匹配信息（2021-1228,保存文本信息至指定文件夹）；
                //displab.Text = dbf_File2 + "\n" +
                //                 "轮廓总面积:  " + sum_area.ToString();

                ////（2021-1228,保存文本信息至指定文件夹）；
                //string txt = displab1.Text;
                //string filename = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息1.txt";   //文件名，可以带路径

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                //sw.Write(displab1.Text);
                //sw.Close();

                //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；
                string txt1 = displab3.Text;
                string filename1 = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息.txt";   //文件名，可以带路径

                FileStream fs1 = new FileStream(filename1, FileMode.Append);
                StreamWriter wr1 = null;
                wr1 = new StreamWriter(fs1);

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                wr1.Write(displab3.Text);
                wr1.Close();
                //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；



                /********************自动模式下---9-用流创建或读取.xlsx文件（同时流关联了文件）--start************************/
                /******************* (add,2022-0111,用流创建或读取.xlsx文件）*********/
                /*************相当于(BinarizedContour_Click下));*******/

                filestream = new FileStream("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息.xlsx", FileMode.OpenOrCreate);
                //(add-2021-1231)用流创建或读取.xlsx文件（同时流关联了文件）


                //创建表和sheet;
                if (indexOfExcel_j == 1)
                {
                    wk = new XSSFWorkbook();   //创建表对象wk
                    isheet = wk.CreateSheet("Sheet1");   //在wk中创建sheet1
                                                         //创建表头
                    IRow rowtitle = null;
                    rowtitle = isheet.CreateRow(0); //创建index=j的行

                    ICell cellTitle0 = null;
                    cellTitle0 = rowtitle.CreateCell(0);  //在index=j的行中创建index=0的单元格
                    cellTitle0.SetCellValue("时间"); //给创建的单元格赋值string

                    ICell cellTitle1 = null;
                    cellTitle1 = rowtitle.CreateCell(1);  //在index=j的行中创建index=0的单元格
                    cellTitle1.SetCellValue("图像名称"); //给创建的单元格赋值string

                    ICell cellTitle2 = null;
                    cellTitle2 = rowtitle.CreateCell(2);  //在index=j的行中创建index=0的单元格
                    cellTitle2.SetCellValue("匹配信息坐标X"); //给创建的单元格赋值string

                    ICell cellTitle3 = null;
                    cellTitle3 = rowtitle.CreateCell(3);  //在index=j的行中创建index=0的单元格
                    cellTitle3.SetCellValue("匹配信息坐标Y"); //给创建的单元格赋值string

                    ICell cellTitle4 = null;
                    cellTitle4 = rowtitle.CreateCell(4);  //在index=j的行中创建index=0的单元格
                    cellTitle4.SetCellValue("最大相似度"); //给创建的单元格赋值string


                    ICell cellTitle5 = null;
                    cellTitle5 = rowtitle.CreateCell(5);  //在index=j的行中创建index=0的单元格
                    cellTitle5.SetCellValue("最小相似度"); //给创建的单元格赋值string

                    ICell cellTitle6 = null;
                    cellTitle6 = rowtitle.CreateCell(6);  //在index=j的行中创建index=0的单元格
                    cellTitle6.SetCellValue("轮廓总面积"); //给创建的单元格赋值strin
                }


                //向单元格中写数据(2021-1231,循环写入行、列数据，先定义行数，再定义列数，这里是2行6列);
                //for (int indexOfExcel_j = 1; indexOfExcel_j < 3; indexOfExcel_j++)//(j:行)；
                //{

                //IRow row2 = null;
                //row2 = isheet.CreateRow(indexOfExcel_j++); //创建index=j的行

                ////datetime格式化；
                DateTime dt1 = DateTime.Now;

                rowWrite = isheet.CreateRow(indexOfExcel_j++); //创建index=j的行

                ICell cell0 = null;
                cell0 = rowWrite.CreateCell(0);  //在index=j的行中创建index=0的单元格
                cell0.SetCellValue(dt1.ToLocalTime().ToString()); //给创建的单元格赋值string

                ICell cell1 = null;
                cell1 = rowWrite.CreateCell(1);  //在index=j的行中创建index=0的单元格
                cell1.SetCellValue(dbf_File2); //给创建的单元格赋值string

                ICell cell2 = null;
                cell2 = rowWrite.CreateCell(2);  //在index=j的行中创建index=0的单元格
                cell2.SetCellValue("X:" + max_loc.X); //给创建的单元格赋值string

                ICell cell3 = null;
                cell3 = rowWrite.CreateCell(3);  //在index=j的行中创建index=0的单元格
                cell3.SetCellValue("Y:" + max_loc.Y); //给创建的单元格赋值string

                ICell cell4 = null;
                cell4 = rowWrite.CreateCell(4);  //在index=j的行中创建index=0的单元格
                cell4.SetCellValue(max.ToString("f2")); //给创建的单元格赋值string


                ICell cell5 = null;
                cell5 = rowWrite.CreateCell(5);  //在index=j的行中创建index=0的单元格
                cell5.SetCellValue(min.ToString("f2")); //给创建的单元格赋值string

                ICell cell6 = null;
                cell6 = rowWrite.CreateCell(6);  //在index=j的行中创建index=0的单元格
                cell6.SetCellValue(sum_area.ToString()); //给创建的单元格赋值string


                //通过流将表中写入的数据一次性写入文件中;
                wk.Write(filestream); //通过流filestream将表wk写入文件


                //(add,20211231)
                //filestream.Close(); //关闭文件流filestream
                //wk.Close(); //关闭Excel表对象wk



                Mat mask = contour_img.ToImage<Bgr, byte>().CopyBlank().Mat;//获取一张背景为黑色的图像，尺寸大小与contour_img一样(类型Bgr);
                CvInvoke.DrawContours(mask, use_contours, -1, new MCvScalar(0, 0, 255));//采用红色画笔在mask掩膜图像中画出所有轮廓；(MCvScalar是一个具有单元素到四元素间的一个数组)
                pictureBox6.Image = mask.ToImage<Bgr, byte>().ToBitmap();//显示轮廓区域图像；


                //显示、保存图像；
                #region
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_mask.bmp", mask); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                #endregion


            }
        }
        
        /********************自动模式下---8-获取二值化轮廓结果并打印轮廓面积信息及将匹配信息写入Excel表中--end************************/
        /******************* (add,2022-0111,获取二值化轮廓信息）**************************************/
        /*************相当于(BinarizedContour_Click));**********************************************/


        /********************自动模式下---9-用流创建或读取.xlsx文件（同时流关联了文件）************************/
        /******************* (add-2022-0104,用流创建或读取.xlsx文件）*********/
        /*************(BinarizedContour_Click下));*******/
        /*******************自动模式下--END*****************************************/

        //private int counti = 0;
        //add(2021-1228，自适应手动调整膨胀次数，默认膨胀次数为1)；




        private void folder_Click(object sender, EventArgs e)
        {
            /*******************************自动模式下遍历文件夹--1--测试整个文件夹(加载待匹配图像)--start************************/
            /*****************************(add,2022-0111)--start***************************/
            /*****************************(add,2022-0111)--start***************************/

            //文件夹测试(2022-0106--start)；
            picturecount = 0;

            #region
            //for (int i = 0; i < classcount.Length; i++)
            //{
            //    classcount[i] = 0;
            //}

            //if (defaultfilepath != "")
            //{
            //    folderBrowserDialog1.SelectedPath = defaultfilepath;
            //}
            //else
            //{
            //    folderBrowserDialog1.SelectedPath = Application.StartupPath;
            //}
            #endregion

            //(打开文件夹函数--2022-0106--start)
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                defaultfilepath = folderBrowserDialog1.SelectedPath;
                thread1 = new Thread(new ThreadStart(ImageProcessingAll));//创建线程
                thread1.Start();
            }
        }
        //文件夹测试函数(2022-0106--end);

        //文件夹测试函数(2022-0106--start);
        private void ImageProcessingAll()   //处理文件指定文件夹下所有图片
        {

            //nMax = 0;
            //nMin = 1;
            //double dThreValue = Convert.ToDouble(ThreshTb.Text);

            //Mat img;  //待测试图片
            DirectoryInfo folder;

            folder = new DirectoryInfo(defaultfilepath);

            //遍历文件夹；
            foreach (FileInfo nextfile in folder.GetFiles())
            {
                //Invoke((EventHandler)delegate { label2.Text = "图片名称：" + Path.GetFileName(nextfile.FullName); });
                //Invoke((EventHandler)delegate { label2.Refresh(); });

                Point max_loc1 = new Point(0, 0);
                Point classNumber = max_loc1;    //最大可能性位置

                //string typeName = typeList[classNumber.X];

                convert_img = CvInvoke.Imread(nextfile.FullName, Emgu.CV.CvEnum.ImreadModes.AnyColor);

                //Image<Bgr, byte> matToimg = match_img.ToImage<Bgr, byte>();


                Image<Bgr, Byte> match_img = convert_img.ToImage<Bgr, Byte>();//Mat 2 Image;

                if (match_img == null)
                {
                    Invoke((EventHandler)delegate { label18.Text = "无法加载文件！"; });
                    Invoke((EventHandler)delegate { label18.Refresh(); });
                    return;
                }

                //开始时间
                #region
                //Stopwatch stopwatch = new Stopwatch();
                //stopwatch.Start();   //开始监视代码运行时间

                //对输入图像数据进行处理
                //Mat blob = DnnInvoke.BlobFromImage(img, 1.0f, new Size(224, 224), new MCvScalar(), true, false);

                //进行图像种类预测
                //Mat prob;

                //net.SetInput(blob);      //设置输入数据
                //prob = net.Forward();    //推理

                //得到最可能分类输出
                //Mat probMat = prob.Reshape(1, 1);
                //double minVal = 0;  //最小可能性
                //double maxVal = 0;  //最大可能性
                //Point minLoc = new Point();
                //Point maxLoc = new Point();

                //CvInvoke.MinMaxLoc(probMat, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                //double classProb = maxVal;     //最大可能性
                //Point classNumber = maxLoc;    //最大可能性位置


                //if (nMin > maxVal)
                //{
                //    nMin = maxVal;
                //}
                //if (nMax < maxVal)
                //{
                //    nMax = maxVal;
                //}

                //string typeName = typeList[classNumber.X];

                //stopwatch.Stop();  //  停止监视
                //long timespan = stopwatch.ElapsedMilliseconds;  //获取当前实例测量得出的总时间

                //classcount[classNumber.X]++;
                //picturecount++;

                //classcount[classNumber.X]++;
                #endregion
                picturecount++;

                //保存图像（NG\OK）；
                #region
                //if (ComboBoxindex == 1)    //保存NG图像
                //{
                //no******/
                //if (classNumber.X == 0)
                //{
                //    CvInvoke.Imwrite("result/NG/" + Path.GetFileName(nextfile.FullName), img);
                //}
                //no******/

                //    if (maxVal < dThreValue)
                //    {
                //        CvInvoke.Imwrite("result/NG/" + Path.GetFileName(nextfile.FullName), img);
                //    }


                //}
                //else if (ComboBoxindex == 2)    //保存OK图像
                //{
                //if (classNumber.X == 1)
                //{
                //    CvInvoke.Imwrite("result/OK/" + Path.GetFileName(nextfile.FullName), img);
                //}
                //if (maxVal > dThreValue)
                //{
                //    CvInvoke.Imwrite("result/OK/" + Path.GetFileName(nextfile.FullName), img);
                //}
                #endregion

                Invoke((EventHandler)delegate { label13.Text = "图片总数：" + picturecount.ToString(); });
                Invoke((EventHandler)delegate { label13.Refresh(); });



                //检测内容
                Invoke((EventHandler)delegate { pictureBox2.Image = BitmapExtension.ToBitmap(match_img); });
                Invoke((EventHandler)delegate { pictureBox2.Refresh(); });

                //显示label信息：图片测试结果、可能性、处理总时间；
                #region
                //Invoke((EventHandler)delegate { label1.Text = "图片测试结果为：" + typeName + "  可能性为：" + classProb.ToString("0.00000") + "  处理总时间为：" + timespan.ToString() + "ms"; });
                //Invoke((EventHandler)delegate { label1.Refresh(); });
                #endregion




                /****************自动模式下遍历文件夹---2-加载待匹配图像并画出矩形框(--start)；*********************/
                /**********************************（add,2022-0111--start);***************************/
                /************************************相当于(Matchimage_Click)*****************************/

                //match_img = new Image<Bgr, Byte>(OpenFileDialog.FileName);               
                Mat result1 = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                CvInvoke.MatchTemplate(match_img, temp, result1, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；
                //****************//
                //CvInvoke.Normalize(result1, result1, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                //double max = 0, min = 0;//创建double极值；
                //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；

                //****************//
                CvInvoke.MinMaxLoc(result1, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                match_img1 = match_img.Copy(); //将原图match_img复制到match_img1中，对match_img1进行画矩形框，避免pictureBox3显示匹配区域出现边框；
                CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
                pictureBox2.Image = match_img1.ToBitmap();//显示找到模板图像的待搜索图像；

                //显示、保存图像； 
                #region
                //****************//
                //CvInvoke.Imshow("img", temp); //显示图片
                //****************//

                //CvInvoke.Imwrite("D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\" + dbf_File2 + "_match_img1.bmp", match_img1); //保存匹配结果图像；

                CvInvoke.Imwrite("test/match_img_result/" + Path.GetFileName(nextfile.FullName), match_img1); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待

                /****************自动模式下遍历文件夹---2-加载待匹配图像并画出矩形框(--end)；*********************/
                /**********************************（add,2022-0111--end);***************************/
                /************************************相当于(Matchimage_Click)*****************************/



                /****************自动模式下遍历文件夹---3-获取匹配区域并打印匹配文本信息(--start)；*********************/
                /**********************************（add,2022-0111--start);*******************************/
                /*********************************相当于(MatchingArea_Click)***********************************/

                // 获取匹配区域；
                Mat result = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                CvInvoke.MatchTemplate(match_img, temp, result, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);//采用系数匹配法(CcorrNormed)，打印出匹配相似度信息,数值越大越接近准确图像；
                //CvInvoke.Normalize(result, result, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                //double max = 0, min = 0;//创建double极值；
                //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
                CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                //string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图match_img复制到match_area_img中，显示匹配区域；
                pictureBox3.Image = match_area_img.ToBitmap();//显示匹配区域；

                //显示、保存图像；
                #region
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("test/match_area_img_result/" + Path.GetFileName(nextfile.FullName), match_area_img); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                #endregion


                //////datetime格式化并在Excel表中写入时间数据；
                #region
                //DateTime dt = DateTime.Now;

                ////在Excel表中写入时间数据时，设置单元格格式，自定义格式yyyy-MM-dd HH:mm:ss
                //(label as TextBox).Text = dt.ToLocalTime().ToString();   //mm-dd才显示08-01，否则显示8-1
                ////fff个数表示小数点后显示的位数，这里精确到小数点后3位
                #endregion

                ////datetime格式化；
                DateTime dt1 = DateTime.Now;

                //打印匹配信息（2021-1228,保存文本信息至指定文件夹）；(nextFile.Extension:图像名称；nextfile.FullName：完整路径)
                displab2.Text = "时间:" + dt1.ToLocalTime().ToString() + "\n" + "图像名称：" + nextfile.FullName + "\n" +
                                "\n" + "匹配信息: X= " +
                                 max_loc.X + "," + " Y= " + max_loc.Y + ";" +
                                "\n" +
                                "最大相似度: " + max.ToString("f2") + ";" + "\n" +
                                "最小相似度: " + min.ToString("f2") + ";" + "\n" + "\n";

                //（2021-1228,保存文本信息至指定文件夹）；
                string txt2 = displab2.Text;
                string filename2 = @"D:\SKQ\VS-Code\Demo\Emgu.CV.CvEnum\Emgu.CV.CvEnum\bin\Debug\test\txt_result\匹配信息.txt";   //文件名，可以带路径

                FileStream fs2 = new FileStream(filename2, FileMode.Append);
                StreamWriter wr2 = null;
                wr2 = new StreamWriter(fs2);

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                wr2.Write(displab2.Text);
                wr2.Close();
                //（2021-1228,保存文本信息至指定文件夹）；
                #region
                //（add-2021-1229）将文本信息（dislab.Text）保存至Excel表格中；
                //string sContext = displab.Text;// 控件的内容
                //string sExcelPath = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息.xlsx"; //excel路径
                //int iSheet = 1;

                // 连接Excel
                //Application app = new Application();
                //WorkBook WB = app.Workbooks.Open(sExcelPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //WorkSheet WS = (Worksheet)WB.Worksheets[iSheet];
                //Range range = (Range)worksheet.Cells[iRow, iCol]; ////得到指定行列的单元格
                //range.Value2 = sContext; // 对单元格赋值

                //（add-2021-1229）将文本信息（dislab.Text）保存至Excel表格中；



                //向单元格中写数据(2021-1229,循环写入行、列数据，先定义行数，再定义列数);

                ////datetime格式化；
                //DateTime dt1 = DateTime.Now;

                ////在Excel表中写入时间数据时，设置单元格格式，自定义格式yyyy-MM-dd HH:mm:ss
                //string current_time = dt1.ToString("yyyy-MM-dd HH:mm:ss.fff");   //mm-dd才显示08-01，否则显示8-1
                ////fff个数表示小数点后显示的位数，这里精确到小数点后3位


                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)
                #endregion

                /****************自动模式下遍历文件夹---3-获取匹配区域并打印匹配文本信息(--end)；*********************/
                /**********************************（add,2022-0111--end);*******************************/
                /*********************************相当于(MatchingArea_Click)***********************************/


                /****************************自动模式下遍历文件夹---4-进行差运算(2022-0111--start)************************************/
                /***************************(add-2022-0111,进行差运算--start)**********************************************/
                /********************************相当于(MatchDifference_Click);*******************************************/

                // 模板与匹配区域差运算方法;
                #region 
                /*模板与匹配区域差运算；        
                Sqdiff = 0(平方差匹配，最好匹配为0)；
                SqdiffNormed = 1(归一化平方差匹配，最好结果为0)；
                Ccorr = 2(相关匹配法，数值越大效果越好）；
                CcorrNormed = 3(归一化相关匹配法，数值越大效果越好）；
                Coeff = 4(系数匹配法，数值越大效果越好）；
                CoeffNormed = 5(归一化系数匹配法，数值越大效果越好）;*/
                #endregion

                //(add-2021-1228,保存处理后的图像至指定文件夹)

                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);


                Mat result2 = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                CvInvoke.MatchTemplate(match_img, temp, result2, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；

                //CvInvoke.Normalize(result2, result2, 255, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                //double max = 0, min = 0;//创建double极值；
                //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
                CvInvoke.MinMaxLoc(result2, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                //打印labbel信息；
                #region
                /*displab.Text =
                    "匹配信息: max value" + "\n" +
                    max.ToString() + "\n" +
                    "min value " + min.ToString() + "\n" +
                    "min_loc value " + min_loc.ToString() + "\n" +
                    "max_loc value " + max_loc.ToString();*/
                #endregion

                match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图img复制到img2中，显示匹配区域；

                match_area_img_1 = match_area_img.AbsDiff(temp);//(1220-add,temp\img2绝对值差值)；
                match_area_img_out = match_area_img_1.Convert<Gray, Byte>();//（将match_area_img_1转化为灰度图）;(image定义的图像通过Convert转化为灰度图);
                pictureBox4.Image = match_area_img_out.ToBitmap();//显示模板图像与匹配区域的差运算结果；

                //(2021-1230)
                binary_img = match_area_img_out;//将模板图像temp-匹配区域imge2的差运算结果（match_area_img_out）拷贝到binary_img;

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)


                //显示、保存图像；
                #region
                ////CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("test/match_area_img_out_result/" + Path.GetFileName(nextfile.FullName), match_area_img_out); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待

                //pictureBox4.Image = result.ToImage<Gray, byte>().ToBitmap();//显示匹配结果，若 pictureBox3.Image = result显示为全黑，必须将类型转换为Byte；
                #endregion
                /****************************自动模式下遍历文件夹---4-进行差运算(2022-0111--end)************************************/
                /***************************(add-2022-0111,进行差运算--end)**********************************************/
                /********************************相当于(MatchDifference_Click);*******************************************/




                /********************自动模式下遍历文件夹---5-进行二值化运算(2022-0111--start)******************************************/
                /******************* (add-2022-0111,二值化--start）**************************************************************/
                /*************相当于(MatchingBinarization_Click))***********************************************************/

                //模板与匹配区域差二值化结果；            
                #region
                //Mat result = new Mat(new Size(img.Width - temp.Width + 1, img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                //CvInvoke.MatchTemplate(img, temp, result, Emgu.CV.CvEnum.TemplateMatchingType.Ccoeff);//采用系数匹配法，数值越大越接近准确图像；

                //CvInvoke.Normalize(result, result, 255, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                //double max = 0, min = 0;//创建double极值；
                //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
                //CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                //img2 = img.Copy(new Rectangle(max_loc, temp.Size)); //将原图img复制到img2中，显示匹配区域；
                //img3 = temp.Sub(img2);//模板图像temp-匹配区域imge2，进行差运算；           
                //pictureBox4.Image = img3.ToBitmap();//显示模板图像与匹配区域的差运算结果；
                #endregion
                //string转换到int类型；
                #region
                //try

                //{
                //    int count1 = Convert.ToInt32(textBox1.Text); //string类型转换int类型(count1为待装换的数值)；
                //}
                //catch (System.Exception ec)
                //{
                //    MessageBox.Show("格式错误!");
                //}
                #endregion

                //(2021-1230,error:OpenCV: !src.empty())
                //binary_img = match_area_img_out;//将模板图像temp-匹配区域imge2的差运算结果（match_area_img_out）拷贝到binary_img;
                //(2021-1230,error:OpenCV: !src.empty())


                ////(add-2021-1228,保存处理后的图像至指定文件夹)
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                //string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                //二值化
                #region
                //Mat binary_img = new Mat("D:\\VS-Code\\Demo\\Emgu.CV.CvEnum\\MatchDifference.bmp");//指定图像；

                //match_area_img_1 = match_area_img.AbsDiff(temp);//(1220-add,|temp-img2|绝对值差值)；

                //Image<Bgr, byte> binary = new Image <Bgr, byte>;

                //阈值20(work)\(自适应调整阈值)Binary类型；
                #endregion

                //自适应阈值
                #region
                //CvInvoke.Threshold(binary_img, binary, 20, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

                //region
                //int num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;                                           
                //CvInvoke.Threshold(binary_img, binary, num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
                #endregion
                //try
                //{
                //binary_img = img3.Copy();//将模板图像temp-匹配区域imge2的差运算结果拷贝到binary_img;                
                //int count1 = Convert.ToInt32(textBox1.Text);  //string类型转换int类型
                int pixel_num = Convert.ToInt32(textBox8.Text);//实现string类型到int类型的转换;

                //add(2021-1227,num>255,num=255;num<0,num=0);
                if (pixel_num > 255)
                {
                    pixel_num = 255;
                }
                if (pixel_num <= 0)
                {
                    pixel_num = 0;
                }
                //add(2021-1227,num>255,num=255;num<0,num=0);

                CvInvoke.Threshold(binary_img, binary, pixel_num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);//num自适应调节;

                pictureBox5.Image = binary.ToImage<Gray, byte>().ToBitmap();
                corrosion_img = binary;



                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)


                //显示、保存图像；
                #region
                ////CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("test/binary_result/" + Path.GetFileName(nextfile.FullName), binary); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待


                //}
                //保存结果；
                #region          
                //CvInvoke.Imwrite("D:\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Binary_Thres-20-Gray.bmp", binary.ToImage<Gray, byte>()); //（保存结果图片(灰度图),dst彩色图）；
                ////CvInvoke.Imwrite("D:\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Binary_Thres-20-RGB.bmp", binary); //保存结果图片(彩色图)；
                //CvInvoke.WaitKey(0); //暂停按键等待
                #endregion
                //腐蚀
                #region
                //腐蚀
                //corrosion_img = binary_img.Copy();// 将二值化结果binary_img复制到corrosion_img；            
                //Mat corrosion_img = binary;
                //Mat corrosion = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3),new Point(-1,-1));//指定参数获得结构元素；
                //CvInvoke.Erode(corrosion_img, corrosion, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行腐蚀操作；
                //pictureBox5.Image = corrosion.ToImage<Bgr, byte>().ToBitmap();
                #endregion
                /********************自动模式下遍历文件夹---5-进行二值化运算(2022-0111--end)******************************************/
                /******************* (add-2022-0111,二值化--end）**************************************************************/
                /*************相当于(MatchingBinarization_Click))***********************************************************/


                /********************自动模式下遍历文件夹---6-进行形态学腐蚀运算(2022-0111--start)******************************************/
                /******************* (add-2022-0111,腐蚀运算--start）**************************************************************/
                /*********************相当于(erode_Click--start)***********************************************************/

                //腐蚀;Tab (代码整体左移)或 Shift+Tab(代码整体右移) ；
                #region
                //corrosion_img = binary_img.Copy();// 将二值化结果binary_img复制到corrosion_img;           

                //try
                //{
                //    //binary_img = img3.Copy();//将模板图像temp-匹配区域imge2的差运算结果拷贝到binary_img;                
                //    //int count1 = Convert.ToInt32(textBox1.Text); ; //string类型转换int类型
                //    int num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;
                //    CvInvoke.Threshold(binary_img, binary, num, 255, Emgu.CV.CvEnum.ThresholdType.Binary);//num自适应调节;
                //    //pictureBox5.Image = binary.ToImage<Gray, byte>().ToBitmap();
                //}
                //catch (System.Exception ex)
                //{
                //    MessageBox.Show("  格式错误，请输入整型！");

                //}     
                #endregion
                //switch (count % 2)
                //{
                //腐蚀（单击一次腐蚀按钮即腐蚀一次）；
                //case 0:

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                //string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)


                Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //指定参数进行形态学开（先腐蚀后膨胀）操作；
                //CvInvoke.MorphologyEx(corrosion_img,corrosion1,Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

                //new Point(-1, -1),表示结构元素中心，3，表示腐蚀迭代次数；

                //add(2021-1228，自适应手动调整腐蚀次数，默认腐蚀次数为1)；


                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)


                int erode_num = Convert.ToInt32(textBox6.Text);//实现string类型到int类型转换（默认腐蚀次数erode_num=1，点击一次腐蚀一次）；

                CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), erode_num, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作（自适应手动调整输入腐蚀次数）；

                //CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作（2：腐蚀2次）；

                pictureBox5.Image = corrosion1.ToImage<Gray, byte>().ToBitmap();
                corrosion_img = corrosion1;//（将第一次腐蚀的结果作为第二次腐蚀的输入，如此循环，一直腐蚀，直至消失）;

                dilate_img = corrosion1;//（将腐蚀结果corrosion1复制给dilate_img，作为膨胀的输入）;
                                        //break;

                //显示、保存图像；
                #region
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("test/corrosion1_result/" + Path.GetFileName(nextfile.FullName), corrosion1); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                #endregion

                /********************自动模式下遍历文件夹---6-进行形态学腐蚀运算(2022-0111--end)******************************************/
                /******************* (add-2022-0111,腐蚀运算--end）**************************************************************/
                /*********************相当于(erode_Click--end)***********************************************************/




                /********************自动模式下遍历文件夹---7-进行形态学膨胀运算(2022-0111--start)******************************************/
                /******************* (add-2022-0111,膨胀运算--start）**************************************************************/
                /*********************相当于(dilate_Click--start)***********************************************************/


                //形态学操作类型（add-2021-1228）
                #region
                //Erode = 0;(形态学腐蚀，消除边界点，使边界向内收缩，可消除没有意义的物体。使用CvInvoke.Erode)；
                //Dilate =1;(形态学膨胀，将与物体接触的所有背景点合并到该物体中，使边界向外扩张，可用来填补物体中的空洞。使用 CvInvoke.Dilate)；
                //open = 2;(形态学开操作，先腐蚀，后膨胀。即：OPEN(X)=D(E(X))，（使用CvInvoke.MorphologyEx（Emgu.CV.CvEnum.MorphOp.Open）作用：消除小物体，在纤细点处分离物体，位置和形状总是不变)；
                //Close = 3;(形态学闭操作，先膨胀，后腐蚀。即：CLOSE(X)=E(D(X)),（使用CvInvoke.MorphologyEx（Emgu.CV.CvEnum.MorphOp.Close））作用：可填平小孔，位置、形状不变。)
                //Gradient = 4;(形态学梯度操作，膨胀与腐蚀差。即：Grad(X)=Dilate(X)-Erode(X)。膨胀：扩大图像的边界，腐蚀：缩小图像的边界，膨胀-腐蚀：即为形态学梯度操作，可保存边缘轮廓)；
                //Tophat = 5;(形态学高帽，原始图像-开操作图像，即：Tophat(X)=X-Open（X）)；
                //Blackhat = 6;(形态学地帽，闭操作图像-原始图像，即：Blackhat(X)=Close(X)-X,结果是：输出图像大部分面积均为偏黑色，故称黑帽操作);
                //形态学操作类型（add-2021-1228）
                #endregion
                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                //string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                //指定参数进行形态学开操作； //new Point(-1, -1),表示结构元素中心，3，表示形态学开操作中腐蚀迭代次数；
                //CvInvoke.MorphologyEx(corrosion_img, corrosion1, Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

                int dilate_num = Convert.ToInt32(textBox5.Text);//实现string类型到int类型转换（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；

                CvInvoke.Dilate(dilate_img, swell, struct_element1, new Point(-1, -1), dilate_num, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；

                //CvInvoke.Dilate(dilate_img, swell, struct_element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作（2：膨胀2次）；
                pictureBox5.Image = swell.ToImage<Gray, byte>().ToBitmap();

                dilate_img = swell; //（将第一次膨胀的结果作为第二次膨胀的输入，如此循环，一直膨胀）;

                //显示、保存图像；
                #region
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite("test/swell_result/" + Path.GetFileName(nextfile.FullName), swell); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                #endregion
                //}//(2022-0110)

                /********************自动模式下遍历文件夹---7-进行形态学膨胀运算(2022-0111--end)******************************************/
                /******************* (add-2022-0111,膨胀运算--end）**************************************************************/
                /*********************相当于(dilate_Click--end)***********************************************************/


                /********************自动模式下遍历文件夹---8-进行二值化轮廓筛选及面积运算(2022-0111--start)******************************************/
                /******************* (add-2022-0111,轮廓运算--start）**************************************************************/
                /*********************相当于(BinarizedContour_Click--start)***********************************************************/

                // canny算子；
                #region
                //Mat corrosion_img = binary;
                //Mat corrosion1 = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                ////指定参数进行形态学开操作；
                //CvInvoke.MorphologyEx(corrosion_img, corrosion1, Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

                ////开运算2（先腐蚀后膨胀）；
                //Mat img7 = corrosion1;
                //Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
                //Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                ////指定参数进行形态学开操作；
                //CvInvoke.MorphologyEx(img7, corrosion2, Emgu.CV.CvEnum.MorphOp.Open, struct_element1, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));
                #endregion

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                //string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                //(add-2021-1228,显示图像名称)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,显示图像名称)

                contour_img = swell;

                CvInvoke.Canny(contour_img, canny_img, 120, 180); //指定阈值（第一阈值：120，第二阈值：180）进行Canny算子处理(这里是否需要Canny算子？？)；                       

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储查找到的轮廓；
                                                                             //add(1215，用于面积筛选)
                VectorOfVectorOfPoint use_contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储筛选后的轮廓；
                                                                                 //add(1215，用于面积筛选)

                //(RetrType.External:提取最外层轮廓；RetrType.List:提取所有轮廓;Emgu.CV.CvEnum.RetrType.Ccomp检索所有轮廓，并将它们组织成两级层级结构：水平是组件的外部边界，二级约束边界的洞）；
                CvInvoke.FindContours(canny_img, contours, null, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);//查找dst所有轮廓并保存（contour_img<-->canny_img）；                                                            

                //add(2021-1217，面积筛选)
                //遍历完所有面积，打印总面积；
                double sum_area = 0;
                int ksize = contours.Size;//获取连通区域的个数；
                for (int i = 0; i < ksize; i++)//遍历每个连通轮廓；
                {
                    VectorOfPoint contour = contours[i];//获取独立的连通轮廓；
                    double area = CvInvoke.ContourArea(contour);//计算连通轮廓的面积；

                    double area1 = Convert.ToDouble(textBox7.Text);//实现string类型到double类型的转换;
                                                                   //add(2021-1227,当输入面积小于0时，area==0);
                    if (area1 <= 0)
                    {
                        area1 = 0;
                    }
                    //add(2021-1227,当输入面积小于0时，num1==0);

                    //add(2021-1227，当轮廓面积大于阈值面积时，才执行下列语句，实现面积相加，否则跳出条件语句）；
                    if (area > area1)//对每一轮廓区域面积进行面积筛选；                                
                    {

                        use_contours.Push(contour);//添加筛选后的连通轮廓；
                        sum_area += area;//遍历结果相加；

                    }

                }
                //打印轮廓总面积信息；
                displab3.Text = "轮廓总面积: " + sum_area.ToString() + ";" + "\n" + "\n" + "\n";//打印遍历完的总面积（0044轮廓面积,当阈值选择30时，显示出错,试试RetrType.Ccomp）


                //打印匹配信息（2021-1228,保存文本信息至指定文件夹）；
                #region
                //displab.Text = dbf_File2 + "\n" +
                //                 "轮廓总面积:  " + sum_area.ToString();

                ////（2021-1228,保存文本信息至指定文件夹）；
                //string txt = displab1.Text;
                //string filename = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息1.txt";   //文件名，可以带路径

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                //sw.Write(displab1.Text);
                //sw.Close();
                #endregion
                //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；
                string txt3 = displab3.Text;
                string filename3 = @"D:\SKQ\VS-Code\Demo\Emgu.CV.CvEnum\Emgu.CV.CvEnum\bin\Debug\test\txt_result\匹配信息.txt";   //文件名，可以带路径

                FileStream fs3 = new FileStream(filename3, FileMode.Append);
                StreamWriter wr3 = null;
                wr3 = new StreamWriter(fs3);

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                wr3.Write(displab3.Text);
                wr3.Close();
                //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；

                //(add-2021-1231)用流创建或读取.xlsx文件（同时流关联了文件）

                /********************自动模式下---9-用流创建或读取.xlsx文件（同时流关联了文件）************************/
                /******************* (add-2022-0111,用流创建或读取.xlsx文件）*********/
                /*************(BinarizedContour_Click下));*******/
                /*******************自动模式下--start*****************************************/


                filestream = new FileStream(@"D:\SKQ\VS-Code\Demo\Emgu.CV.CvEnum\Emgu.CV.CvEnum\bin\Debug\test\xlsx_result\匹配信息.xlsx", FileMode.OpenOrCreate);
                //(add-2021-1231)用流创建或读取.xlsx文件（同时流关联了文件）

                //add(2022-0110)
                DirectoryInfo folder1;

                folder1 = new DirectoryInfo(defaultfilepath);

                //遍历文件
                //foreach (FileInfo nextfile in folder1.GetFiles())
                //{


                    //创建表和sheet;
                    if (indexOfExcel_j == 1)
                    {
                        wk = new XSSFWorkbook();   //创建表对象wk
                        isheet = wk.CreateSheet("Sheet1");   //在wk中创建sheet1
                                                             //创建表头
                        IRow rowtitle = null;
                        rowtitle = isheet.CreateRow(0); //创建index=j的行

                        ICell cellTitle0 = null;
                        cellTitle0 = rowtitle.CreateCell(0); //在index=j的行中创建index=0的单元格
                        cellTitle0.SetCellValue("时间"); //给创建的单元格赋值string

                        ICell cellTitle1 = null;
                        cellTitle1 = rowtitle.CreateCell(1);  //在index=j的行中创建index=0的单元格
                        cellTitle1.SetCellValue("图像名称"); //给创建的单元格赋值string

                        ICell cellTitle2 = null;
                        cellTitle2 = rowtitle.CreateCell(2);  //在index=j的行中创建index=0的单元格
                        cellTitle2.SetCellValue("匹配信息坐标X"); //给创建的单元格赋值string

                        ICell cellTitle3 = null;
                        cellTitle3 = rowtitle.CreateCell(3);  //在index=j的行中创建index=0的单元格
                        cellTitle3.SetCellValue("匹配信息坐标Y"); //给创建的单元格赋值string

                        ICell cellTitle4 = null;
                        cellTitle4 = rowtitle.CreateCell(4);  //在index=j的行中创建index=0的单元格
                        cellTitle4.SetCellValue("最大相似度"); //给创建的单元格赋值string


                        ICell cellTitle5 = null;
                        cellTitle5 = rowtitle.CreateCell(5);  //在index=j的行中创建index=0的单元格
                        cellTitle5.SetCellValue("最小相似度"); //给创建的单元格赋值string

                        ICell cellTitle6 = null;
                        cellTitle6 = rowtitle.CreateCell(6);  //在index=j的行中创建index=0的单元格
                        cellTitle6.SetCellValue("轮廓总面积"); //给创建的单元格赋值strin
                    }


                    //向单元格中写数据(2021-1231,循环写入行、列数据，先定义行数，再定义列数，这里是2行6列);
                    //for (int indexOfExcel_j = 1; indexOfExcel_j < 3; indexOfExcel_j++)//(j:行)；
                    //{

                    //IRow row2 = null;
                    //row2 = isheet.CreateRow(indexOfExcel_j++); //创建index=j的行

                    ////datetime格式化；
                    DateTime dt2 = DateTime.Now;

                    rowWrite = isheet.CreateRow(indexOfExcel_j++); //创建index=j的行

                    ICell cell0 = null;
                    cell0 = rowWrite.CreateCell(0);  //在index=j的行中创建index=0的单元格
                    cell0.SetCellValue(dt2.ToLocalTime().ToString()); //给创建的单元格赋值string

                    ICell cell1 = null;
                    cell1 = rowWrite.CreateCell(1);  //在index=j的行中创建index=0的单元格
                    cell1.SetCellValue(nextfile.FullName); //给创建的单元格赋值string

                    ICell cell2 = null;
                    cell2 = rowWrite.CreateCell(2);  //在index=j的行中创建index=0的单元格
                    cell2.SetCellValue("X:" + max_loc.X); //给创建的单元格赋值string

                    ICell cell3 = null;
                    cell3 = rowWrite.CreateCell(3);  //在index=j的行中创建index=0的单元格
                    cell3.SetCellValue("Y:" + max_loc.Y); //给创建的单元格赋值string

                    ICell cell4 = null;
                    cell4 = rowWrite.CreateCell(4);  //在index=j的行中创建index=0的单元格
                    cell4.SetCellValue(max.ToString("f2")); //给创建的单元格赋值string


                    ICell cell5 = null;
                    cell5 = rowWrite.CreateCell(5);  //在index=j的行中创建index=0的单元格
                    cell5.SetCellValue(min.ToString("f2")); //给创建的单元格赋值string

                    ICell cell6 = null;
                    cell6 = rowWrite.CreateCell(6);  //在index=j的行中创建index=0的单元格
                    cell6.SetCellValue(sum_area.ToString()); //给创建的单元格赋值string


                    //通过流将表中写入的数据一次性写入文件中;
                    wk.Write(filestream); //通过流filestream将表wk写入文件


                    //(add,20211231)
                    //filestream.Close(); //关闭文件流filestream
                    //wk.Close(); //关闭Excel表对象wk


                    Mat mask = contour_img.ToImage<Bgr, byte>().CopyBlank().Mat;//获取一张背景为黑色的图像，尺寸大小与contour_img一样(类型Bgr);
                    CvInvoke.DrawContours(mask, use_contours, -1, new MCvScalar(0, 0, 255));//采用红色画笔在mask掩膜图像中画出所有轮廓；(MCvScalar是一个具有单元素到四元素间的一个数组)
                    pictureBox6.Image = mask.ToImage<Bgr, byte>().ToBitmap();//显示轮廓区域图像；


                    //显示、保存图像；
                    #region
                    //CvInvoke.Imshow("img", temp); //显示图片
                    CvInvoke.Imwrite(@"test/mask_result/" + Path.GetFileName(nextfile.FullName), mask); //保存匹配结果图像；
                    CvInvoke.WaitKey(0); //暂停按键等待
                    #endregion

                /********************自动模式下遍历文件夹---8-进行二值化轮廓筛选及面积运算(2022-0111--end)******************************************/
                /******************* (add-2022-0111,轮廓运算--end）**************************************************************/
                /*********************相当于(BinarizedContour_Click--end)***********************************************************/



                /********************自动模式下---9-用流创建或读取.xlsx文件（同时流关联了文件）************************/
                /******************* (add-2022-0111,用流创建或读取.xlsx文件）*********/
                /*************(BinarizedContour_Click下));*******/
                /*******************自动模式下--end*****************************************/
            } //add(2022-0110，和前面遍历文件foreach (FileInfo nextfile in folder.GetFiles())中的'{'相对应);
        
            #endregion            
        }

        }
    }


            //add(1215,矩形、轮廓、圆形、椭圆)
            #region
            //    Contour<Point> ContourBufPre = image3.FindContours(ChainApproxMethod.ChainApproxSimple, Emgu.CV.CvEnum.RetrType.External);//寻找轮廓

            //    image4.Draw(ContourBufPre, new Bgr(0, 0, 255), new Bgr(255, 0, 255), 2, 1, new Point(0, 0));
            //    //矩形
            //    image4.Draw(ContourBufPre.BoundingRectangle, new Bgr(0, 255, 255), 1);
            //    //最小矩形
            //    MCvBox2D box = ContourBufPre.HNext.GetMinAreaRect();
            //    PointF[] pointfs = box.GetVertices();
            //    Point[] ps = new Point[pointfs.Length];
            //    for (int i = 0; i < pointfs.Length; i++)
            //    {
            //        ps[i] = new Point((int)pointfs[i].X, (int)pointfs[i].Y);
            //        //image4.Draw(new LineSegment2D(ps[i], ps[(i + 1) % 4]), new Bgr(0, 0, 255), 1);
            //    }
            //    //for (int i = 0; i < pointfs.Length; i++)
            //    //{
            //    //    //ps[i] = new Point((int)pointfs[i].X, (int)pointfs[i].Y);
            //    //    image4.Draw(new LineSegment2D(ps[i], ps[(i + 1) % 4]), new Bgr(0, 0, 255), 1);

            //    //}
            //    image4.DrawPolyline(ps, true, new Bgr(0, 0, 255), 1);
            //    //圆形
            //    PointF center;
            //    float radius;
            //    CvInvoke.cvMinEnclosingCircle(ContourBufPre.Ptr, out center, out radius);
            //    image4.Draw(new CircleF(center, radius), new Bgr(255, 0, 255), 2);
            //    //椭圆               
            //    if (ContourBufPre.Total >= 6) //轮廓点数小于6，不能创建外围椭圆
            //    {
            //        MCvBox2D box1 = CvInvoke.cvFitEllipse2(ContourBufPre.Ptr);
            //        image4.Draw(new Ellipse(box), new Bgr(255, 255, 255), 2);
            //    }
            //    pictureBox3.Image = image4.ToBitmap();
            //}
            #endregion

   
        
            //遍历各连通域，获得连通域的矩；
            #region
            //for (int i = 0; i < contours.Size; i++) //遍历每个连通域；
            //{

            //    VectorOfPoint contour = contours[i];
            //    MCvMoments moment = CvInvoke.Moments(contour);//获得连通域的矩；
            //    Point p = new Point((int)(moment.M10 / moment.M00), (int)(moment.M01 / moment.M00));//获得连通域的中心位置；
            //    CvInvoke.Rectangle(contours, new Rectangle(p, img1.Size), new MCvScalar(0, 0, 255), 4);//绘制匹配区域；
            //}
            #endregion

    
            #endregion
            #endregion
            #endregion