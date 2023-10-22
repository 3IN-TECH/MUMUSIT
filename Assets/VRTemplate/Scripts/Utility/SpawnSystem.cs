using UnityEngine;

namespace metaverse_template
{

    /// <summary>
    /// This class is responsible for returning its children as spawn
    /// </summary>
    public class SpawnSystem : MonoBehaviour
    {
        public static SpawnSystem instance;

        void Awake()
        {
            if (SpawnSystem.instance == null)
            {
                SpawnSystem.instance = this;
            }
            else
            {
                if (SpawnSystem.instance != this)
                {
                    Destroy(SpawnSystem.instance.gameObject);
                    SpawnSystem.instance = this;
                }
            }
        }

        /// <summary>
        /// Returns the transform of a random spawn
        /// </summary>
        public Transform GetSpawn()
        {
            return GetSpawn(Random.Range(0, transform.childCount));
        }

        /// <summary>
        /// Returns the transform of a specific spawn
        /// </summary>
        /// <param name="num">index of the specific spawn</param>
        public Transform GetSpawn(int num)
        {
            return transform.GetChild(num);
        }
    }
}