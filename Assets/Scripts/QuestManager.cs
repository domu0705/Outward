﻿// -------------------------------------------------------------------------------------------------
// quest 관리
// -------------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 시작 설정
 * talkTriggerLine, animTriggerLine,  JhonnyLine, aiChasingLine 네개 다 시작 전에 setactive(false)싱태로 해두기 
 */

// -------------------------------------------------------------------------------------------------
public class QuestManager : MonoBehaviour
{
    public int questId; // 지금 진행중인 quest id
    public int questActionIndex; // 현재 quest 중에서 이야기의 순서에 맞게 진행되게 하기 위한 변수
    public int count;

    public GameObject[] questObject; // 정전 수리 부품.
    public GameManager manager;
    public PlayerMove player;
    public ElevatorManager elevatorManager;
    public autoMove autoMoving;
    public TypeEffect typeEffect;

    public GameObject playerObject;
    public GameObject jhonny;
    public GameObject marin;
    public GameObject green; // 1층 로비에 있는 green
    public GameObject green2; //2층에 있는 green 
    public GameObject green3thfloor;
    public GameObject benny;
    public GameObject[] policeAIAry;
    public GameObject[] ExitpoliceAIAry;//exit에서 나오는 police ai들의 그룹
    public GameObject fitraDesk;

    public GameObject talkTriggerLine; // 필요할때 setactive true로 해두고 끝나면 바로 끔.
    public GameObject animTriggerLine;
    public GameObject JhonnyLine;
    public GameObject aiChasingLine;

    public GameObject secretExitTilemap;//3층의 비밀통로
    public GameObject exitTalkTriggerLine; // 필요할때 setactive true로 해두고 끝나면 바로 끔.
    public GameObject exitBlocker; // 3층의 비밀길이 열리기 전 막고있던 collider

    public Vector3 MarinPos;


    /*피트라의 자동이동을 위한 변수*/
    Vector3 targetPos;


    /*캐릭터 자동 이동에 사용되는 변수*/
    bool isMarinMovingToFitra;
    bool isMarinReturning;
    bool isFitraMovingToHumanDoorY;
    bool isFitraMovingToHumanDoorX;

    /*quest control 용 변수들.   (if (questActionIndex == 1) 이면, questActionIndex가 1일때 controlobject함수가 불리면 몇번이고 계속 if문안으로 들어감. 그래서 if문에 한번만 들어가게 하기 위한 변수임. if의 조건문에 이 변수를 넣고 한번 if안에 들어가면 변수를 false로 바꿔줌  )*/
    public bool startQuest;//control object에서 quest가 시작하자마자 바뀌는 상황들을 나타내주기 위한 변수
    public bool state1;
    bool state2;
    Dictionary <int, QuestData> questList;


    // -------------------------------------------------------------------------------------------------
    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
        setQuestText();
    }


    private void Update()
    {
        if (isMarinMovingToFitra)//마린을 피트라 옆으로 옮기기
            moveMarinToFitra();

        if (isMarinReturning)//마린을 원위치로 되돌려놓기
            returnMarin();

        /*피트라를 1층의 Human Door 앞으로 이동시키기*/
        if (isFitraMovingToHumanDoorX)
            StartCoroutine("moveFitraToHumanDoorX");// ();
        if (isFitraMovingToHumanDoorY)
            moveFitraToHumanDoorY();
    }


    void GenerateData()
    {
        //quest가 끝나면 index가 ++된 뒤 다음 퀘스트로 넘어감. 즉 퀘스트 10번은 questactionindex가 2로 바뀌었다가(이때 talkindex는 0) 퀘스트 20번으로 넘어감.
        questList.Add(10, new QuestData("신디씨에게 서류를 받자", new int[] { 20000, 30000 }));
        questList.Add(20, new QuestData("서류를 전달하자.", new int[] { 40000,20000 }));
        questList.Add(30, new QuestData("내 자리로 가자.", new int[] { 3000 }));
        questList.Add(40, new QuestData("퇴근하자.", new int[] { 8000,40000, 9000 })); //800, 4000은 TALK 함수 강제 호출하면 될듯
        questList.Add(50, new QuestData("내 자리로 가자.", new int[] { 3000}));
        questList.Add(60, new QuestData("부품 3개를 모아 수리 센터로 가자", new int[] { 10000, 10000,4000,4000,4000,60000, 60000 }));
        questList.Add(70, new QuestData("신디씨가 할 말이 있다고 했어.", new int[] { 6000, 30000  }));
        questList.Add(80, new QuestData("건물을 나가자.", new int[] { 9000,8000, 7500 }));
        questList.Add(90, new QuestData("도움 받을만한 사람을 찾아보자", new int[] { 6500,40000 }));
        questList.Add(100, new QuestData("건물을 탈출하자.", new int[] { 10000, 9500 }));
        questList.Add(110, new QuestData("첫 발걸음.", new int[] { 10000 }));
        questList.Add(120, new QuestData("Game Clear.", new int[] { 0 }));
    }


    public int GetQuestTalkIndex(int id) //quest번호를 반환
    {
        return questId + questActionIndex;
    }


    public string CheckQuest(int id)
    {
        if (id == questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;
        }
        ControlObject(); 

        if (questActionIndex == questList[questId].npcId.Length)
        {
            /*다음 quest로 넘어가기*/
            ControlObject();
            NextQuest();
        }
        return questList[questId].questName;
    }


    public string CheckQuest()
    {
        return questList[questId].questName;
    }


    public void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
        setQuestText();
        ControlObject(); //추가해봄. 없애도 됨
    }


    public void setQuestText()
    {
        Text questTxt = manager.questText.GetComponent<Text>();
        questTxt.text = CheckQuest();
    }


    /*Quest와 관련된 object들을 관리하는 함수
     * 한 talkData 안에 여러 스트링이 있잖아. 
     * 그 중 몇번쨰 string에 특정 일이 일어날때를 관리하기 위해서 talkIndex를 받아옴.
     */
    public void ControlObject() 
    {
        switch (questId)
        {
            case 10://Sindy에게 서류를 받자

                /*조니가 자동으로 피트라에게 말을 건다*/
                if (questActionIndex == 0 && manager.talkIndex == 0  && !state1)
                {
                    state1 = true;
                    Invoke("jhonnyTalkToFitra", 1);
                }
                if(questActionIndex == 1)
                {
                    manager.isPlayerPause = false;
                }
                if (questActionIndex == 2)
                {
                    questObject[1].SetActive(true);//문서 켜기
                }
                break;

            case 20://서류를 그린씨에게 전달하자
                if (questActionIndex == 1)
                {
                    questObject[1].SetActive(false);//문서 끄기
                }
                break;

            case 30://피트라의 자리로 가자.
                /*마린을 피트라 옆으로 이동시키기*/
                if (manager.talkIndex == 4)
                {
                    /*bool값을 바꿔줌으로써 moveMarinToFitra 함수를 호출*/
                    isMarinMovingToFitra = true;
                    manager.objData.isNpc = true;//피트라의 첫 대화상대가 npc가 아닌 자기 책상이기 때문에 portrait가 안나옴. portrait의 등장을 위해 isnpc를 true로 바꿔줌.
                }

                /*마린을 원래 자기자리로 이동시키기*/
                if (manager.talkIndex == 13)
                {
                    manager.objData.isNpc = false;//피트라의 첫 대화상대가 npc가 아닌 자기 책상이기때문에 (portrait의 등장을 위해 isnpc를 true로 바꿨던 것을) 다시 false로 돌려놓아줌

                    /*bool값을 바꿔줌으로써 returnMarin 함수를 호출*/
                    isMarinReturning = true;
                    manager.checkControlState = true;
                }
                break;

            case 40://1층으로 나가 퇴근하자
                animTriggerLine.SetActive(true);

                /*2층의 그린을 없애기*/
                green2.SetActive(false);
                green.SetActive(true);

                /*quest를 위해 처음 문 근처로 왔을때 자동으로 HUMAN Door로 player을 옮겨줌 */
                if (player.scanObject && player.scanObject.gameObject.tag == "Anim Trigger Line")
                {
                    manager.checkControlState = false;//playerMove의 fixedupdate에서 계속 controlObject를 부르지 않도록 다시 변수를 false로 바꿈
                    ObjectData scanObj = player.scanObject.GetComponent<ObjectData>();
                    if (scanObj.isChecked)
                        break;
                    else
                    {
                        //자동으로 움직일때는 UI끔
                        manager.gameUIPanel.SetActive(false);

                        Debug.Log("case 40의 자동이동 시작");
                        scanObj.isChecked = true;//이후로 이 길을 지나갈때는 이 에니메이션이 진행되지 않음.
                        manager.isAction = true;//player의 키 입력에 의해 피트라의 자동 움직임이 방해되지 않도록 피트라의 움직임 입력받기를 정지

                        /*피트라가 왼쪽으로 걸어가는 에니메이션으로 바꿔줌*/
                        manager.isAutoMoving = true;
                        player.anim.SetBool("isChange", true);
                        player.anim.SetInteger("hAxisRaw", -1);

                        /*목표 위치 설정*/
                        targetPos = new Vector3(26.1f, player.transform.position.y, player.transform.position.z);

                        /*bool값을 바꿔줌으로써 moveFitraToHumanDoor 함수를 호출*/
                        isFitraMovingToHumanDoorX = true;
                    }
                }

                /*문과 말할때 피트라의 portrait를 보이게 하기 위해 잠시 문의 isNpc를 true로 바꿈*/
                if(manager.objData && questActionIndex == 0 && manager.talkIndex == 1)
                    manager.objData.isNpc = true;
                if (manager.objData && questActionIndex == 0 && manager.talkIndex == 2)
                    manager.objData.isNpc = false;

                /*Human Door 앞에서 그린씨가 피트라에게 말걸기*/
                if (questActionIndex == 1)//문과의 대화가 끝났다면 questActionIndex가 1로 바뀜.
                {
                    manager.isAction = true;//player의 키 입력에 의해 그린이 말걸기 전에 다른 곳으로 플레이어가 이동하지 않도록 정지시킴.
                    
                    ObjectData greenScript = green.GetComponent<ObjectData>();
                    if (!greenScript.isArrived)
                    {
                        manager.canPressSpace = false;
                        autoMoving.startAutoMove(green, new Vector3(25.4f, green.transform.position.y, green.transform.position.z),2.5f);
                        manager.checkControlState = true;//그린이 피트라 옆에 가면 playermove script에서 controlState함수를 불러주기 위함.
                    }
                    if (greenScript.isArrived && manager.checkControlState) // 그린이 피트라 옆에 도착한다면
                    {
                        //자동으로 움직이는것이 끝나면 UI켬                        
                        manager.gameUIPanel.SetActive(true);

                        /*피트라가 그린에게 말걸도록 왼쪽을 보게 함*/
                        player.dirRayVec = Vector3.left;

                        manager.checkControlState = false;
                        autoMoving.arrivedToDest = false;
                        manager.Action(green);
                        manager.canPressSpace = true;
                    }
                }

                /*AI door 로 나감(퇴근)*/
                if (questActionIndex == 3 && manager.talkIndex == 0)
                {
                    manager.ScreenLightDarken("Day 2");
                    manager.checkControlState = true;
                }
                break;

            case 50: /*출근. 피트라의 자리로 가기*/
                talkTriggerLine.SetActive(true);

                /*1층 로비의 그린을 없애고 2층 그린을 보여주기*/
                green.SetActive(false);
                green2.SetActive(true);

                /*1층의 Talk Trigger Line을 지나면 피트라의 자리로 가자는 talkbubble 띄우기*/
                if (player.scanObject && player.scanObject.gameObject.tag == "Talk Trigger Line")
                {
                    manager.checkControlState = false;//playerMove의 fixedupdate에서 계속 controlObject를 부르지 않도록 다시 변수를 false로 바꿈
                    ObjectData scanObj = player.scanObject.GetComponent<ObjectData>();
                    if (scanObj.isChecked)
                        break;
                    else
                    {
                        manager.Action(talkTriggerLine);
                        scanObj.isChecked = true;
                    }
                }

                /*정전 구현*/
                if (questActionIndex == 1 && !manager.isBlackOut) // 정전이 한번 실행되면 이 if문 안으로 다시 안들어가도록 하기 위해 지금 벌써 정전중인지도 확인.
                {
                    manager.isBlackOut = true;
                    manager.blackout();
                    manager.isPlayerPause = true; // player가 못움직이게 함
                    manager.canPressSpace = false; // 정전 시 fitra가 다음 대사를 미리 보지 않도록 space바도 못누르게 함.
                    StartCoroutine(wait(3)); // 여기서 startQuest 를 true로 바꿔줌 
                }
                break;

            case 60: /*정전을 고칠 부품들을 모아 수리센터로 가자*/

                /*피트라 책상 앞에서 혼잣말 시작. "앗, 출근 하자마자 정전..?" */
                if (startQuest && questActionIndex == 0)
                {
                    /*앗, 출근하자마자.. 대사 시작*/
                    startQuest = false; // case 50이 끝나고 3초뒤 true가 됐던 변수를 다시 false로 바꿔줌
                    StartCoroutine("inCase60_1");
                }

                /*피트라에게 정전 수리 도우라는 문자 보내기*/
                if(questActionIndex == 1 && state1)
                {
                    state1 = false;
                    startQuest = true;

                    /*피트라에게 정전 수리 도우라는 문자 보내기 시작*/
                    manager.checkControlState = true;
                    StartCoroutine("inCase60_2");
                }

                if(questActionIndex == 1 && manager.talkIndex == 1 && startQuest && !state1)
                {
                    manager.checkControlState = false;
                    startQuest = false;
                    manager.beepSound.Play();
                }

                /*피트라가 혼자말하게 하는 것 취소하기.*/
                if (questActionIndex == 2)
                {
                    player.isFitraMonologing = false;// 피트라가 혼자말하게 하는 것 취소하기.
                    state1 = true;
                }

                /*수리하며 화면 어두워졌다가 수리 끝났다는 대화 하기*/
                if (questActionIndex == 6 && state1)
                {
                    state1 = false;
                    manager.canPressSpace = false;
                    manager.isPlayerPause = true;

                    manager.componentItem.useComponent();
                    
                    manager.ScreenLightDarken("");
                    manager.blackoutPanel.SetActive(false);
                    /*베니가 수리 완료 대화 하기*/
                    Invoke("waitForRepair1",6);
                }

                /*Day 2끝 화면 어두워지기*/
                if (questActionIndex == 7 && manager.talkIndex == 0)
                {
                    manager.ScreenLightDarken("Day 3");

                    /*Day 3가 시작하면 quest 70에서 신디씨에게 문자왔다는 이야기를 해야하므로 다시 talkTriggerline을 감지하기위해 ischecked를 false로 해줌*/
                    talkTriggerLine.SetActive(true);
                    ObjectData talkTriggerLineScript = talkTriggerLine.GetComponent<ObjectData>();
                    talkTriggerLineScript.isChecked = false;

                    /*quest 70에서 talk trigger line을 지나는 것을 확인하기 위함*/
                    manager.checkControlState = true ;
                    startQuest = true;
                }
                break;

            case 70:
                /*소리 끄기*/
                manager.mainBGM.Stop();
                typeEffect.canHearSound = false;

                /*1층의 Talk Trigger Line을 지나면 "신디씨한테 문자 와있던데... 먼저 들러보자"라는 talkbubble 띄우기*/
                if (player.scanObject && player.scanObject.gameObject.tag == "Talk Trigger Line")
                {
                    manager.checkControlState = false;//playerMove의 fixedupdate에서 계속 controlObject를 부르지 않도록 다시 변수를 false로 바꿈
                    ObjectData scanObj = player.scanObject.GetComponent<ObjectData>();
                    if (scanObj.isChecked)
                        break;
                    else
                    {
                        /*70 말 시작*/
                        manager.Action(talkTriggerLine);
                        scanObj.isChecked = true;
                        questObject[7].SetActive(false); // 조니자리의 컴퓨터 켜두기
                        jhonny.SetActive(false); // 조니자리의 컴퓨터 켜두기
                        green2.SetActive(false);
                    }
                }
                break;

            case 80:
                if(questActionIndex == 1)
                {
                    manager.checkControlState = true;
                    JhonnyLine.SetActive(true);
                }
                /*조니상사의 자리에 왔을 때 문서 발견*/
                if(questActionIndex == 2)
                {
                    if (player.scanObject && player.scanObject.gameObject.tag == "Jhonny Line" )
                    {                   
                        ObjectData scanObj = player.scanObject.GetComponent<ObjectData>();
                        if (scanObj.isChecked)
                            break;
                        else
                        {
                            manager.checkControlState = false;//playerMove의 fixedupdate에서 계속 controlObject를 부르지 않도록 다시 변수를 false로 바꿈

                            /*조니 문서 말 시작*/
                            scanObj.isChecked = true;
                            manager.Action(JhonnyLine);
                        }
                    }
                }

                if (questActionIndex == 3 && manager.talkIndex == 0)
                {
                    manager.checkControlState = true;
                    aiChasingLine.SetActive(true);
                }
                    break;

            case 90:
                /*소리 다시 들린다는 말. Police AI가 폐기시작이라는 말 함*/
                if (questActionIndex == 0)
                {
                    if (player.scanObject && player.scanObject.gameObject.tag == "AI Chasing Line")
                    {
                        manager.checkControlState = false;//playerMove의 fixedupdate에서 계속 controlObject를 부르지 않도록 다시 변수를 false로 바꿈
                        ObjectData scanObj = player.scanObject.GetComponent<ObjectData>();

                        if (scanObj.isChecked)
                            break;
                        else
                        {
                            /*소리 켬*/
                            manager.mainBGM.Play();
                            typeEffect.canHearSound = true;

                            PoliceAisetActive(true);

                            /*AI Chasing 말 시작*/
                            scanObj.isChecked = true;
                            manager.Action(scanObj.gameObject);
                            
                        }
                    }
                }

                /*Police AI가 피트라를 따라오기 시작함*/
                if (questActionIndex == 1)
                {
                    foreach (GameObject policeAI in policeAIAry)
                    {
                        PoliceAiStartChasing(policeAI);
                    }
                }

                /*그린과 말이 끝나면 비밀통로가 열림*/
                if (questActionIndex == 1 && manager.talkIndex == 14)
                {
                    secretExitTilemap.SetActive(true);
                    exitBlocker.SetActive(false);
                }

                /*player script에서 collider에 닿으면 exit floor로 순간이동하면서 rule설명 창이 열림 (gamemanager 의 teleportToExit함수가 호출됨)*/
                break;
            case 100:

                /*피트라가 경찰 발결하고 "뛰어"라고 말함*/
                if(questActionIndex == 0 && state1)
                {
                    //Gamemanager의 closeButton1 함수에서 아래 주석친 일들을 대신 함
                    /*
                    Debug.Log("말해");
                    state1 = false;
                    player.isFitraMonologing = true;// 피트라가 혼자말하게 함. 무조건 scanobj가 fitra로 바뀜.
                    manager.Action(playerObject); // 도망쳐!
                    */
                }

                /*police ai들이 피트라를 쫒아가기 시작함*/
                if (questActionIndex == 1 && player.isFitraMonologing)
                {
                    player.isFitraMonologing = false;
                    foreach (GameObject policeAI in ExitpoliceAIAry)
                    {
                        PoliceAiStartChasing(policeAI);
                    }
                    state1 = true;
                    manager.checkControlState = true; // 아래 if문에서 사용
                }

                /*피트라가 "..출구다" 라고 말함.  Police AI 를 멈추기*/
                if (questActionIndex == 1 && state1)
                {
                    if (player.scanObject && player.scanObject.gameObject.tag == "Exit Talk Trigger Line")
                    {
                        manager.checkControlState = false;//playerMove의 fixedupdate에서 계속 controlObject를 부르지 않도록 다시 변수를 false로 바꿈
                        ObjectData scanObj = player.scanObject.GetComponent<ObjectData>();
                        if (scanObj.isChecked)
                            break;
                        else
                        {
                            state1 = false;
                            /*Police AI 를 멈추기*/
                            foreach (GameObject policeAI in ExitpoliceAIAry)
                            {
                                PoliceAiStopChasing(policeAI);
                            }
                            manager.Action(exitTalkTriggerLine);
                            scanObj.isChecked = true;
                            manager.checkControlState = true;

                            /*exit브금 끄기*/
                            manager.exitBGM.Stop();
                        }
                    }
                }
                break;

            case 110:
                /*playermove script에서 gameManager.teleportToOutside 함수가 먼저 실행됨*/

                /*피트라가 건물을 나옴. 마지막 멘트 하기.*/
                if (questActionIndex == 0 && state1)
                {
                    manager.gameUIPanel.SetActive(false);

                    /*피트라의 마지막 멘트*/
                    state1 = false;
                    player.isFitraMonologing = true;// 피트라가 혼자말하게 함. 무조건 scanobj가 fitra로 바뀜.
                    manager.Action(playerObject);
                }

                /*피트라 옆으로 걷기 시작*/
                if(questActionIndex == 1 && player.isFitraMonologing)
                {
                    player.isFitraMonologing = false;
                    manager.canPressSpace = false;

                    /*플레이어 이동*/
                    player.transform.position = new Vector3(40.4f, 14.27f, player.transform.position.z);
                    Vector3 targetPos = new Vector3(player.transform.position.x + 30f, player.transform.position.y, player.transform.position.z);
                    autoMoving.startAutoMove(player.gameObject, targetPos, 0.8f);

                    /*플레이어 옆으로 걷는 anim으로 바꿔줌*/
                    player.anim.SetInteger("hAxisRaw", 1); //anim이 idle_sideplayer로 넘어가지 않게 0이 아닌 값으로 바꿔줌.
                    player.anim.SetTrigger("walkSide");
                    player.spriteRenderer.flipX = false;

                    /*화면 어둡게 및 엔딩 씬*/
                    StartCoroutine( startEnding());
                }
                break;
        }
    } 


    /*조니가 처음으로 피트라를 꺠운다*/
    void jhonnyTalkToFitra()
    {
        if (player.scanObject)
        {//player.scanObject가 없다는 에러 방지용 if문.
            manager.Action(player.scanObject);
        }
    }


    /*player을 'time' 초동안 멈추게 함*/
    IEnumerator wait(float time)
    {
        manager.isPlayerPause = true;
        yield return new WaitForSeconds(time);
        manager.isPlayerPause = false;

        startQuest = true;
        ControlObject();
    }


    void waitForRepair1()
    {
        /* 베니의 수리끝났다는 대화 시작*/
        manager.Action(benny); // 베니가 "피트라씨?"라고 부르는 창이 뜬 뒤 1초뒤에 space 를 누를 수 있게 함
        Invoke("waitForRepair2",4);
    }


    void waitForRepair2()
    {
        /*플레이어 움직이게 함*/
        manager.canPressSpace = true;
        manager.isPlayerPause = false;

        startQuest = true;
    }


    IEnumerator inCase60_1()
    {
        yield return new WaitForSeconds(2f);
       
        player.isFitraMonologing = true;// 피트라가 혼자말하게 함. 무조건 scanobj가 fitra로 바뀜.
        manager.Action(playerObject); // 앗, 출근 하자마자 정전..?
        manager.canPressSpace = true; //대사 넘겨야 하기떄문에 space바누를수 있게 함.
    }


    IEnumerator inCase60_2(){
        yield return new WaitForSeconds(2f);
        questObject[6].SetActive(false);//3층 C동의 길막고 있는 상자 치우기
        manager.Action(playerObject); // 수리 문자 받기. 
    }


    /*police AI를 보여주거나 끄는 함수*/
    public void PoliceAisetActive(bool isShowing)
    {
        foreach (GameObject policeAI in policeAIAry)
        {
            policeAI.SetActive(isShowing);
        }
    }


    /*police AI가 추격을 시작하게 하는 함수*/
    void PoliceAiStartChasing(GameObject policeAI)
    {
        PoliceAI policeAIScript = policeAI.GetComponent<PoliceAI>();
        policeAIScript.startChasing = true;
    }


    /*police AI가 추격을 멈추게 하는 함수*/
    void PoliceAiStopChasing(GameObject policeAI)
    {
        PoliceAI policeAIScript = policeAI.GetComponent<PoliceAI>();
        policeAIScript.startChasing = false;
    }


    /*마린을 피트라 옆으로 이동시키는 함수*/
    void moveMarinToFitra()
    {
        Vector3 targetPos = new Vector3(player.transform.position.x + 0.5f, player.transform.position.y, player.transform.position.z);
        marin.transform.position = Vector3.MoveTowards(marin.transform.position, targetPos, 1.5f * Time.deltaTime);

        if (marin.transform.position == targetPos)
            isMarinMovingToFitra = false;
    }


    /*마린을 원위치로 되돌려놓는 함수*/
    void returnMarin()
    {
        marin.transform.position = Vector3.MoveTowards(marin.transform.position, MarinPos, 1.5f * Time.deltaTime);

        if (marin.transform.position == MarinPos)
            isMarinReturning = false;
    }


    /*피트라가 문으로 가는 x축 이동 함수*/
    IEnumerator moveFitraToHumanDoorX()
    {
        /*anim이 계속 새로 시작하는 것 방지용*/
        player.anim.SetBool("isChange", false);
        player.anim.SetInteger("hAxisRaw", -1);
        player.transform.position = Vector3.MoveTowards(player.transform.position, targetPos, 2.5f * Time.deltaTime);

        if (player.transform.position == targetPos)
        {
            /*moveFitraToHumanDoorX 함수의 실행을 멈추기*/
            isFitraMovingToHumanDoorX = false;
            
            /*X축 이동이 끝났으니 이제 Y축으로 이동시키기*/
            targetPos = new Vector3(player.transform.position.x, 14.7f, player.transform.position.z);

            player.mustAnimBack = true;// 강제로 playerMovescript에서  anim을 조절하는 변수들의 값을 뱐화시키는 것을 막는 변수임.

            /*뒤로 걷는 anim을 위한 변수 설정*/
            player.anim.SetInteger("vAxisRaw", 1);
            player.anim.SetBool("isHorizonMove", false);
            player.anim.SetBool("isChange", true);

            yield return new WaitForSeconds(0.2f); // ischange가 너무 빨리 false로 바뀌어서 anim이 안바뀌는 현상을 방지.

            /*moveFitraToHumanDoorY 함수 시작*/
            isFitraMovingToHumanDoorY = true;
        }
    }


    /*피트라가 문으로 가는 y축 이동 함수*/
    void moveFitraToHumanDoorY()
    {
        /*anim 이 무한루프로 시작하지 않게 유지용*/
        player.anim.SetBool("isChange", false);

        player.transform.position = Vector3.MoveTowards(player.transform.position, targetPos, 2.5f * Time.deltaTime);

        if (player.transform.position == targetPos)
        {
            /*자동으로 뒤를 보게 했기떄문에 player의 direction ray도 자동으로 맞춰놓기*/
            player.dirRayVec = Vector3.up;

            /*moveFitraToHumanDoorY함수의 실행을 멈추기*/
            isFitraMovingToHumanDoorY = false;



            /*바로 문에 말걸도록 함*/
            Invoke("autoTalkToHumanDoor", 0.5f);

            /*피트라의 움직임을 다시 player의 키 입력으로 조절함*/
            player.mustAnimBack = false;
            manager.isAutoMoving = false;
        }
    }


    void autoTalkToHumanDoor()
    {
        manager.Action(player.scanObject);
    }
    

    /*2초뒤 화면 어두워지게 시작*/
    IEnumerator startEnding()
    {
        yield return new WaitForSeconds(1);
        manager.ScreenLightDarken("Thanks for Playing!");
    }
}
