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
    public class Player
    {
        private MouseState prevmous;
        private List<Bullet> bullets;
        private Model bulletmodel;
        private Map map;
        private FirstPersonCamera cam;
        public Player(ContentManager content,Map map,FirstPersonCamera cam)
        {
            prevmous = Mouse.GetState();
            bullets = new List<Bullet>();
            this.bulletmodel = content.Load<Model>("Models/bullet");
            this.map = map;
            this.cam = cam;
        }
        public void update()
        {
            MouseState state = Mouse.GetState();
            cam.update();

            for (int i = bullets.Count-1; i >=0; i--)
            {
                bool destroyed = bullets[i].update(map);
                if (destroyed)
                {
                    bullets.RemoveAt(i);
                }
            }

            if (state.LeftButton == ButtonState.Pressed && prevmous.LeftButton != ButtonState.Pressed)
            {
                bullets.Add(new Bullet(bulletmodel, cam));
            }

            prevmous = state;
        }
        public void draw()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].draw();
            }
        }

    }
}
