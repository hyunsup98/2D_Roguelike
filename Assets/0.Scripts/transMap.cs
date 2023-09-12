using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransMap : MonoBehaviour
{
    //이미 해당 좌표에 맵이 할당되어 있는지 확인
    public Map map;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
