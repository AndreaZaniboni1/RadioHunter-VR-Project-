using MimicSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSeesEnemy : MonoBehaviour
{
    public List<GameObject> targets;
    public Camera cam;

    private void Start()
    {
        cam=GetComponent<Camera>();
    }

    private bool IsVisible(Camera c, GameObject target)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var point = target.transform.position;

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        return true;
    }

    public void ImDead(int index)
    {
        targets.RemoveAt(index);
    }

    private void Update()
    {
        if (targets.Count > 0)
        {
            foreach (var target in targets)
            {
                var targetRender = target.GetComponent<Renderer>();

                if (IsVisible(cam, target))
                {
                    target.GetComponent<MovementModified>().IsTrulySeen();
                }
                else
                {
                    target.GetComponent<MovementModified>().isSeen = false;
                }

            }

        }

    }
}
