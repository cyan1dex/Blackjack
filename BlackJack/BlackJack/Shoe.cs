using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack {
   public class Shoe {
        private int numDecks;
        private int stopPercent;
        private Card[] shoeCards;
        private int currentCard;

        public Shoe(int numDecks, int stopPercent)
        {
            this.numDecks = numDecks;
            shoeCards = new Card[52 * numDecks];
            this.stopPercent = stopPercent;
        }

       public String CurrentPercentLeft()
       {        
           return String.Format("{0:0.00}", ((double)(52*numDecks )/(52*numDecks+currentCard))*100);
       }

        public Shoe(int numDecks)
        {
            this.numDecks = numDecks;
            shoeCards = new Card[52 * numDecks];
            this.stopPercent = 20;
        }

        public Shoe()//default
        {
            numDecks = 5;//5 decks
            shoeCards = new Card[52 * numDecks];
            this.stopPercent = 20; //20 percent deck left stoppage
        }

        public void BuildShoe()
        {
            int curCard = 0;
            for (int i = 0; i < numDecks; i++)
            {
                Deck d = new Deck();
                for(int j = 0; j < 52; j++)
                    shoeCards[curCard++] = d.deck[j];
                             
            }          
        }

        public void ShuffleShoe()
        {
            Random rand = new Random();
            for (int i = 0; i < 52*numDecks; i++)
            {
                int val1 = rand.Next(52 * numDecks);
                int val2 = rand.Next(52 * numDecks);

                Card temp = shoeCards[val1].Copy();

                shoeCards[val1] = shoeCards[val2].Copy();
                shoeCards[val2] = temp.Copy();
            }
        }

        public Card DealCard()
        {
            currentCard++;
            if ((currentCard / (numDecks * 52)) >= (100 - stopPercent)) //If the current card exceeds stop point, deal stop card
                return new Card(0, SUIT.UNKNWN, Type.STOP);
            else
                return shoeCards[currentCard];
        }

        public int CurrentCard()
        {
            return this.currentCard;
        }


    }
}
