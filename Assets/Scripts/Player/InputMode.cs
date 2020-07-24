/*
 * Class: InputMode
 * Date: 2020.7.24
 * Last Modified : 2020.7.24
 * Author: Hyukin Kwon 
 * Description: 인풋 모드(모바일용,PC용)를 다루는 클래스
 *              1. 더 쾌적한 유지보수를 위해 추가함.
                2. 더 많은 인풋 시스템을 추가하게 될수록 효과적인 클래스.
 */

using UnityEngine;

public static class InputMode
{
    public enum InputEnum
    {
        Mobile, PC
    }

    //사용되는 플랫폼에 기반해 인풋 변수를 자동으로 설정
#if (UNITY_ANDROID || UNITY_IOS)
    private static InputEnum currentInput = InputEnum.Mobile;
#elif (UNITY_STANDALONE)
    private static InputEnum currentInput = InputEnum.PC;
#endif

    //모바일용 인풋 헨들러 (터치를 받음)
    private static void PlayerMobileInputHandler(PlayerController target)
    {
        //터치하지 않으면 버트 초기화
        if (Input.touchCount == 0)
        {
            target.SetIsLeftButtonPressed(false);
            target.SetIsRightButtonPressed(false);
            target.SetIsJumpButtonPressed(false);
        }
        else
        {
            //터치 감지용 레이케스트
            int layerMask = LayerMask.GetMask("JumpButton", "LeftButton", "RightButton");
            RaycastHit2D hit;
            for (int i = 0; i < Input.touchCount; i++)
            {
                //버튼 터치를 했으면 위치를 추적해 어떤 버튼인지 찾는 과정
                Touch touch = Input.GetTouch(i);
                Vector2 pos = touch.position;
                Vector2 dir = Camera.main.ScreenToWorldPoint(pos);
                hit = Physics2D.Raycast(pos, Vector2.zero, 1f, layerMask);
                Debug.Log(hit);

                //버튼을 터치 했으면
                if (hit)
                {
                    int layer = hit.transform.gameObject.layer;

                    if (layer == LayerMask.NameToLayer("JumpButton"))  //점프 버튼 이벤트
                    {
                        if (touch.phase == TouchPhase.Began && target.GetIsGrounded())
                        {
                            target.SetIsJumpButtonPressed(true);
                        }
                        if (touch.phase == TouchPhase.Ended)
                        {
                            target.SetIsJumpButtonPressed(false);
                        }
                    }
                    else if (layer == LayerMask.NameToLayer("LeftButton")) //왼쪽 버튼 이벤트
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            target.SetIsLeftButtonPressed(true);
                            if (target.GetIsRightButtonPressed())
                            {
                                target.SetIsRightButtonPressed(false);
                            }
                        }
                        if (touch.phase == TouchPhase.Moved)
                        {
                            //터치가 다른곳에서 시작했어도 때지않고 왼쪽 버튼으로 왔으면 터치로 인정
                            target.SetIsRightButtonPressed(false);
                            target.SetIsLeftButtonPressed(true);
                        }
                        if (touch.phase == TouchPhase.Ended)
                        {
                            target.SetIsLeftButtonPressed(false);
                        }
                    }
                    else if (layer == LayerMask.NameToLayer("RightButton")) //오른쪽 버튼 이벤트
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            target.SetIsRightButtonPressed(true);
                            if (target.GetIsLeftButtonPressed())
                            {
                                target.SetIsLeftButtonPressed(false);
                            }
                        }
                        if (touch.phase == TouchPhase.Moved)
                        {
                            //터치가 다른곳에서 시작했어도 때지않고 오른쪽 버튼으로 왔으면 터치로 인정
                            target.SetIsRightButtonPressed(true);
                            target.SetIsLeftButtonPressed(false);
                        }
                        if (touch.phase == TouchPhase.Ended)
                        {
                            target.SetIsRightButtonPressed(false);
                        }
                    }
                }
            }
        }

        //오른쪽 왼쪽 버튼 상태에 따라 X축 속도 조정 (-1~1)
        float horizontalMove = 0;
        float increamentSpeed = 60f; //증가 속도 값
        if (target.GetIsRightButtonPressed())
        {
            horizontalMove += Time.deltaTime * increamentSpeed;
            target.SetIsLeftButtonPressed(false);
        }
        if (target.GetIsLeftButtonPressed())
        {
            horizontalMove -= Time.deltaTime * increamentSpeed;
            target.SetIsRightButtonPressed(false);
        }
        if (!target.GetIsLeftButtonPressed() && !target.GetIsRightButtonPressed())
        {
            horizontalMove = 0;
        }
        horizontalMove = Mathf.Clamp(horizontalMove, -1, 1); //최소 최대값 설정
        target.SetIsHorizontalMove(horizontalMove);
    }
    
    //PC용 인풋 핸들러 (키보드와 마우스를 받음)
    private static void PlayerPcInputHandler(PlayerController target)
    {
        if (Input.GetButtonDown("Jump"))
        {
            target.SetIsJumpButtonPressed(true);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            target.SetIsJumpButtonPressed(false);
        }

        //X축 속도에 따라 오른쪽 왼쪽 버트 상태 선택 
        target.SetIsHorizontalMove(Input.GetAxisRaw("Horizontal"));
        if (target.GetHorizontalMove() < 0)
        {
            target.SetIsLeftButtonPressed(true);
            target.SetIsRightButtonPressed(false);
        }
        else if (target.GetHorizontalMove() > 0)
        {
            target.SetIsLeftButtonPressed(false);
            target.SetIsRightButtonPressed(true);
        }
        else
        {
            target.SetIsLeftButtonPressed(false);
            target.SetIsRightButtonPressed(false);
        }
    }


    //currentInput를 참조해 현재 인풋 모드에 맞는 인풋헨들러를 사용 
    public static void PlayerInputHandler(PlayerController target)
    {
        if(currentInput == InputEnum.Mobile)
        {
            PlayerMobileInputHandler(target);
        }
        else if(currentInput == InputEnum.PC)
        {
            PlayerPcInputHandler(target);
        }
    }
}
