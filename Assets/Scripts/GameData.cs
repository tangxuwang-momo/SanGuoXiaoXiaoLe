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

    baXing = 6,
    jiuXing = 7,
    count = 8,

}

public struct XiaoNode{
    private Vector3[] allNode;
    private int nodeType;
    private Vector3 centerPos;

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

    public Vector3 m_centerPos{
        get {return this.centerPos;}
        set {this.centerPos = value;}
    }
}

public struct NodeTypeData{
    public int m_type;
    public int m_typeToCnt;
    public int m_scorePower;

    public NodeTypeData(int type, int typeToCnt, int scorePower) : this(){
        this.m_type = type;
        this.m_typeToCnt = typeToCnt;
        this.m_scorePower = scorePower;
    }
}

public sealed class GameData
{

    private static GameData _instance = null;

    public NodeTypeData[] allNodeTypeData = {                           //每个消除类型的数据
        new NodeTypeData((int)AllNodeType.sanXing,   3, 1 * 3),
        new NodeTypeData((int)AllNodeType.siXing,    4, 2 * 4),
        new NodeTypeData((int)AllNodeType.xieWuXing, 5, 3 * 5),
        new NodeTypeData((int)AllNodeType.wuXing,    5, 5 * 5),
        new NodeTypeData((int)AllNodeType.liuXing,   6, 7 * 6),
        new NodeTypeData((int)AllNodeType.qiXing,    7, 10* 7),
        new NodeTypeData((int)AllNodeType.baXing,    8, 15* 8),
        new NodeTypeData((int)AllNodeType.jiuXing,   9, 20* 9),
    };

    public const string totalScoreStr = "TOTAL_SCORE";

    public const int standardPX = 1;        //标准每像素
    public const int maxPlayer = 8;        //最大角色人物
    public const int heightCnt = 12;        //高度总值
    public const int widthCnt = 8;          //宽度总值
    public const int maxStepCnt = 99;       //最大行动步数

    public GameObject[,] allTouchNode = new GameObject[heightCnt,widthCnt];         //总触摸角色按钮
    public XiaoNode[] allXNode = new XiaoNode[100];          //等待消除的节点列表

    private int gameStep = 0;                //游戏步骤
    public int GameStep{set;get;}  

    public Vector3 choosePos;               //当前选择的节点
    public Vector3 ChoosePos{get; set;}

    private int stepCnt;                     //总步数
    public int StepCnt{get; set;}

    private int score;                       //游戏分数
    public int Score{get; set;}

    public int curXiaoIndex;
    public int CurXiaoIndex{get; set;}

    private int m_totalScore;                       //游戏总分数
    public int TotalScore{
        get{
            if(m_totalScore == 0){
                m_totalScore = PlayerPrefs.GetInt(totalScoreStr);
            }
            return m_totalScore;
        }
        set{
            m_totalScore = value;
            PlayerPrefs.SetInt(totalScoreStr, m_totalScore);
        }
    }

    private int calcXNodeIndex;
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
        Score = 0; 
        StepCnt = 0;
        CurXiaoIndex = 0;
        resetXiaoNode();
    }

    //初始化触摸节点
    public void resetAllTouchNode(){
        for(int y = 0; y < heightCnt; y++){
            for(int x = 0; x < widthCnt; x++){
                if(allTouchNode[y,x]){
                    UnityEngine.Object.Destroy(allTouchNode[y,x]);
                    allTouchNode[y,x] = null;
                }
            }
        }
    }

    public void resetXiaoNode(){
        CalcXNodeIndex = 0;
    }

}