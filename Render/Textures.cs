using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace gameProject.Render
{
    public static class Textures
    {
        public static Texture2D enemyTexture;
        public static Texture2D weakTexture;
        public static Texture2D fastTexture;
        public static Texture2D tankTexture;
        public static Texture2D _fastTankTexture;
        public static Texture2D logoTexture;
        public static Texture2D weaponTexture;
        public static Texture2D bulletTexture;
        public static Texture2D playerTexture;
        public static Texture2D gemTexture;
        public static Texture2D mapTexture;
        public static Texture2D floorTexture;
        public static Texture2D wallTexture;
        public static Texture2D agentEnemyTexture;
        public static Texture2D wolfSpriteList;
        public static Texture2D dragonflySpritelist;
        public static Texture2D fastTankTexture;
        public static Texture2D whiteSoldier;
        public static Texture2D plagueDoctor;
        public static Texture2D pixel;
        public static Texture2D noteTexture;
        public static Texture2D arrowTexture;
        public static Texture2D coffin;
        public static Texture2D coffinOpen;
        public static Texture2D guitar;








        public static void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData([Color.White]);
            mapTexture = content.Load<Texture2D>("background-grass");
            weaponTexture = content.Load<Texture2D>("beam");
            bulletTexture = content.Load<Texture2D>("bulletSprite");
            enemyTexture = content.Load<Texture2D>("Sprite-0003");
            gemTexture = content.Load<Texture2D>("pie2");
            weakTexture = content.Load<Texture2D>("weakTexture");
            fastTexture = content.Load<Texture2D>("fastTankTexture");
            tankTexture = content.Load<Texture2D>("SoldierRun");
            _fastTankTexture = content.Load<Texture2D>("fastTankTexture");
            logoTexture = content.Load<Texture2D>("TAIGA_logo");
            playerTexture = content.Load<Texture2D>("guitarPlayer");
            floorTexture = content.Load<Texture2D>("floorTexture");
            wallTexture = content.Load<Texture2D>("wallTexture");
            agentEnemyTexture = content.Load<Texture2D>("agentEnemy (1)");
            wolfSpriteList = content.Load<Texture2D>("wolfSpriteList");
            dragonflySpritelist = content.Load<Texture2D>("dragonflySpritelist");
            whiteSoldier = content.Load<Texture2D>("whiteSoldier");
            plagueDoctor = content.Load<Texture2D>("plagueDoctorSprite");
            noteTexture = content.Load<Texture2D>("noteTexture");
            arrowTexture = content.Load<Texture2D>("arrow");
            coffin = content.Load<Texture2D>("coffin");
            coffinOpen = content.Load<Texture2D>("coffinOpen");
            guitar = content.Load<Texture2D>("guitar");



        }
    }
}
