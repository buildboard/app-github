namespace BB.App.Github.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Commands;
    using Constants;
    using ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    [Route("[controller]")]
    [ApiVersion("1.0")]
    public class CarsController : ControllerBase
    {
        private readonly Lazy<IDeleteCarCommand> _deleteCarCommand;
        private readonly Lazy<IGetCarCommand> _getCarCommand;
        private readonly Lazy<IGetCarPageCommand> _getCarPageCommand;
        private readonly Lazy<IPatchCarCommand> _patchCarCommand;
        private readonly Lazy<IPostCarCommand> _postCarCommand;
        private readonly Lazy<IPutCarCommand> _putCarCommand;

        public CarsController(
            Lazy<IDeleteCarCommand> deleteCarCommand,
            Lazy<IGetCarCommand> getCarCommand,
            Lazy<IGetCarPageCommand> getCarPageCommand,
            Lazy<IPatchCarCommand> patchCarCommand,
            Lazy<IPostCarCommand> postCarCommand,
            Lazy<IPutCarCommand> putCarCommand)
        {
            _deleteCarCommand = deleteCarCommand;
            _getCarCommand = getCarCommand;
            _getCarPageCommand = getCarPageCommand;
            _patchCarCommand = patchCarCommand;
            _postCarCommand = postCarCommand;
            _putCarCommand = putCarCommand;
        }

        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        /// <response code="200">The allowed HTTP methods.</response>
        [HttpOptions]
        public IActionResult Options()
        {
            HttpContext.Response.Headers.Add(
                "Allow",
                string.Join(",", new string[]
                {
                    HttpMethods.Get,
                    HttpMethods.Head,
                    HttpMethods.Options,
                    HttpMethods.Post
                }));
            return Ok();
        }

        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods for a car with the specified unique identifier.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        /// <response code="200">The allowed HTTP methods.</response>
        [HttpOptions("{carId}")]
        public IActionResult Options(int carId)
        {
            HttpContext.Response.Headers.Add(
                "Allow",
                string.Join(",", new string[]
                {
                    HttpMethods.Delete,
                    HttpMethods.Get,
                    HttpMethods.Head,
                    HttpMethods.Options,
                    HttpMethods.Patch,
                    HttpMethods.Post,
                    HttpMethods.Put
                }));
            return Ok();
        }

        /// <summary>
        /// Deletes the car with the specified ID.
        /// </summary>
        /// <param name="carId">The car ID.</param>
        /// <returns>A 204 No Content response if the car was deleted or a 404 Not Found if a car with the specified ID
        /// was not found.</returns>
        /// <response code="204">The car with the specified ID was deleted.</response>
        /// <response code="404">A car with the specified ID was not found.</response>
        [HttpDelete("{carId}", Name = CarsControllerRoute.DeleteCar)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public Task<IActionResult> Delete(int carId) =>
            _deleteCarCommand.Value.ExecuteAsync(carId);

        /// <summary>
        /// Gets the car with the specified ID.
        /// </summary>
        /// <param name="carId">The car ID.</param>
        /// <returns>A 200 OK response containing the car or a 404 Not Found if a car with the specified ID was not
        /// found.</returns>
        /// <response code="200">The car with the specified ID.</response>
        /// <response code="404">A car with the specified ID could not be found.</response>
        [HttpGet("{carId}", Name = CarsControllerRoute.GetCar)]
        [HttpHead("{carId}")]
        [ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public Task<IActionResult> Get(int carId) =>
            _getCarCommand.Value.ExecuteAsync(carId);

        /// <summary>
        /// Gets a collection of cars using the specified page number and number of items per page.
        /// </summary>
        /// <param name="pageOptions">The page options.</param>
        /// <returns>A 200 OK response containing a collection of cars, a 400 Bad Request if the page request
        /// parameters are invalid or a 404 Not Found if a page with the specified page number was not found.
        /// </returns>
        /// <response code="200">A collection of cars for the specified page.</response>
        /// <response code="400">The page request parameters are invalid.</response>
        /// <response code="404">A page with the specified page number was not found.</response>
        [HttpGet("", Name = CarsControllerRoute.GetCarPage)]
        [HttpHead("")]
        [ProducesResponseType(typeof(PageResult<Car>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetPage([FromQuery] PageOptions pageOptions) =>
            _getCarPageCommand.Value.ExecuteAsync(pageOptions);

        /// <summary>
        /// Patches the car with the specified ID.
        /// </summary>
        /// <param name="carId">The car ID.</param>
        /// <param name="patch">The patch document. See http://jsonpatch.com/.</param>
        /// <returns>A 200 OK if the car was patched, a 400 Bad Request if the patch was invalid or a 404 Not Found
        /// if a car with the specified ID was not found.</returns>
        /// <response code="200">The patched car with the specified ID.</response>
        /// <response code="400">The patch document is invalid.</response>
        /// <response code="404">A car with the specified ID could not be found.</response>
        [HttpPatch("{carId}", Name = CarsControllerRoute.PatchCar)]
        [ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public Task<IActionResult> Patch(int carId, [FromBody] JsonPatchDocument<SaveCar> patch) =>
            _patchCarCommand.Value.ExecuteAsync(carId, patch);

        /// <summary>
        /// Creates a new car.
        /// </summary>
        /// <param name="car">The car to create.</param>
        /// <returns>A 201 Created response containing the newly created car or a 400 Bad Request if the car is
        /// invalid.</returns>
        /// <response code="201">The car was created.</response>
        /// <response code="400">The car is invalid.</response>
        [HttpPost("", Name = CarsControllerRoute.PostCar)]
        [ProducesResponseType(typeof(Car), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Post([FromBody] SaveCar car) =>
            _postCarCommand.Value.ExecuteAsync(car);

        /// <summary>
        /// Updates an existing car with the specified ID.
        /// </summary>
        /// <param name="carId">The car identifier.</param>
        /// <param name="car">The car to update.</param>
        /// <returns>A 200 OK response containing the newly updated car, a 400 Bad Request if the car is invalid or a
        /// or a 404 Not Found if a car with the specified ID was not found.</returns>
        /// <response code="200">The car was updated.</response>
        /// <response code="400">The car is invalid.</response>
        /// <response code="404">A car with the specified ID could not be found.</response>
        [HttpPut("{carId}", Name = CarsControllerRoute.PutCar)]
        [ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public Task<IActionResult> Put(int carId, [FromBody] SaveCar car) =>
            _putCarCommand.Value.ExecuteAsync(carId, car);
    }
}
