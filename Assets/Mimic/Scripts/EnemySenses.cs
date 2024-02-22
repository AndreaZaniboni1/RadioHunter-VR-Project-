using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemySenses : MonoBehaviour
{
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float viewRadius;
    public float viewAngle;
    public Transform player;

    private static readonly Vector4[] s_UnitSphere = MakeUnitSphere(16);
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Color colora = UnityEngine.Color.red;
        UnityEngine.Color colors = UnityEngine.Color.green;
        DrawSphere(transform.position, viewRadius, colora);
        Vector3 playerTarget = (player.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward,playerTarget) < viewAngle /3)
        {
            float distanceToTarget = Vector3.Distance(transform.position, player.position);
            if (distanceToTarget < viewRadius)
            {
                if(Physics.Raycast(transform.position, playerTarget, distanceToTarget, obstacleMask) == false)
                {
                    Debug.DrawLine(transform.position, player.position, colors);
                    Debug.Log("I see You");
                }
            }
        }
    }
    private static Vector4[] MakeUnitSphere(int len)
    {
        Debug.Assert(len > 2);
        var v = new Vector4[len * 3];
        for (int i = 0; i < len; i++)
        {
            var f = i / (float)len;
            float c = Mathf.Cos(f * (float)(Math.PI * 2.0));
            float s = Mathf.Sin(f * (float)(Math.PI * 2.0));
            v[0 * len + i] = new Vector4(c, s, 0, 1);
            v[1 * len + i] = new Vector4(0, c, s, 1);
            v[2 * len + i] = new Vector4(s, 0, c, 1);
        }
        return v;
    }
    public static void DrawSphere(Vector4 pos, float radius, UnityEngine.Color color)
    {
        Vector4[] v = s_UnitSphere;
        int len = s_UnitSphere.Length / 3;
        for (int i = 0; i < len; i++)
        {
            var sX = pos + radius * v[0 * len + i];
            var eX = pos + radius * v[0 * len + (i + 1) % len];
            var sY = pos + radius * v[1 * len + i];
            var eY = pos + radius * v[1 * len + (i + 1) % len];
            var sZ = pos + radius * v[2 * len + i];
            var eZ = pos + radius * v[2 * len + (i + 1) % len];
            Debug.DrawLine(sX, eX, color);
            Debug.DrawLine(sY, eY, color);
            Debug.DrawLine(sZ, eZ, color);
        }
    }
}
