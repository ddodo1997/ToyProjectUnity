using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public int keyCount = 0;
    public bool isGetReward = false;
    public bool isDead = false;
    public GameManager manager;

    [Header("Rotate")]
    public float mouseSpeed;
    float yRotation;
    float xRotation;
    Camera cam;

    [Header("Move")]
    public float moveSpeed;
    float h;
    float v;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb.freezeRotation = true;
        isDead = false;
        isGetReward = false;
        keyCount = 0;

        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (isDead || manager.isGameClear)
            return;
        Rotate();
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Key"))
        {
            keyCount++;
            other.gameObject.SetActive(false);
        }
        if(other.CompareTag("Reward"))
        {
            other.gameObject.SetActive(false);
            manager.OnClearAble();
        }
        if(other.CompareTag("Escape") && manager.isGameClearAble)
        {
            other.GetComponent<Escape>().GameClear();
            manager.OnClear();
        }
    }
    private void Move()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        Vector3 moveVec = transform.forward * v + transform.right * h;
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? 10f : 5f;

        transform.position += moveVec.normalized * moveSpeed * Time.deltaTime;
        cam.transform.position = transform.Find("Eye").position;
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        transform.Find("Eye").Find("Sight").rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void OnDie()
    {
        isDead = true;
        manager.isGameOver = true;
    }
}