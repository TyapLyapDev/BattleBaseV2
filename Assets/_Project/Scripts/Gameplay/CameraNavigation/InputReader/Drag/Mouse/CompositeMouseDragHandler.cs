using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class CompositeMouseDragHandler : IDragHandler
    {
        private readonly IMouseDragHandler _mouseDragHandler;
        private readonly IKeyboardDragHandler _keyboardDragHandler;

        public CompositeMouseDragHandler(
            IMouseDragHandler mouseDragHandler,
            IKeyboardDragHandler keyboardDragHandler)
        {
            _mouseDragHandler = mouseDragHandler ?? throw new ArgumentNullException(nameof(mouseDragHandler));
            _keyboardDragHandler = keyboardDragHandler ?? throw new ArgumentNullException(nameof(keyboardDragHandler));
        }

        public Vector3? Update(float deltaTime)
        {
            Vector3? mouseDelta = _mouseDragHandler.Update();

            if (mouseDelta.HasValue)
                return mouseDelta;

            return _keyboardDragHandler.Update(deltaTime);
        }
    }
}