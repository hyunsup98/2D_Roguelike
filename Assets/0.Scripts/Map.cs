using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map : MonoBehaviour
{
    //���⺰ ��Ż, �⺻���� SetActive(false)
    public GameObject upPortal;
    public GameObject downPortal;
    public GameObject leftPortal;
    public GameObject rightPortal;

    //���� �ʿ��� �̵������� ������ true, �Ұ����� false
    public bool isUp;
    public bool isDown;
    public bool isLeft;
    public bool isRight;

}
