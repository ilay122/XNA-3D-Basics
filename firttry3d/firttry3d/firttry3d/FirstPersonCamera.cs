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
    public class FirstPersonCamera
    {
        private Vector3 position;
        private Vector3 lastpos;
        private Vector3 rotation;
        private Vector3 lookat;

        private Vector3 mouserot;
        private float camspeed;
        private float mousespeed;

        private Map map;

        private KeyboardState lastkeyb;
        private MouseState lastmouse;

        public Matrix projection;

        public FirstPersonCamera(Vector3 pos, Vector3 rot, float movspeed, float mousespeed)
        {
            this.rotation = rot;
            this.camspeed = movspeed;
            this.position = pos;
            this.mousespeed = mousespeed;
            lastkeyb = Keyboard.GetState();
            lastmouse = Mouse.GetState();

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Game1.graphics.GraphicsDevice.Viewport.AspectRatio, Consts.WORLDSCALE / 100f, Consts.WORLDSCALE*100f);
            moveTo(pos, rot);
            updateLookAt();
        }
        public Matrix getView()
        {
            return Matrix.CreateLookAt(position, lookat, Vector3.Up);
        }
        public void setMap(Map map)
        {
            this.map = map;
        }
        private void moveTo(Vector3 pos, Vector3 rot)
        {
            this.position = pos;
            this.rotation = rot;
        }

        public Vector3 getPosition()
        {
            return position;
        }
        public float getMouseSpeed()
        {
            return mousespeed;
        }
        public void setMouseSpeed(float speed)
        {
            this.mousespeed = speed;
        }

        public Vector3 getRotation()
        {
            return rotation;
        }

        public void setPosition(Vector3 pos)
        {
            this.position = pos;
            updateLookAt();
        }
        public void setRotation(Vector3 rot)
        {
            this.rotation = rot;
            updateLookAt();
        }

        private void updateLookAt()
        {
            Matrix matrixrot = Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y);
            Vector3 offset = Vector3.Transform(Vector3.UnitZ, matrixrot);

            lookat = position + offset;
        }

        private Vector3 ifmovewherebe(Vector3 move)
        {
            Matrix rotat = Matrix.CreateRotationY(rotation.Y);
            Vector3 movment = new Vector3(move.X, move.Y, move.Z);
            movment = Vector3.Transform(movment, rotat);

            return position + new Vector3(
            map.collidesWithMap(position + new Vector3(movment.X, 0, 0), new Vector3(Consts.WORLDSCALE / 1.05f, Consts.WORLDSCALE / 1.05f, Consts.WORLDSCALE / 1.05f)) ? 0 : movment.X,
            map.collidesWithMap(position + new Vector3(0, movment.Y, 0), new Vector3(Consts.WORLDSCALE / 1.05f, Consts.WORLDSCALE / 1.05f, Consts.WORLDSCALE / 1.05f)) ? 0 : movment.Y,
            map.collidesWithMap(position + new Vector3(0, 0, movment.Z), new Vector3(Consts.WORLDSCALE / 1.05f, Consts.WORLDSCALE / 1.05f, Consts.WORLDSCALE / 1.05f)) ? 0 : movment.Z);


        }

        private void move(Vector3 mov)
        {
            moveTo(ifmovewherebe(mov), rotation);
        }

        public void update()
        {
            KeyboardState keyb = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            Vector3 movevec = Vector3.Zero;

            if (keyb.IsKeyDown(Keys.W))
            {
                movevec.Z = 1;
            }
            if (keyb.IsKeyDown(Keys.S))
            {
                movevec.Z = -1;
            }
            if (keyb.IsKeyDown(Keys.D))
            {
                movevec.X = -1;
            }
            if (keyb.IsKeyDown(Keys.A))
            {
                movevec.X = 1;
            }

            if (movevec != Vector3.Zero)
            {
                movevec.Normalize();
                movevec *= camspeed;
                lastpos = position;
                move(movevec);

            }
            float deltax;
            float deltay;

            if (mouse != lastmouse)
            {
                deltax = mouse.X - (Consts.WIDTH / 2);
                deltay = mouse.Y - (Consts.HEIGHT / 2);

                mouserot.X -= mousespeed * deltax;
                mouserot.Y -= mousespeed * deltay;


                if (mouserot.Y < MathHelper.ToRadians(-75.0f))
                {
                    mouserot.Y = mouserot.Y - (mouserot.Y - MathHelper.ToRadians(-75.0f));
                }


                if (mouserot.Y > MathHelper.ToRadians(75.0f))
                {
                    mouserot.Y = mouserot.Y - (mouserot.Y - MathHelper.ToRadians(75.0f));
                }

                rotation = new Vector3(-MathHelper.Clamp(mouserot.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)), MathHelper.WrapAngle(mouserot.X), 0);
                deltax = 0;
                deltax = 0;
                Mouse.SetPosition(Consts.WIDTH / 2, Consts.HEIGHT / 2);
            }

            lastkeyb = keyb;

            updateLookAt();
        }
        public void returnToLastPosition()
        {
            setPosition(lastpos);
        }
        public Vector3 getLookAt()
        {
            return lookat;
        }
    }
}
