using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryController : MonoBehaviour
{
    private int choosen;
    private bool haveLocation = false;
    [SerializeField]private DriverControl dc;
    [SerializeField]private GameObject[] locations;

    // Start is called before the first frame update
    void Start()
    {
        dc = FindObjectOfType<DriverControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(haveLocation == false) {
            //if we need a location for package
             if(!dc.ifTookPackage()){
                choosen = GenerateLocation();
                locations[choosen].tag = "package";
                locations[choosen].SetActive(true);
                haveLocation = true;
             }
             //if we need a location for delivery point
             else{
                choosen = GenerateLocation();
                locations[choosen].tag = "delivery";
                locations[choosen].SetActive(true);
                haveLocation = true;
             }
        }
    }

    int GenerateLocation()
    { 
        int tmp = (int) Random.Range(0f,(float)(locations.Length));   
        if(tmp != choosen)
            return tmp;
        return GenerateLocation();
    }

    public void SetHaveLocation(bool b){
        haveLocation = b;
    }

    public bool IfHaveLocation() {
        return haveLocation;
    }

    public Vector3 LocationOfPlace(){
        return locations[choosen].transform.position; 
    }
}
