using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPInRepositoryPro.Models;
using SPInRepositoryPro.Repository;

namespace SPInRepositoryPro.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IGenericRepository<Employee> _repository;
        private readonly IGenericRepository<Department> _DepartmentRepository = new GenericRepository<Department>(new ProgramDbContext());


        public EmployeesController(IGenericRepository<Employee> repository)
        {
            _repository = repository;
        }

        // GET: Employees
        public async Task<IActionResult> Index( string SearchTerm = "", int? PageNumber = 1, int PageSize = 10,  string SortColumn = "Id", string SortDirection = "ASC")
        {
            var departments = await _DepartmentRepository.GetAllAsync();
            var departmentNames = departments.ToDictionary(d => d.DepartmentId, d => d.Name);

            ViewBag.DepartmentNames = departmentNames;


            //var employees = 

            ViewBag.PageNumber = PageNumber;
            ViewBag.SortColumn = SortColumn;
            ViewBag.SortDirection = SortDirection;
            ViewBag.PageSize = PageSize;
            ViewBag.SearchTerm = SearchTerm;
            //int currentPage = PageNumber ?? 1;

            var employees = await _repository.GetAllAsync(SearchTerm, PageNumber, PageSize, SortColumn, SortDirection);
            int TotalRecords = await _repository.GetCount(SearchTerm);
           
            int TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            ViewBag.TotalPages = TotalPages;

            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            var Departments = await _DepartmentRepository.GetAllAsync();
            var DepartmentNames = Departments.ToDictionary(d => d.DepartmentId, d => d.Name);

            if (employee.DepartmentId != null && DepartmentNames.ContainsKey(employee.DepartmentId))
            {
                ViewBag.DepartmentName = DepartmentNames[employee.DepartmentId];
            }
            else
            {
                ViewBag.DepartmentName = "No Department Assigned";
            }
            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            var departments = await _DepartmentRepository.GetAllAsync();
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _repository.InsertAsync(employee);
                await _repository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            var departments = await _DepartmentRepository.GetAllAsync();
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var departments = await _DepartmentRepository.GetAllAsync();

            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "Name", employee.DepartmentId);

            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                await _repository.UpdateAsync(employee);
                await _repository.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            var departments = await _DepartmentRepository.GetAllAsync();

            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            var Departments = await _DepartmentRepository.GetAllAsync();
            var departmentNames = Departments.ToDictionary(d => d.DepartmentId, d => d.Name);

            if (employee.DepartmentId != null && departmentNames.ContainsKey(employee.DepartmentId))
            {
                ViewBag.DepartmentName = departmentNames[employee.DepartmentId];
            }
            else
            {
                ViewBag.DepartmentName = "No department Name ";
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee != null)
            {
                await _repository.DeleteAsync(id);

            }

            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EmployeeExists(int id)
        {
            var Employee = await _repository.GetByIdAsync(id);
            if (Employee == null) return false;
            return true;
        }
    }
}
