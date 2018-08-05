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
using TGN.Model.Entities;
using TGN.Model.Logic;
using TGN.Model.Repository;

namespace TGN
{
    /// <summary>
    /// Логика взаимодействия для UpgradeForm.xaml
    /// </summary>
    public partial class UpgradeForm : Window
    {
       
        public UpgradeForm(int СhosenUnit_)
        {
            InitializeComponent();

            DataRepository.upgradeForm = this;
            SelectedUnit = СhosenUnit_;
            CurRuneNum = -1;

            UnitNumLb.Content = DataRepository.stringsForLb.UnitName + (SelectedUnit + 1);

            UpdateStatsUnit();

            RunesBtn = new Button[100]
            {
                Rune0, Rune1, Rune2, Rune3, Rune4, Rune5, Rune6, Rune7, Rune8, Rune9, Rune10,
                Rune11, Rune12, Rune13, Rune14, Rune15, Rune16, Rune17, Rune18, Rune19, Rune20,
                Rune21, Rune22, Rune23, Rune24, Rune25, Rune26, Rune27, Rune28, Rune29, Rune30,
                Rune31, Rune32, Rune33, Rune34, Rune35, Rune36, Rune37, Rune38, Rune39, Rune40,
                Rune41, Rune42, Rune43, Rune44, Rune45, Rune46, Rune47, Rune48, Rune49, Rune50,
                Rune51, Rune52, Rune53, Rune54, Rune55, Rune56, Rune57, Rune58, Rune59, Rune60,
                Rune61, Rune62, Rune63, Rune64, Rune65, Rune66, Rune67, Rune68, Rune69, Rune70,
                Rune71, Rune72, Rune73, Rune74, Rune75, Rune76, Rune77, Rune78, Rune79, Rune80,
                Rune81, Rune82, Rune83, Rune84, Rune85, Rune86, Rune87, Rune88, Rune89, Rune90, 
                Rune91, Rune92, Rune93, Rune94, Rune95, Rune96, Rune97, Rune98, Rune99
            };
            RunesImg = new Image[100]
            {
                ImgRune0, ImgRune1, ImgRune2, ImgRune3, ImgRune4, ImgRune5, ImgRune6, ImgRune7, ImgRune8, ImgRune9, ImgRune10,
                ImgRune11, ImgRune12, ImgRune13, ImgRune14, ImgRune15, ImgRune16, ImgRune17, ImgRune18, ImgRune19, ImgRune20,
                ImgRune21, ImgRune22, ImgRune23, ImgRune24, ImgRune25, ImgRune26, ImgRune27, ImgRune28, ImgRune29, ImgRune30,
                ImgRune31, ImgRune32, ImgRune33, ImgRune34, ImgRune35, ImgRune36, ImgRune37, ImgRune38, ImgRune39, ImgRune40,
                ImgRune41, ImgRune42, ImgRune43, ImgRune44, ImgRune45, ImgRune46, ImgRune47, ImgRune48, ImgRune49, ImgRune50,
                ImgRune51, ImgRune52, ImgRune53, ImgRune54, ImgRune55, ImgRune56, ImgRune57, ImgRune58, ImgRune59, ImgRune60,
                ImgRune61, ImgRune62, ImgRune63, ImgRune64, ImgRune65, ImgRune66, ImgRune67, ImgRune68, ImgRune69, ImgRune70,
                ImgRune71, ImgRune72, ImgRune73, ImgRune74, ImgRune75, ImgRune76, ImgRune77, ImgRune78, ImgRune79, ImgRune80,
                ImgRune81, ImgRune82, ImgRune83, ImgRune84, ImgRune85, ImgRune86, ImgRune87, ImgRune88, ImgRune89, ImgRune90,
                ImgRune91, ImgRune92, ImgRune93, ImgRune94, ImgRune95, ImgRune96, ImgRune97, ImgRune98, ImgRune99
            };
            RunesUnit = new Button[3]
            {
                GooseRuneSpd, GooseRuneAtk, GooseRuneProfit
            };
            RunesGImg = new Image[3]
            {
                ImgGRune0,ImgGRune1,ImgGRune2
            };

        }
        private void LoadForm(object sender, RoutedEventArgs e)
        {
            UpdateRuneImgs();
        }

        public int SelectedUnit;
        private int CurRuneNum;
        private Button[] RunesUnit;
        private Button[] RunesBtn;
        private Image[] RunesImg;
        private Image[] RunesGImg;

        //ResultGrind RG;

        public void UpdateRuneImgs() 
        {
            for (int i = 0; i < 100; i++)
            {
                if (GameLogic.gameData.player.StorgradeRunes.Count > i)
                    RunesImg[i].Source = DataRepository.images.RunesQuality[GameLogic.gameData.player.StorgradeRunes[i].Quality];
                else
                    RunesImg[i].Source = null;
            }


            for (int i = 0; i < 3; i++)
                if (GameLogic.gameData.player.Units[DataRepository.mainWindow.SelectedUnit].RuneSlot[i] != null)
                    RunesGImg[i].Source = DataRepository.images.RunesQuality[GameLogic.gameData.player.Units[DataRepository.mainWindow.SelectedUnit].RuneSlot[i].Quality];
                else
                    RunesGImg[i].Source = null;
            
        }
        private void Closing_Form(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DataRepository.upgradeForm = null;
        }
      

        private void UpgradeRune_Click(object sender, RoutedEventArgs e)
        {
            if (CurRuneNum != -1) 
            {
                int Attempts;
                if(Int32.TryParse(UpgradeRuneTb.Text, out Attempts))
                {
                    UpgradeRune(Attempts);
                }

            }
                
        }
        private void UpgradeRune(int AttemptsGrind)
        {
            if (GameLogic.UpgaradeRune(CurRuneNum, SelectedUnit, AttemptsGrind))
            {
                SeeRune(CurRuneNum);
                UpdateStatsUnit();
            }
        }

        private void UpgradeRuneTb_KeyUp(object sender, KeyEventArgs e)
        {
            if (UpgradeRuneTb.Text.Length > 5)
            {
                UpgradeRuneTb.Clear();
                UpgradeRuneTb.Text = "99999";
            }

            int Attempts;
            if (CurRuneNum != -1 && Int32.TryParse(UpgradeRuneTb.Text, out Attempts))
            {
                if (CurRuneNum < 100)
                {
                    UpdateRuneUpgradeCost(GameLogic.gameData.player.StorgradeRunes[CurRuneNum].LvlGrind);
                }
                else if (CurRuneNum >= 100 && GameLogic.gameData.player.Units[SelectedUnit].RuneSlot[CurRuneNum - 100] != null)
                {
                    UpdateRuneUpgradeCost(GameLogic.gameData.player.Units[SelectedUnit].RuneSlot[CurRuneNum - 100].LvlGrind);
                }
            }
            
        }
        private void EquipRune_Click(object sender, RoutedEventArgs e) 
        {
            if (CurRuneNum != -1 && CurRuneNum < 100) 
            {
                Rune rune = new Rune(GameLogic.gameData.player.StorgradeRunes[CurRuneNum]);
                GameLogic.gameData.PlayerEquipRune(SelectedUnit, CurRuneNum);

                RunesGImg[rune.Type].Source = DataRepository.images.RunesQuality[rune.Quality];
                GameLogic.gameData.UpdateplayersDPS();

                ResetSeeStatslbRune();
                UpdateRuneImgs();
                UpdateStatsUnit();
                CurRuneNum = -1;
            }
        }
        private void UnEquipRune_Click(object sender, RoutedEventArgs e) 
        {
            if (CurRuneNum != -1 && CurRuneNum >= 100) 
            {
                GameLogic.gameData.PlayerUnEquipRune(SelectedUnit, CurRuneNum - 100);
                UpdateRuneImgs();
                ResetSeeStatslbRune();
            }   
        } 
        private void SellRune_Click(object sender, RoutedEventArgs e)
        {
            if (CurRuneNum != -1) 
            {
                GameLogic.gameData.PlayerSellRune(CurRuneNum);
                UpdateRuneImgs();
                ResetSeeStatslbRune();
            }
        }
        private void SelectedRune_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 100; i++)
                if (e.OriginalSource == RunesBtn[i]) 
                    SeeRune(i);
                  
        }
        public void SelectedUnitRune_click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 3; i++)
                if (e.OriginalSource == RunesUnit[i])
                    SeeRune(i + 100);
        }



        private void UpdateRuneUpgradeCost(int LvlGrind) 
        {
            int Attempts;
            if (Int32.TryParse(UpgradeRuneTb.Text, out Attempts))
            {
                double UpgradeCost = GameLogic.CalculationCostUpgradeRune(LvlGrind, Int32.Parse(UpgradeRuneTb.Text));
                RuneUpCostLb.Content = DataRepository.stringsForLb.Cost + DataRepository.LineSeparation(UpgradeCost);
                if (GameLogic.gameData.player.Money < UpgradeCost)
                {
                    RuneUpCostLb.Foreground = new SolidColorBrush(Colors.DarkRed);
                }
                else
                {
                    RuneUpCostLb.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            else 
            {
                RuneUpCostLb.Foreground = new SolidColorBrush(Colors.Black);
                RuneUpCostLb.Content = DataRepository.stringsForLb.Cost + "0";
            }


        }

        private void SeeRune(int Num)
        {
            if (GameLogic.gameData.player.StorgradeRunes.Count > Num)
            {
                CurRuneNum = Num;
                Rune SeeRune = new Rune(GameLogic.gameData.player.StorgradeRunes[Num]);

                LbRuneName.Content = DataRepository.stringsForLb.TypeRuneStr[SeeRune.Type]
                    + " - (" + SeeRune.SpecialName + DataRepository.stringsForLb.Rune + ") +" + SeeRune.LvlGrind;

                LbRuneQuality.Content = DataRepository.stringsForLb.RuneQuality
                    + DataRepository.stringsForLb.QualityRuneStr[SeeRune.Quality];
                LbRuneMainStat.Content = DataRepository.stringsForLb.RuneMainStat
                    + SeeRune.MainStat;
                UpdateRuneUpgradeCost(SeeRune.LvlGrind);
                LbRuneSellCost.Content = SeeRune.CostSell;
            }
            else if (Num >= 100 && GameLogic.gameData.player.Units[SelectedUnit].RuneSlot[Num - 100] != null)
            {
                CurRuneNum = Num;
                Rune SeeRune = new Rune(GameLogic.gameData.player.Units[SelectedUnit].RuneSlot[Num - 100]);

                LbRuneName.Content = DataRepository.stringsForLb.TypeRuneStr[SeeRune.Type]
                    + " - (" + SeeRune.SpecialName + DataRepository.stringsForLb.Rune + ") +" + SeeRune.LvlGrind;

                LbRuneQuality.Content = DataRepository.stringsForLb.RuneQuality
                    + DataRepository.stringsForLb.QualityRuneStr[SeeRune.Quality];
                LbRuneMainStat.Content = DataRepository.stringsForLb.RuneMainStat
                    + SeeRune.MainStat;
                UpdateRuneUpgradeCost(SeeRune.LvlGrind);
                LbRuneSellCost.Content = SeeRune.CostSell;
            }
        }

        private void UpdateStatsUnit()
        {
            if (SelectedUnit == 0)
            {
                UnitImg.Source = DataRepository.images.LeaderUnit;
            }
            else
            {
                UnitImg.Source = DataRepository.images.CommonUnit;
            }

            LbSpdG.Content = GameLogic.CalculationSpd(GameLogic.gameData.player.Units[SelectedUnit]);
            LbAtkG.Content = GameLogic.CalculationAtk(GameLogic.gameData.player.Units[SelectedUnit]);
            LbProfitG.Content = GameLogic.CalculationProfit(GameLogic.gameData.player.Units[SelectedUnit]);
        }

        public void ResetSeeStatslbRune() 
        {
            LbRuneName.Content = "None Runa +0";
            LbRuneMainStat.Content = "MainStats: None";
            LbRuneSellCost.Content = "None";
            RuneUpCostLb.Content = "None";
        }

        /*
        private int TypeRune;
        private Button DragRuneBtn;
        private BitmapImage DragRuneImg;
        */
        private void Btns_MouseDown(object sender, RoutedEventArgs e) 
        {/*
            bool IsStorageRune = false;
            Button btn = (Button)sender;

            int NumRune;
            for (NumRune = 0; NumRune < 100; NumRune++)
                if (btn == RunesBtn[NumRune])
                {
                    IsStorageRune = true;
                    break;
                }

            if (IsStorageRune == true)
            {
                if (GameLogic.gameData.player.StorageRune_[NumRune] != null)
                {
                    TypeRune = GameLogic.gameData.player.StorageRune_[NumRune].Type;
                    DragRuneBtn = RunesBtn[NumRune];
                    DragRuneImg = DataRepository.images.RunesQuality[GameLogic.gameData.player.StorageRune_[NumRune].Quality];
                    SeeRune(NumRune);
                    DragDrop.DoDragDrop(btn, btn, DragDropEffects.Move);
                }
            }
            else
            {
                for (NumRune = -1; NumRune > -4; NumRune--)
                    if (btn == RunesGooose[NumRune + 3])
                        break;

                if (GameLogic.gameData.player.GooseExistRune(NumRune + 3, SelectedUnit))
                {
                    TypeRune = GameLogic.gameData.player.GetRuneGoose(NumRune + 3, SelectedUnit).Type;

                    DragRuneBtn = RunesGooose[NumRune + 3];
                    SeeGooseRune(NumRune + 3, SelectedUnit);
                   
                    DragDrop.DoDragDrop(btn, btn, DragDropEffects.Move);
                }
            }*/
        }
        private void Btns_Drop(object sender, DragEventArgs e) 
        {/*
            bool IsStorageRune = false;
            Button btn = (Button)sender;
            if (btn != DragRuneBtn)
            {

                int NumRune;
                for (NumRune = 0; NumRune < 100; NumRune++)
                    if (btn == RunesBtn[NumRune])
                    {
                        IsStorageRune = true;
                        break;
                    }

                if (IsStorageRune)
                {
                    int CurRuneNum_ = CurRuneNum;
                    if (GameLogic.gameData.player.GooseExistRune(TypeRune, SelectedUnit))
                    {
                        switch (TypeRune)
                        {
                            case 0:
                                CurRuneNum = -1;
                                break;
                            case 1:
                                CurRuneNum = -2;
                                break;
                            case 2:
                                CurRuneNum = -3;
                                break;
                        }
                        if (CurRuneNum_ <= 0)
                            UnEquipRune_Click(sender, e);
                    }
                }
                else
                {
                    int EqR = CurRuneNum;
                    if (GameLogic.gameData.player.GooseExistRune(TypeRune, SelectedUnit))
                    {
                        switch (TypeRune)
                        {
                            case 0:
                                CurRuneNum = -1;
                                break;
                            case 1:
                                CurRuneNum = -2;
                                break;
                            case 2:
                                CurRuneNum = -3;
                                break;
                        }
                   
                        UnEquipRune_Click(sender, e);
                        CurRuneNum = EqR;
                        EquipRune_Click(sender, e);
                    }
                    else 
                    {
                        EquipRune_Click(sender, e);
                    }
                }
            }
            // ((Button)sender) = (string)e.Data.GetData(DataFormats.Text);*/
        }
       
        bool IsNowDragRune = false;
        private void Drag_Mouse() 
        {
            while (IsNowDragRune)
            {
                
            }
        }

       

       

    }
}
