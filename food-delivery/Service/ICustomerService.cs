using food_delivery.Domain;
using food_delivery.Dto;

namespace food_delivery.Service
{
    public interface ICustomerService
    {
        Customer Authenticate(Credentials credentials);
        string AddCustomer(RegisterDto customer);
        Customer UpdateCustomer(RegisterDto customerDto);
        Customer DeleteCustomer(Credentials creds);
        Customer GetCustomerByUsername(string username);
        Customer DepositMoney(string userName, double amount);
    }
}