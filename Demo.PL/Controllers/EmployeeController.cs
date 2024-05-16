using AutoMapper;
using Demo.BLL.Interfacies;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _EmployeeRepo;
        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            //_EmployeeRepo = EmployeeRepo;
            //_departmentRepository = departmentRepository;
        }
        public IActionResult Index(string searchInp)
        {
            #region Binding through View's dictionary
            //Binding through View's dictionary : transfer data from Action to View [one way]
            //1. ViewData
            //2. ViewBag


            //1. ViewData
            //ViewData["Message"] = "Hello ViewData";

            //2. ViewBag
            //ViewBag.Message = "Hello ViewBag"; 
            #endregion

            if (string.IsNullOrEmpty(searchInp))
            {
                var Employees = _unitOfWork.EmployeeRepository.GetAll();

                var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);

                return View(mappedEmp);
            }
            else
            {
                var Employees = _unitOfWork.EmployeeRepository.GetEmployeeByName(searchInp);

                var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);

                return View(mappedEmp);
            }
            
        }

        [HttpGet] //default
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();
            ////or
            //ViewBag.Departments = _departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel EmployeeVm)
        {
            if (ModelState.IsValid) //server side validation
            {
                EmployeeVm.ImageName = DocumentSettings.UploadFile(EmployeeVm.Image, "images");

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVm);

                _unitOfWork.EmployeeRepository.Add(mappedEmp);
                _unitOfWork.Complete();

                //if (count > 0)
                //{
                //    //3. TempData
                //    TempData["Message"] = "Employ created succesfuly";
                //}
                //else
                //{
                //    TempData["Message"] = "Employ not created succesfuly";
                //}
                return RedirectToAction(nameof(Index));
            }
            return View(EmployeeVm);
        }
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var Employee = _unitOfWork.EmployeeRepository.Get(id.Value);

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(Employee);

            if (Employee == null)
            {
                return NotFound();
            }
            return View(viewName, mappedEmp);
        }
        public IActionResult Edit(int? id)
        {
            //ViewBag.Departments = _departmentRepository.GetAll();
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel EmployeeVm, [FromRoute] int id)
        {
            if (id != EmployeeVm.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    EmployeeVm.ImageName = DocumentSettings.UploadFile(EmployeeVm.Image, "images");
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVm);

                     _unitOfWork.EmployeeRepository.Update(mappedEmp);
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

            return View(EmployeeVm);
        }
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(EmployeeViewModel EmployeeVm, [FromRoute] int id)
        {
            
            if (id != EmployeeVm.Id)
                return BadRequest();

            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVm);


                DocumentSettings.DeleteFile(EmployeeVm.ImageName, "images");

                _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(EmployeeVm);
        }
    }
}
