    // your network class inherits SceneBehaviour instead of MonoBehaviour.
   
using UnityEngine;
   
    public class SceneBehaviour : MonoBehaviour
    {
        public static EventProcessor EventProcessor
        {
            get;
            set;
        }

        /// <summary>
        /// YOU MUST CASE Base.Awake() before setting up events!
        /// </summary>
        protected virtual void Awake()
        {
            EventProcessor = Component.FindObjectOfType<EventProcessor>();
        }
    }
