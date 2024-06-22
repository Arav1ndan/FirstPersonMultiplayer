using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform ViewPoint;
    [SerializeField]
    private float MouseSensitivity = 1f;
    private float VerticalRoationStore;
    private Vector2 MouseInput;
    [SerializeField]
    private float MoveSpeed = 5f, RunSpeed = 8f;
    private float ActiveMoveSpeed;
    private Vector3 MoveDirection, Movement;
    public float JumpForce = 12f ,gravityMod = 2.5f;

    public Transform groundChecckPoint;
    private bool isGrounded;
    public LayerMask groundLayer;
    public GameObject bulletImapact;
    public float timeBetweenShots =.1f;
    private float shotCounter;
    [SerializeField]
    private CharacterController CharCont;
    private Camera cam;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
    }

   
    void Update()
    {
        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"),Input.GetAxisRaw("Mouse Y")) * MouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y + MouseInput.x,transform.rotation.eulerAngles.x);

        VerticalRoationStore += MouseInput.y;
        VerticalRoationStore = Mathf.Clamp(VerticalRoationStore, -60f,60);

        ViewPoint.rotation = Quaternion.Euler(-VerticalRoationStore, ViewPoint.rotation.eulerAngles.y,ViewPoint.rotation.eulerAngles.z);

        MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0f, Input.GetAxisRaw("Vertical"));
        if(Input.GetKey(KeyCode.LeftShift)){
            ActiveMoveSpeed = RunSpeed;
        }else{
            ActiveMoveSpeed = MoveSpeed;
        }
        float yVel = Movement.y;
        Movement = ((transform.forward * MoveDirection.z) + (transform.right * MoveDirection.x)).normalized * ActiveMoveSpeed ;
        Movement.y = yVel;
        
        if(CharCont.isGrounded){
            Movement.y = 0f;
        }
        isGrounded = Physics.Raycast(groundChecckPoint.position, Vector3.down, .25f, groundLayer);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            Movement.y = JumpForce;
        }
        Movement.y += Physics.gravity.y * Time.deltaTime * gravityMod;

        CharCont.Move(Movement * Time.deltaTime);

        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        if(Input.GetMouseButton(0))
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0){
                Shoot();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }else if(Cursor.lockState == CursorLockMode.None){
            if(Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
       
    }
    private void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(.5f,.5f,0f));
        ray.origin = cam.transform.position;

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("we hit "+ hit.collider.name);
           GameObject bulletImpactObject =  Instantiate(bulletImapact, hit.point + (hit.normal * .002f), Quaternion.LookRotation(hit.normal, Vector3.up));

           Destroy(bulletImpactObject, 10f);
        }
        shotCounter = timeBetweenShots;
    }
    private void LateUpdate()
    {
        cam.transform.position = ViewPoint.position;
        cam.transform.rotation = ViewPoint.rotation;
    }
}
