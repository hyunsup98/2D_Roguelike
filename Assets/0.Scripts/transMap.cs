using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransMap : MonoBehaviour
{
    //�̹� �ش� ��ǥ�� ���� �Ҵ�Ǿ� �ִ��� Ȯ��
    public Map map;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
