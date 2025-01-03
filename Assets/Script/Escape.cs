using UnityEngine;

public class Escape : MonoBehaviour
{
    public Player player;
    public GameManager gameManager;
    public void GameClear()
    {
        if(player.isGetReward)
        {
            gameManager.isGameClear = true;
        }
    }
}
