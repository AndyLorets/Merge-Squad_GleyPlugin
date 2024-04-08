using System;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace D2D.Utils
{
    [Serializable]
    public class ProductionData
    {
        public string appName;
        public string companyName;
        public string className;
        public int version = 1;
    }
}