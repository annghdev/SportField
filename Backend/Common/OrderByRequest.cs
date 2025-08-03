namespace Common;

public class OrderByRequest
{
    public IEnumerable<OrderByOption> Options { get; set; } = [];
}
public record OrderByOption(string column, bool asc = true);