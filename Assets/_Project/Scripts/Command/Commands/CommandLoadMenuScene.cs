using System;
using BattleBase.DI;
using BattleBase.SceneLoadingService;
using BattleBase.Utils;
using VContainer;

namespace BattleBase.Commands
{
    public sealed class CommandLoadMenuScene : CommandBase, IInjectable
    {
        private ISceneLoader _sceneLoader;

        [Inject]
        public void Construct(ISceneLoader sceneLoader) =>
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));

        protected override void OnExecute() =>
            _sceneLoader.Load(Constants.MenuSceneName);
    }
}