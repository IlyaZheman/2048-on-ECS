using Morpeh.Globals;
using UnityEngine;

namespace Services
{
    [DefaultExecutionOrder(-1000)]
    public class GlobalEventsProvider : MonoBehaviour
    {
        public static GlobalEventsProvider Instance { get; private set; }

        [SerializeField] private GlobalEvent createLevel;
        [SerializeField] private GlobalEventInt sendMovement;

        public GlobalEvent CreateLevel => createLevel;
        public GlobalEventInt SendMovement => sendMovement;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}