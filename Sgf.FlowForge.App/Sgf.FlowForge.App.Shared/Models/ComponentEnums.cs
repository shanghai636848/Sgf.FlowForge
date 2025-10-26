// Models/ComponentEnums.cs
namespace Sgf.FlowForge.App.Shared.Models;

public enum ButtonVariant
{
    Primary,
    Secondary,
    Accent,
    Outline,
    Ghost,
    Link,
    Success,
    Warning,
    Error
}

public enum IconPosition
{
    Left,
    Right
}

public enum ButtonSize
{
    Small,      // h-6 (24px)
    Medium,     // h-7 (28px)
    Large       // h-8 (32px)
}

public enum InputSize
{
    Small,
    Medium,
    Large
}

public enum AlertType
{
    Success,
    Warning,
    Error,
    Info
}

public enum BadgeVariant
{
    Primary,
    Secondary,
    Success,
    Warning,
    Error,
    Info
}

public enum AvatarSize
{
    ExtraSmall,  // 1.5rem (24px)
    Small,       // 2rem (32px)
    Medium,      // 2.5rem (40px)
    Large,       // 3rem (48px)
    ExtraLarge   // 4rem (64px)
}


public class NavigationGroup
{
    public string? Title { get; set; }
    public List<MenuItem> Items { get; set; } = new();
}
public class MenuItem
{
    public string Icon { get; set; } = string.Empty;
    public string TitleKey { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Color { get; set; }
    public string? Badge { get; set; }
    public bool IsExpanded { get; set; }
    public List<MenuItem>? Children { get; set; }
}