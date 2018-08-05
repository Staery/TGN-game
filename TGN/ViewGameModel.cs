using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using TGN.Infrastructure;
using TGN.Model.Repository;

namespace TGN
{

    [Serializable]
    public class ViewGameModel : ViewModelBase
    {
        private string money;
        private double wave;
        private double dps;
        private double enemyCount;
        private double playerUnitsCount;
        private double costNextPlayerUnit;

        public string PlayerMoney 
        {
            get 
            {
                return money;
            }
            set 
            {
                money = DataRepository.stringsForLb.Money + value;
                OnPropertyChanged("PlayerMoney");
            }
        }
        public double Wave
        {
            get
            {
                return wave;
            }
            set
            {
                wave = value;
                OnPropertyChanged("Wave");
            }
        }
        public double Dps
        {
            get
            {
                return dps;
            }
            set
            {
                dps = value;
                OnPropertyChanged("Dps");
            }
        }
        public double EnemyCount
        {
            get
            {
                return enemyCount;
            }
            set
            {
                enemyCount = value;
                OnPropertyChanged("EnemyCount");
            }
        }
        public double PlayerUnitsCount
        {
            get
            {
                return playerUnitsCount;
            }
            set
            {
                playerUnitsCount = value;
                OnPropertyChanged("PlayerUnitsCount");
            }
        }
        public double CostNextPlayerUnit
        {
            get
            {
                return costNextPlayerUnit;
            }
            set
            {
                costNextPlayerUnit = value;
                OnPropertyChanged("CostNextPlayerUnit");
            }
        }


    }
}
