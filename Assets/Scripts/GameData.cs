using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AllGameStep{
    initGame = 0,          //初始化游戏
    chooseQiDian,        //选择空白格子
    gameing,            //游戏中
    endGame,            //游戏结束
}

enum AllNodeType{
    /*
        * * *
    */
    sanXing = 0,        //三星     
    /*
        * * * *
    */
    siXing = 1,         //四星       
    /*
          *
          *
        * * *
    */  
    xieWuXing = 2,      //斜五星
    /*
        * * * * *
    */
    wuXing = 3,         //五星
    /*
          *
          *
        * * * *
    */
    liuXing = 4,        //六星
    /*
            *
            *
        * * * * *
    */
    qiXing = 5,         //七星

}

public struct XiaoNode{
    private Vector3[] allNode;
    private int nodeType;

    public XiaoNode(Vector3[] m_allNode, int m_nodeType) : this(){
        this.allNode = m_allNode;
        this.nodeType = m_nodeType;
    }

    public Vector3[] m_allNode{
        get {return this.allNode;}
        set {this.allNode = value;}
    }

    public int m_nodeType{
        get {return this.nodeType;}
        set {this.nodeType = value;}
    }
}

public sealed class GameData
{

    private static GameData _instance = null;

    public int[] nodeTypeToCnt = new int[6]{3, 4, 5, 5, 6, 7};

    public const int standardPX = 1;        //标准每像素
    public const int maxPlayer = 6;        //最大角色人物
    public const int heightCnt = 12;        //高度总值
    public const int widthCnt = 8;          //宽度总值

    public GameObject[,] allTouchNode = new GameObject[heightCnt,widthCnt];         //总触摸角色按钮

    public int gameStep = 0;                //游戏步骤
    public int GameStep{set;get;}  

    public Vector3 choosePos;               //当前选择的节点
    public Vector3 ChoosePos{get; set;}

    public XiaoNode[] allXNode = new XiaoNode[100];          //等待消除的节点列表
    public int calcXNodeIndex;
    public int CalcXNodeIndex{get; set;}

    private GameData(){}                    //单例
    public static GameData Instance{
        get{
            if(_instance == null){
                _instance = new GameData();
            }
            return _instance;
        }
    }

    //初始化游戏数据
    public void initGame(){                 
        GameStep = (int)AllGameStep.initGame;
        resetAllTouchNode();
    }

    //初始化触摸节点
    public void resetAllTouchNode(){
        for(int y = 0; y < heightCnt; y++){
            for(int x = 0; x < widthCnt; x++){
                if(allTouchNode[y,x]){
                    allTouchNode[y,x] = null;
                    //Destroy(allTouchNode[y,x]);
                }
            }
        }
    }

    public void resetXiaoNode(){
        calcXNodeIndex = 0;
    }

}