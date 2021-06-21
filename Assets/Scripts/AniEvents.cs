using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniEvents : MonoBehaviour
{
    public GameScene gameScene;

    void stopAni(){
        gameScene.callXEnd();
    }
    
}
