using UnityEngine;

namespace metaverse_template
{

    public class DontDestroy : MonoBehaviour
    {
        private void Awake()
        {
            this.transform.parent = null;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}