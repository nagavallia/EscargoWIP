using UnityEngine.SceneManagement;

public interface GameManager {
    void Startup();

    void Load(Scene scene, LoadSceneMode mode);

    void Unload(Scene scene);
}
