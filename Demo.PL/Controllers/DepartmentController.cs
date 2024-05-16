using Demo.BLL.Interfacies;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            //_departmentRepo = departmentRepo;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(departments);
        }

        [HttpGet] //default
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] 
        public IActionResult Create(Department department)
        {
            if(ModelState.IsValid) //server side validation
            {
                 _unitOfWork.DepartmentRepository.Add(department);
                _unitOfWork.Complete();
                //if(count > 0)
                //{

                //}
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        public IActionResult Details(int? id, string viewName= "Details")
        {
            if(!id.HasValue)
            {
                return BadRequest();
            }
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if(department == null) 
            {
                return NotFound();
            }
            return View(viewName, department);
        }
        public IActionResult Edit(int? id)
        {
            //if (!id.HasValue)
            //{
            //    return BadRequest();
            //}
            //var department = _departmentRepo.Get(id.Value);
            //if (department == null)
            //{
            //    return NotFound();
            //}
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {

                    _unitOfWork.DepartmentRepository.Update(department);
                    _unitOfWork.Complete();
                    //if (count > 0)
                    //{

                    //}
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(department);
        }
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            try
            {
                _unitOfWork.DepartmentRepository.Delete(department);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(department);
        }
    }
}
