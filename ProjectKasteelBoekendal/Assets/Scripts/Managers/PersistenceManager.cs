using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Singleton manager that persists between scene loads.
    /// Any GameObjects that should remain available across scenes
    /// can be parented under this manager's GameObject.
    /// </summary>
    public class PersistenceManager : MonoBehaviour
    {
        public static PersistenceManager instance;
        
        /// <summary>
        /// Initializes the singleton instance and prevents the manager
        /// from being destroyed when loading a new scene.
        /// </summary>
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
        
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
