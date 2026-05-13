// Helpers/TaskExtensions.cs
namespace Traqtiv.Mobile.Helpers;

public static class TaskExtensions
{
    public static async void FireAndForget(this Task task)
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            await AlertHelper.ShowErrorAsync(ex.Message);
        }
    }
}