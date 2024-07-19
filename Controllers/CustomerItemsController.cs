using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomersApi.Models;
using CustomrsApi.Helpers;

namespace CustomersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerItemsController : ControllerBase
    {
        private readonly CustomerContext _context;

        public CustomerItemsController(CustomerContext context)
        {
            _context = context;
        }

        // GET: api/CustomerItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomerItems()
        {
            return await _context.CustomerItems.ToListAsync();
        }

        // GET: api/CustomerItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(long id)
        {
            var customer = await _context.CustomerItems.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/CustomerItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(long id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CustomerItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.CustomerItems.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/CustomerItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            var customer = await _context.CustomerItems.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.CustomerItems.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(long id)
        {
            return _context.CustomerItems.Any(e => e.Id == id);
        }
    }
}
