using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace firttry3d
{
    public abstract class GameState
    {
        protected SpriteBatch batch;
        protected GraphicsDeviceManager graphics;
        protected GameStateManager gsm;
        protected ContentManager content;
        public GameState(GameStateManager gsm,ContentManager content)
        {
            batch = Game1.spriteBatch;
            graphics = Game1.graphics;
            this.gsm = gsm;
            this.content = content;
        }

        public abstract void update(GameTime gametime);


        public abstract void draw();
        
    }
}
