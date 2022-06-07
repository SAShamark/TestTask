using System;
using UnityEngine;

public class GoldPenguin : MonoBehaviour
{
    public static event Action OnClick;

    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }
}