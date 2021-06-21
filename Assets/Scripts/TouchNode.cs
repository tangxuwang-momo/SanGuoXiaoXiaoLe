using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchNode : MonoBehaviour
{
    public GameObject lightImg;

    private int index;
    public int Index{get; set;}

    public Vector3 pos;
    public Vector3 Pos{get; set;}
    
    public void setLight(bool isLight){
        lightImg.gameObject.SetActive(isLight);
        if(isLight){
            GameData.Instance.ChoosePos = Pos;
            Vector3 newPos = this.transform.position;
            newPos.z = -1;
            this.transform.position = newPos;
        }
        else {
            Vector3 newPos = this.transform.position;
            newPos.z = 0;
            this.transform.position = newPos;
        }

    }

}