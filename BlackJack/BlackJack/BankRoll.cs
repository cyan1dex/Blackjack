using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack {
    public class BankRoll
    {
        private int dollars;

        public BankRoll(int dollars)
        {
            this.dollars = dollars;
        }

        public int GetStackSize()
        {
            return dollars;
        }

        public void AddMoney(int amount)
        {
            dollars += amount;
        }

        public void Bet(int amount)
        {
            if(amount > dollars)
                throw new Exception("not enough money in bank roll");
            else
                dollars -= amount;
        }
    }
}
