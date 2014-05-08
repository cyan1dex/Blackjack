using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack {
    class Deck {

        public Card[] deck = new Card[52];
        static int jack = 10;
        static int queen = 10;
        static int king = 10;
        static public int ace = 11;
        public int[] cards = { 2, 3, 4, 5, 6, 7, 8, 9, 10, jack, queen, king, ace };

        public Deck()
        {
            Build();
            Shuffle();

        }

        public void Build()
        {
            int deckPortion = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j <= 13; j++ )
                {
                    Type type = Type.REG;
                    if (j == 10)
                        type = Type.JACK;
                    else if (j == 11)
                        type = Type.QUEEN;
                    else if (j == 12)
                        type = Type.KING;
                    else if (j == 13)
                        type = Type.ACE;

                    Card card = new Card(cards[j-1], GetRelevantSuit(i), type);
                    deck[deckPortion] = card;
                    deckPortion++;
                }             
            }
        }

        public void Shuffle()
        {
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                int val1 = rand.Next(52);
                int val2 = rand.Next(52);

                Card temp = deck[val1].Copy();

                deck[val1] = deck[val2].Copy();
                deck[val2] = temp.Copy();
            }
        }

        public SUIT GetRelevantSuit(int val)
        {
            switch (val)
            {
                case 0:
                    return SUIT.CLUB;
                case 1:
                    return SUIT.DIAMOND;
                case 2:
                    return SUIT.HEART;
                case 3:
                    return SUIT.SPADE;
                default:
                    return SUIT.UNKNWN;
            }
        }

        #region Testing

        public void ShuffleTest()
        {
            int same = 0;
            foreach (Card c in deck)
            {
                foreach (Card c1 in deck)
                {
                    c1.Equals(c);
                    same++;

                    if (same == 2)
                        Console.WriteLine("fail");
                }
                same = 0;
            }
            Console.WriteLine("Success");

        }
        #endregion

    }
}
