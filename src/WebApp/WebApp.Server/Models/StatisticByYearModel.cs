namespace WebApp.Server.Models;

public class StatisticByYearModel
{
    public int Year { get; set; }

    public int CreatedAtCount { get; set; }

    public int UpdatedAtCount { get; set; }

    public int ExpiredAtCount { get; set; }
}
