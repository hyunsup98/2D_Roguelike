using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //�� �������� ���� �迭
    [SerializeField] List<Map> mapList = new List<Map>();
    //���� ���� ��ġ ������
    [SerializeField] TransMap transMap;

    //�� �������� �� �迭
    public TransMap[,] maps;
    //2���� �迭�� [x, y]��
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
        TransMap startPos = maps[randRow, randCol];

        //���� ���� ������ ��ġ�� ���
        startPos.GetComponent<TransMap>().map = startMap;
        startMap.transform.SetParent(startPos.transform);
        startMap.transform.localPosition = Vector3.zero;

        //���� 2���� �迭 �ּҸ� ����


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

    //�ʿ��� �̵��� �� �ִ� ������ üũ
    void CheckCanMove(Map map, int row, int col)
    {
        //mapList�� ���� ���� ���� ��� �ߴ�
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



        //���� ������ �� ����Ʈ�� ���� �� �ߴ�
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
        */

    }
    /* ���̷��� �õ��� ��
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

    //���� Ư�� ���� ��Ż �����ֱ�
    void PortalCreate(GameObject portal)
    {
        portal.gameObject.SetActive(true);
    }
}
