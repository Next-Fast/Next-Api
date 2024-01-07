using UnityEngine.SceneManagement;

namespace NextShip.Api.Extension;

public static class ModStampExtension
{
    private static bool Added = false;
    public static void UseModStamp()
    {
        if (Added)
            return;
        
        SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>) ((scene, _) =>
        {
            if (scene.name == "MainMenu")
            {
                ModManager.Instance.ShowModStamp();
            }
        }));
        
        Added = true;
    }
}