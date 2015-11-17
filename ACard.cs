using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card
{
    class ACard : Form1
    {
        private int rank;
        private int suit;

        //Return current card's rank
        public int getRank()
        {
            return rank;
        }

        //Return current card's suit
        public int getSuit()
        {
            return suit;
        }

        //New Card
        public ACard(int rankin,int suitin)
        {
            rank = rankin;
            suit = suitin;
        }

    }
}
