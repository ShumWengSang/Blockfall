using UnityEngine;
using System.Collections;
using AdvancedInspector;
public class SplitBlock : MonoBehaviour {
    //Usually we want this running on editor time.
    //But just in case we run this at start as wel.


    public GameObject BasePrefab;

    [Inspect, Method(AdvancedInspector.MethodDisplay.Invoke)]
    void SplitTheBlock()
    {
        int FullScaleX = (int)transform.localScale.x;
        int FullScaleY = (int)transform.localScale.y;
        int HalfScaleX = FullScaleX % 2 == 0 ? FullScaleX / 2 : (FullScaleX - 1) / 2;
        int HalfScaleY = FullScaleY % 2 == 0 ? FullScaleY / 2 : (FullScaleY - 1) / 2;
        Debug.Log("Half ScaleX is " + HalfScaleX + " and HalfScaleY is " + HalfScaleY);

        for (int i = 0; i < HalfScaleX / 2; i++)
        {
            GameObject obj = GameObject.Instantiate(BasePrefab, );
        }

        for (int i = 0; i < HalfScaleX / 2; i++)
        {

        }

        for (int i = 0; i < transform.localScale.y; i++)
        {

        }
    }

    void Start()
    {
        SplitTheBlock();
    }
}
