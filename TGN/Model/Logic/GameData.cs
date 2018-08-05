using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGN.Model.Entities;
using TGN.Model.Repository;

namespace TGN.Model.Logic
{
    [Serializable]
    public class GameData
    {
     
        public double totalDPS;

        public double TotalDPS 
        {
            get 
            {
                return totalDPS;
            }
            set 
            {
                if (totalDPS > achievements.DPS) 
                {
                    achievements.DPS = value;
                    DataRepository.mainWindow.Dispatcher.BeginInvoke(
                GameLogic.sendForLb,
                DataRepository.stringsForLb.DPS,
                value.ToString(),
                DataRepository.mainWindow.DPSLb);
                }
                totalDPS = value;
            }
        }

        public Achievements achievements; 
        public Player player; 
        public Wave wave; 

        public GameData() 
        {
            achievements = new Achievements();
            wave = new Wave();
            player = new Player();

            TotalDPS = player.Units[0].Atk;
        }

        public void UpdateplayersDPS()
        {
            TotalDPS = player.DPS;
        }

        public void PlayerSellRune(int NumRune) 
        {
            if (NumRune >= 100)
            {
                player.Money = player.Units[DataRepository.upgradeForm.SelectedUnit].RuneSlot[NumRune - 100].CostSell;
                player.Units[DataRepository.upgradeForm.SelectedUnit].RuneSlot[NumRune - 100] = null;
            }
            else if(player.StorgradeRunes.Count > NumRune) 
            {
                player.Money += player.StorgradeRunes[NumRune].CostSell;
                player.StorgradeRunes.Remove(player.StorgradeRunes[NumRune]);
            }

            UpdateAllstats();
        }
        public void PlayerUnEquipRune(int UnitNum, int NumRune)
        {
            player.StorgradeRunes.Add(
                new Rune(player.Units[UnitNum].RuneSlot[NumRune]));
            player.Units[DataRepository.upgradeForm.SelectedUnit].RuneSlot[NumRune] = null;

            UpdateAllstats();
        }
        public void PlayerEquipRune(int UnitNum, int RuneNum)
        {
            if (player.Units[UnitNum].RuneSlot[player.StorgradeRunes[RuneNum].Type] != null)
            {
                Rune rune = player.Units[UnitNum].RuneSlot[player.StorgradeRunes[RuneNum].Type];
                player.Units[UnitNum].RuneSlot[player.StorgradeRunes[RuneNum].Type] = new Rune(player.StorgradeRunes[RuneNum]);
                player.StorgradeRunes[RuneNum] = new Rune(rune);
            }
            else 
            {
                player.Units[UnitNum].RuneSlot[player.StorgradeRunes[RuneNum].Type] = new Rune(player.StorgradeRunes[RuneNum]);
                player.StorgradeRunes.Remove(player.StorgradeRunes[RuneNum]);
            }

            player.UpdateStats();
        }
        public void UpdateAllstats()
        {
            player.UpdateStats();
            UpdateplayersDPS();
        }

        public bool CalculateWaveStats()
        {
            UpdateplayersDPS();
            return wave.CalculateWave(TotalDPS);
        }

        public void GiveRewardForEnemy()
        {
            player.Money += wave.CurrReward + Math.Floor((wave.CurrReward / 100) * player.AdditionalProfit);
        }
        public void GiveNewRune(string SpecialName)
        {
            Rune NewRune = new Rune(SpecialName);
            if (player.StorgradeRunes.Count < 100)
            {
                player.StorgradeRunes.Add(new Rune(NewRune));
            }
            else
            {
                player.Money += NewRune.CostSell;
            }
        }


    }
   
}
