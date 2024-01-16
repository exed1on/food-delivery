using food_delivery.Domain;
using food_delivery.Dto;

namespace food_delivery.Service
{
    public interface ICustomerService
    {
        Customer Authenticate(Credentials credentials);
        Customer AddCustomer(CustomerDto customer);
        Customer UpdateCustomer(CustomerDto customerDto);
        Customer DeleteCustomer(Credentials creds);
        Customer GetCustomerByUsername(string username);
        bool CheckCustomerByUsername(string username);
        Customer DepositMoney(string userName, double amount);
    }
}