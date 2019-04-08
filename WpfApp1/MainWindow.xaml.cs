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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int[] nums = new int[Convert.ToInt32(groupsBox.Text)];
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
            }
            showResults(nums);
        }

        private int[] Simpson()
        {
            List<double> randoms1 = new List<double>();
            double x = 0.5;
            Random r = new Random();
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                randoms1.Add(r.NextDouble()+ r.NextDouble() + x);
            }
            int[] nums = sortNums(randoms1);
            return nums;
        }

        private int[] congr()
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
            int[] nums = sortNums(randoms);
            return nums;
        }

        private int[] Lemer()
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
            int[] nums = sortNums(randoms);
            return nums;
        }

        private int[] innerFunction()
        {
            List<double> randoms = new List<double>();
            Random r = new Random();
            for (int i = 0; i < Convert.ToInt32(NumBox.Text); i++)
            {
                randoms.Add(r.NextDouble());
            }
            int[] nums = sortNums(randoms);
            return nums;
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
                        nums[i-1]++;
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
            foreach(var cur in nums)
            {
                N++;
                Rectangle foo = new Rectangle()
                {
                    Width = canv.Width/Convert.ToInt32(groupsBox.Text),
                    Height = (double)cur/(double)nums.Max()*canv.Height,
                    Fill = Brushes.LightGray,
                    StrokeThickness = 1,
                };
                canv.Children.Add(foo);
                Canvas.SetLeft(foo, (double)5 + (canv.Width / Convert.ToInt32(groupsBox.Text)*(N-1)));
                Canvas.SetBottom(foo, (double)10);
            }
        }

        private void WindowM_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canv.Width = windowM.Width - 14;
            canv.Height = windowM.Height - 143;
            
        }
    }
}
