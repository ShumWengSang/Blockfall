﻿using UnityEngine;

namespace AdvancedInspector
{
    public class AIExample25_Style : MonoBehaviour
    {
        // The Style attribute allows you to change the style that is used for drawing the field.
        // See EditorStyles class for a number of style that are pickable by name.
        // You can find the complete list here; http://kwnetzwelt.net/wordpress/archives/2200
        [Style("ToolbarTextField")]
        public float myField;

        [Inspect, Style("ToolbarButton")]
        public void MyMethod()
        {
            myField++;
        }
    }
}