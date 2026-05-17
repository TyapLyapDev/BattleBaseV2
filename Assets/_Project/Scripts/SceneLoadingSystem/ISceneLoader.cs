namespace BattleBase.SceneLoadingService
{
    public interface ISceneLoader
    {
        public string CurrentSceneName { get; }

        public void Load(string name);

        public void ReloadCurrentScene();
    }
}