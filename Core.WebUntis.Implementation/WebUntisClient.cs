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
using Holiday = Core.WebUntis.Interface.Types.Holiday;
using Homework = Core.WebUntis.Interface.Types.Homework;
using Room = Core.WebUntis.Interface.Types.Room;

namespace Core.WebUntis.Implementation;

public class WebUntisClient : IWebUntisClient
{
    private readonly string _baseUrl;
    private Uri BaseUri => new(_baseUrl);
    private string BaseUrlJsonRpc => _baseUrl + "/WebUntis/jsonrpc.do";
    private string BaseUrlJsonRpcIntern => _baseUrl + "/WebUntis/jsonrpc_intern.do";
    private string BaseUrlRest => _baseUrl + "/WebUntis/api";
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

        _cookies = new CookieContainer();

        if (token != null)
        {
            AddToken(token);
        }

        var handler = new HttpClientHandler { CookieContainer = _cookies };
        _httpClient = new HttpClient(handler);
    }

    private void AddToken(string token)
    {
        _cookies.Add(BaseUri, new Cookie("JSESSIONID", token));
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

        var authentication = authenticateResponse.Convert();

        AddToken(authentication.Token);

        return authentication;
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

        var authentication = new Authentication
        {
            Token = _cookies.GetCookies(new Uri(BaseUrlJsonRpcIntern))["JSESSIONID"]!.Value
        };

        AddToken(authentication.Token);

        return authentication;
    }

    public async Task<IEnumerable<Class>> GetClasses(int schoolYear)
    {
        var classesResponse = await JsonRpcRequest<ClassResponse[]>(
            "getKlassen",
            null,
            new Dictionary<string, string>
            {
                {"schoolYear", schoolYear.ToString()}
            }
        );
        return classesResponse
            .Select(x => x.Convert());
    }

    public async Task<IEnumerable<Room>> GetRooms()
    {
        var roomsResponse = await JsonRpcRequest<RoomResponse[]>(
            "getRooms"
        );
        return roomsResponse
            .Select(x => x.Convert());
    }

    public async Task<IEnumerable<Teacher>> GetTeachers()
    {
        var teachers = File.ReadLines("CSV/teachers.csv")
            .Skip(1)
            .Select(x => x.Split(";"))
            .Select(x => new Teacher
            {
                Id = Int32.Parse(x[0]),
                Name = x[1],
                LastName = x[2],
                FirstName = x[3]
            }
            );
        return teachers;
    }

    public async Task<IEnumerable<Homework>> GetHomeworks(DateTime startDate, DateTime endDate)
    {
        var homeworkResponse = await RestRequest<HomeworkResponse>(
            method: HttpMethod.Get,
            path: $"/homeworks/lessons",
            urlParameters: new Dictionary<string, string>
            {
                {"startDate", UntisDateTimeMethods.ConvertDateToUntisDate(startDate).ToString()},
                {"endDate", UntisDateTimeMethods.ConvertDateToUntisDate(endDate).ToString()}
            }
        );
        return homeworkResponse.Convert();
    }

    public async Task<IEnumerable<Holiday>> GetHolidays()
    {
        var holidayResponse = await JsonRpcRequest<HolidayResponse[]>(
            method: "getHolidays"
        );
        return holidayResponse.Select(x => x.Convert());
    }

    public async Task<IEnumerable<Subject>> GetSubjects() //shows every subject from arche.webuntis.com
    {
        var subjectResponse = await JsonRpcRequest<SubjectResponse[]>("getSubjects");
        return subjectResponse
            .Select(x => x.Convert());
    }

    public async Task<IEnumerable<Timetable>> GetTimetable(ElementType type, int? personId, DateTime startDate,
        DateTime endDate)
    {
        var timetableResponse = await JsonRpcRequest<TimetableResponse[]>("getTimetable",
            new TimetableRequest
            {
                Type = (int)type,
                Id = personId,
                StartDate = UntisDateTimeMethods.ConvertDateToUntisDate(startDate),
                EndDate = UntisDateTimeMethods.ConvertDateToUntisDate(endDate)
            },
            new Dictionary<string, string>
            {
                {"school", _school}
            });
        return timetableResponse.Select(x => x.Convert());
    }

    [Obsolete]
    public Task<IEnumerable<ExamType>> GetExamTypes()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Exam>> GetExams(int examTypeId, DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Student>> GetStudents()
    {
        var classes = await GetClasses(2022);
        var teachers = await GetTeachers();

        var students = File.ReadLines("CSV/students.csv")
            .Skip(1)
            .Select(x => x.Split(";"))
            .Select(student => new Student()
            {
                ClassId = classes.Single(x => x.Name == student[0].ToUpper()).Id,
                ClassTeacherId = teachers.Single(x => $"{x.FirstName} {x.LastName}" == student[1]).Id,
                LastName = student[2],
                FirstName = student[3],
                Email = student[4]
            }
            )
            .ToList();
        return students;
    }

    private async Task<TResponse> JsonRpcRequest<TResponse>(
        string method,
        object? body = null,
        Dictionary<string, string>? urlParameters = null
    )
    {
        var response = await Request(
            HttpMethod.Post,
            BaseUrlJsonRpc,
            GetJsonRpcRequestContent(method, body),
            urlParameters
        );
        var responseJson = JsonNode.Parse(await response.Content.ReadAsStringAsync())!;

        var errorJson = responseJson["error"];
        if (errorJson != null)
        {
            var errorCode = int.Parse(errorJson["code"]!.ToString());

            throw errorCode switch
            {
                -8520 => new InvalidTokenException(),
                -8509 => new InsufficientRightsException(),
                _ => new NotImplementedException()
            };
        }

        var resultJsonString = responseJson["result"]!.ToJsonString();
        return JsonSerializer.Deserialize<TResponse>(resultJsonString)!;
    }

    private async Task<TResponse> RestRequest<TResponse>(
        HttpMethod method,
        string path,
        object? body = null,
        Dictionary<string, string>? urlParameters = null
    )
    {
        var response = await Request(
            method,
            $"{BaseUrlRest}/{path}",
            GetRestRequestContent(body),
            urlParameters
        );

        if (!response.IsSuccessStatusCode)
        {
            var errorCode = response.StatusCode;

            throw errorCode switch
            {
                _ => new NotImplementedException()
            };
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseJson)!;
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

    private static string GetUrlWithParameters(string url, Dictionary<string, string>? urlParameters = null)
    {
        return url + (urlParameters != null
            ? $"?{string.Join("&", urlParameters.Select(entry => $"{entry.Key}={entry.Value}"))}"
            : "");
    }

    private static HttpContent GetJsonRpcRequestContent(
        string method,
        object? body = null
    )
    {
        var json = new JsonObject
        {
            ["method"] = method,
            ["id"] = 0,
            ["jsonrpc"] = "2.0"
        };

        if (body != null)
            json["params"] = JsonNode.Parse(JsonSerializer.Serialize(body));

        return new StringContent(json.ToJsonString(), Encoding.UTF8, "application/json");
    }

    private static HttpContent GetRestRequestContent(
        object? body = null
    )
    {
        return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
    }
}
