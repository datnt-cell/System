using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using ConditionEngine.Presentation;

public class GameConfigManagerWindow : OdinMenuEditorWindow
{
    [MenuItem("Tools/Game Tools/Game Config Manager")]
    private static void Open()
    {
        var window = GetWindow<GameConfigManagerWindow>();
        window.titleContent = new GUIContent("Game Config Manager");
        window.Show();
    }

    // =========================
    // MENU TREE
    // =========================

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        tree.Config.DrawSearchToolbar = true;

        // ECONOMY
        tree.Add("ECONOMY/💰 Currencies", CurrencyGlobalConfig.Instance);
        tree.Add("ECONOMY/📦 Currency Bundles", CurrencyBundleGlobalConfig.Instance);
        tree.Add("ECONOMY/🛒 Store Items", StoreItemsGlobalConfig.Instance);

        // OFFERS
        tree.Add("OFFERS/🎁 Game Offers", GameOfferGlobalConfig.Instance);
        tree.Add("OFFERS/🗂 Offer Groups", GameOfferGroupGlobalConfig.Instance);

        // CONDITIONS
        tree.Add("CONDITIONS/⚙ Condition Config", ConditionGlobalConfig.Instance);

        return tree;
    }

    // =========================
    // TOOLBAR
    // =========================

    protected override void OnBeginDrawEditors()
    {
        SirenixEditorGUI.BeginHorizontalToolbar();

        GUILayout.Space(8);

        GUILayout.Label("⚙ GAME CONFIG MANAGER", SirenixGUIStyles.BoldLabel);

        GUILayout.FlexibleSpace();

        if (SirenixEditorGUI.ToolbarButton(new GUIContent("💾 Save All", "Save all config assets")))
        {
            SaveAllConfigs();
        }

        if (SirenixEditorGUI.ToolbarButton(new GUIContent("🔄 Reload", "Reload all configs")))
        {
            ReloadConfigs();
        }

        GUILayout.Space(8);

        SirenixEditorGUI.EndHorizontalToolbar();
    }

    // =========================
    // ACTIONS
    // =========================

    private void SaveAllConfigs()
    {
        AssetDatabase.SaveAssets();

        ShowToast("✅ All configs saved");
        Debug.Log("<color=green>[GameConfig]</color> All configs saved");
    }

    private void ReloadConfigs()
    {
        AssetDatabase.Refresh();

        ShowToast("🔄 Configs reloaded");
        Debug.Log("<color=yellow>[GameConfig]</color> Configs refreshed");
    }

    double _toastEndTime;

    private void ShowToast(string message, float duration = 1.0f)
    {
        ShowNotification(new GUIContent(message));

        _toastEndTime = EditorApplication.timeSinceStartup + duration;

        EditorApplication.update -= UpdateToast;
        EditorApplication.update += UpdateToast;
    }

    private void UpdateToast()
    {
        if (EditorApplication.timeSinceStartup > _toastEndTime)
        {
            RemoveNotification();
            EditorApplication.update -= UpdateToast;
        }
    }
}