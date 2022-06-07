using System;
using UnityEngine;

namespace Units
{
    public class GoldPenguin : MonoBehaviour
    {
        public static event Action OnClick;

        private void OnMouseDown()
        {
            OnClick?.Invoke();
        }
    }
}