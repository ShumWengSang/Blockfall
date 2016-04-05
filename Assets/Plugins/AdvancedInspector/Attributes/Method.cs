using System;
using System.Collections.Generic;

namespace AdvancedInspector
{
    /// <summary>
    /// Used when inspected a method, gives control over how it is displayed or handled.
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

        public MethodAttribute() { }

        public MethodAttribute(MethodDisplay display)
        {
            this.display = display;
        }
    }
}