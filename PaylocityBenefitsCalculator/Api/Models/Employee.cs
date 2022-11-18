namespace Api.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal Salary { get; set; }
        public string? DateOfBirth { get; set; }
        public ICollection<Dependent> Dependents { get; set; } = new List<Dependent>();
        private bool _hasPartner = false;
        public bool HasPartner
        {
            get
            {
                return _hasPartner;
            }
            set
            {
                _hasPartner = value;
            }
        }
    }
}
