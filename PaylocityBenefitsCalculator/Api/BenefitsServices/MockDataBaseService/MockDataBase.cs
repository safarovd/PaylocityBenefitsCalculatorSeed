using Api.BenefitsServices.BenefitsHelper;
using Api.Dtos.Employee;
using Api.Dtos.Dependent;
using Api.Models;

namespace Api.BenefitsServices.MockDataBaseService
{
    public class MockDataBase : IMockDataBaseService
    {
        // hidden from the user, this will act as our table
        private AllEntities _data;
        // reads/writes to our fake table
        protected JsonLoader JsonLoader;
        // counters for entity Id's
        protected static int _numEmployees;
        protected static int _numDependents;
        // caching for our queried entities, speeding up our query process
        private Dictionary<int, Employee> _employeeCache = new Dictionary<int, Employee>();
        private Dictionary<int, Dependent> _dependentCache = new Dictionary<int, Dependent>();
        // path to data
        protected string MockEntitiesPath = "BenefitsServices\\MockDataBaseService\\MockData\\MockEntities\\MockEntities.json";
        public MockDataBase()
        {
            // Hook up and load in our Employee data
            JsonLoader = new JsonLoader();
            _data = JsonLoader.LoadJson<AllEntities>(MockEntitiesPath);
            // initialize our session counters
            _numEmployees = _data.Employees.Count == 0 ? 0 : _data.Employees.Last().Id + 1;
            _numDependents = GetLastestDependentId();
        }

        private int GetLastestDependentId()
        {
            for (int i = _data.Employees.Count - 1; i >= 0; i--)
            {
                if (_data.Employees[i].Dependents.Count > 0)
                {
                    return _data.Employees[i].Dependents.Last().Id + 1;

                }
            }
            return 0;
        }

        public List<Dependent> QueryAllDependents()
        {
            // simply loop through all employees and store dependents and cache the queried data
            var dependents = new List<Dependent>();
            foreach (Employee employee in _data.Employees)
            {
                dependents.AddRange(employee.Dependents);
                CacheData();
            }
            return dependents;
        }

        public List<Employee> QueryAllEmployees()
        {
            // return all queried employees and cache
            CacheData();
            return _data.Employees;
        }

        public Dependent QueryDependentById(int id)
        {
            // if we have queried this dependend before, access their data in O(1) time
            if (_dependentCache.ContainsKey(id))
            {
                return _dependentCache[id];
            }
            // Otherwise they are a new entity and we need to manually query. Recache all data just in case.
            else
            {
                CacheData();
                foreach (Employee employee in _data.Employees)
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
            // if we have queried this employee before, access their data in O(1) time
            if (_employeeCache.ContainsKey(id))
            {
                return _employeeCache[id];
            }
            // Otherwise they are a new entity and we need to manually query. Recache all data just in case.
            else
            {
                CacheData();
                var employee = _data.Employees.Where(emp => emp.Id == id).FirstOrDefault();
                if (employee == null) throw new InvalidOperationException($"Employee {id} doesn't exist.");
                return employee;
            }
            return null;
        }

        public void InsertEmployee(Employee employee)
        {
            // make sure all dependents follow requirements
            if (CanAddDependents(employee))
            {
                // incremenet Employee id
                employee.Id = MockDataBase._numEmployees++;
                foreach (Dependent dependent in employee.Dependents)
                {
                    dependent.Id = MockDataBase._numDependents++;
                    // map our dependent to an employee
                    dependent.EmployeeId = employee.Id;
                    // cache new data
                    _dependentCache[dependent.Id] = dependent;
                }
                _employeeCache[employee.Id] = employee;
                CacheData();
                // write to our backend data
                _data.Employees.Add(employee);
                JsonLoader.WriteJson(_data, MockEntitiesPath);
            }
        }

        public void UpdateEmployee(int id, UpdateEmployeeDto updatedEmployee)
        {
            var employee = QueryEmployeeById(id);
            if (employee == null) throw new InvalidOperationException($"Employee {id} doesn't exist.");
            // update the values
            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.Salary = updatedEmployee.Salary;
            _employeeCache[employee.Id] = employee;
            CacheDependents(employee);
            JsonLoader.WriteJson(_data, MockEntitiesPath);
        }

        public void DeleteEmployee(int id)
        {
            var employee = QueryEmployeeById(id);
            if (!_employeeCache.ContainsKey(id) || employee == null) throw new InvalidOperationException($"Employee {id} doesn't exist.");
            foreach (Dependent dep in employee.Dependents)
            {
                if (_dependentCache.ContainsKey(dep.Id))
                {
                    // clean dependent cache too
                    _dependentCache.Remove(dep.Id);
                }
            }
            // clean employee cache
            _employeeCache.Remove(id);
            // remove employee from backend data
            _data.Employees.Remove(employee);
            // write our updated data to our mock storage
            JsonLoader.WriteJson(_data, MockEntitiesPath);
        }

        public void InsertDependent(Dependent dependent)
        {
            var employee = QueryEmployeeById(dependent.EmployeeId);
            if (CanAddDependent(dependent, employee))
            {
                // set dependent Id and inc our Id counter
                dependent.Id = MockDataBase._numDependents++;
                _dependentCache[dependent.Id] = dependent;
                employee.Dependents.Add(dependent);
                JsonLoader.WriteJson(_data, MockEntitiesPath);
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
            JsonLoader.WriteJson(_data, MockEntitiesPath);
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
            JsonLoader.WriteJson(_data, MockEntitiesPath);
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
        // we want to be able to easily cache our entities so that whenever data is updated
        // we can re-cache and later access it in O(1) time.
        private void CacheData()
        {
            foreach (Employee employee in _data.Employees)
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

        private bool CanAddDependent(Dependent dependent, Employee employee)
        {
            if ((dependent.Relationship == Relationship.Spouse || dependent.Relationship == Relationship.DomesticPartner) && employee.HasPartner)
            {
                throw new InvalidOperationException("Error 1: Employee cannot have two household parters of type Spouse or DomesticPartner.");
            }
            else if ((dependent.Relationship == Relationship.Spouse || dependent.Relationship == Relationship.DomesticPartner) && !employee.HasPartner) 
            {
                employee.HasPartner = true;
            }
            return true;
        }

        private bool CanAddDependents(Employee employee)
        {
            foreach (Dependent dep in employee.Dependents)
            {
                // As per requirements an Employee can only have 1 household partner
                if ((dep.Relationship == Relationship.Spouse || dep.Relationship == Relationship.DomesticPartner) && employee.HasPartner)
                {
                    throw new InvalidOperationException("Error 2: Employee cannot have two household parters of type Spouse or DomesticPartner.");
                } else if ((dep.Relationship == Relationship.Spouse || dep.Relationship == Relationship.DomesticPartner) && !employee.HasPartner)
                {
                    // set this to true so that we do not add another partner when we add dependents to this employee
                    employee.HasPartner = true;
                }
            }
            return true;
        }
    }
}
