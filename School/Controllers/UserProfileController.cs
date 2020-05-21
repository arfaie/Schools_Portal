using School.Helpers;
using School.Models.Helpers;
using School.Models.Helpers.OptionEnums;
using School.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using School.Data;

namespace School.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //Comment
            var select = await _context.Students.Where(x => x.IdUser == user.Id)/*.Include(x => x.City.State)*/.FirstOrDefaultAsync();

            //var cities = _context.Cities.Where(x => x.Id == select.CityId).FirstOrDefault();
            //select.id

            return View(select);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var select = await _context.Students.Where(x => x.IdUser == user.Id)/*.Include(x=>x.SchoolsLevels)*/.FirstOrDefaultAsync();
            //var ss=select.LevelIds..Where(x=>x.Id=="").
            //var levels = _context.SchoolsLevels.Where(x => x.SchoolId == select.Id);

            //select.LevelIds = levels.Select(x => x.LevelId).ToArray();

            //ViewBag.UserSchool = new SelectList(await _context.Schools.Where(c => c.Id == select.Id).ToListAsync(), "Id", "Name");

            //var SchoolTypes = await _context.SchoolTypes.OrderBy(c => c.Title).ToListAsync();
            //var HowMets = await _context.HowMets.OrderBy(c => c.Name).ToListAsync();
            //var Cities = await _context.Cities.OrderBy(c => c.Name).ToListAsync();
            //var States = await _context.States.OrderBy(c => c.Name).ToListAsync();
            //var Levels = await _context.Levels.OrderBy(c => c.Name).ToListAsync();

            //ViewBag.SchoolTypes = SchoolTypes;
            //ViewBag.HowMets = HowMets;
            //ViewBag.Cities = Cities;
            //ViewBag.States = States;
            //ViewBag.Levels = Levels;


            return PartialView("EditProfile", select);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Student model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var school = await _context.Students.Where(x => x.IdUser == user.Id).AsNoTracking().FirstOrDefaultAsync();
                if (user == null)
                {
                    TempData["Notification"] = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red);
                    return RedirectToAction("Index");
                }

                //SchoolsLevels obSchoolLeves;
                //if (model.LevelIds!=null)
                //{
                //    var schoolLevels = _context.SchoolsLevels.Where(x => x.SchoolId == model.Id);
                //    if (schoolLevels.Count() > 0)
                //    {
                //        _context.SchoolsLevels.RemoveRange(schoolLevels);
                //    }
                //    foreach (var id in model.LevelIds)
                //    {
                //        obSchoolLeves = new SchoolsLevels();
                //        obSchoolLeves.LevelId = id.ToString();
                //        obSchoolLeves.SchoolId = model.Id;
                //        await _context.SchoolsLevels.AddAsync(obSchoolLeves);
                //    }

                //}
                model.IdUser = user.Id;
                _context.Entry(model).State = EntityState.Modified;
                int save = await _context.SaveChangesAsync();
                string ChekRedirect = HttpContext.Session.GetString("EditProfile");
                if (ChekRedirect== "NotComplete")
                {
                    HttpContext.Session.SetString("EditProfile", "Complete");
                    return RedirectToAction("Cart", "Payment");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangePasswordProfile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return PartialView("ChangePasswordProfile", new AdminChangePasswordViewModel { Id = user.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordProfile(AdminChangePasswordViewModel model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    return RedirectToAction("Index");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    TempData["Notification"] = Notification.ShowNotif("خطایی رخ داد.", ToastType.Red);
                    return PartialView("ChangePasswordProfile", model);
                }

                //await _signInManager.RefreshSignInAsync(user);
                TempData["Notification"] = Notification.ShowNotif("رمز عبور شما با موفقیت ویرایش شد.", ToastType.Green);
                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            return PartialView("ChangePasswordProfile", model);
        }

        //[HttpGet]
        //[AutoValidateAntiforgeryToken]
        //public async Task<IActionResult> userAddress()
        //{
        //    var user = await _userManager.GetUserAsync(HttpContext.User);


        //    ViewBag.UserFullName = user.FullName;
        //    ViewBag.UserMobile = user.PhoneNumber;

        //    ViewBag.States = await _context.States.OrderBy(x => x.Name).ToListAsync();

        //    var cities = await _context.Cities.OrderBy(x => x.Name).ToListAsync();



        //    ViewBag.Cities = cities;

        //    return View(userAddresses);
        //}

        //[HttpGet]
        //[AutoValidateAntiforgeryToken]
        //public async Task<IActionResult> AddEditUserAddress(string id)
        //{
        //    var address = await _context.Addresses.FirstOrDefaultAsync(c => c.Id == id);

        //    ViewBag.States = new SelectList(await _context.States.ToListAsync(), "Id", "Name");
        //    ViewBag.Cities = new SelectList(await _context.Cities.ToListAsync(), "Id", "Name");

        //    if (address != null)
        //    {
        //        return View(address);
        //    }

        //    return View(new Address());
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddEditUserAddress(string id, Address model, string redirectUrl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.GetUserAsync(HttpContext.User);
        //        model.UserId = user.Id;

        //        ViewBag.UserFullName = user.FullName;
        //        ViewBag.UserMobile = user.PhoneNumber;

        //        if (string.IsNullOrWhiteSpace(id))
        //        {
        //            _context.Addresses.Add(model);
        //            await _context.SaveChangesAsync();

        //            TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
        //            return PartialView("_SuccessfulResponse", redirectUrl);
        //        }

        //        _context.Addresses.Update(model);
        //        await _context.SaveChangesAsync();

        //        TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
        //        return PartialView("_SuccessfulResponse", redirectUrl);
        //    }

        //    if (string.IsNullOrWhiteSpace(id))
        //    {
        //        TempData["Notification"] = Notification.ShowNotif(MessageType.AddError, ToastType.Red);
        //    }
        //    else
        //    {
        //        TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, ToastType.Red);
        //    }

        //    return View(model);
        //}

        //[HttpGet]
        //[AutoValidateAntiforgeryToken]
        //public async Task<IActionResult> DeleteUserAddress(string id)
        //{
        //    var select = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
        //    if (select != null)
        //    {
        //        _context.Addresses.Remove(select);
        //        await _context.SaveChangesAsync();

        //        TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
        //        return RedirectToAction("userAddress");
        //    }

        //    return RedirectToAction("userAddress");
        //}

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> userOrders()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var student = _context.Students.Where(x => x.IdUser == user.Id).FirstOrDefault();
            if (student != null)
            {
                ViewBag.UserFullName = student.FirstName + " " + student.LastName;
                ViewBag.UserMobile = student.FatherMobile;
            }
            

            return View(await _context.Payments.Where(o => o.Factor.IdUser == user.Id).Include(o => o.Factor).OrderByDescending(x => x.TransactionDate).ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> orderDetaile(string id, bool isReturned = false)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            //ViewBag.path = "/upload/normalimage/";
            ViewBag.path = "/img/Logo/";

            ViewBag.barasi = "";
            ViewBag.amadesazi = "";
            ViewBag.post = "";
            ViewBag.moshtari = "";

            ViewBag.isReturned = isReturned;

            var select = await _context.Payments.Include(o => o.Factor)/*.ThenInclude(o => o.Address)*/
                                              /*.Include(x => x.Factor..Schools) */                                             /*.ThenInclude(o => o.City)*/.Include(x => x.Factor)/*.ThenInclude(x => x.fa)*//*.ThenInclude(x => x.Product)*/
                .FirstOrDefaultAsync(o => o.Id == id);
            //switch (select.StatusId)
            //{
            //    case "6f9c65d681937c32dafcec03":
            //        ViewBag.barasi = "profile-order-steps-item-active";
            //        break;

            //    case "6f9c65d681937c32dafcec04":
            //        ViewBag.amadesazi = "profile-order-steps-item-active";
            //        break;

            //    case "6f9c65d681937c32dafcec05":
            //        ViewBag.post = "profile-order-steps-item-active";
            //        break;

            //    case "6f9c65d681937c32dafcec06":
            //        ViewBag.moshtari = "profile-order-steps-item-active";
            //        break;

            //    default:
            //        ViewBag.barasi = "";
            //        ViewBag.amadesazi = "";
            //        ViewBag.post = "";
            //        ViewBag.moshtari = "";
            //        break;
            //}


            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            return View(select);
        }

        //[HttpGet]
        //[AutoValidateAntiforgeryToken]
        //public async Task<IActionResult> userComments()
        //{
        //    var user = await _userManager.GetUserAsync(HttpContext.User);

        //    ViewBag.UserFullName = user.FullName;
        //    ViewBag.UserMobile = user.PhoneNumber;

        //    return View(await _context.CommentAndStars.Include(c => c.User).Include(c => c.Product)
        //        .Where(c => c.UserId == user.Id).ToListAsync());
        //}

        //[HttpGet]
        //[AutoValidateAntiforgeryToken]
        //public async Task<IActionResult> History()
        //{
        //    var user = await _userManager.GetUserAsync(HttpContext.User);

        //    ViewBag.UserFullName = user.FullName;
        //    ViewBag.UserMobile = user.PhoneNumber;

        //    var histories = await _context.Histories.Where(x => x.UserId == user.Id).OrderByDescending(x => x.RegistrationDateAndTime).ToListAsync();

        //    var products = new List<Product>();
        //    foreach (var history in histories)
        //    {
        //        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == history.ProductId);

        //        if (product != null)
        //        {
        //            products.Add(product);
        //        }
        //    }

        //    return View(products);
        //}

        //[HttpGet]
        //public async Task<IActionResult> userCommentEdit(string id)
        //{
        //    var select = await _context.CommentAndStars.FirstOrDefaultAsync(c => c.Id == id);

        //    var user = await _userManager.GetUserAsync(HttpContext.User);

        //    ViewBag.UserFullName = user.FullName;
        //    ViewBag.UserMobile = user.PhoneNumber;

        //    if (select == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return PartialView("userCommentEdit", select);
        //}

        //[HttpPost]
        //public async Task<IActionResult> userCommentEdittt(string id, CommentAndStar model, string redirectUrl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (!string.IsNullOrWhiteSpace(id))
        //        {
        //            _context.CommentAndStars.Update(model);
        //            await _context.SaveChangesAsync();

        //            TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
        //            return PartialView("_SuccessfulResponse", redirectUrl);
        //        }
        //    }

        //    return PartialView("userCommentEdit", model);
        //}

        //[HttpGet]
        //[AutoValidateAntiforgeryToken]
        //public async Task<IActionResult> DeleteUserComment(string id)
        //{
        //    var select = await _context.CommentAndStars.FirstOrDefaultAsync(a => a.Id == id);

        //    var user = await _userManager.GetUserAsync(HttpContext.User);

        //    ViewBag.UserFullName = user.FullName;
        //    ViewBag.UserMobile = user.PhoneNumber;

        //    if (select != null)
        //    {
        //        _context.CommentAndStars.Remove(select);
        //        await _context.SaveChangesAsync();

        //        TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
        //        return RedirectToAction("userComments");
        //    }

        //    return RedirectToAction("userComments");
        //}

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ReturnedGoods()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _context.Payments.Where(x => x.Factor.IdUser == user.Id /*&& x.Status.Title != "پرداخت نشده"*/)/*.Include(o => o.Status)*/.Include(o => o.Factor).OrderByDescending(x => x.TransactionDate).ToListAsync();

            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> ReturnRequest(string orderId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red) });
            }

            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            var order = await _context.Payments/*.Include(x => x.Status)*/.FirstOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن سفارش", ToastType.Red) });
            }

            //if (order.Status.Title == "تحویل به مشتری" || order.Status.Title == "در صف بررسی" || order.Status.Title == "آماده سازی سفارش" || order.Status.Title == "تحویل به پست")
            //{
            //    var status = await _context.Statuses.FirstOrDefaultAsync(x => x.Title == "درخواست مرجوعی");

            //    if (status == null)
            //    {
            //        return Json(new { status = "fail", message = Notification.ShowNotif("خطا در ویرایش سفارش", ToastType.Red) });
            //    }

            //    order.StatusId = status.Id;
            //    _context.Orders.Update(order);
            //    await _context.SaveChangesAsync();

            //    return Json(new { status = "success", message = Notification.ShowNotif("درخواست مرجوعی برای این سفارش ارسال شد", ToastType.Green) });
            //}

            return Json(new { status = "fail", message = Notification.ShowNotif("امکان درخواست مرجوعی برای این سفارش وجود ندارد.", ToastType.Red) });
        }

        //[HttpPost]
        //public async Task<IActionResult> AddAddress(string recipient, string mobile, string phone, string postalCode, string description, string city)
        //{
        //    var user = await _userManager.GetUserAsync(HttpContext.User);

        //    if (user == null)
        //    {
        //        return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red) });
        //    }

        //    var address = new Address
        //    {
        //        CityId = city,
        //        Description = description,
        //        Recipient = recipient,
        //        Mobile = mobile,
        //        Phone = phone,
        //        PostalCode = postalCode,
        //        UserId = user.Id
        //    };

        //    _context.Addresses.Add(address);

        //    try
        //    {
        //        await _context.SaveChangesAsync();

        //        return Json(new { status = "success", message = Notification.ShowNotif("آدرس جدید ثبت شد.", ToastType.Green) });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { status = "fail", message = Notification.ShowNotif("خطا در ثبت آدرس", ToastType.Red) });
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditAddress(string id, string recipient, string mobile, string phone, string postalCode, string description, string city)
        //{
        //    var user = await _userManager.GetUserAsync(HttpContext.User);

        //    if (user == null)
        //    {
        //        return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red) });
        //    }

        //    var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

        //    if (address == null)
        //    {
        //        return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن آدرس", ToastType.Red) });
        //    }

        //    address.CityId = city;
        //    address.Description = description;
        //    address.Recipient = recipient;
        //    address.Mobile = mobile;
        //    address.Phone = phone;
        //    address.PostalCode = postalCode;

        //    _context.Addresses.Update(address);

        //    try
        //    {
        //        await _context.SaveChangesAsync();

        //        return Json(new { status = "success", message = Notification.ShowNotif("آدرس اصلاح شد.", ToastType.Green) });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { status = "fail", message = Notification.ShowNotif("خطا در اصلاح آدرس", ToastType.Red) });
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeleteAddress(string id)
        //{
        //    var user = await _userManager.GetUserAsync(HttpContext.User);

        //    if (user == null)
        //    {
        //        return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red) });
        //    }

        //    var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

        //    if (address == null)
        //    {
        //        return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن آدرس", ToastType.Red) });
        //    }

        //    _context.Addresses.Remove(address);

        //    try
        //    {
        //        await _context.SaveChangesAsync();

        //        return Json(new { status = "success", message = Notification.ShowNotif("آدرس حذف شد.", ToastType.Green) });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { status = "fail", message = Notification.ShowNotif("خطا در حذف آدرس", ToastType.Red) });
        //    }
        //}
    }
}