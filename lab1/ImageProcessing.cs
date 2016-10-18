using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class ImageProcessing
    {
        public static BrightnessDistribution CalculateBrightness(Bitmap bitmap)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;
            BrightnessDistribution result = new BrightnessDistribution();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result.R[bitmap.GetPixel(x, y).R]++;
                    result.G[bitmap.GetPixel(x, y).G]++;
                    result.B[bitmap.GetPixel(x, y).B]++;
                    result.Sum[GetAverageBrightness(bitmap, x, y)]++;
                }
            }
            return result;
        }

        private static int GetAverageBrightness(Bitmap bitmap, int x, int y)
        {
            return (int) (0.59 * bitmap.GetPixel(x, y).R + 0.3 * bitmap.GetPixel(x, y).G + 0.11 * bitmap.GetPixel(x, y).B);
        }

        public static Bitmap MakeGrayscale(Bitmap bitmap)
        {
            Bitmap result = new Bitmap(bitmap);
            int width = bitmap.Width;
            int height = bitmap.Height;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int averageBrightness = GetAverageBrightness(bitmap, x, y);
                    SetAverageBrightness(result, x, y, averageBrightness);
                }
            }
            return result;
        }

        private static void SetAverageBrightness(Bitmap bitmap, int x, int y, int averageBrightness)
        {
            bitmap.SetPixel(x, y, Color.FromArgb(averageBrightness, averageBrightness, averageBrightness));
        }

        public static Bitmap LineContr(Bitmap bitmap, int gMin, int gMax)
        {
            Bitmap result = new Bitmap(bitmap);
            int width = bitmap.Width;
            int height = bitmap.Height;
            int fMin = 500;
            int fMax = 0;
            double NewR;
            double NewG;
            double NewB;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int averageBrightness = GetAverageBrightness(bitmap, x, y);
                    if (averageBrightness > fMax)
                        fMax = averageBrightness;
                    if (averageBrightness < fMin)
                        fMin = averageBrightness;
                }
            }
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    /*int averageBrightness = GetAverageBrightness(bitmap, x, y);
                    averageBrightness = ((averageBrightness - fMin) / (fMax - fMin)) * (gMax - gMin) + gMin;
                    SetAverageBrightness(result, x, y, averageBrightness);*/

                    NewR = bitmap.GetPixel(x, y).R;
                    NewR = ((NewR - fMin) / (fMax - fMin)) * (gMax - gMin) + gMin;

                    NewG = bitmap.GetPixel(x, y).G;
                    NewG = ((NewG - fMin) / (fMax - fMin)) * (gMax - gMin) + gMin;

                    NewB = bitmap.GetPixel(x, y).B;
                    NewB = ((NewB - fMin) / (fMax - fMin)) * (gMax - gMin) + gMin;

                    if ((int)NewR < 0)
                        NewR = 0.0;
                    if ((int)NewR > 255)
                        NewR = 255.0;
                    if ((int)NewG < 0)
                        NewG = 0.0;
                    if ((int)NewG > 255)
                        NewG = 255.0;
                    if ((int)NewB < 0)
                        NewB = 0.0;
                    if ((int)NewB > 255)
                        NewB = 255.0;
                    var gamma = 0.8;
                    result.SetPixel(x, y, Color.FromArgb(ConvertColor(bitmap.GetPixel(x, y).R, GetAverageBrightness(bitmap, x, y), 1, gamma), ConvertColor(bitmap.GetPixel(x, y).G, GetAverageBrightness(bitmap, x, y), 1, gamma), ConvertColor(bitmap.GetPixel(x, y).B, GetAverageBrightness(bitmap, x, y), 1, gamma)));
                }
            }
            return result;
        }

        private static int ConvertColor(double color, int brightness, double a, double gamm)
        {
            var i = Convert.ToInt32(a*Math.Pow(color, gamm));
            return i>255? 255: i<0 ? 0 : i;
        }

        public static Bitmap LineContrGray(Bitmap bitmap, int gMin, int gMax)
        {
            Bitmap result = new Bitmap(bitmap);
            int width = bitmap.Width;
            int height = bitmap.Height;
            int fMin = 500;
            int fMax = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int averageBrightness = bitmap.GetPixel(x, y).R;//GetAverageBrightness(bitmap, x, y);
                    if (averageBrightness > fMax)
                        fMax = averageBrightness;
                    if (averageBrightness < fMin)
                        fMin = averageBrightness;
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double averageBrightness = bitmap.GetPixel(x, y).R;//GetAverageBrightness(bitmap, x, y);
                    averageBrightness = ((averageBrightness - fMin) / (fMax - fMin)) * (gMax - gMin) + gMin;
                    SetAverageBrightness(result, x, y, (int)averageBrightness);

                }
            }
            return result;
        }

        public static Bitmap LFilter(Bitmap bitmap)
        {
            Bitmap result = new Bitmap(bitmap);
            int width = bitmap.Width;
            int height = bitmap.Height;
            int NewR;
            int NewG;
            int NewB;

            for (int x = 1; x < width-1; x++)
            {
                for (int y = 1; y < height-1; y++)
                {
                    //int averageBrightness = GetAverageBrightness(bitmap, x, y);
                    //SetAverageBrightness(result, x, y, averageBrightness);

                    NewR = bitmap.GetPixel(x, y).R;
                    NewR = (bitmap.GetPixel(x - 1, y - 1).R + bitmap.GetPixel(x, y - 1).R + bitmap.GetPixel(x + 1, y - 1).R + bitmap.GetPixel(x - 1, y).R + bitmap.GetPixel(x, y).R + bitmap.GetPixel(x + 1, y).R + bitmap.GetPixel(x - 1, y + 1).R + bitmap.GetPixel(x, y + 1).R + bitmap.GetPixel(x + 1, y + 1).R) / 9;

                    NewG = bitmap.GetPixel(x, y).G;
                    NewG = (bitmap.GetPixel(x - 1, y - 1).G + bitmap.GetPixel(x, y - 1).G + bitmap.GetPixel(x + 1, y - 1).G + bitmap.GetPixel(x - 1, y).G + bitmap.GetPixel(x, y).G + bitmap.GetPixel(x + 1, y).G + bitmap.GetPixel(x - 1, y + 1).G + bitmap.GetPixel(x, y + 1).G + bitmap.GetPixel(x + 1, y + 1).G) / 9;

                    NewB = bitmap.GetPixel(x, y).B;
                    NewB = (bitmap.GetPixel(x - 1, y - 1).B + bitmap.GetPixel(x, y - 1).B + bitmap.GetPixel(x + 1, y - 1).B + bitmap.GetPixel(x - 1, y).B + bitmap.GetPixel(x, y).B + bitmap.GetPixel(x + 1, y).B + bitmap.GetPixel(x - 1, y + 1).B + bitmap.GetPixel(x, y + 1).B + bitmap.GetPixel(x + 1, y + 1).B) / 9;

                    result.SetPixel(x, y, Color.FromArgb(NewR, NewG, NewB));
                }
            }


            return result;
        }

        public static Bitmap LFilterGray(Bitmap bitmap)
        {
            Bitmap result = new Bitmap(bitmap);
            int width = bitmap.Width;
            int height = bitmap.Height;
          

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    int averageBrightness = GetAverageBrightness(bitmap, x, y);
                    averageBrightness = (GetAverageBrightness(bitmap, x - 1, y - 1) + GetAverageBrightness(bitmap, x, y - 1) + GetAverageBrightness(bitmap, x + 1, y - 1) + GetAverageBrightness(bitmap, x - 1, y) + GetAverageBrightness(bitmap, x, y) + GetAverageBrightness(bitmap, x + 1, y) + GetAverageBrightness(bitmap, x - 1, y + 1) + GetAverageBrightness(bitmap, x, y + 1) + GetAverageBrightness(bitmap, x + 1, y + 1)) / 9;
                    SetAverageBrightness(result, x, y, averageBrightness);

                    //NewR = bitmap.GetPixel(x, y).R;
                    //NewR = (bitmap.GetPixel(x - 1, y - 1).R + bitmap.GetPixel(x, y - 1).R + bitmap.GetPixel(x + 1, y - 1).R + bitmap.GetPixel(x - 1, y).R + bitmap.GetPixel(x, y).R + bitmap.GetPixel(x + 1, y).R + bitmap.GetPixel(x - 1, y + 1).R + bitmap.GetPixel(x, y + 1).R + bitmap.GetPixel(x + 1, y + 1).R) / 9;

                    //NewG = bitmap.GetPixel(x, y).G;
                    //NewG = (bitmap.GetPixel(x - 1, y - 1).G + bitmap.GetPixel(x, y - 1).G + bitmap.GetPixel(x + 1, y - 1).G + bitmap.GetPixel(x - 1, y).G + bitmap.GetPixel(x, y).G + bitmap.GetPixel(x + 1, y).G + bitmap.GetPixel(x - 1, y + 1).G + bitmap.GetPixel(x, y + 1).G + bitmap.GetPixel(x + 1, y + 1).G) / 9;

                    //NewB = bitmap.GetPixel(x, y).B;
                    //NewB = (bitmap.GetPixel(x - 1, y - 1).B + bitmap.GetPixel(x, y - 1).B + bitmap.GetPixel(x + 1, y - 1).B + bitmap.GetPixel(x - 1, y).B + bitmap.GetPixel(x, y).B + bitmap.GetPixel(x + 1, y).B + bitmap.GetPixel(x - 1, y + 1).B + bitmap.GetPixel(x, y + 1).B + bitmap.GetPixel(x + 1, y + 1).B) / 9;

                    //result.SetPixel(x, y, Color.FromArgb(NewR, NewG, NewB));
                }
            }


            return result;
        }
    }
}
