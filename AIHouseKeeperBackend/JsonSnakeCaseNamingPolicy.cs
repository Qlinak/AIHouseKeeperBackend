namespace AIHouseKeeperBackend;

using System.Text;
using System.Text.Json;

internal sealed class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }
        
        var sb = new StringBuilder();
        for (var i = 0; i < name.Length; i++)
        {
            if (i == 0 && char.IsUpper(name[i]))
            {
                sb.Append(char.ToLower(name[i]));
            }
            else if (i > 0 && char.IsUpper(name[i]))
            {
                sb.Append('_');
                sb.Append(char.ToLower(name[i]));
            }
            else
            {
                sb.Append(name[i]);
            }
        }

        return sb.ToString();
    }
}