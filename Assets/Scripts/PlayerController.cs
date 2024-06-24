using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviourPunCallbacks
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
    public float JumpForce = 12f, gravityMod = 2.5f;

    public Transform groundChecckPoint;
    private bool isGrounded;
    public LayerMask groundLayer;
    public GameObject bulletImapact;
    //public float timeBetweenShots = .1f;
    private float shotCounter;
    public float muzzleDisplaytime;
    private float muzzelCounter;
    public float MaxheatValue = 10f, /*heatPerShot = 1f,*/ CoolRate = 4f, OverHeatCoolRate = 5f;
    private float heatCounter;
    private bool OverHeated;
    [SerializeField]
    private CharacterController CharCont;
    private Camera cam;

    public Gun[] allGuns;
    private int seletedGun;
    [Space]

    public GameObject playerHitImapct;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
        UIManager.instance.weaponTempSlider.maxValue = MaxheatValue;
        SwitchGun();

        //Transform newTrans = SpawnManager.instance.GetSpawnPoint();
        //transform.position = newTrans.position;
        //transform.position = newTrans.position;
    }


    void Update()
    {
        if (photonView.IsMine)
        {
            MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * MouseSensitivity;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + MouseInput.x, transform.rotation.eulerAngles.x);

            VerticalRoationStore += MouseInput.y;
            VerticalRoationStore = Mathf.Clamp(VerticalRoationStore, -60f, 60);

            ViewPoint.rotation = Quaternion.Euler(-VerticalRoationStore, ViewPoint.rotation.eulerAngles.y, ViewPoint.rotation.eulerAngles.z);

            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            if (Input.GetKey(KeyCode.LeftShift))
            {
                ActiveMoveSpeed = RunSpeed;
            }
            else
            {
                ActiveMoveSpeed = MoveSpeed;
            }
            float yVel = Movement.y;
            Movement = ((transform.forward * MoveDirection.z) + (transform.right * MoveDirection.x)).normalized * ActiveMoveSpeed;
            Movement.y = yVel;

            if (CharCont.isGrounded)
            {
                Movement.y = 0f;
            }
            isGrounded = Physics.Raycast(groundChecckPoint.position, Vector3.down, .25f, groundLayer);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Movement.y = JumpForce;
            }
            Movement.y += Physics.gravity.y * Time.deltaTime * gravityMod;

            CharCont.Move(Movement * Time.deltaTime);


            if (allGuns[seletedGun].muzzelflash.activeInHierarchy)
            {
                muzzelCounter -= Time.deltaTime;

                if (muzzelCounter <= 0)
                {
                    allGuns[seletedGun].muzzelflash.SetActive(false);
                }

            }



            if (!OverHeated)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    Shoot();
                }
                if (Input.GetMouseButton(0) && allGuns[seletedGun].isAutomatic)
                {
                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        Shoot();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else if (Cursor.lockState == CursorLockMode.None)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                }
                heatCounter -= CoolRate * Time.deltaTime;
            }
            else
            {
                heatCounter -= OverHeatCoolRate * Time.deltaTime;
                if (heatCounter <= 0)
                {

                    OverHeated = false;
                    UIManager.instance.overheatedtext.gameObject.SetActive(false);
                }
            }
            if (heatCounter < 0)
            {
                heatCounter = 0f;
            }
            UIManager.instance.weaponTempSlider.value = heatCounter;

            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                seletedGun++;
                if (seletedGun >= allGuns.Length)
                {
                    seletedGun = 0;
                }
                SwitchGun();
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                seletedGun--;
                if (seletedGun < 0)
                {
                    seletedGun = allGuns.Length - 1;
                }
                SwitchGun();
            }
        }
    }
    private void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("hit " + hit.collider.gameObject.GetPhotonView().Owner.NickName);
                PhotonNetwork.Instantiate(playerHitImapct.name, hit.point, Quaternion.identity);

                hit.collider.gameObject.GetPhotonView().RPC("DealDamage", RpcTarget.All, photonView.Owner.NickName);

            }
            else
            {
                GameObject bulletImpactObject = Instantiate(bulletImapact, hit.point + (hit.normal * .002f), Quaternion.LookRotation(hit.normal, Vector3.up));

                Destroy(bulletImpactObject, 10f);
            }
        }
        shotCounter = allGuns[seletedGun].timeBetweenShots;

        heatCounter += allGuns[seletedGun].heatPerShot;
        if (heatCounter >= MaxheatValue)
        {
            heatCounter = MaxheatValue;

            OverHeated = true;
            UIManager.instance.overheatedtext.gameObject.SetActive(true);
        }
        allGuns[seletedGun].muzzelflash.SetActive(true);
        muzzelCounter = muzzleDisplaytime;
    }
    [PunRPC]
    public void DealDamage(string damager)
    {
        TakeDamage(damager);
    }
    public void TakeDamage(string damager)
    {
        if (photonView.IsMine)
        {
            //Debug.Log(photonView.Owner.NickName + "I've been hit by " + damager);
            PlayerSpawner.instance.Die();
           
        }

    }
    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            cam.transform.position = ViewPoint.position;
            cam.transform.rotation = ViewPoint.rotation;
        }
    }
    void SwitchGun()
    {
        foreach (Gun gun in allGuns)
        {
            gun.gameObject.SetActive(false);
        }

        allGuns[seletedGun].gameObject.SetActive(true);
        allGuns[seletedGun].muzzelflash.SetActive(false);
    }
}
