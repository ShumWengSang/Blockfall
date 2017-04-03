﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AdvancedInspector
{
    /// <summary>
    /// When affixes to a collection, prevent this collection's size to be modified by the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CollectionAttribute : Attribute, IListAttribute, IRuntimeAttribute<string[]>
    {
        public delegate string[] CollectionDelegate();
        public delegate string[] CollectionStaticDelegate(CollectionAttribute collection, object instance, object value);

        private int size = -1;

        /// <summary>
        /// Size of this collection.
        /// Default -1; size is not controlled by code.
        /// 0 means the collection's size will be handled internally.
        /// > 0 indicate the same of the collection.
        /// </summary>
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        private bool sortable = true;

        /// <summary>
        /// If true, the list can be sorted by hand.
        /// </summary>
        public bool Sortable
        {
            get { return sortable; }
            set { sortable = value; }
        }

        private CollectionDisplay display = CollectionDisplay.List;

        /// <summary>
        /// If not default, removes the collection list and only display one item at a time.
        /// </summary>
        public CollectionDisplay Display
        {
            get { return display; }
            set { display = value; }
        }

        private int maxDisplayedItems = 25;

        /// <summary>
        /// When a collection is very large, it get up/down arrows to scrolls in items instead of displaying them all.
        /// This property controls how many items are displayed before the scrolls appears. 
        /// </summary>
        public int MaxDisplayedItems
        {
            get { return maxDisplayedItems; }
            set { maxDisplayedItems = value; }
        }

        private int maxItemsPerRow = 6;

        /// <summary>
        /// When display is using Button, this is the maximum number of button displayed per rows before creating a new one.
        /// </summary>
        public int MaxItemsPerRow
        {
            get { return maxItemsPerRow; }
            set { maxItemsPerRow = value; }
        }

        private bool alwaysExpanded = false;

        /// <summary>
        /// The AlwaysExpanded property of the Expandable attribute doesn't work on the root collection, because of the IListAttribute.
        /// This is the work around to make the collection always expanded.
        /// </summary>
        public bool AlwaysExpanded
        {
            get { return alwaysExpanded; }
            set { alwaysExpanded = value; }
        }

        private bool expandElements = false;

        /// <summary>
        /// Collection element - if expandable - will be expanded by default.
        /// </summary>
        public bool ExpandElements
        {
            get { return expandElements; }
            set { expandElements = value; }
        }

        private Type enumType = null;

        /// <summary>
        /// Bind the size of a collection to the values of an enum.
        /// The name of the indexed are displayed using the enum values' names. 
        /// </summary>
        public Type EnumType
        {
            get { return enumType; }
            set
            {
#if !NETFX_CORE
                if (!value.IsEnum)
                    return;
#endif

                int index = 0;
                foreach (object i in Enum.GetValues(value))
                {
                    if ((int)i != index)
                        return;

                    index++;
                }

                enumType = value;
            }
        }

        #region IRuntime Implementation
        private string methodName = "";

        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }

        public Type Template
        {
            get { return typeof(CollectionDelegate); }
        }

        public Type TemplateStatic
        {
            get { return typeof(CollectionStaticDelegate); }
        }

        private List<Delegate> delegates = new List<Delegate>();

        public List<Delegate> Delegates
        {
            get { return delegates; }
            set { delegates = value; }
        }

        public string[] Invoke(int index, object instance, object value)
        {
            if (delegates.Count == 0 || index >= delegates.Count)
                return new string[0];

            try
            {
                if (delegates[index].Target == null)
                {
                    return ((string[])delegates[index].DynamicInvoke(this, instance, value));
                }
                else
                {
                    return ((string[])delegates[index].DynamicInvoke());
                }
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException)
                    e = ((TargetInvocationException)e).InnerException;

                Debug.LogError(string.Format("Invoking a method to retrieve a Collection attribute failed. The exception was \"{0}\".", e.Message));
                return new string[0];
            }
        }
        #endregion

        #region Compile Time Constructors
        public CollectionAttribute() { }

        public CollectionAttribute(int size)
            : this(size, true) { }

        public CollectionAttribute(Type enumType)
            : this(enumType, true) { }

        public CollectionAttribute(bool sortable)
            : this(-1, sortable) { }

        public CollectionAttribute(string methodName)
            : this(methodName, -1, true, CollectionDisplay.List) { }

        public CollectionAttribute(CollectionDisplay display)
            : this("", -1, true, display) { }

        public CollectionAttribute(int size, bool sortable)
            : this("", size, sortable, CollectionDisplay.List) { }

        public CollectionAttribute(Type enumType, bool sortable)
            : this(enumType, sortable, CollectionDisplay.List) { }

        public CollectionAttribute(int size, CollectionDisplay display)
            : this("", size, true, display) { }

        public CollectionAttribute(Type enumType, CollectionDisplay display)
            : this(enumType, true, display) { }

        public CollectionAttribute(string methodName, int size, bool sortable, CollectionDisplay display)
        {
            this.methodName = methodName;
            this.size = size;
            this.sortable = sortable;
            this.display = display;
        }

        public CollectionAttribute(Type enumType, bool sortable, CollectionDisplay display)
        {
            this.enumType = enumType;
            this.sortable = sortable;
            this.display = display;
        }
        #endregion

        #region Runtime Constructor
        public CollectionAttribute(Delegate method)
            : this(method, -1, true, CollectionDisplay.List) { }

        public CollectionAttribute(Delegate method, int size)
            : this (method, size, true, CollectionDisplay.List) { }

        public CollectionAttribute(Delegate method, bool sortable)
            : this(method, -1, sortable, CollectionDisplay.List) { }

        public CollectionAttribute(Delegate method, CollectionDisplay display)
            : this(method, -1, true, display) { }

        public CollectionAttribute(Delegate method, int size, bool sortable, CollectionDisplay display)
        {
            this.size = size;
            this.sortable = sortable;
            this.display = display;
            this.delegates.Add(method);
        }
        #endregion
    }
}