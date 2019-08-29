﻿namespace CGJ.System
{
    public interface ISystems
    {
        CGJSceneLoadingSystem sceneLoadingSystem { get; set;}
        CGJEventsSystem eventsSystem { get; set; }
        UIManager uiManager { get; set; }
        CheckpointSystem checkpointSystem { get; set; }
    }
}