using Units.UI;
using UnityEngine;

namespace Units
{
    public class Horse : TextController
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            CreatingTexts();
        }

        public void PrintText(float bananasPerClick)
        {
            StartCoroutine(PrintText(bananasPerClick, _camera));
        }
    }
}