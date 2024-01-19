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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private System.Timers.Timer mTimer;
        private delegate void ProcessTimerCB();
        private delegate void SaveStateCB();

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            mTimer = new System.Timers.Timer();
            mTimer.AutoReset = false;
            mTimer.Interval = 2000;
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(mTimer_Elapsed);
            mTimer.Start();


        }



        private void mTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs args)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new ProcessTimerCB(ProcessTimer));
        }
        private void ProcessTimer()
        {
            mTimer.Stop();

            Browser.Navigate("https://localhost:7067/");

        }
    }
}
