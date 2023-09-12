using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //맵 프리팹을 넣을 배열
    [SerializeField] List<Map> mapList = new List<Map>();
    //맵을 담을 위치 프리팹
    [SerializeField] TransMap transMap;

    //맵 프리팹이 들어갈 배열
    public TransMap[,] maps;
    //2차원 배열의 [x, y]값
    public int x, y;

    private void Awake()
    {
        maps = new TransMap[x, y];

        int count = 0;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                TransMap tmap = Instantiate(transMap, transform);
                maps[i, j] = tmap;
                tmap.transform.position = new Vector3(j, -i);
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
        TransMap startPos = maps[randRow, randCol];

        //시작 맵을 지정한 위치에 담기
        startPos.GetComponent<TransMap>().map = startMap;
        startMap.transform.SetParent(startPos.transform);
        startMap.transform.localPosition = Vector3.zero;

        //맵의 2차원 배열 주소를 저장


        while (true)
        {
            if (maps[x, y].map.isDown)
            {
                if (maps[x, y - 1].map.isUp)
                {
                    y--;
                }
            }
        }
    }

    //맵에서 이동할 수 있는 방향을 체크
    void CheckCanMove(Map map, int row, int col)
    {
        //mapList의 남은 맵이 없는 경우 중단
        if (mapList.Count == 0)
            return;

        List<GameObject> canMovePortalList = new List<GameObject>();

        if (map.isUp && row != 0)
        {
            List<Map> isDownMap = new List<Map>();
            foreach (var item in mapList)
            {
                if (item.isDown)
                    isDownMap.Add(item);
            }

            if(isDownMap.Count != 0)
            {
                canMovePortalList.Add(map.upPortal);
            }
        }

        if (map.isDown && row != maps.GetLength(0) - 1)
        {
            canMovePortalList.Add(map.downPortal);
        }

        if (map.isLeft && col != 0)
        {
            canMovePortalList.Add(map.leftPortal);
        }

        if (map.isRight && col != maps.GetLength(1) - 1)
        {
            canMovePortalList.Add(map.rightPortal);
        }

        int randPortal = Random.Range(1, canMovePortalList.Count);



        //생성 가능한 맵 리스트가 없을 때 중단
        if (mapList.Count == 0)
            return;

        List<TransMap> nextMapList = new List<TransMap>();
        for (int x = -1; x < 1; x += 2)
        {
            for (int y = -1; y < 1; y += 2)
            {

            }
        }


        /*
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
        */

    }
    /* 줄이려고 시도한 것
    bool NextMapCheck()
    {
        List<Map> isWhereMap = new List<Map>();
        foreach (var item in mapList)
        {
            if ()
                isWhereMap.Add(item);
        }

        if (isWhereMap.Count != 0)
        {
            return true;
        }

        return false;
    }
    */

    //맵의 특정 방향 포탈 열어주기
    void PortalCreate(GameObject portal)
    {
        portal.gameObject.SetActive(true);
    }
}
