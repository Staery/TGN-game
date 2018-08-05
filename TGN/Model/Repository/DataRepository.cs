using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Media;
using TGN.Model.Logic;

namespace TGN.Model.Repository
{
    public static class DataRepository
    {
        public static bool OpenMw = false;
        public static MainWindow mainWindow;
        public static UpgradeForm upgradeForm;

        public static StringsForLb stringsForLb = new StringsForLb();
        public static Images images = new Images();
        public static Options options = new Options();

        public static string FolderSave = @"Save\";
        public static string FileGame = @"Game.dat";
        public static string FileAchievements = @"Achievements.dat";

        public static void SaveGame(GameData gameData_)
        {
            if (Directory.Exists(FolderSave) == false)
                Directory.CreateDirectory(FolderSave);

            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream fStream = new FileStream(FolderSave + FileGame, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(fStream, gameData_);
            }
        }
        public static GameData LoadGame() 
        {
            GameData gameData_;
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream fStream = File.OpenRead(FolderSave + FileGame))
            {
                gameData_ = (GameData)binFormat.Deserialize(fStream);
            }
            return gameData_; 
        }
        public static bool SaveFileIsExist()
        {
            if (Directory.Exists(FolderSave)) 
            {
                if (File.Exists(FolderSave + FileGame))
                    return true;
            }
            return false;
        }

        public static string NumSeparation(string str)
        {
            string[] Buff = new string[2] { "", "" };
            for (int i = 0, count = 0; i < str.Length; i++, count++) 
            {
                if (count == 3)
                {
                    Buff[0] += " ";
                    count = 0;
                }
                Buff[0] += str[str.Length - 1 - i];
            }
                
            for (int i = 0; i < Buff[0].Length; i++)
                Buff[1] += Buff[0][Buff[0].Length - 1 - i];

            return Buff[1];
        }
        public static string LineSeparation(double Num) 
        {
            return NumSeparation(Num.ToString());
        }
    }


    public class Options 
    {
        public int SaveTime = 10; //Save game every 10 sec
        
        //Modify
        public int Modify_CommonReward = 4; 
        public int Modify_CommonHP = 400;

        public int Modify_BossReward = 300;
        public int Modify_BossHP = 10000;

        public byte[] SellCost = new byte[3] {10,30,100 };

        //waves
        public int ChanceDropRuneInCommonWave = 10; //default 10%
    }
    public class StringsForLb
    {
        #region MainForm
        public string CostUnit = "Cost Unit: ";
        public string Units = "Units: ";
        public string DPS = "DPS: ";
        public string Money = "ChoGall's: ";
        public string EnemyAmount = "Enemies: ";
        public string Wave = "Wave: ";
        public string PassedWave = "Passed Wave: ";
        public string LvlGrind = "Lvl Grind: ";
        #endregion

        #region UpdateForm
        public string UnitName = "Unit num: ";
        public string Rune = " Rune";
        public string RuneQuality = "Quality: ";
        public string RuneMainStat = "MainStats: ";
        public string Cost = "Cost: ";

        public string[] TypeRuneStr = new string[]
        {" Spd "," Atk "," Profit "};
        public string[] QualityRuneStr = new string[] 
        { " Common ", " Rare ", " Legendary " };
        #endregion
    }
    public class Images
    {
        #region UpdateForm
        public BitmapImage AddUnits { get; private set; }
        public BitmapImage LeaderUnit { get; private set; }
        public BitmapImage CommonUnit { get; private set; }
        public BitmapImage[] RunesQuality { get; private set; }
        #endregion

        #region MainForm
        public ImageBrush CommonWave { get; private set; }
        public ImageBrush BossWave { get; private set; }
        public ImageBrush DeadCommonWave { get; private set; }
        public ImageBrush DeadBossWave { get; private set; }

        #endregion


        public Images() 
        {
            AddUnits = new BitmapImage(new Uri("Resources/UnitAddImg.png", UriKind.Relative));
            LeaderUnit = new BitmapImage(new Uri("Resources/LeaderUnitIcon.png", UriKind.Relative));
            CommonUnit = new BitmapImage(new Uri("Resources/CommonUnitIcon.png", UriKind.Relative));
            RunesQuality = new BitmapImage[]
            {
                new BitmapImage(new Uri("Resources/icon_ampstone_common.png", UriKind.Relative)),
                new BitmapImage(new Uri("Resources/icon_ampstone_epic.png", UriKind.Relative)),
                new BitmapImage(new Uri("Resources/icon_ampstone_legend.png", UriKind.Relative))
            };

            CommonWave = new ImageBrush(new BitmapImage(new Uri("Resources/CommonWave.png", UriKind.Relative)));
            BossWave = new ImageBrush(new BitmapImage(new Uri("Resources/BossWave.png", UriKind.Relative)));
            DeadCommonWave = new ImageBrush(new BitmapImage(new Uri("Resources/DeadCommonWave.png", UriKind.Relative)));
            DeadBossWave = new ImageBrush(new BitmapImage(new Uri("Resources/DeadBoosWave.png", UriKind.Relative)));
        }

       
    }
}
