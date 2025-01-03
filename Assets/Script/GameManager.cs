using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameOver = false;
    public bool isGameClearAble = false;
    public bool isGameClear = false;

    public Player player;
    public Escape escape;

    public TextMeshProUGUI currentKeys;
    public TextMeshProUGUI centerMsg;
    public float fadeSpeed = 0.5f;
    private void OnEnable()
    {
        isGameOver = false;
        isGameClearAble = false;
        isGameClear = false;
        centerMsg.text = "Collect Three Keys";
    }


    private void Update()
    {
        currentKeys.text = $"Keys : {player.keyCount}";

        if (!isGameOver && !isGameClear)
        {
            if(centerMsg.alpha > 0f)
                centerMsg.alpha -= Time.deltaTime * fadeSpeed;
        }

        if (isGameOver || isGameClear)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void OnClear()
    {
        isGameClear = true;
        centerMsg.alpha = 1f;
        centerMsg.text = "Clear!";
    }

    public void OnClearAble()
    {
        isGameClearAble = true;
        centerMsg.alpha = 1f;
        centerMsg.text = "Get Out Now!!!";
    }

    public void OnGameOver()
    {
        centerMsg.alpha = 1f;
        centerMsg.text = $"Press R To ReStart";
    }
}
