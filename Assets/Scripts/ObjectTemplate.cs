using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic Object template
/// </summary>
[Serializable]
public class ObjectTemplate
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Color color;

    public ObjectTemplate(string templateName)
    {
        name = templateName;
        position = Vector3.zero;
        rotation = Quaternion.identity;
        scale = Vector3.one;
        color = Color.white;
    }
    public ObjectTemplate()
    {
        // Default constructor
    }
}


[System.Serializable]
public class TemplateContainer
{
    public List<ObjectTemplate> templates;
}