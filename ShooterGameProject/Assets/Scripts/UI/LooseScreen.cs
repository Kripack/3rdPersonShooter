using UnityEngine;
using UnityEngine.SceneManagement;
public class LooseScreen : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject holder;
    [SerializeField] private float delay = 4f;

    private void OnEnable()
    {
        player.Health.Die += InvokeUI;
    }
    
    private void OnDisable()
    {
        player.Health.Die -= InvokeUI;
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
        holder.SetActive(true);
    }
}
