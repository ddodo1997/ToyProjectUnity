using UnityEngine;

public class Door1 : MonoBehaviour
{
    public Player player;


    private void OnEnable()
    {
        gameObject.SetActive(true);
    }
    private void Update()
    {
        if(player.keyCount == 3)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
