using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using System.Globalization;

namespace Api.Services.BenefitsHelper
{
    public static class PaycheckCalculator
    {
        public static decimal CalculatePaycheck(GetEmployeeDto getEmployeeDto)
        {
            decimal salary = getEmployeeDto.Salary;
            // If salary is over 80k, incure 2% fee
            salary = SalaryCapFee(salary);
            var monthlySalary = ConvertSalaryToMonthly(salary);
            // every employee pays a base fee of $1000
            monthlySalary = BaseFee(monthlySalary);
            // every dependent costs $600 per month for benefits
            monthlySalary = DependentFee(monthlySalary, getEmployeeDto);
            // if employye is 50 years or older, deduct $200 per month
            monthlySalary = OldAgeFee(getEmployeeDto, monthlySalary);
            // convert monthly to actual paycheck
            var paycheck = ConvertToPaycheck(monthlySalary);
            if (paycheck < 0) throw new InvalidOperationException($"Employee {getEmployeeDto.Id} earns peanuts, pay more.");
            // round to 2 decimal places
            return decimal.Round(paycheck, 2, MidpointRounding.AwayFromZero); ;
        }
        private static decimal ConvertSalaryToMonthly(decimal salary)
        {
            return salary / 12;
        }

        private static decimal ConvertToPaycheck(decimal monthlySalary)
        {
            // 26 paychecks per year with deductions spread as evenly as possible on each 
            // paycheck
            return (monthlySalary * 12) / 26;
        }

        private static int GetAge(string date)
        {
            var cultureInfo = new CultureInfo("de-DE");
            var birthday = DateTime.Parse(date, cultureInfo,
                                            DateTimeStyles.NoCurrentDateDefault);
            int age = (DateTime.Now - birthday).Days / 365;
            return age;
        }

        private static decimal SalaryCapFee(decimal salary)
        {
            // employees that make more than $80,000 per year will incur an additional 2% of their 
            // yearly salary in benefits costs
            if (salary >= 80000)
            {
                return salary * (decimal)0.98;
            }
            return salary;
        }

        private static decimal OldAgeFee(GetEmployeeDto employee, decimal monthlySalary)
        {
            // each dependent represents an additional $600 cost per month (for benefits)
            foreach (GetDependentDto dep in employee.Dependents)
            {
                if (GetAge(dep.DateOfBirth) >= 50)
                {
                    monthlySalary -= 200;
                }
            }
            return monthlySalary;
        }

        private static decimal BaseFee(decimal monthlySalary)
        {
            // deduct 1000 from every salary per month for benefits
            return monthlySalary - 1000;
        }

        private static decimal DependentFee(decimal monthlySalary, GetEmployeeDto getEmployeeDto)
        {
            // each dependent represents an additional $600 cost per month (for benefits)
            return monthlySalary - (getEmployeeDto.Dependents.Count * 600);
        }
    }
}
