using UnityEngine;

public class Items : MonoBehaviour
{
    float rotateSpeed = 150f;

    private void Update()
    {
        transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0), Space.World);
    }
}
