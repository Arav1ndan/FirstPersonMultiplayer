using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform ViewPoint;
    public float MouseSensitivity = 1f;
    private float VerticalRoationStore;
    private Vector2 MouseInput;
    void Start()
    {
        
    }

   
    void Update()
    {
        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"),Input.GetAxisRaw("Mouse Y")) * MouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y + MouseInput.x,transform.rotation.eulerAngles.x);

        VerticalRoationStore += MouseInput.y;
        VerticalRoationStore = Mathf.Clamp(VerticalRoationStore, -60f,60);

        ViewPoint.rotation = Quaternion.Euler(-VerticalRoationStore, ViewPoint.rotation.eulerAngles.y,ViewPoint.rotation.eulerAngles.z);


    }
}
