using System;
using BattleBase.DI;
using BattleBase.SceneLoadingService;
using VContainer;

namespace BattleBase.Commands
{
    public sealed class CommandReloadCurrentScene : CommandBase, IInjectable
    {
        private ISceneLoader _sceneLoader;

        [Inject]
        public void Construct(ISceneLoader sceneLoader) =>
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));

        protected override void OnExecute() =>
            _sceneLoader.ReloadCurrentScene();
    }
}