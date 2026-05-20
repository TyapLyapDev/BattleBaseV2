using BattleBase.Commands;
using BattleBase.Core;
using BattleBase.Gameplay;
using BattleBase.Gameplay.CameraNavigation;
using BattleBase.Gameplay.CameraNavigation.InputReader;
using BattleBase.Gameplay.MiniMap;
using BattleBase.UI;
using BattleBase.UI.PopUps;
using BattleBase.Utils.Constants;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using YG;

namespace BattleBase.DI
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CameraRig _cameraRig;
        [SerializeField] private CameraArea _cameraArea;
        [SerializeField] private MouseInputConfig _mouseMapCameraConfig;
        [SerializeField] private TouchInputConfig _touchMapCameraConfig;
        [SerializeField] private IconMapObject _iconMapObjectPrefab;
        [SerializeField] private ProductionItem _itemPrefab;
        [SerializeField] private ItemInfoPopUp _itemInfoPopUp;
        [SerializeField] private CommandBase _commandShowItemInfoPopUp;

        private IContainerBuilder _builder;

        protected override void Configure(IContainerBuilder builder)
        {
            _builder = builder;

            RegisterMiniMapSystem();
            RegisterCameraSystem();
            RegisterCameraInputReader();
            RegisterBuildingSiteSelector();
        }

        private void RegisterBuildingSiteSelector()
        {
            _builder.Register<IBuildingSiteSelector, BuildingSiteSelector>(Lifetime.Scoped);
            _builder.Register<IProductionItemFactory, ProductionItemFactory>(Lifetime.Scoped);
            _builder.Register<IEntityFactory, EntityFactory>(Lifetime.Scoped);
            _builder.RegisterInstance(_itemPrefab);

            _builder.RegisterComponent(_itemInfoPopUp);
            _builder.RegisterInstance(_commandShowItemInfoPopUp).Keyed(VContainerKeys.CommandShowItemInfoPopUp);
        }

        private void RegisterMiniMapSystem()
        {
            _builder.Register<IEntityTrackersRegistry, EntityTrackersRegistry>(Lifetime.Scoped);
            _builder.Register<IPool<IconMapObject>, Pool<IconMapObject>>(Lifetime.Scoped);
            _builder.Register<IEntitySizeCalculator, WorldBoundsSizeCalculator>(Lifetime.Transient);
            _builder.Register<IEntityTrackerFactory, EntityTrackerFactory>(Lifetime.Scoped);
            _builder.Register<IFactory<IconMapObject>, IconMapObjectFactory>(Lifetime.Scoped);
            _builder.RegisterInstance(_iconMapObjectPrefab);
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
            _builder.Register<ICameraOrientationAdapter, GameSceneCameraOrientationAdapter>(Lifetime.Scoped);
            _builder.Register<IScreenSizeTracker, ScreenSizeTracker>(Lifetime.Scoped);
            _builder.Register<IScreenOrientationTracker, ScreenOrientationTracker>(Lifetime.Scoped);
            _builder.Register<ICameraZoom, CameraZoom>(Lifetime.Scoped);
            _builder.Register<ICameraDragger, CameraDragger>(Lifetime.Scoped);
            _builder.Register<IPositionRestrictor, PositionRestrictor>(Lifetime.Scoped);
            _builder.Register<IResistanceCalculator, ResistanceCalculator>(Lifetime.Scoped);
            _builder.Register<IDragApplier, DragApplier>(Lifetime.Scoped);
            _builder.Register<IInertiaSnapbackApplier, InertiaSnapbackApplier>(Lifetime.Scoped);
        }

        private void RegisterCameraInputReader()
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