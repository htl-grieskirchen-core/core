using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using AspNetCore.Totp;
using Core.WebUntis.Implementation.RequestTypes;
using Core.WebUntis.Implementation.ResponseTypes;
using Core.WebUntis.Interface;
using Core.WebUntis.Interface.Exceptions;
using Core.WebUntis.Interface.Types;
using Room = Core.WebUntis.Interface.Types.Room;

namespace Core.WebUntis.Implementation;

public class WebUntisClient : IWebUntisClient
{
    private readonly string _baseUrl;
    private string BaseUrlJsonRpc => _baseUrl + "/WebUntis/jsonrpc.do";
    private string BaseUrlJsonRpcIntern => _baseUrl + "/WebUntis/jsonrpc_intern.do";
    private string BaseUrlRest => _baseUrl + "/WebUntis";
    private readonly string _school;
    private readonly string _client;

    private readonly HttpClient _httpClient;
    private readonly CookieContainer _cookies;

    public WebUntisClient(
        string baseUrl,
        string school,
        string client,
        string? token = null
    )
    {
        _baseUrl = baseUrl;
        _school = school;
        _client = client;

        var baseAddress = new Uri(BaseUrlJsonRpc);

        _cookies = new CookieContainer();
        if (token != null)
        {
            _cookies.Add(baseAddress, new Cookie("JSESSIONID", token));
        }

        var handler = new HttpClientHandler { CookieContainer = _cookies };
        _httpClient = new HttpClient(handler);
    }

    public async Task<Authentication> Authenticate(string user, string password)
    {
        var authenticateResponse = await JsonRpcRequest<AuthenticateResponse>(
            "authenticate",
            new AuthenticateRequest
            {
                User = user,
                Password = password,
                Client = _client
            },
            new Dictionary<string, string> {
                {"school", _school}
            }
        );

        return authenticateResponse.Convert();
    }

    public async Task<Authentication> AuthenticateWithSecret(string user, string secret)
    {
        var otp = new TotpGenerator().Generate(secret);
        await Request(
            HttpMethod.Post,
            BaseUrlJsonRpcIntern,
            GetJsonRpcRequestContent(
                "getUserData2017",
                new object[] {
                    new AuthenticateWithSecretRequest {
                        Auth = new AuthenticateWithSecretRequestAuth {
                            ClientTime = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                            User = user,
                            Otp = otp.ToString()
                        }
                    }
                }
            ),
            new Dictionary<string, string> {
                {"school", _school}
            }
        );

        return new Authentication
        {
            Token = _cookies.GetCookies(new Uri(BaseUrlJsonRpcIntern))["JSESSIONID"]!.Value
        };
    }

    public async Task<List<Class>> GetClasses(int schoolYear)
    {
        var classesResponse = await JsonRpcRequest<ClassResponse[]>(
            "getKlassen",
            null,
            new Dictionary<string, string> {
                {"schoolYear", schoolYear.ToString()}
            }
        );
        return classesResponse
            .Select(x => x.Convert())
            .ToList();
    }

    public async Task<List<Room>> GetRooms()
    {
        var roomsResponse = await JsonRpcRequest<RoomResponse[]>(
            "getRooms"
        );
        return roomsResponse
            .Select(x => x.Convert())
            .ToList();
    }

    public async Task<List<Subject>> GetSubjects() //shows every subject from arche.webuntis.com
    {
        var subjectResponse = await JsonRpcRequest<SubjectResponse[]>("getSubjects");
        return subjectResponse
            .Select(x => x.Convert())
            .ToList();
    }
    public async Task<List<Holiday>> GetHolidays()
    {
        var holidayResponse = await JsonRpcRequest<HolidayResponse[]>("getHolidays");
        return holidayResponse
            .Select(x => x.Convert())
            .ToList();
    }

    private async Task<TResponse> JsonRpcRequest<TResponse>(
        string method,
        object? request = null,
        Dictionary<string, string>? urlParameters = null
    )
    {
        var response = await Request(
            HttpMethod.Post,
            BaseUrlJsonRpc,
            GetJsonRpcRequestContent(method, request),
            urlParameters
        );
        var responseJson = JsonNode.Parse(await response.Content.ReadAsStringAsync())!;

        var errorJson = responseJson["error"];
        if (errorJson != null)
        {
            var errorCode = int.Parse(errorJson["code"]!.ToString());

            if (errorCode == -8520)
            {
                throw new InvalidTokenException();
            }

            throw new NotImplementedException();
        }

        var resultJsonString = responseJson["result"]!.ToJsonString();
        return JsonSerializer.Deserialize<TResponse>(resultJsonString)!;
    }

    private async Task<TResponse> RestRequest<TResponse>(
        HttpMethod method,
        string path,
        object? request = null,
        Dictionary<string, string>? urlParameters = null
    )
    {
        var response = await Request(
            method,
            $"{BaseUrlRest}/{path}",
            GetRestRequestContent(request),
            urlParameters
        );

        if (!response.IsSuccessStatusCode)
        {
            var errorCode = response.StatusCode;

            throw new NotImplementedException();
        }

        return JsonSerializer.Deserialize<TResponse>(await response.Content.ReadAsStringAsync())!;
    }

    private async Task<HttpResponseMessage> Request(
        HttpMethod method,
        string url,
        HttpContent? httpContent = null,
        Dictionary<string, string>? urlParameters = null
    )
    {
        return await _httpClient.SendAsync(
            new HttpRequestMessage(method, GetUrlWithParameters(url, urlParameters))
            {
                Content = httpContent
            }
        );
    }

    private string GetUrlWithParameters(string url, Dictionary<string, string>? urlParameters = null)
    {
        return url + (urlParameters != null
            ? $"?{string.Join("&", urlParameters.Select(entry => $"{entry.Key}={entry.Value}"))}"
            : "");
    }

    private HttpContent GetJsonRpcRequestContent(
        string method,
        object? request = null
    )
    {
        var json = new JsonObject
        {
            ["method"] = method,
            ["id"] = 0,
            ["jsonrpc"] = "2.0"
        };

        if (request != null)
            json["params"] = JsonNode.Parse(JsonSerializer.Serialize(request));

        return new StringContent(json.ToJsonString(), Encoding.UTF8, "application/json");
    }

    private HttpContent GetRestRequestContent(
        object? request = null
    )
    {
        return new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
    }
}
