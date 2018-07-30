using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TShirtEmpAdmin.CustomFilters;
using TShirtEmpAdmin.Models;
using TShirtEmpAdmin.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;

namespace TShirtEmpAdmin.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        EmailService EmailService = new EmailService();
        
        [AuthLog(Roles = "Registered Users, Administrators")]
        public ActionResult Index(int? shirtId, int unum)
        {
            ViewBag.Warning = TempData["warning"];
            //Define and convert querystring value to int and compare log/ordernum to 
            //num value as first step to check malicious url behaviour
            string newNum = Request.Params["unum"];
            unum = 0;
            var res = int.TryParse(newNum, out unum);
            var oResult = (from l in db.UserLogs where unum == l.Id select l);
            int oCount = oResult.Count();
            if(res == false)
            {
                //ASP.NET IDENTITY Logoff Script
                //var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
                //AuthenticationManager.SignOut();
                return RedirectToAction("Index","Error");
            }
            else
            {
                //get both current logged in user and see user whos log/ordernum matches what customer
                var customerId = User.Identity.GetUserId();
                var Customer = db.Users.Single(u => u.Id == customerId);
                // if log id exist...
                if (oCount < 1)
                {
                    return RedirectToAction("Index", "Error");                    
                }
                else
                {
                    var ocustomerId = User.Identity.GetUserId();
                    var oCustomer = db.Users.Single(u => u.Id == ocustomerId);
                    DateTime startDateTime = DateTime.Today; //Today at 00:00:00
                    DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
                    var myNum = (from mn in db.UserLogs
                                 where oCustomer.Id == mn.Customer.Id
                                 && mn.LogDate >= startDateTime
                                 && mn.LogDate <= endDateTime
                                 select mn).ToList();
                    var todayUCount = myNum.Count();
                    var oNewNum = (from nn in myNum select nn.Id).Last();
                    if(todayUCount == 0)
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    //if the current customer matches the logid stamped with the user that logged in the database
                    var cLogNum = (from u in db.UserLogs where unum == u.Id select u.Id).Single();
                    var cUser = (from u in db.UserLogs where unum == u.Id select u.Customer).Single();
                    if ((cUser != Customer) || (cLogNum != unum) || (unum < oNewNum))
                    {
                        return RedirectToAction("Index", "Error");
                    }

                    //get all shirt count
                    var resultCount = (from rc in db.Shirts select rc).Count();
                    //get active shirt count
                    var activeCount = (from ac in db.Shirts where ac.SiteActive == false select ac).Count();
                    
                    //return no shirts if there are no shirts
                    if (resultCount == activeCount)
                    {
                        return RedirectToAction("NoShirts", new { unum = unum });
                    }

                    
                    var notAvailable = (from na in db.Shirts where na.Id == shirtId select na.SiteActive).Single();
                    if (notAvailable == false)
                    {
                        ViewBag.Available = 0;
                    }
                    else
                    {
                        ViewBag.Available = 1;
                    }


                    //retrieve user defined active shirt of the month
                    var result = (from s in db.Shirts where s.Id == shirtId select s).Include(s => s.Files).FirstOrDefault();                    
                    //retrieve active shirt quantities
                    var activeShirtQuantities = (from sq in db.ShirtQuantities where sq.Active == true select sq).ToList();
                    //retrieve active delivery options
                    var activeDeliveryOptions = (from dop in db.DeliveryOptions where dop.Active == true select dop).ToList();
                    //retrieve true or false for the active shirt's markup price
                    var boolUpC = (from buc in db.Shirts select buc.UpsizeCharge).FirstOrDefault();
                    //retrieve plus sized markup price
                    var upcharge = (from uc in db.Markups select uc.Amount).FirstOrDefault();


                    //retrieve active shirt sizes(removed due to demand for more shirts)
                    //var activeShirtSizes = (from ss in db.ShirtSizes where ss.Active == true select ss).ToList();
                    //retrieve selected shirt sizes
                    var selectedShirts = from sts in db.ShirtToSizes
                                         join ss in db.ShirtSizes on sts.ShirtSizeId equals ss.Id
                                         where sts.ShirtId == shirtId
                                         select new { Id = sts.ShirtSizeId, Name = ss.Name };
                    ViewBag.SizeCount = selectedShirts.Count();

                    ViewBag.Upcharge = boolUpC;
                    //set a visible upcharge(if applicable)for user before it hits the cart
                    if (boolUpC == true)
                    {
                        ViewBag.Upcharge = upcharge;
                    }
                    else
                    {
                        ViewBag.Upcharge = 0.00M;
                    }

                    bool getConfirm = (from gc in db.Users where Customer.Id == gc.Id select gc.EmailConfirmed).First();
                    if (getConfirm == false)
                    {
                        ViewBag.Confirm = "false";
                        ViewBag.Id = Customer.Id;
                        ViewBag.Subject = "Teeshirt Emporium Email Confirmation";
                    }
                    else
                    {
                        ViewBag.Confirm = "true";
                    }

                    ViewBag.TabTitle = null;
                    ViewBag.ShirtSizeId = new SelectList(selectedShirts, "Id", "Name");
                    ViewBag.ShirtQuantityId = new SelectList(activeShirtQuantities, "Id", "Name");
                    ViewBag.DeliveryOptionsId = new SelectList(activeDeliveryOptions, "Id", "Name");
                    if (result != null)
                    {
                        var TabTitle = result.TabName;
                        var TabContent = result.Caption;
                        var TabPrice = result.Price;
                        ViewBag.TabTitle = TabTitle;
                        ViewBag.TabContent = TabContent;
                        ViewBag.TabPrice = TabPrice;
                        var tax = 0.06M;
                        var taxtotal = TabPrice * tax;
                        var total = TabPrice + taxtotal;
                        var gtotal = System.Math.Round(total, 2);
                        ViewBag.OTabPrice = gtotal;
                    }
                    ViewBag.Success = TempData["success"];
                    ViewBag.LogNum = unum;
                    var viewModel = new ViewModels.ShirtOrdersViewModel();
                    viewModel.ShirtSizes = db.ShirtSizes.ToList();
                    viewModel.ShirtQuantities = db.ShirtQuantities.ToList();
                    viewModel.DeliveryOptions = db.DeliveryOptions.ToList();
                    viewModel.Shirts = db.Shirts.ToList();
                    viewModel.Files = db.Files.ToList();
                    viewModel.ShirtId = shirtId.Value;
                    return View(viewModel);
                }
            }
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int shirtId, ShirtCriteriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customerId = User.Identity.GetUserId();
                var oCustomer = db.Users.Single(u => u.Id == customerId);
                var shirtSize = db.ShirtSizes.Find(model.ShirtSizeId);
                var shirtQuantities = db.ShirtQuantities.Find(model.ShirtQuantityId);
                var deliveryOption = db.DeliveryOptions.Find(model.DeliveryOptionId);
                var result = (from s in db.Shirts where s.Id == shirtId select s).FirstOrDefault();
                var gbaseprice = result.Price;
                var plusTwo = 0M;
                var size = shirtSize.Name.ToString();
                var mysize = size.ToLower();
                var myBaseprice = Convert.ToDecimal(gbaseprice);
                var baseWithMarkup = 0M;
                var toChargeMarkup = result.UpsizeCharge;
                var upsizeCharge = (from uc in db.Markups select uc.Amount).First();
                if ((mysize == "small") || (mysize == "medium") || (mysize == "large"))
                {
                    plusTwo = 0M;
                    baseWithMarkup = myBaseprice + plusTwo;

                }
                else
                {
                    if (toChargeMarkup == true)
                    {
                        plusTwo = upsizeCharge;
                        baseWithMarkup = myBaseprice + plusTwo;
                    }
                    else
                    {
                        plusTwo = 0M;
                        baseWithMarkup = myBaseprice + plusTwo;
                    }
                }
                var baseprice = baseWithMarkup;
                var multi = Convert.ToInt32(shirtQuantities.Name);
                var tax = Convert.ToDecimal(0.06);
                var taxtotal = baseprice * multi * tax;
                var total = baseprice * multi + taxtotal;
                var cause = result.ShirtCause;

                var SQ = Convert.ToInt32(shirtQuantities.Name);
                var DOP = deliveryOption.Name;
                var SS = shirtSize.Name;

                //Get current customer info from users table
                var userResults = (from ur in db.Users where oCustomer.Id == ur.Id select ur).ToList();
                var EmpId = (from ei in userResults select ei.EmployeeId).First();
                var FName = (from fn in userResults select fn.FirstName).First();
                var LName = (from ln in userResults select ln.LastName).First();
                var Dept = (from dp in userResults select dp.Department).First();
                var Phone = (from ph in userResults select ph.PhoneNumber).First();
                var Emailz = (from em in userResults select em.Email).First();
                var fileId = (from fi in db.Files where fi.ShirtId == shirtId select fi.FileId).First();

                string newNum = Request.Params["unum"];
                string myId = Request.Params["shirtId"];
                int mynum = 0;
                bool res = int.TryParse(newNum, out mynum);

                var order = new Order
                {
                    ShirtCause = cause,
                    CreateDate = DateTime.Now,
                    ShirtYear = DateTime.Now.Year.ToString(),
                    OrderCompleted = false,
                    Total = total,
                    Customer = db.Users.Single(u => u.Id == customerId),
                    ShirtSize = shirtSize,
                    ShirtQuantity = shirtQuantities,
                    DeliveryOption = deliveryOption,
                    OShirtQuantity = SQ,
                    OShirtSize = SS,
                    ODeliveryOption = DOP,
                    OrderNum = mynum,
                    EmployeeID = EmpId,
                    FirstName = FName,
                    LastName = LName,
                    Department = Dept,
                    PhoneNumber = Phone,
                    Email = Emailz,
                    ShirtId = shirtId,
                    FileId = fileId
                };
                Console.Write(plusTwo);
                TempData["shirtcause"] = cause;
                db.Orders.Add(order);
                db.SaveChanges();
                TempData["success"] = "A new Item has been added to your cart";
                return RedirectToAction("Index", "Orders", new { shirtId = myId, unum = mynum });
            }

            return View(model);

        }

        [AuthLog(Roles = "Administrators")]        
        public ActionResult Export(int shirtId)
        {             
            ViewBag.ShirtId = shirtId;
            var shirtYear = DateTime.Now.Year.ToString();
            var Cause = (from cs in db.Shirts where cs.Id == shirtId select cs.ShirtCause).First();
            var MyCause = Cause+shirtYear+".xls";
            var NewCause = MyCause.Replace(" ", String.Empty);
            // Step 1 - get the data from database
            var exp = from p in db.Orders
                      where p.ShirtYear.Equals(shirtYear) &&
                      p.OrderCompleted == true &&  p.ShirtCause == Cause
                      select p;
            var data = exp.ToList();

            // instantiate the GridView control from System.Web.UI.WebControls namespace
            // set the data source
            GridView gridview = new GridView();
            gridview.DataSource = data;
            gridview.DataBind();

            // Clear all the content from the current response
            Response.ClearContent();
            Response.Buffer = true;
            // set the header
            Response.AddHeader("content-disposition", "attachment;filename="+ NewCause);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            // create HtmlTextWriter object with StringWriter
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    // render the GridView to the HtmlTextWriter
                    gridview.RenderControl(htw);
                    // Output the GridView content saved into StringWriter
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }


        // GET: Orders
        public ActionResult Cart(int? shirtId)
        {
            string newNum = Request.Params["unum"];
            int num = 0;
            bool res = int.TryParse(newNum, out num);
            var oResult = (from o in db.Orders where num == o.OrderNum select o);
            var oCount = oResult.Count();
            ViewBag.OCount = oCount;
            ViewBag.Total = 0.00M;
            if (oCount < 1)
            {
                ViewBag.Total = 0.00M;
            }
            else 
            {
                var gtotal = (from p in oResult select p.Total).Sum();
                ViewBag.Total = gtotal;
                ViewBag.Data = num;
            }
            TempData["gTotal"] = ViewBag.Total;
            ViewBag.LogNum = num;            
            return View(oResult.ToList());
        }


        [AuthLog(Roles = "Administrators")]
        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }


        [AuthLog(Roles = "Administrators")]
        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShirtCause,CreateDate,Total,ShirtYear,OrderCompleted,RecievedShirt")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            string newNum = Request.Params["unum"];
            int mynum = 0;
            bool res = int.TryParse(newNum, out mynum);
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            TempData["warning"] = "An Item has been removed from your cart";
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }


        [AuthLog(Roles = "Registered Users, Administrators")]
        public async Task<ActionResult> Checkout(int? unum)
        {
            //var Cause = TempData["shirtcause"];
            //var Total = TempData["gTotal"];
            var myOrders = (from mo in db.Orders where mo.OrderNum == unum select mo).ToList();
            var gTotal = (from gt in myOrders select gt.Total).Sum();
            StringBuilder bodyMessage = new StringBuilder();
            bodyMessage.Append("Thanks for your order at King's Daughters Tee Shirt Emporium.<br />");
            bodyMessage.Append("Your total payroll deduction for order " + unum + " is " + gTotal + " For the following items:<br /><br />");
            bodyMessage.Append("<style>tr{border-bottom:1px solid #333;}th,td{border-right:1px solid #333;border-collapse:collapse;}</style>");
            bodyMessage.Append("<table style='width:auto; th.padding:10; text-align:left;'>");
            bodyMessage.Append("<tr><th>Quantity</th><th>Size</th><th>Item</th><th>Price</th></tr>");
            foreach (var item in myOrders)
            {
                
                bodyMessage.Append("<tr><td>" + item.OShirtQuantity + "</td><td>"+ item.OShirtSize +"</td><td>"+ item.ShirtCause +" Tee</td><td>"+ item.Total +"</td></tr>");
            }
            bodyMessage.Append("</table><br /><br />We hope you enjoy your new merchandise. Have a nice day!");
            var customerId = User.Identity.GetUserId();
            var Customer = db.Users.Single(u => u.Id == customerId);
            var Email = (from em in db.Users where Customer.Id == em.Id select em.Email).First();

            if (unum != null)
            {
                var allOrders = db.Orders.Where(o => o.OrderNum == unum.Value);
                foreach (var order in allOrders)
                {
                    order.RecievedShirt = false;
                    order.OrderCompleted = true;
                }
                db.SaveChanges();
            }



            await EmailService.SendAsync(new IdentityMessage
            {
                
                Body = bodyMessage.ToString(),
                Destination = Email,
                Subject = "Teeshirt Emporium Order Confirmation"
            });
            return RedirectToAction("Purchased", "Account");        
        }

        public ActionResult NoShirts()
        {
            ViewBag.TabTitle = "Try Back Another Time";
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
