﻿using FN.ViewModel.Helper;

namespace FN.ViewModel.Systems.User
{
    public class RegisterDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    public class RegisterResponse
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
    public class LoginDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
    public class LoginResponse
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DeviceInfo DeviceInfo { get; set; } = new DeviceInfo();
    }
}
