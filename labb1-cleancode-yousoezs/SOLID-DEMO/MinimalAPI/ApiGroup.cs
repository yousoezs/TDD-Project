using FastEndpoints;

namespace Server.MinimalAPI
{
    public class ApiGroup : Group
    {
        public ApiGroup()
        {
            Configure("/api", ep =>
            {
                ep.Description(x => x
                    .Produces(401)
                    .WithTags("API"));
            });
        }
    }
}
