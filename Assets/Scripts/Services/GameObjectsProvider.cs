using UnityEngine;

namespace Services
{
    public class GameObjectsProvider : MonoBehaviour
    {
        public static GameObjectsProvider Instance { get; private set; }

        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject itemPrefab;

        public GameObject Cell => cellPrefab;
        public GameObject Item => itemPrefab;
        
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