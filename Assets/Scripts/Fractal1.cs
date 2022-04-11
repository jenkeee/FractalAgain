using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal1 : MonoBehaviour
{
   // public GameObject prefab;


    [SerializeField, Range(1, 8)] private int _depth = 4;
    [SerializeField, Range(1, 360)] private int _rotationSpeed;

    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;

    private static readonly Vector3[] _directions = new Vector3[]
{
        Vector3.up,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back,
};
    private static readonly Quaternion[] _rotations = new Quaternion[]
{
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f),
};

    int x = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameObject _mainObj = new GameObject("Main 0");
        _mainObj.transform.SetParent(transform, false);
        _mainObj.AddComponent<MeshFilter>().mesh = mesh;
        _mainObj.AddComponent<MeshRenderer>().material = material;






        
       
        for (int currentDepth = 0; currentDepth < _depth; currentDepth++)
        {
            x += Mathf.RoundToInt(Mathf.Pow(5, currentDepth));
        }

        for (int li = 0; li < x - Mathf.Pow(5, _depth - 1); li++)
        {
            CreateChild(transform.GetChild(li).gameObject);
        }

        /*
        Debug.Log($"объектов должно быть: " + x);
        Debug.Log($"детей есть: " + transform.childCount);*/
    }

    // Update is called once per frame
    int dir = 0;
    void Update()
    {


        for (int i = 1; i < transform.childCount; i++)
        {
            if (dir >= 5)
                dir = 0;
            transform.GetChild(i).localPosition = transform.GetChild(Mathf.RoundToInt((i-1)/5)).localPosition + transform.GetChild(Mathf.RoundToInt((i - 1) / 5)).TransformDirection(_directions[dir] * transform.GetChild(Mathf.RoundToInt((i - 1) / 5)).transform.localScale.x);

            dir++;
        }

        for (int i = 0; i < transform.childCount; i++)
        {

            transform.GetChild(i).localRotation *= Quaternion.Euler(0, _rotationSpeed * Time.deltaTime,0);
          //  transform.GetChild(i).localRotation = transform.GetChild(i).localRotation * transform.GetChild(Mathf.RoundToInt((i - 1) / 5)).localRotation * (Quaternion.Euler(0, _rotationSpeed * Time.deltaTime, 0));
        }   
    }



    private void CreateChild(GameObject obgForParent)
    {
        for (int i = 0; i < 5; i++)
        {
            // var go = new GameObject($"Fractal Path L{obgForParent.gameObject.transform.localScale} C{i}");
            var go = new GameObject($"From {obgForParent.name} || C{i}");
            go.transform.SetParent(transform, false);
            go.AddComponent<MeshFilter>().mesh = mesh;
            go.AddComponent<MeshRenderer>().material = material;
            go.transform.localScale = obgForParent.transform.localScale * 0.5f;
            go.transform.localRotation = obgForParent.transform.rotation * _rotations[i];
            go.transform.localPosition += obgForParent.transform.localPosition + obgForParent.transform.TransformDirection(_directions[i]) * obgForParent.transform.localScale.x;
        }

    }
    
}
