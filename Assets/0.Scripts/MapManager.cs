using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //맵 프리팹을 넣을 배열
    [SerializeField] List<Map> mapList = new List<Map>();
    //맵 프리팹이 들어갈 배열
    public transMap[,] maps = new transMap[5, 5];


    private void Awake()
    {
        int count = 0;
        for (int i = 0; i < maps.GetLength(0); i++)
        {
            for (int j = 0; j < maps.GetLength(1); j++)
            {
                maps[i, j] = transform.GetChild(count).GetComponent<transMap>();
                count++;
            }
        }

        SetStartMap();
    }


    //처음 시작할 맵 타입과 위치를 정해주기
    void SetStartMap()
    {

        //맵 생성 밑 리스트에서 삭제하기
        int randMap = Random.Range(0, mapList.Count);
        Map startMap = Instantiate(mapList[randMap]);
        mapList.RemoveAt(randMap);

        //생성한 시작 맵을 담을 랜덤 위치 생성하기
        int randRow = Random.Range(0, maps.GetLength(0));
        int randCol = Random.Range(0, maps.GetLength(1));
        transMap startPos = maps[randRow, randCol];

        //시작 맵을 지정한 위치에 담기
        startPos.GetComponent<transMap>().map = startMap;
        startMap.transform.SetParent(startPos.transform);
        startMap.transform.localPosition = Vector3.zero;

        //맵의 2차원 배열 주소를 저장
        startMap.Row = randRow;
        startMap.Col = randCol;

        CheckCanMove(startMap);
    }

    //맵에서 이동할 수 있는 방향을 체크
    void CheckCanMove(Map map)
    {
        //생성 가능한 맵 리스트가 없을 때 중단
        if (mapList.Count == 0)
            return;

        List<GameObject> canMovePortalList = new List<GameObject>();
        List<transMap> nextMapList = new List<transMap>();

        //isUp
        if (map.Row == 0)
        {
            map.isUp = false;
        }
        else if (maps[map.Row - 1, map.Col].map == null)
        {
            map.isUp = true;
            canMovePortalList.Add(map.upPortal);
            nextMapList.Add(maps[map.Row - 1, map.Col]);
        }
        else if (maps[map.Row - 1, map.Col].map.isDown == false)
        {
            map.isUp = false;
        }
        else
        {
            PortalCreate(map.upPortal);
        }

        //isDown
        if (map.Row == maps.GetLength(0) - 1)
        {
            map.isDown = false;
        }
        else if (maps[map.Row + 1, map.Col].map == null)
        {
            map.isDown = true;
            canMovePortalList.Add(map.downPortal);
            nextMapList.Add(maps[map.Row + 1, map.Col]);
        }
        else if (maps[map.Row + 1, map.Col].map.isUp == false)
        {
            map.isDown = false;
        }
        else
        {
            PortalCreate(map.downPortal);
        }

        //isLeft
        if (map.Col == 0)
        {
            map.isLeft = false;
        }
        else if (maps[map.Row, map.Col - 1].map == null)
        {
            map.isLeft = true;
            canMovePortalList.Add(map.leftPortal);
            nextMapList.Add(maps[map.Row, map.Col - 1]);
        }
        else if (maps[map.Row, map.Col - 1].map.isRight == false)
        {
            map.isLeft = false;
        }
        else
        {
            PortalCreate(map.leftPortal);
        }

        //isRight
        if (map.Col == maps.GetLength(1) - 1)
        {
            map.isRight = false;
        }
        else if (maps[map.Row, map.Col + 1].map == null)
        {
            map.isRight = true;
            canMovePortalList.Add(map.rightPortal);
            nextMapList.Add(maps[map.Row, map.Col + 1]);
        }
        else if (maps[map.Row, map.Col + 1].map.isLeft == false)
        {
            map.isRight = false;
        }
        else
        {
            PortalCreate(map.rightPortal);
        }

        //이동 가능 방향 중 랜덤으로 포탈 만들기
        int randMaxCreateCount = Random.Range(1, canMovePortalList.Count);

        for (int i = 0; i < randMaxCreateCount; i++)
        {
            //랜덤한 방향에 포탈 설치
            int randPortal = Random.Range(0, canMovePortalList.Count);
            PortalCreate(canMovePortalList[randPortal]);
            canMovePortalList.RemoveAt(randPortal);

            //mapList의 맵 중 랜덤으로 생성
            int rand = Random.Range(0, mapList.Count);
            Map nextMap = Instantiate(mapList[rand]);

            //생선한 맵이 값 설정
            nextMap.transform.SetParent(nextMapList[randPortal].transform);
            nextMap.transform.localPosition = Vector3.zero;
            nextMapList[randPortal].map = nextMap;
            nextMapList.RemoveAt(randPortal);
        }

    }

    void PortalCreate(GameObject portal)
    {
        portal.gameObject.SetActive(true);
    }
}
