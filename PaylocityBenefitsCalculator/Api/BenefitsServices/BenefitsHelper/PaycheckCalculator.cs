using Api.Dtos.Employee;
using System.Globalization;

namespace Api.BenefitsServices.BenefitsHelper
{
    public static class PaycheckCalculator
    {
        public static decimal CalculatePaycheck(GetEmployeeDto getEmployeeDto)
        {
            decimal salary = getEmployeeDto.Salary;
            int age = GetEmployeeAge(getEmployeeDto.DateOfBirth);
            // If salary is over 80k, incure 2% fee
            salary = SalaryCapFee(salary);
            var monthlySalary = ConvertSalaryToMonthly(salary);
            // if employye is 50 years or older, deduct $200 per month
            monthlySalary = OldAgeFee(age, monthlySalary);
            // every employee pays a base fee of $1000
            monthlySalary = BaseFee(monthlySalary);
            // every dependent costs $600 per month for benefits
            monthlySalary = DependentFee(monthlySalary, getEmployeeDto);
            // convert monthly to actual paycheck
            var paycheck = ConvertToPaycheck(monthlySalary);
            if (paycheck < 0) throw new InvalidOperationException($"Employee {getEmployeeDto.Id} earns peanuts, pay more.");
            return decimal.Round(paycheck, 2, MidpointRounding.AwayFromZero); ;
        }
        private static decimal ConvertSalaryToMonthly(decimal salary)
        {
            return salary / 12;
        }

        private static decimal ConvertToPaycheck(decimal monthlySalary)
        {
            return (monthlySalary * 12) / 26;
        }

        private static int GetEmployeeAge(string date)
        {
            var cultureInfo = new CultureInfo("de-DE");
            var birthday = DateTime.Parse(date, cultureInfo,
                                            DateTimeStyles.NoCurrentDateDefault);
            int age = (DateTime.Now - birthday).Days / 365;
            return age;
        }

        private static decimal SalaryCapFee(decimal salary)
        {
            if (salary >= 80000)
            {
                return salary * (decimal)0.98;
            }
            return salary;
        }

        private static decimal OldAgeFee(int age, decimal monthlySalary)
        {
            if (age >= 50)
            {
                return monthlySalary - 200;
            }
            return monthlySalary;
        }

        private static decimal BaseFee(decimal monthlySalary)
        {
            return monthlySalary - 1000;
        }

        private static decimal DependentFee(decimal monthlySalary, GetEmployeeDto getEmployeeDto)
        {
            return monthlySalary - (getEmployeeDto.Dependents.Count * 600);
        }
    }
}
