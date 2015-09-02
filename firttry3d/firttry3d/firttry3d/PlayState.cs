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
    class PlayState : GameState
    {
        private KeyboardState keyb;
        private FirstPersonCamera cam;

        private Map map;
        
        public PlayState(GameStateManager gsm, ContentManager content)
            : base(gsm, content)
        {
            keyb = Keyboard.GetState();
            cam = new FirstPersonCamera(Vector3.Zero, Vector3.Zero, Consts.WORLDSCALE/15f, 0.001f);
            map = new Map(content,cam);
            cam.setMap(map);

            map.setLevel(1);


        }
        
        public override void update(GameTime gametime)
        {
            keyb = Keyboard.GetState();
            cam.update();

        }
        public override void draw()
        {
            map.draw();
        }     
       
    }
}