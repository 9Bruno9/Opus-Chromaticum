using System.Collections.Generic;
using System.Text;
using ScriptableObjects;
using UnityEngine;

namespace DebugScripts
{
    public class AlchemyColorHitDebugger : MonoBehaviour
    {
        [SerializeField]
        private AlchemyColor objectColor;
        [SerializeField]
        private AlchemyColor hitWith;

        [SerializeField] private List<AlchemyColor> allColors;
    
        [ContextMenu("Hit it!")]
        private void HitIt()
        {
            Debug.Log(objectColor.HitThisWith(hitWith));
        }
    
        [ContextMenu("Hit all!")]
        private void HitAll()
        {
            StringBuilder sb = new();
        
            foreach (var alchemyColorBase in allColors)
            {
                sb.Append("------\n");
                foreach (var alchemyColorHitter in allColors)
                {
                    sb.Append($"{alchemyColorBase.name} <- {alchemyColorHitter.name}: {alchemyColorBase.HitThisWith(alchemyColorHitter)}\n");
                }
            }
            Debug.Log(sb);
        }
        [ContextMenu("Print Components")]
        private void PrintComponents()
        {
            StringBuilder sb = new();
        
            foreach (var alchemyColorBase in allColors)
            {
                sb.Append($"------\n{alchemyColorBase.name}\n");
                foreach (var col in alchemyColorBase.GetComponenti())
                {
                    sb.Append($"- {col.name} ");
                }

                sb.Append("\n");
            }
            Debug.Log(sb);
        }
        
    }
}
