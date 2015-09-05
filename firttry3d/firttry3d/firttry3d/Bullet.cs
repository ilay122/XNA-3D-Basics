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
    public class Bullet
    {
        private Sprite3D shape;
        private Vector3 velocity;
        private FirstPersonCamera cam;
        

        public Bullet(Model model,FirstPersonCamera cam)
        {
            shape = new Sprite3D(model);
            this.cam = cam;
            shape.setPosition(cam.getPosition());
            shape.setScale(Consts.WORLDSCALE / 10f);

            float dx = cam.getLookAt().X - shape.getPosition().X;
            float dy = cam.getLookAt().Y - shape.getPosition().Y;
            float dz = cam.getLookAt().Z - shape.getPosition().Z;
            float d = (float)(Math.Sqrt(dx * dx + dy * dy + dz * dz));

            this.velocity = new Vector3(dx / d * Consts.BULLETSPEED, dy / d * Consts.BULLETSPEED, dz / d * Consts.BULLETSPEED);

        }
        public bool update(Map map)
        {
            shape.move(velocity);
            return map.collidesWithMap(shape);
            //return false;

        }
        public void draw() 
        {
            shape.draw(cam);
        }
    }
}
