using TMPro;
using UnityEngine;

namespace Units
{
    public class PenguinClick : MonoBehaviour
    {
        [SerializeField] private Vector3 _offSet;
        [SerializeField] private TMP_Text _tmpText;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            _tmpText.transform.position = _camera.WorldToScreenPoint(transform.position + _offSet);
        }
    }
}