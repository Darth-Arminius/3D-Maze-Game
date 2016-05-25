using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Objects;

namespace CoolEngine.Xaml
{
    public class GameConfiguration
    {
        private List<Entity> entities = new List<Entity>();

        public ICollection<Entity> Entities { 
            get { return entities; } 
        }
    }
}
