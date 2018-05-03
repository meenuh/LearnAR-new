using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateCurrentManager : MonoBehaviour
{
    [SerializeField]
    GameObject trailObj;
    [SerializeField]
    GameObject planeObj;
    [SerializeField]
    List<Transform> PointTransforms;
    [SerializeField]
    [Range(0f, 1f)]
    float Period = 0.5f;
    [SerializeField] private GameObject point;


    void SetLocalPoints(IEnumerable<Vector3> local_points)
    {
        int node_number = 1;
        foreach (var local_point in local_points)
        {
            var node = new GameObject("node" + node_number);
            var node_trasform = node.transform;
            node_trasform.SetParent(planeObj.transform);
            node_trasform.localPosition = local_point;
            node_number++;
        }
    }

    //IEnumerator Start()
    //{

    //    Hashtable opts = new Hashtable();
    //    for (; ; )
    //    {
    //        foreach (var pt in PointTransforms)
    //        {
    //            opts.Add("easetype", iTween.EaseType.linear);
    //            var local_pos = pt.localPosition;
    //            var world_pos = planeObj.transform.TransformPoint(local_pos);
    //            opts.Add("x", world_pos.x);
    //            opts.Add("y", world_pos.y);
    //            opts.Add("z", world_pos.z);
    //            opts.Add("time", 0.5f);
    //            //iTween.MoveTo(trailObj, world_pos, Period);
    //            iTween.MoveTo(trailObj, opts);
    //            opts.Clear();
    //            yield return new WaitForSeconds(Period);
    //        }
    //    }
    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    //public void ShowCurrentFlow(float xmin, float xmax, float ymin, float ymax)
    //{
    //    float scale = 0.01f;
    //    float width_canv = 667f;
    //    float height_canv = 504f; // w & h of image is likely smaller

    //    float x_orig = width_canv / 2;
    //    float y_orig = height_canv / 2;

    //    float xmin_conv = (x_orig - xmin) * -1.0f;
    //    float xmax_conv = (x_orig - xmax) * -1.0f;
    //    float ymin_conv = (y_orig - ymin);
    //    float ymax_conv = (y_orig - ymax);

    //    Vector3 topLeft = new Vector3(xmin_conv, ymin_conv, -10);
    //    Vector3 topRight = new Vector3(xmax_conv, ymin_conv, -10);
    //    Vector3 bottomRight = new Vector3(xmax_conv, ymax_conv, -10);
    //    Vector3 bottomLeft = new Vector3(xmin_conv, ymax_conv, -10);
    //    Vector3 back = new Vector3(xmin_conv, ymin_conv, -10);

    //    Vector3[] path = { topLeft, topRight, bottomRight, bottomLeft, back };

    //    Hashtable opts = new Hashtable();
    //    for (; ; )
    //    {
    //        foreach(var pt in path)
    //        {
    //            opts.Add("easetype", iTween.EaseType.linear);
    //            opts.Add("x", pt.x);
    //            opts.Add("y", pt.y);
    //            opts.Add("z", pt.z);
    //            opts.Add("time", 0.5f);
    //            //iTween.MoveTo(trailObj, world_pos, Period);
    //            iTween.MoveTo(trailObj, opts);
    //            opts.Clear();
    //            //yield return new WaitForSeconds(Period);
    //        }
    //    }

    //}

    // should be called by display manager when it receives positions from backend
    public void ShowCurrentFlow(float xmin, float xmax, float ymin, float ymax)
    {
        //calculate positions xmin & ymax should be converted
        float scale = 0.01f;
        float width_canv = 667f;
        float height_canv = 504f; // w & h of image is likely smaller

        float x_orig = width_canv / 2;
        float y_orig = height_canv / 2;

        float xmin_conv = (x_orig - xmin) * -1.0f;
        float xmax_conv = (x_orig - xmax) * -1.0f;
        float ymin_conv = (y_orig - ymin);
        float ymax_conv = (y_orig - ymax);

        Vector3 topLeft = new Vector3(xmin_conv, ymin_conv, -10);
        Vector3 topRight = new Vector3(xmax_conv, ymin_conv, -10);
        Vector3 bottomRight = new Vector3(xmax_conv, ymax_conv, -10);
        Vector3 bottomLeft = new Vector3(xmin_conv, ymax_conv, -10);
        Vector3 back = new Vector3(xmin_conv, ymin_conv, -10);

        //Vector3 topLeft = new Vector3(-333.5f, 252, -10);
        //Vector3 topRight = new Vector3(333.5f, 252, -10);
        //Vector3 bottomRight = new Vector3(333.5f, -252, -10);
        //Vector3 bottomLeft = new Vector3(-333.5f, -252, -10);
        //Vector3 back = new Vector3(-333.5f, 252, -10);

        iTween.MoveTo(trailObj, iTween.Hash("path", new Vector3[] { topLeft, topRight, bottomRight, bottomLeft, back },
            "time", 10, "looptype", "loop", "isLocal", true, "orientToPath", true, "easeType", iTween.EaseType.linear));
    }
}