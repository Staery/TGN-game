using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TGN.Model.Logic;
using TGN.Model.Repository;

namespace TGN.Model.Entities
{

    [Serializable]
    public class Player 
    {
        public double Money
        {
            get
            {
                return money;
            }
            set
            {
                money = value;
                if (money > GameLogic.gameData.achievements.Money)
                {
                    GameLogic.gameData.achievements.Money = value;
                    
                    DataRepository.mainWindow.Dispatcher.BeginInvoke(
                GameLogic.sendForLb, 
                DataRepository.stringsForLb.Money,
                value.ToString(), 
                DataRepository.mainWindow.MoneyLb);
                }
            }
        }

        private double money;
        public double AdditionalProfit;
        public double DPS;

        private byte[] LeaderSkill = new byte[3]; //0 - Spd//1 - Atk//2 - Profit//
        public List<Rune> StorgradeRunes = new List<Rune>();
        public List<Unit> Units = new List<Unit>();

        public Player() 
        {
            Units.Add(new Unit());
            DPS = 1000;
        }

        public int UnitsCount 
        {
            get 
            {
                return Units.Count;
            }
        }
        public double CostBuyNextUnit
        {
            get 
            {
                return Math.Pow((double)Units.Count + (double)1, (double)10) + 1000;
            }
        }

        public void BuyNewUnit()
        {
            Money -= CostBuyNextUnit;
            Units.Add(new Unit());

            UpdateStats();
        }

        public void UpdateStats() 
        {
            double SummAtk = 0;
            double SummSpd = 0;
            double SummProfit = 0;

            for (int i = 0; i < UnitsCount; i++) 
            {
                if (Units[i].RuneSlot[0] != null)
                {
                    SummSpd += Units[i].AtkSpeed + ((Units[i].AtkSpeed / 100)
                            * (Units[i].RuneSlot[0].MainStat + LeaderSkill[0]));
                }
                else 
                {
                    SummSpd += Units[i].AtkSpeed;
                }

                if (Units[i].RuneSlot[1] != null)
                {
                    SummAtk += Units[i].Atk + ((Units[i].Atk / 100)
                            * (Units[i].RuneSlot[1].MainStat + LeaderSkill[1]));
                }
                else
                {
                    SummAtk += Units[i].Atk;
                }

                if (Units[i].RuneSlot[2] != null)
                {
                    SummProfit += (double)(Units[i].RuneSlot[2].MainStat)
                        + ((((double)Units[i].RuneSlot[2].MainStat / (double)100) 
                        * (double)LeaderSkill[2]));
                }
                else
                {
                    SummProfit += 0;
                }
            }

            AdditionalProfit = Math.Floor(SummProfit / 10);
            DPS = Math.Floor(((double)SummAtk * (double)SummSpd) * (double)(UnitsCount));
        }

        

        public void UpgradeStorgradeRune(int RuneNum, int AttemptsGrind) 
        {
            StorgradeRunes[RuneNum] = new Rune(ChanceGrind(StorgradeRunes[RuneNum], AttemptsGrind));
            if (StorgradeRunes[RuneNum].LvlGrind > GameLogic.gameData.achievements.LvlGrind)
                GameLogic.gameData.achievements.LvlGrind = StorgradeRunes[RuneNum].LvlGrind;
            if(StorgradeRunes[RuneNum].MainStat > GameLogic.gameData.achievements.MainStat)
                GameLogic.gameData.achievements.MainStat = StorgradeRunes[RuneNum].MainStat;
        }
        public void UpgradeUnitRune(int RuneNum, int SelectedUnit, int AttemptsGrind) 
        {
            Units[SelectedUnit].RuneSlot[RuneNum] = new Rune(ChanceGrind(Units[SelectedUnit].RuneSlot[RuneNum], AttemptsGrind));
            if (Units[SelectedUnit].RuneSlot[RuneNum].LvlGrind > GameLogic.gameData.achievements.LvlGrind)
                GameLogic.gameData.achievements.LvlGrind = Units[SelectedUnit].RuneSlot[RuneNum].LvlGrind;
            if (Units[SelectedUnit].RuneSlot[RuneNum].MainStat > GameLogic.gameData.achievements.MainStat)
                GameLogic.gameData.achievements.MainStat = Units[SelectedUnit].RuneSlot[RuneNum].MainStat;
        }

        private Rune ChanceGrind(Rune rune, int AttemptsGrind) 
        {
            Rune UpgradedRune = rune;
            for (int i = 0; i < AttemptsGrind; i++)
            {
                if (((double)GameLogic.rnd.Next(1, 101) + (double)GameLogic.rnd.Next(0, 9999999) / (double)10000000)
                    <= ((double)10000 / (((double)UpgradedRune.LvlGrind + (double)1) + (double)10)))
                {
                    UpgradedRune.LvlGrind++;
                    switch (UpgradedRune.Quality)
                    {
                        case 0:
                            UpgradedRune.MainStat += GameLogic.rnd.Next(1, 3);
                            break;
                        case 1:
                            UpgradedRune.MainStat += GameLogic.rnd.Next(2, 6);
                            break;
                        case 2:
                            UpgradedRune.MainStat += GameLogic.rnd.Next(4, 12);
                            break;
                    }
                }
            }
            return UpgradedRune;
        }

    }
}