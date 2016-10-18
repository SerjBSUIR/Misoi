using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        Bitmap bitmap, bitmap2;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadImage(dialog.FileName);
            }
            //this.bitmap = new Bitmap(Image.FromFile(@"C:\Users\inss\Documents\1234.jpg"));
            //PaintImage1();
        }

        private void LoadImage(string FileName)
        {
            this.bitmap = new Bitmap(Image.FromFile(FileName));
            PaintImage1();
        }

        private void PaintImage1()
        {
            this.pictureBox1.Image = this.bitmap;
            //OnLoadImage();
        }

        private void PaintImage2()
        {
            this.pictureBox2.Image = this.bitmap2;
            OnLoadImage();
        }

        private void ToGrayButton_Click(object sender, EventArgs e)
        {
            this.bitmap2 = ImageProcessing.MakeGrayscale(this.bitmap);
            PaintImage2();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int gMin = Convert.ToInt32(textBox1.Text);
            int gMax = Convert.ToInt32(textBox2.Text);
            this.bitmap2 = ImageProcessing.LineContr(this.bitmap, gMin, gMax);
            PaintImage2();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.bitmap2 = ImageProcessing.LFilter(this.bitmap);
            PaintImage2();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.bitmap2 = ImageProcessing.LFilterGray(this.bitmap);
            PaintImage2();
        }

        private void OnLoadImage()
        {
            int[] xValues = new int[256];
            for (int x = 0; x < 256; x++)
            {   
                xValues[x] = x;
            }
            BrightnessDistribution BrightnessDistribution = ImageProcessing.CalculateBrightness(this.bitmap2);
            this.chart1.Series["R"].Points.DataBindXY(xValues, BrightnessDistribution.R);
            this.chart1.Series["G"].Points.DataBindXY(xValues, BrightnessDistribution.G);
            this.chart1.Series["B"].Points.DataBindXY(xValues, BrightnessDistribution.B);
            this.chart1.Series["S"].Points.DataBindXY(xValues, BrightnessDistribution.Sum);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int gMin = Convert.ToInt32(textBox1.Text);
            int gMax = Convert.ToInt32(textBox2.Text);
            this.bitmap2 = ImageProcessing.LineContrGray(this.bitmap, gMin, gMax);
            PaintImage2();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.bitmap2.Save(dialog.FileName);
            }
        }

               
    }
}
