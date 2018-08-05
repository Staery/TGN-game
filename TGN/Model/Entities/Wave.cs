using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGN.Model.Logic;
using TGN.Model.Repository;

namespace TGN.Model.Entities
{
    [Serializable]
    public class Wave
    {
        public bool IsBossWave;
        public int currWaveNum;
        public int CurrWaveNum 
        {
            get 
            {
                return currWaveNum;
            }
            set 
            {
                currWaveNum = value;

                if (currWaveNum - 1 > GameLogic.gameData.achievements.PassedWave) 
                {
                    GameLogic.gameData.achievements.PassedWave = currWaveNum - 1;

                    DataRepository.mainWindow.Dispatcher.BeginInvoke(
               GameLogic.sendForLb,
               DataRepository.stringsForLb.PassedWave,
               GameLogic.gameData.achievements.PassedWave.ToString(),
               DataRepository.mainWindow.PassedWaveLb);
                }
            }
        }
        public int WaveState; //0 - common/ 1 - common Dead/2 - boss/3 - dead boss

        public double CurrReward;
        public double RewardPerOne;

        public int CountEnemy;
        public double HpOneEnemy;
        public double CurrWaveHp;
        public double TotalWaveHp;
        

        public void SpawnNewWave()
        {
            CurrWaveNum++;
            if (CurrWaveNum % 10 == 0)
            {
                IsBossWave = true;
                WaveState = 2;
                CountEnemy = 1;

                HpOneEnemy = ((double)CurrWaveNum * ((double)CurrWaveNum / (double)4)) 
                    * (double)DataRepository.options.Modify_BossHP;

                RewardPerOne = ((double)CurrWaveNum * ((double)CurrWaveNum / (double)4)) 
                    * (double)DataRepository.options.Modify_BossReward;
            }
            else
            {
                IsBossWave = false;
                WaveState = 0;
                CountEnemy = 10;

                HpOneEnemy = ((double)CurrWaveNum * ((double)CurrWaveNum / (double)4)) 
                    * (double)DataRepository.options.Modify_CommonHP;

                RewardPerOne = ((double)CurrWaveNum * ((double)CurrWaveNum / (double)4)) 
                    * (double)DataRepository.options.Modify_CommonReward;
            }
            TotalWaveHp = HpOneEnemy * CountEnemy;
            CurrWaveHp = TotalWaveHp;
        }
        public bool CalculateWave(double TotalDPS) 
        {
            if (CurrWaveHp > TotalDPS)
            {
                int LastCount = CountEnemy;
                CurrWaveHp = CurrWaveHp - TotalDPS;

                if (CurrWaveHp % HpOneEnemy == 0)
                {
                    CountEnemy = (int)(CurrWaveHp / HpOneEnemy);
                }
                else
                {
                    CountEnemy = (int)(CurrWaveHp / HpOneEnemy) + 1;
                }

                if(LastCount != CountEnemy)
                {
                    CurrReward = RewardPerOne * (LastCount - CountEnemy);
                    return true;
                }
            }
            else
            {
                if (IsBossWave)
                {
                    WaveState = 3;
                    GameLogic.GiveNewRune("Boss"); 
                }
                else
                {
                    if (GameLogic.rnd.Next(0, 101) <= DataRepository.options.ChanceDropRuneInCommonWave + GameLogic.rnd.Next(0, 11))
                        GameLogic.GiveNewRune("Wave"); //шанс 10-20% на выпадение руны с обычной волны
                    WaveState = 1;
                }

                CurrReward = RewardPerOne * CountEnemy;
                CountEnemy = 0;
                CurrWaveHp = 0;

                DataRepository.SaveGame(GameLogic.gameData);
                return true;
            }

            return false;
        }

    }
}
