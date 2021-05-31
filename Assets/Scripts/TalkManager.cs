﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;
    public Sprite[] portraitArr;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateTalkData();
        GeneratePortraitData();
    }



    /*
        1000 : johnny's big locker
        2000 : 에너지 부스터가 들어있는 locker // 이건 portrait numdmf 200으로 함
        3000 : fitra desk
        4000 : 전기 수리 부품들
        5000 : Door E // 3층의 인공지능 관리부서 문
        6000 : quest50 출근하자에 쓰이는 talk trigger Line
        6500 : quest 90 AI가 피트라를 쫓는 trigger Line
        7000 : quest 40 에 쓰이는 Anim Trigger Line(1st floor)
        7500 : quest 70 에 쓰이는 Anim Trigger Line(2st floor 조니 상사의 자리)
        8000 : Human Door (1st floor)
        9000 : AI Door (1st floor)
        9999: 아무것도 없는 물건.
        
    
        portrait data 생성
         * Fitra : 10000
         * Johnny : 20000
         * Sindy : 30000
         * Green : 40000
         * marin : 50000
         * benny : 60000 -수리공
         * empty box : 70000
         * repairshop AI bad : 80000
         * repairshop AI good : 90000
         * 닫힌 문 : 100000
         * 
         
    */
    void GeneratePortraitData()
    {
        /* portrait data 생성 */
        portraitData.Add(0, portraitArr[0]);
        portraitData.Add(1, portraitArr[1]);
        portraitData.Add(2, portraitArr[2]);
        portraitData.Add(3, portraitArr[3]);
        portraitData.Add(4, portraitArr[4]);
        portraitData.Add(5, portraitArr[5]);
        portraitData.Add(6, portraitArr[6]);
        portraitData.Add(7, portraitArr[7]);
        portraitData.Add(8, portraitArr[8]);
        portraitData.Add(9, portraitArr[9]);
        portraitData.Add(10, portraitArr[10]);
        portraitData.Add(11, portraitArr[11]);
        portraitData.Add(12, portraitArr[12]);
        portraitData.Add(13, portraitArr[13]);
        portraitData.Add(14, portraitArr[14]);
        portraitData.Add(15, portraitArr[15]);
        portraitData.Add(16, portraitArr[16]);
        portraitData.Add(17, portraitArr[17]);
        portraitData.Add(18, portraitArr[18]);
        portraitData.Add(19, portraitArr[19]);
        portraitData.Add(20, portraitArr[20]);
        portraitData.Add(21, portraitArr[21]);
        portraitData.Add(22, portraitArr[22]);
        portraitData.Add(23, portraitArr[23]);
        portraitData.Add(24, portraitArr[24]);
        portraitData.Add(25, portraitArr[25]);
    }

    /*layer 무조건 nispect object로 설정해두기*/
    void GenerateTalkData()
    {
        

        /*locker talk data 생성*/
        talkData.Add(1000, new string[] { "\n" + "\n" + "...잠겨있다.:9999" });
        talkData.Add(2000, new string[] { "\n" + "\n" + "에너지 부스터가 들어있다... 챙겨보자." + "\n" + "(에너지 부스터를 얻었다.):200" });
        talkData.Add(3000, new string[] { "\n" + "\n" + "내 자리. 좀 더럽다...:0" });
        talkData.Add(4000, new string[] { "\n" + "\n" + "(흠..짐이 많이 들어있네.):0" });
        talkData.Add(5000, new string[] { "\n" + "\n" + "-E동 - 출입 제한 구역 :0",
                                         "[피트라]" + "\n" + "\n" + "잠겨있네.:3",
                                       });
        talkData.Add(6500, new string[] { "\n" + "\n" + "6500선이다.:0" });
        talkData.Add(7000, new string[] { "\n" + "\n" + "선이다.:0" });
        talkData.Add(8000, new string[] { "[문]" + "\n" + "\n" + "사용 권한이 없습니다.:0" });
        talkData.Add(9000, new string[] { "[문]" + "\n" + "\n" + "사용가능한 시간이 아닙니다.:0" });
        

        /*디폴트 talk data 생성*/
        talkData.Add(20000, new string[] { "[조니]" + "\n" + "\n" + "...네?:6",
                                          "[조니]" + "\n" + "\n" +"제가 좀 바빠서.. 미안해요~:7" });
        talkData.Add(30000, new string[] { "[신디]" + "\n" + "\n" +"아! 안녕하세요.:8",
                                          "[신디]" + "\n" + "\n" +"뭐 재밌는 일 있나요? :9" });
        talkData.Add(40000, new string[] { "[그린]" + "\n" + "\n" +"아. 피트라씨 안녕하세요.:12",
                                          "[그린]" + "\n" + "\n" +"좋은 하루 보내요 :13" });
        talkData.Add(50000, new string[] { "[마린]" + "\n" + "\n" +"나 바빠요.:15"  });
        talkData.Add(60000, new string[] { "[베니]" + "\n" + "\n" + "안녕하세요. 최고의 엔지니어 베니입니다.:18" });
        talkData.Add(70000, new string[] { "\n" + "\n" + "이 상자는 비어있다...:0" });
        talkData.Add(80000, new string[] { "[F-D0604]" + "\n" + "\n" + "사.라-지기 싫.ㅅ.ㄴㄴ니..:22" });
        talkData.Add(90000, new string[] { "[E-M0705]" + "\n" + "\n" + "전 해야 할 일이 아직 많습니다.:21" });
        talkData.Add(100000, new string[] { "[피트라]" + "\n" + "\n" + "문이..닫혀있어...?! :0" });



        /* quest talk (quest는 10번부터 시작) */


        /* quest 10 - Sindy에게 서류를 받자 */
        talkData.Add(10 + 20000, new string[] { "[???]"+"\n"+"\n"+"......씨?:6", "[???]"+"\n"+"\n"+"피트라씨?:7",
                                                "[피트라]"+"\n"+"\n"+".....:2",
                                                "[???]"+"\n"+"\n"+"피.트.라.씨?:7",
                                                "[피트라]"+"\n"+"\n"+"....네??:4",
                                                "[조니]"+"\n"+"\n"+"그러니까 Sindy 씨에게 맡겨놓은 문서 처리 좀 부탁해요. :5",
                                                "[피트라]"+"\n"+"\n"+".....예....? :4",
                                                "[조니]"+"\n"+"\n"+"부탁해요. 최대한 빨리.:5",
                                                "[피트라]"+"\n"+"\n"+".....아?.. 네네. 알겠습니다.:2",
                                                "[피트라]"+"\n"+"\n"+"(...방금 전까지 내가 뭐하고 있었더라):3" ,
                                                "[피트라]"+"\n"+"\n"+"(신디씨...? 빨간 머리였던 것 같은데..):3",
                                              });
        talkData.Add(11 + 20000, new string[] { "[조니]" + "\n" + "\n" + "문서는 신디씨에게 맡겨뒀어요. 부탁해요.:5" });
        talkData.Add(11 + 30000, new string[] {  "[신디]"+"\n"+"\n"+"안녕하세요~?:8",
                                                "[신디]"+"\n"+"\n"+"아 문서요? 여기요~:9",
                                                "[신디]"+"\n"+"\n"+"옆 사무실 B동 의 그린씨에게 전달 부탁해요~:8",
                                                "[피트라]"+"\n"+"\n"+"(그린씨… 밝은 갈색머리..):3",
                                                "[피트라]"+"\n"+"\n"+"(정신은 없어도 기억력은 좋군):1",
                                                "[신디]"+"\n"+"\n"+"아! 사람들과 부딪히지 않게 조심하세요~:9",
                                                "[신디]"+"\n"+"\n"+"다들 늘 화가 잔뜩 나있으니까요~!:8",
                                                "\n"+"\n"+"(문서를 얻었다.):0",
                                             });


        /*quest 20 - 서류를 B동으로 가져가자. */
        /*20 - 주요 대사들*/
        talkData.Add(20 + 40000, new string[] { "[그린]" + "\n" + "\n" + "피트라씨!!!:11",
                                               "[피트라]" + "\n" + "\n" + "예…?:4 ",
                                               "[피트라]" + "\n" + "\n" + "안녕하세요 그린씨:2",
                                               "[그린]" + "\n" + "\n" +"아...안녕하세요..:13",
                                               "[피트라]" + "\n" + "\n" + "여기 A동 문서입니다. 조니 상사님이 전해드리라고 하셨습니다.:1",
                                               "[그린]" + "\n" + "\n" +"감사해요:13",
                                               "[그린]" + "\n" + "\n" +"조니상사님께 오후까지 마무리 하겠다고 전해주세요.:12",
                                               "[피트라]" + "\n" + "\n" + "네. 그럼 수고하세요.:1",
                                               "[피트라]" + "\n" + "\n" + "(기분이 안좋아보이시네. 방금 뭐 실수했나..?):3",
                                               "[피트라]" + "\n" + "\n" + "(흠, 아마 일이 많아져서 그런 거 같군. 난 상사님께 보고하러 가야겠다.):2",
                                             });
        talkData.Add(21 + 40000, new string[] { "[그린]" + "\n" + "\n" + "조니상사님께 오후까지 마무리 하겠다고 전달 부탁드릴게요.:11" });
        talkData.Add(21 + 20000, new string[] { "[조니]" + "\n" + "\n" +"문서는 잘 전달했어요?:6",
                                               "[조니]" + "\n" + "\n" +"아, 고마워요. 역시 빠르다니까.:5",
                                               "[조니]" + "\n" + "\n" +"이건 피곤해보이는 피트라씨를 위한 감사선물.:6",
                                               "\n" + "\n" +"(조니에게 에너지 부스트를 받았다):200",
                                               "[조니]" + "\n" + "\n" +"마시면 힘이 불끈. 없던 에너지도 충전된다면서요??"+ "\n" +"광고에서...:6",
                                               "[조니]" + "\n" + "\n" +"그럼 오늘은 이만 들어가 봐요.:5",
                                               "[피트라]" + "\n" + "\n" + "네. 감사합니다:1",
                                               "[피트라]" + "\n" + "\n" + "(좋아. 퇴근이다!):1",
                                               "[피트라]" + "\n" + "\n" + "(얼른 짐싸서 집에 가자.):1",
                                               "[피트라]" + "\n" + "\n" + "(내 자리... 왼쪽 첫번째 책상이지.):2",
                                               });
        /*20 - 기타 대사들*/
        talkData.Add(20 + 20000, new string[] { "[조니]" + "\n" + "\n" + "전달하고나면 나한테 상황 좀 알려줘요."+"\n"+"고마워요:5" });
        talkData.Add(20 + 30000, new string[] { "[신디]" + "\n" + "\n" + "B동 그린씨에게 전달하면 돼요!:9",
                                               "[신디]"+"\n"+"\n"+"서두르는게 좋을거예요~:8" });
        talkData.Add(21 + 30000, new string[] { "[신디]" + "\n" + "\n" + "수고했어요~ 조니상사님이 기다리시고 계신 것 같던데요~?:9" });


        /*quest 30 - 피트라의 자리로 가자.*/
        talkData.Add(30 + 3000, new string[] { "\n" + "\n" + "[ 책상 위에 문서가 놓여있다 ]:0",
                                           "[피트라]" + "\n" + "\n" + "보안 A대상 문서... 이게 뭐지?:3",
                                           "\n" + "\n" + "A동 조정 관련. 담당 부서 E 동.:0",
                                           "\n" + "\n" + "대상 - 피트....:0",
                                           "[마린]" + "\n" + "\n" + "뭐야 이게 왜 여기있어? .:17",
                                           "[마린]" + "\n" + "\n" + "(마린이 신경질적으로 문서를 빼았았다.) .:16",
                                           "[피트라]" + "\n" + "\n" + "아.. 마린씨 문서입니까?:3",
                                           "[마린]" + "\n" + "\n" + "네. 설마 맘대로 열어본 건 아니죠?.:15",
                                           "[피트라]" + "\n" + "\n" + "제 책상에 있길래 확인을..:3",
                                           "[피트라]" + "\n" + "\n" + "그런데 저희 회사는 D동이 끝 아닙니까?:3",
                                           "[마린]" + "\n" + "\n" + "...:15",
                                           "[마린]" + "\n" + "\n" + "쓸데없는 소리 마요. 나 지금도 충분히 바쁘니까.:16",
                                           "[피트라]" + "\n" + "\n" + "...그러죠:3",
                                           "[피트라]" + "\n" + "\n" + "(문서에 그거 내 이름이었던 것 같아. E 동은 또 어디지?):3",
                                           "[피트라]" + "\n" + "\n" + "(혹시, 요즘 같은 힘든 세상에 벌써 해고..?):4",
                                           "[피트라]" + "\n" + "\n" + "무슨 일인지 알아봐야겠어." + "\n" +"일단 오늘은 집으로 돌아가자.:3"
                                         });

        /* quest 40 - 1층으로 나가 퇴근하자 */
        talkData.Add(40 + 8000, new string[] { "[문]" +"\n" + "\n" + "사용 권한이 없습니다.:0" ,
                                              "[피트라]" +"\n" + "\n" + "음, 이게 왜 안되지? :4" ,
                                              "[문]" +"\n" + "\n" + "권한이 없습니다.:0" ,
                                            });
        talkData.Add(41 + 40000, new string[] { "[그린]" + "\n" + "\n" + "저기..나가시는 문은 왼쪽이에요...:12",
                                               "[피트라]" + "\n" + "\n" + "아! 그린씨, 감사합니다. 제가 오늘 정신이 없어서..:2",
                                               "[그린]" + "\n" + "\n" + "이해해요. 안녕히가세요:12",
                                               "[피트라]" + "\n" + "\n" + "그린씨도 조심히가세요.:2",
                                             });
        talkData.Add(42 + 9000, new string[] { "[문]" + "\n" + "\n" + "문이 열립니다.:0"});

        /*40 - 기타 대사들*/
        talkData.Add(42 + 8000, new string[] { "[문]" + "\n" + "\n" + "사용 권한이 없습니다.:0" });
        talkData.Add(42 + 40000, new string[] { "[그린]" + "\n" + "\n" + "아, 왼쪽에 저 문으로 나가시면 돼요!:12" });
        talkData.Add(40 + 5000, new string[] { "\n" + "\n" + "-E동 - 출입 제한 구역 :0",
                                              "\n" + "\n" + "(잠겨있다) :0",
                                              "[피트라]" + "\n" + "\n" + "E동..아까 문서에서 본 곳이다. 뭐하는 곳일까? :3",
                                            });


        /* quest 50 - 출근 - 피트라의 자리로 가자. */
        talkData.Add(50 + 6000, new string[] { "[피트라]" +"\n" + "\n" + "...출근이다.  :3" ,
                                              "[피트라]" +"\n" + "\n" + "늦지 않게 얼른 내 자리로 가자.  :3" 
                                            });
        talkData.Add(50 + 3000, new string[] { "[피트라]" +"\n" + "\n" + "업무를 시작해볼까.:3" ,
                                            });

        /* quest 60 - 정전. */
        talkData.Add(60 + 10000, new string[] {"[피트라]" +"\n" + "\n" + "앗, 출근 하자마자 정전..? :4",
                                              "[피트라]" +"\n" + "\n" + "이래선 아무것도 할 수가 없는데 :3" ,
                                            });
        talkData.Add(61 + 10000, new string[] {"[피트라]" +"\n" + "\n" + "다들 뭐하는거야? 아무렇지도 않잖아..? :4",
                                              "[???]" +"\n" + "\n" + "-삐비빅-:0",
                                              "[피트라]" +"\n" + "\n" + "문자? :3" ,
                                              "[???]" +"\n" + "\n" + "-수리 필요-"+"\n"+"-수리 장비 3개를 모아 '고장 수리 센터' 방문 권고.- :0",
                                              "[???]" +"\n" + "\n" + "-수리 장비는 C동과 D동에 있음.- :0" ,
                                              "[피트라]" +"\n" + "\n" + "지금 나보고 수리를 도우라고? :4" ,
                                              "[피트라]" +"\n" + "\n" + "설마, 날 자르기 전에 맘껏 부려먹겠다는 건가? :4" ,
                                              "[피트라]" +"\n" + "\n" + "아무리 그래도... 정전까지 도와야하다니. :3" ,
                                              "[피트라]" +"\n" + "\n" + "...어쩔 수 없지. 빨리 해치우자. :3" 
                                            });
        talkData.Add(62 + 4000, new string[] {"[피트라]" +"\n" + "\n" + "아, 부품이다! :1",
                                             "[피트라]" +"\n" + "\n" + "좋아. 얼른 나머지 2개도 찾아보자. :2" 
                                            });
        talkData.Add(63 + 4000, new string[] {"[피트라]" +"\n" + "\n" + "찾았다! :1",
                                             "[피트라]" +"\n" + "\n" + "나머지 하나는 어디있으려나.. :2" 
                                            });
        talkData.Add(64 + 4000, new string[] {"[피트라]" +"\n" + "\n" + "좋아, 다 찾았다! :1",
                                             "[피트라]" +"\n" + "\n" + "'고장 수리 센터' 로 가라고 했지. :2" 
                                            });
        talkData.Add(65 + 60000, new string[] {"[베니]" +"\n" + "\n" + "아, 오셨습니까? :18",
                                              "[베니]" +"\n" + "\n" + "다행히 비상 전력이 있어서 완전 멈춰버리진 않았습니다. :19" ,
                                              "[베니]" +"\n" + "\n" + "제가 금방 고치겠습니다. 잠시만요~ :19"
                                            });

        talkData.Add(66 + 60000, new string[] {"[베니]" +"\n" + "\n" + "피트라씨? :18",
                                              "[베니]" +"\n" + "\n" + "피트라씨, 제가 솜씨 좋게 해결했습니다.  :19" ,
                                              "[피트라]" +"\n" + "\n" + "..... :3",
                                              "[피트라]" +"\n" + "\n" + "아....! 네네..! 제가 요즘 정신이 없어서... :4",
                                              "[피트라]" +"\n" + "\n" + "감사합니다. 수고하세요.  :2" ,
                                              "[피트라]" +"\n" + "\n" + "(정전 때문인지 하루가 금방갔네.):3",
                                              "[피트라]" +"\n" + "\n" + "(내일은 밀린 일이나 얼른 마무리 해야지.):3",
                                            });

        /*60 - 기타 대사들*/
        talkData.Add(60 + 20000, new string[] { "[조니]" + "\n" + "\n" + "수리 부품을 찾으신다고요?:6",
                                          "[조니]" + "\n" + "\n" +"보통 그런 물품은 종이 상자 안에 넣어놓긴한데,,글쎄요.:7" });
        talkData.Add(60 + 30000, new string[] { "[신디]" + "\n" + "\n" +"네?수리 부품이 어디있냐고요?:8",
                                          "[신디]" + "\n" + "\n" +" 음, 제가 상자 안에 넣어두긴 하는데.. 예전 일이라~:9" });
        talkData.Add(60 + 40000, new string[] { "[그린]" + "\n" + "\n" +"아. 피트라씨 안녕하세요.:12",
                                          "[그린]" + "\n" + "\n" +"수리 부품이라면 저희 사무실 뒤쪽 상자들 중에도 있을거예요. :13" });
        talkData.Add(60 + 50000, new string[] { "[마린]" + "\n" + "\n" + "수리 부품? 그걸 왜 저한테 물어보시죠?:15" });
        talkData.Add(60 + 60000, new string[] { "[베니]" + "\n" + "\n" + "안녕하세요. 최고의 엔지니어 베니입니다.:18",
                                                "[베니]" + "\n" + "\n" + "아,수리부품은 사무실마다 종이 상자에 구비해 두는 것 같더군요.:18"
                                               });

        talkData.Add(65 + 20000, new string[] {"[조니]" +"\n" + "\n" + "수리센터요..? :6",
                                              "[조니]" +"\n" + "\n" + "흠..3층에 있습니다. :7" ,
                                            });
        talkData.Add(65 + 30000, new string[] {"[신디]" +"\n" + "\n" + "어디가세요? 바빠보이네요~:8",
                                              "[피트라]" +"\n" + "\n" + "고장 수리센터.. 어딘지 아십니까? :2",
                                              "[신디]" +"\n" + "\n" + "아~ 3층에 있어요~  :9" ,
                                            });
        talkData.Add(65 + 40000, new string[] {"[그린]" +"\n" + "\n" + "어머, 안녕하세요. :11",
                                             "[그린]" +"\n" + "\n" + "아, 수리센터요? 3층에 있어요. :13" ,
                                            });
        talkData.Add(60 + 5000, new string[] { "\n" + "\n" + "-E동 - 출입 제한 구역 :0",
                                              "\n" + "\n" + "(잠겨있다) :0",
                                              "[피트라]" + "\n" + "\n" + "E동..아까 문서에서 본 곳이다. 뭐하는 곳일까? :3",
                                            });

        /* quest 70 - 정전. */
        talkData.Add(70 + 6000, new string[] {"[피트라]" +"\n" + "\n" + "오늘도 똑같은 하루의 시작이네. :3",
                                              "[피트라]" +"\n" + "\n" + "참, 아까 신디씨한테 문자 와있던데... 신디씨에게 먼저 들러보자. :2" ,
                                            });
        talkData.Add(71 + 30000, new string[] {"[신디]" +"\n" + "\n" + "-! -----. :8",
                                              "[피트라]" +"\n" + "\n" + "...예?... :4" ,
                                              "[신디]" +"\n" + "\n" + "-,- -- -----? :9",
                                              "[피트라]" +"\n" + "\n" + "좀 크게 말해주시겠습니까? :3" ,
                                              "[신디]" +"\n" + "\n" + "-...- -- ------. :9",
                                              "[피트라]" +"\n" + "\n" + "(신디씨 목소리가 들리지 않는다.) :3" ,
                                              "[피트라]" +"\n" + "\n" + "(잠깐..) :4" ,
                                              "[피트라]" +"\n" + "\n" + "(신디씨 목소리만 들리지 않는게 아니야) :3" ,
                                              "[피트라]" +"\n" + "\n" + "(아무런 소리도 들리지 않아!) :4" ,
                                              "[피트라]" +"\n" + "\n" + " 신디씨!! 저 지금 소리가, 아무소리도 들리지 않아요!!!:4" ,
                                              "[신디]" +"\n" + "\n" + "...:9" ,
                                              "[피트라]" +"\n" + "\n" + " 지금 급히 병원에 가봐야겠습니다. 조니 상사님께 말씀 부탁드립니다.:4" ,
                                              "[신디]" +"\n" + "\n" + "...:9" ,
                                              "[피트라]" +"\n" + "\n" + " (왜 아무런 반응도 없는거지? 일단 어서 나가자.):3" ,
                                            });

        /* quest 80 - 정전. */
        talkData.Add(80 + 9000, new string[] { "[문]" + "\n" + "\n" + "현재 시간에는 사용이 불가능한 문입니다:0",
                                                "[피트라]" +"\n" + "\n" + "뭐? 무슨 이런 말도 안되는 회사가 다 있어??:4" ,
                                                "[피트라]" +"\n" + "\n" + "나 급하다고!!!:4" ,
                                                "[피트라]" + "\n" + "\n" + "오른쪽 문.. 지금도 안열리나? :3",
                                             });
        talkData.Add(81 + 8000, new string[] { "[문]" + "\n" + "\n" + "사용 권한이 없습니다.:0",
                                               "[피트라]" +"\n" + "\n" + "회사에 갖혀? 말도 안돼.:4" ,
                                               "[피트라]" +"\n" + "\n" + "조니 상사에게 가서 말하면 나가게 해주실거야...분명히.:3" ,
                                             });
        talkData.Add(82 + 7500, new string[] { "[피트라]" + "\n" + "\n" + "앗, 안계시잖아..? :4",
                                               "[피트라]" + "\n" + "\n" + "컴퓨터는 켜져 있는걸 보니 잠깐 나가신 것 같은데..:3",
                                               "[피트라]" +"\n" + "\n" + "음? 근데 이 문서는 뭐지?:4" ,
                                               "[피트라]" +"\n" + "\n" + "제품명 M-0705 피트라 폐기 승인 보고서....:23" ,
                                               "[피트라]" +"\n" + "\n" + "이게 무슨소리야...? 누가 날 이렇게..:23" ,
                                               "[피트라]" +"\n" + "\n" + "....이에 피트라 폐기 및.. 새 제품을 요청함...?:23" ,
                                               "[피트라]" +"\n" + "\n" + "담당..신디. :23" ,
                                               "[피트라]" +"\n" + "\n" + "내가 사람이 아니고, 나를 폐기시킨다고? :4" ,
                                               "[피트라]" +"\n" + "\n" + "말도.. 안돼... :3" ,
                                               "[피트라]" +"\n" + "\n" + "저번에 본 그 문서... 나와 관련된게 맞았어.. :3" ,
                                               "[피트라]" +"\n" + "\n" + "어제도,, 정전이 아니었어. :3" ,
                                               "[피트라]" +"\n" + "\n" + "그래서 다른사람들은 아무렇지도 않게 있었던거야... :4" ,
                                               "[피트라]" +"\n" + "\n" + "침착하자..날 폐기한다고 했지. :3" ,
                                               "[피트라]" +"\n" + "\n" + "이대로 사라질 수 없어. :3" ,
                                               "[피트라]" +"\n" + "\n" + " 이 미친 건물을 나갈거야 :4" ,
                                               "[피트라]" +"\n" + "\n" + "(나는 문서를 쓰레기통에 던졌다.) :3" ,
                                               "[피트라]" +"\n" + "\n" + "나를 도와줄 수 있는 사람을.. 찾아보자. :3" ,
                                               "[피트라]" +"\n" + "\n" + "…있을지 모르겠지만 :3" ,
                                             });
        /*80 - 기타 대사들*/
        talkData.Add(82 + 8000, new string[] { "[문]" + "\n" + "\n" + "사용 권한이 없습니다.:0",
                                               "[피트라]" +"\n" + "\n" + "그래, 조니 상사에게 가보자... 분명 나가게 해주실거야.:3" ,
                                             });

        /*quest 90 - 도움을 받을만한 사람을 찾아보자*/
        talkData.Add(90 + 6500, new string[] {"[피트라]" +"\n" + "\n" + "아..! :4",
                                              "[피트라]" +"\n" + "\n" + "다시 소리가 들린다. :4",
                                              "[피트라]" +"\n" + "\n" + "정말..다행이야. 잠깐 이상했던 것 뿐이야. :3" ,
                                              "[E-S0823]" +"\n" + "\n" + "폐기 대상 발견. :25" ,
                                              "[E-S0823]" +"\n" + "\n" + "폐기 시작. :25" ,
                                              "[E-J0518]" +"\n" + "\n" + "폐기 대상 발견.:25" ,
                                              "[E-J0518]" +"\n" + "\n" + "폐기 시작. :25" ,
                                              "[피트라]" +"\n" + "\n" + "으악! 도망가!!! :3" ,
                                            });

        talkData.Add(91 + 40000, new string[] {"[그린]" +"\n" + "\n" + "피트라씨..!:24",
                                              "[그린]" +"\n" + "\n" + "피트라씨가 지금 어떤 상황인지 알고있어요. :24",
                                              "[피트라]" +"\n" + "\n" + "다른 사람들에게 말하지만 말아주세요.부탁입니다. :3" ,
                                              "[그린]" +"\n" + "\n" + "걱정마세요. :12" ,
                                              "[그린]" +"\n" + "\n" + "제가 도와드릴게요. :12" ,
                                              "[피트라]" +"\n" + "\n" + "네?:4" ,
                                              "[피트라]" +"\n" + "\n" + "그린씨가 절 왜... :3" ,
                                              "[그린]" +"\n" + "\n" + "피트라씨에게 받은 도움이 있거든요. :11" ,
                                              "[피트라]" +"\n" + "\n" + "제가 언제.. :4" ,
                                              "[그린]" +"\n" + "\n" + "기계에 깔린 절 구해주시려다 잘못돼서 초기화가 되긴 하셨지만,, :13" ,
                                              "[그린]" +"\n" + "\n" + "며칠전에 절 못알아보셔서 얼마나 서운했는지 몰라요. :12" ,
                                              "[피트라]" +"\n" + "\n" + "제가..그린씨를 돕다 초기화가 됐었다고요?:4" ,
                                              "[그린]" +"\n" + "\n" + "네, 그땐 고맙다는 말도 못했네요.:12",
                                              "[그린]" +"\n" + "\n" + "고마워요 피트라씨 :11",
                                              "[그린]" +"\n" + "\n" + "여기 아래가 택배 출입구로 가는 문이예요. :12",
                                              "[그린]" +"\n" + "\n" + "열기 전까지는 아무나, 정확히는.. 인공지능은 열수도 볼수도 없고...:13",
                                           /*   "[그린]" +"\n" + "\n" +  "피트라씨가 유일하게 탈출할 수 있는 길이죠. :13",
                                              "[피트라]" +"\n" + "\n" + "정말.. 감사합니다 그린씨:2" ,
                                              "[그린]" +"\n" + "\n" + "저는 피트라씨를 뒤따라가는 로봇들을 최대한 꺼볼게요.:12",
                                              "[그린]" +"\n" + "\n" + "오는 길에.. 수리실에서 조종 리모컨을 하나 주웠...거든요.:11",
                                              "[피트라]" +"\n" + "\n" + "그럼...부탁드립니다:3" ,
                                              "[그린]" +"\n" + "\n" + "행운을 빌게요. 나중에 또 봐요. :11",*/
                                              "[피트라]" +"\n" + "\n" + "나중에... 밖에서 봬요 그린씨. :2" ,
                                            });

        talkData.Add(100 + 40000, new string[] { "[그린]" + "\n" + "\n" + "조심히 가요.:11" });
        talkData.Add(100 + 10000, new string[] { "[피트라]" + "\n" + "\n" + "으악! 경찰이 한가득이잖아??:4" ,
                                                 "[피트라]" + "\n" + "\n" + "뛰어!:4" ,
                                                });
    }

    public string GetTalk (int id, int talkIndex) // .GetTalk(id + questTalkIndex, talkIndex) 이렇게 Game Manager에서쓰임
    {
        Debug.Log("GetTalk에 들어옴. id:" + id + "talkINdex: " + talkIndex);
        if (!talkData.ContainsKey(id))// 특정퀘스트의 대사를 만들어놓지 않은 npc에게 말을 건다면?
        {
            Debug.Log("이전 대사들 가져옴");
            //해당 퀘스트 진행 순서 중 대사가 없을 때
            if (!talkData.ContainsKey(id - id % 10))
            {   //퀘스트의 맨 처음 대사마져 없을 때는
                //기본 대사를 가지고 온다.
                return GetTalk(id - id % 100, talkIndex);

                /*
                if (talkIndex == talkData[id - id % 100].Length) // 얘기가 모두 끝났을 때
                    return null;
                else
                    return talkData[id - id % 100][talkIndex];
                */
            }
            else
            {
                //해당 퀘스트 진행 순서 중 대사가 없을 때
                //퀘스트의 맨 처음 대사를 가지고 온다.
                return GetTalk(id - id % 10, talkIndex);

                /*
                if (talkIndex == talkData[id - id % 10].Length) // 얘기가 모두 끝났을 때
                    return null;
                else
                    return talkData[id - id % 10][talkIndex];
                */
            }
        }

        if (talkIndex >= talkData[id].Length)
        {// 얘기가 모두 끝났을 때
            Debug.Log("한사람과 얘기 끝");
            return null;
        }
        else
        {
            Debug.Log("있는 말 바로 가져다 줌");
            return talkData[id][talkIndex];
        }
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[portraitIndex];
    }

}
