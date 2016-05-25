using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolEngine.Xaml;
using System.Xaml;
using OpenGL_Game.Managers;
using System.Reflection;

namespace OpenGL_Game
{
    public class CoolGameBase : Game
    {
        protected EntityManager entityManager = new EntityManager();
        protected SystemManager systemManager = new SystemManager();

        public static float dt;

        public static CoolGameBase gameInstance;

        public static Matrix view, projection;

        public CoolGameBase() : base()
        {
            gameInstance = this;
        }

        public static Vector3 cp;

        public static Vector3 sp1;
        public static Vector3 sp2;

        protected override void Update(GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected void LoadConfiguration(string filepath)
        {
            var gameAssembly = Assembly.GetCallingAssembly();
            var reader = new XamlXmlReader(filepath, new XamlXmlReaderSettings {
                LocalAssembly = gameAssembly
            });
            var conf = (GameConfiguration)XamlServices.Load(reader);

            entityManager.AddEntities(conf.Entities);
        }
    }
}
