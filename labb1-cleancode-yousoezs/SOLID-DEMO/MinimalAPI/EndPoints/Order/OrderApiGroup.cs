using FastEndpoints;

namespace Server.MinimalAPI.EndPoints.Order;
public class OrderApiGroup : SubGroup<ApiGroup>
{
    public OrderApiGroup()
    {
        Configure("/orders", ep =>
        {
            ep.Description(x => x
                .Produces(401)
                .WithTags("Order"));
        });
    }
}
