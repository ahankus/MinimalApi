namespace IntegrationTests.Helpers;

public static class ApiRouteHelper
{
    private const string BaseUri = "http://localhost:5222/api";
    private const string AccountNumbersEndpoint = "accountNumbers";
    private const string CustomersEndpoint = "customers";
    
    public static string AccountNumbers()
    {
        return $"{BaseUri}/{AccountNumbersEndpoint}";
    }
    
    public static string AccountNumberNumber(string number)
    {
        return $"{BaseUri}/{AccountNumbersEndpoint}/{number}";
    }
    
    public static string Customers()
    {
        return $"{BaseUri}/{CustomersEndpoint}";
    }
    
    public static string CustomerId(string id)
    {
        return $"{BaseUri}/{CustomersEndpoint}/{id}";
    }
}