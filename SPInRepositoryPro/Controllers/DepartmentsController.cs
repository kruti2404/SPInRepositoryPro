using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPInRepositoryPro.Models;
using SPInRepositoryPro.Repository;

namespace SPInRepositoryPro.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IGenericRepository<Department> _repository;

        public DepartmentsController(IGenericRepository<Department> repository)
        {
            _repository = repository;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _repository.GetAllAsync();
            return View(departments);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _repository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await _repository.InsertAsync(department);
                await _repository.SaveAsync();
                return RedirectToAction("Index", "Departments");
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _repository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                await _repository.UpdateAsync(department);
                await _repository.SaveAsync();

                return RedirectToAction("Index", "Departments");
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _repository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _repository.GetByIdAsync(id);
            if (department != null)
            {
                await _repository.DeleteAsync(id);
                await _repository.SaveAsync();
            }

            return RedirectToAction("Index", "Departments");
        }

        private async Task<bool> DepartmentExists(int id)
        {
            return await _repository.GetByIdAsync(id) != null;
        }


    }
}
