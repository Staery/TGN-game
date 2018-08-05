using System;
using TGN.Model.Logic;
using TGN.Model.Repository;

namespace TGN.Model.Entities
{
    [Serializable]
    public class Achievements
    {
        public int PassedWave;
        public double DPS;
        public double Money;
        public int lvlGrind;
        public int LvlGrind 
        {
            get 
            {
                return lvlGrind;
            }
            set 
            {
                lvlGrind = value;
                try
                {
                    DataRepository.mainWindow.Dispatcher.BeginInvoke(
                   GameLogic.sendForLb,
                   DataRepository.stringsForLb.LvlGrind,
                   lvlGrind.ToString(),
                   DataRepository.mainWindow.LvlGrindLb);
                }
                catch { }

            }
        }
        public int mainStat;
        public int MainStat
        {
            get
            {
                return mainStat;
            }
            set
            {
                mainStat = value;
                  try
                {
                  
                DataRepository.mainWindow.Dispatcher.BeginInvoke(
               GameLogic.sendForLb,
               DataRepository.stringsForLb.RuneMainStat,
               mainStat.ToString(),
               DataRepository.mainWindow.MainStatLb);
                }
                  catch { }
            }
        }


        public Achievements()
        {
            PassedWave = 0;
            DPS = 0;
            Money = 0;
            LvlGrind = 0;
            MainStat = 0;
        }

    }
}
