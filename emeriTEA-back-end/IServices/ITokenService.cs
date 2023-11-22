namespace emeriTEA_back_end.IServices
{
    public interface ITokenService
    {
        int? ExtractUserIdFromToken(HttpContext httpContext);
        int? ExtractUserIdFromAuthorizationHeader(HttpContext httpContext);
    }
}
