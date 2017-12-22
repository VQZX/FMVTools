using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace VideoTools.UI
{
    [RequireComponent(typeof(VideoPlayer))]
    public class TimeWriter : MonoBehaviour
    {
        [SerializeField]
        protected Text writingText;

        private VideoPlayer player;

        protected virtual void Awake()
        {
            player = GetComponent<VideoPlayer>();
        }

        protected virtual void Update()
        {
            writingText.text = player.time.ToTime();
        }
    }
}