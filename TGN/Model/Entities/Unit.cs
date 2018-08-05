using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGN.Model.Entities
{
    [Serializable]
    public class Unit
    {
        public int Atk = 1000;
        public double AtkSpeed = 1;

        public Rune[] RuneSlot = new Rune[3];
    }
}
