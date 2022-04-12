using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


public struct PositionJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<Vector3> Positions;
    [ReadOnly]
    public NativeArray<float> Sizes;

    public NativeArray<Vector3> NewPosition;
    public void Execute(int index)
    {
        for (int i = 0; i < 5; i++)
        {
                      NewPosition[index] = Positions[index] + Fractal2._directions[i];

        }
    }
   
}
    