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
            //int[] nums = new int[Convert.ToInt32(groupsBox.Text)];
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
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                randoms.Add(r.Next(400,500));
            }

            List<double> randoms2 = new List<double>();
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                randoms2.Add(r.Next(0,300));
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
            //int[] nums = sortNums(randoms);
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
            //int[] nums = sortNums(randoms);
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
            //int[] nums = sortNums(randoms);
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
    }
}
