using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using TGN.Model.Entities;
using TGN.Model.Repository;

namespace TGN.Model.Logic
{
    public delegate void SendForLb(string Name, string Num, Label lb);
    public delegate void SendForPB(double MaxHp, double CurHp, ProgressBar pg, Label lb);
    public delegate void SendForRuneImg(int NumBtn, int QualityRune);
    public delegate void SendForImgBrush(Border Img, ImageBrush IB);
    public delegate void SendUpdate(); //не забудь переназвать

    public static class GameLogic
    {
        public static Random rnd = new Random();
        private static Timer MainGameProc;

        public static SendForLb sendForLb;
        public static SendForPB sendForPB;
        public static SendForRuneImg senSendForRuneImg;
        public static SendForImgBrush sendForImgBrush;
        public static SendUpdate sendUpdate; //не забудь переназвать

        public static GameData gameData;

        public static void NewGame() 
        {
            gameData = new GameData();
            DataRepository.mainWindow.UpdateForStartGUI();

            StartGameThread();
        }
        public static void LoadGame()
        {
            gameData = DataRepository.LoadGame();
            DataRepository.mainWindow.UpdateForStartGUI();
            StartGameThread();
        }
        private static void StartGameThread() 
        {
            sendForLb = DataRepository.mainWindow.GiveLb;
            sendForPB = DataRepository.mainWindow.GivePB;
            sendForImgBrush = DataRepository.mainWindow.GiveCurWave;
            UpdateGameform();

            TimerCallback Timer = new TimerCallback(GameProc); //старт игрового потока
            MainGameProc = new Timer(Timer, null, 0, 1000);
        }

        private static void GameProc(object obj) 
        {
            //for debag
            Stopwatch SW = new Stopwatch();
            SW.Start();



            //game GameLogic
            if (gameData.wave.CurrWaveHp == 0)
            {
                gameData.wave.SpawnNewWave();
            }
            else
            {
                if (gameData.CalculateWaveStats())
                {
                    gameData.GiveRewardForEnemy();
                }
                UpdateGameform();
            }
            //end game GameLogic



            //for debag
            DataRepository.mainWindow.LbSW.Dispatcher.BeginInvoke(
                sendForLb, SW.ElapsedTicks +" - tick | (1sec = 1.000ms) ms:   ", (SW.ElapsedTicks / 200).ToString(),
                DataRepository.mainWindow.LbSW); 
            SW.Stop();   
        }

        private static int LastWaveState;
        private static void UpdateGameform()
        {
            if (LastWaveState != gameData.wave.WaveState)
            {
                LastWaveState = gameData.wave.WaveState;
                switch (gameData.wave.WaveState)
                {
                    case 0:
                        DataRepository.mainWindow.Dispatcher.BeginInvoke(
                            sendForImgBrush, DataRepository.mainWindow.WaveBg, 
                            DataRepository.images.CommonWave);
                        break;
                    case 1:
                        DataRepository.mainWindow.Dispatcher.BeginInvoke(
                            sendForImgBrush, DataRepository.mainWindow.WaveBg, 
                            DataRepository.images.DeadCommonWave);
                        break;
                    case 2:
                        DataRepository.mainWindow.Dispatcher.BeginInvoke(
                            sendForImgBrush, DataRepository.mainWindow.WaveBg,
                            DataRepository.images.BossWave);
                        break;
                    case 3:
                        DataRepository.mainWindow.Dispatcher.BeginInvoke(
                            sendForImgBrush, DataRepository.mainWindow.WaveBg, 
                            DataRepository.images.DeadBossWave);
                        break;
                }
            }
            
            DataRepository.mainWindow.Dispatcher.BeginInvoke(
                sendForLb, DataRepository.stringsForLb.Money, 
                gameData.player.Money.ToString(), DataRepository.mainWindow.LbGall);
            
            DataRepository.mainWindow.Dispatcher.BeginInvoke(
                sendForLb, DataRepository.stringsForLb.EnemyAmount,
                gameData.wave.CountEnemy.ToString(), DataRepository.mainWindow.LbCurZombie);

            DataRepository.mainWindow.Dispatcher.BeginInvoke(
                sendForLb, DataRepository.stringsForLb.Wave,
                gameData.wave.ToString(), DataRepository.mainWindow.LbWave);

            DataRepository.mainWindow.Dispatcher.BeginInvoke(
                sendForLb, DataRepository.stringsForLb.DPS,
                gameData.TotalDPS.ToString(), DataRepository.mainWindow.LbDPS);
            
            DataRepository.mainWindow.Dispatcher.BeginInvoke(
                    sendForLb,
                    DataRepository.stringsForLb.Money,
                    gameData.player.Money.ToString(),
                    DataRepository.mainWindow.LbGall);
            
            DataRepository.mainWindow.Dispatcher.BeginInvoke(
                sendForLb,
                DataRepository.stringsForLb.EnemyAmount,
                gameData.wave.CountEnemy.ToString(),
                DataRepository.mainWindow.LbCurZombie);

            DataRepository.mainWindow.Dispatcher.BeginInvoke(
                sendForLb,
                DataRepository.stringsForLb.Wave,
                gameData.wave.CurrWaveNum.ToString(),
                DataRepository.mainWindow.LbWave);

            DataRepository.mainWindow.Dispatcher.BeginInvoke(
                sendForLb,
                DataRepository.stringsForLb.DPS,
                gameData.TotalDPS.ToString(),
                DataRepository.mainWindow.LbDPS);

            DataRepository.mainWindow.Dispatcher.BeginInvoke(
                sendForPB,
                gameData.wave.TotalWaveHp,
                gameData.wave.CurrWaveHp,
                DataRepository.mainWindow.ZombiesHpBar,
                DataRepository.mainWindow.ZombiesHpBarText);
        }

        public static void GiveNewRune(string SpecialName) 
        {
            gameData.GiveNewRune(SpecialName);
            if (DataRepository.upgradeForm != null)
            {
                sendUpdate = DataRepository.upgradeForm.UpdateRuneImgs;
                DataRepository.upgradeForm.Dispatcher.BeginInvoke(
                 sendUpdate);
            }
        }

        public static double CalculationAtk(Unit unit)
        {
            if (unit.RuneSlot[1] != null) 
            {
                return unit.Atk + ((unit.Atk / 100)
                     * (unit.RuneSlot[1].MainStat));
            }
            return unit.Atk;
     
        }
        public static double CalculationSpd(Unit unit)
        {
            if (unit.RuneSlot[0] != null)
            {
                return unit.AtkSpeed + ((unit.AtkSpeed / 100)
                            * (unit.RuneSlot[0].MainStat));
            }
            return unit.AtkSpeed;           
        }
        public static double CalculationProfit(Unit unit)
        {
            if (unit.RuneSlot[2] != null) 
            {
                return (double)(unit.RuneSlot[2].MainStat);
            }
            return 0;
        }

        public static double CalculationCostUpgradeRune(int RuneLvlGrind, int AttemptsGrind) 
        {
            double CostGrind = 0;
            for (int i = 0; i < AttemptsGrind; i++)
			{
                CostGrind += ((double)(RuneLvlGrind + i) + (double)1) * (double)100 * (double)1.5;
			}

            return Math.Floor(CostGrind);
        }
        public static double CalculationTotalCostRune(int RuneLvlGrind) 
        {
            double CostGrind = 0;
            for (int i = 0; i < RuneLvlGrind; i++)
            {
                CostGrind += ((double)(i) + (double)1) * (double)100 * (double)1.5;
            }

            return Math.Floor(CostGrind);
        }

        public static bool UpgaradeRune(int RuneNum, int SelectedUnit, int AttemptsGrind) 
        {
            if (gameData.player.StorgradeRunes.Count > RuneNum)
            {
                double CostUpgrade = CalculationCostUpgradeRune(gameData.player.StorgradeRunes[RuneNum].LvlGrind, AttemptsGrind);
                if (gameData.player.Money >= CostUpgrade)
                {
                    gameData.player.Money -= CostUpgrade;
                    gameData.player.UpgradeStorgradeRune(RuneNum, AttemptsGrind);
                    gameData.UpdateAllstats();
                    return true;
                }
            }
            else if (RuneNum >= 100 && gameData.player.Units[SelectedUnit].RuneSlot[RuneNum - 100] != null) 
            {
                double CostUpgrade = CalculationCostUpgradeRune(gameData.player.Units[SelectedUnit].RuneSlot[RuneNum - 100].LvlGrind, AttemptsGrind);
                if (gameData.player.Money >= CostUpgrade)
                {
                    gameData.player.Money -= CostUpgrade;
                    gameData.player.UpgradeUnitRune(RuneNum - 100,SelectedUnit, AttemptsGrind);
                    gameData.UpdateAllstats();
                    return true;
                }
            }

            return false;
        }

        // ------------------------------------ Test Func ------------------------------------ //








        // ---------------------------------- End Test Func ---------------------------------- //

    }
}
