using System;
using System.Collections.Generic;
using System.Linq;
using BlackJack;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BlackJack
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Player player;
        private Dealer dealer;

        private Texture2D cardSheet;
        private Texture2D felt;

        static public int screenHeight, screenWidth;

        public static Point cardSize = new Point(73, 98);

        private SpriteFont font;

        private BlackJack blackjack;
        private KeyboardState oldKeyState;

        private int wins, losses;
        private bool playerPrompt = false;
        private bool winnerDetermined;

        private Button _hit, _split, _stay, _double, _deal, _quit;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            this.IsMouseVisible = true;

        }
        void button_Clicked(object sender, EventArgs e)
        {
            if (sender == _hit && !playerPrompt)
            {
                blackjack.Play(DECISION.HIT); 
            }
            if (sender == _stay && !playerPrompt)
            {
                blackjack.Play(DECISION.STAY);
            }
            if (sender == _double && !playerPrompt)
            {
                blackjack.Play(DECISION.DOUBLE);
            }
            if (sender == _split && !playerPrompt)
            {
                blackjack.Play(DECISION.SPLIT);
            }
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _hit = new Button(this);
            _hit.button_texture = Content.Load<Texture2D>("hit");
            _double = new Button(this);
            _double.button_texture = Content.Load<Texture2D>("double");
            _split = new Button(this);
            _split.button_texture = Content.Load<Texture2D>("split");
            _stay = new Button(this);
            _stay.button_texture = Content.Load<Texture2D>("stay");

            InitializeButton(_hit, new Rectangle(20, 240, 150, 100));
            InitializeButton(_stay, new Rectangle(20, 320, 150, 100));
            InitializeButton(_double, new Rectangle(20, 400, 150, 100));
            InitializeButton(_split, new Rectangle(20, 480, 150, 100));

            base.Initialize();
        }

        public void InitializeButton(Button button, Rectangle position)
        {
            //button = new Button(this);
            button.button_rectangle = position;
            button.button_state = BStates.UP;
            button.button_color = Color.White;
            button.button_timer = 0.0;
            button.Clicked += new EventHandler(button_Clicked);
            Components.Add(button);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            font = Content.Load<SpriteFont>("font");

            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;

            cardSheet = Content.Load<Texture2D>("cards");
            felt = Content.Load<Texture2D>("felt");


            player = new Player(1000);
            dealer = new Dealer();

            blackjack = new BlackJack(player, dealer);
            blackjack.Shuffle();
            blackjack.Deal();

            //drawing information           
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void CheckInput()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyUp(Keys.H) && oldKeyState.IsKeyDown(Keys.H) && !playerPrompt)
            {
                blackjack.Play(DECISION.HIT); 
            }
            if (keyState.IsKeyUp(Keys.P) && oldKeyState.IsKeyDown(Keys.P) && !playerPrompt)
            {
                blackjack.Play(DECISION.SPLIT);
            }

            if (keyState.IsKeyUp(Keys.S) && oldKeyState.IsKeyDown(Keys.S) && !playerPrompt)
            {
                blackjack.Play(DECISION.STAY);
            }
            if (keyState.IsKeyUp(Keys.D) && oldKeyState.IsKeyDown(Keys.D) && !playerPrompt)
            {
                blackjack.Play(DECISION.DOUBLE);
            }
            if (keyState.IsKeyUp(Keys.Y) && oldKeyState.IsKeyDown(Keys.Y))
            {
                blackjack.Play(DECISION.NEWHAND);
                player.playing = true;
                winnerDetermined = false;
                playerPrompt = false;
            }
            if (keyState.IsKeyUp(Keys.N) && oldKeyState.IsKeyDown(Keys.N))
            {
               // playerPrompt = false;
            }
            oldKeyState = keyState;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            CheckInput();

            if (!blackjack.player.playing)
            {
                blackjack.DealerPlay();
                if(!winnerDetermined)
                DetermineWinner();
                playerPrompt = true;
            }

            

            HandleButtonState(_double, blackjack.PlayerCanDouble());
            HandleButtonState(_split, blackjack.PlayerCanSplit());

            base.Update(gameTime);
        }

        public void HandleButtonState(Button button, bool isActive)
        {
            button.button_state = !isActive ? BStates.DISABLED : BStates.UP;
        }

        public void DetermineWinner()
        {
            foreach(Hand h in player.hands)
            {
                if (h.Value() > 21)
                    losses++;
                else if (h.Value() < dealer.hand.Value() && dealer.hand.Value() < 22)
                    losses++;
                else if (h.Value() > dealer.hand.Value() || dealer.hand.Value() > 21)
                {
                    wins++;
                    if(h.cards.Count == 2 && h.Value() == 21)//blackjack
                        player.GetBankRoll().AddMoney((int)(h.bet * 2.5));
                    else
                        player.GetBankRoll().AddMoney(h.bet*2);
                }
                else if(h.Value() == dealer.hand.Value()) //push, give money back
                    player.GetBankRoll().AddMoney(h.bet);
            }
            winnerDetermined = true;

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //TODO: draw all buttons,  hit or stay etc when available
            //TODO: can currently hit yes/no whenver, should only be at end of each hand
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(felt, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

           // spriteBatch.DrawString(font, "Dealer has " + dealer.hand.Value().ToString(), Vector2.Zero, Color.Red);

            for (int i = 0; i < blackjack.cardPositions.Count; i++)
            {
                CardPosition p = blackjack.cardPositions[i];
                if (i == 0 && blackjack.player.playing)
                spriteBatch.Draw(cardSheet, new Rectangle(p.destination.X, p.destination.Y, p.destination.Width, p.destination.Height), p.source, Color.Black);
                else
                    spriteBatch.Draw(cardSheet, new Rectangle(p.destination.X, p.destination.Y, p.destination.Width, p.destination.Height), p.source, Color.White);
                
            }
                

            //spriteBatch.DrawString(font, "Shoe percent left " + blackjack.shoe.CurrentPercentLeft(), new Vector2(750, 300), Color.White);
           // spriteBatch.DrawString(font, "WINS " + wins + " LOSSES " + losses, new Vector2(750, 370), Color.White);
            spriteBatch.DrawString(font, "Chip stack " + player.GetBankRoll().GetStackSize(), new Vector2(750, 210), Color.White);

            //for (int i = 0; i < player.hands.Count; i++ )
            //    spriteBatch.DrawString(font, "Player hand "+ (i+1) + " value "+ player.hands[i].Value(), new Vector2(500, 300 + i*75), Color.White);
                

            if (blackjack.player.playing  && !playerPrompt)
            {
                //spriteBatch.DrawString(font, blackjack.Options(), new Vector2(350, 250), Color.White);
            }
            else
                spriteBatch.DrawString(font, blackjack.HandOverPrompt(), new Vector2(350, 200), Color.White);

            //spriteBatch.Draw(_hit.button_texture,_hit. button_rectangle,_hit. button_color);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }


}

