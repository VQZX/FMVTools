using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace VideoTools.UI
{
    // ReSharper disable once InconsistentNaming
    public class VideoPlayerUI : MonoBehaviour
    {
        [SerializeField]
        protected VideoPlayer player;

        [SerializeField]
        protected Text toggleText;

        public void OnToggleValueChanged(bool state)
        {
            if (state)
            {
                player.Play();
            }
            else
            {
                player.Pause();
            }
            toggleText.text = state ? "Pause" : "Play";
        }
    }
}