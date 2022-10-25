using UnityEngine;

public class DeliveryController : MonoBehaviour
{
    [SerializeField] private Sprite package;
    [SerializeField] private Sprite destination;
    [SerializeField] private RuntimeAnimatorController packageAnim;
    [SerializeField] private RuntimeAnimatorController destAnim;
    [SerializeField] private DriverControl dc;

    [SerializeField] private GameObject[] locations;

    private int choosen;
    private bool haveLocation;

    private GameObject selectedObject;
    private Animator tmpAnimator;
    private SpriteRenderer tmpSpriteRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        dc = FindObjectOfType<DriverControl>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (haveLocation == false)
        {
            SetCurrentObject();
            //if we need a location for package
            if (!dc.ifTookPackage())
            {
                selectedObject.tag = "package";
                tmpAnimator.runtimeAnimatorController = packageAnim;
                tmpSpriteRenderer.sprite = package;
                selectedObject.SetActive(true);
            }
            //if we need a location for delivery point
            else
            {
                selectedObject.tag = "delivery";
                tmpAnimator.runtimeAnimatorController = destAnim;
                tmpSpriteRenderer.sprite = destination;
                selectedObject.SetActive(true);
            }
        }
    }


    private int GenerateIndex()
    {
        var tmp = (int)Random.Range(0f, locations.Length);
        if (tmp != choosen)
            return tmp;
        return GenerateIndex();
    }

    private void SetCurrentObject()
    {
        choosen = GenerateIndex();
        selectedObject = locations[choosen];
        haveLocation = true;
        tmpAnimator = selectedObject.GetComponent<Animator>();
        tmpSpriteRenderer = selectedObject.GetComponent<SpriteRenderer>();
    }

    public void SetHaveLocation(bool b)
    {
        haveLocation = b;
    }

    public bool IfHaveLocation()
    {
        return haveLocation;
    }

    public Vector3 LocationOfPlace()
    {
        return locations[choosen].transform.position;
    }
}
