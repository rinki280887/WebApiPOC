namespace WebApiPOC.OAuth.Interface
{
    public interface ITokenManager
    {
        string CreateToken(string userName);
    }
}
