using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BlackJack
{
    public enum DECISION
    {
        HIT,
        STAY,
        DOUBLE,
        SPLIT,
        INSURANCE,
        SURRENDER,
        NEWHAND,
        NEWSHOE,
        QUIT
    };

    public class BlackJack
    {
        public Player player;
        public Dealer dealer;
        public Shoe shoe;
        public int curHand;
        private int playerOne = 1;
        private int compDealer = 0;
        int betAmount = 50;

        public List<CardPosition> cardPositions = new List<CardPosition>();
        public int[] handCardCounts = new int[10];

        public BlackJack(Player player, Dealer dealer)
        {
            this.player = player;
            this.dealer = dealer;
        }

        public void ResetHands()
        {
            cardPositions.Clear();
            player.hands.Clear();
            player.hands.Add(new Hand());
            dealer.hand.cards.Clear();
            curHand = 0;
            handCardCounts = new int[10];
        }

        public void Shuffle()
        {
            shoe = new Shoe();
            shoe.BuildShoe();
            shoe.ShuffleShoe();
        }

        public bool PlayerCanSplit()
        {
            if (player.hands[curHand].cards.Count == 2)
                return (player.hands[curHand].cards[0].Value() == player.hands[curHand].cards[1].Value());
            else
                return false;

        }

        public bool PlayerCanDouble()
        {
            return (player.hands[curHand].cards.Count == 2);
        }

        public bool PlayerBusted()
        {
            return player.hands[curHand].Value() > 21;
        }

        public bool DealerBusted()
        {
            return dealer.hand.Value() > 21;
        }

        public void Double()
        {
            player.DoubleBet(curHand);
            DealCard();
            Play(DECISION.STAY);
            
        }

        public void Split()
        {
            //TODO: if hand was split on Ts cannot make blackjack
            //TODO: deal another card to each hand or else AI can double from single card, new method automatically deals a card if there is less than 2 in current hand
            //TODO: if more than 4 player (curHand == 3) hands shrink all cardpositions by a factor so they all fit on screen
            Hand hand = new Hand();
            Card c = player.hands[curHand].cards[1];

            player.hands[curHand].cards.RemoveAt(1);
            handCardCounts[curHand]--;

            hand.AddCard(c);
            player.hands.Add(hand);
            player.BetOnHand(curHand+1, betAmount);

            cardPositions[cardPositions.Count - 1].destination =
                cardPositions[cardPositions.Count - 1].GetDestination(playerOne, player.hands.Count-1, 0);

           // DealCard();
        }

        public void Deal()
        {
            player.BetOnHand(curHand,betAmount);
            player.GetCard(curHand, shoe.DealCard());
            player.GetCard(curHand, shoe.DealCard());

            dealer.GetCard(shoe.DealCard());
            dealer.GetCard(shoe.DealCard());

            //Dealer positions
            cardPositions.Add(new CardPosition(compDealer, new Rectangle(Xposition(dealer.hand.cards[0]) * Game1.cardSize.X, Yposition(dealer.hand.cards[0]) * Game1.cardSize.Y, Game1.cardSize.X, Game1.cardSize.Y), 0, 0));
            cardPositions.Add(new CardPosition(compDealer, new Rectangle(Xposition(dealer.hand.cards[1]) * Game1.cardSize.X, Yposition(dealer.hand.cards[1]) * Game1.cardSize.Y, Game1.cardSize.X, Game1.cardSize.Y), 0, 1));

            //Player positions
            cardPositions.Add(new CardPosition(playerOne, new Rectangle(Xposition(GetCurrCard()) * Game1.cardSize.X, Yposition(GetCurrCard()) * Game1.cardSize.Y, Game1.cardSize.X, Game1.cardSize.Y), curHand, handCardCounts[curHand]));
            handCardCounts[curHand]++;
            cardPositions.Add(new CardPosition(playerOne, new Rectangle(Xposition(GetCurrCard()) * Game1.cardSize.X, Yposition(GetCurrCard()) * Game1.cardSize.Y, Game1.cardSize.X, Game1.cardSize.Y), curHand, handCardCounts[curHand]));
            handCardCounts[curHand]++;
        }

        public void NextHandOrTerminate()
        {
            if (curHand < player.hands.Count - 1)
            {
                curHand++;
                handCardCounts[curHand]++;
            }
            else
            {
                player.playing = false;
            }
        }

        public void DealCard()
        {
            player.GetCard(curHand, shoe.DealCard());
            cardPositions.Add(new CardPosition(playerOne,
                                               new Rectangle(Xposition(GetCurrCard()) * Game1.cardSize.X,
                                                             Yposition(GetCurrCard()) * Game1.cardSize.Y,
                                                             Game1.cardSize.X, Game1.cardSize.Y),
                                               curHand, handCardCounts[curHand]));
            handCardCounts[curHand]++;
        }

        public void Play(DECISION decision)
        {
            switch (decision)
            {
                case DECISION.HIT:
                    {
                        if (player.hands[curHand].Value() < 21 )
                        {
                            DealCard();                            

                            if(player.hands[curHand].Value() > 21)
                                NextHandOrTerminate();
                        }
                        else
                            NextHandOrTerminate();

                        break;
                    }
                case DECISION.SPLIT:
                    {
                        Split();

                        break;
                    }
                    case DECISION.DOUBLE:
                    {
                        if (PlayerCanDouble())
                            Double();
                        break;
                    }
                case DECISION.STAY:
                    {
                        if (curHand < player.hands.Count - 1)
                        {
                            curHand++;
                            handCardCounts[curHand]++;
                        }
                        else
                        {
                            player.playing = false;
                        }
                        break;
                    }
                case DECISION.NEWHAND:
                    {
                        ResetHands();
                        player.playing = true;
                        Deal();

                        break;
                    }
            }
        }

        public void DealerPlay() //TODO: if dealer has soft 17, hit
        {
            int i = 2;
            while (dealer.hand.Value() < 17)
            {
                dealer.GetCard(shoe.DealCard());
                cardPositions.Add(new CardPosition(compDealer, new Rectangle(Xposition(dealer.hand.cards[i]) * Game1.cardSize.X, Yposition(dealer.hand.cards[i]) * Game1.cardSize.Y, Game1.cardSize.X, Game1.cardSize.Y), 0, i));
                i++;
            }
        }

        public Card GetCurrCard()
        {
            return player.hands[curHand].cards[handCardCounts[curHand]];
        }


        public String Options()
        {
            String split = PlayerCanSplit() ? " or P to split" : "";
            String canDouble = PlayerCanDouble() ? " or D to double" : "";


            return "Press 'H' to hit or 'S' to stay" + split + canDouble;
        }

        public String HandOverPrompt()
        {
            //TODO: if deck has surpassed stop card, prompt for a new shoe
            String busted = PlayerBusted() ? "You Busted, " : "";

            return busted + "Play Again (Y)es or (N)o";
        }

        public int Xposition(Card card)
        {
            if (card.Type() == Type.REG)
                return card.Value() - 1;
            else if (card.Type() == Type.KING)
                return 12;
            else if (card.Type() == Type.JACK)
                return 10;
            else if (card.Type() == Type.QUEEN)
                return 11;
            else
                return 0;
        }

        public int Yposition(Card card)
        {
            if (card.Suit() == SUIT.CLUB)
                return 0;
            if (card.Suit() == SUIT.SPADE)
                return 1;
            if (card.Suit() == SUIT.HEART)
                return 2;
            // if (card.Suit() == SUIT.DIAMOND)
            return 3;

        }
    }
}
