
using RareCrewCSharp.DTO;
using RareCrewCSharp.Models;
using System.Text.Json;

namespace RareCrewCSharp.Services
{
    public class EmployeeService
    {

        private readonly HttpClient _httpClient;
        private readonly string _url;
        private readonly string _endpoint;
        private readonly string _key;

        public EmployeeService(HttpClient _http, IConfiguration config)
        {
            _httpClient = _http;
            _url = config["ApiConfiguration:Url"];
            _endpoint = config["ApiConfiguration:Endpoint"];
            _key=config["ApiConfiguration:Key"];
        }

        public async Task<List<Employee>> GetEmployees()
        {
            var response=await _httpClient.GetAsync(_url+"/"+_endpoint+"?code="+_key);
            response.EnsureSuccessStatusCode();

            var json=await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<List<EmployeeDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<EmployeeDTO>();

            return ValidateEmployees(data);

        }

        private List<Employee> ValidateEmployees(List<EmployeeDTO> employees)
        {
            return employees.Where(e => !string.IsNullOrEmpty(e.EmployeeName) && e.DeletedOn == null && e.StarTimeUtc < e.EndTimeUtc).GroupBy(e => e.EmployeeName).Select(g =>
            {
                double totalHours = g.Sum(x =>
                {
                    var distinction = x.EndTimeUtc - x.StarTimeUtc;
                    var milliseconds = distinction.TotalMilliseconds;
                    return Math.Floor(milliseconds / 3600000);
                });
                return new Employee()
                {
                    EmployeeName=g.Key,
                    TotalHours=totalHours
                };
            }
                
            ).Where(e => e.TotalHours > 0).OrderByDescending(e => e.TotalHours).ToList();
        }



        
        
    }
}
