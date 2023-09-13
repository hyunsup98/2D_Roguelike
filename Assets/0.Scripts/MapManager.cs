using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] Map laspMap;                           //���������� �� �� �ִ� ������ ���� ���� ����
    [SerializeField] List<Map> mapList = new List<Map>();   //�� �������� ���� �迭
    [SerializeField] TransMap transMap;                     //���� ���� ��ġ ������

    List<Map> nextMap = new List<Map>();    //���� üũ�� ���� ���� �ִ� ����Ʈ
    public TransMap[,] maps;                //�� �������� �� �迭

    //2���� �迭�� [x, y]��
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

    //ó�� ������ �� Ÿ�԰� ��ġ�� �����ֱ�
    void SetStartMap()
    {
        //�� ���� �� ����Ʈ���� �����ϱ�
        int randMap = Random.Range(0, mapList.Count);
        Map startMap = Instantiate(mapList[randMap]);
        mapList.RemoveAt(randMap);

        //��ü ���� ���� Ȥ�� ���ΰ� 3ĭ �̻��� ��� ��ŸƮ���� ���� �������� �����ϱ�
        int randRow = x > 2 ? Random.Range(1, maps.GetLength(0) - 1) : Random.Range(0, maps.GetLength(0));
        int randCol = y > 2 ? Random.Range(1, maps.GetLength(1) - 1) : Random.Range(0, maps.GetLength(1));
        TransMap startPos = maps[randRow, randCol];

        //���� ���� ������ ��ġ�� ���
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

    //�ʿ��� �̵��� �� �ִ� ������ üũ
    void CheckCanMove(Map map, int row, int col)
    {
        //���ǿ� �����ϴ� Ư�� ���� ��Ż���� �־�δ� ����Ʈ
        List<GameObject> canMovePortalList = new List<GameObject>();
        List<Map> spareMap = new List<Map>();

        //����1. Ư�� ���⿡ ��Ż�� �ִ���
        //����2. �� ������ ���� �ִ°� �ƴ���
        //����3. ���� ������ �¹����� ���� �����ִ��� (up - down / left - right)
        //�� ���ǵ��� üũ�� �ϰ� �ش�Ǹ� �ٷ� canMovePortalList�� �߰����� �� �������� ������ �ڿ��� �Ѵ�

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

        //���ǿ� �����ϴ� ��Ż�� ���� ��� ����
        if (canMovePortalList.Count != 0)
        {
            //��Ż�� �� �� �������� ���� ���� ����
            int randPortal = Random.Range(1, canMovePortalList.Count + 1);

            for (int i = 0; i < randPortal; i++)
            {
                //������ ���� ��ǥ
                int x = row;
                int y = col;

                //� �������� ���� ������ ���� ���� ����
                int rand = Random.Range(0, canMovePortalList.Count);

                PortalCreate(canMovePortalList[rand]);

                Map m = Instantiate(spareMap[rand]);

                //� ������ ��Ż������ ���� ��ǥ���� ����
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

    //���ǿ� ������ mapList���� �����ξ����� ������� ���� �ʵ��� �ٽ� mapList�� �ֱ�
    void AddMapList(List<Map> map)
    {
        foreach (var item in map)
        {
            mapList.Add(item);
        }
    }

    //��Ż ���� ���ǿ� ������ ����Ʈ�� �־����� ������ ���� ������ false�� ����
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

    //Ư�� ���� ��Ż �����ֱ�
    void PortalCreate(GameObject portal)
    {
        portal.gameObject.SetActive(true);
    }
}




