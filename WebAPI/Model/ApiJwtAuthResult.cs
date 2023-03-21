﻿namespace WebAPI.Model
{
    public class ApiJwtAuthResult
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }

        public string EncryptedToken { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpiryTime { get; set; }
    }
}
