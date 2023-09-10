using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using Image = UnityEngine.UI.Image;

public class ObjectTemplateEditor : EditorWindow
{
    private readonly string jsonFilePath = "Assets/Data/JSON/object.json";

    private string jsonContent = "";
    private Vector2 scrollPosition;
    private bool loadJson = true;

    // Storage for all the available templates. 
    private List<ObjectTemplate> templates = new List<ObjectTemplate>();

    // Temporary object to hold the new templates.
    private ObjectTemplate newTemplate = new ObjectTemplate();

    

    [MenuItem("GG/Object Template Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObjectTemplateEditor>("Object Template Editor");
    }



    private void OnGUI()
    {
        // Check if we need to load the json.
        // to prevent serialization at every loop.
        // will be loaded again only when a new template is stored. 
        if (loadJson)
        {
            LoadJSON();
            loadJson = false;
        }

        EditorGUILayout.LabelField("Object Template Editor", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        // Scroll area
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        jsonContent = EditorGUILayout.TextArea(jsonContent, GUILayout.ExpandHeight(true));
        if (GUILayout.Button("Update JSON"))
        {
            UpdateJsonFromEditor(jsonContent);
        }

        EditorGUILayout.Space();

        ShowAvailableTemplates();

        EditorGUILayout.EndScrollView();

        // Scroll area ends. 

        EditorGUILayout.Space();

        CreateNewObjectTemplate();
    }

    /// <summary>
    /// Method to Create new templates here. 
    /// </summary>
    private void CreateNewObjectTemplate()
    {
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

            // to refresh the json. 
            loadJson = true;
        }
    }

    /// <summary>
    /// Method to show the available templates. 
    /// </summary>
    private void ShowAvailableTemplates()
    {
        // Display available templates on the scroll area and make it customizable. 
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
    }

    /// <summary>
    /// Save the template data to the JSON (Deserialize).
    /// </summary>
    private void SaveJSON()
    {
        try
        {
            var jsonString = JsonUtility.ToJson(new TemplateContainer { templates = templates }, true);
            File.WriteAllText(jsonFilePath, jsonString);
            Debug.Log("JSON data saved successfully.");
            newTemplate = new ObjectTemplate(newTemplate.name);
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving JSON data: " + e.Message);
        }
    }

    /// <summary>
    /// Get the template data from the JSON (Serialize).
    /// </summary>
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

                // Initialize an empty list if deserialization fails
                templates = new List<ObjectTemplate>(); 
            }
        }
        else
        {
            Debug.LogWarning("JSON file not found at path: " + jsonFilePath);
            jsonContent = "";
        }
    }

    /// <summary>
    /// Update the Json file when there is any update in the editor text area.
    /// </summary>
    /// <param name="jsonContent"> json content from the editor text area.</param>
    private void UpdateJsonFromEditor(string jsonContent)
    {
        // Update our local class members.
        var container = JsonUtility.FromJson<TemplateContainer>(jsonContent);
        if (container != null)
        {
            templates = container.templates;
        }
        else
        {
            templates = new List<ObjectTemplate>();
        }

        // Write it to the files now.
        SaveJSON();
    }

    /// <summary>
    /// Initiate a gameObject with the respective template. 
    /// </summary>
    /// <param name="template">Template details.</param>
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

        GameObject canvasObject = new GameObject("Canvas" + template.name);
        canvasObject.AddComponent<Canvas>();

        uiObject.transform.parent = canvasObject.transform;
    }

    

}
