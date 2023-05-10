using LoyaltySystem.Models;
using System.Text.Json.Serialization;

public class Member
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int PurchasesTotalAmount { get; set; }

    public int Points { get; set; }
    public int Discount { get; set; }

    public Member()
    {
        Points = 0;
        Discount = 0;
    }
}