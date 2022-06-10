using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Units.UI
{
    public class TextController : MonoBehaviour
    {
        [SerializeField] private Vector3 _offSet;
        [SerializeField] private GameObject _tmpText;
        [SerializeField] private Transform _spawnPointForText;
        [SerializeField] private float _speedForTextUp;
        [SerializeField] private float _plusToYPosition;
        [SerializeField] private int _amountTextToPool;
        private List<GameObject> _tmpTexts;

        private void Start()
        {
            CreatingTexts();
        }

        protected void CreatingTexts()
        {
            _tmpTexts = new List<GameObject>();
            for (int i = 0; i < _amountTextToPool; i++)
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

        protected IEnumerator PrintText(float value, Camera camera)
        {
            var text = GetText();
            text.SetActive(true);
            text.transform.position = camera.WorldToScreenPoint(transform.position + _offSet);
            var tmpText = text.GetComponent<TMP_Text>();

            tmpText.text = "+" + Mathf.Round(value);
            var rectText = text.GetComponent<RectTransform>();
            var startPositionY = rectText.anchoredPosition.y;
            while (rectText.anchoredPosition.y < startPositionY + _plusToYPosition)
            {
                rectText.anchoredPosition =
                    new Vector2(rectText.anchoredPosition.x, rectText.anchoredPosition.y + _speedForTextUp);
                yield return new WaitForEndOfFrame();
            }

            text.SetActive(false);
        }
    }
}