using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    public bool isDead = false;

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

        cam = Camera.main;
    }

    private void Update()
    {
        if (isDead)
            return;
        Rotate();
    }
    private void FixedUpdate()
    {
        if (isDead)
            return;
        Move();
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
}