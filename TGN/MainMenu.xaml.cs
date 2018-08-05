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
using System.Windows.Shapes;
using TGN.Model.Logic;
using TGN.Model.Repository;

namespace TGN
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window 
    {
        public MainMenu()
        {
            InitializeComponent(); 

        }

        private void ClosedForm(object sender, EventArgs e)
        {
            if (DataRepository.OpenMw == false)
                DataRepository.mainWindow.Close();
        }
        private void MainMenuForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataRepository.SaveFileIsExist()) 
            {
                LoadGameBtn.IsEnabled = true;
            }
        }


        private void NewGame_Click(object sender, RoutedEventArgs e) 
        {
            GameLogic.NewGame();
            DataRepository.OpenMw = true;
            this.Close();
        }
        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            GameLogic.LoadGame();
            DataRepository.OpenMw = true;
            this.Close();
        } 
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
