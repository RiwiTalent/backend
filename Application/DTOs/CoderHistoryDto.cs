using System.Text.Json.Serialization;

namespace RiwiTalent.Application.DTOs
{
  public class CoderHistoryDto
  {
    public string Name { get; set; }

    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // public string? CoderId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Details>? CoderList {get; set;}

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Details>? GroupList {get; set;}
  }

  public class Details
  {
    public string Name {get; set;}
    public string Status {get; set;}

    public static Details CreateDetails(string name, string status)
    {
      return new()
      {
        Name = name,
        Status = status
      };  
    }
  }
}