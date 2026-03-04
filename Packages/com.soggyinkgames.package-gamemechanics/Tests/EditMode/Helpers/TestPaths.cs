using UnityEngine;

public static class TestPaths
{
    public static Vector3[] Line()
    {
        return new[]
        {
            new Vector3(0,0,0),
            new Vector3(0.5f,0,0),
            new Vector3(1f,0,0)
        };
    }

    public static Vector3[] SlightlyOffsetLine()
    {
        return new[]
        {
            new Vector3(0,0.02f,0),
            new Vector3(0.5f,0.01f,0),
            new Vector3(1f,-0.02f,0)
        };
    }

    public static Vector3[] VShape()
    {
        return new[]
        {
            new Vector3(0,0,0),
            new Vector3(0.5f,1f,0),
            new Vector3(1f,0,0)
        };
    }
}