namespace Api.Models
{
    // Reason:
    // I created this class to be a hub for all employees.
    // When I load in Employee.json data it is all stored here.
    public class AllEntities
    {
        public List<Employee> Employees { get; set;}
    }
}
