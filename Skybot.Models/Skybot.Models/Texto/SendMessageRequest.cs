namespace Skybot.Models.Texto
{
    public class SendMessageRequest
    {
        public string FromNumber { get; set; }
        public string ToNumber { get; set; }
        public string Message { get; set; }
    }
}
