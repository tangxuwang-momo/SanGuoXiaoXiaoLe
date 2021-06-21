using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public sealed class MessageBox{
    private static MessageBox instance = null;
    private static GameObject showBox;
    private static GameObject showBox2;
    private GameObject newBox;
    private GameObject newBox2;

    public Action OkAction{get; set;}

    private Action noAction;
    public Action NoAction{
        get{
            noAction = ()=>{
                if(newBox != null){
                    UnityEngine.Object.Destroy(newBox);
                    newBox = null;
                }
                if(newBox2 != null){
                    UnityEngine.Object.Destroy(newBox2);
                    newBox2 = null;
                }
            };
            return noAction;
        }
        set{

        }}

    private MessageBox(){}

    public static MessageBox Instance{
        get{
            if(instance == null){
                showBox = Resources.Load<GameObject>("Prefabs/tip");
                showBox2 = Resources.Load<GameObject>("Prefabs/tip2");
                instance = new MessageBox();
            }
            return instance;
        }
    }

    public GameObject show(string showText, Transform parentTranform, Action okAction = null){
        if(newBox == null){
            newBox = GameObject.Instantiate(showBox, new Vector3(0, 0, 0), Quaternion.identity, parentTranform);
        }
        newBox.transform.localPosition = Vector3.zero;

        Text showStr = newBox.transform.Find("Text").GetComponent<Text>();
        showStr.text = showText;

        if(okAction != null){
            OkAction = okAction;
        }
        else{
            OkAction = NoAction;
        }
        Button okBtn = newBox.transform.Find("OkButton").GetComponent<Button>();
        okBtn.onClick.AddListener(()=>{
            OkAction?.Invoke();
            NoAction();
        });
        return newBox;
    }

    public GameObject show2(string showText, Transform parentTranform, Action okAction = null){
        if(newBox2 == null){
            newBox2 = GameObject.Instantiate(showBox2, new Vector3(0, 0, 0), Quaternion.identity, parentTranform);
        }
        newBox2.transform.localPosition = Vector3.zero;

        Text showStr = newBox2.transform.Find("Text").GetComponent<Text>();
        showStr.text = showText;

        if(okAction != null){
            OkAction = okAction;
        }
        else{
            OkAction = NoAction;
        }
        Button okBtn = newBox2.transform.Find("OkButton").GetComponent<Button>();
        okBtn.onClick.AddListener(()=>{
            OkAction?.Invoke();
            NoAction();
        });

        Button noBtn = newBox2.transform.Find("NoButton").GetComponent<Button>();
        noBtn.onClick.AddListener(()=>{
            NoAction();
        });
        return newBox2;
    }

}