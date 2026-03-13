using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;

    void Start()
    {
        menuCanvas.SetActive(true);

    }

    public void CloseMenu()
    {
        menuCanvas.SetActive(false);
    }
}
