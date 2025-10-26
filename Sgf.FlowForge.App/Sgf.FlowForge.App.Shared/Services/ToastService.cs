// Services/ToastService.cs
namespace Sgf.FlowForge.App.Shared.Services;

public class ToastService
{
    public event Action<ToastMessage>? OnShow;

    public void ShowSuccess(string message, int duration = 3000)
    {
        Show(new ToastMessage
        {
            Type = ToastType.Success,
            Message = message,
            Duration = duration
        });
    }

    public void ShowError(string message, int duration = 5000)
    {
        Show(new ToastMessage
        {
            Type = ToastType.Error,
            Message = message,
            Duration = duration
        });
    }

    public void ShowWarning(string message, int duration = 4000)
    {
        Show(new ToastMessage
        {
            Type = ToastType.Warning,
            Message = message,
            Duration = duration
        });
    }

    public void ShowInfo(string message, int duration = 3000)
    {
        Show(new ToastMessage
        {
            Type = ToastType.Info,
            Message = message,
            Duration = duration
        });
    }

    private void Show(ToastMessage message)
    {
        OnShow?.Invoke(message);
    }
}

public class ToastMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public ToastType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Duration { get; set; }
}

public enum ToastType
{
    Success,
    Error,
    Warning,
    Info
}