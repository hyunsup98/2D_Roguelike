using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //�� �������� ���� �迭
    [SerializeField] List<Map> mapList = new List<Map>();
    //�� �������� �� �迭
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


    //ó�� ������ �� Ÿ�԰� ��ġ�� �����ֱ�
    void SetStartMap()
    {

        //�� ���� �� ����Ʈ���� �����ϱ�
        int randMap = Random.Range(0, mapList.Count);
        Map startMap = Instantiate(mapList[randMap]);
        mapList.RemoveAt(randMap);

        //������ ���� ���� ���� ���� ��ġ �����ϱ�
        int randRow = Random.Range(0, maps.GetLength(0));
        int randCol = Random.Range(0, maps.GetLength(1));
        transMap startPos = maps[randRow, randCol];

        //���� ���� ������ ��ġ�� ���
        startPos.GetComponent<transMap>().map = startMap;
        startMap.transform.SetParent(startPos.transform);
        startMap.transform.localPosition = Vector3.zero;

        //���� 2���� �迭 �ּҸ� ����
        startMap.Row = randRow;
        startMap.Col = randCol;

        CheckCanMove(startMap);
    }

    //�ʿ��� �̵��� �� �ִ� ������ üũ
    void CheckCanMove(Map map)
    {
        //���� ������ �� ����Ʈ�� ���� �� �ߴ�
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

        //�̵� ���� ���� �� �������� ��Ż �����
        int randMaxCreateCount = Random.Range(1, canMovePortalList.Count);

        for (int i = 0; i < randMaxCreateCount; i++)
        {
            //������ ���⿡ ��Ż ��ġ
            int randPortal = Random.Range(0, canMovePortalList.Count);
            PortalCreate(canMovePortalList[randPortal]);
            canMovePortalList.RemoveAt(randPortal);

            //mapList�� �� �� �������� ����
            int rand = Random.Range(0, mapList.Count);
            Map nextMap = Instantiate(mapList[rand]);

            //������ ���� �� ����
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
