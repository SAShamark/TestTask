using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField] private string _androidGameID = "4787583";
        [SerializeField] private string _iosGameID = "4787582";
        [SerializeField] private bool _testMode = true;

        private void Awake()
        {
            InitializeAds();
        }

        private void InitializeAds()
        {
            var gameID = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iosGameID : _androidGameID;
            Advertisement.Initialize(gameID, _testMode, this);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads initialization failed:{error.ToString()}-{message}");
        }
    }
}