using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    public class EntityManager
    {
        List<Entity> entityList;

        public EntityManager()
        {
            entityList = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            //Debug.Assert(FindEntity(entity.Name) != null, "Entity '" + entity.Name + "' already exists");
            entityList.Add(entity);
        }

        public void AddEntities(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                AddEntity(entity);
            }
        }

        /// <summary>
        /// Added this little function so that we can remove objects from the game (mainly the bullets) -Armin
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveEntity(Entity entity)
        {
            entityList.Remove(entity);
        }

        public Entity FindEntity(string name)
        {
            return entityList.Find(delegate(Entity e)
            {
                return e.Name == name;
            }
            );
        }

        public List<Entity> Entities()
        {
            return entityList;
        }
    }
}
