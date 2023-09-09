using System;
using System.Collections.Generic;

[Serializable]
public struct TransformStruct
{
    public string name;
    public jsonVector3 localPosition;
    public jsonQuaternion localRotation;
    public jsonVector3 localScale;
    public List<TransformStruct> Children;
}

[Serializable]
public struct jsonVector3
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
public struct jsonQuaternion
{
    public float x;
    public float y;
    public float z;
    public float w;
}

[Serializable]
public class Template
{
    public List<TransformStruct> objectHierarchy;
}
