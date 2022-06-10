using UnityEngine;

public class GameInit : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("InterstitialAds"))
        {
            PlayerPrefs.SetInt("InterstitialAds", 1);
        }
    }
}