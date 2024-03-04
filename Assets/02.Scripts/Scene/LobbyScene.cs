using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneNames
{
    Lobby,  // 0
    Loading,   // 1
    Main   // 2
}

public class LobbyScene : MonoBehaviour
{
    // 사용자 계정을 새로 저장하거나(회원가입), 저장된 데이터를 읽어
    // 사용자 입력과 일치하는지 검사(로그인)한다.
    //public SceneNames NextScene = SceneNames.Main;

    public TMP_InputField IDInputField;         // 아이디 입력창
    public TMP_InputField PasswordInputField;   // 비밀번호 입력창
    public TextMeshProUGUI NotifyTextUI;        // 알림 텍스트

    private void Start()
    {
        IDInputField.text = string.Empty;
        PasswordInputField.text = string.Empty;
        NotifyTextUI.text = string.Empty;
    }

    // 회원가입 버튼 클릭
    public void OnClickRegisterButton()
    {
        string id = IDInputField.text;
        string pw = PasswordInputField.text;

        // 0. 아이디 또는 비밀번호를 입력하지 않은 경우
        if (id == string.Empty || pw == string.Empty)
        {
            NotifyTextUI.text = "아이디와 비밀번호를 정확하게 입력해주세요.";
            return;
        }

        // 1. 이미 같은 계정으로 회원가입이 되어있는 경우
        if (PlayerPrefs.HasKey(id))
        {
            NotifyTextUI.text = "이미 존재하는 계정입니다.";
        }
        // 2. 회원가입에 성공하는 경우
        else
        {
            NotifyTextUI.text = "회원가입을 완료했습니다.";
            PlayerPrefs.SetString(id, pw);
        }

        IDInputField.text = string.Empty;
        PasswordInputField.text = string.Empty;
    }

    // 로그인 버튼 클릭
    public void OnClickLoginButton()
    {
        string id = IDInputField.text;
        string pw = PasswordInputField.text;

        // 0. 아이디 또는 비밀번호를 입력x   -> "아이디와 비밀번호를 정확하게 입력해주세요."
        if (id == string.Empty || pw == string.Empty)
        {
            NotifyTextUI.text = "아이디와 비밀번호를 정확하게 입력해주세요.";
            return;
        }
        // 1. 없는 아이디                    -> "아이디를 확인해주세요."
        if(!PlayerPrefs.HasKey(id))
        {
            NotifyTextUI.text = "아이디와 비밀번호를 확인해주세요.";
        }
        // 2. 틀린 비밀번호                  -> "비밀번호를 확인해주세요."
        if (PlayerPrefs.GetString(id) != pw)
        {
            NotifyTextUI.text = "아이디와 비밀번호를 확인해주세요.";
        }
        // 3. 로그인 성공                    ->  메인 씬으로 이동
        else
        {
            NotifyTextUI.text = "로그인에 성공했습니다.";
            // 메인 씬으로 이동 // 로딩 씬으로 이동
            //SceneManager.LoadScene("MainScene");
            SceneManager.LoadScene((int)SceneNames.Loading);
        }

    }

}
