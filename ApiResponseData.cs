public class ApiResponseData {
    
    public bool success { get; set; }
    public string token { get; set; }
}

public class ApiResponseData<T> : ApiResponseData {
    public T data { get; set; }
}

