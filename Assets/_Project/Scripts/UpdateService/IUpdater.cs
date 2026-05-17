using System;

namespace BattleBase.UpdateService
{
    public interface IUpdater
    {
        IUpdater Subscribe(Action<float> handler, UpdateType updateType);

        IUpdater Subscribe(Action handler, UpdateType updateType);

        IUpdater Unsubscribe(Action<float> handler, UpdateType updateType);

        IUpdater Unsubscribe(Action handler, UpdateType updateType);

        IUpdater DebugPrint();
    }
}