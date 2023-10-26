namespace MyBlog.Models.Common
{
    public class RequestBox<T>
    {
        public RequestBox(T body)
        {
            Body = body;
        }

        public string Signature { get; set; }

        public RequestHeader Header { get; set; }

        public T Body { get; set; }
    }
}
