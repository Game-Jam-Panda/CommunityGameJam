using System;
using CGJ.Core;
using UnityEngine;

namespace CGJ.System
{
    public class CheckpointSystem: MonoBehaviour
    {
        Checkpoint lastCheckpoint;

        public event Action onCheckpointUpdate;

        public void UpdateCheckpoint(Checkpoint newCheckpoint)
        {
            lastCheckpoint = newCheckpoint;
            SystemManager.systems.eventsSystem.ActivateCheckpointLantern(newCheckpoint);
            onCheckpointUpdate();
        }
    }
}