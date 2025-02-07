﻿// -------------------------------------------------------------------------------------------------
// 전체 game manager
// -------------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;//scene관련 함수를 사용하기 위해 필요함


/*
 * 기본 세팅 : 
 * istfloor ->off / 안에 그린 따로 꺼두기
 * 2nd floor -> 0n / 그 중 room A-> on,  room b->off,   2nd hall -> off             
 * 3rd floor -> 0n / 그 아래 하위폴더들은 모두 off
 * exit floor -> off
 * elevator ->off
 * repair component ->on / 그아래 하위폴더 3개는 모두 off
 * 
 * 
 * 변수
 * state1 = false
 * checkControlState = false;
 * startQuest - false;
 * 
 * 
 * typeEffect.canhearsound = true;
 */

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public ElevatorManager elevatorManager;
    //public CameraManager cameraManager;
    public autoMove autoMovement;
    public TypeEffect typeEffect;

    public Animator portraitLeftAnimator;
    public Animator portraitRightAnimator;

    public GameObject gameUIPanel;
    public GameObject gameStartPanel;
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;
    public GameObject talkPanel;
    public GameObject FloorDirectionPanel;
    public GameObject rule1Panel; // 게임 조작법 설명 panel
    public GameObject rule2Panel;
    public GameObject screenLightPanel; // Day가 바뀔때 켜지는 panel
    public GameObject blackoutPanel; // 정전일 때 켜지는 panel
    public GameObject scanObject;//현재 space바 눌러서 만난 object
    public GameObject[] places;
    public GameObject questText;

    /*음악들*/
    public AudioSource mainBGM;
    public AudioSource exitBGM;
    public AudioSource beepSound;
    public AudioSource elevatorBGM;
    public AudioSource damagedBGM;
    public AudioSource deadBGM;

    public Text heartText;
    public Text screenLightDayText;
    public int talkIndex;


    public bool isBlackOut; // 정전이 실행되는 동안 true인 변수

    public bool isAction; // player움직일 수 없음. talk panel 보임. space바 누를 수 있음.
    public bool isPlayerPause; //player움직일 수 없음. talk panel 안보임. space바 누를 수 있음. talkPanel이 보이지 않는 상황에서 player을 멈춰야 할 때. (isAction을 사용하면 isAction = true일떄는 무조건 talkpanel이 보이게 돼서 안됨)
    public bool canPressSpace;  //player가 space바를 누를 수 있는지 없는지를 결정하는 변수.
    public bool isAutoMoving;
    public bool checkControlState;//playermove에서 controlobject함수를 강제로 불러야 하는 상황에서 사용하는 변수임.
    public bool startFirstTalk;// 게임이 시작하며 화면이 밝아질 때는 ScreenBrighten 함수 안에서 플레이어 관련 설정을 건드리지 않기 위해 만든 변수.

    public Image portraitLeftImg;
    public Image portraitRightImg;
    public Image portraitBigImg;

    public Image screenLightImg;
    public Image blackoutImg;

    public Sprite prevPortrait;//이전 portrait를 저장해두는 변수
    public PlayerMove player;
    public EnergyBooster energyBooster;
    public ComponentItem componentItem;
    public ObjectData objData;//현재 조사중인 물건 or 사람


    private void Start()
    {
        Debug.Log(questManager.CheckQuest());
        heartChanged();
        startFirstTalk = true;
        Screen.SetResolution(Screen.width, Screen.width * 16 / 9,  true); // 16:9 로 개발시
        Screen.SetResolution(1980, 1080, true); //480, 270
        //cameraManager.UseFirstCamera();

        /*게임 시작 화면에서 플레이어가 이동하지 못하도록 함*/
        isPlayerPause = true;
        player.transform.position = new Vector3(-1.88f, -2.02f, player.transform.position.z);//조니 옆으로 위치 옮겨두기

        /*BGM 켜기*/
        mainBGM.Play();
    }

    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }


    public void GameStart()
    {
        gameStartPanel.SetActive(false);
        gameUIPanel.SetActive(true);

        //화면 어두웠다가 밝아지기
        screenLightPanel.SetActive(true);
        screenLightImg.color = new Color(screenLightImg.color.r, screenLightImg.color.g, screenLightImg.color.b, 1);
        
        //플레이어가 왼쪽을 보게 함.
        player.dirRayVec = Vector3.left;
        player.anim.SetTrigger("walkSide");
        player.spriteRenderer.flipX = true;

        Invoke("callScreenBrighten", 1);
        //조니와 피트라의 대화가 시작되는 함수 호출
        Invoke("firstTalkstart", 2);

        Invoke("unblockSpace", 2);
    }


    void unblockSpace()
    {
        canPressSpace = true;
    }


    public void floorPanelOff()
    {
        gameUIPanel.SetActive(true);
        FloorDirectionPanel.SetActive(false);
    }


    void callScreenBrighten()
    {
        StartCoroutine("ScreenBrighten");
    }


    void firstTalkstart()
    {
        questManager.ControlObject();//첫대화 시작용
    }


    /*space bar 누를때 실행되는 함수
      함수의 실행을 위해서는 object의 layer을 ispectObject로 바꿔주는 것 잊지 말기. 
    */
    public void Action(GameObject scanObj) 
    {
        scanObject = scanObj;
        objData = scanObj.GetComponent<ObjectData>();

        if(scanObj.tag == "Door")
        {
            DoorData doorScript = scanObject.GetComponent<DoorData>();
            if (questManager.questId == 90 && doorScript.type != DoorData.DoorType.DoorAOut)//case 90에서는 문 안으로 들어가면 안됨. 잠겨있다는 말 띄워주기. room a에서 나갈떄만 문 열어주기.
            {
                /*말풍선이 있다면 띄워주기*/
                talk(objData.id, objData.isNpc);
                talkPanel.SetActive(isAction);
            }
            else
            {
            /*공간 이동하기. (문을 통해 장소 이동)*/
                isPlayerPause = true;
                StartCoroutine( door(scanObj));
            }
            
        }
        else if (scanObj.tag == "Elevator")
        {
            Animator elevatorAnim = scanObj.GetComponent<Animator>();
            elevatorAnim.SetTrigger("elevatorOn");
            elevatorBGM.Play();
            Invoke("elevator",1.5f);
        }
        else if(scanObj.tag == "Floor Direction")
        {
            gameUIPanel.SetActive(false);
            FloorDirectionPanel.SetActive(true);
        }
        else
        {
            /*말풍선이 있다면 띄워주기*/
            talk(objData.id, objData.isNpc);
            talkPanel.SetActive(isAction);
        }
    }


    /*player가 한번 건드리면 없어져야하는 item을 없애준다. ex) 정전 수리 아이템은 주우면 사라져야함*/
    void eraseItem()
    {
        /*tilemap의 line들을 지우기*/
        if ((objData.id == 6000) || (objData.id == 6500) || (objData.id == 7000) || (objData.id == 7500) || (objData.id == 9500))
        {
            objData.gameObject.SetActive(false);
        }
        /*정전 수리 부품들을 지우기*/
        if((objData.id == 4000) && (questManager.questId == 60))
        {
            objData.gameObject.SetActive(false);
        } 
    }


    void talk(int id,bool isNpc)//playerMove에서 isAction이 false면 안움직임. 그래서 계속 이야기 할 수 있는 것임.
    {
        int questTalkIndex = 0;
        string talkData = "";

        if (typeEffect.isAnim)
        {
            typeEffect.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex); //조니 id : 20000, +10 
        }

        if(talkData == null)//얘기가 더이상 없을 때 (대화가 끝났을 때, 물건 조사가 끝났을 때)
        {
            isAction = false;
            talkIndex = 0;
            Debug.Log(questManager.CheckQuest(id));

            eraseItem();
            return;
        }

        int portraitNum = int.Parse(talkData.Split(':')[1]);

        if (isNpc)//npc와 말할 때
        {
            typeEffect.SetMsg(talkData.Split(':')[0]);

            /* 말하는 대상이 Fitra 일때만 오른쪽 portrait 활성화
             * 혼잣말을 할 때는 아무 캐릭터도 보이지 않게 하기 위해서 투명도를 올림.
             */
            if (portraitNum >= 1 && portraitNum <= 4 ) // 피트라가 말할 때라면 
            {
                portraitRightImg.sprite = talkManager.GetPortrait(portraitNum);
                portraitLeftImg.color = new Color(1, 1, 1, 0);
                portraitRightImg.color = new Color(1, 1, 1, 1);
                portraitBigImg.color = new Color(1, 1, 1, 0);

                if (prevPortrait != portraitRightImg.sprite)
                {
                    if (talkIndex > 0)
                        portraitRightAnimator.SetTrigger("doEffect");
                    prevPortrait = portraitRightImg.sprite;
                }
            }

            else if (portraitNum == 0) // portrait가 안보여야 할 떄라면
            {
                portraitLeftImg.color = new Color(1, 1, 1, 0);
                portraitRightImg.color = new Color(1, 1, 1, 0);
                portraitBigImg.color = new Color(1, 1, 1, 0);
            }

            else if (portraitNum == 200)//대화도중 npc에게 booster을 받았다면 (jhonny만 booster을 주기때문에 portrait는 조니로 고정해둠)
            {
                portraitLeftImg.sprite = talkManager.GetPortrait(5); // jhonny의 웃는얼굴임.
                portraitLeftImg.color = new Color(1, 1, 1, 1);
                portraitRightImg.color = new Color(1, 1, 1, 0);
                portraitBigImg.color = new Color(1, 1, 1, 0);
                energyBooster.getBooster();
            }
            else if(portraitNum == 4000)//정전 수리 component를 발견했다면
            {
                portraitRightImg.sprite = talkManager.GetPortrait(1); // jhonny의 웃는얼굴임.
                portraitLeftImg.color = new Color(1, 1, 1, 0);
                portraitRightImg.color = new Color(1, 1, 1, 1);
                portraitBigImg.color = new Color(1, 1, 1, 0);

                componentItem.getComponentItem();
            }
            else if(portraitNum == 23)
            {
                portraitBigImg.sprite = talkManager.GetPortrait(portraitNum); // 폐기 문서
                portraitLeftImg.color = new Color(1, 1, 1, 0);
                portraitRightImg.color = new Color(1, 1, 1, 0);
                portraitBigImg.color = new Color(1, 1, 1, 1);
            }
            else
            {
                portraitLeftImg.sprite = talkManager.GetPortrait(portraitNum);
                portraitLeftImg.color = new Color(1, 1, 1, 1);
                portraitRightImg.color = new Color(1, 1, 1, 0);
                portraitBigImg.color = new Color(1, 1, 1, 0);

                if (prevPortrait != portraitLeftImg.sprite)
                {
                    if (talkIndex > 0)
                        portraitLeftAnimator.SetTrigger("doEffect");
                    prevPortrait = portraitLeftImg.sprite;
                }     
            }
        }
        else // 물건을 조사할 때
        {
            if (portraitNum == 200) // energybooster 을 발견했을 때. (energy booster의 대화 끝에는 200번 써줌)
            {
                if (objData.isChecked)//이미 확인했던 Locker 라면 아이템이 계속 나오면 안됨
                {
                    typeEffect.SetMsg("\n" + "\n" + "..이미 열었던 곳이야");
                }
                else//처음 확인하는 Locker 이라면 energy booster을 얻는다.
                {
                    typeEffect.SetMsg(talkData.Split(':')[0]);
                    energyBooster.getBooster();
                    objData.isChecked = true;
                }
            }
            else
            {
                typeEffect.SetMsg(talkData.Split(':')[0]);
            }

            portraitLeftImg.color = new Color(1, 1, 1, 0);
            portraitRightImg.color = new Color(1, 1, 1, 0);
            portraitBigImg.color = new Color(1, 1, 1, 0);
        }
        
        isAction = true;
        talkIndex++;
        questManager.ControlObject();
    }


    /*문으로 공간 이동하기*/
    IEnumerator door(GameObject scanObj)
    {
        isPlayerPause = true; //player가 다른곳으로 움직일 수 없게 함
        
        DoorData door = scanObj.GetComponent<DoorData>();
        Animator doorAnim = door.GetComponent<Animator>();

        scanObj.layer = 15; // 잠깐 player가 문과 안부딪치게 layer을 바꿔줌
        
        Debug.Log("자동 움직임 시작");
        /*아래서 위로 올라가면서 들어가는 문일 때*/
        if((door.type == DoorData.DoorType.DoorAOut) || (door.type == DoorData.DoorType.DoorBIn)|| 
            (door.type == DoorData.DoorType.DoorCIn)|| (door.type == DoorData.DoorType.DoorDIn)|| (door.type == DoorData.DoorType.DoorRepairIn))
        {
            player.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 1f, player.transform.position.z);
            Vector3 targetPos = new Vector3(player.transform.position.x, door.transform.position.y +0.1f, player.transform.position.z);
            autoMovement.startAutoMove(player.gameObject, targetPos, 1f);
        }
        else/*위에서 아래로 내려가며 들어가는 문일때*/
        {
            Vector3 targetPos = new Vector3(player.transform.position.x , door.transform.position.y+0.3f, player.transform.position.z);
            autoMovement.startAutoMove(player.gameObject, targetPos,1f);
        }
        

        /*문열리는 에니메이션 시작하고 에니메이션 진행동안(2초)은 공간이동 하지 않고 기다림*/
        doorAnim.SetTrigger("DoorOpen");
        yield return new WaitForSeconds(2);
        autoMovement.isAutoMoving = false;

        switch (door.type)
        {
            case DoorData.DoorType.DoorAIn:
                places[2].SetActive(true);
                places[1].SetActive(false);
                player.transform.position = new Vector3(1.5f, 8.4f, 0);
                break;

            case DoorData.DoorType.DoorAOut:
                places[2].SetActive(false);
                places[1].SetActive(true);
                player.transform.position = new Vector3(1.5f, 11, 0);
                break;

            case DoorData.DoorType.DoorBIn:
                places[1].SetActive(false);
                places[3].SetActive(true);
                player.transform.position = new Vector3(19.5f, 11.1f, 0);
                break;

            case DoorData.DoorType.DoorBOut:
                places[3].SetActive(false);
                places[1].SetActive(true);
                player.transform.position = new Vector3(18.5f, 6.2f, 0);
                break;

            case DoorData.DoorType.DoorCIn:
                places[4].SetActive(false);
                places[5].SetActive(true);
                player.transform.position = new Vector3(31.5f, 21.1f, 0);
                break;

            case DoorData.DoorType.DoorCOut:
                places[5].SetActive(false);
                places[4].SetActive(true);
                player.transform.position = new Vector3(31.5f, 15, 0);
                break;

            case DoorData.DoorType.DoorDIn:
                places[4].SetActive(false);
                places[6].SetActive(true);
                player.transform.position = new Vector3(18.5f, 21.1f, 0);
                break;

            case DoorData.DoorType.DoorDOut:
                places[6].SetActive(false);
                places[4].SetActive(true);
                player.transform.position = new Vector3(18.5f, 15, 0);
                break;

            case DoorData.DoorType.DoorRepairIn:   
                places[4].SetActive(false);
                places[7].SetActive(true);
                player.transform.position = new Vector3(15, 5.6f, 0);
                break;

            case DoorData.DoorType.DoorRepairOut:
                places[7].SetActive(false);
                places[4].SetActive(true);
                player.transform.position = new Vector3(28.8f, 5.1f, 0);
                break;
        }
        
        ObjectData playerObjData = player.GetComponent<ObjectData>();
        door.gameObject.layer = 10;
        isPlayerPause = false;
    }


    /*3층에서 택배 보관실 통로로 이동시키기*/
    public IEnumerator teleportToExit()
    {
        isPlayerPause = true;
        places[4].SetActive(false);
        places[8].SetActive(true);
        questManager.green3thfloor.SetActive(false);

        /*플레이어 이동*/
        player.transform.position = new Vector3(12.5f,15.8f, player.transform.position.z);
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y-1.8f, player.transform.position.z);
        autoMovement.startAutoMove(player.gameObject, targetPos, 1f);
        yield return new WaitForSeconds(2);

        /*게임 설명 창 띄움*/
        rule2Panel.SetActive(true);

        /*exit 브금으로 바꾸기*/
        mainBGM.Stop();
        exitBGM.Play();
        
    }


    /*택배 보관실에서 건물 밖 거리로 이동시키기*/
    public void teleportToOutside()
    {
        Debug.Log("플레이어 멈춰");
        isPlayerPause = true;
        places[8].SetActive(false);
        places[9].SetActive(true);

        /**/
        mainBGM.Play();

        /*출입문 앞으로 이동 & 피트라가 앞을 보도록 돌려놓기*/
        player.transform.position = new Vector3(40.4f, 14.27f, player.transform.position.z);
        player.anim.SetTrigger("seeFront");

        questManager.state1 = true;
        /*다음 동작이 이뤄지도록 questmanager.ControlObject() 를 부름 */
        questManager.ControlObject();
    }



    /*화면 어둡게 및 밝게*/
    public void ScreenLightDarken(string dayText)
    {
        isPlayerPause = true;
        canPressSpace = false;
        screenLightPanel.SetActive(true);
        StartCoroutine(ScreenDarken(dayText));  
    }


    IEnumerator ScreenDarken(string dayText)
    {
        bool isDarkning = true;
        float a = screenLightImg.color.a;
        while (isDarkning)
        {
            screenLightDayText.text = dayText;
            a += Time.deltaTime * 0.7f;
            screenLightImg.color = new Color(screenLightImg.color.r, screenLightImg.color.g, screenLightImg.color.b, a);
            screenLightDayText.color = new Color(screenLightDayText.color.r, screenLightDayText.color.g, screenLightDayText.color.b, a);
            yield return new WaitForSeconds(0.005f);
            if (a >= 1)
            {
                isDarkning = false;

                if (dayText == "Thanks for Playing!")
                {
                    gameClearPanel.SetActive(true);
                }
                else
                {
                    if (dayText == "Day 3")
                    {
                        /*문앞으로 순간이동*/
                        places[0].SetActive(true);
                        places[7].SetActive(false);
                        player.transform.position = new Vector3(21.15f, 15.08f, 0);

                        /*가장 어두워졌을 때 피트라가 앞을 보도록 돌려놓기*/
                        player.anim.SetTrigger("seeFront");
                        player.dirRayVec = Vector3.down;

                        questManager.green.SetActive(false);
                        ScreenLightBrighten();
                    }
                    else if (dayText != "")
                    {
                        /*가장 어두워졌을 때 피트라가 앞을 보도록 돌려놓기*/
                        player.anim.SetTrigger("seeFront");
                        player.dirRayVec = Vector3.down;
                        ScreenLightBrighten();
                    }
                    else // daytext 가 "" 일때.
                    {
                        ScreenLightBrighten();
                    } 
                }
            }
        }
    }


    public void ScreenLightBrighten()
    {
        StartCoroutine("ScreenBrighten");
    }


    IEnumerator ScreenBrighten()
    {
        bool isBrightning = true;
        float a = screenLightImg.color.a;

        while (isBrightning)
        {
            a -= Time.deltaTime * 0.7f;
            screenLightImg.color = new Color(screenLightImg.color.r, screenLightImg.color.g, screenLightImg.color.b, a);
            screenLightDayText.color = new Color(screenLightDayText.color.r, screenLightDayText.color.g, screenLightDayText.color.b, a);
            yield return new WaitForSeconds(0.005f);

            if (a <= 0)
            {
                isBrightning = false;
                if (screenLightDayText.text == "")//정전수리 event라면
                {
                    canPressSpace = true;
                }
                if (!startFirstTalk)//게임 시작화면이 아니라면
                {
                    isPlayerPause = false;
                    canPressSpace = true;
                }
                else
                {//게임 시작하며 조니와 말할때라면
                    startFirstTalk = false;
                }

                screenLightPanel.SetActive(false);
            }
        }
    }


    /*정전*/
    public void blackout()
    {
        blackoutPanel.SetActive(true);
        StartCoroutine(doBlackout());
    }

    IEnumerator doBlackout()
    {
        bool switching = true;
        float a = blackoutImg.color.a;
        while (isBlackOut)
        {
            /*정전의 깜박거리는 효과 만들기*/
            if (switching)
            {
                switching = false;
                 a-= 0.35f;
            }
            else
            {
                switching = true;
                a += 0.35f;
            }
                
            blackoutImg.color = new Color(blackoutImg.color.r, blackoutImg.color.g, blackoutImg.color.b, a);
            yield return new WaitForSeconds(3);
        
        }
    }


    public void stopBlackout()
    {
        blackoutPanel.SetActive(false);
    }


    /*플레이어 하트 감소*/
    public void heartChanged()
    {
        heartText.text = "x" + (player.heart).ToString();
        //string.Format("{0:n0}", player.score);
    }


    public void gameOver()
    {
        Debug.Log("게임 오버");
        gameOverPanel.SetActive(true);

    }


    public void Restart()
    {
        SceneManager.LoadScene(0);
    }


    /*game manager에서 상위 object를 꺼놓으면 여기서 아무리 하위 object를 켜도 안켜지는 점 주의하기
     *  예를들어 여기서 2nd floor을 꺼버리면 그안의 Room A를 켜도 안보임. 
     */
    void elevator()
    {
        /*엘리베이터가 아닌 다른 공간은 모두 끄기*/
        places[0].SetActive(false);
        places[1].SetActive(false);
        places[2].SetActive(false);
        places[3].SetActive(false);
        places[4].SetActive(false);

        //추격하던 PoliceAI가 있다면 끄기.
        questManager.PoliceAisetActive(false);

        /*엘리베이터 내부 이미지 및 UI켜기*/
        elevatorManager.elevatorOn();
    }


    /*게임 룰 1 의 창을 여는 함수*/
    public void rule1Button()
    {
        /*창을 끄고 시작 화면 창 띄움*/
        rule1Panel.SetActive(true);
        gameStartPanel.SetActive(false);
    }


    /*게임 룰 1 의 창을 닫는 함수*/
    public void closeButton1()
    {
        Debug.Log("창닫음");
        /*창을 끄고 시작 화면 창 띄움*/
        rule1Panel.SetActive(false);
        gameStartPanel.SetActive(true);
    }


    /*게임 룰 2 의 창을 닫는 함수*/
    public void closeButton()
    {
        Debug.Log("창닫음");
        /*창을 끔*/
        rule2Panel.SetActive(false);

        /*창을 닫으면 플레이어가 다시 움직일 수 있게 함*/
        isPlayerPause = false;

        /*피트라가 quest100에서 혼잣말 할 수 있게 변수 true로 바꿔줌*/
        questManager.state1 = true;

        Debug.Log("말해");
        questManager.state1 = false;
        player.isFitraMonologing = true;// 피트라가 혼자말하게 함. 무조건 scanobj가 fitra로 바뀜.
        Action(player.gameObject); // 도망쳐!
    }
}
