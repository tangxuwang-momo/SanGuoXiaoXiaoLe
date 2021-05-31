using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesData : MonoBehaviour
{
    
    GameObject playerNode;
    Sprite[] allPlayer = new Sprite[GameData.maxPlayer + 1];
    Sprite onePlayer;

    public GameObject RootNode;

    void Awake() {
        playerNode = Resources.Load<GameObject>("Prefabs/PlayerNode");
        onePlayer = Resources.Load<Sprite>("Images/player/player_2");
        for(int i = 1; i<= GameData.maxPlayer; i++){
            allPlayer[i] = Resources.Load<Sprite>("Images/player/player_" + i);
        }
    }

    public GameObject createPlayerNode(int imgIndex, Vector3 livePos){
        SpriteRenderer spriteRenderer = playerNode.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = allPlayer[imgIndex];
        GameObject createObject = GameObject.Instantiate(playerNode, livePos, Quaternion.identity, RootNode.transform);
        return createObject;
    }
}