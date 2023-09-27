using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableInputField : MonoBehaviour
{
    public InputField[] inputFields;       //Tab키로 이동할 InputField배열
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
                    int nextIndex = (currentIndex + 1) % inputFields.Length;//다음 인풋필드 인덱스 계산

                    //다음 Input랴딩 포커스 이동
                    inputFields[nextIndex].Select();
                    inputFields[nextIndex].ActivateInputField();
                }
            }

        }
    }
}
