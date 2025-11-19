using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<LightCaster> lightCasters;
    
    private List<MovePuzzlePiece> _puzzlePieceMovers;

    [SerializeField] private Transform[] piecesToMove;

    [SerializeField] private FinalSequenceAnimator finalSequenceAnimator;

    //Note: this function may be renamed to something like "UpdatePuzzleInstance" since it also resets the MovePuzzlePiece
    public void UpdateAllLightCasters()
    {
        foreach (var lc in lightCasters)
        {
            lc.RecalculateRay();
        }

        if (!_puzzlePieceMovers.IsUnityNull())
        {
            foreach (var puzzlePieceMover in _puzzlePieceMovers)
            {
                puzzlePieceMover.ResetLocks();
            }
        }

        if (!finalSequenceAnimator.IsUnityNull())
        {
            finalSequenceAnimator.ResetLocks();
        }
    }

    public void DisableAllLightCasters()
    {
        foreach (var lc in lightCasters)
        {
            lc.ResetInteractable();
        }
    }

    public void SubscribeNewPuzzlePieceMover(MovePuzzlePiece movePuzzlePiece)
    {
        if (_puzzlePieceMovers.IsUnityNull())
        {
            _puzzlePieceMovers = new List<MovePuzzlePiece>();
        }
        _puzzlePieceMovers.Add(movePuzzlePiece);
    }


    public void StartPuzzle()
    {
        /*
        if(piecesToMove.Length == 0)
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 6, transform.position.z);
        }
        else
        {
            foreach (var piece in piecesToMove) { piece.localPosition = new Vector3(piece.localPosition.x, piece.localPosition.y + 6, piece.localPosition.z); }
        }

        UpdateAllLightCasters();*/
        DisableAllLightCasters();
        StartCoroutine(AnimatePuzzleStart());
    }

    private IEnumerator AnimatePuzzleStart()
    {
        if(piecesToMove.Length == 0)
        {
            var newPos = transform.localPosition;
            var increment = 6 / (3 / Time.fixedDeltaTime);
            while (newPos.y < 0)
            {
                newPos.y = Mathf.Min(0, newPos.y + increment);
                transform.localPosition = newPos;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            var newPos = new Vector3[piecesToMove.Length];
            for (var i = 0; i < piecesToMove.Length; i++)
            {
                newPos[i] = piecesToMove[i].localPosition;
            }
            var increment = 6 / (3 / Time.fixedDeltaTime);
            while (newPos[0].y < 0)
            {
                for (var i = 0; i < piecesToMove.Length; i++)
                {
                    newPos[i].y = Mathf.Min(0, newPos[i].y + increment);
                    piecesToMove[i].localPosition = newPos[i];
                }
                yield return new WaitForFixedUpdate();
            }
        }
        UpdateAllLightCasters();
    }
}
