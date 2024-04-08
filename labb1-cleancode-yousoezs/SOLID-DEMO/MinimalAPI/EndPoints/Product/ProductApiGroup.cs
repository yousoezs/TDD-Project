using FastEndpoints;

namespace Server.MinimalAPI.EndPoints.Product
{
    public class ProductApiGroup : SubGroup<ApiGroup>
    {
        public ProductApiGroup()
        {
            Configure("/products", ep =>
                {
                    ep.Description(x => x
                    .Produces(401)
                    .WithTags("Product"));
                });

        }
    }
}
