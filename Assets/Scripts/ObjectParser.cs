using UnityEngine;
using System.IO;

public class ObjectParser : MonoBehaviour
{
    private Template objectTemplate;
    public string fileName = "Assets/Data/JSON/object.json";

    void Start()
    {
        string jsonString = File.ReadAllText(fileName);
        objectTemplate = CreateFromJSON(jsonString);
        CreateObjectStructure();
    }

    public static Template CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Template>(jsonString);
    }


    private void CreateObjectStructure()
    {
        foreach (var template in objectTemplate.objectHierarchy)
        {
            CreateGameObject(template);
        }
    }

    private GameObject CreateGameObject(TransformStruct transform)
    {
        GameObject parent = new GameObject(transform.name);
        SetTransformation(parent, transform);

        foreach (var childTransform in transform.Children)
        {
            var childGO = CreateGameObject(childTransform);
            childGO.transform.parent = parent.transform;

        }
        return parent;
    }


    private void SetTransformation(GameObject gameObject, TransformStruct transform)
    {
        gameObject.transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        gameObject.transform.rotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
        gameObject.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

}

