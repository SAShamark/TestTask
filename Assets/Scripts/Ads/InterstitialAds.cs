using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private string _androidAdId = "Interstitial_Android";
        [SerializeField] private string _iOSAdID = "Interstitial_iOS";
        private string _adId;

        private void Awake()
        {
            _adId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSAdID : _androidAdId;
            LoadAd();
        }

        private void LoadAd()
        {
            Debug.Log("Loading Ad:" + _adId);
            Advertisement.Load(_adId, this);
        }

        public void ShowAd()
        {
            Debug.Log("Showing Ad:" + _adId);
            Advertisement.Show(_adId, this);
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            LoadAd();
        }
    }
}