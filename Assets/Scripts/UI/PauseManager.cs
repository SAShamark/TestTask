using Data;
using UnityEngine;

namespace UI
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private SaveData _saveData;

        public void OnPauseAndSave()
        {
            Time.timeScale = 0;
            _saveData.Save();
        }

        public void OffPause()
        {
            Time.timeScale = 1;
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void DeleteAllKey()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
