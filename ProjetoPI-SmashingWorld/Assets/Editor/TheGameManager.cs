using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;

public class TheGameManager : OdinMenuEditorWindow
{
    [OnValueChanged("StateChange")]
    [LabelText("Manager View")]
    [LabelWidth(100f)]
    [EnumToggleButtons]
    [ShowInInspector]
    private ManagerState managerState;
    private int enumIndex = 0;
    private bool treeRebuild = false;

    private DrawSelected<CharactersData> drawCharacters = new DrawSelected<CharactersData>();


    //paths to SOs in project
    private string CharactersPath = "Asset/Prefabs/Characters/CharactersData";

    [MenuItem("Tools/The Game Manager")]
    public static void OpenWindow()
    {
        GetWindow<TheGameManager>().Show();
    }

    private void StateChange()
    {
        treeRebuild = true;
    }
    protected override void Initialize()
    {
        drawCharacters.SetPath(CharactersPath);
    }

    protected override void OnGUI()
    {
        if (treeRebuild && Event.current.type == EventType.Layout)
        {
            ForceMenuTreeRebuild();
            treeRebuild = false;
        }

        SirenixEditorGUI.Title("The Game Manager", "Por motivos de escopo tenso SEMPRE", TextAlignment.Center, true);
        EditorGUILayout.Space();

        switch (managerState)
        {
            case ManagerState.Characters:
            case ManagerState.Stages:
            case ManagerState.Enemies:
                DrawEditor(enumIndex);
                break;
            default:
                break;
        }
        EditorGUILayout.Space();
        
        base.OnGUI();
    }

    protected override void DrawEditors()
    {
        switch (managerState)
        {
            case ManagerState.Characters:
                drawCharacters.SetSelected(this.MenuTree.Selection.SelectedValue);
                break;
            case ManagerState.Stages:
                break;
            case ManagerState.Sfx:
                DrawEditor(enumIndex);
                break;
            case ManagerState.Enemies:
                break;
            default:
                break;
        }
    }

    protected override IEnumerable<object> GetTargets()
    {
        List<object> targets = new List<object>();
        targets.Add(base.GetTarget());

        enumIndex = targets.Count - 1;

        return targets;
    }

    protected override void DrawMenu()
    {
        switch (managerState)
        {
            case ManagerState.Characters:
            case ManagerState.Stages:              
            case ManagerState.Enemies:
                base.DrawMenu();
                break;
            default:
                break;
        }
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        switch (managerState)
        {
            case ManagerState.Characters:
                tree.AddAllAssetsAtPath("Character Data", CharactersPath, typeof(CharactersData));
                break;
            case ManagerState.Stages:
                break;
            case ManagerState.Sfx:
                break;
            case ManagerState.Enemies:
                break;
            default:
                break;
        }
        return tree;
    }

    public enum ManagerState
    {
        Characters,
        Stages,
        Sfx,
        Enemies,

    }
}

public class ColorFoldoutGroupAttribute : PropertyGroupAttribute
{
    public float R, G, B, A;

    public ColorFoldoutGroupAttribute(string path) : base(path)
    {

    }

    public ColorFoldoutGroupAttribute(string path, float r, float g, float b, float a = 1f) : base(path)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }

    protected override void CombineValuesWith(PropertyGroupAttribute other)
    {
        var otherAttr = (ColorFoldoutGroupAttribute)other;

        this.R = Math.Max(otherAttr.R, this.R);
        this.G = Math.Max(otherAttr.G, this.G);
        this.B = Math.Max(otherAttr.B, this.B);
        this.A = Math.Max(otherAttr.A, this.A);
    }
}


public class DrawSelected<T> where T : ScriptableObject
{
    [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
    public T selected;

    [LabelWidth(100)]
    [PropertyOrder(-1)]
    [ColorFoldoutGroup("CreateNew", 1f, 1f,1f)]
    [HorizontalGroup("CreateNew/Horizontal")]
    public string nameForNew;

    private string path;

    [HorizontalGroup("CreateNew/Horizontal")]
    [GUIColor(0.7f,0.7f,1f)]
    [Button]
    public void CreateNew()
    {
        if (nameForNew == "")
            return;
        T newItem = ScriptableObject.CreateInstance<T>();

        if (path == "")
            path = "Assets/";
        AssetDatabase.CreateAsset(newItem, path + "\\" + nameForNew + ".asset");
        AssetDatabase.SaveAssets();

        nameForNew = "";
        
    }

    [HorizontalGroup("CreateNew/Horizontal")]
    [GUIColor(1f, 0.7f,0.7f)]
    [Button]
    public void DeleteSelected()
    {
        if(selected != null)
        {
            string _path = AssetDatabase.GetAssetPath(selected);
            AssetDatabase.DeleteAsset(_path);
            AssetDatabase.SaveAssets();
        }
    }

    public void SetSelected(object item)
    {
        var attempt = item as T;
        if (attempt != null)
            this.selected = attempt;
    }

    public void SetPath(string path)
    {
        this.path = path;
    }
}
