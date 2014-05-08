using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace BlackJack {
    
    public  class Hand
    {
        public List<Card> cards = new List<Card>();
        public int bet;

        public int Value()
        {
            int aces = 0;
            int sum = 0;

            foreach(Card c in cards)
            {
                if (c.Type() == Type.ACE)
                    aces++;
                sum += c.Value();

                while(sum > 21 && aces > 0)
                {
                    sum -= 10;
                    aces--;
                }
            }
               
            return sum;
        }

        public int Size()
        {
            return cards.Count;
        }

        public void AddCard(Card c)
        {
            cards.Add(c);
        }

        public bool HasBlackjack()
        {
            return (cards.Count == 2 && Value() == 21);
        }

    }


}
