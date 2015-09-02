using System;
using System.IO;
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
    public class Map
    {
        private List<Sprite3D> Blocks;
        private List<string> map;
        private int level;
        private ContentManager content;
        private FirstPersonCamera cam;
        private Vector3 playerstartpos;
        public Map(ContentManager content,FirstPersonCamera cam)
        {
            Blocks = new List<Sprite3D>();
            map = new List<string>();
            this.content = content;
            this.cam = cam;
        }
        public void setLevel(int level)
        {
            Blocks.Clear();
            map.Clear();
            
            this.level = level;
            string strlevel = @"Content/maps/level" + level + ".txt";
            using (StreamReader r = new StreamReader(strlevel))
            {
                string line = string.Empty;
                while ((line = r.ReadLine()) != null)
                {
                    map.Add(line);
                }
            }
            Model cube = content.Load<Model>("Models/cube");
            bool sseen = false;
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    char here = map[i][j];
                    switch (here)
                    {
                        case 'W':
                            {
                                Blocks.Add(new Sprite3D(cube, new Vector3(i * Consts.WORLDSCALE * 2, 0, j * Consts.WORLDSCALE*2)));
                                break;
                            }
                        case 's':
                            {
                                if (sseen) continue;
                                playerstartpos = new Vector3(i * Consts.WORLDSCALE * 2, 0, j * Consts.WORLDSCALE*2);
                                cam.setPosition(playerstartpos);
                                sseen = true;
                                break;
                            }
                        default:
                            {
                                continue;
                            }


                    }
                }
            }
        }


        public void draw()
        {
            foreach (Sprite3D block in Blocks)
            {
                block.draw(cam);
            }
        }

        public bool collidesWithMap(Vector3 position,Vector3 dimentions)
        {
            Vector3 playerstats = dimentions;

            BoundingBox cameraBox = new BoundingBox(
            new Vector3(
                position.X - (playerstats.X / 2),
                position.Y - (playerstats.Y),
                position.Z - (playerstats.Z / 2)
            ),
            new Vector3(
                position.X + (playerstats.X / 2),
                position.Y,
                position.Z + (playerstats.Z / 2)
            )
        );
            
            foreach (Sprite3D block in Blocks)
            {
                
                if (block.getBoundings().Contains(cameraBox) != ContainmentType.Disjoint)
                {
                    return true;
                }
                
                 
            }

            return false;
        }
        public bool collidesWithMap(Sprite3D spr)
        {
            foreach (Sprite3D block in Blocks)
            {
                if (spr.intersects(block))
                {
                    return true;
                }

            }
            return false;
        }
        public void restart()
        {
            cam.setPosition(playerstartpos);
        }
        public List<Sprite3D> getCollsionBlocks()
        {
            return Blocks;
        }
        public Vector3 getPlayerStartPosition()
        {
            return playerstartpos;
        }
    }
}
