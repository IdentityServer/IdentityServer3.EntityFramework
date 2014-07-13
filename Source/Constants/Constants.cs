namespace Sample
{
    public static class Constants
    {
        public const string BaseAddress = "http://localhost:3334/core";
        
        public const string AuthorizeEndpoint = BaseAddress + "/connect/authorize";
        public const string LogoutEndpoint = BaseAddress + "/connect/logout";
        public const string TokenEndpoint = BaseAddress + "/connect/token";

        public const string AspNetWebApiSampleApi = "http://localhost:2728/";
    }
}