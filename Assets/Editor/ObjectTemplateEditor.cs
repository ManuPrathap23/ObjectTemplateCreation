using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ObjectTemplateEditor : EditorWindow
{
    private readonly string jsonFilePath = "Assets/Data/JSON/object.json"; 
    private string jsonContent = "";
    private Vector2 scrollPosition;

    private List<ObjectTemplate> templates = new List<ObjectTemplate>();
    private ObjectTemplate newTemplate = new ObjectTemplate();

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

    [MenuItem("GG/Object Template Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObjectTemplateEditor>("Object Template Editor");
    }

    private void OnEnable()
    {
        LoadJSON();
    }

    private void OnGUI()
    {
        LoadJSON();

        EditorGUILayout.LabelField("Object Template Editor", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        jsonContent = EditorGUILayout.TextArea(jsonContent, GUILayout.ExpandHeight(true));

        EditorGUILayout.Space();

        // Display existing templates and customization options
        foreach (var template in templates)
        {
            EditorGUILayout.LabelField("Template: " + template.name);

            // Customizable properties
            template.position = EditorGUILayout.Vector3Field("Position", template.position);
            template.rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation (Euler Angles)", template.rotation.eulerAngles));
            template.scale = EditorGUILayout.Vector3Field("Scale", template.scale);
            template.color = EditorGUILayout.ColorField("Color", template.color);

            // Instantiate the template with updated properties
            if (GUILayout.Button("Create"))
            {
                InstantiateTemplate(template);
            }

            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        // Create a new template
        EditorGUILayout.LabelField("Create New Template", EditorStyles.boldLabel);
        newTemplate.name = EditorGUILayout.TextField("Name", newTemplate.name);
        newTemplate.position = EditorGUILayout.Vector3Field("Position", newTemplate.position);
        newTemplate.rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation (Euler Angles)", newTemplate.rotation.eulerAngles));
        newTemplate.scale = EditorGUILayout.Vector3Field("Scale", newTemplate.scale);
        newTemplate.color = EditorGUILayout.ColorField("Color", newTemplate.color);

        if (GUILayout.Button("Save JSON"))
        {
            templates.Add(newTemplate);
            SaveJSON();
        }
    }

    private void SaveJSON()
    {
        try
        {
            var jsonString = JsonUtility.ToJson(new TemplateContainer { templates = templates }, true);
            File.WriteAllText(jsonFilePath, jsonString);
            Debug.Log("JSON data saved successfully.");
            newTemplate = new ObjectTemplate(newTemplate.name); // Create a new template with the same name
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving JSON data: " + e.Message);
        }
    }

    private void LoadJSON()
    {
        if (File.Exists(jsonFilePath))
        {
            try
            {
                var jsonContent = File.ReadAllText(jsonFilePath);
                var container = JsonUtility.FromJson<TemplateContainer>(jsonContent);
                if (container != null)
                {
                    templates = container.templates;
                }
                else
                {
                    templates = new List<ObjectTemplate>();
                }
                this.jsonContent = jsonContent;
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading JSON data: " + e.Message);
                templates = new List<ObjectTemplate>(); // Initialize an empty list if deserialization fails
            }
        }
        else
        {
            Debug.LogWarning("JSON file not found at path: " + jsonFilePath);
            jsonContent = "";
        }
    }


    private void InstantiateTemplate(ObjectTemplate template)
    {
        // Create a new GameObject for the instantiated UI hierarchy
        GameObject uiObject = new GameObject(template.name);
        RectTransform rectTransform = uiObject.AddComponent<RectTransform>();
        CanvasRenderer canvasRenderer = uiObject.AddComponent<CanvasRenderer>();

        // Set the properties based on the template
        rectTransform.localPosition = template.position;
        rectTransform.localRotation = template.rotation;
        rectTransform.localScale = template.scale;

        // Create and configure UI components based on your template (e.g., Image, Text, etc.)
        Image image = uiObject.AddComponent<Image>();
        image.color = template.color;
    }

    [System.Serializable]
    private class TemplateContainer
    {
        public List<ObjectTemplate> templates;
    }

}
