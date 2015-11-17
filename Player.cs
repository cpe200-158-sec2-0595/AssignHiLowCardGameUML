using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card
{
    class Player : Form1
    {
        private string name;
        private List<ACard> deck = new List<ACard>();
        private List<ACard> pile = new List<ACard>();

        //Return current deck
        public List<ACard> getDeck()
        {
            return deck;
        }

        //Get new deck
        public void inDeck(List<ACard> indeck)
        {
            deck = indeck;
        }

        //Return current amount of cards in deck
        public int getNumdeck()
        {
            return deck.Count;
        }

        //Return current amount of cards in pile
        public int getNumpile()
        {
            return pile.Count;
        }

        //New Player
        public Player(string namein)
        {
            name = namein;
        }

        //Draw Top card from deck
        public ACard getCard()
        {
            ACard tmp = deck[getNumdeck()-1];
            deck.RemoveAt(getNumdeck()-1);
            return tmp;
        }

        //Put card at Top of pile
        public void addPile(ACard cardin)
        {
            pile.Add(cardin);
        }

        //Put card at Top of deck
        public void addDeck(ACard cardin)
        {
            deck.Add(cardin);
        }
    }
}
