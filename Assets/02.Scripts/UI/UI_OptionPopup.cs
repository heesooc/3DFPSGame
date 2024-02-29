using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OptionPopup : MonoBehaviour
{
    public void Open()
    {
        // 사운드 효과음이라던지
        // 초기화 함수
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }


    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void OnContinueButtonClicked()
    {
        Debug.Log("계속하기 버튼 클릭");
        GameManager.Instance.Continue();

        Close();
    }

    public void OnAgainButtonClicked()
    {
        Debug.Log("다시하기 버튼 클릭");
        Close();
    }

    public void OnEndButtonClicked()
    {
        Debug.Log("게임종료 버튼 클릭");
        Close();
    }


}
