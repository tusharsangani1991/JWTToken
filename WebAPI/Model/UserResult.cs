namespace WebAPI.Model
{
    public class UserResult
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string GroupName { get; set; }
        public string Accesstoken { get; set; }
        public string Refreshtoken { get; set; }
    }
    public class ApiAuthLoginRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
