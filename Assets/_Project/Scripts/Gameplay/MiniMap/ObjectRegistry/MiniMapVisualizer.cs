using System;
using System.Collections.Generic;
using BattleBase.Core;
using BattleBase.DI;
using BattleBase.Gameplay.CameraNavigation;
using UnityEngine;
using VContainer;

namespace BattleBase.Gameplay.MiniMap
{
    public class MiniMapVisualizer : MonoBehaviour, IInjectable
    {
        [SerializeField] private MiniMapArea _mapArea;
        [SerializeField] private RectTransform _context;

        private readonly Dictionary<IEntityTracker, IconMapObject> _icons = new();

        private IEntityTrackersRegistry _objectRegistry;
        private ICameraAreaService _areaService;
        private IPool<IconMapObject> _pool;
        private IIconSizeCalculator _calculator;

        [Inject]
        public void Construct(
            IEntityTrackersRegistry objectRegistry,
            ICameraAreaService areaService,
            IPool<IconMapObject> pool)
        {
            _objectRegistry = objectRegistry ?? throw new ArgumentNullException(nameof(objectRegistry));
            _areaService = areaService ?? throw new ArgumentNullException(nameof(areaService));
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));

            _calculator = _mapArea.Orientation == ScreenOrientationType.Portrait
                ? new PortraitIconSizeCalculator()
                : new LandscapeIconSizeCalculator();
        }

        private void OnEnable()
        {
            _objectRegistry.Added += OnTrackerAdded;
            _objectRegistry.Removed += OnTrackerRemoved;
            _areaService.Changed += OnAreaChanged;
            _mapArea.SizeChanged += OnMiniMapSizeChanged;
            RebuildAllIcons();
        }

        private void OnDisable()
        {
            _objectRegistry.Added -= OnTrackerAdded;
            _objectRegistry.Removed -= OnTrackerRemoved;
            _areaService.Changed -= OnAreaChanged;
            _mapArea.SizeChanged -= OnMiniMapSizeChanged;
            ClearAllIcons();
        }

        private void CreateIcon(IEntityTracker tracker)
        {
            if (_pool.TryGive(out IconMapObject icon))
            {
                icon.SetParent(_context);
                icon.ResetScale();
                icon.SetColor(tracker.Color);
                icon.Show();
                _icons[tracker] = icon;
                UpdateIconSize(icon, tracker.WorldSize);
                UpdateIconPosition(tracker, icon);
                UpdateIconRotation(tracker, icon);

                tracker.ColorChanged += OnColorChanged;
                tracker.SizeChanged += OnSizeChanged;
                tracker.PositionChanged += OnPositionChanged;
                tracker.RotationChanged += OnRotationChanged;
            }
        }

        private void ClearAllIcons()
        {
            List<IEntityTracker> trackersToRemove = new(_icons.Keys);

            foreach (IEntityTracker tracker in trackersToRemove)
                ClearIcon(tracker);

            _icons.Clear();
        }

        private void ClearIcon(IEntityTracker tracker)
        {
            if (_icons.TryGetValue(tracker, out IconMapObject icon))
            {
                tracker.ColorChanged -= OnColorChanged;
                tracker.SizeChanged -= OnSizeChanged;
                tracker.PositionChanged -= OnPositionChanged;
                tracker.RotationChanged -= OnRotationChanged;

                icon.Deactivate();
                _icons.Remove(tracker);
            }
        }

        private void RecalculateAllIconSizes()
        {
            foreach (var kvp in _icons)
            {
                UpdateIconSize(kvp.Value, kvp.Key.WorldSize);
                UpdateIconPosition(kvp.Key, kvp.Value);
            }
        }

        private void UpdateIconSize(IconMapObject icon, Vector2 worldSize)
        {
            Bounds bounds = _areaService.AreaBounds;
            Rect rect = _mapArea.Rect;

            Vector2 size = _calculator.GetIconSize(worldSize, bounds, rect);
            icon.SetSize(size);
        }

        private void UpdateIconPosition(IEntityTracker tracker, IconMapObject icon)
        {
            Bounds bounds = _areaService.AreaBounds;
            Rect rect = _mapArea.Rect;

            Vector2 uiPosition = _calculator.WorldToMiniMapPosition(tracker.WorldPosition, bounds, rect);
            icon.SetAnchoredPosition(uiPosition);
        }

        private void UpdateIconRotation(IEntityTracker tracker, IconMapObject icon) =>
            icon.SetRotation(tracker.RotationY);

        private void OnTrackerAdded(IEntityTracker tracker) =>
            CreateIcon(tracker);

        private void OnTrackerRemoved(IEntityTracker tracker) =>
            ClearIcon(tracker);

        private void OnAreaChanged() =>
            RecalculateAllIconSizes();

        private void OnMiniMapSizeChanged() =>
            RecalculateAllIconSizes();

        private void RebuildAllIcons()
        {
            ClearAllIcons();

            foreach (IEntityTracker tracker in _objectRegistry.Trackers)
                CreateIcon(tracker);
        }

        private void OnColorChanged(IEntityTracker tracker)
        {
            if (_icons.TryGetValue(tracker, out IconMapObject icon))
                icon.SetColor(tracker.Color);
        }

        private void OnSizeChanged(IEntityTracker tracker)
        {
            if (_icons.TryGetValue(tracker, out IconMapObject icon))
                UpdateIconSize(icon, tracker.WorldSize);
        }

        private void OnPositionChanged(IEntityTracker tracker)
        {
            if (_icons.TryGetValue(tracker, out IconMapObject icon))
                UpdateIconPosition(tracker, icon);
        }

        private void OnRotationChanged(IEntityTracker tracker)
        {
            if (_icons.TryGetValue(tracker, out IconMapObject icon))
                UpdateIconRotation(tracker, icon);
        }
    }
}