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
    public class GameStateManager
    {
        private SpriteBatch batch;
        private GraphicsDeviceManager graphics;
        private int currentState;
        private List<GameState> gamestates;
        private ContentManager content;
        private Vector2 defaultcampos;

        private Dictionary<int,object> publicsObjs;
        public GameStateManager(ContentManager content)
        {
            
            this.batch = Game1.spriteBatch;
            this.graphics = Game1.graphics;
            gamestates = new List<GameState>();
            this.content = content;
            defaultcampos = Vector2.Zero;

            currentState = Consts.PLAYSTATE;

            publicsObjs = new Dictionary<int, object>();
            
        }
        private void addState(GameState state)
        {
            gamestates.Add(state);
        }
        public void loadContent()
        {
            addState(new PlayState(this, content));
        }
        public void setState(int state)
        {
            this.currentState = state;
        }
        public void update(GameTime gametime)
        {
            gamestates[currentState].update(gametime);
            
        }
        public void draw()
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            gamestates[currentState].draw();
        }
        public void Exit()
        {
            Environment.Exit(0);
        }

        public void addPublicObj(int num,Object obj)
        {
            publicsObjs.Add(num,obj);
        }
    }
}
