using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Units
{
    public class PenguinPerSecond : MonoBehaviour
    {
        [SerializeField] private Vector3 _offSet;
        [SerializeField] private GameObject _tmpText;
        [SerializeField] private Transform _spawnPointForText;
        [SerializeField] private int _numberBananasPerSecond = 5;
        [SerializeField] private int _timeToGetBananas = 1;
        [SerializeField] private float _speedUp;
        [SerializeField] private int _amountToPool;
        private List<GameObject> _tmpTexts;
        private GameManager _gameManager;
        private Camera _camera;

        public int NumberBananasPerSecond
        {
            get => _numberBananasPerSecond;
            set => _numberBananasPerSecond = value;
        }


        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _camera = Camera.main;
            CreatingTexts();
            StartCoroutine(AddBananas(_timeToGetBananas));
        }

        private IEnumerator AddBananas(float time)
        {
            while (true)
            {
                _gameManager.ChangeNumberBananas(NumberBananasPerSecond);
                StartCoroutine(PrintText(NumberBananasPerSecond));
                yield return new WaitForSeconds(time);
            }
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

        private IEnumerator PrintText(float value)
        {
            var text = GetText();
            if (text != null)
            {
                text.SetActive(true);
                text.transform.position = _camera.WorldToScreenPoint(transform.position + _offSet);
                text.GetComponent<TMP_Text>().text = "+" + Mathf.Round(value);
                var rectText = text.GetComponent<RectTransform>();
                gameObject.transform.Rotate(0, 0, 90);
                var startPosition = rectText.anchoredPosition.y;

                while (rectText.anchoredPosition.y < startPosition + 70)
                {
                    rectText.anchoredPosition =
                        new Vector2(rectText.anchoredPosition.x, rectText.anchoredPosition.y + _speedUp);
                    yield return new WaitForEndOfFrame();
                }

                text.SetActive(false);
            }
        }
    }
}