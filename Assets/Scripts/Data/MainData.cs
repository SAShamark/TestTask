using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class MainData 
    {
        public float AllBananas { get; set; }
        public int ClickUpdateLevel { get; set; } = 1;
        public int PerSecondLevel { get; set; } = 1;
    }
}