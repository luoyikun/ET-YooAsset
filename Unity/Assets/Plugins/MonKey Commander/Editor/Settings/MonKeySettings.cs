using System;
using System.Collections.Generic;
using System.Reflection;
using MonKey.Editor.Internal;
using MonKey.Extensions;
using MonKey.Internal;
using MonKey.Settings.Internal;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditorInternal;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

public class MonKeySettings : SerializedScriptableObject
{
    public static readonly string defaultMonKeyInstallPath =
        "Assets/Plugins/MonKey Commander/Editor/Settings/MonKey Settings.asset";

    public static MonKeySettings Instance
    {
        get { return !instance ? InitSettings() : instance; }
    }

    private static MonKeySettings instance;

    public static MonKeySettings InitSettings()
    {
        instance = AssetDatabase.LoadAssetAtPath<MonKeySettings>(defaultMonKeyInstallPath);

        if (!instance)
        {
            return CreateNewInstance();
        }

        SavePrefs();

        CommandManager.FindInstance();
        return instance;
    }

    private static MonKeySettings CreateNewInstance()
    {
        instance = CreateInstance<MonKeySettings>();

        AssetDatabase.CreateAsset(instance, defaultMonKeyInstallPath);
        AssetDatabase.SaveAssets();
        SavePrefs();
        return instance;
    }

    public static void SavePrefs()
    {
        MonKeyInternalSettings internalSettings = MonKeyInternalSettings.Instance;

        if (!internalSettings)
            return;

        internalSettings.UseSortedSelection = instance.UseSortedSelection;
        internalSettings.MaxSortedSelectionSize = instance.MaxSortedSelectionSize;
        internalSettings.ShowSortedSelectionWarning = instance.ShowSortedSelectionWarning;
        internalSettings.MonkeyConsoleOverrideHotKey = instance.MonkeyConsoleOverrideHotKey;
        internalSettings.PauseGameOnConsoleOpen = instance.PauseGameOnConsoleOpen;
        internalSettings.PutInvalidCommandAtEndOfSearch = instance.PutInvalidCommandAtEndOfSearch;
        internalSettings.IncludeMenuItems = instance.IncludeMenuItems;
        internalSettings.IncludeOnlyMenuItemsWithHotKeys = instance.IncludeOnlyMenuItemsWithHotKeys;
        internalSettings.ExcludedAssemblies = instance.ExcludedAssemblies;

        string[] includeAssemblies = new string[instance.IncludedAssemblies.Count];
        for (int i = 0; i < instance.IncludedAssemblies.Count; i++)
        {
            includeAssemblies[i] = instance.IncludedAssemblies[i];
        }

        internalSettings.IncludeAssemblies = string.Join(";", includeAssemblies);
        internalSettings.IncludeModeOnly = instance.IncludeModeOnly;
        internalSettings.ExcludedNameSpaces = instance.ExcludedNameSpaces;
        internalSettings.ForceFocusOnDocked = instance.ForceFocusOnDocked;
        internalSettings.ShowHelpOnlyOnActiveCommand = instance.ShowHelpOnSelectedOnly;

        internalSettings.PostSave();
    }

    [BoxGroup("基础设置")] [LabelText("排序")] public bool UseSortedSelection = true;

    [BoxGroup("基础设置")] [LabelText("排序最大数量")]
    public int MaxSortedSelectionSize = 1000;

    [BoxGroup("基础设置")] [LabelText("显示排序警告")]
    public bool ShowSortedSelectionWarning = true;

    [BoxGroup("基础设置")] [LabelText("快捷键")] public string MonkeyConsoleOverrideHotKey = "";

    public bool UseCustomConsoleKey
    {
        get { return !MonkeyConsoleOverrideHotKey.IsNullOrEmpty(); }
    }

    [BoxGroup("基础设置")] [LabelText("窗口展示时暂停游戏")]
    public bool PauseGameOnConsoleOpen = true;

    [BoxGroup("扫描设置")] [HideInInspector] public bool PutInvalidCommandAtEndOfSearch = false;

    [BoxGroup("扫描设置")] [LabelText("包含MenuItem")]
    public bool IncludeMenuItems = true;

    [HideInInspector] public bool IncludeOnlyMenuItemsWithHotKeys = false;

    [BoxGroup("扫描设置")] [LabelText("仅扫描包含目标程序集")]
    public bool IncludeModeOnly = true;

    [LabelText("排除的程序集")] [HideIf(nameof(IncludeModeOnly))] [Tooltip("以;分割")]
    public string ExcludedAssemblies = "";

    [BoxGroup("扫描设置")] [LabelText("排除的命名空间")] [HideIf(nameof(IncludeModeOnly))]
    public string ExcludedNameSpaces = "";

    [BoxGroup("扫描设置")] [LabelText("目标程序集")]
    public List<string> IncludedAssemblies = new List<string>();

    [HideInInspector] public bool ForceFocusOnDocked = false;
    [HideInInspector] public bool ShowHelpOnSelectedOnly = false;

    private GUIStyle titleStyle;

    [Button("应用设置", ButtonSizes.Medium), GUIColor(113f / 255f, 190 / 255f, 10 / 255f)]
    private void ApplySetting()
    {
        MonKeySettings.SavePrefs();
        CompilationPipeline.RequestScriptCompilation();
    }
}