namespace Game2D.Core;

public class Animation
{
    public string Name { get; }
    public AnimationFrame[] Frames { get; }
    public bool IsLooping { get; }
    public float TotalDuration { get; }

    public Animation(string name, AnimationFrame[] frames, bool isLooping = true)
    {
        Name = name;
        Frames = frames;
        IsLooping = isLooping;
        TotalDuration = 0f;
        foreach (var f in frames)
            TotalDuration += f.Duration;
    }
}
