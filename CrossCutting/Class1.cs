namespace CrossCutting
{
    public class ResultObject<T>
    {
        public T? Content { get; set; }

        public ErrorEventArgs? ErrorResponse { get; set; }

        public bool IsError { get; set; }

        public ResultObject(T content)
        {
            Content = content;
            IsError = false;

        }

    }
}
