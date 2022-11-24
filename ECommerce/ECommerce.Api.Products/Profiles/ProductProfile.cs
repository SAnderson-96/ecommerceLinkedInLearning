using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Profiles
{
    public class ProductProfile : AutoMapper.Profile
    {
        //map product model to product entity (DB)

        public ProductProfile()
        {
            CreateMap<Db.Product, Models.Product>();
        }
    }
}