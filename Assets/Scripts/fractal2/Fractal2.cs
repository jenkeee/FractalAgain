using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class Fractal2 : MonoBehaviour
{
    // public GameObject prefab;


    [SerializeField, Range(1, 8)] private int _depth = 4;
    [SerializeField, Range(1, 360)] private int _rotationSpeed;

    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;

    public static readonly Vector3[] _directions = new Vector3[]
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


    //для джобсов
    private NativeArray<Vector3> positions;
    private NativeArray<Quaternion> _localRotation;
    private NativeArray<Vector3> newPosition;
    private NativeArray<float> sizes;
    private int numberOfEntities;
    private TransformAccessArray transformAccessArray;
   


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





        numberOfEntities = transform.childCount+1;

        positions = new NativeArray<Vector3>(numberOfEntities, Allocator.Persistent);
        _localRotation = new NativeArray<Quaternion>(numberOfEntities, Allocator.Persistent);
        newPosition = new NativeArray<Vector3>(numberOfEntities, Allocator.Persistent);
        sizes = new NativeArray<float>(numberOfEntities, Allocator.Persistent);

        Transform[] transforms = new Transform[numberOfEntities];

        for (int i = 0; i < numberOfEntities-1; i++)
        {
            positions[i] = transform.GetChild(i).localPosition;
            transforms[i] = transform.GetChild(i).transform;
            sizes[i] = transform.GetChild(i).localScale.x;
        }
            transformAccessArray = new TransformAccessArray(transforms);
         /*      
        Debug.Log($"объектов должно быть: " + x);
        Debug.Log($"детей есть: " + transform.childCount);
        Debug.Log($"в [] transforms объектов " + transforms.Length + "||numberOfEntities" + numberOfEntities);*/

    }

    // Update is called once per frame
    int dir = 0;
    void Update()
    {
        
        /*
        for (int i = 1, li = 0 ; i < transform.childCount ; i++, li++)
        {
            if (li >= 5)
                li = 0;            
                transform.GetChild(i).localPosition = transform.GetChild(Mathf.RoundToInt((i - 1) / 5)).localPosition + transform.GetChild(Mathf.RoundToInt((i - 1) / 5)).TransformDirection(_directions[li] * transform.GetChild(Mathf.RoundToInt((i - 1) / 5)).transform.localScale.x);
        }
        */
        PositionJob positionJob = new PositionJob()
        {
            Positions = positions,
            Sizes = sizes,
            NewPosition = newPosition,
        };

        JobHandle positionHandle = positionJob.Schedule(numberOfEntities, 0);

        RotationJob rotationJob = new RotationJob()
        {
            Positions = positions,  
            NewPosition = newPosition,
            RotationSpeed = _rotationSpeed,
            LocalRotation = _localRotation,
            Sizes = sizes,
        DeltaTime = Time.deltaTime
        };
        JobHandle rotationHandle = rotationJob.Schedule(transformAccessArray, positionHandle);
        rotationHandle.Complete();
       









        for (int i = 0; i < transform.childCount; i++)
        {

            //  transform.GetChild(i).localRotation *= Quaternion.Euler(0, _rotationSpeed * Time.deltaTime,0);
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
    private void OnDestroy()
    {
        positions.Dispose();
        _localRotation.Dispose();
        newPosition.Dispose();
        sizes.Dispose();

        transformAccessArray.Dispose();
    }

}
