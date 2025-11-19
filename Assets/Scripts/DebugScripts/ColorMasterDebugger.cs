using System.Text;
using UnityEngine;

public class ColorMasterDebugger : MonoBehaviour
{
    private void Start()
    {
        var sb = new StringBuilder();
        sb.Append("PrimaryColors :\n");
        foreach (var col in ColorMaster.Instance.GetPrimaryColors())
        {
            sb.Append(col.LatinName);
            sb.Append("\n");
        }
        sb.Append("SecondaryColors :\n");
        foreach (var col in ColorMaster.Instance.GetSecondaryColors())
        {
            sb.Append(col.LatinName);
            sb.Append("\n");
        }
        sb.Append("ComplexColors :\n");
        foreach (var col in ColorMaster.Instance.GetComplexColors())
        {
            sb.Append(col.LatinName);
            sb.Append("\n");
        }

        Debug.Log(sb.ToString());
    }
}
