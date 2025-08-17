using System;
using System.Data.SqlClient;

public class DynamicPricingService
{
    private string connectionString;

    public DynamicPricingService(string dbConnectionString)
    {
        connectionString = dbConnectionString;
    }

    public void DisplayCurrentPrice(int slotId)
    {
        string query = @"
            SELECT TOP 1 Price
            FROM DynamicPricing
            WHERE SlotID = @SlotID AND EffectiveTime <= @Now
            ORDER BY EffectiveTime DESC";

        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@SlotID", slotId);
            cmd.Parameters.AddWithValue("@Now", DateTime.Now);

            conn.Open();
            var result = cmd.ExecuteScalar();

            if (result != null)
            {
                Console.WriteLine($"Current price for slot {slotId}: R{result}");
            }
            else
            {
                Console.WriteLine("No price found for this slot.");
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // TODO: Replace with your actual connection string if you want🥲
        string connectionString = "Data Source=LAPTOP-79D94187\\SQLEXPRESS;Initial Catalog=MabasoDB;Integrated Security=True";
        var pricingService = new DynamicPricingService(connectionString);

        Console.Write("Enter Slot ID: ");
        if (int.TryParse(Console.ReadLine(), out int slotId))
        {
            pricingService.DisplayCurrentPrice(slotId);
        }
        else
        {
            Console.WriteLine("Invalid Slot ID.");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}