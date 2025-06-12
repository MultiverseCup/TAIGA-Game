using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Entities
{
    public class Entity
    {
        private Dictionary<Type, object> _components = new();

        public void AddComponent<T>(T component)
        {
            _components[typeof(T)] = component;
        }

        public void RemoveComponent<T>()
        {
            _components.Remove(typeof(T));
        }

        public bool HasComponent<T>()
        {
            return _components.ContainsKey(typeof(T));
        }

        public T GetComponent<T>()
        {
            return _components.TryGetValue(typeof(T), out var comp) ? (T)comp : default;
        }

        public bool TryGetComponent<T>(out T component) where T : class
        {
            if (_components.TryGetValue(typeof(T), out var value) && value is T typedValue)
            {
                component = typedValue;
                return true;
            }

            component = null;
            return false;
        }
    }

}
