using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour
{
    public Button startGame;
    public Text bingMaText;

    private void Awake() {
        startGame = GameObject.Find("StartBtn").GetComponent<Button>();
        startGame.onClick.AddListener(()=>{
            SceneManager.LoadScene("GameScene");
        });

        bingMaText = GameObject.Find("BingMa").GetComponent<Text>();
        bingMaText.text = GameData.Instance.TotalScore.ToString();
    }

}
