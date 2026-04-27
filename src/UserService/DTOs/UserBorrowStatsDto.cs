namespace UserService.DTOs;

public class UserBorrowStatsDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BorrowCount { get; set; }
}
