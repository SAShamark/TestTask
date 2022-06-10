using System.Collections;
using DG.Tweening;
using Units.UI;
using UnityEngine;

namespace Units
{
    public class PenguinPerSecond : TextController
    {
        [SerializeField] private int _numberBananasPerSecond = 5;
        [SerializeField] private int _timeToGetBananas = 1;
        private GameMechanicsManager _gameMechanicsManager;
        private const float RotateSpeed = 0.1f;
        private Camera _camera;

        public int NumberBananasPerSecond
        {
            get => _numberBananasPerSecond;
            set => _numberBananasPerSecond = value;
        }


        private void Start()
        {
            _gameMechanicsManager = FindObjectOfType<GameMechanicsManager>();
            _camera = Camera.main;
            CreatingTexts();
            StartCoroutine(AddBananas(_timeToGetBananas));
        }

        private IEnumerator AddBananas(float time)
        {
            while (true)
            {
                _gameMechanicsManager.ChangeNumberBananas(NumberBananasPerSecond);
                ChangeRotation();
                StartCoroutine(PrintText(NumberBananasPerSecond, _camera));
                yield return new WaitForSeconds(time);
            }
        }

        private void ChangeRotation()
        {
            var gameObjectRotate = new Vector3(0, 0, transform.rotation.eulerAngles.z + 90);
            transform.DORotate(gameObjectRotate, RotateSpeed);
        }
    }
}