namespace GeoSensePlus.Core.ServiceModels;

public class UserInfo
{
    public string CognitoUserPoolId { get; set; }
    public string CognitoUserName { get; set; }

    /// <summary>
    /// ASP.NET Core Identity user ID
    /// </summary>
    public string IdentityUserId { get; set; }
}
