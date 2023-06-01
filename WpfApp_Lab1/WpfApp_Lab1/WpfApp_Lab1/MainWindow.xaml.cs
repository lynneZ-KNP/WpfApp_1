using System;
using System.Collections.Generic;
using System.Globalization;
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
using DataLibrary;

namespace WpfApp_Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand FromControlsCommand =
             new RoutedCommand("FromControlsCommand", typeof(WpfApp_Lab1.MainWindow));
        ViewData viewData = new ViewData();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewData;
            comboBox_FEnum.ItemsSource = Enum.GetValues(typeof(FRawEnum));
        }

        private void CanFromControlsCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (textBox_LeftEnd == null || Validation.GetHasError(textBox_LeftEnd) ||
                textBox_RightEnd == null || Validation.GetHasError(textBox_RightEnd) ||
                textBox_nRawItems == null || Validation.GetHasError(textBox_nRawItems)) e.CanExecute = false;
            else e.CanExecute = true;
        }

        private void FromControlsCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            viewData.Execute();
            DataContext = null;
            DataContext = viewData;
        }

        private void Button_TestMKL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SplineData splineData = new SplineData();
                splineData.Test();
                MessageBox.Show(splineData.test_str);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }

        private void ToSave(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "RawData";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                viewData.Save(filename);
            }
        }

        public class RawDataToStringConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is RawData rawData)
                {
                    List<GridValue> gridValues = new List<GridValue>();
                    for (int i = 0; i < rawData.N_points; i++)
                    {
                        GridValue gridValue = new()
                        {
                            Point = rawData.Points[i],
                            Value = rawData.Values[i]
                        };
                        gridValues.Add(gridValue);
                    }

                    return gridValues;
                }
                return "";
            }

            public class SplineDataItemTostringConverter : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    if (value is SplineDataItem sItem)
                    {
                        return $"Point: {sItem.Point_counted:F2}\nSplineValue: {sItem.SplineValue:F2}\nFirstDerivative: {sItem.FirstDerivative:F2}\nSecondDerivative: {sItem.SecondDerivative:F2}";
                    }
                    return Binding.DoNothing;
                }

                public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    throw new NotImplementedException();
                }
            }

        }
    }
  }
