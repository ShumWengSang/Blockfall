using UnityEngine;
using System.Collections;

public class CloseObjectAfterAnimation : MonoBehaviour {
    public float time;
    WaitForSeconds wait;
    void Start()
    {
        wait = new WaitForSeconds(time);
    }
    public void closeObject()
    {
        StartCoroutine(destroy());
    }

    IEnumerator destroy()
    {
        yield return wait;
        this.gameObject.SetActive(false);   
    }
}
