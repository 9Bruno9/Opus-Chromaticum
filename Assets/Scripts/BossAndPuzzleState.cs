using UnityEngine;

public class BossAndPuzzleState : MonoBehaviour
{
    public GameObject Boss;
    public PuzzleManager Puzzle;
    public int Nscene;

    [SerializeField] private GameEvent puzzleSolvedEvent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if(GameManager.instance.BossDestroyed[Nscene]) 
        {
            Puzzle.StartPuzzle();
        }

        if (GameManager.instance.BossDestroyed[Nscene] && GameManager.instance.puzzleResolved[Nscene])
        {
            puzzleSolvedEvent.TriggerEvent();
        }   
    }

    
}
