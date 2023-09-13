using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] Map laspMap;                           //보스맵으로 갈 수 있는 마지막 방은 따로 관리
    [SerializeField] List<Map> mapList = new List<Map>();   //맵 프리팹을 넣을 배열
    [SerializeField] TransMap transMap;                     //맵을 담을 위치 프리팹

    List<Map> nextMap = new List<Map>();    //조건 체크할 다음 맵을 넣는 리스트
    public TransMap[,] maps;                //맵 프리팹이 들어갈 배열

    //2차원 배열의 [x, y]값
    public int x, y;

    int a = 0;

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

        //전체 맵의 가로 혹은 세로가 3칸 이상일 경우 스타트맵은 안쪽 공간에서 시작하기
        int randRow = x > 2 ? Random.Range(1, maps.GetLength(0) - 1) : Random.Range(0, maps.GetLength(0));
        int randCol = y > 2 ? Random.Range(1, maps.GetLength(1) - 1) : Random.Range(0, maps.GetLength(1));
        TransMap startPos = maps[randRow, randCol];

        //시작 맵을 지정한 위치에 담기
        startPos.GetComponent<TransMap>().map = startMap;
        startMap.transform.SetParent(startPos.transform);
        startMap.transform.localPosition = Vector3.zero;

        CheckCanMove(startMap, randRow, randCol);

    }

    void CheckDirPortal(Map map, Map nextMap, ref bool rotate, ref List<Map> spareMap, ref List<GameObject> canMovePortalList)
    {
        if (nextMap == null)
        {
            List<Map> mm = new List<Map>();
            foreach (var item in mapList)
            {
                if (item.isDown)
                    mm.Add(item);
            }
            if (mm.Count > 0)
            {
                int rand = Random.Range(0, mm.Count);
                spareMap.Add(mm[rand]);
                mapList.Remove(mm[rand]);

                canMovePortalList.Add(map.upPortal);
            }
            else
                rotate = false;
        }
        else
        {
            if (nextMap.isDown)
            {
                rotate = true;
                map.upPortal.gameObject.SetActive(true);
            }
            else
                rotate = false;
        }
    }

    //맵에서 이동할 수 있는 방향을 체크
    void CheckCanMove(Map map, int row, int col)
    {
        //조건에 충족하는 특정 방향 포탈들을 넣어두는 리스트
        List<GameObject> canMovePortalList = new List<GameObject>();
        List<Map> spareMap = new List<Map>();

        //조건1. 특정 방향에 포탈이 있는지
        //조건2. 그 방향의 끝에 있는게 아닌지
        //조건3. 서로 방향이 맞물리는 맵이 남아있는지 (up - down / left - right)
        //위 조건들을 체크만 하고 해당되면 바로 canMovePortalList에 추가해준 뒤 실질적인 동작은 뒤에서 한다

        //isUp
        if (map.isUp && row != 0)
        {
            if (maps[row - 1, col].map == null)
            {
                List<Map> mm = new List<Map>();
                foreach (var item in mapList)
                {
                    if (item.isDown)
                        mm.Add(item);
                }
                if (mm.Count > 0)
                {
                    int rand = Random.Range(0, mm.Count);
                    spareMap.Add(mm[rand]);
                    mapList.Remove(mm[rand]);

                    canMovePortalList.Add(map.upPortal);
                }
                else
                    map.isUp = false;
            }
            else
            {
                if (maps[row - 1, col].map.isDown)
                {
                    map.isUp = true;
                    map.upPortal.gameObject.SetActive(true);
                }
                else
                    map.isUp = false;
            }
        }
        else
            map.isUp = false;

        //isDown
        if (map.isDown && row != maps.GetLength(0) - 1)
        {
            if (maps[row + 1, col].map == null)
            {
                List<Map> mm = new List<Map>();
                foreach (var item in mapList)
                {
                    if (item.isUp)
                        mm.Add(item);
                }
                if (mm.Count > 0)
                {
                    int rand = Random.Range(0, mm.Count);
                    spareMap.Add(mm[rand]);
                    mapList.Remove(mm[rand]);

                    canMovePortalList.Add(map.downPortal);
                }
                else
                    map.isDown = false;
            }
            else
            {
                if (maps[row + 1, col].map.isUp)
                {
                    map.isDown = true;
                    map.downPortal.gameObject.SetActive(true);
                }
                else
                    map.isDown = false;
            }

        }
        else
            map.isDown = false;

        //isLeft
        if (map.isLeft && col != 0)
        {
            if (maps[row, col - 1].map == null)
            {
                List<Map> mm = new List<Map>();
                foreach (var item in mapList)
                {
                    if (item.isRight)
                        mm.Add(item);
                }
                if (mm.Count > 0)
                {
                    int rand = Random.Range(0, mm.Count);
                    spareMap.Add(mm[rand]);
                    mapList.Remove(mm[rand]);

                    canMovePortalList.Add(map.leftPortal);
                }
                else
                    map.isLeft = false;
            }
            else
            {
                if (maps[row, col - 1].map.isRight)
                {
                    map.isLeft = true;
                    map.leftPortal.gameObject.SetActive(true);
                }
                else
                    map.isLeft = false;
            }
        }
        else
            map.isLeft = false;

        //isRight
        if (map.isRight && col != maps.GetLength(1) - 1)
        {
            if (maps[row, col + 1].map == null)
            {
                List<Map> mm = new List<Map>();
                foreach (var item in mapList)
                {
                    if (item.isLeft)
                        mm.Add(item);
                }
                if (mm.Count > 0)
                {
                    int rand = Random.Range(0, mm.Count);
                    spareMap.Add(mm[rand]);
                    mapList.Remove(mm[rand]);

                    canMovePortalList.Add(map.rightPortal);
                }
                else
                    map.isRight = false;
            }
            else
            {
                if (maps[row, col + 1].map.isLeft)
                {
                    map.isRight = true;
                    map.rightPortal.gameObject.SetActive(true);
                }
                else
                    map.isRight = false;
            }
        }
        else
            map.isRight = false;

        //조건에 충족하는 포탈이 있을 경우 실행
        if (canMovePortalList.Count != 0)
        {
            //포탈을 몇 개 만들지를 정할 랜덤 난수
            int randPortal = Random.Range(1, canMovePortalList.Count + 1);

            for (int i = 0; i < randPortal; i++)
            {
                //생성할 맵의 좌표
                int x = row;
                int y = col;

                //어떤 방향으로 길을 뚫을지 정할 랜덤 난수
                int rand = Random.Range(0, canMovePortalList.Count);

                PortalCreate(canMovePortalList[rand]);

                Map m = Instantiate(spareMap[rand]);

                //어떤 방향의 포탈인지에 따라 좌표값을 변경
                switch (canMovePortalList[rand].name)
                {
                    case "upPortal":
                        x--;
                        break;
                    case "downPortal":
                        x++;
                        break;
                    case "leftPortal":
                        y--;
                        break;
                    case "rightPortal":
                        y++;
                        break;
                }

                m.transform.SetParent(maps[x, y].transform);
                m.transform.localPosition = Vector3.zero;
                maps[x, y].map = m;

                nextMap.Add(m);

                canMovePortalList.RemoveAt(rand);
                spareMap.RemoveAt(rand);
            }

            OffBool(map, canMovePortalList);
            AddMapList(spareMap);
        }

        while (nextMap.Count > a)
        {
            CheckCanMove(nextMap[a], (int)nextMap[a].transform.parent.position.y * -1, (int)nextMap[a++].transform.parent.position.x);
        }
    }

    void CreateNextMap()
    {

    }

    //조건에 만족해 mapList에서 꺼내두었지만 사용하지 않은 맵들을 다시 mapList에 넣기
    void AddMapList(List<Map> map)
    {
        foreach (var item in map)
        {
            mapList.Add(item);
        }
    }

    //포탈 생성 조건에 만족해 리스트에 넣었지만 쓰이지 않은 방향을 false로 만듬
    void OffBool(Map map, List<GameObject> list)
    {
        foreach (var item in list)
        {
            switch (item.name)
            {
                case "upPortal":
                    map.isUp = false;
                    break;
                case "downPortal":
                    map.isDown = false;
                    break;
                case "leftPortal":
                    map.isLeft = false;
                    break;
                case "rightPortal":
                    map.isRight = false;
                    break;
            }
        }
    }

    //특정 방향 포탈 열어주기
    void PortalCreate(GameObject portal)
    {
        portal.gameObject.SetActive(true);
    }
}




