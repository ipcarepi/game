using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

namespace CoverShooter
{
    /// <summary>
    /// Denotes a single point in space and prefabs that are by default spanwed here when required.
    /// </summary>
    public class SpawnPoint : MonoBehaviour
    {
        /// <summary>
        /// A random prefab is taken to be instantiated during a spawn.
        /// </summary>
        [Tooltip("A random prefab is taken to be instantiated during a spawn.")]
        public GameObject[] Prefabs;
        private LevelController levelController;

        public float SpawnDelay = 5f;

        private void Awake() {
            levelController = FindObjectOfType<LevelController>();
        }

        private void OnEnable() {
            StartCoroutine(Spawnroutine());
        }

        private IEnumerator Spawnroutine() {
            while(true) {
                Spawn(null);
                yield return new WaitForSeconds(SpawnDelay);
            }
        }

        /// <summary>
        /// Spawns a specific prefab. Returns the clone.
        /// </summary>
        public static GameObject SpawnPrefab(GameObject prefab, Vector3 position, BaseActor caller)
        {
            var clone = GameObject.Instantiate(prefab);
            clone.transform.SetParent(null);
            clone.transform.position = position;
            clone.SetActive(true);
            if (caller == null) {
            }
            clone.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

            return clone;
        }


        /// <summary>
        /// Spawns and returns a clone of a random prefab.
        /// </summary>
        /// <returns></returns>
        public GameObject Spawn(BaseActor caller)
        {
            if (Prefabs == null || Prefabs.Length == 0)
                return null;

            for (int i = 0; i < Prefabs.Length; i++) {
                if (Prefabs[i] == null)
                {
                    return null;
                }
            }
            // levelController.AddZombie();
            return SpawnPrefab(Prefabs[Random.Range(0, Prefabs.Length)], transform.position, caller);
        }
    }
}
