using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // This method is called to change the scene to the specified scene name
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // Loads the scene with the given name, allowing for scene transitions in the game
    }
}