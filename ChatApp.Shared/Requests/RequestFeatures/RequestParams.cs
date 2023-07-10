namespace ChatApp.Shared.Requests.RequestFeatures;

public abstract class RequestParams
{
    private const int _maxPageSize = 100;
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 50;

    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = (value > _maxPageSize) ? _maxPageSize : value; }
    }
}
