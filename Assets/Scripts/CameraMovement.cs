using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject car;
    
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position = car.transform.position + new Vector3(0, 0, -1);
        // transform.rotation = Quaternion.Euler(new Vector3(0,0, car.transform.localRotation.eulerAngles.z));
    }
}