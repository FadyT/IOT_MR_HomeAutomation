using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class CustomXRPoke : MonoBehaviour
{
    [SerializeField]
    string Name;
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    MeshRenderer material;
    bool clicked = false;
    [SerializeField]
    int pinNumber;



    public void ClickButton()
    {
        if (clicked)
        {
            text.text = Name + " is ON";
            material.material.color = Color.green;
            StartCoroutine(GetRequest("http://aquaponic.c1.is/esp-outputs-action.php?action=output_update&id=" + pinNumber + "&state=0"));
            clicked= false;
        }
        else
        {
            text.text = Name + " is OFF";
            material.material.color = Color.yellow;
            StartCoroutine(GetRequest("http://aquaponic.c1.is/esp-outputs-action.php?action=output_update&id=" + pinNumber + "&state=1"));
            clicked = true;
        }
    }




    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }

    }
}