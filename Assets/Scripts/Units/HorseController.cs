using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Units
{
    public class HorseController : MonoBehaviour
    {
        [SerializeField] private Vector3 _offSet;
        [SerializeField] private GameObject _tmpText;
        [SerializeField] private Transform _spawnPointForText;
        [SerializeField] private float _speedUp;
        [SerializeField] private int _amountToPool;
        private List<GameObject> _tmpTexts;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            CreatingTexts();
        }

        private void CreatingTexts()
        {
            _tmpTexts = new List<GameObject>();
            for (int i = 0; i < _amountToPool; i++)
            {
                GameObject text = Instantiate(_tmpText, _spawnPointForText);
                text.SetActive(false);
                _tmpTexts.Add(text);
            }
        }

        private GameObject GetText()
        {
            foreach (var text in _tmpTexts)
            {
                if (!text.activeInHierarchy)
                {
                    return text;
                }
            }

            GameObject tmpText = Instantiate(_tmpText, _spawnPointForText);
            tmpText.SetActive(false);
            _tmpTexts.Add(tmpText);
            return tmpText;
        }

        public IEnumerator PrintText(float value)
        {
            var text = GetText();

            text.SetActive(true);
            text.transform.position = _camera.WorldToScreenPoint(transform.position + _offSet);
            text.GetComponent<TMP_Text>().text = "+" + Mathf.Round(value);
            var rectText = text.GetComponent<RectTransform>();
            var startPositionY = rectText.anchoredPosition.y;
            while (rectText.anchoredPosition.y < startPositionY + 150)
            {
                rectText.anchoredPosition =
                    new Vector2(rectText.anchoredPosition.x, rectText.anchoredPosition.y + _speedUp);
                yield return new WaitForEndOfFrame();
            }

            text.SetActive(false);
        }
    }
}