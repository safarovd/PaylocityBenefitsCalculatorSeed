namespace Api.Models
{
    public class Dependent
    {
        // Step 1: remove all the 'sets' because we do not want
        //         just anyone to have the option to set new values on our properties.
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DateOfBirth { get; set; }
        public Relationship Relationship { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
