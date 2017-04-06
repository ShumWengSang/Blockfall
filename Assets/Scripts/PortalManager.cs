using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

    public List<Color> portalColors;
    //List<PortalPair> PortalPairs;
    Dictionary<Portal, Portal> DicPortal;
    class PortalPair
    {
        public PortalPair(GameObject one, GameObject two)
        {
            portal_one = one;
            portal_two = two;
        }
        GameObject portal_one;
        GameObject portal_two;
    }

	// Use this for initialization
	void Start () {
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

        foreach (Portal portal in blks)
        {
            if(!DicPortal.ContainsKey(portal) && !DicPortal.ContainsValue(portal))
            {
                DicPortal.Add(portal, portal.OtherPortal);
            }
        }

        int Counter = 0;
        foreach(var portal in DicPortal)
        {
            portal.Key.GetComponent<SpriteRenderer>().color = portalColors[Counter];
            portal.Value.GetComponent<SpriteRenderer>().color = portalColors[Counter];
            Counter++;
        }
    }
}
