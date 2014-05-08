using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BlackJack {
    public class CardPosition {
        public Rectangle destination;
        public Rectangle source;
        private int offset = 5;

        private int cardWidth, cardHeight;

        public CardPosition(int player, Rectangle source, int hand, int card)
        {
            this.source = source;
            cardWidth = (Game1.screenWidth / 8);
            cardHeight = (Game1.screenHeight / 6);
            this.destination = GetDestination(player, hand, card);
        }


        public Rectangle GetDestination(int player, int hand, int card)
        {
            if (card > 1)
                offset = -25;
            if (player == 0) //computer
                return new Rectangle(cardWidth * card + offset * card + offset+200, cardHeight / 4-22, cardWidth, cardHeight);
            //else player
            return new Rectangle(cardWidth * card + offset * card + 200, cardHeight * hand +hand*offset+ Game1.screenHeight / 3, cardWidth, cardHeight);

        }
    }
}
