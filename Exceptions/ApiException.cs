namespace API.Exceptions
{
    public class ApiException
    {
        public int statusCode { get; set; }

        public string message { get; set; }

        public string detaiels { get; set; }
        public ApiException(int codeStatus, string Message , string Detailes )
        {

            statusCode = codeStatus;
            message = Message;
            detaiels = Detailes;

        }
      

    }
}
