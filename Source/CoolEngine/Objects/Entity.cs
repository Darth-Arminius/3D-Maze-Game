using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using OpenGL_Game.Components;
using System.Windows.Markup;
using System.Collections.ObjectModel;

namespace OpenGL_Game.Objects
{
    [RuntimeNameProperty("Name")]
    [ContentProperty("Components")]
    public class Entity
    {
        ObservableCollection<IComponent> components = new ObservableCollection<IComponent>();
        ComponentTypes mask;

        public Entity()
        {
            components.CollectionChanged += components_CollectionChanged;
        }

        public Entity(string name) : this()
        {
            this.Name = name;
        }

        void components_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (IComponent component in e.NewItems)
            {
                Debug.Assert(component != null, "Component cannot be null");

                mask |= component.ComponentType;
            }
        }

        /// <summary>Adds a single component</summary>
        public void AddComponent(IComponent component)
        {
            components.Add(component);
        }

        public String Name { get; set; }

        public ComponentTypes Mask
        {
            get { return mask; }
        }

        public Collection<IComponent> Components
        {
            get { return components; }
        }

        public T GetComponent<T>() where T : IComponent
        {
            var component = Components.OfType<T>().FirstOrDefault();
            if (component == null)
            {
                throw new ArgumentException("A component of this type is not included in the entity.");
            }
            return component;
        }
    }
}
