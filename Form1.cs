using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Card
{
    public partial class Form1 : Form
    {
        private List<ACard> all;
        private Player Player1;
        private Player Player2; 

        private ACard a; //Drawed card from deckA.
        private ACard b; //Drawed card from deckB.
        private ACard pilea; //Current card in pileA.
        private ACard pileb; //Current card in pileB.
        private ACard tempa; //Current card that show in pileA.
        private ACard tempb; //Current card that show in pileB.
        private ACard currA;
        private ACard currB;

        private Boolean checkFirstCompare; //Avoid some error when user use COMPARE before DRAW their card.
        private Boolean checkManyHitCompare; //Avoid some error when user rapidly click COMPARE button many times.
        private Boolean checkManyHitDraw; //Avoid some error when user rapidly click DRAW button many times.
        private Boolean checkFirstPileA; //Decide when to show card on pileA.
        private Boolean checkFirstPileB; //Decide when to show card on pileB.

        //Form function
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            deckA.Image = Image.FromFile("back.jpg");
            deckB.Image = Image.FromFile("back.jpg");
            Prepare();
        }

        //Prepare Function
        public void Prepare()
        {
            checkFirstCompare = true;
            checkFirstPileA = true;
            checkFirstPileB = true;
            all = new List<ACard>();
            Player1 = new Player("A");
            Player2 = new Player("B");
            for (int i = 1; i <= 13; i++) for (int j = 1; j <= 4; j++) all.Add(new ACard(i, j));
            all = ShuffleList(all);
            for (int i=0;i<52;i+=2)
            {
                Player1.addDeck(all[i]);
                Player2.addDeck(all[i+1]);
            }
        }

        //Shuffle Function
        private List<ACard> ShuffleList(List<ACard> inputList)
        {
            List<ACard> randomList = new List<ACard>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count);
                randomList.Add(inputList[randomIndex]);
                inputList.RemoveAt(randomIndex);
            }

            return randomList;
        }

        //Show Last Card in pile
        private void showPileA(ACard incard)
        {
            lastA3.Image = lastA2.Image;
            lastA2.Image = lastA1.Image;
            lastA1.Image = pileA.Image;
            pileA.Image = Image.FromFile(getAddress(incard.getRank(), incard.getSuit()));
            tempa = incard;
        }
        private void showPileB(ACard incard)
        {
            lastB3.Image = lastB2.Image;
            lastB2.Image = lastB1.Image;
            lastB1.Image = pileB.Image;
            pileB.Image = Image.FromFile(getAddress(incard.getRank(), incard.getSuit()));
            tempb = incard;
        }

        //Click to Draw card
        private void hit_Click(object sender, EventArgs e)
        {
            if (Player1.getNumdeck() == 1)
            {
                deckA.Hide();
                deckB.Hide();
            }
            if (!checkManyHitDraw)
            {
                if (Player1.getNumdeck() > 0) Draw();
                else endprog();
            }
        }

        //Draw Function
        public void Draw()
        {
            clearcard();
            checkFirstCompare = false;
            checkManyHitCompare = false;
            checkManyHitDraw = true;

            if (!checkFirstPileA && tempa != pilea) //If last card in pileA called 'pilea' not show as 'tempa', update it.
            {
                if (a.getRank() == b.getRank())
                {
                    showPileA(currB);
                    showPileA(currA);
                }
                showPileA(b);
                showPileA(a);
                if (Player1.getNumpile() > 4) numpileA.Text = "+" + Convert.ToString(Player1.getNumpile() - 4);
            }
            if (!checkFirstPileB && tempb != pileb)
            {
                if (a.getRank() == b.getRank())
                {
                    showPileB(currA);
                    showPileB(currB);
                }
                showPileB(a);
                showPileB(b);
                if (Player2.getNumpile() > 4) numpileB.Text = "+" + Convert.ToString(Player2.getNumpile() - 4);
            }

            a = Player1.getCard();
            b = Player2.getCard();
            showA.Image = Image.FromFile(getAddress(a.getRank(), a.getSuit()));
            showB.Image = Image.FromFile("back.jpg");
        }

        //Click to compare Card
        private void com_Click(object sender, EventArgs e)
        {
            if (!checkManyHitCompare && !checkFirstCompare) Compare();
        }

        //Compare Function
        public void Compare()
        {
            checkManyHitCompare = true;
            checkManyHitDraw = false;
            showB.Image = Image.FromFile(getAddress(b.getRank(), b.getSuit()));
            int rankA = a.getRank();
            int rankB = b.getRank();

            if (rankA > rankB)
            {
                Player2.addPile(a);
                Player2.addPile(b);
                pileb = b;
                checkFirstPileB = false;
            }
            else if (rankB > rankA)
            {
                Player1.addPile(b);
                Player1.addPile(a);
                pilea = a;
                checkFirstPileA = false;
            }
            else if (!(Player1.getNumdeck() - a.getRank() <= 0))
            {
                MessageBox.Show("Rank is Equal at Rank " + Convert.ToString(a.getRank()));

                holda.Image = Image.FromFile(getAddress(a.getRank(), a.getSuit()));
                holdb.Image = Image.FromFile(getAddress(b.getRank(), b.getSuit()));

                List<ACard> tmp1 = new List<ACard>();
                List<ACard> tmp2 = new List<ACard>();

                int num = a.getRank(), i;

                for (i = 0; i < num; i++)
                {
                    ACard tempcard = Player1.getCard();
                    if (i < num - 1) holdcard(tempcard);
                    tmp1.Add(tempcard);
                    tmp2.Add(Player2.getCard());
                }
                showA.Image = Image.FromFile(getAddress(tmp1[i - 1].getRank(), tmp1[i - 1].getSuit()));
                showB.Image = Image.FromFile(getAddress(tmp2[i - 1].getRank(), tmp2[i - 1].getSuit()));
                if (tmp1[i - 1].getRank() > tmp2[i - 1].getRank())
                {
                    MessageBox.Show("Player2 Win " + (num * 2 + 2) + " Cards!!!");
                    for (i = 0; i < num; i++)
                    {
                        Player2.addPile(tmp1[i]);
                        Player2.addPile(tmp2[i]);
                        pileb = b;
                        checkFirstPileB = false;
                    }
                    currA = tmp1[i - 1];
                    currB = tmp2[i - 1];
                    Player2.addPile(a);
                    Player2.addPile(b);
                }
                else if (tmp1[i - 1].getRank() < tmp2[i - 1].getRank())
                {
                    MessageBox.Show("Player1 Win " + (num * 2 + 2) + " Cards!!!");
                    for (i = 0; i < num; i++)
                    {
                        Player1.addPile(tmp2[i]);
                        Player1.addPile(tmp1[i]);
                        pilea = a;
                        checkFirstPileA = false;
                    }
                    currA = tmp1[i - 1];
                    currB = tmp2[i - 1];
                    Player1.addPile(b);
                    Player1.addPile(a);
                }
                else
                {
                    MessageBox.Show("Equal again!!! Return card to your deck and shuffle");
                    for (i = 0; i < num; i++)
                    {
                        Player1.addDeck(tmp1[i]);
                        Player2.addDeck(tmp2[i]);
                        showA.Image = null;
                        showB.Image = null;
                    }
                    Player1.inDeck(ShuffleList(Player1.getDeck()));
                    Player2.inDeck(ShuffleList(Player2.getDeck()));
                }
            }
            else
            {
                MessageBox.Show("Rank is Equal at Rank " + Convert.ToString(a.getRank()) + " ,you need to draw " + Convert.ToString(a.getRank()) + " cards from your deck but we know that your deck is not have enough cards to draw, So...");
                endprog();
            }
        }

        //Generate name of card to use for call card's image
        public string getAddress(int rank,int suit)
        {
            return Convert.ToString(rank)+"-"+Convert.ToString(suit)+".jpg";
        }

        //End Condition
        private void endprog()
        {
            if (Player1.getNumpile() > Player2.getNumpile()) MessageBox.Show("All cards were drawed! Player A is !!!WINNER!!!");
            else MessageBox.Show("All cards were drawed! COMPUTER is !!!WINNER!!!");
            System.Threading.Thread.Sleep(3000);
            Application.Exit();
        }

        //Click Restart
        private void newgame_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        //Update holded Card
        private void holdcard(ACard cardin)
        {
            hold12.Image = hold11.Image;
            hold11.Image = hold10.Image;
            hold10.Image = hold9.Image;
            hold9.Image = hold8.Image;
            hold8.Image = hold7.Image;
            hold7.Image = hold6.Image;
            hold6.Image = hold5.Image;
            hold5.Image = hold4.Image;
            hold4.Image = hold3.Image;
            hold3.Image = hold2.Image;
            hold2.Image = hold1.Image;
            hold1.Image = Image.FromFile(getAddress(cardin.getRank(), cardin.getSuit()));

            chold12.Image = chold11.Image;
            chold11.Image = chold10.Image;
            chold10.Image = chold9.Image;
            chold9.Image = chold8.Image;
            chold8.Image = chold7.Image;
            chold7.Image = chold6.Image;
            chold6.Image = chold5.Image;
            chold5.Image = chold4.Image;
            chold4.Image = chold3.Image;
            chold3.Image = chold2.Image;
            chold2.Image = chold1.Image;
            chold1.Image = Image.FromFile("back.jpg");
        }
        //Clear holded Card
        private void clearcard()
        {
            hold12.Image = null;
            hold11.Image = null;
            hold10.Image = null;
            hold9.Image = null;
            hold8.Image = null;
            hold7.Image = null;
            hold6.Image = null;
            hold5.Image = null;
            hold4.Image = null;
            hold3.Image = null;
            hold2.Image = null;
            hold1.Image = null;
            holda.Image = null;

            chold12.Image = null;
            chold11.Image = null;
            chold10.Image = null;
            chold9.Image = null;
            chold8.Image = null;
            chold7.Image = null;
            chold6.Image = null;
            chold5.Image = null;
            chold4.Image = null;
            chold3.Image = null;
            chold2.Image = null;
            chold1.Image = null;
            holdb.Image = null;
        }
    }
}
