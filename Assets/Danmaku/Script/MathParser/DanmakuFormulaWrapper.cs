using UnityEngine;
using System.Collections;
using DanmakuEditor;

public class DanmakuFormulaWrapper : MonoBehaviour
{
    private static DanmakuFormulaWrapper s_Instance;
    public static DanmakuFormulaWrapper Instance
    {
        get
        {
            if (s_Instance != null)
            {
                return s_Instance;
            }

            s_Instance = FindObjectOfType<DanmakuFormulaWrapper>();
            if (s_Instance != null)
            {
                return s_Instance;
            }

            return null;
        }
    }

    public MathParserRouter mathParserRouter;

    public void Start()
    {
        mathParserRouter = new MathParserRouter();
    }

    public float GetValue(FormulaNode formulaNode)
    {
        if (typeof(MathExpNode) == formulaNode.GetType())
        {
            return mathParserRouter.CalculateAnswer(((MathExpNode)formulaNode).formula);
        }
        else if (typeof(PureNumberNode) == formulaNode.GetType())
        {
            return ((PureNumberNode)formulaNode).value;
        }
        else if (typeof(SequenceNode) == formulaNode.GetType()) {

        }

        return 0;
    }



}
