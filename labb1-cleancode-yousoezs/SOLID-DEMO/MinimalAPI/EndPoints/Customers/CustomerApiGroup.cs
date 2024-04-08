using FastEndpoints;

namespace Server.MinimalAPI.EndPoints.Customers
{
    public class CustomerApiGroup : SubGroup<ApiGroup>
    {
        public CustomerApiGroup()
        {
            Configure("/customers", ep =>
            {
                ep.Description(x => x
                    .Produces(401)
                    .WithTags("Customer"));
            });
        }
    }
}
