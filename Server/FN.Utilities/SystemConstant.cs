namespace FN.Utilities
{
    public struct SystemConstant
    {
        public const string DB_CONNECTION_STRING = "MySQL";
        public const string REDIS_CONNECTION_STRING = "Redis";
        public const string MONGODB_SETTING = "MongoDBSettings";
        public const string SMTP_SETTINGS = "MailJet";

        public const string MESSAGE_PATTERN_EVENT = "myChannel:*";
        public const string MESSAGE_REGISTER_EVENT = "myChannel:RegisterEvent";
        public const string MESSAGE_LOGIN_EVENT = "myChannel:LoginEvent";

        public const string TEMPLATE_ORDER_ID = "6705985";
        public const string TEMPLATE_WELCOME_ID = "6710966";
        public const string TEMPLATE_WARNING_ID = "6710973";

        public const string AVATAR_DEFAULT = "https://res.cloudinary.com/dje3seaqj/image/upload/v1736989161/gatapchoi_biglrl.jpg";
    }
}
