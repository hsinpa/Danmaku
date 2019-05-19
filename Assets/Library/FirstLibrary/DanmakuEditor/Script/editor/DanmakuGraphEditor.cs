
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using DanmakuEditor;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(DanmakuGraph))]
public class DanmakuGraphEditor : NodeGraphEditor
{
    private bool isExecuteOnce = false;

    //public override void OnGUI()
    //{
    //    // Keep repainting the GUI of the active NodeEditorWindow
    //    NodeEditorWindow.current.Repaint();

    //    if (!isExecuteOnce)
    //    {
    //        AIAgentChart chart = (AIAgentChart)target;
    //        chart.SaveEvent -= SaveRecord;
    //        chart.SaveEvent += SaveRecord;
    //        isExecuteOnce = true;
    //    }
    //}

    private void SaveRecord()
    {
        DanmakuGraph chart = (DanmakuGraph)target;
        EditorUtility.SetDirty(target);

        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        AssetDatabase.SaveAssets();
    }

}