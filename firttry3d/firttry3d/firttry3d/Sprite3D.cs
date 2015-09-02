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
    public class Sprite3D
    {
        private Model model;
        private Vector3 position;
        private float scale;

        private Quaternion rotation;

        public Sprite3D(Model model)
        {
            this.model = model;

            this.scale = Consts.WORLDSCALE;
            rotation = new Quaternion();

        }
        public Sprite3D(Model model, Vector3 pos)
        {
            this.model = model;
            this.position = pos;

            this.scale = Consts.WORLDSCALE;
            rotation = new Quaternion();
        }
        public float getScale()
        {
            return scale;
        }
        public void setScale(float scale)
        {
            this.scale = scale;
        }
        public Vector3 getPosition()
        {
            return position;
        }
        public void setPosition(Vector3 position)
        {
            this.position = position;
        }
        public Quaternion getRotation()
        {
            return rotation;
        }
        public void setRotation(Quaternion rotation)
        {
            this.rotation = rotation;
        }
        public void move(Vector3 mov)
        {
            position += mov;
        }
        public void draw(FirstPersonCamera cam)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * getWorld();
                    effect.View = cam.getView();
                    effect.Projection = cam.projection;

                }
                mesh.Draw();
            }

        }
        public Model getModel()
        {
            return model;
        }
        public bool intersects(Sprite3D spr)
        {
            return getBoundings().Intersects(spr.getBoundings());
        }
        public BoundingBox getBoundings()
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), getWorld());

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            // Create and return bounding box
            return new BoundingBox(min, max);
        }
        public Matrix getWorld()
        {
            return Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(position);
        }
        public Matrix getWorldWithoutRotation()
        {
            return Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
        }

    }
}
