using UnityEngine;

namespace Managers
{
    public class PersistenceManager : MonoBehaviour
    {
        public static PersistenceManager instance;
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
