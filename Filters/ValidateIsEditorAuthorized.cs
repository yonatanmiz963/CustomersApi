using CustomersApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomrsApi.Filters
{
    public class ValidateIsEditorAuthorized : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.HttpContext.Items["User"] is UserDTO user)
            {
                if (context.ActionArguments.ContainsKey("updatedUserObj") && context.ActionArguments["updatedUserObj"] is UpdateUserDTO updatedUserObj)
                {
                    if (updatedUserObj.Id != user.Id)
                    {
                        context.Result = new UnauthorizedObjectResult(
                            "You are not authorized to edit this user's information, only the user himself can edit his information."
                        );
                    }
                }
                else
                {
                    throw new ArgumentException("Action method does not contain a parameter named 'userobj' of type User");
                }
            }
        }
    }
}