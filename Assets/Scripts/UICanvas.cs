using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UICanvas : MonoBehaviour
{
    public Text score;
    public Text step;

    void Start()
    {
        
    }

    private void Awake() {
        updateStepText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateText(){
        score.text = "" + GameData.Instance.Score;
    }

    public void updateStepText(){
        step.text = "剩余步数:" + (GameData.maxStepCnt - GameData.Instance.StepCnt);
    }

    public void sureReturn(){
        MessageBox.Instance.show2("确认要提前结束招兵吗？\n本次招兵人数为\n" + (GameData.Instance.Score).ToString(), transform, ()=>{
            GameData.Instance.TotalScore += GameData.Instance.Score;
                    SceneManager.LoadScene("LobbyScene");
                });
    }
}
