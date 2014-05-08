using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack {
    public class Dealer {
      public  Hand hand = new Hand();
        public bool playing = false;

      public void GetCard(Card c)
      {
          hand.AddCard(c);
      }
    }
}
