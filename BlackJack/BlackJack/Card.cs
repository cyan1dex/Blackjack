using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum SUIT {
    SPADE,
    HEART,
    DIAMOND,
    CLUB,
    UNKNWN
};

public enum Type
{
    JACK,
    QUEEN,
    KING,
    ACE,
    REG,
    STOP
};

namespace BlackJack {
    public class Card {
        private int value;
        private SUIT suit;
        private Type type;


        public Card()
        {

        }

        public Card(int value, SUIT suit, Type type)
        {
            this.value = value;
            this.suit = suit;
            this.type = type;
        }


        public Card Copy()
        {
            Card temp = new Card();
            temp.SetValue(this.Value());
            temp.SetSuit(this.Suit());
            temp.SetType(this.Type());

            return temp;
        }

        public Type Type()
        {
            return type;
        }
        public void SetType(Type type)
        {
            this.type = type;
        }

        public int Value()
        {
            return value;
        }

        public SUIT Suit()
        {
            return suit;
        }

        public void SetValue(int value)
        {
            this.value = value;
        }

        public void SetSuit(SUIT suit)
        {
            this.suit = suit;
        }

        public override bool Equals(object obj)
        {
            return (((Card) obj).Value() == this.Value() && this.Suit() == ((Card) obj).Suit() &&
                    ((Card) obj).Type() == this.Type());
        }
    }
}
