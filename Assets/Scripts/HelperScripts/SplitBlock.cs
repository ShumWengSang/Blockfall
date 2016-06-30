using UnityEngine;
using System.Collections;
using AdvancedInspector;
[ExecuteInEditMode]
public class SplitBlock : MonoBehaviour {
    //Usually we want this running on editor time.
    //But just in case we run this at start as wel.


    GameObject BasePrefab;

    [Inspect, Method(AdvancedInspector.MethodDisplay.Button)]
    void SplitTheBlock()
    {
        if (transform.localScale == Vector3.one)
            return;

        BasePrefab = Resources.Load("Prefabs/Block Prefabs/Wall Bock") as GameObject;
        int FullScaleX = (int)transform.localScale.x;
        int FullScaleY = (int)transform.localScale.y;
        int HalfScaleX = FullScaleX % 2 == 0 ? FullScaleX / 2 : (FullScaleX - 1) / 2;
        int HalfScaleY = FullScaleY % 2 == 0 ? FullScaleY / 2 : (FullScaleY - 1) / 2;
        float OffSetX = FullScaleX % 2 == 0 ? 0.5f : 0.0f;
        float OffSetY = FullScaleY % 2 == 0 ? 0.5f : 0.0f;
        Debug.Log("Half ScaleX is " + HalfScaleX + " and HalfScaleY is " + HalfScaleY + " and offset is " + OffSetX);

        float CalculatedTransformStartPosX = transform.position.x - (HalfScaleX - OffSetX);
        float CalculatedTransformStartPosY = transform.position.y - (HalfScaleY - OffSetY);
        Debug.Log("Calculated new Transform start is " + CalculatedTransformStartPosX);


        for (int i = 0, j = 0; i < FullScaleX; i++)
        {
            //GameObject obj = Instantiate(BasePrefab, new Vector3(CalculatedTransformStartPosX + i, CalculatedTransformStartPosY + j, transform.position.z), Quaternion.identity) as GameObject;
            //obj.transform.parent = transform.parent;

            for (j = 0; j < FullScaleY; j++)
            {
                GameObject obj2 = Instantiate(BasePrefab, new Vector3(CalculatedTransformStartPosX + i, CalculatedTransformStartPosY + j, transform.position.z), Quaternion.identity) as GameObject;
                obj2.transform.parent = transform.parent;
            }

        }

        


        if (Application.isPlaying)
            Destroy(this.gameObject);
        else
            DestroyImmediate(this.gameObject);
    }

    void Start()
    {
        if(!Application.isPlaying)
            SplitTheBlock();
    }
}
