﻿using UnityEngine;

namespace AdvancedInspector
{
    public class AIExample20_RangeValue : MonoBehaviour
    {
        // Unity already have a RangeAttribute.
        // However, this [Range] can only be applied on fields.
        [RangeValue(0, 10)]
        public float myField;

        // However, the RangeValue attribute can also be applied on property.
        [Inspect, RangeValue(0, 10)]
        public float MyProperty
        {
            get { return myField; }
            set { myField = value; }
        }

        // Unity's range attribute also works, but cannot be applied to properties. 
        [Range(0, 10)]
        public float unityRange;
    }
}