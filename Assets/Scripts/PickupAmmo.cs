using UnityEngine;
using System.Collections;

public class PickupAmmo : MonoBehaviour {

    public GameObject AmmoObject;
    public GameObject gun;

    Hashtable rotateHt = new Hashtable();
    float timeToReactivate = 3;

	// Use this for initialization
	void Start () {

        //setting for ammo box rotation
        rotateHt.Add("y", 360);
        rotateHt.Add("time", 1.5f);
        rotateHt.Add("easetype", "linear");
        rotateHt.Add("LoopType", "loop");
        iTween.RotateAdd(gameObject, rotateHt);     // start ammo box rotation tween

        gun = GameObject.Find("CartoonSMG");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void deactivate()
    {
        StartCoroutine(activateAfterSec());
        AmmoObject.SetActive(false);
    }
    void activate()
    {
        AmmoObject.SetActive(true);
    }

    IEnumerator activateAfterSec()
    {
        yield return new WaitForSeconds(timeToReactivate);
        activate();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gun.GetComponent<Gun>().pickupAmmo(40);
            deactivate();
        }
    }

    
}
