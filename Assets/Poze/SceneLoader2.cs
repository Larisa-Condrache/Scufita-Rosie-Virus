using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader2 : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex - 1
        );
    }
}