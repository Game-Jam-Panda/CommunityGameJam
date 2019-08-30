namespace CGJ.System
{
    public interface ISystems
    {
        CGJSceneLoadingSystem sceneLoadingSystem { get; set;}
        CGJEventsSystem eventsSystem { get; set; }
        UIManager uiManager { get; set; }
        CheckpointSystem checkpointSystem { get; set; }
        SoundManager soundManager { get; set; }
        MusicManager musicManager { get; set; }
        NarratorSystem narratorSystem { get; set; }
        PickupSystem pickupSystem { get; set; }
    }
}