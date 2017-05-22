using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

    public List<Color> portalColors;
    //List<PortalPair> PortalPairs;
    Dictionary<Portal, Portal> DicPortal;


	// Use this for initialization
	public void Init () {
        DicPortal = new Dictionary<Portal, Portal>();
        //PortalPairs = new List<PortalPair>();

        //Find all the portals
        //find all the blocks
        List<Portal> blks = new List<Portal>();

        Transform[] allPortals = GameObject.FindObjectsOfType<Transform>();
        foreach (Transform obj in allPortals)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                //we use game component snapobject to identify if its a portal...
                if (obj.GetComponent<Portal>() != null)
                {
                    blks.Add(obj.GetComponent<Portal>());
                }
            }
        }

        for(int i = 0; i < blks.Count; i++)
        { 
            if(!DicPortal.ContainsKey(blks[i]) && !DicPortal.ContainsValue(blks[i]))
            {
                DicPortal.Add(blks[i], blks[i].OtherPortal);
            }
        }
        if(DicPortal.Count < 0 )
        {
            return;
        }
        int Counter = 0;
        foreach(var portal in DicPortal)
        {
            portal.Key.GetComponent<SpriteRenderer>().color = portalColors[Counter];
            portal.Value.GetComponent<SpriteRenderer>().color = portalColors[Counter];
            Counter++;
            if(Counter >= 4)
            {
                Counter = 0;
            }
        }
    }
}
