using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using TechnomediaLabs;

namespace Zetcil
{
    public class LoginModel : MonoBehaviour
    {
        public bool isEnabled;

        [Header("Login Settings")]
        public string MainURL;
        [HideInInspector] public string SubmitURL;

        [Header("Variable Settings")]
        public InputField ID;
        public InputField Password;

        [Header("Output Settings")]
        public VarString RequestOutput;
        public bool PrintDebugConsole;

        [Header("Login Result")]
        public UnityEvent SuccessEvent;
        public UnityEvent FailedEvent;

        [Header("User Output")]
        public bool usingUserOutput;
        public VarString UserID;
        public VarString UserEmail;
        public VarString UserPassword;
        public VarScore UserScore;
        public VarFloat UserEnergy;
        public VarFloat UserCoin;
        public VarFloat UserDiamond;

        public void InvokeAdminLogin()
        {
            if (ID.text == "admin" && Password.text == "admin")
            {
                SuccessEvent.Invoke();
            } else
            {
                FailedEvent.Invoke();
            }
        }

        public void InvokeLogin()
        {
            SubmitURL = MainURL + 
                       "/" + ID.text +
                       "/" + Password.text;
            StartCoroutine(StartPHPRequest());
        }

        IEnumerator StartPHPRequest()
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(SubmitURL);
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debugger.Save(webRequest.error);
                if (PrintDebugConsole)
                {
                    Debug.Log(SubmitURL);
                    Debug.Log(webRequest.downloadHandler.text);
                }
            }
            else
            {
                RequestOutput.CurrentValue = webRequest.downloadHandler.text;
                Debugger.Save(webRequest.downloadHandler.text);
                if (PrintDebugConsole)
                {
                    Debug.Log(SubmitURL);
                    Debug.Log(webRequest.downloadHandler.text);
                }
            }

            string[] resLogin = RequestOutput.CurrentValue.Split('|');

            if (resLogin[0] == "LOGIN_FAILED")
            {
                FailedEvent.Invoke();
            }
            else if (resLogin[0] == "LOGIN_SUCCESS")
            {
                SuccessEvent.Invoke();

                if (usingUserOutput)
                {
                    UserID.CurrentValue = resLogin[1];
                    UserEmail.CurrentValue = resLogin[2];
                    UserPassword.CurrentValue = resLogin[3];
                    UserScore.CurrentValue = ToFloat(resLogin[4]);
                    UserEnergy.CurrentValue = ToFloat(resLogin[5]);
                    UserCoin.CurrentValue = ToFloat(resLogin[6]);
                    UserDiamond.CurrentValue = ToFloat(resLogin[7]);
                }
            }
        }

        float ToFloat(string aValue)
        {
            float result = 0;
            if (aValue == "")
            {
                result = 0;
            } else 
            {
                result = float.Parse(aValue);
            }
            return result;
        }

        public void LoginFailed()
        {
            Debug.Log("Message::LoginFailed");
        }

        public void LoginSuccess()
        {
            Debug.Log("Message::LoginSuccess");
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
