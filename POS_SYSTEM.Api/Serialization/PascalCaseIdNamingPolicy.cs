using System.Text.Json;

namespace POS_SYSTEM.Api.Serialization;

public class PascalCaseIdNamingPolicy : JsonNamingPolicy
{
    // Exceptions: these two are spelled with a lowercase 'd' in the original FastAPI schemas,
    // unlike every other Xxx*ID* field which uses a capital 'D'.
    private static readonly HashSet<string> LowercaseDExceptions = new()
    {
        "CustomerId",
        "SupplierId"
    };

    public override string ConvertName(string name)
    {
        if (LowercaseDExceptions.Contains(name))
            return name; // keep exactly as-is: "CustomerId", "SupplierId"

        if (name.EndsWith("Ids"))
            return name[..^3] + "IDs";

        if (name.EndsWith("Id"))
            return name[..^2] + "ID";

        return name;
    }
}