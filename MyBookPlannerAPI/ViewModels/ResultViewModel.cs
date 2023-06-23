namespace MyBookPlannerAPI.ViewModels
{
    // Is used to have a standard return in the API.
    // Using this will make the API always return a JSON with data and errors.
    public class ResultViewModel<T>
    {
        public T Data { get; private set; }

        public List<string> Errors { get; private set; } = new List<string>();

        public ResultViewModel(T data, List<string> errors)
        {
            Data = data;
            Errors = errors;
        }

        public ResultViewModel(T data)
        {
            Data = data;
        }

        public ResultViewModel(List<string> errors)
        {
            Errors = errors;
        }

        public ResultViewModel(string error)
        {
            Errors.Add(error);
        }
    }
}