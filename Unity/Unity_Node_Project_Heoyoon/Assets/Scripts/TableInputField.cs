using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableInputField : MonoBehaviour
{
    public InputField[] inputFields;       //TabŰ�� �̵��� InputField�迭
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject!= null)
            {
                InputField currentInputField = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();
                if (currentInputField != null)
                {
                    int currentIndex = System.Array.IndexOf(inputFields, currentInputField);
                    int nextIndex = (currentIndex + 1) % inputFields.Length;//���� ��ǲ�ʵ� �ε��� ���

                    //���� Input���� ��Ŀ�� �̵�
                    inputFields[nextIndex].Select();
                    inputFields[nextIndex].ActivateInputField();
                }
            }

        }
    }
}
