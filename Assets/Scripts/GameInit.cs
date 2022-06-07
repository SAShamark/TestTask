using UnityEngine;

public class GameInit : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("FirstStart"))
        {
            PlayerPrefs.SetInt("FirstStart", 1);
            PlayerPrefs.SetFloat("AllBananas", 0);
            PlayerPrefs.SetInt("ClickLevel", 1);
            PlayerPrefs.SetInt("PerSecondLevel", 1);
        }
    }
}