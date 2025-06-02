using GlobalAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace GlobalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeoSearchController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _geoapifyApiKey;

        public GeoSearchController(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _geoapifyApiKey = config["Geoapify:ApiKey"] ?? throw new ArgumentNullException("API key not found");
        }

        [HttpPost("search-location")]
        public async Task<ActionResult<ResponseDto>> SearchByLocation([FromBody] GeoSearchRequestDto request)
        {
            var responseDto = new ResponseDto();

            try
            {
                if (request.Latitude == 0 || request.Longitude == 0)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Error = "Latitude and Longitude are required.";
                    return BadRequest(responseDto);
                }

                const int radius = 1000; // 1 km radius
                string categoryEncoded = Uri.EscapeDataString(request.Category);

                // Construct the request URL with both the circle and bias filters
                string url = $"https://api.geoapify.com/v2/places?categories={categoryEncoded}&filter=circle:{request.Longitude},{request.Latitude},{radius}&bias=proximity:{request.Longitude},{request.Latitude}&limit=20&apiKey={_geoapifyApiKey}";

                responseDto.RequestUrl = url; // Save request URL to return in response

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Error = "Geoapify API request failed.";
                    return StatusCode((int)response.StatusCode, responseDto);
                }

                var content = await response.Content.ReadAsStringAsync();
                var resultJson = JsonSerializer.Deserialize<object>(content);

                responseDto.Result = resultJson;
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Error = ex.Message;
                return StatusCode(500, responseDto);
            }
        }


        [HttpPost("search-place")]
        public async Task<ActionResult<ResponseDto>> SearchByPlaceId([FromBody] GeoSearchByPlaceIdDto request)
        {
            var responseDto = new ResponseDto();

            try
            {
                string url = $"https://api.geoapify.com/v2/places?filter=place:{request.PlaceId}&limit=1&apiKey={_geoapifyApiKey}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Error = "Geoapify API request failed.";
                    return StatusCode((int)response.StatusCode, responseDto);
                }

                var content = await response.Content.ReadAsStringAsync();
                var resultJson = JsonSerializer.Deserialize<object>(content);

                responseDto.Result = resultJson;
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Error = ex.Message;
                return StatusCode(500, responseDto);
            }
        }
    }
}



