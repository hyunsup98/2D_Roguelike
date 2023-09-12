using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject upPortal;
    public GameObject downPortal;
    public GameObject leftPortal;
    public GameObject rightPortal;

    //현재 맵에서 이동가능한 방향은 true, 불가능은 false
    public bool isUp;
    public bool isDown;
    public bool isLeft;
    public bool isRight;

    //현재 맵의 2차원 배열 주소값
    public int Row;
    public int Col;
}
