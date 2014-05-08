using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlackJack {
    public enum BStates { HOVER, UP, JUST_RELEASED, DOWN, DISABLED }
    public class Button : DrawableGameComponent {

        public event EventHandler Clicked;
        public event EventHandler Hover;

        public int numberOfButtons;
        public int BUTTON_HEIGHT, BUTTON_WIDTH;
        public Color button_color;
        public Rectangle button_rectangle;
        public BStates button_state;
        public Texture2D button_texture, depressedTexture, oldTexture;
        public double button_timer;

        //mouse pressed and mouse just pressed
        public bool mpressed, prev_mpressed = false;
        //mouse location in window
        public int mx, my;
        public double frame_time;
        public int screenWidth, screenHeight;


        public Button(Game game)
            : base(game)
        {
            this.screenWidth = Game1.screenWidth;
            this.screenHeight = Game1.screenHeight;
            BUTTON_HEIGHT = screenHeight / 10; //fix
            BUTTON_WIDTH = screenWidth / 9; //fix
        }

        public override void Update(GameTime gameTime)
        {
            frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            update_buttons();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            spriteBatch.Draw(button_texture, button_rectangle, button_color);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected Boolean hit_image_alpha(Rectangle rect, Texture2D tex, int x, int y)
        {
            if (rect.Width == 0 || rect.Height == 0)
                return false;
            if (rect.Y == 0 || rect.X == 0)
                return false;
            else
                return hit_image_alpha(0, 0, tex, tex.Width * (x - rect.X) /
                    rect.Width, tex.Height * (y - rect.Y) / rect.Height);
        }

        // wraps hit_image then determines if hit a transparent part of image 
        protected Boolean hit_image_alpha(float tx, float ty, Texture2D tex, int x, int y)
        {
            if (hit_image(tx, ty, tex, x, y))
            {
                uint[] data = new uint[tex.Width * tex.Height];
                tex.GetData<uint>(data);
                if ((x - (int)tx) + (y - (int)ty) *
                    tex.Width < tex.Width * tex.Height)
                {
                    return ((data[
                        (x - (int)tx) + (y - (int)ty) * tex.Width
                        ] &
                                0xFF000000) >> 24) > 20;
                }
            }
            return false;
        }

        // determine if x,y is within rectangle formed by texture located at tx,ty
        protected Boolean hit_image(float tx, float ty, Texture2D tex, int x, int y)
        {
            return (x >= tx &&
                x <= tx + tex.Width &&
                y >= ty &&
                y <= ty + tex.Height);
        }

        // determine state and color of button
        public void update_buttons()
        {
            if (button_state != BStates.DISABLED)
            {
                MouseState mouse_state = Mouse.GetState();
                mx = mouse_state.X;
                my = mouse_state.Y;
                prev_mpressed = mpressed;
                mpressed = mouse_state.LeftButton == ButtonState.Pressed;

                if (hit_image_alpha(
                    button_rectangle, button_texture, mx, my))
                {
                    button_timer = 0.0;
                    if (mpressed)
                    {
                        // mouse is currently down
                        button_state = BStates.DOWN;
                        button_color = Color.SteelBlue;
                        if (depressedTexture != null)
                        {
                            button_texture = depressedTexture;
                        }
                    }
                    else if (!mpressed && prev_mpressed)
                    {
                        // mouse was just released
                        if (button_state == BStates.DOWN)
                        {
                            // button i was just down
                            button_state = BStates.JUST_RELEASED;
                            if (oldTexture != null)
                            {
                                button_texture = oldTexture;
                            }
                        }
                    }
                    else
                    {
                        button_state = BStates.HOVER;
                        button_color = Color.Blue;
                        if (Hover != null)
                            Hover(this, null);
                    }
                }
                else
                {
                    button_state = BStates.UP;
                    if (button_timer > 0)
                    {
                        button_timer = button_timer - frame_time;
                    }
                    else
                    {
                        if (oldTexture != null)
                        {
                            button_texture = oldTexture;
                        }
                        button_color = Color.White;
                    }
                }

                if (button_state == BStates.JUST_RELEASED)
                {
                    if (Clicked != null)
                        Clicked(this, null);
                }
            }
            else
            {
                button_color = Color.Black;
            }
        }
    }
}
