using BattleBase.Commands;
using BattleBase.Core;
using BattleBase.Gameplay.CameraNavigation;
using BattleBase.Gameplay.CameraNavigation.InputReader;
using BattleBase.Gameplay.Map;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using YG;

namespace BattleBase.DI
{
    public class MapScope : LifetimeScope
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CameraRig _cameraRig;
        [SerializeField] private CameraArea _cameraArea;
        [SerializeField] private TerritorySelectPopUp _territorySelectPopUpPrefab;
        [SerializeField] private CommandLoadGameScene _commandLoadGameScene;
        [SerializeField] private CommandRebuildLayout _commandRebuildLayout;
        [SerializeField] private MouseInputConfig _mouseMapCameraConfig;
        [SerializeField] private TouchInputConfig _touchMapCameraConfig;
        [SerializeField] private TerritoryPositionAnimationConfig _territoryPositionAnimationConfig;

        private IContainerBuilder _builder;

        protected override void Configure(IContainerBuilder builder)
        {
            _builder = builder;

            RegisterCommands();
            RegisterCameraSystem();
            RegisterTerritorySystem();
            RegisterInputReader();
        }

        private void RegisterCommands()
        {
            _builder.RegisterComponent(_commandLoadGameScene);
            _builder.RegisterComponent(_commandRebuildLayout);
        }

        private void RegisterCameraSystem()
        {
            _builder.RegisterComponent(_camera);
            _builder.RegisterComponent(_cameraRig);
            _builder.RegisterComponent<ICameraArea>(_cameraArea);
            _builder.RegisterComponent(_cameraArea.Config).AsImplementedInterfaces();

            _builder.Register<ICameraTracker, CameraTracker>(Lifetime.Scoped);
            _builder.Register<IFrustumProjectionService, FrustumProjectionService>(Lifetime.Scoped);
            _builder.Register<ICameraAreaService, CameraAreaService>(Lifetime.Scoped);
            _builder.Register<ICameraSnapBack, CameraSnapBack>(Lifetime.Scoped);
            _builder.Register<IUIPointerChecker, UIPointerChecker>(Lifetime.Scoped);
            _builder.Register<ICameraBoundsLimiter, CameraBoundsLimiter>(Lifetime.Scoped);
            _builder.Register<IVerticalFactorCalculator, VerticalFactorCalculator>(Lifetime.Scoped);
            _builder.Register<ICameraOrientationAdapter, MapSceneCameraOrientationAdapter>(Lifetime.Scoped);
            _builder.Register<IScreenSizeTracker, ScreenSizeTracker>(Lifetime.Scoped);
            _builder.Register<IScreenOrientationTracker, ScreenOrientationTracker>(Lifetime.Scoped);
            _builder.Register<ICameraZoom, CameraZoom>(Lifetime.Scoped);
            _builder.Register<ICameraDragger, CameraDragger>(Lifetime.Scoped);
            _builder.Register<IPositionRestrictor, PositionRestrictor>(Lifetime.Scoped);
            _builder.Register<IResistanceCalculator, ResistanceCalculator>(Lifetime.Scoped);
            _builder.Register<IDragApplier, DragApplier>(Lifetime.Scoped);
            _builder.Register<IInertiaSnapbackApplier, InertiaSnapbackApplier>(Lifetime.Scoped);
        }

        private void RegisterTerritorySystem()
        {

            _builder.RegisterComponent(_territorySelectPopUpPrefab);
            _builder.RegisterComponent(_territoryPositionAnimationConfig);

            _builder.Register<TerritoryElevator>(Lifetime.Scoped);
            _builder.Register<TerritoryPopUpShower>(Lifetime.Scoped);

            _builder.Register<ITerritorySelector, TerritorySelector>(Lifetime.Scoped);
            _builder.Register<IPool<TerritorySelectPopUp>, Pool<TerritorySelectPopUp>>(Lifetime.Scoped);
            _builder.Register<IFactory<TerritorySelectPopUp>, TerritorySelectPopUpFactory>(Lifetime.Scoped);

            _builder.RegisterBuildCallback(container =>
            {
                container.Resolve<TerritoryElevator>();
                container.Resolve<TerritoryPopUpShower>();
            });
        }

        private void RegisterInputReader()
        {
            _builder.Register<ICameraInputReader, CameraInputReader>(Lifetime.Scoped);

            if (YG2.envir.isDesktop)
            {
                _builder.RegisterComponent(_mouseMapCameraConfig).AsImplementedInterfaces();
                _builder.Register<IClickDetector, MouseClickDetector>(Lifetime.Scoped);
                _builder.Register<IMouseDragHandler, MouseDragHandler>(Lifetime.Scoped);
                _builder.Register<IDragHandler, CompositeMouseDragHandler>(Lifetime.Scoped);
                _builder.Register<IKeyboardDragHandler, KeyboardDragHandler>(Lifetime.Scoped);
                _builder.Register<IZoomHandler, MouseZoomHandler>(Lifetime.Scoped);
            }
            else
            {
                _builder.RegisterComponent(_touchMapCameraConfig).AsImplementedInterfaces();
                _builder.Register<IClickDetector, TouchClickDetector>(Lifetime.Scoped);
                _builder.Register<IDragHandler, TouchDragHandler>(Lifetime.Scoped);
                _builder.Register<IZoomHandler, TouchPinchHandler>(Lifetime.Scoped);
            }
        }
    }
}