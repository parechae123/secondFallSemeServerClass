using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;

public class UnityToNode : MonoBehaviour
{
    public string host;         //127.0.0.1
    public int port;                //3030
    public string idUri;            //경로 주소 설정
    public string postUri;

    public Button btnGetExample;
    public Button btnPostExample;

    public int id;
    public string data;
    public void Start()
    {
        this.btnPostExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}:{1}/{2}", host, port, postUri);
            Debug.Log(url);

            var req = new Protocals.Packets.req_data();
            req.cmd = 1000;
            req.id = id;
            req.data = data;
            var json = JsonConvert.SerializeObject(req);
            Debug.Log(json);

            StartCoroutine(this.PostData(url, json, (raw) =>
            {
                Protocals.Packets.common res = JsonConvert.DeserializeObject<Protocals.Packets.common>(raw);
                Debug.LogFormat("{0},{1}", res.cmd, res.message);
            }));
        });

        this.btnGetExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}:{1}/{2}", host, port, idUri);      //URL 주소 생성 
            Debug.Log(url);

            StartCoroutine(GetData(url, (raw) =>
            {
                var res = JsonConvert.DeserializeObject<Protocals.Packets.common>(raw); //Json 변환

                Debug.LogFormat("{0},{1}",res.cmd,res.message);         //디버그로그로 서버에서 보내준 것 확인
            }));
        });
    }
    private IEnumerator GetData(string url,System.Action<string> callback)
    {
        var webRequest = UnityWebRequest.Get(url);              //유니티 함수 UnityWebRequest의 Get
        yield return webRequest.SendWebRequest();               //통신이 돌아올때 까지 코루틴 대기

        Debug.Log("-->" + webRequest.downloadHandler.text);

        if(webRequest.result == UnityWebRequest.Result.ConnectionError||            //커넥션 Error 이거나
            webRequest.result == UnityWebRequest.Result.ProtocolError)                  //통신이 돌아오거나
        {
            Debug.Log("서버 통신 에러");

        }
        else        //에러가 없을 경우
        {
            callback(webRequest.downloadHandler.text);
        }

    }
    private IEnumerator PostData(string url, string json, System.Action<string> callback)
    {
        var webRequest = new UnityWebRequest(url, "POST");
        var bodyRaw = Encoding.UTF8.GetBytes(json);     //json 인코딩

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        yield return webRequest.SendWebRequest();
        if (webRequest.result == UnityWebRequest.Result.ConnectionError ||            //커넥션 Error 이거나
            webRequest.result == UnityWebRequest.Result.ProtocolError)                  //통신이 돌아오거나
        {
            Debug.Log("서버 통신 에러");

        }
        else        //에러가 없을 경우
        {
            Debug.LogFormat("{0}\n{1}\n{2}", webRequest.responseCode, webRequest.downloadHandler.data, webRequest.downloadHandler.text);
            callback(webRequest.downloadHandler.text);
        }
        webRequest.Dispose();           //연결 해제 (없으면 메모리 누수)
    }
}
