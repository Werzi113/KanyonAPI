using JWT.Algorithms;
using JWT.Builder;
using Newtonsoft.Json;
using System.Text.Json;

namespace WebApplication1.Services
{
    public class TokensService
    {
        const string PASSWORD = "CHICKEN";

        public string Create(int id)
        {
            return JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm())
                      .WithSecret(PASSWORD)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim("userID", id)
                      .Encode();
        }

        public bool Verify(string header)
        {
            try
            {
                if (header == null)
                {
                    return false;
                }

                string[] parts = header.Split(' ');

                if (parts.Length != 2)
                {
                    return false;
                }

                var payload = JwtBuilder.Create()
                            .WithAlgorithm(new HMACSHA256Algorithm())
                            .WithSecret(PASSWORD)
                            .MustVerifySignature()
                            .Decode<IDictionary<string, object>>(parts[1]);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int? GetUserIDFromToken(string header)
        {
            try
            {
                if (header == null)
                {
                    return null;
                }

                string[] parts = header.Split(' ');

                if (parts.Length != 2)
                {
                    return null;
                }

                var payload = JwtBuilder.Create()
                            .WithAlgorithm(new HMACSHA256Algorithm())
                            .WithSecret(PASSWORD)
                            .MustVerifySignature()
                            .Decode<IDictionary<string, object>>(parts[1]);

                JsonElement id = (JsonElement)payload["userID"];

                return id.GetInt32();
            }
            catch
            {
                return null;
            }
        }
    }
}
