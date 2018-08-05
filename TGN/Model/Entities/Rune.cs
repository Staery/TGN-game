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
    public class Rune
    {
        public string SpecialName;

        public byte Type; //0 - Spd//1 - Atk//2 - Profit//
        public byte Quality; //0 - Common//1 - Rare//2 - Legendary//
        public int MainStat;
        public int LvlGrind;


        public Rune(string SpecialName_)
        {
            LvlGrind = 0;
            SpecialName = SpecialName_;
            Type = (byte)GameLogic.rnd.Next(0, 3);

            int RndQuality = GameLogic.rnd.Next(0, 101);
            if (RndQuality <= 70)
            {
                Quality = 0;
                MainStat = 5;
            }
            else if (RndQuality <= 95)
            {
                Quality = 1;
                MainStat = 10;
            }
            else
            {
                Quality = 2;
                MainStat = 20;
            }
        }
        public Rune(Rune rune)
        {
            SpecialName = rune.SpecialName;

            Type = rune.Type;
            Quality = rune.Quality;
            MainStat = rune.MainStat;
            LvlGrind = rune.LvlGrind;
        }

        public double CostSell
        {
            get
            {
                return Math.Floor(((double)DataRepository.options.SellCost[Quality] * ((double)LvlGrind + (double)1)));
            }
        }

    }
}