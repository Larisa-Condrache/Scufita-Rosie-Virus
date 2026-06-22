using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader4 : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 2
        );
    }
}