using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] boosts;
    [SerializeField] private GameStatus gs;
    private DriverControl dc;

    [SerializeField] private float timeForBoost = 10f;
    [SerializeField] private bool isThereBoost = false;
    [SerializeField] private float boostStayTime = 3f;

    private GameObject currentCreatedBoost;

    // Start is called before the first frame update
    void Start()
    {
        dc = GetComponentInParent<DriverControl>();
        gs = FindObjectOfType<GameStatus>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!isThereBoost && timeForBoost > 0)
            timeForBoost -= 1*Time.deltaTime;

        if(isThereBoost && boostStayTime <= 0){
            Debug.Log("to be destroyed: " + currentCreatedBoost.name);
            Destroy(currentCreatedBoost);
            isThereBoost = false;
            currentCreatedBoost = null;
        }

        else if(isThereBoost && boostStayTime > 0)
            boostStayTime -= 1 * Time.deltaTime;

        if(timeForBoost <= 0 && gs.isThereTime() && !isThereBoost){
            createBoost();
        }

    }

    void createBoost(){
        //choose a random boost from the list
        int chosenIndex = (int) Random.Range(0, boosts.Length-1);

        //create instantiation location
        Vector2 creationLocation = dc.transform.position + dc.transform.up * 10;
        Collider2D tmp = Physics2D.OverlapCircle(creationLocation, 2);
        if(tmp != null){
            return;
        }

        //create a boost for 5 seconds
        currentCreatedBoost = Instantiate(boosts[chosenIndex], creationLocation, Quaternion.identity);

        Debug.Log("created : " + currentCreatedBoost.name);

        isThereBoost = true;
        boostStayTime = 3f;
        timeForBoost = 10f;
    }

}
