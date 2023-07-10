namespace ChatApp.Shared.Requests.RequestFeatures;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; private set; }

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData
        {
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };

        AddRange(items);
    }

    public static PagedList<T> ToPagedList(IEnumerable<T> source, int count, int pageNumber, 
        int pageSize)
    {
        //var items = source
        //    .Skip((pageNumber - 1) * pageSize)
        //    .Take(pageSize)
        //    .ToList();

        return new PagedList<T>(source.ToList(), count, pageNumber, pageSize);
    }
}
