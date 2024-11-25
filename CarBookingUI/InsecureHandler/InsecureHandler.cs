namespace CarBookingUI.InsecureHandler
{
    internal static class InsecureHandler
    {
        public static HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler(); 
            handler.ServerCertificateCustomValidationCallback = (HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors) => true; 
            return handler;
        }
    }
}
