using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;

public class UnityToNode : MonoBehaviour
{
    public string host;                 //127.0.0.1 
    public int port;                    //3030

    public string idUrl;                //경로 주소 설정
    public string postUrl;
    public string resDataUrl;
    public string startConstructionUrl;
    public string checkConstructionUrl;

    public Button btnGetExample;
    public Button btnPostExample;
    public Button btnResDataExample;
    public Button btnConstruction_Post;
    public Button btnConstruction_Get;

    public int id;
    public string data;

    private void Start()
    {
        this.btnPostExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}/{2}", host, port, postUrl);      //URL 주소 생성
            Debug.Log(url);

            var req = new Protocols.Packets.req_data();                         //Req 프로토콜 데이터 입력
            req.cmd = 1000;
            req.id = id;
            req.data = data;
            var json = JsonConvert.SerializeObject(req);
            Debug.Log(json);

            StartCoroutine(this.PostData(url, json, (raw) =>
            {
                Protocols.Packets.common res = JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);
                Debug.LogFormat("{0}, {1}", res.cmd, res.message);
            }));

        });

        this.btnGetExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}/{2}", host, port, idUrl);      //URL 주소 생성
            Debug.Log(url);

            StartCoroutine(this.GetData(url, (raw) =>
            {
                var res = JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);       //JSON 변환

                Debug.LogFormat("{0}, {1}", res.cmd, res.message);          //디버그로그로 서버에서 보내준것 확인
            }));

        });

        this.btnResDataExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}/{2}", host, port, resDataUrl);      //URL 주소 생성
            Debug.Log(url);

            StartCoroutine(this.GetData(url, (raw) =>
            {
                var res = JsonConvert.DeserializeObject<Protocols.Packets.res_data>(raw);       //JSON 변환

                foreach (var user in res.result)
                {
                    Debug.LogFormat("{0}, {1}", user.id, user.data);          //디버그로그로 서버에서 보내준것 확인
                }

            }));
        });

        this.btnConstruction_Post.onClick.AddListener(() =>             //건설 시작 POST 통신 
        {
            var url = string.Format("{0}/{2}", host, port, startConstructionUrl);      //URL 주소 생성
            Debug.Log(url);

            var req = new Protocols.Packets.req_data();                                 //프로토콜을 만들어준다. 
            req.cmd = 1000;
            req.id = id;
            req.data = data;
            var json = JsonConvert.SerializeObject(req);
            Debug.Log(json);

            StartCoroutine(this.PostData(url, json, (raw) =>
            {
                Protocols.Packets.common res = JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);
                Debug.Log(res);
            }));
        });

        this.btnConstruction_Get.onClick.AddListener(() =>              //건설 확인 GET 통신
        {
            var url = string.Format("{0}/{2}", host, port, checkConstructionUrl);      //URL 주소 생성
            Debug.Log(url);

            StartCoroutine(this.GetData(url, (raw) =>
            {
                var res = JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);
                Debug.LogFormat("{0}, {1}", res.cmd, res.message);
            }));
        });
    }

    private IEnumerator GetData(string url, System.Action<string> callback)
    {
        var webRequest = UnityWebRequest.Get(url);                //유니티 함수 UnityWebRequest의 Get
        yield return webRequest.SendWebRequest();                   //통신이 돌아올때 까지 코루틴 대기

        Debug.Log("-->" + webRequest.downloadHandler.text);

        if (webRequest.result == UnityWebRequest.Result.ConnectionError ||   //커넥션 Error 이거나
            webRequest.result == UnityWebRequest.Result.ProtocolError)      //프로토콜 Error 일경우
        {
            Debug.Log("서버 통신 에러");
        }
        else  //에러가 없을 경우
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

        if (webRequest.result == UnityWebRequest.Result.ConnectionError ||   //커넥션 Error 이거나
           webRequest.result == UnityWebRequest.Result.ProtocolError)      //프로토콜 Error 일경우
        {
            Debug.Log("서버 통신 에러");
        }
        else  //에러가 없을 경우
        {
            Debug.LogFormat("{0}\n{1}\n{2}", webRequest.responseCode, webRequest.downloadHandler.data, webRequest.downloadHandler.text);
            callback(webRequest.downloadHandler.text);
        }

        webRequest.Dispose();       //연결 해제 (없으면 메모리 누수)
    }


}
