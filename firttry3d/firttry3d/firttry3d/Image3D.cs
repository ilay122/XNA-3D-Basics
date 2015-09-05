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
    public class Image3D
    {
        private Vector3 position;
        private Quaternion rotation;
        private float scale;
        private Texture2D texture;
        private VertexPositionNormalTexture[] vertices;
        private BasicEffect effect;

        private Vector3 rot;

        private bool isFloor;
        private bool leftWall;
        private BoundingBox bounding;
        
        private static Vector2[] textureCoords = new Vector2[4] {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        public Image3D(Texture2D texture, Vector3 position,bool isfloor)
        {
            this.texture = texture;
            this.position = position;
            this.scale = Consts.WORLDSCALE;

            this.rotation = Quaternion.CreateFromYawPitchRoll(0,0,0);
            this.isFloor = isfloor;

            leftWall = true;
            setIsFloor(isfloor);
            
            vertices = new VertexPositionNormalTexture[4];            
            vertices[0] = new VertexPositionNormalTexture(new Vector3(-1, 1, 0), Vector3.Forward, textureCoords[0]);
            vertices[1] = new VertexPositionNormalTexture(new Vector3(1, 1, 0), Vector3.Forward, textureCoords[1]);
            vertices[2] = new VertexPositionNormalTexture(new Vector3(-1, -1, 0), Vector3.Forward, textureCoords[2]);
            vertices[3] = new VertexPositionNormalTexture(new Vector3(1, -1, 0), Vector3.Forward, textureCoords[3]);

            effect = new BasicEffect(Game1.graphics.GraphicsDevice);
            
            updateBoundingBox();
        }
        public void setPosition(Vector3 pos)
        {
            this.position = pos;
        }
        public void move(Vector3 mov)
        {
            this.position += mov;
        }
        public void setScale(float scale)
        {
            this.scale = scale;
        }
        public float getScale()
        {
            return scale;
        }
        public void setIsFloor(bool isFloor)
        {
            if (isFloor)
            {
                rot = new Vector3(0, MathHelper.ToRadians(90), 0);
                //move(new Vector3(0, -Consts.WORLDSCALE, 0));
            }
            else
            {
                if (leftWall)
                {
                    rot = new Vector3(MathHelper.ToRadians(270), 0, 0);
                }
                else
                {
                    rot = new Vector3(0, 0, MathHelper.ToRadians(180));
                }
            }
            this.isFloor = isFloor;

            updateBoundingBox();
        }
        public void draw(FirstPersonCamera camera)
        {

            drawMatrix(camera, getWorld());
            drawMatrix(camera, getWorldReversed());
        }
        private void drawMatrix(FirstPersonCamera cam, Matrix world)
        {
            effect.EnableDefaultLighting();
            effect.LightingEnabled = true;
            effect.World = world;
            effect.View = cam.getView();
            effect.Projection = cam.projection;
            effect.Texture = texture;
            effect.TextureEnabled = true;
            
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                effect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);
            }
        }
        public Matrix getWorld()
        {
            return Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(rot.X , rot.Y, rot.Z)) * Matrix.CreateTranslation(position);
        }
        public Matrix getWorldReversed()
        {
            return Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(rot.X,rot.Y + MathHelper.ToRadians(180),rot.Z)) * Matrix.CreateTranslation(position);
        }

        private void updateBoundingBox()
        {
            BoundingBox box;
            if(isFloor){
            box = new BoundingBox(
                new Vector3(
                    position.X - (texture.Width / 2),
                    position.Y - (texture.Height / 2),
                    position.Z - (Consts.WORLDSCALE / 2)
                ),
                new Vector3(
                    position.X + (texture.Width / 2),
                    position.Y + (texture.Height / 2),
                    position.Z + (Consts.WORLDSCALE / 2)
                 )
             );
            }
            else{
                box =new BoundingBox();
            }
            this.bounding = box;

        }
        public BoundingBox getBoundingBox()
        {
            return bounding;
        }

        }
    }

