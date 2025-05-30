using UnityEngine;

namespace BridgeEdu.UI {
    [RequireComponent(typeof(Camera))]
    public class ScreenResolutionController : MonoBehaviour {
        private struct Resolution {
            public int width;
            public int height;
        }
        public float targetAspectRatio = 16f / 9f;

        void OnEnable() {
            LoadUserSettings();
        }

        public void ApplyResolutionSettings(int width, int height) {
            // Apply the new resolution
            Screen.SetResolution(width, height, FullScreenMode.Windowed);

            // Save settings
            PlayerPrefs.SetInt("ResolutionWidth", width);
            PlayerPrefs.SetInt("ResolutionHeight", height);
            PlayerPrefs.Save(); // Save changes immediately

            Debug.Log($"Applied Resolution: {width}x{height}");
        }

        private void LoadUserSettings() {
            // Load from PlayerPrefs or current screen settings
            int currentWidth = PlayerPrefs.GetInt("ResolutionWidth", Screen.currentResolution.width);
            int currentHeight = PlayerPrefs.GetInt("ResolutionHeight", Screen.currentResolution.height);

            Screen.SetResolution(currentWidth, currentHeight, FullScreenMode.Windowed);
        }
    }
}