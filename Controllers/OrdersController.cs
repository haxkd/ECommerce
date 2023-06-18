using EComm.Interface;
using EComm.Models;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Cors;

namespace EComm.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : Controller
    {

        readonly DatabaseContext _context;
        readonly IHttpContextAccessor _httpcontext;

        public OrdersController(DatabaseContext dbContext, IHttpContextAccessor httpcontext)
        {
            _context = dbContext;
            _httpcontext = httpcontext ?? throw new ArgumentNullException(nameof(httpcontext));
        }
        [Authorize]
        [HttpPost("AddToCart/")]
        public async Task<IActionResult> AddToCart(AddCartModel addcart)
        {
            try
            {
                int uid = Convert.ToInt32(_httpcontext.HttpContext.User.Claims
                       .First(i => i.Type == "UserId").Value);
                var cart1 = _context.Carts.FirstOrDefault(x => x.uid == uid && x.pid == addcart.pid);
                if (cart1 != null)
                {
                    cart1.quantity = cart1.quantity + addcart.quantity;
                }
                else
                {
                    Cart cart = new Cart();
                    cart.pid = addcart.pid;
                    cart.quantity = addcart.quantity;
                    cart.uid = uid;
                    _context.Carts.Add(cart);
                }

                await _context.SaveChangesAsync();
                return Ok("Product Added to cart");
            }
            catch
            {
                return BadRequest("Failed to add cart");
            }
        }



        [Authorize]
        [HttpGet("GetCart/")]
        public async Task<ActionResult<IEnumerable<CartModel>>> GetCart()
        {
            try
            {
                int uid = Convert.ToInt32(_httpcontext.HttpContext.User.Claims
                       .First(i => i.Type == "UserId").Value);

                List<CartModel> cartModels = new List<CartModel>();
                var carts = _context.Carts.Where(x => x.uid == uid).ToList();
                var products = _context.Products.ToList();
                foreach(Cart cart in carts)
                {
                    CartModel cartModel = new CartModel();
                    cartModel.cid = cart.cid;
                    cartModel.uid = uid;
                    cartModel.pid = cart.pid;
                    cartModel.product = products.FirstOrDefault(x => x.pid == cart.pid);
                    cartModel.quantity = cart.quantity;
                    cartModels.Add(cartModel);
                }

                return Ok(cartModels);
            }
            catch
            {
                return BadRequest("Failed to fetch cart");
            }
        }

        [Authorize]
        [HttpPut("EditCart/{id}")]
        public async Task<ActionResult<IEnumerable<Cart>>> PutCart(int id, int quantity)
        {
            try
            {
                var cart = _context.Carts.FirstOrDefault(x => x.cid == id);
                if (cart == null)
                {
                    return BadRequest("Wrong cart id");
                }

                cart.quantity = quantity;

                _context.SaveChanges();
                return Ok(cart);
            }
            catch
            {
                return BadRequest("Failed to fetch cart");
            }

        }

        [Authorize]
        [HttpPost("MakeOrder")]
        public async Task<ActionResult<IEnumerable<Cart>>> MakeOrder()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string OrderId = "OID" + (new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray()));

            try
            {
                int uid = Convert.ToInt32(_httpcontext.HttpContext.User.Claims
                       .First(i => i.Type == "UserId").Value);
                var carts = _context.Carts.Where(x => x.uid == uid).ToList();
                if (carts == null)
                {
                    return BadRequest("cart is empty");
                }
                List<Order> orders = new List<Order>();
                foreach (var cart in carts)
                {
                    if (cart.quantity == 0)
                    {
                        continue;
                    }
                    Order order = new Order();
                    order.PId = cart.pid;
                    order.quantity = cart.quantity;
                    order.OrderdId = OrderId;
                    order.date = DateTime.Now.ToString();
                    order.uid = uid;
                    orders.Add(order);
                    _context.Carts.Remove(cart);
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                }

                _context.SaveChanges();
                return Ok(orders);
            }
            catch
            {
                return BadRequest("Failed to fetch cart");
            }
        }
        [Authorize]
        [HttpGet("Orders")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> Orders()
        {
            try
            {
                int uid = Convert.ToInt32(_httpcontext.HttpContext.User.Claims
                       .First(i => i.Type == "UserId").Value);
                var orders = _context.Orders.Where(x => x.uid == uid).ToList();
                var products = _context.Products.ToList();
                List<OrderModel> ordersModel = new List<OrderModel>();
                foreach (Order order in orders)
                {
                    OrderModel orderModel = new OrderModel();
                    orderModel.OrderdId = order.OrderdId;
                    orderModel.OrderId = order.OrderId;
                    orderModel.uid = order.uid;
                    orderModel.PId = order.PId;
                    orderModel.quantity = order.quantity;
                    orderModel.date = order.date;
                    orderModel.product = products.FirstOrDefault(x => x.pid == order.PId);
                    ordersModel.Add(orderModel);
                }
                return Ok(ordersModel);
            }
            catch
            {
                return BadRequest("Failed to fetch cart");
            }



        }
    }
}