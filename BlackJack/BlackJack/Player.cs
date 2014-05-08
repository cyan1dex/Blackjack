using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack {
    public class Player {
        public List<Hand> hands = new List<Hand>();
        private BankRoll bankRoll;
        public bool playing = true;

        public Player(int dollars)
        {
            bankRoll = new BankRoll(dollars);
            Hand h = new Hand();
            hands.Add(h);
        }

        public Player()
        {
            Hand h = new Hand();
            hands.Add(h);
        }

        public BankRoll GetBankRoll()
        {
            return bankRoll;
        }

        public void GetCard(int hand, Card c)
        {
            hands[hand].AddCard(c);
        }

        public int NumHands()
        {
            return hands.Count;
        }

        public void BetOnHand(int hand, int amount)
        {
            hands[hand].bet = amount;
            bankRoll.Bet(amount);
        }

        public void DoubleBet(int hand)
        {
            bankRoll.Bet(hands[hand].bet); //remove money again
            hands[hand].bet *= 2; //hands worth is now double
        }
    }
}
