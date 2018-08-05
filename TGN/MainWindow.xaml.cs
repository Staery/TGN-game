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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using TGN.Model.Logic;
using TGN.Model.Repository;



namespace TGN
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button[] GoosesBtn;
        private Image[] GoosesImg;

        public int SelectedUnit;
      
        public static bool IsOpen = false;
        private static UpgradeForm UF;

        public MainWindow()
        {
            InitializeComponent();
            //LbSW.Visibility = Visibility.Hidden; //отладка
        }
        private void ClosingForm(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(UF != null)
                UF.Close();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            GoosesImg = new Image[100]
            {
                ImgGoose0, ImgGoose1, ImgGoose2, ImgGoose3, ImgGoose4, ImgGoose5, ImgGoose6, ImgGoose7, ImgGoose8, ImgGoose9, ImgGoose10,
                ImgGoose11, ImgGoose12, ImgGoose13, ImgGoose14, ImgGoose15, ImgGoose16, ImgGoose17, ImgGoose18, ImgGoose19, ImgGoose20,
                ImgGoose21, ImgGoose22, ImgGoose23, ImgGoose24, ImgGoose25, ImgGoose26, ImgGoose27, ImgGoose28, ImgGoose29, ImgGoose30,
                ImgGoose31, ImgGoose32, ImgGoose33, ImgGoose34, ImgGoose35, ImgGoose36, ImgGoose37, ImgGoose38, ImgGoose39, ImgGoose40,
                ImgGoose41, ImgGoose42, ImgGoose43, ImgGoose44, ImgGoose45, ImgGoose46, ImgGoose47, ImgGoose48, ImgGoose49, ImgGoose50,
                ImgGoose51, ImgGoose52, ImgGoose53, ImgGoose54, ImgGoose55, ImgGoose56, ImgGoose57, ImgGoose58, ImgGoose59, ImgGoose60,
                ImgGoose61, ImgGoose62, ImgGoose63, ImgGoose64, ImgGoose65, ImgGoose66, ImgGoose67, ImgGoose68, ImgGoose69, ImgGoose70, 
                ImgGoose71, ImgGoose72, ImgGoose73, ImgGoose74, ImgGoose75, ImgGoose76, ImgGoose77, ImgGoose78, ImgGoose79, ImgGoose80,
                ImgGoose81, ImgGoose82, ImgGoose83, ImgGoose84, ImgGoose85, ImgGoose86, ImgGoose87, ImgGoose88, ImgGoose89, ImgGoose90,
                ImgGoose91, ImgGoose92, ImgGoose93, ImgGoose94, ImgGoose95, ImgGoose96, ImgGoose97, ImgGoose98, ImgGoose99
            };
            GoosesBtn = new Button[100] 
            { 
                Goose0, Goose1, Goose2, Goose3, Goose4, Goose5, Goose6, Goose7, Goose8, Goose9, Goose10,
                Goose11, Goose12, Goose13, Goose14, Goose15, Goose16, Goose17, Goose18, Goose19, Goose20,
                Goose21, Goose22, Goose23, Goose24, Goose25, Goose26, Goose27, Goose28, Goose29, Goose30,
                Goose31, Goose32, Goose33, Goose34, Goose35, Goose36, Goose37, Goose38, Goose39, Goose40,
                Goose41, Goose42, Goose43, Goose44, Goose45, Goose46, Goose47, Goose48, Goose49, Goose50,
                Goose51, Goose52, Goose53, Goose54, Goose55, Goose56, Goose57, Goose58, Goose59, Goose60,
                Goose61, Goose62, Goose63, Goose64, Goose65, Goose66, Goose67, Goose68, Goose69, Goose70,
                Goose71, Goose72, Goose73, Goose74, Goose75, Goose76, Goose77, Goose78, Goose79, Goose80,
                Goose81, Goose82, Goose83, Goose84, Goose85, Goose86, Goose87, Goose88, Goose89, Goose90,
                Goose91, Goose92, Goose93, Goose94, Goose95, Goose96, Goose97, Goose98, Goose99 
            };

            GoosesImg[0].Source = DataRepository.images.LeaderUnit;
            GoosesImg[1].Source = DataRepository.images.CommonUnit;

            DataRepository.mainWindow = this;

            this.Visibility = Visibility.Collapsed;
            MainMenu MM = new MainMenu();
            MM.Show();
        }

        public void UpdateForStartGUI()
        {
            this.Visibility = Visibility.Visible;
            LbCountGooses.Content = DataRepository.stringsForLb.Units + (GameLogic.gameData.player.UnitsCount).ToString();

            GoosesImg[0].Source = DataRepository.images.LeaderUnit;
            GoosesBtn[0].Visibility = Visibility.Visible;
            for (int i = 1; i < GameLogic.gameData.player.UnitsCount; i++) 
            {
                GoosesBtn[i].Visibility = Visibility.Visible;
                GoosesImg[i].Source = DataRepository.images.CommonUnit;
            }
            if (GameLogic.gameData.player.UnitsCount < 100) 
            {
                GoosesBtn[GameLogic.gameData.player.UnitsCount].Visibility = Visibility.Visible;
                GoosesImg[GameLogic.gameData.player.UnitsCount].Source = DataRepository.images.AddUnits;
            }

            GiveLb(DataRepository.stringsForLb.CostUnit, GameLogic.gameData.player.CostBuyNextUnit.ToString(), LbCostGooses);

            // --- Achievements --- //
            DPSLb.Content = DataRepository.stringsForLb.DPS + GameLogic.gameData.achievements.DPS.ToString();
            MoneyLb.Content = DataRepository.stringsForLb.Money + GameLogic.gameData.achievements.Money.ToString();
        }

        public void GiveLb(string Name, string Num, Label lb) 
        {
            lb.Content = Name + DataRepository.NumSeparation(Num);
        }
        public void GivePB(double MaxHp, double CurHp, ProgressBar pg,Label lb)
        {
            pg.Maximum = MaxHp;
            pg.Value = CurHp;
            lb.Content = DataRepository.NumSeparation(CurHp.ToString())
                + "/" + DataRepository.NumSeparation(MaxHp.ToString());
        }
        public void GiveCurWave(Border BImg, ImageBrush IB) 
        {
            BImg.Background = IB;
        }

        private void Unit_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 100; i++ )
                if (e.OriginalSource == GoosesBtn[i]) 
                {
                    if (GameLogic.gameData.player.UnitsCount <= i)
                    {
                        if (GameLogic.gameData.player.Money >= GameLogic.gameData.player.CostBuyNextUnit)
                        {
                            GameLogic.gameData.player.BuyNewUnit();
                            if (i != 99)
                            {
                                GoosesBtn[i + 1].Visibility = Visibility.Visible;
                                GoosesImg[i + 1].Source = DataRepository.images.AddUnits;
                            }

                            GoosesImg[i].Source = DataRepository.images.CommonUnit;
                            LbCountGooses.Content = DataRepository.stringsForLb.Units + (GameLogic.gameData.player.UnitsCount).ToString();

                            GiveLb(DataRepository.stringsForLb.CostUnit, GameLogic.gameData.player.CostBuyNextUnit.ToString(), LbCostGooses);
                        } 
                    }
                    else 
                    {
                        SelectedUnit = i;
                        if (UF != null)
                            UF.Close();
                        UF = new UpgradeForm(i);
                        UF.Show();
                    }
                    break;
                }                    
        }
    }
}