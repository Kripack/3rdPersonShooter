using UnityEngine;
using UnityEngine.SceneManagement;
public class LooseScreen : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject looseScreen;
    [SerializeField] private float delay = 4f;

    private void Start()
    {
        player.Health.Die += InvokeUI;
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void InvokeUI()
    {
        Invoke(nameof(ShowUI), delay);
    }

    private void ShowUI()
    {
        Cursor.lockState = CursorLockMode.None;
        looseScreen.SetActive(true);
    }
}
