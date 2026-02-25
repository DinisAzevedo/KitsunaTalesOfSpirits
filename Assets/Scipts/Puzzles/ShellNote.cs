using UnityEngine;

public class ShellNote : MonoBehaviour, IInterectable
{
    public int noteID; // 0,1,2,3
    public AudioSource audioSource;
    public MusicPuzzleManager puzzleManager;

    public void Interact()
    {
        PlayNote();
        puzzleManager.RegisterNote(noteID);
    }

    public bool CanInteract()
    {
        return true;
    }

    private void PlayNote()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}