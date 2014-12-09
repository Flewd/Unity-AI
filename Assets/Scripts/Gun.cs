using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    int damage = 2;

    public GameObject crossair;
    public GameObject muzzleFlash;
    public GameObject GunModel;
    public GameObject ClipModel;

    float fireRate = 0.15f;
    float gunTimer = 0;


    int clipMax = 20;
    int ammoClip = 20;
    int ammoTotal = 60;
    float reloadSpeed = 2f;
    bool reloadStarted = false;
    Vector3 clipDefaultPos;
    public GameObject ammoClipGuiTxt;
    public GameObject ammoTotalGuiTxt;

    Hashtable reloadHt1 = new Hashtable();
    Hashtable reloadHt2 = new Hashtable();
	// Use this for initialization
	void Start () {

        ammoClipGuiTxt.guiText.text = ammoClip.ToString();
        ammoTotalGuiTxt.guiText.text = ammoTotal.ToString();

        clipDefaultPos = ClipModel.transform.localPosition;

        reloadHt1.Add("z", -4);
        reloadHt1.Add("time", reloadSpeed/2);
        reloadHt1.Add("oncomplete", "playSecondHalfOfReload");
        reloadHt1.Add("onCompleteTarget", gameObject);

        reloadHt2.Add("z", 4);
        reloadHt2.Add("time", reloadSpeed/2);
        reloadHt2.Add("easetype", "linear");
	}
    void OnEnable()
    {
        /*
        if (reloadStarted == true)
        {
            ClipModel.transform.localPosition = clipDefaultPos;
            reloadStarted = false;
        }
         * */
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 fwd = crossair.transform.TransformDirection(Vector3.forward);   //direction gun fires in

        //left mouse initialy pressed
        if (Input.GetMouseButtonDown(0))
        {
            if (ammoClip > 0 && reloadStarted == false )    //check if you have ammo in clip before shooting
            {
                muzzleFlash.GetComponent<ParticleSystem>().Play();  //play muzzle flash particle
                gunTimer = 0;       //initialize gunTimer to zero 

                RaycastHit hit;

                if (Physics.Raycast(crossair.transform.position, fwd, out hit))     //shoot raycast at the position of the crosshair
                {
                    if (hit.transform.tag == "creep")       //if the ray hits a creep
                    {
                        hit.transform.gameObject.GetComponent<CreepController>().applyDamage(damage);   //apply the damage of the gun to the creep that was hit
                    }
                }
                ammoClip--;     //subtract ammo from clip
                ammoClipGuiTxt.guiText.text = ammoClip.ToString();  //update gui
            }
            else if (reloadStarted == false && ammoTotal > 0)   //if no ammo then reload
            {
                iTween.MoveBy(ClipModel, reloadHt1);    //tween clip down

                StartCoroutine(reload());   //start timer for how long reload takes
                reloadStarted = true;       //set reloadStarted flag so you can't shoot while gun is reloading
            }

        }

        //left mouse held down (mostly the same as above. i commented the differences
        if (Input.GetMouseButton(0))
        {
            if (gunTimer >= fireRate)   // only shoot if the time between bullets is greater then firerate
            {
                if (ammoClip > 0 && reloadStarted == false)
                {
                    muzzleFlash.GetComponent<ParticleSystem>().Play();

                    RaycastHit hit;

                    if (Physics.Raycast(crossair.transform.position, fwd, out hit))
                    {
                        if (hit.transform.tag == "creep")
                        {
                            hit.transform.gameObject.GetComponent<CreepController>().applyDamage(damage);
                        }
                    }
                    gunTimer = 0;
                    ammoClip--;
                    ammoClipGuiTxt.guiText.text = ammoClip.ToString();
                }
                else if(reloadStarted == false && ammoTotal > 0)   
                {
                    iTween.MoveBy(ClipModel, reloadHt1);

                    StartCoroutine(reload());
                    reloadStarted = true;
                }
            }
            gunTimer += Time.deltaTime;     //update guntimer
        }

        //get key for reload
        if (Input.GetKeyDown("r"))  //if the r key is pressed
        {
            if (reloadStarted == false && ammoClip < clipMax && ammoTotal > 0)     //if reload is not already in progress, and ammo clip is not already full, and you have more then 0 ammo
            {
                iTween.MoveBy(ClipModel, reloadHt1);    //start reload animation by tweening clip down

                StartCoroutine(reload());   //start timer for how long reload takes
                reloadStarted = true;       //set reloadStarted flag so you can't shoot while gun is reloading
            }
        }
	}

    IEnumerator reload()
    {
        yield return new WaitForSeconds(reloadSpeed);

        //math stuff to check how much ammo is deducted from total ammo and such

        int clipDifference = clipMax - ammoClip;
        int ammoTotalBeforeDifference = ammoTotal;
        ammoTotal -= clipDifference;
        if (ammoTotal < 0)
        {
            ammoTotal = 0;
        }

        if (ammoTotal < clipMax)
        {
            ammoClip = ammoTotalBeforeDifference;
        }
        else
        {
            ammoClip = clipMax;
        }

        ammoClipGuiTxt.guiText.text = ammoClip.ToString();
        ammoTotalGuiTxt.guiText.text = ammoTotal.ToString();
        reloadStarted = false;
    }

    public void playSecondHalfOfReload()
    {
         iTween.MoveBy(ClipModel, reloadHt2);
    }
    public void pickupAmmo(int amount)  //this is called when player walks over an ammo crate
    {
        ammoTotal += amount;
        ammoTotalGuiTxt.guiText.text = ammoTotal.ToString();
    }
}
