using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public struct RotationJob : IJobParallelForTransform
{
    public NativeArray<Vector3> Positions;
    [ReadOnly]
    public float DeltaTime;
    [ReadOnly]
    public NativeArray<float> Sizes;
    [ReadOnly]
    public float RotationSpeed;
    [ReadOnly]
    public NativeArray<Vector3> NewPosition;
    [ReadOnly]
    public NativeArray<Quaternion> LocalRotation;

    public void Execute(int index, TransformAccess transform)
    {

        transform.rotation *= Quaternion.Euler(0,RotationSpeed* DeltaTime,0);


        // transform.position = NewPosition[index];
        /*
                for (int i = 1, li = 0; i < Positions.Length; i++, li++)
                    {
                    if (i == index) continue;
                    if (li >= 5)
                            li = 0;
                    Positions[i] = Positions[(Mathf.RoundToInt((i - 1) / 5))] + Fractal2._directions[li] * Sizes[(Mathf.RoundToInt((i - 1) / 5))];
                    }
        */
        for (int i = 0; i < 5; i++)
        {
            transform.position = NewPosition[index] + transform.rotation* Fractal2._directions[i];
        }
       

    }
}