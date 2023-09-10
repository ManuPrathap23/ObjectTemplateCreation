Object Template Creation

Description - The project basically helps you create an UI template for GameObjects. And later you can use these templates to instantiate a gameobject with that template in your scene. 

Available features: 

1. JSON Data: Have developed a JSON file format that contains data representing a UI object hierarchy. Each template includes properties such as the object's name, position, rotation, scale, color.
2. Unity Custom Editor: Created a custom Unity editor window that allows users to load, view, and edit the JSON data for object templates within the Unity Editor.
3. Template Creation: Implemented a feature that allows users to create new object templates directly within the custom editor. Users would be able to specify the template's properties and save them to the JSON file.
4. Template Instantiation: Developed functionality to instantiate the UI Hierarchy in a Unity scene. When users select a template from the custom editor, it should create a Canvas Hierarchy
with the specified properties in the current scene.
5.Error Handling: Includes error handling mechanisms to address issues such as missing or corrupted JSON files, and provide informative error messages.


How to use:

1.Once you load the scene, you can see the menu option GG on your menu bar. When you click it, you will see an option "Object Template Editor". 
2.You can see an option to create a new template and a button to save the data into your JSON. 
3. Once it is stored, you will the template above, where you will be given an option to create. On clicking create, a new GO with that properties will be instantiated on the scene. 
