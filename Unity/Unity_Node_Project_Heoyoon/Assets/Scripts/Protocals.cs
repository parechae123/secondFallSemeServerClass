using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocals : MonoBehaviour
{
    public class Packets
    {
        public int adf;
        public class common
        {
            public int cmd;                     //Ŀ�ǵ� ��ȣ
            public string message;      //�޼���
        }
        public class req_data : common  //Common ���
        {
            public int id;
            public string data;
        }
    }
}
