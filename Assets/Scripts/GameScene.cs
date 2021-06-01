using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GameScene : MonoBehaviour
{
    public ResourcesData resourcesData;
    bool isTouch = false;
    Vector3 touPos;
    bool canMove = false;           //选中格子是否在移动
    GameObject chooseNode;
    Vector3 newPos = new Vector3(0, 0, 0);
    int speed = 10;

    int xiaoAni = 0;
    float moveTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameData.Instance.GameStep == (int)AllGameStep.initGame){
            createAllNode();
            GameData.Instance.allTouchNode[GameData.heightCnt / 2, GameData.widthCnt / 2].gameObject.GetComponent<TouchNode>().setLight(true);
            GameData.Instance.GameStep = (int)AllGameStep.chooseQiDian;
            return;
        }

        if(EventSystem.current.currentSelectedGameObject != null){
            return;
        }
        else{
            //Debug.Log("点击的是屏幕（非Button属性的）");
        }

        if(xiaoAni > 0){
            removeAllXNode();
            return;
        }

        if(canMove){
            chooseNode.transform.position = Vector3.MoveTowards(chooseNode.transform.position, newPos, speed * Time.deltaTime);

            if(chooseNode.transform.position == newPos){
                moveTime = 1;
                xiaoAni++;
                calcAllXNode(); //计算所有可消除点
                removeAllXNode();
                canMove = false;
            }
            return;
        }

        if(isTouchStart()){
            isTouch = true;

            Vector3 touPosIndex = touPos;
            touPosIndex.x += GameData.widthCnt / 2;
            touPosIndex.y += GameData.heightCnt / 2;
            if(GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x)] != null){
                GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x)].gameObject.GetComponent<TouchNode>().setLight(false);
            }
            GameData.Instance.allTouchNode[(int)(touPosIndex.y), (int)(touPosIndex.x)].gameObject.GetComponent<TouchNode>().setLight(true);
        }
        if(isTouchMove() && isTouch){
            Vector3 touPosIndex = touPos;
            touPosIndex.x += GameData.widthCnt / 2;
            touPosIndex.y += GameData.heightCnt / 2;
            if(GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x)] != null){
                GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x)].gameObject.GetComponent<TouchNode>().setLight(false);
            }
            GameData.Instance.allTouchNode[(int)(touPosIndex.y), (int)(touPosIndex.x)].gameObject.GetComponent<TouchNode>().setLight(true);
        }
        else if(isTouchEnd() && isTouch){
            //选择奇点
            if(GameData.Instance.GameStep == (int)AllGameStep.chooseQiDian){
                Destroy(GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x)]);
                GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x)] = null;
                GameData.Instance.GameStep = (int)AllGameStep.gameing;
            }
            //移动格子
            else if(GameData.Instance.GameStep == (int)AllGameStep.gameing){
                Vector3 newIndexPos = new Vector3(0, 0, 0);
                chooseNode = GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x)];
                Vector3 oldPos = chooseNode.transform.position;
                //上
                if(GameData.Instance.ChoosePos.y + 1 < GameData.heightCnt && GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y + 1), (int)(GameData.Instance.ChoosePos.x)] == null){
                    newPos = new Vector3(oldPos.x, oldPos.y + 1 * GameData.standardPX, -1);
                    newIndexPos = new Vector3(GameData.Instance.ChoosePos.x, GameData.Instance.ChoosePos.y + 1, 0);
                    canMove =  true;
                    GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y + 1), (int)(GameData.Instance.ChoosePos.x)] = chooseNode;
                }
                //下
                else if(GameData.Instance.ChoosePos.y - 1 >= 0 && GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y - 1), (int)(GameData.Instance.ChoosePos.x)] == null){
                    newPos = new Vector3(oldPos.x, oldPos.y - 1 * GameData.standardPX, -1);
                    newIndexPos = new Vector3(GameData.Instance.ChoosePos.x, GameData.Instance.ChoosePos.y - 1, 0);
                    canMove =  true;
                    GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y - 1), (int)(GameData.Instance.ChoosePos.x)] = chooseNode;
                }
                //左
                else if(GameData.Instance.ChoosePos.x - 1 >= 0 && GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x - 1)] == null){
                    newPos = new Vector3(oldPos.x - 1 * GameData.standardPX, oldPos.y, -1);
                    newIndexPos = new Vector3(GameData.Instance.ChoosePos.x - 1, GameData.Instance.ChoosePos.y, 0);
                    canMove =  true;
                    GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x - 1)] = chooseNode;
                }
                //右
                else if(GameData.Instance.ChoosePos.x + 1 < GameData.widthCnt && GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x + 1)] == null){
                    newPos = new Vector3(oldPos.x + 1 * GameData.standardPX, oldPos.y, -1);
                    newIndexPos = new Vector3(GameData.Instance.ChoosePos.x + 1, GameData.Instance.ChoosePos.y, 0);
                    canMove =  true;
                    GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x + 1)] = chooseNode;
                }
                if(canMove){
                    GameData.Instance.allTouchNode[(int)(GameData.Instance.ChoosePos.y), (int)(GameData.Instance.ChoosePos.x)] = null;
                    chooseNode.gameObject.GetComponent<TouchNode>().Pos = newIndexPos;
                    GameData.Instance.ChoosePos = newIndexPos;
                }
            }
        }
        else if(isTouchCancle()){
            isTouch = false;
        }
    }

    private bool isTouchStart(){
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            var mScreenPos = Input.GetTouch(0).position;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mScreenPos);
            touPos = worldPos;
            return true;
        }
        if(Input.GetMouseButtonDown(0)){
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            var mScreenPos = Input.mousePosition;
            mScreenPos.z = screenPos.z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mScreenPos);
            touPos = worldPos;
            return true;
        }
        return false;
    }

    private bool isTouchMove(){
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved){
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            var mScreenPos = Input.GetTouch(0).position;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mScreenPos);
            touPos = worldPos;
            return true;
        }
        if (Input.GetMouseButton(0)){
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            var mScreenPos = Input.mousePosition;
            mScreenPos.z = screenPos.z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mScreenPos);
            touPos = worldPos;
            return true;

        }

        return false;
    }

    private bool isTouchEnd(){
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended){
            return true;
        }
        if (Input.GetMouseButtonUp(0)){
            return true;
        }

        return false;
    }

    private bool isTouchCancle(){
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Canceled){
            return true;
        }
        if (Input.GetMouseButtonUp(0)){
            return true;

        }

        return false;
    }

    void createAllNode(){
        for(int y = 0; y < GameData.heightCnt; y++){
            for(int x = 0; x < GameData.widthCnt; x++){
                Vector3 livePos = new Vector3((float)(x - GameData.widthCnt / 2 + 0.5), (float)(y - GameData.heightCnt / 2 + 0.5), 0);
                int random = UnityEngine.Random.Range(1, GameData.maxPlayer);
                GameObject newNode = resourcesData.createPlayerNode(random, livePos);
                GameData.Instance.allTouchNode[y,x] = newNode;
                TouchNode touchNode = GameData.Instance.allTouchNode[y,x].gameObject.GetComponent<TouchNode>();
                touchNode.Pos = new Vector3(x, y, 0);
                touchNode.Index = random;
            }
        }
    }

    void calcAllXNode(){
        GameData.Instance.resetXiaoNode();
        int[,] allNode = new int[GameData.heightCnt, GameData.widthCnt];
        int xIndex = 0;
        int calcNode = -1;
        int calcCnt = 0;
        TouchNode touchNode = null;// = new TouchNode();

        for(int y = 0; y < GameData.heightCnt; y++){
            calcNode = -1;
            calcCnt = 0;
            for(int x = 0; x < GameData.widthCnt; x++){
                bool calcNow = false;
                bool isNullNode = false;
                if(GameData.Instance.allTouchNode[y,x] == null){
                    calcNow = true;
                    isNullNode = true;
                }
                else{
                    touchNode = GameData.Instance.allTouchNode[y,x].gameObject.GetComponent<TouchNode>();
                    if(calcNode == -1){
                        calcNode = touchNode.Index;
                        calcCnt ++;
                    }
                    else{
                        if(calcNode == touchNode.Index){
                            calcCnt ++;
                        }
                        if(calcNode != touchNode.Index || x == GameData.widthCnt - 1){
                            calcNow = true;
                            if(x == GameData.widthCnt - 1){
                                x++;
                            }
                        }
                    }
                }
                if(calcNow){
                    if(calcCnt >= 3){
                        xIndex ++;
                        Vector3[] allXPos = new Vector3[10];
                        int posIndex = 0;
                        for(int i = calcCnt; i > 0; i--){
                            allNode[y,x - i] = xIndex;
                            allXPos[posIndex++] = new Vector3(x - i, y, 0);
                        }
                        int calcNodeType = (int)AllNodeType.sanXing;
                        if (calcCnt == 4){
                            calcNodeType = (int)AllNodeType.siXing;
                        }
                        else if(calcCnt == 5){
                            calcNodeType = (int)AllNodeType.wuXing;
                        }
                        GameData.Instance.allXNode[xIndex] = new XiaoNode(allXPos, calcNodeType);
                        Vector3 startPos = GameData.Instance.allTouchNode[(int)(allXPos[0].y), (int)(allXPos[0].x)].transform.position;
                        Vector3 endPos = GameData.Instance.allTouchNode[(int)(allXPos[posIndex-1].y), (int)(allXPos[posIndex-1].x)].transform.position;
                        GameData.Instance.allXNode[xIndex].m_centerPos = new Vector3((startPos.x + endPos.x) / 2, (startPos.y + endPos.y) / 2, 0);
                    }
                    if(isNullNode){
                        calcNode = -1;
                        calcCnt = 0;
                    }
                    else{
                        if(touchNode){
                            calcNode = touchNode.Index;
                        }
                        calcCnt = 1;
                    }
                }
            }
        }

        for(int x = 0; x < GameData.widthCnt; x++){
            calcNode = -1;
            calcCnt = 0;
            for(int y = 0; y < GameData.heightCnt; y++){
                bool calcNow = false;
                bool isNullNode = false;
                if(GameData.Instance.allTouchNode[y,x] == null){
                    calcNow = true;
                    isNullNode = true;
                }
                else{
                    touchNode = GameData.Instance.allTouchNode[y,x].gameObject.GetComponent<TouchNode>();
                    if(calcNode == -1){
                        calcNode = touchNode.Index;
                        calcCnt ++;
                    }
                    else{
                        if(calcNode == touchNode.Index){
                            calcCnt ++;
                        }
                        if(calcNode != touchNode.Index || y == GameData.heightCnt - 1){
                            calcNow = true;
                            if(y == GameData.heightCnt - 1){
                                y++;
                            }
                        }
                    }
                }
                if(calcNow){
                    if(calcCnt >= 3){
                        int calcXIndex = 0;
                        for(int i = calcCnt; i > 0; i--){
                            if(allNode[y - i,x] > 0){
                                calcXIndex = allNode[y - i,x];
                                break;
                            }
                        }

                        Vector3[] allXPos = new Vector3[10];
                        int calcNodeType = (int)AllNodeType.sanXing;
                        int posIndex = 0;
                        Vector3 centerPos = new Vector3();
                        if (calcXIndex == 0){
                            calcXIndex = ++xIndex;

                            for(int i = calcCnt; i > 0; i--){
                                allNode[y - i,x] = calcXIndex;
                                allXPos[posIndex++] = new Vector3(x, y - i, 0);
                            }

                            if (calcCnt == 4){
                                calcNodeType = (int)AllNodeType.siXing;
                            }
                            else if(calcCnt == 5){
                                calcNodeType = (int)AllNodeType.wuXing;
                            }

                            Vector3 startPos = GameData.Instance.allTouchNode[(int)(allXPos[0].y), (int)(allXPos[0].x)].transform.position;
                            Vector3 endPos = GameData.Instance.allTouchNode[(int)(allXPos[posIndex-1].y), (int)(allXPos[posIndex-1].x)].transform.position;
                            centerPos = new Vector3((startPos.x + endPos.x) / 2, (startPos.y + endPos.y) / 2, 0);
                        }
                        else{
                            for(int i = 0; i < GameData.Instance.nodeTypeToCnt[GameData.Instance.allXNode[xIndex].m_nodeType]; i++){
                                allXPos[posIndex++] = GameData.Instance.allXNode[xIndex].m_allNode[i];
                            }
                            for(int i = calcCnt; i > 0; i--){
                                if(allNode[y - i,x] == 0){
                                    allNode[y - i,x] = calcXIndex;
                                    allXPos[posIndex++] = new Vector3(x, y - i, 0);
                                }
                                else{
                                    centerPos = GameData.Instance.allTouchNode[y - i, x].transform.position;
                                }
                            }

                            if(calcCnt + GameData.Instance.nodeTypeToCnt[GameData.Instance.allXNode[xIndex].m_nodeType] - 1 == 5){
                                calcNodeType = (int)AllNodeType.xieWuXing;
                            }
                            else if(calcCnt + GameData.Instance.nodeTypeToCnt[GameData.Instance.allXNode[xIndex].m_nodeType] - 1 == 6){
                                calcNodeType = (int)AllNodeType.liuXing;
                            }
                            else if(calcCnt + GameData.Instance.nodeTypeToCnt[GameData.Instance.allXNode[xIndex].m_nodeType] - 1 == 7){
                                calcNodeType = (int)AllNodeType.qiXing;
                            }
                        }
                            
                        GameData.Instance.allXNode[xIndex] = new XiaoNode(allXPos, calcNodeType);
                        GameData.Instance.allXNode[xIndex].m_centerPos = centerPos;
                    }
                    if(isNullNode){
                        calcNode = -1;
                        calcCnt = 0;
                    }
                    else{
                        calcNode = touchNode.Index;
                        calcCnt = 1;
                    }
                }
            }
        }
        GameData.Instance.CalcXNodeIndex = xIndex;
    }

    void removeAllXNode(){
        bool isEnd = true;
        for(int i = 1; i <= GameData.Instance.CalcXNodeIndex; i++){
            for(int j = 0; j < GameData.Instance.nodeTypeToCnt[GameData.Instance.allXNode[i].m_nodeType]; j++){
                Vector3 tochNodePos = GameData.Instance.allXNode[i].m_allNode[j];
                GameObject touchNode = GameData.Instance.allTouchNode[(int)tochNodePos.y, (int)tochNodePos.x];
                if(touchNode == null){
                    continue;
                }
                else{
                    isEnd = false;
                }
                touchNode.transform.position = Vector3.MoveTowards(touchNode.transform.position, GameData.Instance.allXNode[i].m_centerPos, speed * Time.deltaTime); 
                if(touchNode.transform.position == GameData.Instance.allXNode[i].m_centerPos){
                    Destroy(touchNode);
                    GameData.Instance.allTouchNode[(int)tochNodePos.y, (int)tochNodePos.x] = null;
                } 
            }
        }
        if(isEnd){
            xiaoAni --;
        }
    }
}
