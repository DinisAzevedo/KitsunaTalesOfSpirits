using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicPuzzleManager : MonoBehaviour
{
    public List<int> correctSequence = new List<int> { 0, 2, 1, 3 };
    private List<int> playerSequence = new List<int>();

    public UnityEvent onPuzzleSolved;
    public UnityEvent onPuzzleFailed;

    public void RegisterNote(int noteID)
    {
        playerSequence.Add(noteID);

        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                Fail();
                return;
            }
        }

        if (playerSequence.Count == correctSequence.Count)
        {
            Success();
        }
    }

    void Success()
    {
        Debug.Log("Puzzle Complete!");
        onPuzzleSolved?.Invoke();
        playerSequence.Clear();
    }

    void Fail()
    {
        Debug.Log("Wrong sequence!");
        onPuzzleFailed?.Invoke();
        playerSequence.Clear();
    }
}