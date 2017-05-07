namespace BB.App.Github.ViewModelSchemaFilters
{
    using System.Collections.Generic;
    using BB.App.Github.ViewModels;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class PageResultCarSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema model, SchemaFilterContext context)
        {
            model.Default = new PageResult<Car>()
            {
                Count = 2,
                Items = new List<Car>()
                {
                    new Car()
                    {
                        CarId = 1,
                        Cylinders = 6,
                        Make = "Honda",
                        Model = "Civic"
                    },
                    new Car()
                    {
                        CarId = 2,
                        Cylinders = 8,
                        Make = "Lambourghini",
                        Model = "Countach"
                    }
                },
                Page = 1,
                Total = 10
            };
        }
    }
}
