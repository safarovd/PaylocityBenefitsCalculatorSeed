using Api.BenefitsServices.BenefitsHelper;
using Api.Dtos.Employee;
using Api.Dtos.Dependent;
using Api.Models;

namespace Api.BenefitsServices.MockDataBaseService
{
    public class MockDataBase : IMockDataBaseService
    {
        protected AllEntities Data;
        protected JsonLoader JsonLoader;
        protected static int _numEmployees = 0;
        protected static int _numDependents = 0;
        private Dictionary<int, Employee> _employeeCache = new Dictionary<int, Employee>();
        private Dictionary<int, Dependent> _dependentCache = new Dictionary<int, Dependent>();
        protected string MockEntitiesPath = "MockData\\MockEntities\\MockEntities.json";
        public MockDataBase()
        {
            // Hook up and load in our Employee data
            JsonLoader = new JsonLoader();
            Data = JsonLoader.LoadJson<AllEntities>(MockEntitiesPath);
        }

        public List<Dependent> QueryAllDependents()
        {
            var dependents = new List<Dependent>();
            foreach (Employee employee in Data.Employees)
            {
                dependents.AddRange(employee.Dependents);
                CacheData();
            }
            return dependents;
        }

        public List<Employee> QueryAllEmployees()
        {
            CacheData();
            return Data.Employees;
        }

        public Dependent QueryDependentById(int id)
        {
            if (_dependentCache.ContainsKey(id))
            {
                return _dependentCache[id];
            }
            else
            {
                CacheData();
                foreach (Employee employee in Data.Employees)
                {
                    foreach(Dependent dependent in employee.Dependents)
                    {
                        if (dependent.Id == id)
                        {
                            return dependent;
                        }
                    }
                }
            }
            return null;
        }

        public Employee QueryEmployeeById(int id)
        {
            if (_employeeCache.ContainsKey(id))
            {
                return _employeeCache[id];
            }
            else
            {
                CacheData();
                var employee = Data.Employees.Where(emp => emp.Id == id).FirstOrDefault();
                if (employee == null) throw new InvalidOperationException($"Employee {id} doesn't exist.");
                return employee;
            }
            return null;
        }

        public void InsertEmployee(Employee employee)
        {
            if (CanAddDependents(employee))
            {
                employee.Id = MockDataBase._numEmployees++;
                foreach (Dependent dependent in employee.Dependents)
                {
                    dependent.Id = MockDataBase._numDependents++;
                    dependent.EmployeeId = employee.Id;
                    _dependentCache[dependent.Id] = dependent;
                }
                _employeeCache[employee.Id] = employee;
                CacheData();
                Data.Employees.Add(employee);
                JsonLoader.WriteJson(Data, MockEntitiesPath);
            }
        }

        public void UpdateEmployee(int id, UpdateEmployeeDto updatedEmployee)
        {
            var employee = Data.Employees.Where(emp => emp.Id == id).FirstOrDefault();
            if (employee == null) throw new InvalidOperationException($"Employee {id} doesn't exist.");
            // update the values
            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.Salary = updatedEmployee.Salary;
            _employeeCache[employee.Id] = employee;
            CacheDependents(employee);
            JsonLoader.WriteJson(Data, MockEntitiesPath);
        }

        public void DeleteEmployee(int id)
        {
            var employee = QueryEmployeeById(id);
            if (!_employeeCache.ContainsKey(id) || employee == null) throw new InvalidOperationException($"Employee {id} doesn't exist.");
            foreach (Dependent dep in employee.Dependents)
            {
                if (_dependentCache.ContainsKey(dep.Id))
                {
                    _dependentCache.Remove(dep.Id);
                }
            }
            _employeeCache.Remove(id);
            Data.Employees.Remove(employee);
            // write our updated data to our mock storage
            JsonLoader.WriteJson(Data, MockEntitiesPath);
        }

        public void InsertDependent(Dependent dependent)
        {
            var employee = QueryEmployeeById(dependent.EmployeeId);
            if (CanAddDependents(employee))
            {
                dependent.Id = MockDataBase._numDependents++;
                _dependentCache[dependent.Id] = dependent;
                employee.Dependents.Add(dependent);
                JsonLoader.WriteJson(Data, MockEntitiesPath);
            }
        }

        public void UpdateDependent(int id, UpdateDependentDto update)
        {
            var dependent = QueryDependentById(id);
            if (dependent == null) throw new InvalidOperationException($"Dependent {id} doesn't exist.");
            // update the values
            dependent.FirstName = update.FirstName;
            dependent.LastName = update.LastName;
            dependent.DateOfBirth = update.DateOfBirth;
            dependent.Relationship = update.Relationship;
            _dependentCache[dependent.Id] = dependent;
            JsonLoader.WriteJson(Data, MockEntitiesPath);
        }

        public void DeleteDependent(int id)
        {
            var dependent = QueryDependentById(id);
            if (!_dependentCache.ContainsKey(id) || dependent == null) throw new InvalidOperationException($"Dependent {id} doesn't exist, cannot remove dependent.");
            var employee = QueryEmployeeById(dependent.EmployeeId);
            if (!_employeeCache.ContainsKey(dependent.EmployeeId) || employee == null) throw new InvalidOperationException($"Employee {id} doesn't exist, cannot remove dependent.");
            _dependentCache.Remove(id);
            employee.Dependents.Remove(dependent);
            // write our updated data to our mock storage
            JsonLoader.WriteJson(Data, MockEntitiesPath);
        }

        private void CacheDependents(Employee employee)
        {
            foreach (Dependent dependent in employee.Dependents)
            {
                if (_dependentCache.ContainsKey(dependent.Id))
                {
                    _dependentCache[dependent.Id] = dependent;
                } else
                {
                    _dependentCache.Add(dependent.Id, dependent);
                }    
            }
        }

        private void CacheData()
        {
            foreach (Employee employee in Data.Employees)
            {
                if (_employeeCache.ContainsKey(employee.Id))
                {
                    _employeeCache[employee.Id] = employee;
                } 
                else
                {
                    _employeeCache.Add(employee.Id, employee);
                }
                CacheDependents(employee);
                
            }
        }

        private bool CanAddDependents(Employee employee)
        {
            foreach (Dependent dep in employee.Dependents)
            {
                if ((dep.Relationship == Relationship.Spouse || dep.Relationship == Relationship.DomesticPartner) && employee.HasPartner)
                {
                    throw new InvalidOperationException("Employee cannot have two household parters of type Spouse or DomesticPartner.");
                } else if ((dep.Relationship == Relationship.Spouse || dep.Relationship == Relationship.DomesticPartner) && !employee.HasPartner)
                {
                    employee.HasPartner = true;
                }
            }
            return true;
        }
    }
}
