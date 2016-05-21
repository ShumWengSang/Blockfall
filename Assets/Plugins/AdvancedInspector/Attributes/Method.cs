using System;
using System.Collections.Generic;

namespace AdvancedInspector
{
    /// <summary>
    /// Used when inspected a method, gives control over how it is displayed or handled.
    /// If the undo message is not an empty string, the inspector will attempt to record a undo stack of the modified self-object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodAttribute : Attribute
    {
        private MethodDisplay display = MethodDisplay.Button;

        public MethodDisplay Display
        {
            get { return display; }
            set { display = value; }
        }

        private string undoMessageOnClick = "";

        public string UndoMessageOnClick
        {
            get { return undoMessageOnClick; }
            set { undoMessageOnClick = value; }
        }

        public MethodAttribute() { }

        public MethodAttribute(MethodDisplay display)
        {
            this.display = display;
        }
    }
}