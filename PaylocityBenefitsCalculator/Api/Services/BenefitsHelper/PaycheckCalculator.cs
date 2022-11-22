using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using System.Globalization;

namespace Api.Services.BenefitsHelper
{
    // Reason:
    // I did the paycheck calculations on the backend because the front end would not 
    // build and run for me. It seemed like npm was referencing Paylocity servers. I decided to focus 
    // on the backend while awaiting responses on the issue from Paylocity devs. Due to dev vacation times
    // and my personal work/life responsibilities, I only had enough time to meet this requirement by coding here.
    // However, I would have done the below implementations on the client side for a couple of reasons.
    // If caclulations are done on the front end then the user gets instant response which is always good
    // user experience. Additionally, doing the calculations on the front end reduces loads on our servers
    // and being that math isn't too heavy for client side computation, it just seems like a more practical
    // approach to calculate employee paychecks on the front end.

    // HOWEVER! If we needed the paycheck calculations for more than just the UI (for example, we need to 
    // also generate documentation using those numbers and perhaps pass that value to some payment service)
    // then we should NOT trust the client and instead do the work on the backend. That way we have a secure
    // source of truth that can be passed around fairly efficiently to any service we need to. Additionally (a side thought), 
    // if there are frequent benefit calculation updates/changes I think we can benefit from OOP techniques to help
    // us abstract out types of calculations for types of employees, etc. etc.
    // Especially useful for different countries or states (i.e. taxes)
    // i.e. HR has x deductions
    //      Devs have y deductions
    //      Temps have z deductions
    // but this could probably be done on the front end as well....all depends on use case and business requirements I guess.
    // 
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
            // if a dependent is 50 years or older, deduct $200 per month
            monthlySalary = OldAgeFee(getEmployeeDto, monthlySalary);
            // convert monthly to actual paycheck
            var paycheck = ConvertToPaycheck(monthlySalary);
            if (paycheck < 0) throw new InvalidOperationException($"Employee {getEmployeeDto.Id} earns peanuts, pay more.");
            // round to 2 decimal places
            return decimal.Round(paycheck, 2, MidpointRounding.AwayFromZero);
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
