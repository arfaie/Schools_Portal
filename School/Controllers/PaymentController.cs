using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Helpers;
using School.Helpers.OptionEnums;
using School.Helpers.ZarinPal;
using School.Models;
using School.Services;

namespace School.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ISmsSender _smsSender;

        public PaymentController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ISmsSender smsSender)
        {
            _userManager = userManager;
            _context = context;
            _smsSender = smsSender;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Cart()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var student = await _context.Students.Where(x => x.IdUser == user.Id).AsNoTracking().FirstOrDefaultAsync();
            if (!(student.FatherName == "" || student.LastName == null))
            {
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (HttpContext.Session.Keys.Contains("CartItems"))
                {
                    var cartItems = HttpContext.Session.GetComplexData<List<string>>("CartItems");
                    var products = await _context.Costs.Where(x => cartItems.Contains(x.Id))/*.Include(x => x.OfferItems)*/.ToListAsync();

                    //await Helper.AddOfferToProductsAsync(_context, user, products);

                    var factors = await _context.Factors.Where(x => !x.IsPaid && x.IdUser == user.Id).OrderByDescending(x => x.Date).Include(x => x.FactorItems).ThenInclude(x => x.Cost)/*.ThenInclude(x => x.OfferItems)*/.ToListAsync();

                    // check if there is an unpaid factor with the same items
                    var factor = factors.FirstOrDefault(x => cartItems.All(x.FactorItems.Select(y => y.CostId).Contains) && x.FactorItems.Count == cartItems.Count);

                    if (factor == null)
                    {
                        factor = new Factor
                        {
                            Date = DateTime.UtcNow,
                            IdUser = user.Id,
                        };

                        // generate random factor code
                        var allFactorCodes = await _context.Factors.Select(x => x.FactorCode).ToListAsync();

                        var random = new Random();
                        do
                        {
                            factor.FactorCode = $"Sahad-{random.Next(10000, 99999)}";
                        } while (allFactorCodes.Contains(factor.FactorCode));

                        await using var transaction = await _context.Database.BeginTransactionAsync();
                        try
                        {
                            await _context.Factors.AddAsync(factor);
                            await _context.SaveChangesAsync();

                            foreach (var product in products)
                            {
                                var factorItem = new FactorItem
                                {
                                    FactorId = factor.Id,
                                    CostId = product.Id,
                                    Cost = product,
                                    Price=product.Value
                                    //UnitCount = 1,
                                    //UnitPrice = product.Price,
                                    //Discount = product.Price - product.PriceWithDiscount
                                };

                                await _context.FactorItems.AddAsync(factorItem);
                            }

                            factor.TotalPrice = (int)factor.FactorItems.Sum(x => x.Cost.Value);

                            _context.Factors.Update(factor);
                            await _context.SaveChangesAsync();

                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                        }
                    }
                    else
                    {
                        if (factor.FactorItems?.Count > 0)
                        {
                            factor.TotalPrice = (int)factor.FactorItems.Sum(x => x.Cost.Value);
                        }
                        factor.Date = DateTime.UtcNow;

                        _context.Factors.Update(factor);
                        await _context.SaveChangesAsync();
                    }

                    return View(factor);
                }

                return View(new Factor());
            }
            else
            {
                HttpContext.Session.SetString("EditProfile", "NotComplete");
                ViewData["Profile"] = "جهت ادامه پروفایل کاربری خود را تکمیل نمایید";
                return RedirectToAction("EditProfile", "UserProfile");
            }


        }

        [HttpGet]
        public async Task<IActionResult> PaymentCheckout(string id)
        {
            var factor = await _context.Factors.Include(x => x.FactorItems).FirstOrDefaultAsync(x => x.Id == id);

            if (factor == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.User = user;

            //var addresses = await _context.Addresses.Where(x => x.UserId == user.Id).Include(x => x.City).ThenInclude(x => x.State).ToListAsync();

            //if (addresses.Count > 0)
            //{
            //	factor.AddressId = addresses.ElementAt(0).Id;
            //	factor.ShippingCost = await Helper.CalculateShippingCostAsync(_context, factor.AddressId, factor.FactorItems);
            //}

            //ViewBag.Addresses = addresses;

            if (factor.FactorItems?.Count > 0)
            {
                factor.TotalPrice = (int)factor.FactorItems.Sum(x => x.Price);

                _context.Factors.Update(factor);
                await _context.SaveChangesAsync();

                //var productIds = factor.FactorItems.Select(x => x.ProductId).ToList();
                //ViewBag.Products = await _context.Products.Where(x => productIds.Contains(x.Id)).ToListAsync();
            }

            //ViewBag.States = await _context.States.OrderBy(x => x.Name).ToListAsync();

            //var cities = await _context.Cities.OrderBy(x => x.Name).ToListAsync();

            

            return View(factor);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentCheckout(Factor model)
        {
            var factor = await _context.Factors.Include(x => x.FactorItems).FirstOrDefaultAsync(x => x.Id == model.Id);

            if (factor != null)
            {
                //if (String.IsNullOrWhiteSpace(model.AddressId))
                //{
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                ViewBag.User = user;

                //ViewBag.Addresses = await _context.Addresses.Where(x => x.UserId == user.Id).Include(x => x.City).ToListAsync();

                if (factor.FactorItems?.Count > 0)
                {
                    factor.TotalPrice = (int)factor.FactorItems.Sum(x => x.Price);

                    _context.Factors.Update(factor);
                    await _context.SaveChangesAsync();

                    var CostIds = factor.FactorItems.Select(x => x.CostId).ToList();
                    ViewBag.Costs = await _context.Costs.Where(x => CostIds.Contains(x.Id)).ToListAsync();
                }

                
                _context.Factors.Update(factor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PaymentConnect), new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> PaymentConnect(string id)
        {
            var factor = await _context.Factors.Include(x => x.FactorItems).FirstOrDefaultAsync(x => x.Id == id);

            if (factor == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (factor.FactorItems == null || factor.FactorItems.Count == 0)
            {
                ModelState.AddModelError(String.Empty, "خطا در محاسبه مبلغ فاکتور");
                return View(factor);
            }

            if (factor.TotalPrice == 0)
            {
                ModelState.AddModelError("AmountError", "مبلغ نمی تواند خالی باشد. لطفا مبلغی را بیشتر از 100 تومان وارد نمایید.");
                return View(factor);
            }

            _context.Factors.Update(factor);
            await _context.SaveChangesAsync();

            var callbackUrl = $"http://{Request.Host}/Payment/{nameof(PaymentVerify)}/{factor.Id}";
            var description = "پرداخت در سایت پوناس";

            try
            {
                var response = ZarinPalPayment.Request(factor.TotalPrice, description, callbackUrl);

                // if there is an error show this page again
                if (response.Status == 100)
                {
                    Response.Redirect(ZarinPalPayment.GetPaymentGatewayUrl(response.Authority));
                }
                else
                {
                    ModelState.AddModelError(String.Empty, $"خطا در پرداخت. کد خطا: {response.Status} ");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, "خطا در پرداخت. کد خطا: عدم اتصال به درگاه ");
            }

            return View(factor);
        }

        [HttpGet]
        public async Task<IActionResult> PaymentVerify(string id)
        {
            var collection = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.Value);
            var status = collection["Status"];

            var factor = await _context.Factors.FirstOrDefaultAsync(x => x.Id == id);

            if (factor == null)
            {
                return RedirectToAction(nameof(FailedPayment), new { error = "خطا در تایید فاکتور" });
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            user = _context.Users.Where(x => x.Id == user.Id).Include(x => x.Students).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction(nameof(FailedPayment), new { factorCode = factor.FactorCode, error = "خطا در تایید کاربر" });
            }

            if (status != "OK")
            {
                var failStatus = await _context.Statuses.FirstOrDefaultAsync(x => x.Title == "پرداخت نشده");

                if (failStatus == null)
                {
                    failStatus = new Status
                    {
                        Title = "پرداخت نشده"
                    };

                    await _context.Statuses.AddAsync(failStatus);
                    await _context.SaveChangesAsync();
                }

                var order = new Order
                {
                    Description = user.UserName,
                    TransactionNumber = collection["Authority"].TrimStart('0'),
                    StatusId = failStatus.Id,
                    TransactionDate = DateTime.UtcNow,
                    //IssueCode = 0,
                    FactorId = factor.Id,
                    TransactionStatus = false
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(FailedPayment), new { factorCode = factor.FactorCode, error = "خطا در پرداخت" });
            }

            var authority = collection["Authority"];

            TempData["FactorCode"] = factor.FactorCode;

            if (factor.TotalPrice == 0)
            {
                return RedirectToAction(nameof(FailedPayment), new { factorCode = factor.FactorCode, error = "خطا در مبلغ درست پرداختی" });
            }

            var verificationResponse = ZarinPalPayment.Verify(factor.TotalPrice, authority);

            if (!verificationResponse.IsSuccess)
            {
                var failStatus = await _context.Statuses.FirstOrDefaultAsync(x => x.Title == "پرداخت نشده");

                if (failStatus == null)
                {
                    failStatus = new Status
                    {
                        Title = "پرداخت نشده"
                    };

                    await _context.Statuses.AddAsync(failStatus);
                    await _context.SaveChangesAsync();
                }

                var order = new Order
                {
                    Description = user.UserName,
                    TransactionNumber = verificationResponse.RefId,
                    StatusId = failStatus.Id,
                    TransactionDate = DateTime.UtcNow,
                    //IssueCode = 0,
                    FactorId = factor.Id,
                    TransactionStatus = false
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(FailedPayment), new { factorCode = factor.FactorCode, error = "خطا در تایید پرداخت" });
            }

            //string dateTimes = Helper.GetPersianDateText(DateTime.Now);

            var statuses = await _context.Statuses.FirstOrDefaultAsync(x => x.Title == "پرداخت شده	");
            if (statuses == null)
            {
                statuses = new Status
                {
                    Title = "پرداخت شده	"
                };

                await _context.Statuses.AddAsync(statuses);
                await _context.SaveChangesAsync();
            }

            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = new Order
                    {
                        Description = user.UserName,
                        TransactionNumber = verificationResponse.RefId,
                        StatusId = statuses.Id,
                        TransactionDate = DateTime.UtcNow,
                        //IssueCode = 1,
                        FactorId = factor.Id,
                        TransactionStatus = true
                    };

                    await _context.Orders.AddAsync(order);

                    factor.IsPaid = true;
                    _context.Factors.Update(factor);

                    HttpContext.Session.Remove("CartItems");

                    var factorItems = await _context.FactorItems.Where(x => x.FactorId == factor.Id).ToListAsync();

                    var products = await _context.Costs.Where(x => factorItems.Select(y => y.CostId).Contains(x.Id)).ToListAsync();

                    foreach (var factorItem in factorItems)
                    {
                        var product = products.FirstOrDefault(x => x.Id == factorItem.CostId);

                        //if (product != null)
                        //{
                        //	product.Inventory -= factorItem.UnitCount;

                        //	if (product.Inventory < 0)
                        //	{
                        //		product.Inventory = 0;
                        //	}

                        //	_context.Products.Update(product);
                        //}
                    }

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return RedirectToAction(nameof(FailedPayment), new { factorCode = factor.FactorCode, error = ex.Message });
                }
            }
            var manager = user.Students.Where(x => x.IdUser == user.Id).FirstOrDefault().LastName;

            await _smsSender.SendSmsAsync(user.UserName, SmsTypes.SchoolPaymentDone, factor.FactorCode, (!String.IsNullOrWhiteSpace(manager) ? manager : "فرهیخته"));

            return RedirectToAction(nameof(SuccessfulPayment), new { factorCode = factor.FactorCode });
        }

        [HttpGet]
        public IActionResult FailedPayment(string factorCode, string error)
        {
            ViewBag.FactorCode = factorCode;

            ViewBag.StatusMessage = error;

            return View();
        }

        [HttpGet]
        public IActionResult SuccessfulPayment(string factorCode)
        {
            ViewBag.FactorCode = factorCode;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CalculateShippingCost(string addressId, string factorId)
        {
            var factor = await _context.Factors.Include(x => x.FactorItems).FirstOrDefaultAsync(x => x.Id == factorId);

            if (factor != null)
            {
                //return Json(new { cost = await Helper.CalculateShippingCostAsync(_context, addressId, factor.FactorItems) });
            }

            return Json(new { cost = 0 });
        }

        public async Task<IActionResult> Costs()
        {
            var costs = await _context.Costs.ToListAsync();
            return View(costs);
        }

        /*Product*/

        public async Task<IActionResult> AddToCart(string costId)
        {
            var product = await _context.Costs.SingleOrDefaultAsync(b => b.Id == costId);
            if (product == null)
            {
                return Json(new { status = "fail", message = "این محصول موجود نیست" });
            }

            if (!HttpContext.Session.Keys.Contains("CartItems"))
            {
                HttpContext.Session.SetComplexData("CartItems", new List<string> { costId });

                return Json(new { status = "success", message = "آیتم به لیست درخواستی شما اضافه شد", count = 1 });
            }

            var cartItems = HttpContext.Session.GetComplexData<List<string>>("CartItems");

            if (cartItems.Contains(costId))
            {
                return Json(new { status = "success", message = "این آیتم از قبل در لیست درخواستی شما وجود دارد", count = cartItems.Count });
            }

            cartItems.Add(costId);

            HttpContext.Session.SetComplexData("CartItems", cartItems);

            return Json(new { status = "success", message = "آیتم به لیست درخواستی شما اضافه شد", count = cartItems.Count });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeFactorItemCount(string factorItemId, int value)
        {
            var factorItem = await _context.FactorItems.FirstOrDefaultAsync(x => x.Id == factorItemId);

            if (factorItem != null)
            {
                //var changeValue = value - factorItem.UnitCount;
                //factorItem.UnitCount = value;

                _context.FactorItems.Update(factorItem);
                await _context.SaveChangesAsync();

                //return Json(new { status = "success", changeValue, unitPrice = factorItem.UnitPrice, discount = factorItem.Discount });
            }
            return Json(new { status = "success", changeValue = 0, unitPrice = 0, discount = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFactorItem(string factorItemId)
        {
            var factorItem = await _context.FactorItems.FirstOrDefaultAsync(x => x.Id == factorItemId);

            if (factorItem != null)
            {
                if (HttpContext.Session.Keys.Contains("CartItems"))
                {
                    var cartItems = HttpContext.Session.GetComplexData<List<string>>("CartItems");

                    var list = new List<string>(cartItems);
                    list.Remove(factorItem.CostId);

                    HttpContext.Session.SetComplexData("CartItems", list.ToArray());
                }

                var value = 1;

                _context.FactorItems.Remove(factorItem);
                await _context.SaveChangesAsync();

                return Json(new { status = "success", value });
            }
            return Json(new { status = "success", value = 0 });
        }

        /*Product*/
    }
}