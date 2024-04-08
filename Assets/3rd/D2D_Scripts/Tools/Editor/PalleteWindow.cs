using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Tools
{
    public class PaletteWindow : EditorWindow
    {
        protected class Tab
        {
            public string Name = "";
            
            public List<(GlobalObjectId key, Object resolvedValue)> TrackedObjects =
                new List<(GlobalObjectId, Object)>();
        }

        enum DropInType
        {
            Replace,
            InsertAfter
        };
        
        protected virtual string PlayerPrefKeyPrefix => "PaletteSelection";

        protected virtual System.Type TypeFilter => typeof(Object);

        private bool UseCells => width >= 50;
        
        private Vector2 scrollPosition = Vector2.zero;

        private int width = 100; // Width of a palette cell

        private bool editingTabName;
        private int? hotControl;

        private List<Tab> tabs = new List<Tab>();

        private int activeTab;

        private int mouseDownIndex = -1;

        private int dropInTargetIndex = -1; // Current slot a DragDrop is being considered as a destination
        private DropInType dropInType = DropInType.Replace;

        private bool isDragging; // If a DragDrop from a slot is in progress

        private int deferredIndexToRemove = -1;

        [MenuItem("Window/Palette")]
        private static void CreateAndShow()
        {
            EditorWindow window = GetWindow<PaletteWindow>("Palette");

            window.minSize = new Vector2(120, 120);

            window.Show();
        }

        private void OnEnable()
        {
            LoadAndRepaint();

            // Some palette window types respect undo, so make sure we can reload if needed
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private void OnDisable()
        {
            for (int i = 0; i < 9; i++)
            {
                Save(i);
            }

            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }

        private void LoadAndRepaint()
        {
            for (int i = 0; i < 9; i++)
            {
                Load(i);
            }

            Repaint();
        }

        protected virtual void OnUndoRedoPerformed()
        {
            LoadAndRepaint();
        }

        private void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            LoadAndRepaint();
        }

        private void OnGUI()
        {
            Event e = Event.current;

            int columnCount = (int) (Screen.width / EditorGUIUtility.pixelsPerPoint) / (width + _coreData.tools.paletteSpacing1);
            var trackedObjects = tabs[activeTab].TrackedObjects;
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            GUILayout.Space(_coreData.tools.paletteSpace);
            
            // Make sure there's an empty one at the end to drag into
            if (trackedObjects.Count == 0 || trackedObjects[trackedObjects.Count - 1].key.identifierType != 0)
            {
                trackedObjects.Add((new GlobalObjectId(), null));
            }

            if (e.rawType == EventType.MouseUp || e.rawType == EventType.MouseMove || 
                e.rawType == EventType.DragUpdated || e.rawType == EventType.ScrollWheel)
            {
                dropInTargetIndex = -1;
                dropInType = DropInType.Replace;
            }

            List<(GlobalObjectId, Object)> deferredInsertObjects = null;
            int deferredInsertIndex = -1;

            for (int i = 0; i < trackedObjects.Count; i++)
            {
                int columnIndex = i % columnCount;

                if (UseCells && columnIndex == 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(_coreData.tools.paletteSpace);
                }

                List<(GlobalObjectId key, Object resolvedValue)> newSelectedObjects = null;
                bool dropAccepted = false;
                DrawElement(e, trackedObjects[i], i, out newSelectedObjects, out dropAccepted);

                if (dropAccepted || newSelectedObjects.Count > 1 || 
                    newSelectedObjects[0].resolvedValue != trackedObjects[i].resolvedValue)
                {
                    if (dropInType == DropInType.InsertAfter)
                    {
                        // Defer the insert until after we've drawn the UI so we don't mismatch UI mid-draw
                        deferredInsertIndex = i + 1;
                        deferredInsertObjects = newSelectedObjects;
                    }
                    else
                    {
                        trackedObjects[i] = newSelectedObjects[0];
                        if (newSelectedObjects.Count > 1)
                        {
                            deferredInsertIndex = i + 1;
                            newSelectedObjects.RemoveAt(0);
                            deferredInsertObjects = newSelectedObjects;
                        }

                        Save(activeTab);
                    }
                }

                GUILayout.Space(UseCells ? _coreData.tools.paletteSpace/2 : _coreData.tools.paletteSpace);

                if (UseCells && (columnIndex == columnCount - 1 || i == trackedObjects.Count - 1)) // If last in row
                {
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (e.type == EventType.MouseDrag && !isDragging && mouseDownIndex != -1 && 
                trackedObjects[mouseDownIndex].resolvedValue != null)
            {
                isDragging = true;
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = new[] {trackedObjects[mouseDownIndex].resolvedValue};
                DragAndDrop.StartDrag(trackedObjects[mouseDownIndex].resolvedValue.name);
            }

            if (e.rawType == EventType.MouseUp || e.rawType == EventType.MouseMove || 
                e.rawType == EventType.DragPerform || e.rawType == EventType.DragExited)
            {
                isDragging = false;
                mouseDownIndex = -1;
            }


            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            GUIStyle boxStyle = new GUIStyle(GUI.skin.box) { margin = new RectOffset(0, 0, 0, 0) };
            RectOffset padding = boxStyle.padding;
            padding.top += 1;
            boxStyle.padding = padding;
            GUILayout.Box(new GUIContent(), boxStyle, GUILayout.ExpandWidth(true));

            Rect lastRect = GUILayoutUtility.GetLastRect();

            Rect buttonRect = lastRect;
            buttonRect.y += 1;

            buttonRect.width = (buttonRect.width - 90) / 9f;

            GUIStyle activeStyle = EditorStyles.toolbarButton;
            buttonRect.height = activeStyle.CalcHeight(new GUIContent(), 20);
            for (int i = 0; i < 9; i++)
            {
                string tabName = (i + 1).ToString();
                if (!string.IsNullOrEmpty(tabs[i].Name))
                {
                    tabName = tabs[i].Name;
                }

                bool oldValue = activeTab == i;

                if (oldValue && editingTabName)
                {
                    GUI.SetNextControlName("PaletteTabName");
                    tabs[activeTab].Name = GUI.TextField(buttonRect, tabs[activeTab].Name);

                    if (!hotControl.HasValue)
                    {
                        hotControl = GUIUtility.hotControl;
                        GUI.FocusControl("PaletteTabName");
                    }
                    else
                    {
                        if (GUIUtility.hotControl != hotControl.Value // Clicked off it
                            || Event.current.type == EventType.KeyDown && Event.current.character == (char) 10) // Return pressed
                        {
                            editingTabName = false;
                            hotControl = null;
                            Save(activeTab);
                        }
                    }
                }
                else
                {
                    bool newValue = GUI.Toggle(buttonRect, oldValue, tabName, activeStyle);
                    if (newValue != oldValue)
                    {
                        if (newValue)
                        {
                            activeTab = i;
                            Repaint();
                            editingTabName = false;
                        }
                        else if (newValue == false)
                        {
                            editingTabName = true;
                            hotControl = null;
                        }
                    }
                }

                buttonRect.x += buttonRect.width;
            }

            Rect sliderRect = lastRect;
            sliderRect.xMax -= 10;
            sliderRect.xMin = sliderRect.xMax - 60;

            // User configurable tile size
            width = (int) GUI.HorizontalSlider(sliderRect, width, 49, 100);

            // Delete at the end of the OnGUI so we don't mismatch any UI groups
            if (deferredIndexToRemove != -1)
            {
                trackedObjects.RemoveAt(deferredIndexToRemove);
                deferredIndexToRemove = -1;
                Save(activeTab);
            }

            // Insert at the end of the OnGUI so we don't mismatch any UI groups
            if (deferredInsertObjects != null)
            {
                trackedObjects.InsertRange(deferredInsertIndex, deferredInsertObjects);
                Save(activeTab);
            }

            // Carried out a DragPerform, so reset drop in states
            if (e.rawType == EventType.DragPerform)
            {
                dropInTargetIndex = -1;
                dropInType = DropInType.Replace;
                Repaint();
            }
        }

        public void DrawElement(Event e, (GlobalObjectId key, Object resolvedValue) selectedObject, int index, out List<(GlobalObjectId key, Object resolvedValue)> newSelection, out bool dropAccepted)
        {
            newSelection = new List<(GlobalObjectId, Object)>();

            dropAccepted = false;
            EditorGUILayout.BeginVertical();

            Texture2D previewTexture = null;
            Object resolvedValue = selectedObject.resolvedValue;

            if (resolvedValue != null)
            {
                previewTexture = AssetPreview.GetAssetPreview(resolvedValue);

                if (previewTexture == null)
                {
                    if (AssetPreview.IsLoadingAssetPreview(resolvedValue.GetInstanceID()))
                    {
                        // Not loaded yet, so tell it to repaint
                        Repaint();
                    }
                    else
                    {
                        previewTexture = AssetPreview.GetMiniThumbnail(resolvedValue);
                    }
                }
            }

            Rect previewRect;
            Rect insertAfterRect;

            if (UseCells)
            {
                GUIStyle style = new GUIStyle(GUI.skin.box)
                {
                    normal = EditorStyles.wordWrappedLabel.normal,
                    alignment = TextAnchor.MiddleCenter
                };

                GUIContent guiContent = new GUIContent();
                if (selectedObject.key.identifierType != 0)
                {
                    // Construct a tooltip that has more information about what is tracked
                    string trackedObjectType = selectedObject.key.identifierType == 2 ? "Scene Object" : "Asset";

                    if (selectedObject.resolvedValue != null)
                    {
                        guiContent.tooltip = $"{selectedObject.resolvedValue.name} ({selectedObject.resolvedValue.GetType().Name} {trackedObjectType})\n{selectedObject.key}";
                    }
                    else
                    {
                        guiContent.tooltip = $"Unresolved {trackedObjectType}\n{selectedObject.key}";
                    }

                    if (selectedObject.key.identifierType == 2)
                    {
                        string sceneAssetPath = AssetDatabase.GUIDToAssetPath(selectedObject.key.assetGUID.ToString());
                        if (string.IsNullOrEmpty(sceneAssetPath))
                        {
                            guiContent.tooltip += "\nIn unresolved scene";
                        }
                        else
                        {
                            guiContent.tooltip += "\nIn scene " + sceneAssetPath;
                        }
                    }
                }

                if (previewTexture != null)
                {
                    guiContent.image = previewTexture;
                    style.padding = new RectOffset(0, 0, 0, 0);
                }
                else
                {
                    style.padding = new RectOffset(5, 5, 5, 5);
                    if (selectedObject.key.identifierType == 0)
                    {
                        guiContent.text = "Drag an object here";
                    }
                    else if (selectedObject.key.identifierType == 2) // Scene Object - from Docs is the identifier type represented by an integer (0 = Null, 1 = Imported Asset, 2 = Scene Object, 3 = Source Asset).
                    {
                        var sceneName = Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(selectedObject.key.assetGUID.ToString()));

                        if (string.IsNullOrEmpty(sceneName))
                            guiContent.text = $"Scene (not resolvable) not loaded";
                        else
                            guiContent.text = $"Scene {sceneName} not loaded";
                    }
                    else
                    {
                        guiContent.text = "Asset object not resolved";
                    }
                }

                GUILayout.Box(guiContent, style, GUILayout.Width(width), GUILayout.Height(width));

                previewRect = GUILayoutUtility.GetLastRect();
                insertAfterRect = new Rect(previewRect.xMax, previewRect.y, _coreData.tools.paletteSpace, previewRect.height);

                // Colour code the outline based on type - from Docs is the identifier type represented by an integer (0 = Null, 1 = Imported Asset, 2 = Scene Object, 3 = Source Asset).
                if (selectedObject.key.identifierType == 2) // 2 = Scene Object
                {
                    DrawOutline(previewRect, 1, new Color(0f, 0.36f, 0f));
                }
                else if (selectedObject.key.identifierType == 1 || selectedObject.key.identifierType == 3) // 1 = Imported Asset, 3 = Source Asset
                {
                    DrawOutline(previewRect, 1, new Color(0.21f, 0.26f, 0.36f));
                }

                EditorGUI.BeginChangeCheck();
                resolvedValue = EditorGUILayout.ObjectField(resolvedValue, TypeFilter, false, GUILayout.Width(width));
                if (EditorGUI.EndChangeCheck())
                {
                    selectedObject.key = GlobalObjectId.GetGlobalObjectIdSlow(resolvedValue);
                }
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                resolvedValue = EditorGUILayout.ObjectField(resolvedValue, TypeFilter, false);
                if (EditorGUI.EndChangeCheck())
                {
                    selectedObject.key = GlobalObjectId.GetGlobalObjectIdSlow(resolvedValue);
                }

                previewRect = GUILayoutUtility.GetLastRect();
                insertAfterRect = new Rect(previewRect.xMin, previewRect.yMax, previewRect.width, _coreData.tools.paletteSpace);
            }

            if (dropInTargetIndex == index)
            {
                if (dropInType == DropInType.InsertAfter)
                {
                    Rect insertAfterRectDisplay = insertAfterRect;
                    insertAfterRectDisplay.xMin += 2;
                    insertAfterRectDisplay.xMax -= 2;
                    DrawOutline(insertAfterRectDisplay, 2, Color.blue);
                }
                else
                {
                    DrawOutline(previewRect, 2, Color.blue);
                }
            }

            bool mouseInRect = previewRect.Contains(e.mousePosition);
            bool mouseInInsertAfterRect = insertAfterRect.Contains(e.mousePosition);

            if (mouseInRect && e.type == EventType.MouseDown)
            {
                mouseDownIndex = index;
            }

            if (mouseInRect && e.type == EventType.MouseUp && !isDragging)
            {
                if (e.button == 0)
                {
                    Selection.activeObject = resolvedValue;
                }
                else
                {
                    if (resolvedValue == null)
                    {
                        deferredIndexToRemove = index;
                    }
                    else
                    {
                        selectedObject = (new GlobalObjectId(), null);
                    }

                    Repaint();
                }
            }

            if (mouseInRect && e.type == EventType.MouseDown && !isDragging)
            {
                if (e.button == 0)
                {
                    if (e.clickCount == 2 && resolvedValue != null)
                    {
                        AssetDatabase.OpenAsset(resolvedValue);
                    }
                }
            }

            if (e.type == EventType.DragUpdated || e.type == EventType.DragPerform)
            {
                foreach (Object objectReference in DragAndDrop.objectReferences)
                {
                    if (objectReference.GetType() == TypeFilter || objectReference.GetType().IsSubclassOf(TypeFilter))
                    {
                        if (mouseInRect || mouseInInsertAfterRect)
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                            if (e.type == EventType.DragPerform)
                            {
                                DragAndDrop.AcceptDrag();

                                newSelection.Add((GlobalObjectId.GetGlobalObjectIdSlow(objectReference), objectReference));

                                dropAccepted = true;
                            }
                            else
                            {
                                dropInTargetIndex = index;
                                dropInType = mouseInInsertAfterRect ? DropInType.InsertAfter : DropInType.Replace;
                            }

                            Repaint();
                        }
                    }
                }
            }

            EditorGUILayout.EndVertical();

            // We didn't change selection, so just fill it in with what is occupying the cell
            if (newSelection.Count == 0)
            {
                newSelection.Add(selectedObject);
            }
        }

        void DrawOutline(Rect lastRect, int lineThickness, Color color)
        {
            var oldColor = GUI.color;
            GUI.color = color;

            GUI.DrawTexture(new Rect(lastRect.xMin, lastRect.yMin, lineThickness, lastRect.height), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(lastRect.xMax - lineThickness, lastRect.yMin, lineThickness, lastRect.height), EditorGUIUtility.whiteTexture);

            GUI.DrawTexture(new Rect(lastRect.xMin + lineThickness, lastRect.yMin, lastRect.width - lineThickness * 2, lineThickness), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(lastRect.xMin + lineThickness, lastRect.yMax - lineThickness, lastRect.width - lineThickness * 2, lineThickness), EditorGUIUtility.whiteTexture);

            GUI.color = oldColor; // Reset GUI color
        }

        /// <summary>
        /// By default use PlayerPrefs to persist palette objects, but allow derived classes to implement alternative
        /// persistence methods
        /// </summary>
        protected virtual void SaveImplementation(int tabIndex, string tabName, string outputString)
        {
            string key = PlayerPrefKeyPrefix;
            if (tabIndex > 0)
            {
                key += tabIndex;
            }

            PlayerPrefs.SetString(key, outputString);

            PlayerPrefs.SetString(key + "-Tab", tabName);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// By default use PlayerPrefs to persist palette objects, but allow derived classes to implement alternative
        /// persistence methods
        /// </summary>
        protected virtual void LoadImplementation(int tabIndex, out string trackedString, out string tabName)
        {
            string key = PlayerPrefKeyPrefix;
            if (tabIndex > 0)
            {
                key += tabIndex;
            }

            trackedString = PlayerPrefs.GetString(key);
            tabName = PlayerPrefs.GetString(key + "-Tab");
        }

        private void Save(int tabIndex)
        {
            while (tabs.Count <= tabIndex)
            {
                tabs.Add(new Tab());
            }

            var trackedObjects = tabs[tabIndex].TrackedObjects;
            StringBuilder output = new StringBuilder();
            for (var index = 0; index < trackedObjects.Count; index++)
            {
                var trackedObject = trackedObjects[index];
                output.Append(trackedObject.key.ToString());
                if (index != trackedObjects.Count - 1)
                {
                    output.Append(",");
                }
            }

            SaveImplementation(tabIndex, tabs[tabIndex].Name, output.ToString());
        }

        private void Load(int tabIndex)
        {
            while (tabs.Count <= tabIndex)
            {
                tabs.Add(new Tab());
            }

            tabs[tabIndex].TrackedObjects.Clear();

            LoadImplementation(tabIndex, out var trackedString, out var tabName);

            string[] trackedObjectStrings = trackedString.Split(',');
            foreach (string trackedObjectString in trackedObjectStrings)
            {
                if (GlobalObjectId.TryParse(trackedObjectString, out GlobalObjectId globalObjectId))
                {
                    // Resolve
                    var resolvedObject = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(globalObjectId);
                    // Track
                    tabs[tabIndex].TrackedObjects.Add((globalObjectId, resolvedObject));
                }
            }

            tabs[tabIndex].Name = tabName;
        }
    }
}