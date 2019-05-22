using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int[] numsP;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<double> nums = new List<double>();
            switch (cBox.SelectedIndex)
            {
                case 0:
                    nums = innerFunction();
                    break;
                case 1:
                    nums = congr();
                    break;
                case 2:
                    nums = Lemer();
                    break;
                case 3:
                    nums = Simpson();
                    break;
                case 4:
                    nums = Gauss();
                    break;
                case 5:
                    nums = Exponent(Convert.ToDouble(additionalBox1.Text));
                    break;
                case 6:
                    nums = HyperExponent();
                    break;
                case 7:
                    nums = Erlang();
                    break;
                case 8:
                    nums = Normal();
                    break;
            }
            var sorts = sortNums(nums);
            numsP = sorts;
            showResults(sorts);
        }

        private List<double> Gauss()
        {
            List<double> randoms = new List<double>();
            NormalRandom nr = new NormalRandom();
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                randoms.Add((nr.NextDouble()*Convert.ToDouble(additionalBox1.Text)) + Convert.ToDouble(additionalBox2.Text));
            }
            return randoms;
        }

        private List<double> Normal()
        {
            List<double> randoms = new List<double>();
            Random random = new Random();
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                randoms.Add(SampleGaussian(random, Convert.ToDouble(additionalBox1.Text), Convert.ToDouble(additionalBox2.Text)));
            }
            return randoms;
        }

        //private List<double> Normal()
        //{
        //    for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
        //    {
        //        Random rand = new Random(); //reuse this if you are generating many
        //        double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
        //        double u2 = 1.0 - rand.NextDouble();
        //        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
        //                     Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
        //        double randNormal =
        //                     mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        //    }
        //}
        //public void GaussMethod(double[] massive, double mu, double sigma, int num)
        //{
        //    double dSumm = 0, dRandValue = 0;
        //    Random ran = new Random();
        //    for (int n = 0; n <= num; n++)
        //    {
        //        dSumm = 0;
        //        for (int i = 0; i <= 12; i++)
        //        {
        //            double R = ran.NextDouble();
        //            dSumm = dSumm + R;
        //        }
        //        dRandValue = Math.Round((mu + sigma * (dSumm - 6)), 3);
        //        massive[n] = dRandValue;
        //    }

        //}
        public static double SampleGaussian(Random random, double mean, double stddev)
        {
           // Random random = new Random();
            // The method requires sampling from a uniform random of (0,1]
            // but Random.NextDouble() returns a sample of [0,1).
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 * stddev + mean;
        }

        private List<double> Exponent(double l)
        {
            List<double> randoms = new List<double>();
            //var l = Convert.ToDouble(additionalBox1.Text);
            Random random = new Random();
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                randoms.Add(((-1d / l) * Math.Log(random.NextDouble())));
            }
            //var min = randoms.Min();
            //var max = randoms.Max();
            return randoms;
        }

        private List<double> HyperExponent()
        {
            List<double> randoms = new List<double>();
            randoms = Exponent(Convert.ToDouble(additionalBox1.Text));
            List<double> randoms2 = new List<double>();
            randoms2 = Exponent(Convert.ToDouble(additionalBox2.Text));
            for (int i = 0; i < randoms.Count; i++) 
            {
                randoms[i] += randoms2[i];
            }
            return randoms;
        }

        private List<double> Erlang()
        {
            List<double> randoms = new List<double>();
            randoms = Exponent(Convert.ToDouble(additionalBox1.Text));
            List<double> randoms2 = new List<double>();
            for (int j = 0; j < Convert.ToInt32(additionalBox2.Text); j++)
            {
                randoms2 = Exponent(Convert.ToDouble(additionalBox1.Text));
                for (int i = 0; i < randoms.Count; i++)
                {
                    randoms[i] += randoms2[i];
                }
            }
            return randoms;
        }

        class NormalRandom : Random
        {
            double prevSample = double.NaN;
            protected override double Sample()
            {
                if (!double.IsNaN(prevSample))
                {
                    double result = prevSample;
                    prevSample = double.NaN;
                    return result;
                }
                double u, v, s;
                do
                {
                    u = 2 * base.Sample() - 1;
                    v = 2 * base.Sample() - 1;
                    s = u * u + v * v;
                }
                while (u <= -1 || v <= -1 || s >= 1 || s == 0);
                double r = Math.Sqrt(-2 * Math.Log(s) / s);

                prevSample = r * v;
                return r * u;
            }
        }

        private List<double> Simpson()
        {
            List<double> randoms = new List<double>();
            Random r = new Random();
            int fMin = Convert.ToInt32(additionalBox1.Text);
            int fMax = Convert.ToInt32(additionalBox2.Text);
            int sMin = Convert.ToInt32(additionalBox3.Text);
            int sMax = Convert.ToInt32(additionalBox4.Text);
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                randoms.Add(r.Next(fMin,fMax));
            }

            List<double> randoms2 = new List<double>();
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                randoms2.Add(r.Next(sMin,sMax));
            }
            for (int i = 0; i< randoms.Count; i++)
                randoms[i] += randoms2[i];
            return randoms;
        }

        private List<double> congr()
        {
            List<double> randoms = new List<double>();
            double Xi = DateTime.Now.Second;
            double A = DateTime.Now.Hour * DateTime.Now.Minute;
            double Xn = 0;
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                Xn = (A * Xi) % 9973;
                randoms.Add(Xn);
                Xi = Xn;
            }
            return randoms;
        }

        private List<double> Lemer()
        {
            List<double> randoms = new List<double>();
            double Xi = DateTime.Now.Second;
            double A = 42135815;
            double c = 13542135;
            double m = 4257287;
            double Xn = 0;
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                Xn = (A * Xi + c) % m;
                randoms.Add(Xn);
                Xi = Xn;
            }
            return randoms;
        }

        private List<double> innerFunction()
        {
            List<double> randoms = new List<double>();
            Random r = new Random();
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                randoms.Add(r.NextDouble());
            }
            return randoms;
        }

        private int[] sortNums(List<double> unsortedNums)
        {
            var groups = Convert.ToInt32(groupsBox.Text);
            int[] nums = new int[groups];
            double max = unsortedNums.Max();
            double N = (double)1/groups;
            foreach (var cur in unsortedNums)
            {
                N = 1 / groups;
                for (int i = 0; i < groups; i++)
                {
                    if (cur <= N * max)
                    {
                        nums[i]++;
                        break;
                    }
                    N += (double)1 / groups;
                }
            }
            return nums;
        }

        private void showResults(int[] nums)
        {
            canv.Children.Clear();
            int N = 0;
            double wid = canv.Width / Convert.ToInt32(groupsBox.Text);
            //if (wid < 2) wid = 2;
            foreach (var cur in nums)
            {
                N++;
                Rectangle foo = new Rectangle()
                {
                    Width = wid,
                    Height = (double)cur/(double)nums.Max()*canv.Height,
                    Fill = Brushes.LightGray,
                    StrokeThickness = 1,
                };
                canv.Children.Add(foo);
                Canvas.SetLeft(foo, (double)5 + (wid*(N-1)));
                Canvas.SetBottom(foo, (double)10);
            }
        }

        private void WindowM_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canv.Width = windowM.Width - 14;
            canv.Height = windowM.Height - 143;
            try
            {
                showResults(numsP);
            }
            catch { }
        }

        private void CBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cBox.SelectedIndex)
            {
                case 0:
                    additionalBox1.Visibility = Visibility.Hidden;
                    additionalBox2.Visibility = Visibility.Hidden;
                    additionalBox3.Visibility = Visibility.Hidden;
                    additionalBox4.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    additionalBox1.Visibility = Visibility.Hidden;
                    additionalBox2.Visibility = Visibility.Hidden;
                    additionalBox3.Visibility = Visibility.Hidden;
                    additionalBox4.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    additionalBox1.Visibility = Visibility.Hidden;
                    additionalBox2.Visibility = Visibility.Hidden;
                    additionalBox3.Visibility = Visibility.Hidden;
                    additionalBox4.Visibility = Visibility.Hidden;
                    break;
                case 3:
                    additionalBox1.Visibility = Visibility.Visible;
                    additionalBox2.Visibility = Visibility.Visible;
                    additionalBox3.Visibility = Visibility.Visible;
                    additionalBox4.Visibility = Visibility.Visible;
                    break;
                case 4:
                    additionalBox1.Visibility = Visibility.Visible;
                    additionalBox2.Visibility = Visibility.Visible;
                    additionalBox3.Visibility = Visibility.Hidden;
                    additionalBox4.Visibility = Visibility.Hidden;
                    break;
                case 5:
                    additionalBox1.Visibility = Visibility.Visible;
                    additionalBox2.Visibility = Visibility.Hidden;
                    additionalBox3.Visibility = Visibility.Hidden;
                    additionalBox4.Visibility = Visibility.Hidden;
                    break;
                case 6:
                    additionalBox1.Visibility = Visibility.Visible;
                    additionalBox2.Visibility = Visibility.Visible;
                    additionalBox3.Visibility = Visibility.Hidden;
                    additionalBox4.Visibility = Visibility.Hidden;
                    break;
                case 7:
                    additionalBox1.Visibility = Visibility.Visible;
                    additionalBox2.Visibility = Visibility.Visible;
                    additionalBox3.Visibility = Visibility.Hidden;
                    additionalBox4.Visibility = Visibility.Hidden;
                    break;
                case 8:
                    additionalBox1.Visibility = Visibility.Visible;
                    additionalBox2.Visibility = Visibility.Visible;
                    additionalBox3.Visibility = Visibility.Hidden;
                    additionalBox4.Visibility = Visibility.Hidden;
                    break;
            }
        }
    }
}
