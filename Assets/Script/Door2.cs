using UnityEngine;

public class Door2 : MonoBehaviour
{
    public GameManager manager;


    private void OnEnable()
    {
        gameObject.SetActive(true);
    }
    private void Update()
    {
        if(manager.isGameClearAble)
        {
            gameObject.SetActive(false);
        }
    }
}
