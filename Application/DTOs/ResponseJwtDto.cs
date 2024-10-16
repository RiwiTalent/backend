namespace RiwiTalent.Application.DTOs;

public class ResponseJwtDto
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public string Email { get; set; }
}