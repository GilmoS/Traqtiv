namespace Traqtiv.Mobile.Helpers;

public static class AppConstants
{
    // the sever address
    public const string ApiBaseUrl = "http://10.0.2.2:5000/api/";

    public const string TokenKey = "traqtiv_token"; // the key used to store the JWT token in secure storage

    // the routes used for navigation in the app
    public static class Routes
    {
        public const string Login = "//login";
        public const string Register = "//register";
        public const string Home = "//home";
        public const string Workouts = "workouts";
        public const string AddWorkout = "addworkout";
        public const string DailyActivity = "dailyactivity";
        public const string BodyMetrics = "bodymetrics";
        public const string Recommendations = "recommendations";
        public const string Profile = "profile";
    }
}