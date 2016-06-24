using UnityEngine;
using System.Collections;
using AdvancedInspector;
public class SplitBlock : MonoBehaviour {
    //Usually we want this running on editor time.
    //But just in case we run this at start as wel.


    public GameObject BasePrefab;

    [Inspect, Method(AdvancedInspector.MethodDisplay.Button)]
    void SplitTheBlock()
    {
        if (transform.localScale == Vector3.one)
            return;
        int FullScaleX = (int)transform.localScale.x;
        int FullScaleY = (int)transform.localScale.y;
        int HalfScaleX = FullScaleX % 2 == 0 ? FullScaleX / 2 : (FullScaleX - 1) / 2;
        int HalfScaleY = FullScaleY % 2 == 0 ? FullScaleY / 2 : (FullScaleY - 1) / 2;
        float OffSetX = FullScaleX % 2 == 0 ? 0.5f : 0.0f;
        Debug.Log("Half ScaleX is " + HalfScaleX + " and HalfScaleY is " + HalfScaleY);

        for (int i = 0; i < HalfScaleX; i++)
        {

            GameObject obj = Instantiate(BasePrefab, new Vector3(transform.position.x + i + 1 + OffSetX, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            obj.transform.parent = transform.parent;
        }

        for (int i = 0; i < HalfScaleX; i++)
        {
            GameObject obj;
            if(HalfScaleX % 2 == 0)
            {
                obj = Instantiate(BasePrefab, new Vector3(transform.position.x - i + OffSetX, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            }
            else
            {
                obj = Instantiate(BasePrefab, new Vector3(transform.position.x - i - 1 + OffSetX, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            }

            obj.transform.parent = transform.parent;

        }

        for (int i = 0; i < transform.localScale.y; i++)
        {

        }
        if (Application.isPlaying)
            Destroy(this.gameObject);
        else
            DestroyImmediate(this.gameObject);
    }

    void Start()
    {
        SplitTheBlock();
    }
}
