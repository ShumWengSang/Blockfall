﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data_Blocks
{

    //Heiracrchy
    /*
      --SCENE_BLOCK
        --block_data      
    */

    [System.Serializable]
    public class Scene_Block
    {
        [SerializeField]
        public List<block_data> blocks;
        public int world, level;
    }

    [System.Serializable]
    public class block_data
    {
        public custom_vector_4 pos;
        public custom_vector_4 scale;
        public custom_vector_4 portal_ref_pos;
        public blockType type;
        public Direction direction;
    }

    [System.Serializable]
    public class custom_vector_4
    {
        public custom_vector_4()
        { }
        public custom_vector_4(float x, float y, float z, float w)
        { this.x = x; this.y = y; this.z = z; this.w = w; }
        public custom_vector_4(Vector3 vec)
        { x = vec.x; y = vec.y; z = vec.z; w = 0; }
        public custom_vector_4(Vector4 vec)
        { x = vec.x; y = vec.y; z = vec.z; w = vec.w; }
        public float x, y, z, w;
    }

    [System.Serializable]
    public enum blockType
    {
        wooden,
        goal,
        vortex,
        iron,
        portal,
        one_way_b,
        one_way_g,
        walls,
        none
    }
}