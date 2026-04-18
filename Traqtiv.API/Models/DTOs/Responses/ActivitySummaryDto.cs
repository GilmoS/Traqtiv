namespace Traqtiv.API.Models.DTOs.Responses
{
    public class ActivitySummaryDto : BaseResponseDto
    {
    public int TotalSteps { get; set; }
    public int TotalCaloriesBurned { get; set; }
    public int TotalActiveMinutes { get; set; }
    public double TotalDistanceKm { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    }
}
