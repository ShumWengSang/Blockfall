using System;

using UnityEditor;
using UnityEngine;

namespace AdvancedInspector
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SkinnedMeshRenderer), true)]
    public class SkinnedMeshRendererEditor : RendererEditor
    {
        protected override void RefreshFields()
        {
            base.RefreshFields();
            Type type = typeof(SkinnedMeshRenderer);

            Fields.Add(new InspectorField(type, Instances, type.GetProperty("quality"),
                new DescriptorAttribute("Quality", "The maximum number of bones affecting a single vertex.", "http://docs.unity3d.com/ScriptReference/SkinnedMeshRenderer-quality.html")));
            Fields.Add(new InspectorField(type, Instances, type.GetProperty("updateWhenOffscreen"),
                new DescriptorAttribute("Update Off Screen", "If enabled, the Skinned Mesh will be updated when offscreen. If disabled, this also disables updating animations.", "http://docs.unity3d.com/ScriptReference/SkinnedMeshRenderer-updateWhenOffscreen.html")));
            
#if UNITY_5_6
            Fields.Add(new InspectorField(type, Instances, type.GetProperty("skinnedMotionVectors"),
                new DescriptorAttribute("Skinned Motion Vectors", "Specifies whether skinned motion vectors should be used for this renderer.", "https://docs.unity3d.com/ScriptReference/SkinnedMeshRenderer-skinnedMotionVectors.html")));
#endif

            Fields.Add(new InspectorField(type, Instances, type.GetProperty("sharedMesh"),
                new DescriptorAttribute("Mesh", "The mesh used for skinning.", "http://docs.unity3d.com/ScriptReference/SkinnedMeshRenderer-sharedMesh.html")));
            Fields.Add(new InspectorField(type, Instances, type.GetProperty("rootBone"), 
                new DescriptorAttribute("Root", "The root boot of the skeletton hierarchy.")));
            Fields.Add(new InspectorField(type, Instances, type.GetProperty("bones"),
                new CollectionAttribute(-1), new DescriptorAttribute("Bones", "The bones used to skin the mesh.", "http://docs.unity3d.com/ScriptReference/SkinnedMeshRenderer-bones.html"), new InspectAttribute(InspectorLevel.Advanced)));

            Fields.Add(new InspectorField(type, Instances, type.GetProperty("localBounds"),
                new DescriptorAttribute("Bounds", "AABB of this Skinned Mesh in its local space.", "http://docs.unity3d.com/ScriptReference/SkinnedMeshRenderer-localBounds.html"), new InspectAttribute(InspectorLevel.Advanced)));

            /*InspectorField blendShape = new InspectorField("Blendshapes");
            blendShape.AddAttribute(new InspectAttribute(new InspectAttribute.InspectDelegate(HasBlendshapes)));

            // Insert future blend shape... As soon as I know how.

            Fields.Add(blendShape);*/
        }

        private bool HasBlendshapes()
        {
            foreach (object instance in Instances)
            {
                SkinnedMeshRenderer skin = instance as SkinnedMeshRenderer;
                if (skin == null || skin.sharedMesh == null)
                    return false;

                if (skin.sharedMesh.blendShapeCount > 0)
                    return true;
            }

            return false;
        }
    }
}
