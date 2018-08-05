using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TGN
{
    /// <summary>
    /// Логика взаимодействия для ResultGrind.xaml 
    /// </summary>
    public delegate void CloseForm();
    public partial class ResultGrind : Window 
    {
        Thread ThToClose;
        CloseForm CF;
        public ResultGrind(int[] RusultG)
        {
            InitializeComponent();

            AttemptsGrind.Content = RusultG[0];
            AttemptsSuccess.Content = RusultG[1];
            AttemptsFail.Content = RusultG[2];

            CF = CloseThis;
            ThToClose = new Thread(WaitAndCloseForm); 
            ThToClose.Start();
        }
        public void CloseThis() 
        {
            Close();
        }
        private void WaitAndCloseForm()
        {
            Thread.Sleep(4000);
            Dispatcher.BeginInvoke(CF);
        }
    }
}
