
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader3 : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex - 4
        );
    }
}