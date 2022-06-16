using System.Collections;
using System.IO;
using UnityEngine;

namespace Data
{
    public class SaveData : MonoBehaviour
    {
        private GameMechanicsManager _gameMechanicsManager;
        private MainData _mainData;
        private string _savePath;

        private void Awake()
        {
            _mainData = new MainData();

#if UNITY_ANDROID && !UNITY_EDITOR
            _savePath = Path.Combine(Application.persistentDataPath, "Save.json");
#else
            _savePath = Path.Combine(Application.dataPath, "Save.json");
#endif
            _gameMechanicsManager = GameMechanicsManager.SingletonGameMechanicsManager;
            print(_gameMechanicsManager);
        }

        private void Start()
        {
            StartCoroutine(SaveMainData());
        }

        private IEnumerator SaveMainData()
        {
            while (true)
            {
                yield return new WaitForSeconds(30);
                Save();
            }
        }

        public void Save()
        {
            _mainData = _gameMechanicsManager._mainData;
            string data = JsonUtility.ToJson(_mainData);
            File.WriteAllText(_savePath, data);
        }

        public void Load()
        {
            if (File.Exists(_savePath))
            {
                string fileData = File.ReadAllText(_savePath);
                _mainData = JsonUtility.FromJson<MainData>(fileData);
                _gameMechanicsManager._mainData = _mainData;
            }
        }
    }
}